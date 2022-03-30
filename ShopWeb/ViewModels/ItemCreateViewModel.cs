using ShopWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopWeb.ViewModels
{
  public class ItemCreateViewModel
  {
    [DisplayName("圖片")]
    //[FileExtensions(ErrorMessage ="不是圖片")]
    public HttpPostedFileBase ItemImage { get; set; }
    public Item NewData { get; set; }
  }
}