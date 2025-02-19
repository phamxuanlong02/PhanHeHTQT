﻿using System;
using System.Collections.Generic;

namespace PhanHeHTQT.Models.DM;

public partial class DmPhanLoaiCoSo
{
    public int IdPhanLoaiCoSo { get; set; }

    public string? PhanLoaiCoSo { get; set; }

    public virtual ICollection<TbCoSoGiaoDuc> TbCoSoGiaoDucs { get; set; } = new List<TbCoSoGiaoDuc>();
}
