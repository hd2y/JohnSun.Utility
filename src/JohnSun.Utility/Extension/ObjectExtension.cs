using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Object扩展方法
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// 判断值是否为空或默认值
        /// </summary>
        /// <typeparam name="T">需要进行判断的数据类型</typeparam>
        /// <param name="value">要进行判断的值</param>
        /// <param name="def">默认值</param>
        /// <returns>值是否为空或与默认值相同</returns>
        public static bool IsNullOrDefault<T>(this T value, T def = default(T))
        {
            return value == null || value is DBNull || value.Equals(def);
        }
    }
}
