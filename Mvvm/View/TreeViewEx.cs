﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfUtilV1.Mvvm.ViewModel;

namespace WpfUtilV1.Mvvm.View
{
    public class TreeViewEx : TreeView
    {
        public TreeViewEx() : base()
        {
            SelectedItemChanged += lvw_SelectionChanged;
        }

        void lvw_SelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var o = e.OldValue as IListViewItem;
            if (o != null) o.IsSelected = false;
            var n = e.NewValue as IListViewItem;
            if (n != null) n.IsSelected = true;
        }

    }
}
