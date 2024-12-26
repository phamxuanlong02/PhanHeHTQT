using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PhanHeHTQT.API;
using PhanHeHTQT.Models;
using PhanHeHTQT.Models.DM;
using System.Globalization;

namespace PhanHeHTQT.Controllers.HTQT
{
    public class TbToChucHopTacQuocTesController : Controller
    {
        private readonly ApiServices ApiServices_;

        public TbToChucHopTacQuocTesController(ApiServices services)
        {
            ApiServices_ = services;
        }

        private async Task<List<TbToChucHopTacQuocTe>> TbToChucHopTacQuocTes()
        {
            List<TbToChucHopTacQuocTe> tbToChucHopTacQuocTes = await ApiServices_.GetAll<TbToChucHopTacQuocTe>("/api/htqt/ToChucHopTacQuocTe");
            List<DmQuocTich> dmQuocTiches = await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich");
            List<DmHinhThucHopTac> dmHinhThucHopTacs = await ApiServices_.GetAll<DmHinhThucHopTac>("/api/dm/HinhThucHopTac");
            tbToChucHopTacQuocTes.ForEach(item =>
            {
                item.IdQuocGiaNavigation = dmQuocTiches.FirstOrDefault(x => x.IdQuocTich == item.IdQuocGia);
            });
            return tbToChucHopTacQuocTes;
        }

        // GET: TbToChucHopTacQuocTes
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbToChucHopTacQuocTe> getall = await TbToChucHopTacQuocTes();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);

                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> Statistics()
        {
            try
            {
                List<TbToChucHopTacQuocTe> getall = await TbToChucHopTacQuocTes();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);

                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: TbToChucHopTacQuocTes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id < 0) //Kiểm tra ID null hoặc số âm
            {
                ModelState.AddModelError("Id", "ID không được là số âm hoặc null.");
                return NotFound();
            }

            var tbToChucHopTacQuocTes = await TbToChucHopTacQuocTes();
            var tbToChucHopTacQuocTe = tbToChucHopTacQuocTes.FirstOrDefault(m => m.IdToChucHopTacQuocTe == id);
            if (tbToChucHopTacQuocTe == null)
            {
                return NotFound();
            }

            return View(tbToChucHopTacQuocTe);
        }

        // GET: TbToChucHopTacQuocTes/Create
        public async Task<IActionResult> Create()
        {
            ViewData["IdQuocGia"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc");
            ViewData["IdHinhThucHopTac"] = new SelectList(await ApiServices_.GetAll<DmHinhThucHopTac>("/api/dm/HinhThucHopTac"), "IdHinhThucHopTac", "TenHinhThuc"); // Adjust the fields as necessary



            return View();
        }

        // POST: TbToChucHopTacQuocTes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdToChucHopTacQuocTe,MaHopTac,TenToChuc,IdQuocGia,NoiDung,ThoiGianKyKet,KetQuaHopTac,LoaiToChuc")] TbToChucHopTacQuocTe tbToChucHopTacQuocTe)
        {
            try
            {
                if (tbToChucHopTacQuocTe.IdToChucHopTacQuocTe < 0)
                {
                    ModelState.AddModelError("IdToChucHopTacQuocTe", "ID không được là số âm.");
                }
                if (string.IsNullOrWhiteSpace(tbToChucHopTacQuocTe.MaHopTac))
                {
                    ModelState.AddModelError("MaHopTac", "Mã hợp tác không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbToChucHopTacQuocTe.TenToChuc))
                {
                    ModelState.AddModelError("TenToChuc", "Tên tổ chức không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbToChucHopTacQuocTe.NoiDung))
                {
                    ModelState.AddModelError("NoiDung", "Nội dung không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbToChucHopTacQuocTe.KetQuaHopTac))
                {
                    ModelState.AddModelError("KetQuaHopTac", "Kết quả hợp tác không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbToChucHopTacQuocTe.LoaiToChuc))
                {
                    ModelState.AddModelError("LoaiToChuc", "Loại tổ chức không được để trống");
                }
                if (tbToChucHopTacQuocTe.ThoiGianKyKet == null)
                {
                    ModelState.AddModelError("ThoiGianKyKet", "Thời gian ký kết không được để trống.");
                }

                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbToChucHopTacQuocTe>("/api/htqt/ToChucHopTacDoanhNghiep", tbToChucHopTacQuocTe);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdQuocGia"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc");
                ViewData["IdHinhThucHopTac"] = new SelectList(await ApiServices_.GetAll<DmHinhThucHopTac>("/api/dm/HinhThucHopTac"), "IdHinhThucHopTac", "TenHinhThuc");
                return View(tbToChucHopTacQuocTe);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: TbToChucHopTacQuocTes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id < 0) //Kiểm tra ID null hoặc số âm
            {
                ModelState.AddModelError("Id", "ID không được là số âm hoặc null.");
                return NotFound();
            }

            var tbToChucHopTacQuocTe = await ApiServices_.GetId<TbToChucHopTacQuocTe>("/api/htqt/ToChucHopTacQuocTe", id ?? 0);
            if (tbToChucHopTacQuocTe == null)
            {
                return NotFound();
            }
            ViewData["IdQuocGia"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc");
            return View(tbToChucHopTacQuocTe);
        }

        // POST: TbToChucHopTacQuocTes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdToChucHopTacQuocTe,MaHopTac,TenToChuc,IdQuocGia,NoiDung,ThoiGianKyKet,KetQuaHopTac,LoaiToChuc")] TbToChucHopTacQuocTe tbToChucHopTacQuocTe)
        {
            try
            {
                if (id != tbToChucHopTacQuocTe.IdToChucHopTacQuocTe)
                {
                    return NotFound();
                }

                if (tbToChucHopTacQuocTe.IdToChucHopTacQuocTe < 0)
                {
                    ModelState.AddModelError("IdToChucHopTacQuocTe", "ID không được là số âm.");
                }
                if (string.IsNullOrWhiteSpace(tbToChucHopTacQuocTe.MaHopTac))
                {
                    ModelState.AddModelError("MaHopTac", "Mã hợp tác không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbToChucHopTacQuocTe.TenToChuc))
                {
                    ModelState.AddModelError("TenToChuc", "Tên tổ chức không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbToChucHopTacQuocTe.NoiDung))
                {
                    ModelState.AddModelError("NoiDung", "Nội dung không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbToChucHopTacQuocTe.KetQuaHopTac))
                {
                    ModelState.AddModelError("KetQuaHopTac", "Kết quả hợp tác không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbToChucHopTacQuocTe.LoaiToChuc))
                {
                    ModelState.AddModelError("LoaiToChuc", "Loại tổ chức không được để trống");
                }
                if (tbToChucHopTacQuocTe.ThoiGianKyKet == null)
                {
                    ModelState.AddModelError("ThoiGianKyKet", "Thời gian ký kết không được để trống.");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbToChucHopTacQuocTe>("/api/htqt/ToChucHopTacQuocTe", id, tbToChucHopTacQuocTe);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbToChucHopTacQuocTeExists(tbToChucHopTacQuocTe.IdToChucHopTacQuocTe) == false)
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
                ViewData["IdQuocGia"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc");
                return View(tbToChucHopTacQuocTe);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: TbToChucHopTacQuocTes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tbToChucHopTacQuocTes = await TbToChucHopTacQuocTes();
            var tbToChucHopTacQuocTe = tbToChucHopTacQuocTes.FirstOrDefault(m => m.IdToChucHopTacQuocTe == id);
            if (tbToChucHopTacQuocTe == null)
            {
                return NotFound();
            }

            return View(tbToChucHopTacQuocTe);
        }

        // POST: TbToChucHopTacQuocTes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await ApiServices_.Delete<TbToChucHopTacQuocTe>("/api/htqt/ToChucHopTacQuocTe", id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TbToChucHopTacQuocTeExists(int id)
        {
            var tbToChucHopTacQuocTes = await ApiServices_.GetAll<TbToChucHopTacQuocTe>("/api/htqt/ToChucHopTacQuocTe");
            return tbToChucHopTacQuocTes.Any(e => e.IdToChucHopTacQuocTe == id);
        }

        public async Task<IActionResult> Receive(string json)
        {
            try
            {
                // Khai báo thông báo mặc định
                var message = "Không phát hiện lỗi";
                // Giải mã dữ liệu JSON từ client
                List<List<string>> data = JsonConvert.DeserializeObject<List<List<string>>>(json);

                // Danh sách lưu các đối tượng TbToChucHopTacQuocTe
                List<TbToChucHopTacQuocTe> lst = new List<TbToChucHopTacQuocTe>();

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();
                List<TbToChucHopTacQuocTe> tbToChucHopTacQuocTes = await ApiServices_.GetAll<TbToChucHopTacQuocTe>("/api/htqt/ToChucHopTacQuocTe");
                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    TbToChucHopTacQuocTe model = new TbToChucHopTacQuocTe();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (lst.Any(t => t.IdToChucHopTacQuocTe == id) || tbToChucHopTacQuocTes.Any(t => t.IdToChucHopTacQuocTe == id)); // Kiểm tra id có tồn tại không


                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdToChucHopTacQuocTe = id; // Gán ID
                    model.MaHopTac = item[0];
                    model.TenToChuc = item[1];
                    model.NoiDung = item[2];
                    model.ThoiGianKyKet = DateOnly.ParseExact(item[3], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.KetQuaHopTac = item[4];
                    model.LoaiToChuc = item[5];
                    model.IdQuocGia = ParseInt(item[6]);
                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu (giả sử có một phương thức tạo đối tượng trong DB)
                foreach (var item in lst)
                {
                    await CreateTbToChucHopTacQuocTe(item); // Giả sử có phương thức tạo dữ liệu vào DB
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về thông báo lỗi
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbToChucHopTacQuocTe(TbToChucHopTacQuocTe item)
        {
            await ApiServices_.Create<TbToChucHopTacQuocTe>("/api/htqt/ToChucHopTacQuocTe", item);
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
