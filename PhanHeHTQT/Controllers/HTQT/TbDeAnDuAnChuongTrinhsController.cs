using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using PhanHeHTQT.API;
using PhanHeHTQT.Models;
using PhanHeHTQT.Models.DM;
using System.Globalization;


namespace PhanHeHTQT.Controllers.HTQT
{
    public class TbDeAnDuAnChuongTrinhsController : Controller
    {
        private readonly ApiServices ApiServices_;

        public TbDeAnDuAnChuongTrinhsController(ApiServices services)
        {
            ApiServices_ = services;
        }


        private async Task<List<TbDeAnDuAnChuongTrinh>> TbDeAnDuAnChuongTrinhs()
        {
            List<TbDeAnDuAnChuongTrinh> tbDeAnDuAnChuongTrinhs = await ApiServices_.GetAll<TbDeAnDuAnChuongTrinh>("/api/htqt/DeAnDuAnChuongTrinh");
            List<DmNguonKinhPhiChoDeAn> dmNguonKinhPhiChoDeAns = await ApiServices_.GetAll<DmNguonKinhPhiChoDeAn>("/api/dm/NguonKinhPhiChoDeAn");
            tbDeAnDuAnChuongTrinhs.ForEach(item =>
            {
                item.IdNguonKinhPhiDeAnDuAnChuongTrinhNavigation = dmNguonKinhPhiChoDeAns.FirstOrDefault(x => x.IdNguonKinhPhiChoDeAn == item.IdNguonKinhPhiDeAnDuAnChuongTrinh);
            });
            return tbDeAnDuAnChuongTrinhs;
        }

        // GET: TbDeAnDuAnChuongTrinhs
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbDeAnDuAnChuongTrinh> getall = await TbDeAnDuAnChuongTrinhs();
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
                List<TbDeAnDuAnChuongTrinh> getall = await TbDeAnDuAnChuongTrinhs();
                    // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                    return View(getall);

                    // Bắt lỗi các trường hợp ngoại lệ
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }
            }
        // GET: TbDeAnDuAnChuongTrinhs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null || id < 0) //Kiểm tra ID null hoặc số âm
                {
                    ModelState.AddModelError("Id", "ID không được là số âm hoặc null.");
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbDeAnDuAnChuongTrinhs = await TbDeAnDuAnChuongTrinhs();
                var tbDeAnDuAnChuongTrinh = tbDeAnDuAnChuongTrinhs.FirstOrDefault(m => m.IdDeAnDuAnChuongTrinh == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbDeAnDuAnChuongTrinh == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbDeAnDuAnChuongTrinh);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: TbDeAnDuAnChuongTrinhs/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdNguonKinhPhiDeAnDuAnChuongTrinh"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhiChoDeAn>("/api/dm/NguonKinhPhiChoDeAn"), "IdNguonKinhPhiChoDeAn", "NguonKinhPhiChoDeAn");
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: TbDeAnDuAnChuongTrinhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDeAnDuAnChuongTrinh,MaDeAnDuAnChuongTrinh,TenDeAnDuAnChuongTrinh,NoiDungTomTat,MucTieu,ThoiGianHopTacTu,ThoiGianHopTacDen,TongKinhPhi,IdNguonKinhPhiDeAnDuAnChuongTrinh")] TbDeAnDuAnChuongTrinh tbDeAnDuAnChuongTrinh)
        {
            try
            {
                
                if (await TbDeAnDuAnChuongTrinhExists(tbDeAnDuAnChuongTrinh.IdDeAnDuAnChuongTrinh)) ModelState.AddModelError("IdDeAnDuAnChuongTrinh", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbDeAnDuAnChuongTrinh>("/api/htqt/DeAnDuAnChuongTrinh", tbDeAnDuAnChuongTrinh);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdNguonKinhPhiDeAnDuAnChuongTrinh"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhiChoDeAn>("/api/htqt/DeAnDuAnChuongTrinh"), "IdNguonKinhPhiChoDeAn", "NguonKinhPhiChoDeAn", tbDeAnDuAnChuongTrinh.IdNguonKinhPhiDeAnDuAnChuongTrinh);
                return View(tbDeAnDuAnChuongTrinh);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: TbDeAnDuAnChuongTrinhs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id < 0) //Kiểm tra ID null hoặc số âm
            {
                ModelState.AddModelError("Id", "ID không được là số âm hoặc null.");
                return NotFound();
            }

            var tbDeAnDuAnChuongTrinh = await ApiServices_.GetId<TbDeAnDuAnChuongTrinh>("/api/htqt/DeAnDuAnChuongTrinh", id ?? 0);
            if (tbDeAnDuAnChuongTrinh == null)
            {
                return NotFound();
            }
            ViewData["IdNguonKinhPhiDeAnDuAnChuongTrinh"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhiChoDeAn>("/api/dm/NguonKinhPhiChoDeAn"), "IdNguonKinhPhiChoDeAn", "NguonKinhPhiChoDeAn");
            return View(tbDeAnDuAnChuongTrinh);
        }

        // POST: TbDeAnDuAnChuongTrinhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDeAnDuAnChuongTrinh,MaDeAnDuAnChuongTrinh,TenDeAnDuAnChuongTrinh,NoiDungTomTat,MucTieu,ThoiGianHopTacTu,ThoiGianHopTacDen,TongKinhPhi,IdNguonKinhPhiDeAnDuAnChuongTrinh")] TbDeAnDuAnChuongTrinh tbDeAnDuAnChuongTrinh)
        {
            try
            {
                if (id != tbDeAnDuAnChuongTrinh.IdDeAnDuAnChuongTrinh)
                {
                    return NotFound();
                }


                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbDeAnDuAnChuongTrinh>("/api/htqt/DeAnDuAnChuongTrinh", id, tbDeAnDuAnChuongTrinh);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbDeAnDuAnChuongTrinhExists(tbDeAnDuAnChuongTrinh.IdDeAnDuAnChuongTrinh) == false)
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
                ViewData["IdNguonKinhPhiDeAnDuAnChuongTrinh"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhiChoDeAn>("/api/htqt/DeAnDuAnChuongTrinh"), "IdNguonKinhPhiChoDeAn", "NguonKinhPhiChoDeAn", tbDeAnDuAnChuongTrinh.IdNguonKinhPhiDeAnDuAnChuongTrinh);
                return View(tbDeAnDuAnChuongTrinh);
            } 
            catch (Exception ex){
                return BadRequest();
                }
            }

        // GET: TbDeAnDuAnChuongTrinhs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var tbDeAnDuAnChuongTrinhs = await TbDeAnDuAnChuongTrinhs();
                var tbDeAnDuAnChuongTrinh = tbDeAnDuAnChuongTrinhs.FirstOrDefault(m => m.IdDeAnDuAnChuongTrinh == id);
                if (tbDeAnDuAnChuongTrinh == null)
                {
                    return NotFound();
                }

                return View(tbDeAnDuAnChuongTrinh);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: TbDeAnDuAnChuongTrinhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbDeAnDuAnChuongTrinh>("/api/htqt/DeAnDuAnChuongTrinh", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            
        }

        //Import Excel

        private async Task<bool> TbDeAnDuAnChuongTrinhExists(int id)
        {
            var tbDeAnDuAnChuongTrinh = await ApiServices_.GetAll<TbDeAnDuAnChuongTrinh>("/api/htqt/DeAnDuAnChuongTrinh");
            return tbDeAnDuAnChuongTrinh.Any(e => e.IdDeAnDuAnChuongTrinh == id);
        }
        public async Task<IActionResult> Receive(string json)
        {
            try
            {
                // Khai báo thông báo mặc định
                var message = "Không phát hiện lỗi";
                List<DmNguonKinhPhi> dmNguonKinhPhis = await ApiServices_.GetAll<DmNguonKinhPhi>("/api/dm/NguonKinhPhiChoDeAn");
                // Giải mã dữ liệu JSON từ client
                List<List<string>> data = JsonConvert.DeserializeObject<List<List<string>>>(json);

                // Danh sách lưu các đối tượng TbDeAnDuAnChuongTrinh
                List<TbDeAnDuAnChuongTrinh> lst = new List<TbDeAnDuAnChuongTrinh>();

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();

                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    TbDeAnDuAnChuongTrinh model = new TbDeAnDuAnChuongTrinh();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (await TbDeAnDuAnChuongTrinhExists(id)); // Kiểm tra id có tồn tại không

                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdDeAnDuAnChuongTrinh = id; // Gán ID
                    model.MaDeAnDuAnChuongTrinh = item[0];
                    model.TenDeAnDuAnChuongTrinh = item[1];
                    model.NoiDungTomTat = item[2]; 
                    model.MucTieu = item[3];
                    model.ThoiGianHopTacTu = DateOnly.ParseExact(item[4], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.ThoiGianHopTacDen = DateOnly.ParseExact(item[5], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.TongKinhPhi = double.Parse(item[6], CultureInfo.InvariantCulture);
                    /*var model_NguonKinhPhi = dmNguonKinhPhis.FirstOrDefault(t => t.NguonKinhPhi == item[7]);
                    if (model_NguonKinhPhi != null)
                    {
                        model.IdNguonKinhPhiDeAnDuAnChuongTrinh = model_NguonKinhPhi.IdNguonKinhPhi;  // Giả sử bạn có một trường Id trong DmNguonKinhPhi
                    }
                    else
                    {
                        // Nếu không tìm thấy, bạn có thể xử lý theo cách khác (ví dụ: gán giá trị mặc định, log lỗi, v.v.)
                        model.IdNguonKinhPhiDeAnDuAnChuongTrinh = null; // hoặc gán một giá trị mặc định
                    }*/
                    model.IdNguonKinhPhiDeAnDuAnChuongTrinh = ParseInt(item[7]);
                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu (giả sử có một phương thức tạo đối tượng trong DB)
                foreach (var item in lst)
                {
                    await CreateTbDeAnDuAnChuongTrinh(item); // Giả sử có phương thức tạo dữ liệu vào DB
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về thông báo lỗi
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbDeAnDuAnChuongTrinh(TbDeAnDuAnChuongTrinh item)
        {
            await ApiServices_.Create<TbDeAnDuAnChuongTrinh>("/api/htqt/DeAnDuAnChuongTrinh", item);
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
