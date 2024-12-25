using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PhanHeHTQT.Models.DM;

namespace PhanHeHTQT.Models;

public partial class TbToChucHopTacDoanhNghiep
{
    [Display(Name = "Id tổ chức hợp tác doanh nghiệp")]

    public int IdToChucHopTacDoanhNghiep { get; set; }
    [Display(Name = "Mã tổ chức")]

    public string? MaToChucHopTacDoanhNghiep { get; set; }
    [Display(Name = "Tên tổ chức ")]

    public string? TenToChucHopTacDoanhNghiep { get; set; }
    [Display(Name = "Nội dung hợp tác")]

    public string? NoiDungHopTac { get; set; }
    [Display(Name = "Ngày ký kết")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? NgayKyKet { get; set; }
    [Display(Name = "Kết quả hợp tác")]

    public string? KetQuaHopTac { get; set; }
    [Display(Name = "Loại đề án")]

    public int? IdLoaiDeAn { get; set; }
    [Display(Name = "Giá trị giao dịch")]

    public double? GiaTriGiaoDichCuaThiTruong { get; set; }
    [Display(Name = "Loại đề án")]

    public virtual DmLoaiDeAnChuongTrinh? IdLoaiDeAnNavigation { get; set; }
}
