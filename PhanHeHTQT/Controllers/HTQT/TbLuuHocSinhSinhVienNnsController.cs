using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using PhanHeHTQT.API;
using PhanHeHTQT.Models;
using PhanHeHTQT.Models.DM;

namespace PhanHeHTQT.Controllers.HTQT
{
    public class TbLuuHocSinhSinhVienNnsController : Controller
    {
        private readonly ApiServices ApiServices_;

        public TbLuuHocSinhSinhVienNnsController(ApiServices services)
        {
            ApiServices_ = services;
        }

        private async Task<List<TbLuuHocSinhSinhVienNn>> TbLuuHocSinhSinhVienNns()
        {
            List<TbLuuHocSinhSinhVienNn> tbLuuHocSinhSinhVienNns = await ApiServices_.GetAll<TbLuuHocSinhSinhVienNn>("/api/htqt/LuuHocSinhSinhVienNN");
            List<DmLoaiLuuHocSinh> dmLoaiLuuHocSinhs = await ApiServices_.GetAll<DmLoaiLuuHocSinh>("/api/dm/LoaiLuuHocSinh");
            List<DmNguonKinhPhiChoLuuHocSinh> dmNguonKinhPhiChoLuuHocSinhs = await ApiServices_.GetAll<DmNguonKinhPhiChoLuuHocSinh>("/api/dm/NguonKinhPhiChoLuuHocSinh");
            tbLuuHocSinhSinhVienNns.ForEach(item => {
                item.IdLoaiLuuHocSinhNavigation = dmLoaiLuuHocSinhs.FirstOrDefault(x => x.IdLoaiLuuHocSinh == item.IdLoaiLuuHocSinh);
                item.IdNguonKinhPhiLuuHocSinhNavigation = dmNguonKinhPhiChoLuuHocSinhs.FirstOrDefault(x => x.IdNguonKinhPhiChoLuuHocSinh == item.IdNguonKinhPhiLuuHocSinh);
            });
            return tbLuuHocSinhSinhVienNns;
        }

        // GET: TbLuuHocSinhSinhVienNns
        public async Task<IActionResult> Index()
        {
            List<TbLuuHocSinhSinhVienNn> getall = await TbLuuHocSinhSinhVienNns();
            return View(getall);
        }
        public async Task<IActionResult> Statistics()
        {
            List<TbLuuHocSinhSinhVienNn> getall = await TbLuuHocSinhSinhVienNns();
            return View(getall);
        }

        // GET: TbLuuHocSinhSinhVienNns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id < 0) //Kiểm tra ID null hoặc số âm
            {
                ModelState.AddModelError("Id", "ID không được là số âm hoặc null.");
                return NotFound();
            }

            var tbLuuHocSinhSinhVienNns = await TbLuuHocSinhSinhVienNns();
            var tbLuuHocSinhSinhVienNn = tbLuuHocSinhSinhVienNns.FirstOrDefault(m => m.IdLuuHocSinhSinhVienNn == id);
            if (tbLuuHocSinhSinhVienNn == null)
            {
                return NotFound();
            }

            return View(tbLuuHocSinhSinhVienNn);
        }

        // GET: TbLuuHocSinhSinhVienNns/Create
        public async Task<IActionResult> Create()
        {
            ViewData["IdLoaiLuuHocSinh"] = new SelectList(await ApiServices_.GetAll<DmLoaiLuuHocSinh>("/api/dm/LoaiLuuHocSinh"), "IdLoaiLuuHocSinh", "LoaiLuuHocSinh");
            ViewData["IdNguonKinhPhiLuuHocSinh"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhiChoLuuHocSinh>("/api/dm/NguonKinhPhiChoLuuHocSinh"), "IdNguonKinhPhiChoLuuHocSinh", "NguonKinhPhiChoLuuHocSinh");
            return View();
        }

        // POST: TbLuuHocSinhSinhVienNns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLuuHocSinhSinhVienNn,IdNguoiHoc,IdNguonKinhPhiLuuHocSinh,IdLoaiLuuHocSinh")] TbLuuHocSinhSinhVienNn tbLuuHocSinhSinhVienNn)
        {
            try
            {
                if (tbLuuHocSinhSinhVienNn.IdNguoiHoc < 0)
                {
                    ModelState.AddModelError("IdNguoiHoc", "Id người học không được là số âm.");
                }
                if (tbLuuHocSinhSinhVienNn.IdLuuHocSinhSinhVienNn < 0)
                {
                    ModelState.AddModelError("IdLuuHocSinhSinhVienNn", "Id luu học sinh sinh viên không được là số âm.");
                }
                if (await TbLuuHocSinhSinhVienNnExists(tbLuuHocSinhSinhVienNn.IdLuuHocSinhSinhVienNn))
                {
                    ModelState.AddModelError("IdLuuHocSinhSinhVienNn", "Id này đã tồn tại!");
                }

                if (await TbLuuHocSinhSinhVienNnExists(tbLuuHocSinhSinhVienNn.IdLuuHocSinhSinhVienNn)) ModelState.AddModelError("IdLuuHocSinhSinhVienNn", "Id này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbLuuHocSinhSinhVienNn>("/api/htqt/LuuHocSinhSinhVienNN", tbLuuHocSinhSinhVienNn);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdLoaiLuuHocSinh"] = new SelectList(await ApiServices_.GetAll<DmLoaiLuuHocSinh>("/api/dm/LoaiLuuHocSinh"), "IdLoaiLuuHocSinh", "LoaiLuuHocSinh", tbLuuHocSinhSinhVienNn.IdLoaiLuuHocSinh);
                ViewData["IdNguonKinhPhiLuuHocSinh"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhiChoLuuHocSinh>("/api/dm/NguonKinhPhiChoLuuHocSinh"), "IdNguonKinhPhiChoLuuHocSinh", "NguonKinhPhiChoLuuHocSinh", tbLuuHocSinhSinhVienNn.IdNguonKinhPhiLuuHocSinh);
                return View(tbLuuHocSinhSinhVienNn);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: TbLuuHocSinhSinhVienNns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id < 0) //Kiểm tra ID null hoặc số âm
            {
                ModelState.AddModelError("Id", "ID không được là số âm hoặc null.");
                return NotFound();
            }

            var tbLuuHocSinhSinhVienNn = await ApiServices_.GetId<TbLuuHocSinhSinhVienNn>("/api/htqt/LuuHocSinhSinhVienNN", id ?? 0);
            if (tbLuuHocSinhSinhVienNn == null)
            {
                return NotFound();
            }
            ViewData["IdLoaiLuuHocSinh"] = new SelectList(await ApiServices_.GetAll<DmLoaiLuuHocSinh>("/api/dm/LoaiLuuHocSinh"), "IdLoaiLuuHocSinh", "LoaiLuuHocSinh", tbLuuHocSinhSinhVienNn.IdLoaiLuuHocSinh);
            ViewData["IdNguonKinhPhiLuuHocSinh"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhiChoLuuHocSinh>("/api/dm/NguonKinhPhiChoLuuHocSinh"), "IdNguonKinhPhiChoLuuHocSinh", "NguonKinhPhiChoLuuHocSinh", tbLuuHocSinhSinhVienNn.IdNguonKinhPhiLuuHocSinh);
            return View(tbLuuHocSinhSinhVienNn);
        }

        // POST: TbLuuHocSinhSinhVienNns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdLuuHocSinhSinhVienNn,IdNguoiHoc,IdNguonKinhPhiLuuHocSinh,IdLoaiLuuHocSinh")] TbLuuHocSinhSinhVienNn tbLuuHocSinhSinhVienNn)
        {
            try
            {
                if (id != tbLuuHocSinhSinhVienNn.IdLuuHocSinhSinhVienNn)
                {
                    return NotFound();
                }

                if (tbLuuHocSinhSinhVienNn.IdNguoiHoc < 0)
                {
                    ModelState.AddModelError("IdNguoiHoc", "Id người học không được là số âm.");
                }
                if (tbLuuHocSinhSinhVienNn.IdLuuHocSinhSinhVienNn < 0)
                {
                    ModelState.AddModelError("IdLuuHocSinhSinhVienNn", "Id luu học sinh sinh viên không được là số âm.");
                }
                if (await TbLuuHocSinhSinhVienNnExists(tbLuuHocSinhSinhVienNn.IdLuuHocSinhSinhVienNn))
                {
                    ModelState.AddModelError("IdLuuHocSinhSinhVienNn", "Id này đã tồn tại!");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbLuuHocSinhSinhVienNn>("/api/htqt/LuuHocSinhSinhVienNN", id, tbLuuHocSinhSinhVienNn);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbLuuHocSinhSinhVienNnExists(tbLuuHocSinhSinhVienNn.IdLuuHocSinhSinhVienNn) == false)
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
                ViewData["IdLoaiLuuHocSinh"] = new SelectList(await ApiServices_.GetAll<DmLoaiLuuHocSinh>("/api/dm/LoaiLuuHocSinh"), "IdLoaiLuuHocSinh", "LoaiLuuHocSinh", tbLuuHocSinhSinhVienNn.IdLoaiLuuHocSinh);
                ViewData["IdNguonKinhPhiLuuHocSinh"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhiChoLuuHocSinh>("/api/dm/NguonKinhPhiChoLuuHocSinh"), "IdNguonKinhPhiChoLuuHocSinh", "NguonKinhPhiChoLuuHocSinh", tbLuuHocSinhSinhVienNn.IdNguonKinhPhiLuuHocSinh);
                return View(tbLuuHocSinhSinhVienNn);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: TbLuuHocSinhSinhVienNns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbLuuHocSinhSinhVienNns = await TbLuuHocSinhSinhVienNns();
            var tbLuuHocSinhSinhVienNn = tbLuuHocSinhSinhVienNns.FirstOrDefault(m => m.IdLuuHocSinhSinhVienNn == id);
            if (tbLuuHocSinhSinhVienNn == null)
            {
                return NotFound();
            }

            return View(tbLuuHocSinhSinhVienNn);
        }

        // POST: TbLuuHocSinhSinhVienNns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await ApiServices_.Delete<TbLuuHocSinhSinhVienNn>("/api/htqt/LuuHocSinhSinhVienNN", id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TbLuuHocSinhSinhVienNnExists(int id)
        {
            var tbLuuHocSinhSinhVienNns = await ApiServices_.GetAll<TbLuuHocSinhSinhVienNn>("/api/htqt/HoiThaoHoiNghi");
            return tbLuuHocSinhSinhVienNns.Any(e => e.IdLuuHocSinhSinhVienNn == id);
        }

        public async Task<IActionResult> Receive(string json)
        {
            try
            {
                // Khai báo thông báo mặc định
                var message = "Không phát hiện lỗi";
                // Giải mã dữ liệu JSON từ client
                List<List<string>> data = JsonConvert.DeserializeObject<List<List<string>>>(json);

                // Danh sách lưu các đối tượng TbLuuHocSinhSinhVienNn
                List<TbLuuHocSinhSinhVienNn> lst = new List<TbLuuHocSinhSinhVienNn>();

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();
                List<TbLuuHocSinhSinhVienNn> tbLuuHocSinhSinhVienNns = await ApiServices_.GetAll<TbLuuHocSinhSinhVienNn>("/api/htqt/LuuHocSinhSinhVienNN");
                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    TbLuuHocSinhSinhVienNn model = new TbLuuHocSinhSinhVienNn();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (lst.Any(t => t.IdLuuHocSinhSinhVienNn == id) || tbLuuHocSinhSinhVienNns.Any(t => t.IdLuuHocSinhSinhVienNn == id)); // Kiểm tra id có tồn tại không


                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdLuuHocSinhSinhVienNn = id; // Gán ID
                    model.IdNguoiHoc = ParseInt(item[0]);
                    model.IdLoaiLuuHocSinh = ParseInt(item[1]);
                    model.IdNguonKinhPhiLuuHocSinh = ParseInt(item[2]);
                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu (giả sử có một phương thức tạo đối tượng trong DB)
                foreach (var item in lst)
                {
                    await CreateTbLuuHocSinhSinhVienNn(item); // Giả sử có phương thức tạo dữ liệu vào DB
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về thông báo lỗi
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbLuuHocSinhSinhVienNn(TbLuuHocSinhSinhVienNn item)
        {
            await ApiServices_.Create<TbLuuHocSinhSinhVienNn>("/api/htqt/LuuHocSinhSinhVienNN", item);
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
