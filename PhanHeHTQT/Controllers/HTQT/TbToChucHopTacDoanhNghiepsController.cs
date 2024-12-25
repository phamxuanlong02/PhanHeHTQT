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
    public class TbToChucHopTacDoanhNghiepsController : Controller
    {
        private readonly ApiServices ApiServices_;

        public TbToChucHopTacDoanhNghiepsController(ApiServices services)
        {
            ApiServices_ = services;
        }
        private async Task<List<TbToChucHopTacDoanhNghiep>> TbToChucHopTacDoanhNghieps()
        {
            List<TbToChucHopTacDoanhNghiep> tbToChucHopTacDoanhNghieps = await ApiServices_.GetAll<TbToChucHopTacDoanhNghiep>("/api/htqt/ToChucHopTacDoanhNghiep");
            List<DmLoaiDeAnChuongTrinh> dmLoaiDeAnChuongTrinhs = await ApiServices_.GetAll<DmLoaiDeAnChuongTrinh>("/api/dm/LoaiDeAnChuongTrinh");
            tbToChucHopTacDoanhNghieps.ForEach(item =>
            {
                item.IdLoaiDeAnNavigation = dmLoaiDeAnChuongTrinhs.FirstOrDefault(x => x.IdLoaiDeAnChuongTrinh == item.IdLoaiDeAn);
            });
            return tbToChucHopTacDoanhNghieps;
        }
        // GET: TbToChucHopTacDoanhNghieps
        public async Task<IActionResult> Index()
        {
            List<TbToChucHopTacDoanhNghiep> getall = await TbToChucHopTacDoanhNghieps();
            return View(getall);
        }
        public async Task<IActionResult> Statistics()
        {
            List<TbToChucHopTacDoanhNghiep> getall = await TbToChucHopTacDoanhNghieps();
            return View(getall);
        }

        // GET: TbToChucHopTacDoanhNghieps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id < 0) //Kiểm tra ID null hoặc số âm
            {
                ModelState.AddModelError("Id", "ID không được là số âm hoặc null.");
                return NotFound();
            }

            var tbToChucHopTacDoanhNghieps = await TbToChucHopTacDoanhNghieps();
            var tbToChucHopTacDoanhNghiep = tbToChucHopTacDoanhNghieps.FirstOrDefault(m => m.IdToChucHopTacDoanhNghiep == id);

            if (tbToChucHopTacDoanhNghiep == null)
            {
                return NotFound();
            }

            return View(tbToChucHopTacDoanhNghiep);
        }

        // GET: TbToChucHopTacDoanhNghieps/Create
        public async Task<IActionResult> Create()
        {
            ViewData["IdLoaiDeAn"] = new SelectList(await ApiServices_.GetAll<DmLoaiDeAnChuongTrinh>("/api/dm/LoaiDeAnChuongTrinh"), "IdLoaiDeAnChuongTrinh", "LoaiDeAnChuongTrinh");
            return View();
        }

        // POST: TbToChucHopTacDoanhNghieps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdToChucHopTacDoanhNghiep,MaToChucHopTacDoanhNghiep,TenToChucHopTacDoanhNghiep,NoiDungHopTac,NgayKyKet,KetQuaHopTac,IdLoaiDeAn,GiaTriGiaoDichCuaThiTruong")] TbToChucHopTacDoanhNghiep tbToChucHopTacDoanhNghiep)
        {

            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbToChucHopTacDoanhNghiep>("/api/htqt/ToChucHopTacDoanhNghiep", tbToChucHopTacDoanhNghiep);

                return RedirectToAction(nameof(Index));
            }
            ViewData["IdLoaiDeAn"] = new SelectList(await ApiServices_.GetAll<DmLoaiDeAnChuongTrinh>("/api/dm/LoaiDeAnChuongTrinh"), "IdLoaiDeAnChuongTrinh", "LoaiDeAnChuongTrinh", tbToChucHopTacDoanhNghiep.IdLoaiDeAn);
            return View(tbToChucHopTacDoanhNghiep);
        }

        // GET: TbToChucHopTacDoanhNghieps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id < 0) //Kiểm tra ID null hoặc số âm
            {
                ModelState.AddModelError("Id", "ID không được là số âm hoặc null.");
                return NotFound();
            }

            var tbToChucHopTacDoanhNghiep = await ApiServices_.GetId<TbToChucHopTacDoanhNghiep>("/api/htqt/ToChucHopTacDoanhNghiep", id ?? 0);
            if (tbToChucHopTacDoanhNghiep == null)
            {
                return NotFound();
            }
            ViewData["IdLoaiDeAn"] = new SelectList(await ApiServices_.GetAll<DmLoaiDeAnChuongTrinh>("/api/dm/LoaiDeAnChuongTrinh"), "IdLoaiDeAnChuongTrinh", "LoaiDeAnChuongTrinh", tbToChucHopTacDoanhNghiep.IdLoaiDeAn);
            return View(tbToChucHopTacDoanhNghiep);
        }

        // POST: TbToChucHopTacDoanhNghieps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdToChucHopTacDoanhNghiep,MaToChucHopTacDoanhNghiep,TenToChucHopTacDoanhNghiep,NoiDungHopTac,NgayKyKet,KetQuaHopTac,IdLoaiDeAn,GiaTriGiaoDichCuaThiTruong")] TbToChucHopTacDoanhNghiep tbToChucHopTacDoanhNghiep)
        {
            if (id != tbToChucHopTacDoanhNghiep.IdToChucHopTacDoanhNghiep)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbToChucHopTacDoanhNghiep>("/api/htqt/ToChucHopTacDoanhNghiep", id, tbToChucHopTacDoanhNghiep);
                    if (id != tbToChucHopTacDoanhNghiep.IdToChucHopTacDoanhNghiep)
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {

                    if (await TbToChucHopTacDoanhNghiepExists(tbToChucHopTacDoanhNghiep.IdToChucHopTacDoanhNghiep) == false)
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
            ViewData["IdLoaiDeAn"] = new SelectList(await ApiServices_.GetAll<DmLoaiDeAnChuongTrinh>("/api/dm/LoaiDeAnChuongTrinh"), "IdLoaiDeAnChuongTrinh", "LoaiDeAnChuongTrinh", tbToChucHopTacDoanhNghiep.IdLoaiDeAn);
            return View(tbToChucHopTacDoanhNghiep);
        }

        // GET: TbToChucHopTacDoanhNghieps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbToChucHopTacDoanhNghieps = await TbToChucHopTacDoanhNghieps();
            var tbToChucHopTacDoanhNghiep = tbToChucHopTacDoanhNghieps.FirstOrDefault(m => m.IdToChucHopTacDoanhNghiep == id);
            if (tbToChucHopTacDoanhNghiep == null)
            {
                return NotFound();
            }

            return View(tbToChucHopTacDoanhNghiep);
        }

        // POST: TbToChucHopTacDoanhNghieps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await ApiServices_.Delete<TbToChucHopTacDoanhNghiep>("/api/htqt/ToChucHopTacDoanhNghiep", id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TbToChucHopTacDoanhNghiepExists(int id)
        {
            var tbToChucHopTacDoanhNghieps = await ApiServices_.GetAll<TbToChucHopTacDoanhNghiep>("/api/htqt/ToChucHopTacDoanhNghiep");
            return tbToChucHopTacDoanhNghieps.Any(e => e.IdToChucHopTacDoanhNghiep == id);
        }

        public async Task<IActionResult> Receive(string json)
        {
            try
            {
                // Khai báo thông báo mặc định
                var message = "Không phát hiện lỗi";
                // Giải mã dữ liệu JSON từ client
                List<List<string>> data = JsonConvert.DeserializeObject<List<List<string>>>(json);

                List<TbToChucHopTacDoanhNghiep> lst = new List<TbToChucHopTacDoanhNghiep>();

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();
                List<TbToChucHopTacDoanhNghiep> tbToChucHopTacDoanhNghieps = await ApiServices_.GetAll<TbToChucHopTacDoanhNghiep>("/api/htqt/ToChucHopTacDoanhNghiep");
                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    TbToChucHopTacDoanhNghiep model = new TbToChucHopTacDoanhNghiep();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (lst.Any(t => t.IdToChucHopTacDoanhNghiep == id) || tbToChucHopTacDoanhNghieps.Any(t => t.IdToChucHopTacDoanhNghiep == id)); // Kiểm tra id có tồn tại không


                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdToChucHopTacDoanhNghiep = id; // Gán ID
                    model.MaToChucHopTacDoanhNghiep = item[0];
                    model.TenToChucHopTacDoanhNghiep = item[1];
                    model.NoiDungHopTac = item[2];
                    model.NgayKyKet = DateOnly.ParseExact(item[3], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.KetQuaHopTac = item[4];
                    model.GiaTriGiaoDichCuaThiTruong = double.Parse(item[5]);
                    model.IdLoaiDeAn = ParseInt(item[2]);
                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu (giả sử có một phương thức tạo đối tượng trong DB)
                foreach (var item in lst)
                {
                    await CreateTbToChucHopTacDoanhNghiep(item); // Giả sử có phương thức tạo dữ liệu vào DB
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về thông báo lỗi
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbToChucHopTacDoanhNghiep(TbToChucHopTacDoanhNghiep item)
        {
            await ApiServices_.Create<TbToChucHopTacDoanhNghiep>("/api/htqt/ToChucHopTacDoanhNghiep", item);
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
