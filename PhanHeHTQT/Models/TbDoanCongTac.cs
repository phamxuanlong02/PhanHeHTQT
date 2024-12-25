using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PhanHeHTQT.Models.DM;

namespace PhanHeHTQT.Models;

public partial class TbDoanCongTac
{
    [Display(Name = "Id đoàn công tác")]

    public int IdDoanCongTac { get; set; }
    [Display(Name = "Mã đoàn công tác")]

    public string? MaDoanCongTac { get; set; }
    [Display(Name = "Đầu ra/Đầu vào")]

    public int? IdPhanLoaiDoanRaDoanVao { get; set; }
    [Display(Name = "Tên đoàn công tác")]

    public string? TenDoanCongTac { get; set; }
    [Display(Name = "Số quyết định")]

    public string? SoQuyetDinh { get; set; }
    [Display(Name = "Ngày quyết định")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? NgayQuyetDinh { get; set; }

    [Display(Name = "Quốc gia")]

    public int? IdQuocGiaDoan { get; set; }
    [Display(Name = "Thời gian bắt đầu")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? ThoiGianBatDau { get; set; }
    [Display(Name = "Thời gian kết thúc")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? ThoiGianketThuc { get; set; }
    [Display(Name = "Mục đích")]

    public string? MucDichCongTac { get; set; }
    [Display(Name = "Kết quả")]

    public string? KetQuaCongTac { get; set; }
    [Display(Name = "Đầu ra/Đầu vào")]

    public virtual DmPhanLoaiDoanRaDoanVao? IdPhanLoaiDoanRaDoanVaoNavigation { get; set; }
    [Display(Name = "Quốc gia")]

    public virtual DmQuocTich? IdQuocGiaDoanNavigation { get; set; }

    public virtual ICollection<TbThanhPhanThamGiaDoanCongTac> TbThanhPhanThamGiaDoanCongTacs { get; set; } = new List<TbThanhPhanThamGiaDoanCongTac>();
}
