using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhanHeHTQT.Models;
using PhanHeHTQT.Models.DM;
using PhanHeHTQT.API;
using Newtonsoft.Json;
using System.Globalization;
namespace PhanHeHTQT.Controllers.HTQT
{
    public class TbThongTinHopTacsController : Controller
    {
        private readonly ApiServices ApiServices_;

        public TbThongTinHopTacsController(ApiServices services)
        {
            ApiServices_ = services;
        }
        private async Task<List<TbThongTinHopTac>> TbThongTinHopTacs()
        {
            List<TbThongTinHopTac> tbThongTinHopTacs = await ApiServices_.GetAll<TbThongTinHopTac>("/api/htqt/thongtinhoptac");
            List<DmHinhThucHopTac> dmHinhThucHopTacs = await ApiServices_.GetAll<DmHinhThucHopTac>("/api/dm/HinhThucHopTac");
            List<TbToChucHopTacQuocTe> tbToChucHopTacQuocTes = await ApiServices_.GetAll<TbToChucHopTacQuocTe>("/api/htqt/ToChucHopTacQuocTe");
            tbThongTinHopTacs.ForEach(item =>
            {
                item.IdHinhThucHopTacNavigation = dmHinhThucHopTacs.FirstOrDefault(x => x.IdHinhThucHopTac == item.IdHinhThucHopTac);
                item.IdToChucHopTacNavigation = tbToChucHopTacQuocTes.FirstOrDefault(x => x.IdToChucHopTacQuocTe == item.IdToChucHopTac);
            });
            return tbThongTinHopTacs;
        }
        // GET: TbThongTinHopTacs
        public async Task<IActionResult> Index()
        {
            List<TbThongTinHopTac> getall = await TbThongTinHopTacs();
            return View(getall);
        }
        public async Task<IActionResult> Statistics()
        {
            List<TbThongTinHopTac> getall = await TbThongTinHopTacs();
            return View(getall);
        }
        // GET: TbThongTinHopTacs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id < 0) //Kiểm tra ID null hoặc số âm
            {
                ModelState.AddModelError("Id", "ID không được là số âm hoặc null.");
                return NotFound();
            }

            var tbThongTinHopTacs = await TbThongTinHopTacs();
            var tbThongTinHopTac = tbThongTinHopTacs.FirstOrDefault(m => m.IdThongTinHopTac == id);
            if (tbThongTinHopTac == null)
            {
                return NotFound();
            }

            return View(tbThongTinHopTac);
        }

        // GET: TbThongTinHopTacs/Create
        public async Task<IActionResult> Create()
        {


            ViewData["IdHinhThucHopTac"] = new SelectList(await ApiServices_.GetAll<DmHinhThucHopTac>("/api/dm/HinhThucHopTac"), "IdHinhThucHopTac", "HinhThucHopTac");
            ViewData["IdToChucHopTac"] = new SelectList(await ApiServices_.GetAll<TbToChucHopTacQuocTe>("/api/htqt/ToChucHopTacQuocTe"), "IdToChucHopTacQuocTe", "ToChucHopTacQuocTe");
            return View();
        }

        // POST: TbThongTinHopTacs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdThongTinHopTac,IdToChucHopTac,ThoiGianHopTacTu,ThoiGianHopTacDen,TenThoaThuan,ThongTinLienHeDoiTac,MucTieu,DonViTrienKhai,IdHinhThucHopTac,SanPhamChinh")] TbThongTinHopTac tbThongTinHopTac)
        {
            try
            {
                if (tbThongTinHopTac.IdToChucHopTac < 0)
                {
                    ModelState.AddModelError("IdToChucHopTac", "ID Tổ chức hợp tác không được là số âm.");
                }
                if (tbThongTinHopTac.IdHinhThucHopTac < 0)
                {
                    ModelState.AddModelError("IdHinhThucHopTac", "ID Hình thức hợp tác không được là số âm.");
                }
                if (string.IsNullOrWhiteSpace(tbThongTinHopTac.ThongTinLienHeDoiTac))
                {
                    ModelState.AddModelError("ThongTinLienHeDoiTac", "Thông tin liên hệ không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbThongTinHopTac.TenThoaThuan))
                {
                    ModelState.AddModelError("TenThoaThuan", "Tên thoả thuận không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbThongTinHopTac.MucTieu))
                {
                    ModelState.AddModelError("MucTieu", "Mục tiêu không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbThongTinHopTac.DonViTrienKhai))
                {
                    ModelState.AddModelError("DonViTrienKhai", "Đơn vị triển khai không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbThongTinHopTac.SanPhamChinh))
                {
                    ModelState.AddModelError("SanPhamChinh", "Sản phẩm chính không được để trống");
                }
                if (tbThongTinHopTac.ThoiGianHopTacTu > tbThongTinHopTac.ThoiGianHopTacDen)
                {
                    ModelState.AddModelError("ThoiGianHopTacTu", "Thời gian hợp tác đến phải lớn hơn hoặc bằng Thời gian hợp tác từ.");
                }
                if (tbThongTinHopTac.ThoiGianHopTacTu == null)
                {
                    ModelState.AddModelError("ThoiGianHopTacTu", "Thời gian hợp tác từ không được để trống.");
                }
                if (tbThongTinHopTac.ThoiGianHopTacDen == null)
                {
                    ModelState.AddModelError("ThoiGianHopTacDen", "Thời gian hợp tác đến không được để trống.");
                }

                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbThongTinHopTac>("/api/htqt/ThongTinHopTac", tbThongTinHopTac);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdHinhThucHopTac"] = new SelectList(await ApiServices_.GetAll<DmHinhThucHopTac>("/api/dm/HinhThucHopTac"), "IdHinhThucHopTac", "HinhThucHopTac", tbThongTinHopTac.IdHinhThucHopTac);
                ViewData["IdToChucHopTac"] = new SelectList(await ApiServices_.GetAll<TbToChucHopTacQuocTe>("/api/htqt/ToChucHopTacQuocTe"), "IdToChucHopTacQuocTe", "ToChucHopTacQuocTe", tbThongTinHopTac.IdToChucHopTac);
                return View(tbThongTinHopTac);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: TbThongTinHopTacs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id < 0) //Kiểm tra ID null hoặc số âm
            {
                ModelState.AddModelError("Id", "ID không được là số âm hoặc null.");
                return NotFound();
            }

            var tbThongTinHopTac = await ApiServices_.GetId<TbThongTinHopTac>("/api/htqt/ThongTinHoptac", id ?? 0);
            if (tbThongTinHopTac == null)
            {
                return NotFound();
            }
            ViewData["IdHinhThucHopTac"] = new SelectList(await ApiServices_.GetAll<DmHinhThucHopTac>("/api/dm/HinhThucHopTac"), "IdHinhThucHopTac", "HinhThucHopTac", tbThongTinHopTac.IdHinhThucHopTac);
            ViewData["IdToChucHopTac"] = new SelectList(await ApiServices_.GetAll<TbToChucHopTacQuocTe>("/api/htqt/ToChucHopTacQuocTe"), "IdToChucHopTacQuocTe", "TenToChuc", tbThongTinHopTac.IdToChucHopTac);
            return View(tbThongTinHopTac);
        }

        // POST: TbThongTinHopTacs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdThongTinHopTac,IdToChucHopTac,ThoiGianHopTacTu,ThoiGianHopTacDen,TenThoaThuan,ThongTinLienHeDoiTac,MucTieu,DonViTrienKhai,IdHinhThucHopTac,SanPhamChinh")] TbThongTinHopTac tbThongTinHopTac)
        {
            try
            {
                if (id != tbThongTinHopTac.IdThongTinHopTac)
                {
                    return NotFound();
                }

                if (tbThongTinHopTac.IdToChucHopTac < 0)
                {
                    ModelState.AddModelError("IdToChucHopTac", "ID Tổ chức hợp tác không được là số âm.");
                }
                if (tbThongTinHopTac.IdHinhThucHopTac < 0)
                {
                    ModelState.AddModelError("IdHinhThucHopTac", "ID Hình thức hợp tác không được là số âm.");
                }
                if (string.IsNullOrWhiteSpace(tbThongTinHopTac.TenThoaThuan))
                {
                    ModelState.AddModelError("TenThoaThuan", "Tên thoả thuận không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbThongTinHopTac.MucTieu))
                {
                    ModelState.AddModelError("MucTieu", "Mục tiêu không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbThongTinHopTac.DonViTrienKhai))
                {
                    ModelState.AddModelError("DonViTrienKhai", "Đơn vị triển khai không được để trống");
                }
                if (string.IsNullOrWhiteSpace(tbThongTinHopTac.SanPhamChinh))
                {
                    ModelState.AddModelError("SanPhamChinh", "Sản phẩm chính không được để trống");
                }
                if (tbThongTinHopTac.ThoiGianHopTacTu > tbThongTinHopTac.ThoiGianHopTacDen)
                {
                    ModelState.AddModelError("ThoiGianHopTacTu", "Thời gian hợp tác đến phải lớn hơn hoặc bằng Thời gian hợp tác từ.");
                }
                if (tbThongTinHopTac.ThoiGianHopTacTu == null)
                {
                    ModelState.AddModelError("ThoiGianHopTacTu", "Thời gian hợp tác từ không được để trống.");
                }
                if (tbThongTinHopTac.ThoiGianHopTacDen == null)
                {
                    ModelState.AddModelError("ThoiGianHopTacDen", "Thời gian hợp tác đến không được để trống.");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbThongTinHopTac>("/api/htqt/ThongTinHopTac", id, tbThongTinHopTac);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbThongTinHopTacExists(tbThongTinHopTac.IdThongTinHopTac) == false)
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
                ViewData["IdHinhThucHopTac"] = new SelectList(await ApiServices_.GetAll<DmHinhThucHopTac>("/api/dm/HinhThucHopTac"), "IdHinhThucHopTac", "HinhThucHopTac", tbThongTinHopTac.IdHinhThucHopTac);
                ViewData["IdToChucHopTac"] = new SelectList(await ApiServices_.GetAll<TbToChucHopTacQuocTe>("/api/htqt/ToChucHopTacQuocTe"), "IdToChucHopTacQuocTe", "TenToChuc", tbThongTinHopTac.IdToChucHopTac);
                return View(tbThongTinHopTac);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: TbThongTinHopTacs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbThongTinHopTacs = await TbThongTinHopTacs();
            var tbThongTinHopTac = tbThongTinHopTacs.FirstOrDefault(m => m.IdThongTinHopTac == id);
            if (tbThongTinHopTac == null)
            {
                return NotFound();
            }

            return View(tbThongTinHopTac);
        }

        // POST: TbThongTinHopTacs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await ApiServices_.Delete<TbThongTinHopTac>("/api/htqt/ThongTinHopTac", id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TbThongTinHopTacExists(int id)
        {
            var tbThongTinHopTacs = await ApiServices_.GetAll<TbThongTinHopTac>("/api/htqt/ThongTinHopTac");
            return tbThongTinHopTacs.Any(e => e.IdHinhThucHopTac == id);
        }

        public async Task<IActionResult> Receive(string json)
        {
            try
            {
                // Khai báo thông báo mặc định
                var message = "Không phát hiện lỗi";
                // Giải mã dữ liệu JSON từ client
                List<List<string>> data = JsonConvert.DeserializeObject<List<List<string>>>(json);

                // Danh sách lưu các đối tượng TbThongTinHopTac
                List<TbThongTinHopTac> lst = new List<TbThongTinHopTac>();

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();
                List<TbThongTinHopTac> tbThongTinHopTacs = await ApiServices_.GetAll<TbThongTinHopTac>("/api/htqt/ThongTinHopTac");
                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    TbThongTinHopTac model = new TbThongTinHopTac();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (lst.Any(t => t.IdThongTinHopTac == id) || tbThongTinHopTacs.Any(t => t.IdThongTinHopTac == id)); // Kiểm tra id có tồn tại không


                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdThongTinHopTac = id; // Gán ID
                    model.ThoiGianHopTacTu = DateOnly.ParseExact(item[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.ThoiGianHopTacDen = DateOnly.ParseExact(item[1], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.TenThoaThuan = item[2];
                    model.ThongTinLienHeDoiTac = item[3];
                    model.MucTieu = item[4];
                    model.DonViTrienKhai = item[5];
                    model.SanPhamChinh = item[6];
                    model.IdHinhThucHopTac = ParseInt(item[7]);
                    model.IdToChucHopTac = ParseInt(item[8]);
                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu (giả sử có một phương thức tạo đối tượng trong DB)
                foreach (var item in lst)
                {
                    await CreateTbThongTinHopTac(item); // Giả sử có phương thức tạo dữ liệu vào DB
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về thông báo lỗi
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbThongTinHopTac(TbThongTinHopTac item)
        {
            await ApiServices_.Create<TbThongTinHopTac>("/api/htqt/ThongTinHopTac", item);
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
