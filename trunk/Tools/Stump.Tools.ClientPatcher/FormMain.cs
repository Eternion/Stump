using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using Stump.Core.Cryptography;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Tools.ClientPatcher.Properties;

namespace Stump.Tools.ClientPatcher
{
    public partial class FormMain : Form
    {
#if DEBUG
        public const string ServerAddress = "localhost";
#else
        public const string ServerAddress = "188.165.237.43";
#endif
        public const string OfficialServerAddress = "213.248.126.180";

        public FormMain()
        {
            InitializeComponent();
        }

        private void ButtonPatchClick(object sender, EventArgs e)
        {
            buttonPatch.Enabled = false;
            var dofusPath = FindDofusPath();

            // remove the hidden code in the i18n file
            var i18nDir = Path.Combine(dofusPath, "data", "i18n");
            var i18n = new I18NFile(Path.Combine(i18nDir, "i18n_fr.d2i"));

            i18n.SetText("ui.link.changelog", Convert.ToBase64String(Resources.Empty));
            i18n.SetText("ui.login.forgottenPassword", "Client patched");
            i18n.Update();


            // update the meta file
            var document = new XmlDocument();
            document.Load(Path.Combine(i18nDir, "data.meta"));
            var navigator = document.CreateNavigator();

            navigator.SelectSingleNode("//file[@name='i18n_fr.d2i']").SetValue(Cryptography.GetMD5Hash(File.ReadAllText(Path.Combine(i18nDir, "i18n_fr.d2i"))));

            document.Save(Path.Combine(i18nDir, "data.meta"));

            // change the config file
            var config = new XmlDocument();
            config.Load(Path.Combine(dofusPath, "config.xml"));
            var configNavigator = document.CreateNavigator();

            configNavigator.SelectSingleNode("//<entry [@key='connection.host']").SetValue(ServerAddress);

            config.Save(Path.Combine(dofusPath, "config.xml"));

            // replace the swf file with the patched one
            File.WriteAllBytes(Path.Combine(dofusPath, "DofusInvoker.swf"), Resources.DofusInvokerModified);

            // clear the client cache
            ClearCache();

            buttonPatch.Enabled = true;
            MessageBox.Show("Patch applied !");

        }

        private void ButtonUnpatchClick(object sender, EventArgs e)
        {
            buttonUnpatch.Enabled = false;
            var dofusPath = FindDofusPath();

            // rechange the i18n file
            var i18nDir = Path.Combine(dofusPath, "data", "i18n");
            var i18n = new I18NFile(Path.Combine(i18nDir, "i18n_fr.d2i"));

            i18n.SetText("ui.link.changelog", Convert.ToBase64String(Resources.Empty));
            i18n.SetText("ui.login.forgottenPassword", "Mot de passe oublié");
            i18n.Update();

            // change the meta file
            var document = new XmlDocument();
            document.Load(Path.Combine(i18nDir, "data.meta"));
            var navigator = document.CreateNavigator();

            navigator.SelectSingleNode("//file[@name='i18n_fr.d2i']").SetValue(Cryptography.GetMD5Hash(File.ReadAllText(Path.Combine(i18nDir, "i18n_fr.d2i"))));

            document.Save(Path.Combine(i18nDir, "data.meta"));

            // reset the config
            var config = new XmlDocument();
            config.Load(Path.Combine(dofusPath, "config.xml"));
            var configNavigator = document.CreateNavigator();

            configNavigator.SelectSingleNode("//<entry [@key='connection.host']").SetValue(OfficialServerAddress);

            config.Save(Path.Combine(dofusPath, "config.xml"));

            // replace the patched swf by the original one
            File.WriteAllBytes(Path.Combine(dofusPath, "DofusInvoker.swf"), Resources.DofusInvokerOriginal);

            // clear the cache
            ClearCache();

            buttonUnpatch.Enabled = true;
            MessageBox.Show("Patch removed !");

        }

        private static void ClearCache()
        {
            string appData = Environment.GetEnvironmentVariable("appdata");
            string dofuscache = Path.Combine(appData, "Dofus 2");

            foreach (var file in Directory.EnumerateFiles(dofuscache, "*", SearchOption.TopDirectoryOnly))
            {
                File.Delete(file);
            }
        }

        private static string FindDofusPath()
        {
            string programFiles = Environment.GetEnvironmentVariable("programfiles(x86)");

            if (string.IsNullOrEmpty(programFiles))
                programFiles = Environment.GetEnvironmentVariable("programfiles");

            string dofusDataPath = Path.Combine(programFiles, "Dofus 2", "app");

            if (Directory.Exists(dofusDataPath))
                return dofusDataPath;

            return string.Empty;
        }
    }
}
