using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Waf.Applications;
using Stump.Core.Reflection;
using Stump.Tools.Toolkit.Documents;
using Stump.Tools.Toolkit.ModelViews;
using Stump.Tools.Toolkit.ModelViews.D2P;
using Stump.Tools.Toolkit.Views;

namespace Stump.Tools.Toolkit.Controllers
{
    [Export(typeof(ApplicationController))]
    public class ApplicationController : Controller 
    {
        private readonly CompositionContainer m_container;
        private LoadScreenViewModel m_loadScreen;
        private readonly FileController m_fileController;

        [ImportingConstructor]
        public ApplicationController(CompositionContainer container, FileController fileController)
        {
            m_container = container;
            m_fileController = fileController;
        }

        public void Initialize()
        {
            m_fileController.Initialize();
        }

        public void Run()
        {
            // run
            var doc = m_fileController.Open(@"C:\Program Files (x86)\Dofus 2\app\content\maps\maps0.d2p") as D2PDocument;

            var view = new D2PViewModel(new D2PView(), doc);
            ( view.View as D2PView ).Show();
        }
    }
}