using Jose;
using ShopWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ShopWeb
{
  public class MvcApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
    }
    protected void Application_OnPostAuthenticateRequest(object sender, EventArgs e)
    {
      HttpRequest httpRequest = HttpContext.Current.Request;
      string SecretKey = WebConfigurationManager.AppSettings["SecretKey"].ToString();
      string cookieName = WebConfigurationManager.AppSettings["CookieName"].ToString();
      if ((httpRequest.Cookies[cookieName] != null) && (httpRequest.Cookies[cookieName].Value.ToString() != ""))
      {
        JwtObject jwtObject = JWT.Decode<JwtObject>(Convert.ToString(httpRequest.Cookies[cookieName].Value), Encoding.UTF8.GetBytes(SecretKey), JwsAlgorithm.HS512);
        string[] roles = jwtObject.Role.Split(new char[] { ',' });
        Claim[] claims = new Claim[] {
        new Claim(ClaimTypes.Name,jwtObject.Account),
        new Claim(ClaimTypes.Role,jwtObject.Role)
        };
        var claimsIdentity = new ClaimsIdentity(claims, cookieName);
        claimsIdentity.AddClaim(new Claim(@"http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "My Identity", @"http://w3.org/2001/XMLSchema#string"));
        HttpContext.Current.User = new GenericPrincipal(claimsIdentity, roles);
        Thread.CurrentPrincipal = HttpContext.Current.User;
      }
    }
  }
}
