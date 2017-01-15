using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2.contract
{
    static class actionConst
    {

        public const string connect = "";

        /// <summary>
        /// 登陆
        /// </summary>
        public const string signOn = "signOn";

        /// <summary>
        /// 注册
        /// </summary>
        public const string signIn = "signIn";

        /// <summary>
        /// 退出
        /// </summary>
        public const string signOff = "signOff";

        /// <summary>
        /// 消息传递
        /// </summary>
        public const string communication = "communication";

        public const string server = "server";

        /// <summary>
        /// 监听的端口
        /// </summary>
        public const int port = 1500;

        /// <summary>
        /// 数据请求
        /// </summary>
        public const string dataRequest = "dataRequest";

        /// <summary>
        /// 数据答复
        /// </summary>
        public const string dataAnswer = "dataAnswer";

        /// <summary>
        /// 对于消息的确认
        /// </summary>
        public const string confirm = "confirm";

        
    }
}
