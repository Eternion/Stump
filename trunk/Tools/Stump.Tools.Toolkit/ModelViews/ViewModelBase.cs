using System.ComponentModel;
using Stump.Tools.Toolkit.Views;

namespace Stump.Tools.Toolkit.ModelViews
{
    public abstract class ViewModelBase<T> : INotifyPropertyChanged
        where T : IView
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public T View
        {
            get;
            protected set;
        }

        protected ViewModelBase(T view)
        {
            View = view;

            view.DataContext = this;
        }

    }
}