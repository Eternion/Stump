using System;
using System.Reflection;
using Stump.Server.BaseServer.Initialization;
using Stump.Tools.Toolkit.Views;

namespace Stump.Tools.Toolkit.ModelViews
{
    public class LoadScreenViewModel : ViewModelBase<LoadScreen>
    {
        public LoadScreenViewModel(LoadScreen view) 
            : base(view)
        {
        }

        public void Show()
        {
            View.Show();
        }

        public void Close()
        {
            View.Close();
        }

        public void InitializeApp()
        {
            InitializationManager.Instance.AddAssembly(Assembly.GetExecutingAssembly());
            var passes = Enum.GetValues(typeof(InitializationPass)) as InitializationPass[];
            InitializationManager.Instance.ProcessInitialization +=
                OnProcessInitialization;

            LoadingPercent = 0;

            for (int i = 0; i < passes.Length; i++)
            {
                InitializationManager.Instance.Initialize(passes[i]);
                LoadingPercent = ( (double)i / passes.Length ) * 100;
            }
        }

        private void OnProcessInitialization(string str)
        {
            LoadingMessage = str;
        }

        public string LoadingMessage
        {
            get;
            set;
        }

        public double LoadingPercent
        {
            get;
            set;
        }
    }
}