using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopWeb.Models
{
  public class Item
  {
    [DisplayName("商品編號")]
    public int Id { get; set; }
    [DisplayName("商品名稱")]
    [Required(ErrorMessage ="必須輸入")]
    public string Name { get; set; }
    [DisplayName("商品價格")]
    [Required(ErrorMessage = "必須輸入")]
    [Range(typeof(int),"1","999",ErrorMessage ="1~999")]
    public int Price { get; set; }
    [DisplayName("圖片")]
    public string Image { get; set; }
  }
}