using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfUtilV1.Mvvm.ViewModel;

namespace WpfUtilV1.Mvvm.View
{
    public class ListViewEx : ListView
    {
        public ListViewEx() : base()
        {
            SelectionChanged += lvw_SelectionChanged;
        }

        void lvw_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.RemovedItems.Cast<IListViewItem>())
                item.IsSelected = false;
            foreach (var item in e.AddedItems.Cast<IListViewItem>())
                item.IsSelected = true;
        }
    }
}
