using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using ShopWeb.Models;
using ShopWeb.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace ShopWeb.Services
{
  public class MemberDBService
  {
    private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
    private readonly SqlConnection conn = new SqlConnection(cnstr);

    public void Register(Member newMember)
    {
      newMember.Password = Hash(newMember.Password);
      string sql = $@" INSERT INTO Members (Account,Password,Name,Image,Email,AuthCode,IsAdmin) VALUES ('{newMember.Account}','{newMember.Password}','{newMember.Name}','{newMember.Image}','{newMember.Email}','{newMember.AuthCode}','0')";

      try
      {
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader dr = cmd.ExecuteReader();
      }
      catch (Exception e)
      {
        throw new Exception(e.ToString());
      }
      finally
      {
        conn.Close();
      }

    }
    public string Hash(string Password)
    {

      string saltkey = "1q2w3e4r5t6ysassa";
      string saltAndPassword = string.Concat(Password, saltkey);

      SHA256CryptoServiceProvider sha256Hasher = new SHA256CryptoServiceProvider();

      byte[] PasswordData = Encoding.Default.GetBytes(saltAndPassword);
      byte[] HashData = sha256Hasher.ComputeHash(PasswordData);
      string Hashresult = Convert.ToBase64String(HashData);

      return Hashresult;
    }
    private Member GetMemberByAccount(string Account)
    {
      Member Data = new Member();
      string sql = $@"SELECT * FROM Members WHERE Account='{Account}'; ";

      try
      {
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
          Data.Account = dr["Account"].ToString();
          Data.Password = dr["Password"].ToString();
          Data.Name = dr["Name"].ToString();
          Data.Email = dr["Email"].ToString();
          Data.AuthCode = dr["AuthCode"].ToString();
          Data.IsAdmin = Convert.ToBoolean(dr["IsAdmin"]);

        }
        else
        {

          Data = null;
        }

      }
      catch (Exception e)
      {
        throw new Exception(e.ToString());
      }
      finally
      {
        conn.Close();
      }

      return Data;
    }
    public bool AccountCheck(string Account)
    {
      Member Data = GetMemberByAccount(Account);
      return Data == null;
    }
    public Member GetDataByAccount(string Account)
    {
      Member Data = new Member();
      string sql = $@"SELECT * FROM Members WHERE Account='{Account}'; ";

      try
      {
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
          Data.Image = dr["Image"].ToString();
          Data.Account = dr["Account"].ToString();
          Data.Name = dr["Name"].ToString();
          Data.IsAdmin = Convert.ToBoolean(dr["IsAdmin"]);
        }
        else
        {

          Data = null;
        }

      }
      catch (Exception e)
      {
        throw new Exception(e.ToString());
      }
      finally
      {
        conn.Close();
      }
      return Data;
    }
    public string EmailValidate(string Account, string AuthCode)
    {
      Member ValidateMember = GetMemberByAccount(Account);
      string ValidateStr = string.Empty;
      if (ValidateMember != null)
      {
        if (ValidateMember.AuthCode == AuthCode)
        {
          string sql = $@"UPDATE Members set AuthCode ='{string.Empty}' WHERE Account ='{Account}'";
          try
          {
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader dr = cmd.ExecuteReader();
          }
          catch (Exception e)
          {
            throw new Exception(e.ToString());
          }
          finally
          {
            conn.Close();
          }
          ValidateStr = "成功";
        }
        else
        {
          ValidateStr = "失敗";
        }

      }
      else
      {
        ValidateStr = "請重新輸入";
      }

      return ValidateStr;
    }
    public string LoginCheck(string Account, string Password)
    {
      Member LoginMember = GetMemberByAccount(Account);
      if (LoginMember != null)
      {
        if (string.IsNullOrWhiteSpace(LoginMember.AuthCode))
        {
          if (PasswordCheck(LoginMember, Password))
          {
            return "";
          }
          else
          {
            return "密碼錯誤";
          }
        }
        else
        {
          return "信箱未驗證";
        }
      }
      return "無此會員去註冊";
    }
    public bool PasswordCheck(Member CheckMember, string Password)
    {
      bool Resault = CheckMember.Password.Equals(Hash(Password));
      return Resault;
    }
    public string ChangePassword(string Account, string Password, string NewPassword)
    {
      Member ChangeMember = GetMemberByAccount(Account);
      if (PasswordCheck(ChangeMember, Password))
      {
        ChangeMember.Password = Hash(NewPassword);
        string sql = $@"UPDATE Members SET Password = '{ChangeMember.Password}' WHERE Account = '{Account}'";
        try
        {
          conn.Open();
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataReader dr = cmd.ExecuteReader();
        }
        catch (Exception e)
        {
          throw new Exception(e.ToString());
        }
        finally
        {
          conn.Close();
        }
        return "密碼修改成功";
      }
      else
      {

        return "密碼錯誤";
      }
    }
    public bool CheckImage(string ContentType)
    {
      switch (ContentType)
      {
        case "image/jpg":
        case "image/jpeg":
        case "image/png":
          return true;
      }
      return false;
    }
    public string GetRole(string Account)
    {
      string Role = "User";
      Member LoginMember = GetDataByAccount(Account);
      if (LoginMember.IsAdmin)
      {
        Role += ",Admin";
      }
      return Role;
    }




  }
}