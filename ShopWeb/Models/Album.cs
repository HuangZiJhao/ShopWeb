using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopWeb.Models
{
  public class Album
  {
    public int Alb_Id { get; set; }
    public string FileName { get; set; }
    public string Url { get; set; }
    public int Size { get; set; }
    public string Type { get; set; }
    public string Account { get; set; }
    public DateTime CreateTime { get; set; }

    public Member Member { get; set; } = new Member();


  }
}