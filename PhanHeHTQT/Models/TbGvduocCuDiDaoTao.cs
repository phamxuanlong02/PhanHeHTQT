using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PhanHeHTQT.Models.DM;

namespace PhanHeHTQT.Models;

public partial class TbGvduocCuDiDaoTao
{
    [Display(Name = "Id Giảng viên")]

    public int IdGvduocCuDiDaoTao { get; set; }
    [Display(Name = "Id cán bộ")]

    public int? IdCanBo { get; set; }
    [Display(Name = "Hình thức tham gia")]

    public int? IdHinhThucThamGiaGvduocCuDiDaoTao { get; set; }
    [Display(Name = "Quốc gia đến")]

    public int? IdQuocGiaDen { get; set; }
    [Display(Name = "Tên cơ sở tham gia")]

    public string? TenCoSoGiaoDucThamGiaOnn { get; set; }
    [Display(Name = "Thời gian bắt đầu")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? ThoiGianBatDau { get; set; }
    [Display(Name = "Thời gian kết thúc")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? ThoiGianKetThuc { get; set; }
    [Display(Name ="Tình trạng")]

    public int? IdTinhTrangGvduocCuDiDaoTao { get; set; }
    [Display(Name = "Nguồn kinh phí")]

    public int? IdNguonKinhPhiCuaGv { get; set; }
    [Display(Name = "Cán bộ")]

    public virtual TbCanBo? IdCanBoNavigation { get; set; }
    [Display(Name = "Hình thức tham gia")]

    public virtual DmHinhThucThamGiaGvduocCuDiDaoTao? IdHinhThucThamGiaGvduocCuDiDaoTaoNavigation { get; set; }
    [Display(Name = "Nguồn kinh phí")]

    public virtual DmNguonKinhPhiCuaGvduocCuDiDaoTao? IdNguonKinhPhiCuaGvNavigation { get; set; }
    [Display(Name = "Quốc gia đến")]

    public virtual DmQuocTich? IdQuocGiaDenNavigation { get; set; }
    [Display(Name = "Tình trạng")]

    public virtual DmTinhTrangGiangVienDuocCuDiDaoTao? IdTinhTrangGvduocCuDiDaoTaoNavigation { get; set; }
}
