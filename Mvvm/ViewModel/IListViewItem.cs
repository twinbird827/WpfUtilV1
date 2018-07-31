using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV1.Mvvm.ViewModel
{
    /// <summary>
    /// ListViewItemに割り付けるViewModelが継承することを想定します。
    /// ListViewExを使用すると選択行のIsSelectedﾌﾟﾛﾊﾟﾃｨがtrueになります。
    /// </summary>
    public interface IListViewItem
    {
        /// <summary>
        /// 選択中かどうか
        /// </summary>
        bool IsSelected
        {
            get;
            set;
        }
    }
}
