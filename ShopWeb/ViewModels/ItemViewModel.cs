using ShopWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ShopWeb.ViewModels
{
  public class ItemViewModel
  {
    public List<int> IdList { get; set; }
    public List<ItemDetailViewModel> ItemBlock { get; set; }
    public ForPaging Paging { get; set; }
  }
}