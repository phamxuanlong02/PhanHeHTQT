﻿using System;
using System.Collections.Generic;

namespace PhanHeHTQT.Models.DM;

public partial class DmNguonKinhPhiChoDeAn
{
    public int IdNguonKinhPhiChoDeAn { get; set; }

    public string? NguonKinhPhiChoDeAn { get; set; }

    public virtual ICollection<TbDeAnDuAnChuongTrinh> TbDeAnDuAnChuongTrinhs { get; set; } = new List<TbDeAnDuAnChuongTrinh>();
}
