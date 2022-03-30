using ShopWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopWeb.ViewModels
{
  public class ItemDetailViewModel
  {
    public Item Data { get; set; }
    public bool InCart { get; set; }
  }
}