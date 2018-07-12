using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV1.Mvvm.ViewModel
{
    public class ViewModelBase : BindableBase
    {
        public ViewModelBase()
            : base()
        {

        }

        public ViewModelBase(BindableBase source)
            : base(source)
        {

        }
    }
}
