using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;//数据/验证

namespace Model
{
    public class Product
    {
        /// <summary>
        /// 产品编号
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public int ID { get; set; }
        /// <summary>
        ///  产品类型编号
        /// </summary>
        public int ProductTypeID { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        [Required(ErrorMessage = "产品名称")]
        [MaxLength(50)]   
        public string ProductName { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// 价格
        /// </summary>     
        public decimal MarketPrice { get; set; }
        public decimal NewPrice { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime GetDate { get; set; }//注意这个
        /// <summary>
        /// 是否上架
        /// </summary>       
        public bool? Enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual ProductType ProductType { get; set; }
      
    }

   
}
