using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using PhanHeHTQT.API;
using PhanHeHTQT.Models;
using PhanHeHTQT.Models.DM;
using System.Globalization;

namespace PhanHeHTQT.Controllers.HTQT
{
    public class TbHoiThaoHoiNghisController : Controller
    {
        private readonly ApiServices ApiServices_;

        public TbHoiThaoHoiNghisController(ApiServices services)
        {
            ApiServices_ = services;
        }

        private async Task<List<TbHoiThaoHoiNghi>> TbHoiThaoHoiNghis()
        {
            List<TbHoiThaoHoiNghi> tbHoiThaoHoiNghis = await ApiServices_.GetAll<TbHoiThaoHoiNghi>("/api/htqt/HoiThaoHoiNghi");
            List<DmNguonKinhPhi> dmNguonKinhPhis = await ApiServices_.GetAll<DmNguonKinhPhi>("/api/dm/NguonKinhPhi");
            tbHoiThaoHoiNghis.ForEach(item => { 
            item.IdNguonKinhPhiHoiThaoNavigation = dmNguonKinhPhis.FirstOrDefault(x => x.IdNguonKinhPhi == item.IdNguonKinhPhiHoiThao);
            });
            return tbHoiThaoHoiNghis;
        }
        // GET: TbHoiThaoHoiNghis
        public async Task<IActionResult> Index()
        {
            List<TbHoiThaoHoiNghi> getall = await TbHoiThaoHoiNghis();
            return View(getall);
        }
        public async Task<IActionResult> Statistics()
        {
            List<TbHoiThaoHoiNghi> getall = await TbHoiThaoHoiNghis();
            return View(getall);
        }

        // GET: TbHoiThaoHoiNghis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id < 0) //Kiểm tra ID null hoặc số âm
            {
                ModelState.AddModelError("Id", "ID không được là số âm hoặc null.");
                return NotFound();
            }

            var tbHoiThaoHoiNghis = await TbHoiThaoHoiNghis();
            var tbHoiThaoHoiNghi = tbHoiThaoHoiNghis.FirstOrDefault(m => m.IdHoiThaoHoiNghi == id);

           if (tbHoiThaoHoiNghi == null)
            {
                return NotFound();
            }

            return View(tbHoiThaoHoiNghi);
        }

        // GET: TbHoiThaoHoiNghis/Create
        public async Task<IActionResult> Create()
        {
            ViewData["IdNguonKinhPhiHoiThao"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhi>("/api/dm/NguonKinhPhi"), "IdNguonKinhPhi", "NguonKinhPhi");
            return View();
        }

        // POST: TbHoiThaoHoiNghis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHoiThaoHoiNghi,MaHoiThaoHoiNghi,TenHoiThaoHoiNghi,CoQuanCoThamQuyenCapPhep,MucTieu,NoiDung,SoLuongDaiBieuThamDu,SoLuongDaiBieuQuocTeThamDu,ThoiGianToChuc,DiaDiemToChuc,IdNguonKinhPhiHoiThao,DonViChuTri")] TbHoiThaoHoiNghi tbHoiThaoHoiNghi)
        {

            if (await TbHoiThaoHoiNghiExists(tbHoiThaoHoiNghi.IdHoiThaoHoiNghi)) ModelState.AddModelError("IdHoiThaoHoiNghi", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbHoiThaoHoiNghi>("/api/htqt/HoiThaoHoiNghi", tbHoiThaoHoiNghi);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdNguonKinhPhiHoiThao"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhi>("/api/htqt/NguonKinhPhi"), "IdNguonKinhPhi", "NguonKinhPhi", tbHoiThaoHoiNghi.IdNguonKinhPhiHoiThao);
            return View(tbHoiThaoHoiNghi);
        }

        // GET: TbHoiThaoHoiNghis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id < 0) //Kiểm tra ID null hoặc số âm
            {
                ModelState.AddModelError("Id", "ID không được là số âm hoặc null.");
                return NotFound();
            }

            var tbHoiThaoHoiNghi = await ApiServices_.GetId<TbHoiThaoHoiNghi>("/api/htqt/HoiThaoHoiNghi", id ?? 0);
            if (tbHoiThaoHoiNghi == null)
            {
                return NotFound();
            }
            ViewData["IdNguonKinhPhiHoiThao"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhi>("/api/dm/NguonKinhPhi"), "IdNguonKinhPhi", "NguonKinhPhi", tbHoiThaoHoiNghi.IdNguonKinhPhiHoiThao);
            return View(tbHoiThaoHoiNghi);
        }

        // POST: TbHoiThaoHoiNghis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdHoiThaoHoiNghi,MaHoiThaoHoiNghi,TenHoiThaoHoiNghi,CoQuanCoThamQuyenCapPhep,MucTieu,NoiDung,SoLuongDaiBieuThamDu,SoLuongDaiBieuQuocTeThamDu,ThoiGianToChuc,DiaDiemToChuc,IdNguonKinhPhiHoiThao,DonViChuTri")] TbHoiThaoHoiNghi tbHoiThaoHoiNghi)
        {
            if (id != tbHoiThaoHoiNghi.IdHoiThaoHoiNghi)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbHoiThaoHoiNghi>("/api/htqt/HoiThaoHoiNghi", id, tbHoiThaoHoiNghi);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbHoiThaoHoiNghiExists(tbHoiThaoHoiNghi.IdHoiThaoHoiNghi) == false)
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
            ViewData["IdNguonKinhPhiHoiThao"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhi>("/api/dm/NguonKinhPhi"), "IdNguonKinhPhi", "NguonKinhPhi", tbHoiThaoHoiNghi.IdNguonKinhPhiHoiThao);
            return View(tbHoiThaoHoiNghi);
        }

        // GET: TbHoiThaoHoiNghis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbHoiThaoHoiNghis = await TbHoiThaoHoiNghis();;
            var tbHoiThaoHoiNghi = tbHoiThaoHoiNghis.FirstOrDefault(m => m.IdHoiThaoHoiNghi == id);
            if (tbHoiThaoHoiNghi == null)
            {
                return NotFound();
            }

            return View(tbHoiThaoHoiNghi);
        }

        // POST: TbHoiThaoHoiNghis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await ApiServices_.Delete<TbHoiThaoHoiNghi>("/api/htqt/HoiThaoHoiNghi", id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TbHoiThaoHoiNghiExists(int id)
        {
            var tbHoiThaoHoiNghis = await ApiServices_.GetAll<TbHoiThaoHoiNghi>("/api/htqt/HoiThaoHoiNghi");
            return tbHoiThaoHoiNghis.Any(e => e.IdHoiThaoHoiNghi == id);
        }

        public async Task<IActionResult> Receive(string json)
        {
            try
            {
                // Khai báo thông báo mặc định
                var message = "Không phát hiện lỗi";
                // Giải mã dữ liệu JSON từ client
                List<List<string>> data = JsonConvert.DeserializeObject<List<List<string>>>(json);

                List<TbHoiThaoHoiNghi> lst = new List<TbHoiThaoHoiNghi>();

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();
                List<TbHoiThaoHoiNghi> tbHoiThaoHoiNghis = await ApiServices_.GetAll<TbHoiThaoHoiNghi>("/api/htqt/HoiThaoHoiNghi");
                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    TbHoiThaoHoiNghi model = new TbHoiThaoHoiNghi();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (lst.Any(t => t.IdHoiThaoHoiNghi == id) || tbHoiThaoHoiNghis.Any(t => t.IdHoiThaoHoiNghi == id)); // Kiểm tra id có tồn tại không

                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdHoiThaoHoiNghi = id; // Gán ID
                    model.MaHoiThaoHoiNghi = item[0];
                    model.TenHoiThaoHoiNghi = item[1];
                    model.CoQuanCoThamQuyenCapPhep = item[2];
                    model.MucTieu = item[3];
                    model.NoiDung = item[4];
                    model.SoLuongDaiBieuThamDu = int.Parse(item[5]);
                    model.SoLuongDaiBieuQuocTeThamDu = int.Parse(item[6]);
                    model.ThoiGianToChuc = DateOnly.ParseExact(item[7], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.DiaDiemToChuc = item[8];
                    model.DonViChuTri = item[9];
                    model.IdNguonKinhPhiHoiThao = ParseInt(item[10]);
                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu (giả sử có một phương thức tạo đối tượng trong DB)
                foreach (var item in lst)
                {
                    await CreateTbHoiThaoHoiNghi(item); // Giả sử có phương thức tạo dữ liệu vào DB
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về thông báo lỗi
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbHoiThaoHoiNghi(TbHoiThaoHoiNghi item)
        {
            await ApiServices_.Create<TbHoiThaoHoiNghi>("/api/htqt/HoiThaoHoiNghi", item);
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
