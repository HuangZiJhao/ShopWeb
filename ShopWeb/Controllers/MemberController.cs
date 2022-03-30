using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ShopWeb.Services;
using ShopWeb.ViewModels;
using ShopWeb.Security;

namespace ShopWeb.Controllers
{

  public class MemberController : Controller
  {
    private readonly MemberDBService memberService = new MemberDBService();
    private readonly MailService mailService = new MailService();
    private readonly CartServiece cartServiece = new CartServiece();

    // GET: Member
    public ActionResult Index()
    {
      return View();
    }
    public ActionResult Login()
    {
      if (User.Identity.IsAuthenticated)
        return RedirectToAction("Index", "Item");
      return View();
    }
    [HttpPost]
    public ActionResult Login(MemberLoginViewModel LoginMember)
    {
      string ValidateStr = memberService.LoginCheck(LoginMember.Account, LoginMember.Password);
      if (string.IsNullOrEmpty(ValidateStr))
      {
        HttpContext.Session.Clear();
        string Cart = cartServiece.GetCartSave(LoginMember.Account);
        if (Cart != null)
        {
          HttpContext.Session["Cart"] = Cart;
        }

        string RoleData = memberService.GetRole(LoginMember.Account);
        JwtService jwtService = new JwtService();
        string cookieName = WebConfigurationManager.AppSettings["CookieName"].ToString();
        string Token = jwtService.GenerateToken(LoginMember.Account, RoleData);
        HttpCookie cookie = new HttpCookie(cookieName);
        cookie.Value = Server.UrlEncode(Token);
        Response.Cookies.Remove(cookieName);
        Response.Cookies.Add(cookie);
        Response.Cookies[cookieName].Expires = DateTime.Now.AddMinutes(Convert.ToInt32(WebConfigurationManager.AppSettings["ExpireMinutes"]));
        return RedirectToAction("Index", "Item");
      }
      else
      {
        ModelState.AddModelError("", ValidateStr);
        return View(LoginMember);
      }
    }
    public ActionResult Register()
    {
      if (User.Identity.IsAuthenticated)
        return RedirectToAction("Index", "Item");
      return View();
    }
    [HttpPost]
    public ActionResult Register(MemberRegisterViewModel RegisterMember)
    {
      if (ModelState.IsValid)
      {
        if (RegisterMember.MembersImage != null)
        {
          if (memberService.CheckImage(RegisterMember.MembersImage.ContentType))
          {
            string fileName = Path.GetFileName(RegisterMember.MembersImage.FileName);
            string url = Path.Combine(Server.MapPath($@"~/Upload/Members/"), fileName);
            RegisterMember.MembersImage.SaveAs(url);
            RegisterMember.newMember.Password = RegisterMember.Password;
            string AuthCode = mailService.GetValidateCode();
            RegisterMember.newMember.AuthCode = string.Empty;//AuthCode;
            memberService.Register(RegisterMember.newMember);
            string TempMail = System.IO.File.ReadAllText(Server.MapPath("~/Views/Shared/RegisterEmailTemplate.html"));
            UriBuilder ValidateUrl = new UriBuilder(Request.Url)
            {
              Path = Url.Action("EmailValidate", "Members", new { Account = RegisterMember.newMember.Account, AuthCode = AuthCode })
            };
            string MailBody = mailService.GetRegisterMailBody(TempMail, RegisterMember.newMember.Name, ValidateUrl.ToString().Replace("%3F", "?"));
            //mailService.SendRegisterMail(MailBody, RegisterMember.newMember.Email);
            TempData["RegisterState"] = "成功";
            return RedirectToAction("RegisterResult");
          }
          else
          {
            ModelState.AddModelError("MembersImage", "檔案格式不對");
          }


        }
        else
        {
          ModelState.AddModelError("MembersImage", "必須上傳圖片");
          return View(RegisterMember);
        }
      }
      RegisterMember.Password = null;
      RegisterMember.PasswordCheck = null;
      return View(RegisterMember);
    }
    public ActionResult RegisterResult()
    {
      return View();
    }
    public JsonResult AccountCheck(MemberRegisterViewModel RegisterMember)
    {
      return Json(memberService.AccountCheck(RegisterMember.newMember.Account), JsonRequestBehavior.AllowGet);
    }
    public ActionResult EmailValidate(string Account, string AuthCode)
    {
      ViewData["EmailValidate"] = memberService.EmailValidate(Account, AuthCode);
      return View();
    }
    [Authorize]
    public ActionResult ChangePassword()
    {
      return View();
    }

    [HttpPost]
    public ActionResult ChangePassWord(MemberChangeViewModel ChangeMember)
    {
      if (ModelState.IsValid)
      {
        ViewData["ChangeState"] = memberService.ChangePassword(User.Identity.Name, ChangeMember.NewPassword, ChangeMember.NewPassword);

      }
      return View();
    }
    [Authorize]
    public ActionResult Logout()
    {
      string cookieName = WebConfigurationManager.AppSettings["CookieName"].ToString();
      HttpCookie cookie = new HttpCookie(cookieName);
      cookie.Expires = DateTime.Now.AddDays(-1);
      cookie.Values.Clear();
      Response.Cookies.Set(cookie);
      return RedirectToAction("Login");

    }

  }
}