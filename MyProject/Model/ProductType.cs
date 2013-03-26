using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;//数据验证
using System.Web;


namespace Model
{
    public class ProductType
    {
        /// <summary>
        /// 产品类型编号
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        /// <summary>
        /// 产品类型名称
        /// </summary>
        [Required(ErrorMessage = "产品类型名称")]
        [MaxLength(50)]   
        public string ProductTypeName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>       
        [MaxLength(100)]   
        public string Description { get; set; }
        /// <summary>
        ///  导航属性，将作为外键
        /// </summary>
        public virtual ICollection<Product> Product { get; set; }
        //public List<Product> Product { get; set; } 都可以
        
    }

    
}
