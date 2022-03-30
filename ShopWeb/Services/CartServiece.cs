using ShopWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ShopWeb.Services
{
  public class CartServiece
  {
    private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
    private readonly SqlConnection conn = new SqlConnection(cnstr);

    public List<CartBuy> GetItemFromCart(string Cart)
    {
      List<CartBuy> DataList = new List<CartBuy>();
      string sql = $@"select * from CartBuy m inner join Item d on m.Item_Id = d.Id where Cart_Id = '{Cart}' ";
      try
      {
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
          CartBuy Data = new CartBuy();
          Data.Cart_Id = dr["Cart_Id"].ToString();
          Data.Item_Id = Convert.ToInt32(dr["Item_Id"]);
          Data.Item.Id = Convert.ToInt32(dr["Id"]);
          Data.Item.Image = dr["Image"].ToString();
          Data.Item.Name = dr["Name"].ToString();
          Data.Item.Price = Convert.ToInt32(dr["Price"]);
          DataList.Add(Data);
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
      return DataList;

    }

    public bool CheckInCart(string Cart, int Item_Id)
    {
      CartBuy Data = new CartBuy();
      string sql = $@"Select * from CartBuy m inner Join Item d on m.Item_Id = d.Id where Cart_Id = '{Cart}' and Item_Id = {Item_Id} ";
      try
      {
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
          Data.Cart_Id = dr["Cart_Id"].ToString();
          Data.Item_Id = Convert.ToInt32(dr["Item_Id"]);
          Data.Item.Id = Convert.ToInt32(dr["Id"]);
          Data.Item.Image = dr["Image"].ToString();
          Data.Item.Name = dr["Name"].ToString();
          Data.Item.Price = Convert.ToInt32(dr["Price"]);
        }
        else
          Data = null;
      }
      catch (Exception e)
      {
        throw new Exception(e.ToString());
      }
      finally
      {
        conn.Close();
      }
      return (Data != null);
    }
    public void AddtoCart(string Cart, int Item_Id)
    {
      string sql = $@" INSERT INTO CartBuy(Cart_Id,Item_Id) VALUES ('{Cart}','{Item_Id}')";
      try
      {
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.ExecuteNonQuery();
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
    public void RemoveForCart(string Cart, int Item_Id)
    {
      string sql = $@"Delete from CartBuy where Cart_Id = '{Cart}' and Item_Id = {Item_Id}";
      try
      {
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.ExecuteNonQuery();
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
    public bool CheckCartSave(string Account, string Cart)
    {
      CartSave Data = new CartSave();
      string sql = $@"Select * from CartSave m inner Join Members d on m.Account = d.Account where m.Account = '{Account}' and Cart_Id = '{Cart}'";
      try
      {
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
          Data.Account = dr["Account"].ToString();
          Data.Cart_Id = dr["Car_Id"].ToString();
          Data.Member.Name = dr["Name"].ToString();
        }
        else
          Data = null;
      }
      catch (Exception e)
      {
        throw new Exception(e.ToString());
      }
      finally
      {
        conn.Close();
      }
      return (Data != null);

    }
    public string GetCartSave(string Account)
    {
      CartSave Data = new CartSave();
      string sql = $@"Select * from CartSave m inner Join Members d on m.Account = d.Account where m.Account = '{Account}'";
      try
      {
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
          Data.Account = dr["Account"].ToString();
          Data.Cart_Id = dr["Car_Id"].ToString();
          Data.Member.Name = dr["Name"].ToString();
        }
        else
          Data = null;
      }
      catch (Exception e)
      {
        throw new Exception(e.ToString());
      }
      finally
      {
        conn.Close();
      }

      if (Data != null)
      {
        return Data.Cart_Id;
      }
      else
      {
        return null;
      }
    }
    public void SaveCart(string Account, string Cart)
    {
      string sql = $@"INSERT INTO CartSave(Account,Car_Id) Values('{Account}','{Cart}')";
      try
      {
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.ExecuteNonQuery();
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
    public void SaveCartRemove(string Account)
    {
      string sql = $@"Delete from CartSave where Account = '{Account}'";
      try
      {
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.ExecuteNonQuery();
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

  }
}