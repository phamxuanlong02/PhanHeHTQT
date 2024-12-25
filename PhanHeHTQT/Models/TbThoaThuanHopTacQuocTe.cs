using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PhanHeHTQT.Models.DM;

namespace PhanHeHTQT.Models;

public partial class TbThoaThuanHopTacQuocTe
{
    [Display(Name = "Id thỏa thuận")]

    public int IdThoaThuanHopTacQuocTe { get; set; }
    [Display(Name = "Mã thỏa thuận")]

    public string? MaThoaThuan { get; set; }
    [Display(Name = "Tên thỏa thuận")]

    public string? TenThoaThuan { get; set; }
    [Display(Name = "Nội dung tóm tắt")]

    public string? NoiDungTomTat { get; set; }
    [Display(Name = "Tên tổ chức")]

    public string? TenToChuc { get; set; }
    [Display(Name = "Ngày ký kết")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? NgayKyKet { get; set; }
    [Display(Name = "Số văn bản ký kết")]

    public string? SoVanBanKyKet { get; set; }
    [Display(Name = "Quốc gia")]

    public int? IdQuocGia { get; set; }
    [Display(Name = "Ngày hết hạn")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? NgayHetHan { get; set; }
    [Display(Name = "Quốc gia")]

    public virtual DmQuocTich? IdQuocGiaNavigation { get; set; }
}
