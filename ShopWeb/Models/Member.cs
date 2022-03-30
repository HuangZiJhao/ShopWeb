using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopWeb.Models
{
  public class Member
  {
    [DisplayName("帳號")]
    [Required(ErrorMessage = "Input Account")]
    [StringLength(30, MinimumLength = 6, ErrorMessage = "6~30")]
    [Remote("AccountCheck", "Member", ErrorMessage = "已被用過")]
    public string Account { get; set; }
    public string Password { get; set; }
    [DisplayName("姓名")]
    [Required(ErrorMessage = "Input Name")]
    [StringLength(20, ErrorMessage = "MAX 20")]
    public string Name { get; set; }
    [DisplayName("照片")]
    public string Image { get; set; }
    [DisplayName("信箱")]
    [Required(ErrorMessage = "Input Email")]
    [StringLength(50, ErrorMessage = "MAX 50")]
    [EmailAddress(ErrorMessage ="請用Email格式")]
    public string Email { get; set; }
    public string AuthCode { get; set; }
    public bool IsAdmin { get; set; }
  }
}