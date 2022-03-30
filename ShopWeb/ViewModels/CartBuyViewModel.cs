using ShopWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopWeb.ViewModels
{
  public class CartBuyViewModel
  {
    public List<CartBuy> DataList { get; set; }
    public bool isCartsave { get; set; }
  }
}