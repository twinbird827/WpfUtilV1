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
        private BindableBase Source { get; set; }

        public ViewModelBase()
            : this(null)
        {

        }

        public ViewModelBase(BindableBase source)
        {
            Source = source;
            
            if (Source != null)
            {
                Source.PropertyChanged += OnPropertyChanged;
            }
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            if (Source != null)
            {
                Source.PropertyChanged -= OnPropertyChanged;
            }
            Source = null;
        }
    }
}
