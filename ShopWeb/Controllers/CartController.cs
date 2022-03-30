using ShopWeb.Services;
using ShopWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopWeb.Controllers
{
  public class CartController : Controller
  {
    private readonly CartServiece cartServiece = new CartServiece();
    // GET: Cart
    [Authorize]
    public ActionResult Index()
    {
      CartBuyViewModel Data = new CartBuyViewModel();
      string Cart = (HttpContext.Session["Cart"] != null) ? HttpContext.Session["Cart"].ToString() : null;
      Data.DataList = cartServiece.GetItemFromCart(Cart);
      return View(Data);
    }
    [Authorize]
    public ActionResult CartSave()
    {
      string Cart;
      if (HttpContext.Session["Cart"] != null)
      {
        Cart = HttpContext.Session["Cart"].ToString();
      }
      else
      {

        Cart = DateTime.Now.ToString() + User.Identity.Name;
        HttpContext.Session["Cart"] = Cart;

      }
      cartServiece.SaveCart(User.Identity.Name, Cart);
      return RedirectToAction("Index");
    }
    [Authorize]
    public ActionResult CartSaveRemove()
    {
      cartServiece.SaveCartRemove(User.Identity.Name);
      return RedirectToAction("Index");
    }
    [Authorize]
    public ActionResult Pop(int Id, string toPage)
    {
      string Cart = (HttpContext.Session["Cart"] != null) ? HttpContext.Session["Cart"].ToString() : null;
      cartServiece.RemoveForCart(Cart, Id);
      if (toPage == "Item")
        return RedirectToAction("Item", "Item", new { Id = Id });
      else if (toPage == "ItemBlock")
        return RedirectToAction("ItemBlock", "Item", new { Id = Id });
      else
        return RedirectToAction("Index");
    }
    [Authorize]
    public ActionResult Put(int Id, string toPage)
    {
      if (HttpContext.Session["Cart"] == null)
        HttpContext.Session["Cart"] = DateTime.Now.ToString() + User.Identity.Name;
      cartServiece.AddtoCart(HttpContext.Session["Cart"].ToString(), Id);
      if (toPage == "Item")
        return RedirectToAction("Item", "Item", new { Id = Id });
      else if (toPage == "ItemBlock")
        return RedirectToAction("ItemBlock", "Item", new { Id = Id });
      else
        return RedirectToAction("Index");

    }
  }
}