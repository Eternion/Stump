using Stump.Core.Reflection;
using Stump.Tools.Toolkit.ModelViews;
using Stump.Tools.Toolkit.Views;

namespace Stump.Tools.Toolkit.Controllers
{
    public class ApplicationController : Singleton<ApplicationController>
    {
        private LoadScreenViewModel m_loadScreen;

        public void Initialize()
        {
            m_loadScreen = new LoadScreenViewModel(new LoadScreen());
            m_loadScreen.Show();
            m_loadScreen.InitializeApp();
            m_loadScreen.Close();
        }

        public void Run()
        {
            // run
        }
    }
}