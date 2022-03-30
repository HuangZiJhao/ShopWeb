using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShopWeb.ViewModels
{
  public class MemberLoginViewModel
  {
    [DisplayName("帳號")]
    [Required(ErrorMessage ="必須輸入帳號")]
    public string Account { get; set; }
    [DisplayName("密碼")]
    [Required(ErrorMessage = "必須輸入密碼")]
    public string Password { get; set; }
  }
}