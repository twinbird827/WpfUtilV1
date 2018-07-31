using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfUtilV1.Mvvm.ViewModel;

namespace WpfUtilV1.Mvvm.View
{
    /// <summary>
    /// ListViewｶｽﾀﾑｺﾝﾄﾛｰﾙｸﾗｽです。
    /// 選択行のIsSelectedﾌﾟﾛﾊﾟﾃｨを変更します。
    /// ItemsSourceの項目数に変化があるとき、ｽｸﾛｰﾙ位置をScrollPositionﾌﾟﾛﾊﾟﾃｨの通り変更します。
    /// </summary>
    public class ListViewEx : ListView
    {
        public ListViewScrollPosition ScrollPosition
        {
            get { return (ListViewScrollPosition)base.GetValue(ScrollPositionProperty); }
            set { base.SetValue(ScrollPositionProperty, value); }
        }
        public static readonly DependencyProperty ScrollPositionProperty =
            DependencyProperty.Register("ScrollPosition", typeof(ListViewScrollPosition), typeof(ListViewEx), new PropertyMetadata(ListViewScrollPosition.Top));

        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public ListViewEx() : base()
        {
            // ItemsSource更新時にｽｸﾛｰﾙ位置を調整する
            TargetUpdated += lvw_TargetUpdated;

            // 選択行変更時にIsSelectedﾌﾟﾛﾊﾟﾃｨを変更する
            SelectionChanged += lvw_SelectionChanged;
        }

        /// <summary>
        /// 選択行変更時ｲﾍﾞﾝﾄ
        /// </summary>
        private void lvw_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.RemovedItems.Cast<IListViewItem>())
                item.IsSelected = false;
            foreach (var item in e.AddedItems.Cast<IListViewItem>())
                item.IsSelected = true;
        }

        /// <summary>
        /// ItemsSource変更時ｲﾍﾞﾝﾄ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvw_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            (ItemsSource as INotifyCollectionChanged).CollectionChanged += new NotifyCollectionChangedEventHandler(lvw_CollectionChanged);
        }

        /// <summary>
        /// ItemsSource項目数変更時ｲﾍﾞﾝﾄ
        /// </summary>
        private void lvw_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Items.Count == 0) return;

            switch (ScrollPosition)
            {
                case ListViewScrollPosition.Top:
                    ScrollIntoView(Items[0]);
                    break;
                case ListViewScrollPosition.Bottom:
                    ScrollIntoView(Items[Items.Count - 1]);
                    break;
            }
        }
    }
}
