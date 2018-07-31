using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfUtilV1.Mvvm.View
{
    /// <summary>
    /// DataGridｶｽﾀﾑｺﾝﾄﾛｰﾙｸﾗｽです。
    /// Enterｷｰ押下時に右へﾌｫｰｶｽを移します。
    /// </summary>
    public class DataGridEx : DataGrid
    {
        public DataGridEx() : base()
        {

        }

        /// <summary>
        /// ｷｰﾀﾞｳﾝ時のｲﾍﾞﾝﾄ
        /// </summary>
        /// <param name="e">ｲﾍﾞﾝﾄ</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                case Key.Tab:
                    // 右のｾﾙに移動する。Shift押下時は左に移動する。
                    this.MoveNextCell(this.CurrentCell, Keyboard.Modifiers == ModifierKeys.Shift);
                    e.Handled = true;
                    break;
                case Key.Left:
                case Key.Right:
                    // 右ｷｰ押下で右のｾﾙに移動する。左ｷｰ押下で左のｾﾙに移動する。
                    this.MoveNextCell(this.CurrentCell, e.Key == Key.Left);
                    e.Handled = true;
                    break;
                case Key.Up:
                case Key.Down:
                    // 上下ｷｰ押下で編集を確定する
                    if (this.CommitEdit())
                    {
                        // その後は、ﾃﾞﾌｫﾙﾄ処理と同一処理を実行する
                        goto default;
                    }
                    else
                    {
                        // 編集確定で入力値検証ｴﾗｰとなった場合は中断
                        break;
                    }
                case Key.V:
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        // ﾍﾟｰｽﾄ
                        // ※ ｺﾋﾟｰはDataGridのｵﾌﾟｼｮﾝで実施できる。
                        this.OnPastingCellClipboardContent();
                    }
                    break;
                default:
                    // ﾃﾞﾌｫﾙﾄ
                    base.OnKeyDown(e);
                    break;
            }
        }

        /// <summary>
        /// ｸﾘｯﾌﾟﾎﾞｰﾄﾞの中身を現在のｾﾙに貼り付ける。
        /// </summary>
        private void OnPastingCellClipboardContent()
        {
            // ｸﾘｯﾌﾟﾎﾞｰﾄﾞのﾃﾞｰﾀ
            var dat = (string)Clipboard.GetData(DataFormats.Text);

            // 編集開始
            this.BeginEdit();

            // ﾍﾟｰｽﾄ処理
            CurrentColumn.OnPastingCellClipboardContent(CurrentItem, dat);

            // 編集確定
            if (!this.CommitEdit(DataGridEditingUnit.Row, true))
            {
                // 編集確定できなかった場合はｷｬﾝｾﾙ
                this.CancelEdit();
            }
        }

        /// <summary>
        /// 次のｾﾙを選択する。
        /// </summary>
        /// <param name="dgci">ﾃﾞｰﾀｸﾞﾘｯﾄﾞｾﾙ情報</param>
        /// <param name="shift">Shiftｷｰが押下されているかどうか</param>
        private void MoveNextCell(DataGridCellInfo dgci, bool shift)
        {
            // 最大行列を取得
            var maxCol = this.Columns.Count;
            var maxRow = this.Items.Count;

            // 現在行列を取得
            var currentCol = dgci.Column.DisplayIndex;
            var currentRow = this.Items.IndexOf(dgci.Item);

            // ｲﾝｸﾘﾒﾝﾄする方向をShiftによって決定する
            var incriment = (shift ? -1 : +1);

            // 次の列＝ｲﾝｸﾘﾒﾝﾄを足した方向。ただし最大列を超えた場合は先頭列に移動する
            var col = (maxCol + currentCol + incriment) % maxCol;
            // 次の行＝列が先頭行に移動していない場合は同一行。移動した場合は次の行。ただし最大行を超えた場合は先頭行に移動する
            var row = (maxRow + currentRow + (currentCol + incriment == col ? 0 : incriment)) % maxRow;

            if (0 <= row && row < Items.Count)
            {
                var cc = new DataGridCellInfo(Items[row], this.Columns[col]);

                if (cc.Column.IsReadOnly || cc.Column.Visibility != Visibility.Visible)
                {
                    // 選択せずに次のｶﾗﾑへ
                    MoveNextCell(cc, shift);
                }
                else if (this.CommitEdit(DataGridEditingUnit.Row, true))
                {
                    // 編集が終了できた場合、次のｾﾙを選択状態にする
                    SelectCell(cc);
                }
            }
        }

        /// <summary>
        /// 指定した列位置、行位置に紐付くｾﾙを選択する。
        /// </summary>
        /// <param name="col">列位置</param>
        /// <param name="row">行位置</param>
        public void SelectCell(int col, int row)
        {
            if (0 <= row && row < Items.Count && 0 <= col && col < Columns.Count)
            {
                SelectCell(new DataGridCellInfo(Items[row], this.Columns[col]));
            }
        }

        /// <summary>
        /// 指定したﾃﾞｰﾀｸﾞﾘｯﾄﾞｾﾙ情報に紐付くｾﾙを選択する。
        /// </summary>
        /// <param name="dataGridCellInfo">ﾃﾞｰﾀｸﾞﾘｯﾄﾞｾﾙ情報</param>
        public void SelectCell(DataGridCellInfo dataGridCellInfo)
        {
            this.CurrentCell = dataGridCellInfo;
            FrameworkElement fe = this.CurrentCell.Column.GetCellContent(CurrentCell.Item);
            if (fe != null)
            {
                fe = fe.Parent as DataGridCell;
            }
            if (fe != null)
            {
                ((DataGridCell)fe).IsSelected = true;
            }
        }
    }
}
