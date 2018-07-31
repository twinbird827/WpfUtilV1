using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV1.Mvvm.View
{
    public enum ListViewScrollPosition
    {
        /// <summary>
        /// ﾘｽﾄﾋﾞｭｰの中身が変更されたﾀｲﾐﾝｸﾞでｽｸﾛｰﾙを先頭行に移します。
        /// </summary>
        Top,

        /// <summary>
        /// ｽｸﾛｰﾙ位置は規定です。
        /// </summary>
        Default,

        /// <summary>
        /// ﾘｽﾄﾋﾞｭｰの中身が変更されたﾀｲﾐﾝｸﾞでｽｸﾛｰﾙを最終行に移します。
        /// </summary>
        Bottom
    }
}
