using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Linq扩展方法
    /// </summary>
    public static class CollectionExtension
    {
        /// <summary>
        /// 判断数组或集合是否为空，注意判断使用了 "Count()" 方法
        /// </summary>
        /// <typeparam name="T">数组或集合内元素类型</typeparam>
        /// <param name="collection">数组或集合</param>
        /// <returns>是否为空</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || collection.Count() == 0;
        }

        /// <summary>
        /// 判断集合是否为空，如果进入迭代器内部循环则判断非空
        /// </summary>
        /// <param name="collection">集合</param>
        /// <returns>是否为空</returns>
        public static bool IsNullOrEmptyAsIEnumerable(this IEnumerable collection)
        {
            if (collection == null)
                return true;
            foreach (object item in collection)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 创建集合，为空返回一个空集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static List<T> ToSafeList<T>(this IEnumerable<T> collection)
        {
            return collection == null ? new List<T>() : collection.ToList();
        }

        /// <summary>
        /// 创建数组，为空返回一个空数组
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="collection">指定的数据源</param>
        /// <returns>空或数组</returns>
        public static T[] ToSafeArray<T>(this IEnumerable<T> collection)
        {
            return collection == null ? new T[] { } : collection.ToArray();
        }

        /// <summary>
        /// 获取集合长度，为空则返回 0
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="collection">指定的数据源</param>
        /// <returns>数组或集合的长度</returns>
        public static int SafeCount<T>(this IEnumerable<T> collection)
        {
            return collection == null ? 0 : collection.Count();
        }
    }
}
