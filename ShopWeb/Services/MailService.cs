using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ShopWeb.Services
{
  public class MailService
  {
    private string GMAIL_ACCOUNT = "leo12160201@gmail.com";
    private string GMAIL_PASSWORD = "*****";
    private string Gmail_mail = "leo12160201@gmail.com";

    public string GetValidateCode()
    {
      string[] CodeArray = { "A", "B", "C", "1", "2", "3", "a", "b", "c" };
      string ValidateCode = string.Empty;
      Random rd = new Random();
      for (int i = 0; i < 10; i++)
      {
        ValidateCode += CodeArray[rd.Next(CodeArray.Count())];
      }
      return ValidateCode;
    }
    public string GetRegisterMailBody(string TempString, string UserName, string ValidateUrl)
    {
      TempString = TempString.Replace("{{UserName}}", UserName);
      TempString = TempString.Replace("{{ValidateUrl}}", ValidateUrl);
      return TempString;
    }
    public void SendRegisterMail(string MailBody, string ToEmail) {
      SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
      SmtpServer.Port = 587;
      SmtpServer.Credentials = new System.Net.NetworkCredential(GMAIL_ACCOUNT, GMAIL_PASSWORD);
      SmtpServer.EnableSsl = true;
      MailMessage mail = new MailMessage();
      mail.From = new MailAddress(Gmail_mail);
      mail.To.Add(ToEmail);
      mail.Subject = "會員註冊";
      mail.Body = MailBody;
      mail.IsBodyHtml = true;
      SmtpServer.Send(mail);


    }








  }
}