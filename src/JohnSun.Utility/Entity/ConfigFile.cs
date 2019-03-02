using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JohnSun.Utility.Helper
{
    /// <summary>
    /// 数据库文件配置
    /// </summary>
    public class ConfigFile
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Describe { get; set; }
    }
}
