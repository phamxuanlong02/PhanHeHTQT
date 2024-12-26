using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class TbThoaThuanHopTacQuocTesController : Controller
    {
        private readonly ApiServices ApiServices_;

        public TbThoaThuanHopTacQuocTesController(ApiServices services)
        {
            ApiServices_ = services;
        }

        private async Task<List<TbThoaThuanHopTacQuocTe>> TbThoaThuanHopTacQuocTes()
        {
            List<TbThoaThuanHopTacQuocTe> tbThoaThuanHopTacQuocTes = await ApiServices_.GetAll<TbThoaThuanHopTacQuocTe>("/api/htqt/ThoaThuanHopTacQuocTe");
            List<DmQuocTich> dmQuocTiches = await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich");
            tbThoaThuanHopTacQuocTes.ForEach(item =>
            {
                item.IdQuocGiaNavigation = dmQuocTiches.FirstOrDefault(x => x.IdQuocTich == item.IdQuocGia);
            });
            return tbThoaThuanHopTacQuocTes;
        }

        // GET: TbThoaThuanHopTacQuocTes
        public async Task<IActionResult> Index()
        {
            List<TbThoaThuanHopTacQuocTe> getall = await TbThoaThuanHopTacQuocTes();
            return View(getall);
        }
        public async Task<IActionResult> Statistics()
        {
            List<TbThoaThuanHopTacQuocTe> getall = await TbThoaThuanHopTacQuocTes();
            return View(getall);
        }

        // GET: TbThoaThuanHopTacQuocTes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id < 0) //Kiểm tra ID null hoặc số âm
            {
                ModelState.AddModelError("Id", "ID không được là số âm hoặc null.");
                return NotFound();
            }

            var tbThoaThuanHopTacQuocTes = await TbThoaThuanHopTacQuocTes();
            var tbThoaThuanHopTacQuocTe = tbThoaThuanHopTacQuocTes.FirstOrDefault(m => m.IdThoaThuanHopTacQuocTe == id);
            if (tbThoaThuanHopTacQuocTe == null)
            {
                return NotFound();
            }

            return View(tbThoaThuanHopTacQuocTe);
        }

        // GET: TbThoaThuanHopTacQuocTes/Create
        public async Task<IActionResult> Create()
        {
            ViewData["IdQuocGia"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc");
            return View();
        }

        // POST: TbThoaThuanHopTacQuocTes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdThoaThuanHopTacQuocTe,MaThoaThuan,TenThoaThuan,NoiDungTomTat,TenToChuc,NgayKyKet,SoVanBanKyKet,IdQuocGia,NgayHetHan")] TbThoaThuanHopTacQuocTe tbThoaThuanHopTacQuocTe)
        {
            try
            {
                if (tbThoaThuanHopTacQuocTe.IdThoaThuanHopTacQuocTe < 0)
                {
                    ModelState.AddModelError("IdThoaThuanHopTacQuocTe", "ID không được là số âm.");
                }
                if (string.IsNullOrWhiteSpace(tbThoaThuanHopTacQuocTe.TenThoaThuan))
                {
                    ModelState.AddModelError("TenThoaThuan", "Tên thoả thuận không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbThoaThuanHopTacQuocTe.MaThoaThuan))
                {
                    ModelState.AddModelError("MaThoaThuan", "Mã thoả thuận không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbThoaThuanHopTacQuocTe.TenToChuc))
                {
                    ModelState.AddModelError("TenToChuc", "Tên tổ chức không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbThoaThuanHopTacQuocTe.NoiDungTomTat))
                {
                    ModelState.AddModelError("NoiDungTomTat", "Nội dung tóm tắt không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbThoaThuanHopTacQuocTe.SoVanBanKyKet))
                {
                    ModelState.AddModelError("SoVanBanKyKet", "Số văn bản ký kết không được để trống");
                }
                if (tbThoaThuanHopTacQuocTe.NgayKyKet > tbThoaThuanHopTacQuocTe.NgayHetHan)
                {
                    ModelState.AddModelError("NgayHetHan", "Ngày hết hạn phải lớn hơn hoặc bằng ngày ký kết.");
                }
                if (tbThoaThuanHopTacQuocTe.NgayKyKet == null)
                {
                    ModelState.AddModelError("NgayKyKet", "Ngày ký kết không được để trống.");
                }
                if (tbThoaThuanHopTacQuocTe.NgayHetHan == null)
                {
                    ModelState.AddModelError("NgayHetHan", "Ngày hết hạn không được để trống.");
                }

                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbThoaThuanHopTacQuocTe>("/api/htqt/ThoaThuanHopTacQuocTe", tbThoaThuanHopTacQuocTe);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdQuocGia"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc", tbThoaThuanHopTacQuocTe.IdQuocGia);
                return View(tbThoaThuanHopTacQuocTe);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: TbThoaThuanHopTacQuocTes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id < 0) //Kiểm tra ID null hoặc số âm
            {
                ModelState.AddModelError("Id", "ID không được là số âm hoặc null.");
                return NotFound();
            }

            var tbThoaThuanHopTacQuocTe = await ApiServices_.GetId<TbThoaThuanHopTacQuocTe>("/api/htqt/ThoaThuanHopTacQuocTe", id ?? 0);
            if (tbThoaThuanHopTacQuocTe == null)
            {
                return NotFound();
            }
            ViewData["IdQuocGia"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc", tbThoaThuanHopTacQuocTe.IdQuocGia);
            return View(tbThoaThuanHopTacQuocTe);
        }

        // POST: TbThoaThuanHopTacQuocTes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdThoaThuanHopTacQuocTe,MaThoaThuan,TenThoaThuan,NoiDungTomTat,TenToChuc,NgayKyKet,SoVanBanKyKet,IdQuocGia,NgayHetHan")] TbThoaThuanHopTacQuocTe tbThoaThuanHopTacQuocTe)
        {
            try
            {
                if (id != tbThoaThuanHopTacQuocTe.IdThoaThuanHopTacQuocTe)
                {
                    return NotFound();
                }

                if (tbThoaThuanHopTacQuocTe.IdThoaThuanHopTacQuocTe < 0)
                {
                    ModelState.AddModelError("IdThoaThuanHopTacQuocTe", "ID không được là số âm.");
                }
                if (string.IsNullOrWhiteSpace(tbThoaThuanHopTacQuocTe.TenThoaThuan))
                {
                    ModelState.AddModelError("TenThoaThuan", "Tên thoả thuận không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbThoaThuanHopTacQuocTe.MaThoaThuan))
                {
                    ModelState.AddModelError("MaThoaThuan", "Mã thoả thuận không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbThoaThuanHopTacQuocTe.TenToChuc))
                {
                    ModelState.AddModelError("TenToChuc", "Tên tổ chức không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbThoaThuanHopTacQuocTe.NoiDungTomTat))
                {
                    ModelState.AddModelError("NoiDungTomTat", "Nội dung tóm tắt không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbThoaThuanHopTacQuocTe.SoVanBanKyKet))
                {
                    ModelState.AddModelError("SoVanBanKyKet", "Số văn bản ký kết không được để trống");
                }
                if (tbThoaThuanHopTacQuocTe.NgayKyKet > tbThoaThuanHopTacQuocTe.NgayHetHan)
                {
                    ModelState.AddModelError("NgayHetHan", "Ngày hết hạn phải lớn hơn hoặc bằng ngày ký kết.");
                }
                if (tbThoaThuanHopTacQuocTe.NgayKyKet == null)
                {
                    ModelState.AddModelError("NgayKyKet", "Ngày ký kết không được để trống.");
                }
                if (tbThoaThuanHopTacQuocTe.NgayHetHan == null)
                {
                    ModelState.AddModelError("NgayHetHan", "Ngày hết hạn không được để trống.");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbThoaThuanHopTacQuocTe>("/api/htqt/ThoaThuanHopTacQuocTe", id, tbThoaThuanHopTacQuocTe);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbThoaThuanHopTacQuocTeExists(tbThoaThuanHopTacQuocTe.IdThoaThuanHopTacQuocTe) == false)
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
                ViewData["IdQuocGia"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc", tbThoaThuanHopTacQuocTe.IdQuocGia);
                return View(tbThoaThuanHopTacQuocTe);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: TbThoaThuanHopTacQuocTes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbThoaThuanHopTacQuocTes = await TbThoaThuanHopTacQuocTes();
            var tbThoaThuanHopTacQuocTe = tbThoaThuanHopTacQuocTes.FirstOrDefault(m => m.IdThoaThuanHopTacQuocTe == id);
            if (tbThoaThuanHopTacQuocTe == null)
            {
                return NotFound();
            }

            return View(tbThoaThuanHopTacQuocTe);
        }

        // POST: TbThoaThuanHopTacQuocTes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await ApiServices_.Delete<TbThoaThuanHopTacQuocTe>("/api/htqt/ThoaThuanHopTacQuocTe", id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TbThoaThuanHopTacQuocTeExists(int id)
        {
            var TbThoaThuanHopTacQuocTes = await ApiServices_.GetAll<TbThoaThuanHopTacQuocTe>("/api/htqt/ThoaThuanHopTacQuocTe");
            return TbThoaThuanHopTacQuocTes.Any(e => e.IdThoaThuanHopTacQuocTe == id);
        }

        public async Task<IActionResult> Receive(string json)
        {
            try
            {
                // Khai báo thông báo mặc định
                var message = "Không phát hiện lỗi";
                // Giải mã dữ liệu JSON từ client
                List<List<string>> data = JsonConvert.DeserializeObject<List<List<string>>>(json);

                // Danh sách lưu các đối tượng TbThoaThuanHopTacQuocTe
                List<TbThoaThuanHopTacQuocTe> lst = new List<TbThoaThuanHopTacQuocTe>();

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();
                List<TbThoaThuanHopTacQuocTe> tbThoaThuanHopTacQuocTes = await ApiServices_.GetAll<TbThoaThuanHopTacQuocTe>("/api/htqt/ThoaThuanHopTacQuocTe");
                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    TbThoaThuanHopTacQuocTe model = new TbThoaThuanHopTacQuocTe();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (lst.Any(t => t.IdThoaThuanHopTacQuocTe == id) || tbThoaThuanHopTacQuocTes.Any(t => t.IdThoaThuanHopTacQuocTe == id)); // Kiểm tra id có tồn tại không


                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdThoaThuanHopTacQuocTe = id; // Gán ID
                    model.MaThoaThuan = item[0];
                    model.TenThoaThuan = item[1];
                    model.NoiDungTomTat = item[2];
                    model.TenToChuc = item[3];
                    model.NgayKyKet = DateOnly.ParseExact(item[4], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.SoVanBanKyKet = (item[5]);
                    model.NgayHetHan = DateOnly.ParseExact(item[6], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.IdQuocGia = ParseInt(item[7]);
                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu (giả sử có một phương thức tạo đối tượng trong DB)
                foreach (var item in lst)
                {
                    await CreateTbThoaThuanHopTacQuocTe(item); // Giả sử có phương thức tạo dữ liệu vào DB
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về thông báo lỗi
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbThoaThuanHopTacQuocTe(TbThoaThuanHopTacQuocTe item)
        {
            await ApiServices_.Create<TbThoaThuanHopTacQuocTe>("/api/htqt/ThoaThuanHopTacQuocTe", item);
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
