using ShopWeb.Services;
using ShopWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopWeb.Controllers
{
  public class ItemController : Controller
  {
    private readonly ItemService itemService = new ItemService();
    private readonly CartServiece cartServiece = new CartServiece();
    // GET: Item
    public ActionResult Index(int Page = 1)
    {
      ItemViewModel Data = new ItemViewModel();
      Data.Paging = new ForPaging();
      Data.IdList = itemService.GetIdList(Data.Paging);
      Data.ItemBlock = new List<ItemDetailViewModel>();
      foreach (var Id in Data.IdList)
      {
        ItemDetailViewModel newBlock = new ItemDetailViewModel();
        newBlock.Data = itemService.GetItemById(Id);
        string Cart = (HttpContext.Session["Cart"] != null) ? HttpContext.Session["Cart"].ToString() : null;
        newBlock.InCart = cartServiece.CheckInCart(Cart, Id);
        Data.ItemBlock.Add(newBlock);
      }
      return View(Data);
    }
    public ActionResult Item(int Id)
    {
      ItemDetailViewModel ViewData = new ItemDetailViewModel();
      ViewData.Data = itemService.GetItemById(Id);
      string Cart = (HttpContext.Session["Cart"] != null) ? HttpContext.Session["Cart"].ToString() : null;
      ViewData.InCart = cartServiece.CheckInCart(Cart, Id);
      return View(ViewData);
    }
    public ActionResult ItemBlock(int Id)
    {
      ItemDetailViewModel ViewData = new ItemDetailViewModel();
      ViewData.Data = itemService.GetItemById(Id);
      string Cart = (HttpContext.Session["Cart"] != null) ? HttpContext.Session["Cart"].ToString() : null;
      ViewData.InCart = cartServiece.CheckInCart(Cart, Id);
      return View(ViewData);
    }
    [Authorize(Roles = "Admin")]
    public ActionResult Create()
    {
      return View();
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public ActionResult Create(ItemCreateViewModel Data)
    {
      if (Data.ItemImage != null)
      {
        string filename = Path.GetFileName(Data.ItemImage.FileName);
        string Url = Path.Combine(Server.MapPath("~/Upload/"), filename);
        Data.ItemImage.SaveAs(Url);
        Data.NewData.Image = filename;
        itemService.Insert(Data.NewData);
        return RedirectToAction("Index", "Item");
      }
      else
      {
        ModelState.AddModelError("ItemImage", "請選檔案");
        return View(Data);
      }
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public ActionResult Delete(int Id)
    {
      itemService.DeleteItem(Id);
      return RedirectToAction("Index", "Item");

    }
  }
}