using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class TbGvduocCuDiDaoTaosController : Controller
    {
        private readonly ApiServices ApiServices_;

        public TbGvduocCuDiDaoTaosController(ApiServices services)
        {
            ApiServices_ = services;
        }

        private async Task<List<TbGvduocCuDiDaoTao>> TbGvduocCuDiDaoTaos()
        {
            List<TbGvduocCuDiDaoTao> tbGvduocCuDiDaoTaos = await ApiServices_.GetAll<TbGvduocCuDiDaoTao>("/api/htqt/GVDuocCuDiDaoTao");
            List<TbCanBo> tbCanBos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<DmHinhThucThamGiaGvduocCuDiDaoTao> dmHinhThucThamGiaGvduocCuDiDaoTaos = await ApiServices_.GetAll<DmHinhThucThamGiaGvduocCuDiDaoTao>("/api/dm/HinhThucThamGiaGvduocCuDiDaoTao");
            List<DmNguonKinhPhiCuaGvduocCuDiDaoTao> dmNguonKinhPhiCuaGvduocCuDiDaoTaos = await ApiServices_.GetAll<DmNguonKinhPhiCuaGvduocCuDiDaoTao>("/api/dm/NguonKinhPhiCuaGvduocCuDiDaoTao");
            List<DmQuocTich> dmQuocTiches = await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich");
            List<DmTinhTrangGiangVienDuocCuDiDaoTao> dmTinhTrangGiangVienDuocCuDiDaoTaos = await ApiServices_.GetAll<DmTinhTrangGiangVienDuocCuDiDaoTao>("/api/dm/TinhTrangGiangVienDuocCuDiDaoTao");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            tbGvduocCuDiDaoTaos.ForEach(item =>
            {
                item.IdCanBoNavigation = tbCanBos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdHinhThucThamGiaGvduocCuDiDaoTaoNavigation = dmHinhThucThamGiaGvduocCuDiDaoTaos.FirstOrDefault(x => x.IdHinhThucThamGiaGvduocCuDiDaoTao == item.IdHinhThucThamGiaGvduocCuDiDaoTao);
                item.IdNguonKinhPhiCuaGvNavigation = dmNguonKinhPhiCuaGvduocCuDiDaoTaos.FirstOrDefault(x => x.IdNguonKinhPhiCuaGvduocCuDiDaoTao == item.IdNguonKinhPhiCuaGv);
                item.IdQuocGiaDenNavigation = dmQuocTiches.FirstOrDefault(x => x.IdQuocTich == item.IdQuocGiaDen);
                item.IdTinhTrangGvduocCuDiDaoTaoNavigation = dmTinhTrangGiangVienDuocCuDiDaoTaos.FirstOrDefault(x => x.IdTinhTrangGiangVienDuocCuDiDaoTao == item.IdTinhTrangGvduocCuDiDaoTao);
                item.IdCanBoNavigation.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdCanBoNavigation.IdNguoi);



            });
            return tbGvduocCuDiDaoTaos;
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
        // GET: TbGvduocCuDiDaoTaos
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbGvduocCuDiDaoTao> getall = await TbGvduocCuDiDaoTaos();
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
                List<TbGvduocCuDiDaoTao> getall = await TbGvduocCuDiDaoTaos();
                return View(getall);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: TbGvduocCuDiDaoTaos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var tbGvduocCuDiDaoTaos = await TbGvduocCuDiDaoTaos();
                var tbGvduocCuDiDaoTao = tbGvduocCuDiDaoTaos.FirstOrDefault(m => m.IdGvduocCuDiDaoTao == id);
                if (tbGvduocCuDiDaoTao == null)
                {
                    return NotFound();
                }

                return View(tbGvduocCuDiDaoTao);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: TbGvduocCuDiDaoTaos/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name");
                ViewData["IdHinhThucThamGiaGvduocCuDiDaoTao"] = new SelectList(await ApiServices_.GetAll<DmHinhThucThamGiaGvduocCuDiDaoTao>("/api/dm/HinhThucThamGiaGvduocCuDiDaoTao"), "IdHinhThucThamGiaGvduocCuDiDaoTao", "HinhThucThamGiaGvduocCuDiDaoTao");
                ViewData["IdNguonKinhPhiCuaGv"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhiCuaGvduocCuDiDaoTao>("/api/dm/NguonKinhPhiCuaGvduocCuDiDaoTao"), "IdNguonKinhPhiCuaGvduocCuDiDaoTao", "NguonKinhPhiCuaGvduocCuDiDaoTao");
                ViewData["IdQuocGiaDen"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc");
                ViewData["IdTinhTrangGvduocCuDiDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangGiangVienDuocCuDiDaoTao>("/api/dm/TinhTrangGiangVienDuocCuDiDaoTao"), "IdTinhTrangGiangVienDuocCuDiDaoTao", "TinhTrangGiangVienDuocCuDiDaoTao");
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: TbGvduocCuDiDaoTaos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGvduocCuDiDaoTao,IdCanBo,IdHinhThucThamGiaGvduocCuDiDaoTao,IdQuocGiaDen,TenCoSoGiaoDucThamGiaOnn,ThoiGianBatDau,ThoiGianKetThuc,IdTinhTrangGvduocCuDiDaoTao,IdNguonKinhPhiCuaGv")] TbGvduocCuDiDaoTao tbGvduocCuDiDaoTao)
        {
            if (await TbGvduocCuDiDaoTaoExists(tbGvduocCuDiDaoTao.IdGvduocCuDiDaoTao)) ModelState.AddModelError("IdGvduocCuDiDaoTao", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbGvduocCuDiDaoTao>("/api/htqt/GVDuocCuDiDaoTao", tbGvduocCuDiDaoTao);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbGvduocCuDiDaoTao.IdCanBo);
            ViewData["IdHinhThucThamGiaGvduocCuDiDaoTao"] = new SelectList(await ApiServices_.GetAll<DmHinhThucThamGiaGvduocCuDiDaoTao>("/api/dm/HinhThucThamGiaGvduocCuDiDaoTao"), "IdHinhThucThamGiaGvduocCuDiDaoTao", "HinhThucThamGiaGvduocCuDiDaoTao", tbGvduocCuDiDaoTao.IdHinhThucThamGiaGvduocCuDiDaoTao);
            ViewData["IdNguonKinhPhiCuaGv"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhiCuaGvduocCuDiDaoTao>("/api/dm/NguonKinhPhiCuaGvduocCuDiDaoTao"), "IdNguonKinhPhiCuaGvduocCuDiDaoTao", "NguonKinhPhiCuaGvduocCuDiDaoTao", tbGvduocCuDiDaoTao.IdNguonKinhPhiCuaGv);
            ViewData["IdQuocGiaDen"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc", tbGvduocCuDiDaoTao.IdQuocGiaDen);
            ViewData["IdTinhTrangGvduocCuDiDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangGiangVienDuocCuDiDaoTao>("/api/dm/TinhTrangGiangVienDuocCuDiDaoTao"), "IdTinhTrangGiangVienDuocCuDiDaoTao", "TinhTrangGiangVienDuocCuDiDaoTao", tbGvduocCuDiDaoTao.IdTinhTrangGvduocCuDiDaoTao);
            return View(tbGvduocCuDiDaoTao);
        }

        // GET: TbGvduocCuDiDaoTaos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbGvduocCuDiDaoTao = await ApiServices_.GetId<TbGvduocCuDiDaoTao>("/api/htqt/GVDuocCuDiDaoTao", id ?? 0);
            if (tbGvduocCuDiDaoTao == null)
            {
                return NotFound();
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbGvduocCuDiDaoTao.IdCanBo);
            ViewData["IdHinhThucThamGiaGvduocCuDiDaoTao"] = new SelectList(await ApiServices_.GetAll<DmHinhThucThamGiaGvduocCuDiDaoTao>("/api/dm/HinhThucThamGiaGvduocCuDiDaoTao"), "IdHinhThucThamGiaGvduocCuDiDaoTao", "HinhThucThamGiaGvduocCuDiDaoTao", tbGvduocCuDiDaoTao.IdHinhThucThamGiaGvduocCuDiDaoTao);
            ViewData["IdNguonKinhPhiCuaGv"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhiCuaGvduocCuDiDaoTao>("/api/dm/NguonKinhPhiCuaGvduocCuDiDaoTao"), "IdNguonKinhPhiCuaGvduocCuDiDaoTao", "NguonKinhPhiCuaGvduocCuDiDaoTao", tbGvduocCuDiDaoTao.IdNguonKinhPhiCuaGv);
            ViewData["IdQuocGiaDen"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc", tbGvduocCuDiDaoTao.IdQuocGiaDen);
            ViewData["IdTinhTrangGvduocCuDiDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangGiangVienDuocCuDiDaoTao>("/api/dm/TinhTrangGiangVienDuocCuDiDaoTao"), "IdTinhTrangGiangVienDuocCuDiDaoTao", "TinhTrangGiangVienDuocCuDiDaoTao", tbGvduocCuDiDaoTao.IdTinhTrangGvduocCuDiDaoTao);
            return View(tbGvduocCuDiDaoTao);
        }

        // POST: TbGvduocCuDiDaoTaos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdGvduocCuDiDaoTao,IdCanBo,IdHinhThucThamGiaGvduocCuDiDaoTao,IdQuocGiaDen,TenCoSoGiaoDucThamGiaOnn,ThoiGianBatDau,ThoiGianKetThuc,IdTinhTrangGvduocCuDiDaoTao,IdNguonKinhPhiCuaGv")] TbGvduocCuDiDaoTao tbGvduocCuDiDaoTao)
        {
            if (id != tbGvduocCuDiDaoTao.IdGvduocCuDiDaoTao)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbGvduocCuDiDaoTao>("/api/htqt/GVDuocCuDiDaoTao", id, tbGvduocCuDiDaoTao);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbGvduocCuDiDaoTaoExists(tbGvduocCuDiDaoTao.IdGvduocCuDiDaoTao) == false)
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
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbGvduocCuDiDaoTao.IdCanBo);
            ViewData["IdHinhThucThamGiaGvduocCuDiDaoTao"] = new SelectList(await ApiServices_.GetAll<DmHinhThucThamGiaGvduocCuDiDaoTao>("/api/dm/HinhThucThamGiaGvduocCuDiDaoTao"), "IdHinhThucThamGiaGvduocCuDiDaoTao", "HinhThucThamGiaGvduocCuDiDaoTao", tbGvduocCuDiDaoTao.IdHinhThucThamGiaGvduocCuDiDaoTao);
            ViewData["IdNguonKinhPhiCuaGv"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhiCuaGvduocCuDiDaoTao>("/api/dm/NguonKinhPhiCuaGvduocCuDiDaoTao"), "IdNguonKinhPhiCuaGvduocCuDiDaoTao", "NguonKinhPhiCuaGvduocCuDiDaoTao", tbGvduocCuDiDaoTao.IdNguonKinhPhiCuaGv);
            ViewData["IdQuocGiaDen"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc", tbGvduocCuDiDaoTao.IdQuocGiaDen);
            ViewData["IdTinhTrangGvduocCuDiDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangGiangVienDuocCuDiDaoTao>("/api/dm/TinhTrangGiangVienDuocCuDiDaoTao"), "IdTinhTrangGiangVienDuocCuDiDaoTao", "TinhTrangGiangVienDuocCuDiDaoTao", tbGvduocCuDiDaoTao.IdTinhTrangGvduocCuDiDaoTao);
            return View(tbGvduocCuDiDaoTao);
        }

        // GET: TbGvduocCuDiDaoTaos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbGvduocCuDiDaoTaos = await TbGvduocCuDiDaoTaos();
            var tbGvduocCuDiDaoTao = tbGvduocCuDiDaoTaos.FirstOrDefault(m => m.IdGvduocCuDiDaoTao == id);
            if (tbGvduocCuDiDaoTao == null)
            {
                return NotFound();
            }

            return View(tbGvduocCuDiDaoTao);
        }

        // POST: TbGvduocCuDiDaoTaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await ApiServices_.Delete<TbGvduocCuDiDaoTao>("/api/htqt/GVDuocCuDiDaoTao", id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TbGvduocCuDiDaoTaoExists(int id)
        {
            var tbGvduocCuDiDaoTaos = await ApiServices_.GetAll<TbGvduocCuDiDaoTao>("/api/htqt/GVDuocCuDiDaoTao");
            return tbGvduocCuDiDaoTaos.Any(e => e.IdGvduocCuDiDaoTao == id);
        }

        public async Task<IActionResult> Receive(string json)
        {
            try
            {
                // Khai báo thông báo mặc định
                var message = "Không phát hiện lỗi";
                // Giải mã dữ liệu JSON từ client
                List<List<string>> data = JsonConvert.DeserializeObject<List<List<string>>>(json);

                List<TbGvduocCuDiDaoTao> lst = new List<TbGvduocCuDiDaoTao>();

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();
                List<TbGvduocCuDiDaoTao> tbDoanCongTacs = await ApiServices_.GetAll<TbGvduocCuDiDaoTao>("/api/htqt/DoanCongTac");
                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    TbGvduocCuDiDaoTao model = new TbGvduocCuDiDaoTao();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (lst.Any(t => t.IdGvduocCuDiDaoTao == id) || tbDoanCongTacs.Any(t => t.IdGvduocCuDiDaoTao == id)); // Kiểm tra id có tồn tại không

                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdGvduocCuDiDaoTao = id; // Gán ID
                    model.TenCoSoGiaoDucThamGiaOnn = item[0];
                    model.ThoiGianBatDau = DateOnly.ParseExact(item[1], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.ThoiGianKetThuc = DateOnly.ParseExact(item[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.IdCanBo = ParseInt(item[3]);
                    model.IdHinhThucThamGiaGvduocCuDiDaoTao = ParseInt(item[4]);
                    model.IdNguonKinhPhiCuaGv = ParseInt(item[5]);
                    model.IdQuocGiaDen = ParseInt(item[6]);
                    model.IdTinhTrangGvduocCuDiDaoTao = ParseInt(item[7]);
                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu (giả sử có một phương thức tạo đối tượng trong DB)
                foreach (var item in lst)
                {
                    await CreateTbGvduocCuDiDaoTao(item); // Giả sử có phương thức tạo dữ liệu vào DB
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về thông báo lỗi
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbGvduocCuDiDaoTao(TbGvduocCuDiDaoTao item)
        {
            await ApiServices_.Create<TbGvduocCuDiDaoTao>("/api/htqt/GVDuocCuDiDaoTao", item);
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
