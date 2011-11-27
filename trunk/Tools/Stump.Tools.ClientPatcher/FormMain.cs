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
        public const string ServerPort = "443";

        public const string ProxyAddress = "localhost";
        public const string ProxyPort = "5555";

#else
        public const string ServerAddress = "176.31.230.164";
        public const string ServerPort = "443";

        public const string ProxyAddress = "176.31.230.164";
        public const string ProxyPort = "5554";
#endif

        public const string OfficialServerAddress = "213.248.126.180";
        public const string OfficialServerPort = "5555,443";

        public FormMain()
        {
            InitializeComponent();
        }

        private void OnFormMainLoad(object sender, EventArgs e)
        {
            textBoxDofusPath.Text = FindDofusPath();
        }

        private void ButtonBrowseClick(object sender, EventArgs e)
        {
            var browser = new FolderBrowserDialog();

            if (textBoxDofusPath.Text != string.Empty)
                browser.SelectedPath = textBoxDofusPath.Text;

            if (browser.ShowDialog() == DialogResult.OK)
            {
                textBoxDofusPath.Text = browser.SelectedPath;
            }
        }

        private void ButtonPatchClick(object sender, EventArgs e)
        {
            var dofusPath = textBoxDofusPath.Text;

            if (dofusPath == string.Empty)
            {
                MessageBox.Show(LangFile.Path_not_defined);
                return;
            }

            // remove the hidden code in the i18n file
            var i18nDir = Path.Combine(dofusPath, "data", "i18n");
            var i18n = new I18NFile(Path.Combine(i18nDir, "i18n_fr.d2i"));

            i18n.SetText("ui.link.changelog", Convert.ToBase64String(Resources.Empty));
            i18n.SetText("ui.login.forgottenPassword", "Client patched");

            try
            {
                i18n.Update();
            }
            catch (IOException)
            {
                MessageBox.Show(LangFile.Cannot_access_dofus_file);
                return;
            }

            // update the meta file
            var document = new XmlDocument();
            document.Load(Path.Combine(i18nDir, "data.meta"));
            var navigator = document.CreateNavigator();

            navigator.SelectSingleNode("//file[@name='i18n_fr.d2i']").SetValue(Cryptography.GetMD5Hash(File.ReadAllText(Path.Combine(i18nDir, "i18n_fr.d2i"))));

            document.Save(Path.Combine(i18nDir, "data.meta"));

            // change the config file
            var config = new XmlDocument();
            config.Load(Path.Combine(dofusPath, "config.xml"));
            var configNavigator = config.CreateNavigator();

            configNavigator.SelectSingleNode("//entry[@key='connection.host']").SetValue(ServerAddress); 
            configNavigator.SelectSingleNode("//entry[@key='connection.port']").SetValue(ServerPort);

            config.Save(Path.Combine(dofusPath, "config.xml"));

            // replace the swf file with the patched one
            File.WriteAllBytes(Path.Combine(dofusPath, "DofusInvoker.swf"), Resources.DofusInvokerModified);

            // clear the client cache
            ClearCache();

            MessageBox.Show(LangFile.Patch_applied);

        }

        private void ButtonUnpatchClick(object sender, EventArgs e)
        {
            var dofusPath = textBoxDofusPath.Text;

            if (dofusPath == string.Empty)
            {
                MessageBox.Show(LangFile.Path_not_defined);
                return;
            }

            // rechange the i18n file
            var i18nDir = Path.Combine(dofusPath, "data", "i18n");
            var i18n = new I18NFile(Path.Combine(i18nDir, "i18n_fr.d2i"));

            i18n.SetText("ui.link.changelog", Convert.ToBase64String(Resources.Empty));
            i18n.SetText("ui.login.forgottenPassword", "Mot de passe oublié");

            try
            {
                i18n.Update();
            }
            catch (IOException)
            {
                MessageBox.Show(LangFile.Cannot_access_dofus_file);
                return;
            }

            // change the meta file
            var document = new XmlDocument();
            document.Load(Path.Combine(i18nDir, "data.meta"));
            var navigator = document.CreateNavigator();

            navigator.SelectSingleNode("//file[@name='i18n_fr.d2i']").SetValue(Cryptography.GetMD5Hash(File.ReadAllText(Path.Combine(i18nDir, "i18n_fr.d2i"))));

            document.Save(Path.Combine(i18nDir, "data.meta"));

            // reset the config
            var config = new XmlDocument();
            config.Load(Path.Combine(dofusPath, "config.xml"));
            var configNavigator = config.CreateNavigator();

            configNavigator.SelectSingleNode("//entry[@key='connection.host']").SetValue(OfficialServerAddress);
            configNavigator.SelectSingleNode("//entry[@key='connection.port']").SetValue(OfficialServerPort);

            config.Save(Path.Combine(dofusPath, "config.xml"));

            // replace the patched swf by the original one
            File.WriteAllBytes(Path.Combine(dofusPath, "DofusInvoker.swf"), Resources.DofusInvokerOriginal);

            // clear the cache
            ClearCache();

            MessageBox.Show(LangFile.Patch_removed);

        }

        private void ButtonProxyPatchClick(object sender, EventArgs e)
        {
            var dofusPath = textBoxDofusPath.Text;

            if (dofusPath == string.Empty)
            {
                MessageBox.Show(LangFile.Path_not_defined);
                return;
            }

            // rechange the i18n file
            var i18nDir = Path.Combine(dofusPath, "data", "i18n");
            var i18n = new I18NFile(Path.Combine(i18nDir, "i18n_fr.d2i"));

            i18n.SetText("ui.link.changelog", Convert.ToBase64String(Resources.Empty));
            i18n.SetText("ui.login.forgottenPassword", "Mot de passe oublié");

            try
            {
                i18n.Update();
            }
            catch (IOException)
            {
                MessageBox.Show(LangFile.Cannot_access_dofus_file);
                return;
            }

            // change the meta file
            var document = new XmlDocument();
            document.Load(Path.Combine(i18nDir, "data.meta"));
            var navigator = document.CreateNavigator();

            navigator.SelectSingleNode("//file[@name='i18n_fr.d2i']").SetValue(Cryptography.GetMD5Hash(File.ReadAllText(Path.Combine(i18nDir, "i18n_fr.d2i"))));

            document.Save(Path.Combine(i18nDir, "data.meta"));

            // reset the config
            var config = new XmlDocument();
            config.Load(Path.Combine(dofusPath, "config.xml"));
            var configNavigator = config.CreateNavigator();

            configNavigator.SelectSingleNode("//entry[@key='connection.host']").SetValue(ProxyAddress);
            configNavigator.SelectSingleNode("//entry[@key='connection.port']").SetValue(ProxyPort);

            config.Save(Path.Combine(dofusPath, "config.xml"));

            // replace the patched swf by the original one
            File.WriteAllBytes(Path.Combine(dofusPath, "DofusInvoker.swf"), Resources.DofusInvokerOriginal);

            // clear the cache
            ClearCache();

            MessageBox.Show(LangFile.Proxy_patch_applied);
        }

        private static void ClearCache()
        {
            string appData = Environment.GetEnvironmentVariable("appdata");

            if (appData == string.Empty)
            {
                MessageBox.Show(LangFile.Cannot_clear_cache, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
