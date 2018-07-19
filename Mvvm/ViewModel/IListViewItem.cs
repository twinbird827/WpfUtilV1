using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV1.Mvvm.ViewModel
{
    public interface IListViewItem
    {
        bool IsSelected
        {
            get;
            set;
        }
    }
}
