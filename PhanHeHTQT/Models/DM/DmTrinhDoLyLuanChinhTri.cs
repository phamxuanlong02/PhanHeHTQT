﻿using System;
using System.Collections.Generic;

namespace PhanHeHTQT.Models.DM;

public partial class DmTrinhDoLyLuanChinhTri
{
    public int IdTrinhDoLyLuanChinhTri { get; set; }

    public string? TenTrinhDoLyLuanChinhTri { get; set; }

    public virtual ICollection<TbNguoi> TbNguois { get; set; } = new List<TbNguoi>();
}
