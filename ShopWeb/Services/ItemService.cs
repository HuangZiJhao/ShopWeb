using ShopWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace ShopWeb.Services
{
  public class ItemService
  {
    private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;
    private readonly SqlConnection conn = new SqlConnection(cnstr);

    public Item GetItemById(int Id)
    {
      Item Data = new Item();
      string sql = $@"Select * from Item where Id = {Id} ";
      try
      {
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
          Data.Id = Convert.ToInt32(dr["Id"]);
          Data.Image = dr["Image"].ToString();
          Data.Name = dr["Name"].ToString();
          Data.Price = Convert.ToInt32(dr["Price"]);
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
    public List<int> GetIdList(ForPaging Paging)
    {
      SetMaxPaging(Paging);
      List<int> IdList = new List<int>();
      string sql = $@" select Id from (Select row_number() over(order by Id desc ) as sort,* from Item) m Where m.sort Between {(Paging.NowPage - 1) * Paging.ItemNum + 1} and {Paging.NowPage * Paging.ItemNum}";
      try
      {
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
          IdList.Add(Convert.ToInt32(dr["Id"]));
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
      return IdList;
    }
    public void SetMaxPaging(ForPaging Paging)
    {
      int Row = 0;
      string sql = $@"select * from Item";
      try
      {
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
          Row++;
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
      Paging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Row) / Paging.ItemNum));
      Paging.SetRightPage();
    }
    public void Insert(Item newData)
    {
      newData.Id = LastItemFinder();
      string sql = $@"INSERT INTO Item(Id,Name,Price,Image) Values('{newData.Id}','{newData.Name}','{newData.Price}','{newData.Image}')";
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
    public int LastItemFinder()
    {
      int Id;
      string sql = $@"select top 1 * from Item order by Id desc";
      try
      {
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
          Id = Convert.ToInt32(dr["Id"]);
        }
        else
        {
          Id = 0;
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
      return Id + 1;
    }
    public void DeleteItem(int Id)
    {
      StringBuilder sql = new StringBuilder();
      sql.AppendLine($@" Delete from CarBuy where Item_Id");
      sql.AppendLine($@" Delete from Item where Item_Id");
      try
      {
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql.ToString(), conn);
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