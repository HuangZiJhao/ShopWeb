using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ShopWeb.Models;

namespace ShopWeb.ViewModels
{
  public class MemberRegisterViewModel
  {
    [DisplayName("照片")]
    public HttpPostedFileBase MembersImage { get; set; }
    public Member newMember { get; set; }
    [DisplayName("密碼")]
    [Required(ErrorMessage = "input 密碼")]
    public string Password { get; set; }
    [DisplayName("確認密碼")]
    [Compare("Password",ErrorMessage ="密碼不一致")]
    [Required(ErrorMessage = "input 確認密碼")]
    public string PasswordCheck { get; set; }
  }
}