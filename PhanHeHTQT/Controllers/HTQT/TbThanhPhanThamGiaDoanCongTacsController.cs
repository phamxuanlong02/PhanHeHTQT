using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PhanHeHTQT.API;
using PhanHeHTQT.Models;
using PhanHeHTQT.Models.DM;

namespace PhanHeHTQT.Controllers.HTQT
{
    public class TbThanhPhanThamGiaDoanCongTacsController : Controller
    {
        private readonly ApiServices ApiServices_;

        public TbThanhPhanThamGiaDoanCongTacsController(ApiServices services)
        {
            ApiServices_ = services;
        }

        private async Task<List<TbThanhPhanThamGiaDoanCongTac>> TbThanhPhanThamGiaDoanCongTacs()
        {
            List<TbThanhPhanThamGiaDoanCongTac> tbThanhPhanThamGiaDoanCongTacs = await ApiServices_.GetAll<TbThanhPhanThamGiaDoanCongTac>("/api/htqt/ThanhPhanThamGiaDoanCongTac");
            List<TbCanBo> tbCanBos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<TbDoanCongTac> tbDoanCongTacs = await ApiServices_.GetAll<TbDoanCongTac>("/api/htqt/DoanCongTac");
            List<DmVaiTroThamGium> dmVaiTroThamGias = await ApiServices_.GetAll<DmVaiTroThamGium>("/api/dm/VaiTroThamGia");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            tbThanhPhanThamGiaDoanCongTacs.ForEach(item =>
            {
                item.IdCanBoNavigation = tbCanBos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdCanBoNavigation.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdCanBoNavigation.IdNguoi);
                item.IdDoanCongTacNavigation = tbDoanCongTacs.FirstOrDefault(x => x.IdDoanCongTac == item.IdDoanCongTac);
                item.IdVaiTroThamGiaNavigation = dmVaiTroThamGias.FirstOrDefault(x => x.IdVaiTroThamGia == item.IdVaiTroThamGia);
            });
            return tbThanhPhanThamGiaDoanCongTacs;
        }

        private async Task<List<TbCanBo>> TbCanBos()
        {
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            tbcanbos.ForEach(item =>
            {
                item.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdNguoi);

            });

            return tbcanbos;
        }

        // GET: TbThanhPhanThamGiaDoanCongTacs
        public async Task<IActionResult> Index()
        {
            List<TbThanhPhanThamGiaDoanCongTac> getall = await TbThanhPhanThamGiaDoanCongTacs();
            return View(getall);
        }
        public async Task<IActionResult> Statistics()
        {
            List<TbThanhPhanThamGiaDoanCongTac> getall = await TbThanhPhanThamGiaDoanCongTacs();
            return View(getall);
        }
        // GET: TbThanhPhanThamGiaDoanCongTacs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id < 0) //Kiểm tra ID null hoặc số âm
            {
                ModelState.AddModelError("Id", "ID không được là số âm hoặc null.");
                return NotFound();
            }

            var tbThanhPhanThamGiaDoanCongTacs = await TbThanhPhanThamGiaDoanCongTacs();
            var tbThanhPhanThamGiaDoanCongTac = tbThanhPhanThamGiaDoanCongTacs.FirstOrDefault(m => m.IdThanhPhanThamGiaDoanCongTac == id);
            if (tbThanhPhanThamGiaDoanCongTac == null)
            {
                return NotFound();
            }

            return View(tbThanhPhanThamGiaDoanCongTac);
        }

        // GET: TbThanhPhanThamGiaDoanCongTacs/Create
        public async Task<IActionResult> Create()
        {
            ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdCanBo");
            ViewData["IdDoanCongTac"] = new SelectList(await ApiServices_.GetAll<TbDoanCongTac>("/api/htqt/DoanCongTac"), "IdDoanCongTac", "TenDoanCongTac");
            ViewData["IdVaiTroThamGia"] = new SelectList(await ApiServices_.GetAll<DmVaiTroThamGium>("/api/dm/VaiTroThamGia"), "IdVaiTroThamGia", "VaiTroThamGia");
            return View();
        }

        // POST: TbThanhPhanThamGiaDoanCongTacs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdThanhPhanThamGiaDoanCongTac,IdDoanCongTac,IdCanBo,IdVaiTroThamGia")] TbThanhPhanThamGiaDoanCongTac tbThanhPhanThamGiaDoanCongTac)
        {
            try
            {
                if (tbThanhPhanThamGiaDoanCongTac.IdThanhPhanThamGiaDoanCongTac < 0)
                {
                    ModelState.AddModelError("IdThanhPhanThamGiaDoanCongTac", "ID không được là số âm.");
                }
                if (tbThanhPhanThamGiaDoanCongTac.IdCanBo < 0)
                {
                    ModelState.AddModelError("IdCanBo", "ID không được là số âm.");
                }
                if (tbThanhPhanThamGiaDoanCongTac.IdVaiTroThamGia < 0)
                {
                    ModelState.AddModelError("IdVaiTroThamGia", "ID không được là số âm.");
                }

                if (await TbThanhPhanThamGiaDoanCongTacExists(tbThanhPhanThamGiaDoanCongTac.IdThanhPhanThamGiaDoanCongTac)) ModelState.AddModelError("IdThanhPhanThamGiaDoanCongTac", "Id này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbThanhPhanThamGiaDoanCongTac>("/api/htqt/ThanhPhanThamGiaDoanCongTac", tbThanhPhanThamGiaDoanCongTac);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbThanhPhanThamGiaDoanCongTac.IdCanBo);
                ViewData["IdDoanCongTac"] = new SelectList(await ApiServices_.GetAll<TbDoanCongTac>("/api/htqt/DoanCongTac"), "IdDoanCongTac", "TenDoanCongTac", tbThanhPhanThamGiaDoanCongTac.IdDoanCongTac);
                ViewData["IdVaiTroThamGia"] = new SelectList(await ApiServices_.GetAll<DmVaiTroThamGium>("/api/dm/VaiTroThamGia"), "IdVaiTroThamGia", "VaiTroThamGia", tbThanhPhanThamGiaDoanCongTac.IdVaiTroThamGia);

                return View(tbThanhPhanThamGiaDoanCongTac);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: TbThanhPhanThamGiaDoanCongTacs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbThanhPhanThamGiaDoanCongTac = await ApiServices_.GetId<TbThanhPhanThamGiaDoanCongTac>("/api/htqt/ThanhPhanThamGiaDoanCongtac", id ?? 0);
            if (tbThanhPhanThamGiaDoanCongTac == null)
            {
                return NotFound();
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbThanhPhanThamGiaDoanCongTac.IdCanBo);
            ViewData["IdDoanCongTac"] = new SelectList(await ApiServices_.GetAll<TbDoanCongTac>("/api/htqt/DoanCongTac"), "IdDoanCongTac", "TenDoanCongTac", tbThanhPhanThamGiaDoanCongTac.IdDoanCongTac);
            ViewData["IdVaiTroThamGia"] = new SelectList(await ApiServices_.GetAll<DmVaiTroThamGium>("/api/dm/VaiTroThamGia"), "IdVaiTroThamGia", "VaiTroThamGia", tbThanhPhanThamGiaDoanCongTac.IdVaiTroThamGia);
            return View(tbThanhPhanThamGiaDoanCongTac);
        }

        // POST: TbThanhPhanThamGiaDoanCongTacs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdThanhPhanThamGiaDoanCongTac,IdDoanCongTac,IdCanBo,IdVaiTroThamGia")] TbThanhPhanThamGiaDoanCongTac tbThanhPhanThamGiaDoanCongTac)
        {
            try
            {
                if (id != tbThanhPhanThamGiaDoanCongTac.IdThanhPhanThamGiaDoanCongTac)
                {
                    return NotFound();
                }

                if (tbThanhPhanThamGiaDoanCongTac.IdThanhPhanThamGiaDoanCongTac < 0)
                {
                    ModelState.AddModelError("IdThanhPhanThamGiaDoanCongTac", "ID không được là số âm.");
                }
                if (tbThanhPhanThamGiaDoanCongTac.IdCanBo < 0)
                {
                    ModelState.AddModelError("IdCanBo", "ID không được là số âm.");
                }
                if (tbThanhPhanThamGiaDoanCongTac.IdVaiTroThamGia < 0)
                {
                    ModelState.AddModelError("IdVaiTroThamGia", "ID không được là số âm.");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbThanhPhanThamGiaDoanCongTac>("/api/htqt/ThanhPhanThamGiaDoanCongTac", id, tbThanhPhanThamGiaDoanCongTac);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbThanhPhanThamGiaDoanCongTacExists(tbThanhPhanThamGiaDoanCongTac.IdThanhPhanThamGiaDoanCongTac) == false)
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbThanhPhanThamGiaDoanCongTac.IdCanBo);
                ViewData["IdDoanCongTac"] = new SelectList(await ApiServices_.GetAll<TbDoanCongTac>("/api/htqt/DoanCongTac"), "IdDoanCongTac", "TenDoanCongTac", tbThanhPhanThamGiaDoanCongTac.IdDoanCongTac);
                ViewData["IdVaiTroThamGia"] = new SelectList(await ApiServices_.GetAll<DmVaiTroThamGium>("/api/dm/VaiTroThamGia"), "IdVaiTroThamGia", "VaiTroThamGia", tbThanhPhanThamGiaDoanCongTac.IdVaiTroThamGia);
                return View(tbThanhPhanThamGiaDoanCongTac);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: TbThanhPhanThamGiaDoanCongTacs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 0) //Kiểm tra ID null hoặc số âm
            {
                ModelState.AddModelError("Id", "ID không được là số âm hoặc null.");
                return NotFound();
            }

            var tbThanhPhanThamGiaDoanCongTacs = await TbThanhPhanThamGiaDoanCongTacs();
            var tbThanhPhanThamGiaDoanCongTac = tbThanhPhanThamGiaDoanCongTacs.FirstOrDefault(m => m.IdThanhPhanThamGiaDoanCongTac == id);
            if (tbThanhPhanThamGiaDoanCongTac == null)
            {
                return NotFound();
            }

            return View(tbThanhPhanThamGiaDoanCongTac);
        }

        // POST: TbThanhPhanThamGiaDoanCongTacs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await ApiServices_.Delete<TbThanhPhanThamGiaDoanCongTac>("/api/htqt/ThanhPhanThamGiaDoanCongTac", id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TbThanhPhanThamGiaDoanCongTacExists(int id)
        {
            var tbThanhPhanThamGiaDoanCongTacs = await ApiServices_.GetAll<TbThanhPhanThamGiaDoanCongTac>("/api/htqt/ThanhPhanThamGiaDoanCongTac");
            return tbThanhPhanThamGiaDoanCongTacs.Any(e => e.IdThanhPhanThamGiaDoanCongTac == id);
        }

        public async Task<IActionResult> Receive(string json)
        {
            try
            {
                // Khai báo thông báo mặc định
                var message = "Không phát hiện lỗi";
                // Giải mã dữ liệu JSON từ client
                List<List<string>> data = JsonConvert.DeserializeObject<List<List<string>>>(json);

                // Danh sách lưu các đối tượng TbThanhPhanThamGiaDoanCongTac
                List<TbThanhPhanThamGiaDoanCongTac> lst = new List<TbThanhPhanThamGiaDoanCongTac>();

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();
                List<TbThanhPhanThamGiaDoanCongTac> tbThanhPhanThamGiaDoanCongTacs = await ApiServices_.GetAll<TbThanhPhanThamGiaDoanCongTac>("/api/htqt/ThanhPhanThamGiaDoanCongTac");
                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    TbThanhPhanThamGiaDoanCongTac model = new TbThanhPhanThamGiaDoanCongTac();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (lst.Any(t => t.IdThanhPhanThamGiaDoanCongTac == id) || tbThanhPhanThamGiaDoanCongTacs.Any(t => t.IdThanhPhanThamGiaDoanCongTac == id)); // Kiểm tra id có tồn tại không


                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdThanhPhanThamGiaDoanCongTac = id; // Gán ID
                    model.IdCanBo = ParseInt(item[0]);
                    model.IdDoanCongTac = ParseInt(item[1]);
                    model.IdVaiTroThamGia = ParseInt(item[2]);
                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu (giả sử có một phương thức tạo đối tượng trong DB)
                foreach (var item in lst)
                {
                    await CreateTbThanhPhanThamGiaDoanCongTac(item); // Giả sử có phương thức tạo dữ liệu vào DB
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về thông báo lỗi
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbThanhPhanThamGiaDoanCongTac(TbThanhPhanThamGiaDoanCongTac item)
        {
            await ApiServices_.Create<TbThanhPhanThamGiaDoanCongTac>("/api/htqt/ThanhPhanThamGiaDoanCongTac", item);
        }

        private int? ParseInt(string v)
        {
            if (int.TryParse(v, out int result)) // Nếu chuỗi có thể chuyển thành int
            {
                return result; // Trả về giá trị int
            }
            else
            {
                return null; // Nếu không thể chuyển thành int, trả về null
            }
        }

    }
}
