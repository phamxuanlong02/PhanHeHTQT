using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class TbDoanCongTacsController : Controller
    {
        private readonly ApiServices ApiServices_;

        public TbDoanCongTacsController(ApiServices services)
        {
            ApiServices_ = services;
        }

        private async Task<List<TbDoanCongTac>> TbDoanCongTacs()
        {
            List<TbDoanCongTac> tbDoanCongTacs = await ApiServices_.GetAll<TbDoanCongTac>("/api/htqt/DoanCongTac");
            List<DmPhanLoaiDoanRaDoanVao> dmPhanLoaiDoanRaDoanVaos = await ApiServices_.GetAll<DmPhanLoaiDoanRaDoanVao>("/api/dm/PhanLoaiDoanRaDoanVao");
            List<DmQuocTich> dmQuocTiches = await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich");
            tbDoanCongTacs.ForEach(item =>
            {
                item.IdPhanLoaiDoanRaDoanVaoNavigation = dmPhanLoaiDoanRaDoanVaos.FirstOrDefault(x => x.IdPhanLoaiDoanRaDoanVao == item.IdPhanLoaiDoanRaDoanVao);
                item.IdQuocGiaDoanNavigation = dmQuocTiches.FirstOrDefault(x => x.IdQuocTich == item.IdQuocGiaDoan);
            });
            return tbDoanCongTacs;
        }

        // GET: TbDoanCongTacs
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbDoanCongTac> getall = await TbDoanCongTacs();
                return View(getall);
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
                List<TbDoanCongTac> getall = await TbDoanCongTacs();
                return View(getall);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: TbDoanCongTacs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null || id < 0) //Kiểm tra ID null hoặc số âm
                {
                    ModelState.AddModelError("Id", "ID không được là số âm hoặc null.");
                    return NotFound();
                }

                var tbDoanCongTacs = await TbDoanCongTacs();
                var tbDoanCongTac = tbDoanCongTacs.FirstOrDefault(m => m.IdDoanCongTac == id);
                if (tbDoanCongTac == null)
                {
                    return NotFound();
                }

                return View(tbDoanCongTac);
            }
            catch (Exception ex) 
            {
                return BadRequest();
            }
                            
        }

        /// <summary>
        /// hàm khởi tạo
        /// </summary> tạo danh sách 
        /// <returns></returns> 
        public async Task<IActionResult> Create()
        {
            try {
                ViewData["IdPhanLoaiDoanRaDoanVao"] = new SelectList(await ApiServices_.GetAll<DmPhanLoaiDoanRaDoanVao>("/api/dm/PhanLoaiDoanRaDoanVao"), "IdPhanLoaiDoanRaDoanVao", "PhanLoaiDoanRaDoanVao");
                ViewData["IdQuocGiaDoan"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc");
                return View();
            } 
            catch (Exception ex)
            { 
                return BadRequest(); 
            }            
        }

        // POST: TbDoanCongTacs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDoanCongTac,MaDoanCongTac,IdPhanLoaiDoanRaDoanVao,TenDoanCongTac,SoQuyetDinh,NgayQuyetDinh,IdQuocGiaDoan,ThoiGianBatDau,ThoiGianketThuc,MucDichCongTac,KetQuaCongTac")] TbDoanCongTac tbDoanCongTac)
        {
            try
            {

                if (await TbDoanCongTacExists(tbDoanCongTac.IdDoanCongTac)) ModelState.AddModelError("IdDoanCongTac", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbDoanCongTac>("/api/htqt/DoanCongTac", tbDoanCongTac);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdPhanLoaiDoanRaDoanVao"] = new SelectList(await ApiServices_.GetAll<DmPhanLoaiDoanRaDoanVao>("/api/dm/PhanLoaiDoanRaDoanVao"), "IdPhanLoaiDoanRaDoanVao", "PhanLoaiDoanRaDoanVao", tbDoanCongTac.IdPhanLoaiDoanRaDoanVao);
                ViewData["IdQuocGiaDoan"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc", tbDoanCongTac.IdQuocGiaDoan);
                return View(tbDoanCongTac);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            
        }

        // GET: TbDoanCongTacs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try {
                if (id == null || id < 0) //Kiểm tra ID null hoặc số âm
                {
                    ModelState.AddModelError("Id", "ID không được là số âm hoặc null.");
                    return NotFound();
                }

                var tbDoanCongTac = await ApiServices_.GetId<TbDoanCongTac>("/api/htqt/DoanCongTac", id ?? 0);
                if (tbDoanCongTac == null)
                {
                    return NotFound();
                }
                ViewData["IdPhanLoaiDoanRaDoanVao"] = new SelectList(await ApiServices_.GetAll<DmPhanLoaiDoanRaDoanVao>("/api/dm/PhanLoaiDoanRaDoanVao"), "IdPhanLoaiDoanRaDoanVao", "PhanLoaiDoanRaDoanVao", tbDoanCongTac.IdPhanLoaiDoanRaDoanVao);
                ViewData["IdQuocGiaDoan"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc", tbDoanCongTac.IdQuocGiaDoan);
                return View(tbDoanCongTac);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            
        }

        // POST: TbDoanCongTacs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDoanCongTac,MaDoanCongTac,IdPhanLoaiDoanRaDoanVao,TenDoanCongTac,SoQuyetDinh,NgayQuyetDinh,IdQuocGiaDoan,ThoiGianBatDau,ThoiGianketThuc,MucDichCongTac,KetQuaCongTac")] TbDoanCongTac tbDoanCongTac)
        {
            try
            {
                if (id != tbDoanCongTac.IdDoanCongTac)
                {
                    return NotFound();
                }


                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbDoanCongTac>("/api/htqt/DoanCongTac", id, tbDoanCongTac);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbDoanCongTacExists(tbDoanCongTac.IdDoanCongTac) == false)
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
                ViewData["IdPhanLoaiDoanRaDoanVao"] = new SelectList(await ApiServices_.GetAll<DmPhanLoaiDoanRaDoanVao>("/api/dm/PhanLoaiDoanRaDoanVao"), "IdPhanLoaiDoanRaDoanVao", "PhanLoaiDoanRaDoanVao", tbDoanCongTac.IdPhanLoaiDoanRaDoanVao);
                ViewData["IdQuocGiaDoan"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc", tbDoanCongTac.IdQuocGiaDoan);
                return View(tbDoanCongTac);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: TbDoanCongTacs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var tbDoanCongTacs = await TbDoanCongTacs();
                var tbDoanCongTac = tbDoanCongTacs.FirstOrDefault(m => m.IdDoanCongTac == id);
                if (tbDoanCongTac == null)
                {
                    return NotFound();
                }

                return View(tbDoanCongTac);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            
        }

        // POST: TbDoanCongTacs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbDoanCongTac>("/api/htqt/DoanCongTac", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<bool> TbDoanCongTacExists(int id)
        {
            var tbDoanCongTacs = await ApiServices_.GetAll<TbDoanCongTac>("/api/htqt/DoanCongTac");
            return tbDoanCongTacs.Any(e => e.IdDoanCongTac == id);
        }

        public async Task<IActionResult> Receive(string json)
        {
            try
            {
                // Khai báo thông báo mặc định
                var message = "Không phát hiện lỗi";
                // Giải mã dữ liệu JSON từ client
                List<List<string>> data = JsonConvert.DeserializeObject<List<List<string>>>(json);

                List<TbDoanCongTac> lst = new List<TbDoanCongTac>();

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();
                List<TbDoanCongTac> tbDoanCongTacs = await ApiServices_.GetAll<TbDoanCongTac>("/api/htqt/DoanCongTac");
                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    TbDoanCongTac model = new TbDoanCongTac();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (lst.Any(t => t.IdDoanCongTac == id) || tbDoanCongTacs.Any(t => t.IdDoanCongTac == id)); // Kiểm tra id có tồn tại không

                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdDoanCongTac = id; // Gán ID
                    model.MaDoanCongTac = item[0];
                    model.TenDoanCongTac = item[1];
                    model.SoQuyetDinh = item[2];
                    model.NgayQuyetDinh = DateOnly.ParseExact(item[3], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.ThoiGianBatDau = DateOnly.ParseExact(item[4], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.ThoiGianketThuc = DateOnly.ParseExact(item[5], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.MucDichCongTac = item[7];
                    model.KetQuaCongTac = item[8];
                    model.IdPhanLoaiDoanRaDoanVao = ParseInt(item[8]);
                    model.IdQuocGiaDoan = ParseInt(item[9]);
                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu (giả sử có một phương thức tạo đối tượng trong DB)
                foreach (var item in lst)
                {
                    await CreateTbDoanCongTac(item); // Giả sử có phương thức tạo dữ liệu vào DB
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về thông báo lỗi
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbDoanCongTac(TbDoanCongTac item)
        {
            await ApiServices_.Create<TbDoanCongTac>("/api/htqt/DoanCongTac", item);
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
