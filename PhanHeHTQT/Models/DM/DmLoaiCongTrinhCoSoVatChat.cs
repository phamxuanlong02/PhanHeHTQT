﻿using System;
using System.Collections.Generic;

namespace PhanHeHTQT.Models.DM;

public partial class DmLoaiCongTrinhCoSoVatChat
{
    public int IdLoaiCongTrinhCoSoVatChat { get; set; }

    public string? LoaiCongTrinhCoSoVatChat { get; set; }

    public virtual ICollection<TbCongTrinhCoSoVatChat> TbCongTrinhCoSoVatChats { get; set; } = new List<TbCongTrinhCoSoVatChat>();
}
