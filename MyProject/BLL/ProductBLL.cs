using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using Model;
using Models;

using System.Data.Entity;//必须引用
using System.Data;//EntityState.Modified;   这个准备  一定添加命名空间System.Data.Entity和ef4.1
namespace BLL
{
    public class ProductBLL
    {
        public IList<ProductInfo> GetProductList(int pageIndex, int pageSize, out int Total)
        {
            using (MyContext db = new MyContext())
            {
                Total = (from c in db.Product
                         orderby c.ID
                         select c).Count();
                var items = (from c in db.Product
                             orderby c.ID
                             select c).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                IList<ProductInfo> ProductInfos = new List<ProductInfo>();
                foreach (var item in items)
                {
                    ProductInfo info = new ProductInfo();
                    info.ID = item.ID;
                    info.ProductTypeID = item.ProductTypeID;
                    info.ProductTypeName = item.ProductType.ProductTypeName;//导航属性的特点
                    info.Image = item.Image;
                    info.ProductName = item.ProductName;
                    info.MarketPrice = item.MarketPrice;
                    info.NewPrice = item.NewPrice;
                    info.GetDate = item.GetDate.ToShortDateString();
                    info.Enable = item.Enable;
                    ProductInfos.Add(info);
                }

                return ProductInfos;
            }
        }

        public IList<ProductInfo> GetProductsBySerach(int pageIndex, int pageSize, out int Total, int typeId, int id, string name, int NewPrice, bool? b)
        {
            using (MyContext db = new MyContext())
            {
                Total = (from c in db.Product
                         where ((id != 0) ? c.ID == id : true) && ((typeId != 0) ? c.ProductTypeID == typeId : true) &&
                             (!string.IsNullOrEmpty(name) ? c.ProductName.Contains(name) : true) &&
                             ((NewPrice != 0) ? c.NewPrice == NewPrice : true) &&                             
                             ((b == null) ? true : c.Enable == b)//传值为NULL就是全部
                         orderby c.ProductTypeID
                         select c).Count();
                var items = (from c in db.Product
                             where ((id != 0) ? c.ID == id : true) && ((typeId != 0) ? c.ProductTypeID == typeId : true) &&
                            (!string.IsNullOrEmpty(name) ? c.ProductName.Contains(name) : true) &&                           
                            ((NewPrice != 0) ? c.NewPrice == NewPrice : true) &&
                            ((b == null) ? true : c.Enable == b)//传值为NULL就是全部
                             orderby c.ProductTypeID//必须在分页前排序
                             select c).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                List<ProductInfo> ProductInfos = new List<ProductInfo>();
                foreach (var item in items)
                {
                    ProductInfo info = new ProductInfo();
                    info.ID = item.ID;
                    info.ProductTypeID = item.ProductTypeID;
                    info.ProductTypeName = item.ProductType.ProductTypeName;//导航属性的特点
                    info.Image = item.Image;
                    info.ProductName = item.ProductName;
                    info.MarketPrice = item.MarketPrice;
                    info.NewPrice = item.NewPrice;
                    info.GetDate = item.GetDate.ToShortDateString();
                    info.Enable = item.Enable;
                    ProductInfos.Add(info);
                }

                return ProductInfos;
            }
        }

        public bool GreateProduct(Product info)
        {
            using (MyContext db = new MyContext())
            {
                bool a = false;
                try
                {
                    db.Product.Add(info);
                    int b = db.SaveChanges();
                    if (b == 1)
                    { a = true; }
                    return a;
                }

                catch (Exception ex)
                {

                    return a;
                }
            }
        }

        public bool UpdateProduct(Product info)
        {
            using (MyContext db = new MyContext())
            {
                bool a = false;
                try
                {
                    db.Entry(info).State = EntityState.Modified;  //
                    int b = db.SaveChanges();
                    if (b == 1)
                    { a = true; }
                    return a;
                }
                catch (Exception ex)
                {

                    return a;
                }
            }
        }

        public int RemeProduct(string[] Ids)
        {
            using (MyContext db = new MyContext())
            {
                int num = 0;
                try
                {
                    foreach (string productid in Ids)
                    {
                        int id = Convert.ToInt32(productid);
                        var item = db.Product.Single(g => g.ID == id);
                        db.Product.Remove(item);
                        num = db.SaveChanges();
                    }
                    return num;
                }
                catch (Exception ex)
                {

                    return num;
                }

            }
        }
    }
}
