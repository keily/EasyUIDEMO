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
    public class ProductTypeBLL
    {

        public IList<ProductTypeInfo> GetProductTypesList()
        {
            using (MyContext db = new MyContext())
            {
               
                var items = db.ProductType.ToList();
                IList<ProductTypeInfo> ProductTypeInfos = new List<ProductTypeInfo>();
                foreach (var item in items)
                {
                    ProductTypeInfo info = new ProductTypeInfo();
                    info.ID = item.ID;
                    info.ProductTypeName = item.ProductTypeName;
                    info.Description = item.Description;

                    ProductTypeInfos.Add(info);
                }

                return ProductTypeInfos;
               
            }
        }


        public IList<ProductTypeInfo> GetProductTypesList(int pageIndex, int pageSize, out int Total)
        {
            using (MyContext db = new MyContext())
            {
                Total = (from c in db.ProductType
                         orderby c.ID
                         select c).Count();
                var items = (from c in db.ProductType
                             orderby c.ID
                             select c).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                IList<ProductTypeInfo> ProductTypeInfos = new List<ProductTypeInfo>();
                foreach (var item in items)
                {
                    ProductTypeInfo info = new ProductTypeInfo();
                    info.ID = item.ID;
                    info.ProductTypeName = item.ProductTypeName;
                    info.Description = item.Description;

                    ProductTypeInfos.Add(info);
                }

                return ProductTypeInfos;
            }
        }

        public IList<ProductTypeInfo> GetProductTypesBySerach(int pageIndex, int pageSize, out int Total, string name)
        {
            using (MyContext db = new MyContext())
            {
                Total = (from c in db.ProductType
                         where (!string.IsNullOrEmpty(name) ? c.ProductTypeName.Contains(name) : true)
                         orderby c.ID
                         select c).Count();
                var items = (from c in db.ProductType
                             where (!string.IsNullOrEmpty(name) ? c.ProductTypeName.Contains(name) : true)
                             orderby c.ID
                             select c).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
               
                IList<ProductTypeInfo> ProductTypeInfos = new List<ProductTypeInfo>();
                foreach (var item in items)
                {
                    ProductTypeInfo info = new ProductTypeInfo();
                    info.ID = item.ID;
                    info.ProductTypeName = item.ProductTypeName;
                    info.Description = item.Description;

                    ProductTypeInfos.Add(info);
                }

                return ProductTypeInfos;
            }
        }

        public bool GreateProductType(ProductType info)
        {
            using (MyContext db = new MyContext())
            {
                bool a = false;
                try
                {
                    db.ProductType.Add(info);
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

        public bool UpdateProductType(ProductType info)
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

        public int RemeProductType(string[] Id)
        {
            using (MyContext db = new MyContext()) 
            {
                int num = 0;
                try
                {
                    foreach (string typeid in Id)
                    {
                        int id = Convert.ToInt32(typeid);
                        var item = db.ProductType.Single(g => g.ID == id);
                        db.ProductType.Remove(item);
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
