using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhanHeHTQT.Models.DM;

namespace PhanHeHTQT.Models;

public partial class TbDeAnDuAnChuongTrinh
{
    [Display(Name = "ID Đề án, dự án, chương trình")]

    public int IdDeAnDuAnChuongTrinh { get; set; }
    [Display(Name = "Mã đề án")]

    public string? MaDeAnDuAnChuongTrinh { get; set; }
    [Display(Name = "Tên đề án, dự án, chương trình")]

    public string? TenDeAnDuAnChuongTrinh { get; set; }
    [Display(Name = "Nội dung tóm tắt")]

    public string? NoiDungTomTat { get; set; }
    [Display(Name = "Mục tiêu")]

    public string? MucTieu { get; set; }
    [Display(Name = "Thời gian hợp tác từ")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? ThoiGianHopTacTu { get; set; }
    [Display(Name = "Thời gian hợp tác đến")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? ThoiGianHopTacDen { get; set; }
    [Display(Name = "Tổng kinh phí")]
    [Range(0, double.MaxValue, ErrorMessage = "Nhập không đúng định dạng số")]
    [Column(TypeName = "float")]
    [DataType(DataType.Currency)]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public double? TongKinhPhi { get; set; }
    [Display(Name = "Nguồn kinh phí")]

    public int? IdNguonKinhPhiDeAnDuAnChuongTrinh { get; set; }
    [Display(Name = "Nguồn kinh phí")]

    public virtual DmNguonKinhPhiChoDeAn? IdNguonKinhPhiDeAnDuAnChuongTrinhNavigation { get; set; }
}
