﻿using System;
using System.Collections.Generic;
using PhanHeHTQT.Models.DM;

namespace PhanHeHTQT.Models;

public partial class TbDienBienLuong
{
    public int IdDienBienLuong { get; set; }

    public int? IdCanBo { get; set; }

    public int? IdTrinhDoDaoTao { get; set; }

    public DateOnly? NgayThangNam { get; set; }

    public int? IdBacLuong { get; set; }

    public int? IdHeSoLuong { get; set; }

    public virtual DmBacLuong1? IdBacLuongNavigation { get; set; }

    public virtual TbCanBo? IdCanBoNavigation { get; set; }

    public virtual DmHeSoLuong? IdHeSoLuongNavigation { get; set; }

    public virtual DmTrinhDoDaoTao? IdTrinhDoDaoTaoNavigation { get; set; }
}
