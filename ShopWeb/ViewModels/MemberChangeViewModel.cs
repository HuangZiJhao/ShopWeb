﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopWeb.ViewModels
{
  public class MemberChangeViewModel
  {
    [DisplayName("舊密碼")]
    [Required(ErrorMessage ="輸入密碼")]
    public string Password { get; set; }
    [DisplayName("新密碼")]
    [Required(ErrorMessage = "輸入新密碼")]
    public string NewPassword { get; set; }
    [DisplayName("新密碼確認")]
    [Required(ErrorMessage = "輸入確認密碼")]
    public string NewPasswordCheck { get; set; }
  }
}