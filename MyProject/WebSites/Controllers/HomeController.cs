using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using Model;

namespace WebSites.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            var ef = new MyContext();
            //ef.Database.Create();//方法二：直接执行生成数据库
            
            return View();
        }

    }
}
