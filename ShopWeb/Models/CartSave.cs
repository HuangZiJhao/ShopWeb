using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopWeb.Models
{
  public class CartSave
  {
    public string Account { get; set; }
    public string Cart_Id { get; set; }
    public Member Member { get; set; } = new Member();
  }
}