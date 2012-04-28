using System.Waf.Applications;
using System.Windows;

namespace Stump.Tools.Toolkit.ModelViews
{
    public abstract class StatusViewModel<T> : ViewModel<T> where T : IView
    {
        protected StatusViewModel(T view) : base(view)
        {
            StatusDefaultText = "Ready";
            ResetStatus();
        }

        public string StatusDefaultText
        {
            get;
            set;
        }

        public string StatusText
        {
            get;
            set;
        }

        public void ResetStatus()
        {
            StatusText = StatusDefaultText;
        }

        public void SetStatus(string text)
        {
            StatusText = text;
        }
    }
}