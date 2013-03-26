using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using Model;
namespace DAL
{
    public sealed class MyContext : DbContext//继承DbContext 
    {
        private readonly static string CONNECTION_STRING = "MyProject";//web.config的数据库地址的名称//不写这个 默认的就是MyContext

        public MyContext() : base(CONNECTION_STRING)
        {
            this.Database.Initialize(true);//数据库初始化启用
        }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<Product> Product { get; set; }
      

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {           
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();//移除复数表名的契约           
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
        }
     
    }
}
