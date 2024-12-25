using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PhanHeHTQT.Models.DM;

namespace PhanHeHTQT.Models;

public partial class TbLuuHocSinhSinhVienNn
{
    [Display(Name = "Id lưu học sinh sinh viên")]

    public int IdLuuHocSinhSinhVienNn { get; set; }
    [Display(Name = "Id người học")]

    public int? IdNguoiHoc { get; set; }
    [Display(Name = "Nguồn kinh phí")]

    public int? IdNguonKinhPhiLuuHocSinh { get; set; }
    [Display(Name = "Loại lưu học sinh")]

    public int? IdLoaiLuuHocSinh { get; set; }
    [Display(Name = "Loại lưu học sinh")]

    public virtual DmLoaiLuuHocSinh? IdLoaiLuuHocSinhNavigation { get; set; }
   
    [Display(Name = "Nguồn kinh phí")]
    public virtual DmNguonKinhPhiChoLuuHocSinh? IdNguonKinhPhiLuuHocSinhNavigation { get; set; }
}
