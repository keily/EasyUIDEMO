using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    [Serializable()]
    public class Message
    {
        /// <summary>
        /// 信息提示，当提交成功的时候返回给前台json的格式数据
        /// 用到相关json类库：JavaScriptSerializer json = new JavaScriptSerializer();
        /// </summary>
        /// <param name="success"></param>
        /// <param name="msg"></param>
        public Message(bool success, string msg)
        {
            this.msg = msg;
            this.success = success;
        }
        public bool success { get; set; }
        public string msg { get; set; }
    }
}
