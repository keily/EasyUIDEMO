using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Model;
using Models;
using System.Web.Script.Serialization;//对象格式化json来自System.Web.Extentions的DLL中
using Newtonsoft.Json;
namespace WebSites.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        ProductBLL productBLL;
        ProductTypeBLL productTypeBLL;
        public AdminController()
        {
            productTypeBLL = new ProductTypeBLL();
            productBLL = new ProductBLL();
            
        }

        /// ProductType的commbox列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTypesList()
        {
            IList<ProductTypeInfo> ProductTypes = productTypeBLL.GetProductTypesList();
            //System.Web.Extentions的DLL中
            JavaScriptSerializer json = new JavaScriptSerializer();
            string Str = json.Serialize(ProductTypes);//   方法一
            return Content(Str, "text/html;charset=UTF-8");
        }
        /// <summary>
        ///  处理前台传递的Enable变量的的true false
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static bool? Enable(string en)
        {
            if (en == "1" || en == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Product()
        {
            return View();
        }
      
        public ActionResult ProductType()
        {
            return View();
        }

        #region 产品类型管理
        /// <summary>
        /// 加载json数组对象
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadProductTypejson()
        {
            int row = int.Parse(Request["rows"].ToString());
            int pageindex = int.Parse(Request["page"].ToString());
            ProductTypeData ProductTypejson = new ProductTypeData();
            int total;
            ProductTypejson.rows = productTypeBLL.GetProductTypesList(pageindex, row, out total);
            ProductTypejson.total = total;
            JavaScriptSerializer json = new JavaScriptSerializer();
            string Str = json.Serialize(ProductTypejson);//   
            return Content(Str, "text/html;charset=UTF-8");
           
        }

        /// <summary>
        /// 搜索产品类型
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public ActionResult SeachProductTypeInfo(FormCollection collection)
        {
            String Name = collection.Get("Name").Trim().ToString();//上下两种方法都可以获取数据

            int row = int.Parse(Request["rows"].ToString());
            int pageindex = int.Parse(Request["page"].ToString());
            ProductTypeData ProductTypejson = new ProductTypeData();
            int total;
            ProductTypejson.rows = productTypeBLL.GetProductTypesBySerach(pageindex, row, out total, Name);
            ProductTypejson.total = total;
            JavaScriptSerializer json = new JavaScriptSerializer();
            string Str = json.Serialize(ProductTypejson);//   
            return Content(Str, "text/html;charset=UTF-8");
        }

        /// <summary>
        /// 增加产品类型
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public ActionResult CreateProductType(FormCollection collection)
        {
            //int ProductTypeId = Convert.ToInt32(collection.Get("Id"));
            String ProductTypeName = collection.Get("ProductTypeName");//是控件的Name属性而不是id属性
            String Description = collection.Get("Description");
            ProductType info = new ProductType();
            info.ProductTypeName = ProductTypeName;
            info.Description = Description;
           
            Message msg;
            if (productTypeBLL.GreateProductType(info))
            {
                msg = new Message(true, "添加" + ProductTypeName + "信息成功！");
            }
            else
            {
                msg = new Message(false, "添加产品类型失败，操作有误");
            }
            JavaScriptSerializer json = new JavaScriptSerializer();
            string Str = json.Serialize(msg);//   
            return Content(Str, "text/html;charset=UTF-8");
        }

        /// <summary>
        ///  删除产品类型
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult RemeProductTypes(string ids)
        {
            String[] id = ids.Split(',');
            Message msg;
            if (ids != null)
            {
                
                int i = productTypeBLL.RemeProductType(id);

                if (i > 0)
                {
                    msg = new Message(true, "删除成功");
                }
                else
                {
                    msg = new Message(false, "删除失败");
                }
            }
            else
            {
                msg = new Message(false, "传值失败，请告诉开发者解决");
            }
            JavaScriptSerializer json = new JavaScriptSerializer();
            return Content(json.Serialize(msg), "text/html;charset=UTF-8");
        }

        /// <summary>
        /// 更新游戏
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public ActionResult UpdateProductType(int id, FormCollection collection)
        {
             String Description =collection.Get("Description");
             String Name = collection.Get("ProductTypeName");
            ProductType info = new ProductType();
            info.ID = id;
            info.ProductTypeName = Name;
            info.Description = Description;           
            Message msg;
            if (productTypeBLL.UpdateProductType(info))
            {
                msg = new Message(true, "修改成功！");
            }
            else
            {
                msg = new Message(false, "修改失败！");
            }
            JavaScriptSerializer json = new JavaScriptSerializer();
            return Content(json.Serialize(msg), "text/html;charset=UTF-8");
        }
        
        #endregion


        #region 产品管理

        /// <summary>
        /// 加载json数组对象
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadProductjson()
        {
            //string sort = Request["sort"].ToString();暂时实现不了动态的字段倒/正排序 除非是sql语句 用Switch显得很臃肿
            //sort = (!string.IsNullOrEmpty(sort) ? sort : "ProductId");
            //string order = Request["order"].ToString();
            //order = (!string.IsNullOrEmpty(order) ? order : "ascending");
            int row = int.Parse(Request["rows"].ToString());
            int pageindex = int.Parse(Request["page"].ToString());
            ProductData Productjson = new ProductData();
            int total;
            Productjson.rows = productBLL.GetProductList(pageindex, row, out total);
            Productjson.total = total;
            string Str = JsonConvert.SerializeObject(Productjson);
            return Content(Str, "text/html;charset=UTF-8");
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public ActionResult SeachProductInfo(FormCollection collection)
        {
            String Name = collection.Get("ProductName").Trim().ToString();
            //严格判断空值
            String Id = collection.Get("typeId").ToString();
            int typeId = Convert.ToInt32(String.IsNullOrEmpty(Id) ? "0" : Id); //$("#typeId").combobox("getValue"); 通过这个获取的
                       
            String sid = collection.Get("ProductId").ToString();
            int ProductId = Convert.ToInt32(String.IsNullOrEmpty(sid) ? "0" : sid);

            String NewPrice = collection.Get("RealPrice").ToString();
            int RealPrice = Convert.ToInt32(String.IsNullOrEmpty(NewPrice) ? "0" : NewPrice);

            String en = collection.Get("en").ToString();
            bool? enable = String.IsNullOrEmpty(en) ? null : AdminController.Enable(en);
            //上下两种方法都可以获取数据Request[""]和collection.Get("")

            int row = int.Parse(Request["rows"].ToString());
            int pageindex = int.Parse(Request["page"].ToString());
            ProductData Productjson = new ProductData();
            int total;
            Productjson.rows = productBLL.GetProductsBySerach(pageindex, row, out total, typeId, ProductId, Name, RealPrice, enable);
            Productjson.total = total;
            string Str = JsonConvert.SerializeObject(Productjson);
            return Content(Str, "text/html;charset=UTF-8");
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public ActionResult CreateProduct(FormCollection collection)
        {

            Product info = new Product();
            info.ProductTypeID = Convert.ToInt32(collection.Get("ProductTypeID"));          
            string name=(!string.IsNullOrEmpty(collection.Get("ProductName"))? collection.Get("ProductName") : null);
            info.ProductName = name;
            info.Image = collection.Get("Image");
            info.MarketPrice = Convert.ToInt32(collection.Get("MarketPrice"));
            info.NewPrice = Convert.ToInt32(collection.Get("NewPrice"));          
            string en = collection.Get("Enable").ToString();
            info.Enable = AdminController.Enable(en);
            info.GetDate = DateTime.Now;
            Message msg;
            if (productBLL.GreateProduct(info))
            {
                msg = new Message(true, "添加"+name+"信息成功！");
            }
            else
            {
                msg = new Message(false, "失败,操作有误");
            }

            return Content(JsonConvert.SerializeObject(msg), "text/html;charset=UTF-8");
        }

        /// <summary>
        ///  删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult RemeProducts(string ids)
        {
            String[] id = ids.Split(',');
            Message msg;
            if (ids != null)
            {
                int i = productBLL.RemeProduct(id);

                if (i > 0)
                {
                    msg = new Message(true, "删除成功");
                }
                else
                {
                    msg = new Message(false, "删除失败");
                }
            }
            else
            {
                msg = new Message(false, "传值失败，请告诉开发者解决");
            }

            return Content(JsonConvert.SerializeObject(msg), "text/html;charset=UTF-8");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public ActionResult UpdateProduct(int id, FormCollection collection)
        {

            Product info = new Product();           
            info.ID = id;
            info.ProductTypeID = Convert.ToInt32(collection.Get("ProductTypeID"));
            string name = (!string.IsNullOrEmpty(collection.Get("ProductName")) ? collection.Get("ProductName") : null);
            info.ProductName = name;
            info.Image = collection.Get("Image");
            info.MarketPrice = Convert.ToInt32(collection.Get("MarketPrice"));
            info.NewPrice = Convert.ToInt32(collection.Get("NewPrice"));
            string en = collection.Get("Enable").ToString();
            info.Enable = AdminController.Enable(en);
            info.GetDate = DateTime.Now;
            Message msg;
            if (productBLL.UpdateProduct(info))
            {
                msg = new Message(true, "修改" + name + "信息成功！");
            }
            else
            {
                msg = new Message(false, "修改" + name + "失败！");
            }

            return Content(JsonConvert.SerializeObject(msg), "text/html;charset=UTF-8");
        }

        #endregion

    }
}
