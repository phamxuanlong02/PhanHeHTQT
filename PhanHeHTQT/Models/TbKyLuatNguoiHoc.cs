﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PhanHeHTQT.Models.DM;

namespace PhanHeHTQT.Models;

public partial class TbKyLuatNguoiHoc
{
    [Display(Name = "Kỷ luật người học")]
    public int IdKyLuatNguoiHoc { get; set; }
    [Display(Name = "Học viên")]
    public int? IdHocVien { get; set; }
    [Display(Name = "Loại kỷ luật")]
    public int? IdLoaiKyLuat { get; set; }
    [Display(Name = "Lý do")]
    public string? LyDo { get; set; }
    [Display(Name = "Cấp quyết định")]
    public int? IdCapQuyetDinh { get; set; }
    [Display(Name = "Số quyết định")]
    public string? SoQuyetDinh { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    [Display(Name = "Ngày quyết định")]
    public DateOnly? NgayQuyetDinh { get; set; }
    [Display(Name = "Năm bị kỷ luật")]
    public string? NamBiKyLuat { get; set; }
    [Display(Name = "Cấp quyết định")]
    public virtual DmCapKhenThuong? IdCapQuyetDinhNavigation { get; set; }
    [Display(Name = "Học viên")]
    public virtual TbHocVien? IdHocVienNavigation { get; set; }
    [Display(Name = "Loại kỷ luật")]
    public virtual DmLoaiKyLuat? IdLoaiKyLuatNavigation { get; set; }
}