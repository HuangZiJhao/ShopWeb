using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopWeb.Models
{
  public class Article
  {
    public int A_Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Account { get; set; }
    public DateTime CreatTime { get; set; }
    public int Watch { get; set; }
    public Member Member { get; set; } = new Member();

  }
}