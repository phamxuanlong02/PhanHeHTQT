﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace PhanHeHTQT.Models;

public partial class TbKhoanTrichLapQuy
{
    [Display(Name = "ID Khoản Trích Lập Quỹ")]
    public int IdKhoanTrichLapQuy { get; set; }

    [Display(Name = "Mã Khoản Trích Lập Quỹ")]
    public string? MaKhoanTrichLapQuy { get; set; }

    [Display(Name = "Tên Khoản Trích Lập Quỹ")]
    public string? TenKhoanTrichLapQuy { get; set; }

    [Display(Name = "Năm Tài Chính")]
    public string? NamTaiChinh { get; set; }

    [Display(Name = "Số Tiền")]
    public int? SoTien { get; set; }
}
