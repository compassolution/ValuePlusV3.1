using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Database
{
    public enum  TypeDao
    {
      /// <summary>
      /// 字符
      /// </summary>
        Char =0,
        /// <summary>
        /// unicode 字符
        /// </summary>
        NChar=1,
        /// <summary>
        /// 可变长字符串,8000
        /// </summary>
        VarChar=2,
        /// <summary>
        /// 可变长度的字符串4000，unicode
        /// </summary>
        NVarChar=3,
        /// <summary>
        /// 长度为2 30 -1
        /// </summary>
        Text=4,
        /// <summary>
        /// 长度为2 30 -1,unicode
        /// </summary>
        NText=5,
        /// <summary>
        /// 整形
        /// </summary>
        Int =6,
        /// <summary>
        /// 位
        /// </summary>
        Bit = 7,
       /// <summary>
       /// 大整形
       /// </summary>
        BigInt = 8,
        /// <summary>
        /// 长整形
        /// </summary>
        TinyInt = 9,
        /// <summary>
        /// 小整型
        /// </summary>
        SmallInt = 10,
        /// <summary>
        /// 固定精度
        /// </summary>
        Decimal=11,
        /// <summary>
        /// 小货币
        /// </summary>
        SmallMoney = 12,
        /// <summary>
        /// 货币
        /// </summary>
        Money=13,
        /// <summary>
        /// 双精度
        /// </summary>
        Float=14,
        /// <summary>
        /// 单精度
        /// </summary>
        Real=15, 
        /// <summary>
        /// 日期
        /// </summary>
        DateTime=16, 
        /// <summary>
        /// 短日期
        /// </summary>
        SmallDateTime=17,
        /// <summary>
        /// Timestamp
        /// </summary>
        Timestamp=18, 
        /// <summary>
        /// 图片
        /// </summary>
        Image=19,
        /// <summary>
        /// 二进制
        /// </summary>
        Binary=20,
        /// <summary>
        /// 二进制
        /// </summary>
        Variant=21,
        /// <summary>
        /// guid
        /// </summary>
        UniqueIdentifier=22,
        /// <summary>
        /// 二进制
        /// </summary>
        VarBinary = 23,

        NULL = 24
    }
}
