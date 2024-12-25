using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PhanHeHTQT.Models.DM;

namespace PhanHeHTQT.Models;

public partial class TbToChucHopTacQuocTe
{
    [Display(Name = "Id tổ chức hợp tác quốc tế")]

    public int IdToChucHopTacQuocTe { get; set; }
    [Display(Name = "Mã hợp tác")]

    public string? MaHopTac { get; set; }
    [Display(Name = "Tên tổ chức")]

    public string? TenToChuc { get; set; }
    [Display(Name = "Quốc gia")]

    public int? IdQuocGia { get; set; }
    [Display(Name = "Nội dung")]

    public string? NoiDung { get; set; }
    [Display(Name = "Thời gian ký kết")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? ThoiGianKyKet { get; set; }
    [Display(Name = "Kết quả hợp tác")]

    public string? KetQuaHopTac { get; set; }
    [Display(Name = "Loại tổ chức")]

    public string? LoaiToChuc { get; set; }
    [Display(Name = "Quốc gia")]

    public virtual DmQuocTich? IdQuocGiaNavigation { get; set; }

    public virtual ICollection<TbThongTinHopTac> TbThongTinHopTacs { get; set; } = new List<TbThongTinHopTac>();
}
