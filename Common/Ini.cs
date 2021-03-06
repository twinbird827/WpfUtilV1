﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV1.Common
{
    public abstract class Ini
    {
        /// <summary>
        /// INIﾌｧｲﾙのﾊﾟｽ
        /// </summary>
        protected string Path { get; set; }

        /// <summary>
        /// ｾｸｼｮﾝ
        /// </summary>
        protected string Section { get; set; }

        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        /// <param name="path">読み取るINIﾌｧｲﾙのﾊﾟｽ</param>
        /// <param name="section">規定のｾｸｼｮﾝ</param>
        protected Ini(string path, string section)
        {
            Path = path;
            Section = section;
        }

        /// <summary>
        /// sectionとkeyからiniﾌｧｲﾙの設定値を取得、設定します。 
        /// </summary>
        /// <returns>指定したsectionとkeyの組合せが無い場合は""が返ります。</returns>
        public string this[string section, string key, string defaultValue]
        {
            set
            {
                Win32Methods.WritePrivateProfileString(section, key, value, Path);
            }
            get
            {
                StringBuilder sb = new StringBuilder(256);
                Win32Methods.GetPrivateProfileString(section, key, defaultValue, sb, sb.Capacity, Path);
                return sb.ToString();
            }
        }

        /// <summary>
        /// keyからiniﾌｧｲﾙの設定値を取得、設定します。sectionは規定値を指定します。
        /// </summary>
        /// <returns>keyの組合せが無い場合は""が返ります。</returns>
        public string this[string key, string defaultValue]
        {
            set
            {
                this[Section, key, defaultValue] = value;
            }
            get
            {
                return this[Section, key, defaultValue];
            }
        }
    }
}
