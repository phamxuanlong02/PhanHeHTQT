using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PhanHeHTQT.Models.DM;

namespace PhanHeHTQT.Models;

public partial class TbHoiThaoHoiNghi
{
    [Display(Name = "Id hội thảo hội nghị")]

    public int IdHoiThaoHoiNghi { get; set; }
    [Display(Name = "Mã hội thảo hội nghị")]

    public string? MaHoiThaoHoiNghi { get; set; }
    [Display(Name = "Tên hội thảo hội nghị")]

    public string? TenHoiThaoHoiNghi { get; set; }
    [Display(Name = "Cơ quan cấp phép")]

    public string? CoQuanCoThamQuyenCapPhep { get; set; }
    [Display(Name = "Mục tiêu")]

    public string? MucTieu { get; set; }
    [Display(Name = "Nội dung")]

    public string? NoiDung { get; set; }
    [Display(Name = "Số đại biểu tham dự")]

    public int? SoLuongDaiBieuThamDu { get; set; }
    [Display(Name = "Số đại biểu quốc tế")]

    public int? SoLuongDaiBieuQuocTeThamDu { get; set; }
    [Display(Name = "Thời gian tổ chức")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? ThoiGianToChuc { get; set; }
    [Display(Name = "Địa điểm tổ chức")]

    public string? DiaDiemToChuc { get; set; }
    [Display(Name = "Nguồn kinh phí")]

    public int? IdNguonKinhPhiHoiThao { get; set; }
    [Display(Name = "Đơn vị chủ trì")]

    public string? DonViChuTri { get; set; }
    [Display(Name = "Nguồn kinh phí")]

    public virtual DmNguonKinhPhi? IdNguonKinhPhiHoiThaoNavigation { get; set; }
}
