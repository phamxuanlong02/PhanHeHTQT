using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PhanHeHTQT.Models.DM;

namespace PhanHeHTQT.Models;

public partial class TbThanhPhanThamGiaDoanCongTac
{
    [Display(Name = "Id thành phần tham gia")]

    public int IdThanhPhanThamGiaDoanCongTac { get; set; }
    [Display(Name = "Id đoàn công tác")]

    public int? IdDoanCongTac { get; set; }
    [Display(Name = "Id cán bộ")]

    public int? IdCanBo { get; set; }
    [Display(Name = "Vai trò tham gia")]

    public int? IdVaiTroThamGia { get; set; }
    [Display(Name = "Cán bộ")]

    public virtual TbCanBo? IdCanBoNavigation { get; set; }
    [Display(Name = "Đoàn công tác")]

    public virtual TbDoanCongTac? IdDoanCongTacNavigation { get; set; }
    [Display(Name = "Vai trò tham gia")]

    public virtual DmVaiTroThamGium? IdVaiTroThamGiaNavigation { get; set; }
}
