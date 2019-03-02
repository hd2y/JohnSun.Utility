using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace System
{
    /// <summary>
    /// 字符串拓展方法
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 判断指定的字符串是否为日期和时间的字符串
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <param name="style">枚举值的按位组合，该组合定义如何根据当前时区或当前日期解释已分析日期。 要指定的一个典型值为 System.Globalization.DateTimeStyles.None。</param>
        /// <returns>真或假</returns>
        public static bool IsDateTime(this string s, IFormatProvider provider = null, DateTimeStyles? style = null)
        {
            if (style == null)
                style = DateTimeStyles.None;
            return DateTime.TryParse(s, null, style.Value, out DateTime dt);
        }

        /// <summary>
        /// 判断指定的字符串是否为指定格式的日期和时间的字符串
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="format">所需的 s 格式。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <param name="style">枚举值的按位组合，该组合定义如何根据当前时区或当前日期解释已分析日期。 要指定的一个典型值为 System.Globalization.DateTimeStyles.None。</param>
        /// <returns>真或假</returns>
        public static bool IsDateTimeExact(this string s, string format, IFormatProvider provider = null, DateTimeStyles? style = null)
        {
            if (style == null)
                style = DateTimeStyles.None;
            return DateTime.TryParseExact(s, format, null, style.Value, out DateTime dt);
        }

        /// <summary>
        /// 判断指定的字符串是否为布尔型
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <returns>真或假</returns>
        public static bool IsBoolean(this string s)
        {
            return bool.TryParse(s, out bool result);
        }

        /// <summary>
        /// 判断指定的字符串是否为字符型
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <returns>真或假</returns>
        public static bool IsChar(this string s)
        {
            return char.TryParse(s, out char result);
        }

        /// <summary>
        /// 判断指定的字符串是否为字节型
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>真或假</returns>
        public static bool IsByte(this string s, NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Integer;
            return byte.TryParse(s, style.Value, provider, out byte result);
        }

        /// <summary>
        /// 判断指定的字符串是否为有符号字节型
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>真或假</returns>
        public static bool IsSByte(this string s, NumberStyles? style, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Integer;
            return sbyte.TryParse(s, style.Value, provider, out sbyte result);
        }

        /// <summary>
        /// 判断指定的字符串是否为短整型数字
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>真或假</returns>
        public static bool IsInt16(this string s, NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Integer;
            return short.TryParse(s, style.Value, provider, out short result);
        }

        /// <summary>
        /// 判断指定的字符串是否为无符号短整型数字
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>真或假</returns>
        public static bool IsUInt16(this string s, NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Integer;
            return ushort.TryParse(s, style.Value, provider, out ushort result);
        }

        /// <summary>
        /// 判断指定的字符串是否为短整型数字
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>真或假</returns>
        public static bool IsShort(this string s, NumberStyles? style = null, IFormatProvider provider = null)
        {
            return IsInt16(s, style, provider);
        }

        /// <summary>
        /// 判断指定的字符串是否为无符号短整型数字
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>真或假</returns>
        public static bool IsUShort(this string s, NumberStyles? style = null, IFormatProvider provider = null)
        {
            return IsUInt16(s, style, provider);
        }

        /// <summary>
        /// 判断指定的字符串是否为整型数字
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>真或假</returns>
        public static bool IsInt32(this string s, NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Integer;
            return int.TryParse(s, style.Value, provider, out int result);
        }

        /// <summary>
        /// 判断指定的字符串是否为无符号整型数字
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>真或假</returns>
        public static bool IsUInt32(this string s, NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Integer;
            return uint.TryParse(s, style.Value, provider, out uint result);
        }

        /// <summary>
        /// 判断指定的字符串是否为整型数字
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>真或假</returns>
        public static bool IsInt(this string s, NumberStyles? style = null, IFormatProvider provider = null)
        {
            return IsInt32(s, style, provider);
        }

        /// <summary>
        /// 判断指定的字符串是否为无符号整型数字
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>真或假</returns>
        public static bool IsUInt(this string s, NumberStyles? style = null, IFormatProvider provider = null)
        {
            return IsInt32(s, style, provider);
        }

        /// <summary>
        /// 判断指定的字符串是否为长整型数字
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>真或假</returns>
        public static bool IsInt64(this string s, NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Integer;
            return long.TryParse(s, style.Value, provider, out long result);
        }

        /// <summary>
        /// 判断指定的字符串是否为无符号长整型数字
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>真或假</returns>
        public static bool IsUInt64(this string s, NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Integer;
            return ulong.TryParse(s, style.Value, provider, out ulong result);
        }

        /// <summary>
        /// 判断指定的字符串是否为长整型数字
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>真或假</returns>
        public static bool IsLong(this string s, NumberStyles? style = null, IFormatProvider provider = null)
        {
            return IsInt64(s, style, provider);
        }

        /// <summary>
        /// 判断指定的字符串是否为无符号长整型数字
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>真或假</returns>
        public static bool IsULong(this string s, NumberStyles? style = null, IFormatProvider provider = null)
        {
            return IsUInt64(s, style, provider);
        }

        /// <summary>
        /// 判断指定的字符串是否为单精度浮点型数字
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="style">枚举值的一个按位组合，指示 s 所允许的格式。 一个用来指定的典型值为 System.Globalization.NumberStyles.Float 与 System.Globalization.NumberStyles.AllowThousands 的组合。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>真或假</returns>
        public static bool IsFloat(this string s, NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Float | NumberStyles.AllowThousands;
            return float.TryParse(s, style.Value, provider, out float result);
        }

        /// <summary>
        /// 判断指定的字符串是否为单精度浮点型数字
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="style">枚举值的一个按位组合，指示 s 所允许的格式。 一个用来指定的典型值为 System.Globalization.NumberStyles.Float 与 System.Globalization.NumberStyles.AllowThousands 的组合。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>真或假</returns>
        public static bool IsSingle(this string s, NumberStyles? style = null, IFormatProvider provider = null)
        {
            return IsFloat(s, style, provider);
        }

        /// <summary>
        /// 判断指定的字符串是否为双精度浮点型数字
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="style">枚举值的一个按位组合，指示 s 所允许的格式。 一个用来指定的典型值为 System.Globalization.NumberStyles.Float 与 System.Globalization.NumberStyles.AllowThousands 的组合。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>真或假</returns>
        public static bool IsDouble(this string s, NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Float | NumberStyles.AllowThousands;
            return double.TryParse(s, style.Value, provider, out double result);
        }

        /// <summary>
        /// 判断指定的字符串是否为Decimal类型数字
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="style">枚举值的一个按位组合，指示 s 所允许的格式。 一个用来指定的典型值为 System.Globalization.NumberStyles.Float 与 System.Globalization.NumberStyles.AllowThousands 的组合。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>真或假</returns>
        public static bool IsDecimal(this string s, NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Float | NumberStyles.AllowThousands;
            return decimal.TryParse(s, style.Value, provider, out decimal result);
        }

        /// <summary>
        /// 判断指定的字符串是否为 TEnum 类型枚举
        /// </summary>
        /// <typeparam name="TEnum">要判断 value 的枚举类型。</typeparam>
        /// <param name="value">要确认的字符串</param>
        /// <param name="ignoreCase">若要不区分大小写，则为 true；若要区分大小写，则为 false。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>真或假</returns>
        public static bool IsEnum<TEnum>(this string value, bool? ignoreCase = null, IFormatProvider provider = null) where TEnum : struct
        {
            if (ignoreCase == null)
                ignoreCase = false;
            return Enum.TryParse<TEnum>(value, ignoreCase.Value, out TEnum @enum);
        }

        /// <summary>
        /// 判断指定字符串是否为合法IP地址
        /// </summary>
        /// <param name="input">要确认的字符串</param>
        /// <returns>真或假</returns>
        public static bool IsIpAddress(this string input)
        {
            return Regex.IsMatch(input, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 判断指定的字符串是否为Url地址
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <returns>真或假</returns>
        public static bool IsUrl(this string s)
        {
            return Regex.IsMatch(s, "(http[s]{0,1}|ftp)://[a-zA-Z0-9\\.\\-]+\\.([a-zA-Z]{2,4})(:\\d+)?(/[a-zA-Z0-9\\.\\-~!@#$%^&*+?:_/=<>]*)?");
        }

        /// <summary>
        /// 判断指定的字符串是否为合法Email
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <returns>真或假</returns>
        public static bool IsEmail(this string s)
        {
            return Regex.IsMatch(s, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// 判断字符串是否为Null或者为空
        /// </summary>
        /// <param name="input">要判断的字符串</param>
        /// <returns>是否为Null或者为空</returns>
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// 判断字符串是否为Null或者为空字符组成
        /// </summary>
        /// <param name="input">要判断的字符串</param>
        /// <returns>否为Null或者为空字符组成</returns>
        public static bool IsNullOrWhiteSpace(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        /// <summary>
        /// 返回字符串字节长度
        /// </summary>
        /// <param name="s">指定字符串</param>
        /// <param name="e">编码格式</param>
        /// <returns>字符串长度</returns>
        public static int Length(this string s, Encoding e = null)
        {
            if (e == null)
                e = Encoding.UTF8;
            return string.IsNullOrEmpty(s) ? 0 : e.GetBytes(s).Length;
        }

        /// <summary>
        /// 返回指定单词的复数形式
        /// </summary>
        /// <param name="input">单词</param>
        /// <returns>复数形式单词</returns>
        public static string ToPlural(this string input)
        {
            Pluralize.NET.Pluralizer pluralizer = new Pluralize.NET.Pluralizer();
            return pluralizer.Pluralize(input);
        }

        /// <summary>
        /// 返回指定单词的单数形式
        /// </summary>
        /// <param name="input">单词</param>
        /// <returns>单数形式单词</returns>
        public static string ToSingular(this string input)
        {
            Pluralize.NET.Pluralizer pluralizer = new Pluralize.NET.Pluralizer();
            return pluralizer.Singularize(input);
        }

        /// <summary>
        /// 字符串风格转换成 Key 的格式
        /// </summary>
        /// <param name="input">指定字符串</param>
        /// <returns>Key 格式字符串</returns>
        public static string ToKeyCase(this string input)
        {
            return string.IsNullOrEmpty(input)
                ? string.Empty : Regex.Replace(input.Trim().ToLower(), @"\s+", "_");
        }

        /// <summary>
        /// 字符串风格转换为 Pascal 格式
        /// </summary>
        /// <param name="input">指定字符串</param>
        /// <returns>Pascal 格式字符串</returns>
        public static string ToPascalCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            if (input.Length < 2)
                return input.ToUpper();

            string[] words = input.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
            var result = new StringBuilder();
            foreach (string word in words)
            {
                result.Append(word.Substring(0, 1).ToUpper()).Append(word.Substring(1));
            }

            return result.ToString();
        }

        /// <summary>
        /// 字符串风格转换为 Camel 格式
        /// </summary>
        /// <param name="input">指定字符串</param>
        /// <returns>Camel 格式字符串</returns>
        public static string ToCamelCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            if (input.Length < 2)
                return input.ToLower();

            string[] words = input.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
            var result = new StringBuilder(words[0].ToLower());
            for (int i = 1; i < words.Length; i++)
            {
                result.Append(words[i].Substring(0, 1).ToUpper()).Append(words[i].Substring(1));
            }
            return result.ToString();
        }

        /// <summary>
        /// 字符串风格转换为下划线格式
        /// </summary>
        /// <param name="input">指定字符串</param>
        /// <returns>下划线格式字符串</returns>
        public static string ToUnderlineCase(this string input)
        {
            return string.IsNullOrEmpty(input)
                ? string.Empty : Regex.Replace(input.Trim().ToLower(), @"(?<=[a-z0-9])(?=[A-Z])", "_");
        }

        /// <summary>
        /// 字符串风格转换成正常格式
        /// </summary>
        /// <param name="input">指定字符串</param>
        /// <returns>正常格式字符串</returns>
        public static string ToProperCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            if (input.Length < 2)
                return input.ToUpper();

            var result = new StringBuilder(input.Substring(0, 1).ToUpper());
            for (int i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]))
                    result.Append(" ");

                result.Append(input[i]);
            }
            return result.ToString();
        }

        /// <summary>
        /// 字符串风格转换成 Slug 格式
        /// </summary>
        /// <param name="input">指定字符串</param>
        /// <returns>Slug 格式字符串</returns>
        public static string ToSlugCase(this string input)
        {
            return string.IsNullOrEmpty(input)
                ? string.Empty : Regex.Replace(input.Trim().ToLower(), @"\s+", "-");
        }

        /// <summary>
        /// 将指定的字符串转换为日期和时间
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <param name="style">枚举值的按位组合，该组合定义如何根据当前时区或当前日期解释已分析日期。 要指定的一个典型值为 System.Globalization.DateTimeStyles.None。</param>
        /// <returns>日期和时间</returns>
        public static DateTime ToDateTime(this string s, DateTime def = default(DateTime), IFormatProvider provider = null, DateTimeStyles? style = null)
        {
            if (style == null)
                style = DateTimeStyles.None;
            if (!DateTime.TryParse(s, null, style.Value, out DateTime dt))
                dt = def;
            return dt;
        }

        /// <summary>
        /// 将指定的字符串按指定格式转换为日期和时间
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="format">所需的 s 格式。</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <param name="style">枚举值的按位组合，该组合定义如何根据当前时区或当前日期解释已分析日期。 要指定的一个典型值为 System.Globalization.DateTimeStyles.None。</param>
        /// <returns>日期和时间</returns>
        public static DateTime ToDateTimeExact(this string s, string format, DateTime def = default(DateTime), IFormatProvider provider = null, DateTimeStyles? style = null)
        {
            if (style == null)
                style = DateTimeStyles.None;
            if (!DateTime.TryParseExact(s, format, null, style.Value, out DateTime dt))
                dt = def;
            return dt;
        }

        /// <summary>
        /// 将指定的字符串按指定格式转换为布尔型
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <returns>布尔值</returns>
        public static bool ToBoolean(this string s, bool def = default(bool))
        {
            if (!bool.TryParse(s, out bool result))
                result = def;
            return result;
        }

        /// <summary>
        /// 将指定的字符串按指定格式转换为字符型
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <returns>字符型</returns>
        public static char ToChar(this string s, char def = default(char))
        {
            if (!char.TryParse(s, out char result))
                result = def;
            return result;
        }

        /// <summary>
        /// 将指定的字符串按指定格式转换为字节型
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>字节型值</returns>
        public static byte ToByte(this string s, byte def = default(byte), NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Integer;
            if (!byte.TryParse(s, style.Value, provider, out byte result))
                result = def;
            return result;
        }

        /// <summary>
        /// 判断指定的字符串是否为有符号字节型
        /// </summary>
        /// <param name="s">要确认的字符串</param>
        /// <param name="def">转换失败默认返回结果</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>真或假</returns>
        public static sbyte ToSByte(this string s, sbyte def = default(sbyte), NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Integer;
            if (!sbyte.TryParse(s, style.Value, provider, out sbyte result))
                result = def;
            return result;
        }

        /// <summary>
        /// 将指定字符串转换为短整型数字
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>短整型数字</returns>
        public static short ToInt16(this string s, short def = default(short), NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Integer;
            if (!short.TryParse(s, style.Value, provider, out short result))
                result = def;
            return result;
        }

        /// <summary>
        /// 将指定字符串转换为无符号短整型数字
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>无符号短整型数字</returns>
        public static ushort ToUInt16(this string s, ushort def = default(ushort), NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Integer;
            if (!ushort.TryParse(s, style.Value, provider, out ushort result))
                result = def;
            return result;
        }

        /// <summary>
        /// 将指定字符串转换为短整型数字
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>短整型数字</returns>
        public static short ToShort(this string s, short def = default(short), NumberStyles? style = null, IFormatProvider provider = null)
        {
            return ToInt16(s, def, style, provider);
        }

        /// <summary>
        /// 将指定字符串转换为无符号短整型数字
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>无符号短整型数字</returns>
        public static ushort ToUShort(this string s, ushort def = default(ushort), NumberStyles? style = null, IFormatProvider provider = null)
        {
            return ToUInt16(s, def, style, provider);
        }

        /// <summary>
        /// 将指定字符串转换为整型数字
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>整型数字</returns>
        public static int ToInt32(this string s, int def = default(int), NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Integer;
            if (!int.TryParse(s, style.Value, provider, out int result))
                result = def;
            return result;
        }

        /// <summary>
        /// 将指定字符串转换为无符号整型数字
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>无符号整型数字</returns>
        public static uint ToUInt32(this string s, uint def = default(uint), NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Integer;
            if (!uint.TryParse(s, style.Value, provider, out uint result))
                result = def;
            return result;
        }

        /// <summary>
        /// 将指定字符串转换为整型数字
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>整型数字</returns>
        public static int ToInt(this string s, int def = default(int), NumberStyles? style = null, IFormatProvider provider = null)
        {
            return ToInt32(s, def, style, provider);
        }

        /// <summary>
        /// 将指定字符串转换为无符号整型数字
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>无符号整型数字</returns>
        public static uint ToUInt(this string s, uint def = default(uint), NumberStyles? style = null, IFormatProvider provider = null)
        {
            return ToUInt32(s, def, style, provider);
        }

        /// <summary>
        /// 将指定字符串转换为长整型数字
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>长整型数字</returns>
        public static long ToInt64(this string s, long def = default(long), NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Integer;
            if (!long.TryParse(s, style.Value, provider, out long result))
                result = def;
            return result;
        }

        /// <summary>
        /// 将指定字符串转换为无符号长整型数字
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>无符号长整型数字</returns>
        public static ulong ToUInt64(this string s, ulong def = default(ulong), NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Integer;
            if (!ulong.TryParse(s, style.Value, provider, out ulong result))
                result = def;
            return result;
        }

        /// <summary>
        /// 将指定字符串转换为无符号长整型数字
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>无符号长整型数字</returns>
        public static ulong ToULong(this string s, ulong def = default(ulong), NumberStyles? style = null, IFormatProvider provider = null)
        {
            return ToUInt64(s, def, style, provider);
        }

        /// <summary>
        /// 将指定字符串转换为长整型数字
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <param name="style">枚举值的按位组合，用于指示可出现在 s 中的样式元素。 要指定的一个典型值为 System.Globalization.NumberStyles.Integer。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>长整型数字</returns>
        public static long ToLong(this string s, long def = default(long), NumberStyles? style = null, IFormatProvider provider = null)
        {
            return ToInt64(s, def, style, provider);
        }

        /// <summary>
        /// 将指定字符串转换为单精度浮点型数字
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <param name="style">枚举值的一个按位组合，指示 s 所允许的格式。 一个用来指定的典型值为 System.Globalization.NumberStyles.Float 与 System.Globalization.NumberStyles.AllowThousands 的组合。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>单精度浮点型数字</returns>
        public static float ToFloat(this string s, float def = default(float), NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Float | NumberStyles.AllowThousands;
            if (!float.TryParse(s, style.Value, provider, out float result))
                result = def;
            return result;
        }

        /// <summary>
        /// 将指定字符串转换为单精度浮点型数字
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <param name="style">枚举值的一个按位组合，指示 s 所允许的格式。 一个用来指定的典型值为 System.Globalization.NumberStyles.Float 与 System.Globalization.NumberStyles.AllowThousands 的组合。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>单精度浮点型数字</returns>
        public static float ToSingle(this string s, float def = default(float), NumberStyles? style = null, IFormatProvider provider = null)
        {
            return ToFloat(s, def, style, provider);
        }

        /// <summary>
        /// 将指定字符串转换为双精度浮点型数字
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <param name="style">枚举值的一个按位组合，指示 s 所允许的格式。 一个用来指定的典型值为 System.Globalization.NumberStyles.Float 与 System.Globalization.NumberStyles.AllowThousands 的组合。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>双精度浮点型数字</returns>
        public static double ToDouble(this string s, double def = default(double), NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Float | NumberStyles.AllowThousands;
            if (!double.TryParse(s, style.Value, provider, out double result))
                result = def;
            return result;
        }

        /// <summary>
        /// 将指定字符串转换为 Decimal 类型数字
        /// </summary>
        /// <param name="s">要转换的字符串</param>
        /// <param name="def">转换失败返回的结果</param>
        /// <param name="style">枚举值的一个按位组合，指示 s 所允许的格式。 一个用来指定的典型值为 System.Globalization.NumberStyles.Float 与 System.Globalization.NumberStyles.AllowThousands 的组合。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <returns>Decimal 类型数字</returns>
        public static decimal ToDecimal(this string s, decimal def = default(decimal), NumberStyles? style = null, IFormatProvider provider = null)
        {
            if (style == null)
                style = NumberStyles.Float | NumberStyles.AllowThousands;
            if (!decimal.TryParse(s, style.Value, provider, out decimal result))
                result = def;
            return result;
        }

        /// <summary>
        /// 将指定字符串转换为 TEnum 类型枚举
        /// </summary>
        /// <typeparam name="TEnum">要转换 value 的枚举类型。</typeparam>
        /// <param name="def">转换失败返回的结果</param>
        /// <param name="value">要确认的字符串</param>
        /// <param name="ignoreCase">若要不区分大小写，则为 true；若要区分大小写，则为 false。</param>
        /// <returns>TEnum 类型枚举</returns>
        public static TEnum ToEnum<TEnum>(this string value, TEnum def = default(TEnum), bool ignoreCase = false) where TEnum : struct
        {
            if (!Enum.TryParse<TEnum>(value, ignoreCase, out TEnum result))
                result = def;
            return result;
        }

        /// <summary>
        /// String To Encoding
        /// </summary>
        /// <param name="s">String</param>
        /// <param name="def">default</param>
        /// <returns>Encoding</returns>
        public static Encoding ToEncoding(this string s, Encoding def = null)
        {
            try
            {
                if (string.IsNullOrEmpty(s)
                    || string.Equals("default", s, StringComparison.OrdinalIgnoreCase)
                    || string.Equals("ansi", s, StringComparison.OrdinalIgnoreCase))
                    def = Encoding.Default;
                else
                    def = Encoding.GetEncoding(s);
            }
            finally
            {
                def = def ?? Encoding.Default;
            }
            return def;
        }

        /// <summary>
        /// 将指定字符串转换为十六进制Hex字符串形式。
        /// </summary>
        /// <param name="input">要转换的原始字符串。</param>
        /// <param name="e">编码</param>
        /// <returns>转换后的内容</returns>
        public static string ToHex(this string input, Encoding e = null)
        {
            e = e ?? Encoding.UTF8;
            byte[] byteArray = e.GetBytes(input);
            return BitConverter.ToString(byteArray).Replace('-', ' ');
        }

        /// <summary>
        /// 将十六进制Hex字符串转换为原始字符串。
        /// </summary>
        /// <param name="input">十六进制字符串内容</param>
        /// <param name="e">编码</param>
        /// <returns>原始字符串内容</returns>
        public static string ReHex(this string input, Encoding e = null)
        {
            e = e ?? Encoding.UTF8;
            input = input.Replace(" ", string.Empty);
            if (input.Length <= 0) return "";
            byte[] vBytes = new byte[input.Length / 2];
            for (int i = 0; i < input.Length; i += 2)
                vBytes[i / 2] = byte.Parse(input.Substring(i, 2), NumberStyles.HexNumber);
            return e.GetString(vBytes);
        }

        /// <summary>
        /// 将BitConverter.ToString(bytes)转换得到的16进制结果反转为字节数组
        /// </summary>
        /// <param name="input">指定的字符串</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBytesFromBitConverter(this string input)
        {
            string[] strSplit = input.Split('-');
            byte[] bytes = new byte[strSplit.Length];
            for (int i = 0; i < strSplit.Length; i++)
                bytes[i] = byte.Parse(strSplit[i], System.Globalization.NumberStyles.HexNumber);
            return bytes;
        }

        /// <summary>
        /// 将base64string转换为字节数组
        /// </summary>
        /// <param name="input">指定的字符串</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBytesFromBase64String(this string input)
        {
            return Convert.FromBase64String(input);
        }

        /// <summary>
        /// 将指定范围内的字符转换为包含Hex内容的字符。 
        /// </summary>
        /// <param name="input">原始内容</param>
        /// <param name="start">开始字符 默认 NUL-空字符</param>
        /// <param name="end">结束字符 默认 US-单元分隔符</param>
        /// <param name="format">字符转换后Hex字符的内容 默认 #{0}'</param>
        /// <returns>转义后的内容</returns>
        public static string ToExplain(this string input, char start = '\u0000', char end = '\u001f', string format = "#{0}'")
        {
            List<char> listCh = new List<char>();
            listCh.AddRange(input.ToCharArray());
            for (int i = listCh.Count - 1; i >= 0; i--)
            {
                if (listCh[i] >= start && listCh[i] <= end)
                {
                    string temp = string.Format(format, new string(listCh[i], 1).ToHex());
                    listCh.RemoveAt(i);
                    listCh.InsertRange(i, temp);
                }
            }
            return new string(listCh.ToArray());
        }

        /// <summary>
        /// 将ToExplain转换后的字符串转换到原始内容。
        /// </summary>
        /// <param name="input">需要转换的字符串</param>
        /// <param name="pattern">正则匹配 默认匹配如 #05' 格式</param>
        /// <returns>转换后的内容</returns>
        public static string ReExplain(this string input, string pattern = "#([0-9A-F]{2})'")
        {
            Dictionary<string, string> dicRep = new Dictionary<string, string>();
            Regex regex = new Regex(pattern);
            MatchCollection mc = regex.Matches(input);
            for (int i = 0; i < mc.Count; i++)
            {
                if (!dicRep.ContainsKey(mc[i].Value))
                {
                    dicRep.Add(mc[i].Value, mc[i].Groups[1].Value.ReHex());
                }
            }
            foreach (KeyValuePair<string, string> item in dicRep)
            {
                input = input.Replace(item.Key, item.Value);
            }

            return input;
        }

        /// <summary>
        /// 正则替换
        /// </summary>
        /// <param name="input">原始文本</param>
        /// <param name="pattern">正则</param>
        /// <param name="replacement">替换后的内容</param>
        /// <returns>结果</returns>
        public static string RegexReplace(this string input, string pattern, string replacement)
        {
            return Regex.Replace(input ?? string.Empty, pattern, replacement);
        }

        /// <summary>
        /// 提取字符串所有数字
        /// </summary>
        /// <param name="input">原始文本</param>
        /// <returns>结果</returns>
        public static string ExtractDigits(this string input)
        {
            return input.RegexReplace(@"[^0-9]+", string.Empty);
        }

        /// <summary>
        /// 将指定字符串中的格式项替换为指定数组中相应对象的字符串表示形式。
        /// </summary>
        /// <param name="input">复合格式字符串。</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象。</param>
        /// <returns>format 的副本，其中的格式项已替换为 args 中相应对象的字符串表示形式。</returns>
        /// <exception cref="System.ArgumentNullException">input 或 args 为 null。</exception>
        /// <exception cref="System.FormatException">format 无效。- 或 -格式项的索引小于零或大于等于 args 数组的长度。</exception>
        public static string Format(this string input, params object[] args)
        {
            return string.Format(input, args);
        }

        /// <summary>
        /// 对字符串进行 HTML 编码并返回已编码的字符串
        /// </summary>
        /// <param name="content">要编码的文本字符串</param>
        /// <returns>HTML 已编码的文本</returns>
        public static string HtmlEncode(this string content)
        {
            return HttpUtility.HtmlEncode(content);
        }

        /// <summary>
        /// 对 HTML 编码的字符串进行解码，并返回已解码的字符串
        /// </summary>
        /// <param name="content">要解码的文本字符串</param>
        /// <returns>已解码的字符串</returns>
        public static string HtmlDecode(this string content)
        {
            return HttpUtility.HtmlDecode(content);
        }

        /// <summary>
        /// 对 URL 字符串进行编码, 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">要编码的文本</param>
        /// <param name="e">编码</param>
        /// <returns>一个已编码的字符串</returns>
        public static string UrlEncode(this string str, Encoding e = null)
        {
            e = e ?? Encoding.UTF8;
            return HttpUtility.UrlEncode(str, e);
        }

        /// <summary>
        /// 对 URL 字符串进行解码, 返回 URL 字符串的解码结果
        /// </summary>
        /// <param name="str">要解码的文本</param>
        /// <param name="e">编码</param>
        /// <returns>解码结果</returns>
        public static string UrlDecode(this string str, Encoding e = null)
        {
            e = e ?? Encoding.UTF8;
            return HttpUtility.UrlDecode(str);
        }
    }
}
