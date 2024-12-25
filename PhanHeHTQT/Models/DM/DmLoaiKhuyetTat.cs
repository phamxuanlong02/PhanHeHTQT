﻿using System;
using System.Collections.Generic;

namespace PhanHeHTQT.Models.DM;

public partial class DmLoaiKhuyetTat
{
    public int IdLoaiKhuyetTat { get; set; }

    public string? LoaiKhuyetTat { get; set; }

    public virtual ICollection<TbHocVien> TbHocViens { get; set; } = new List<TbHocVien>();
}
