﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PhanHeHTQT.Models.DM;

namespace PhanHeHTQT.Models;

public partial class TbCoCauToChuc
{
    [Display(Name = "ID CƠ CẤU TỔ CHỨC")]
    public int IdCoCauToChuc { get; set; }

    [Display(Name = "MÃ PHÒNG BAN ĐƠN VỊ")]
    public string? MaPhongBanDonVi { get; set; }

    [Display(Name = "ID LOẠI PHÒNG BAN")]
    public int? IdLoaiPhongBan { get; set; }

    [Display(Name = "MÃ PHÒNG BAN ĐƠN VỊ CHA")]
    public string? MaPhongBanDonViCha { get; set; }

    [Display(Name = "TÊN PHÒNG BAN ĐƠN VỊ")]
    public string? TenPhongBanDonVi { get; set; }

    [Display(Name = "SỐ QUYẾT ĐỊNH THÀNH LẬP")]
    public string? SoQuyetDinhThanhLap { get; set; }

    [Display(Name = "NGÀY RA QUYẾT ĐỊNH")]
    public string? NgayRaQuyetDinh { get; set; }

    [Display(Name = "ID TRẠNG THÁI")]
    public int? IdTrangThai { get; set; }

    [Display(Name = "ID LOẠI PHÒNG BAN")]
    public virtual DmLoaiPhongBan? IdLoaiPhongBanNavigation { get; set; }

    [Display(Name = "ID TRẠNG THÁI")]
    public virtual DmTrangThaiCoSoGd? IdTrangThaiNavigation { get; set; }
}