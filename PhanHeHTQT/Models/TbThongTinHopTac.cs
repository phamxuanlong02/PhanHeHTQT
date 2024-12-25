using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PhanHeHTQT.Models.DM;

namespace PhanHeHTQT.Models;

public partial class TbThongTinHopTac
{
    [Display(Name = "Id thông tin hợp tác")]

    public int IdThongTinHopTac { get; set; }
    [Display(Name = "Tổ chức hợp tác")]

    public int? IdToChucHopTac { get; set; }
    [Display(Name = "Thời gian hợp tác từ")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? ThoiGianHopTacTu { get; set; }
    [Display(Name = "Thời gian hợp tác đến")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? ThoiGianHopTacDen { get; set; }
    [Display(Name = "Tên thỏa thuận")]

    public string? TenThoaThuan { get; set; }
    [Display(Name = "Thông tin liên hệ đối tác")]

    public string? ThongTinLienHeDoiTac { get; set; }
    [Display(Name = "Mục tiêu")]

    public string? MucTieu { get; set; }
    [Display(Name = "Đơn vị triển khai")]

    public string? DonViTrienKhai { get; set; }
    [Display(Name = "Id hình thức hợp tác")]

    public int? IdHinhThucHopTac { get; set; }
    [Display(Name = "Sản phẩm chính")]

    public string? SanPhamChinh { get; set; }
    [Display(Name = "Hình thức hợp tác")]

    public virtual DmHinhThucHopTac? IdHinhThucHopTacNavigation { get; set; }
    [Display(Name = "Tổ chức hợp tác")]

    public virtual TbToChucHopTacQuocTe? IdToChucHopTacNavigation { get; set; }
}
