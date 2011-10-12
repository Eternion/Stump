using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Worlds.Effects;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Tools.SpellsExplorer
{
    public partial class FormMain : Form
    {
        private DatabaseAccessor m_databaseAccessor;

        public FormMain()
        {
            InitializeComponent();
        }

        public string SpellSearchPattern
        {
            get { return textBoxSearch.Text; }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            groupBoxSelection.Enabled = false;
            groupBoxSpellInfo.Enabled = false;
        }

        private void OnShown(object sender, EventArgs e)
        {
            var thread = new Thread(Start) {IsBackground = true};
            thread.Start();
        }

        private void Start()
        {
            LogToConsole("Initializing Database...");
            m_databaseAccessor = new DatabaseAccessor(WorldServer.DatabaseConfiguration, Definitions.DatabaseRevision, typeof (WorldBaseRecord<>), typeof (WorldBaseRecord<>).Assembly);
            m_databaseAccessor.Initialize();

            LogToConsole("Opening Database...");
            m_databaseAccessor.OpenDatabase();

            LogToConsole("Loading texts...");
            TextManager.Instance.Intialize();

            LogToConsole("Loading effects...");
            EffectManager.Instance.Initialize();

            LogToConsole("Loading spells...");
            SpellManager.Instance.Initialize();

            RefreshSpellList();
        }

        public void LogToConsole(string log)
        {
            Invoke((Action) (() => richTextBoxConsoleLogs.AppendText(log + "\n")));
        }

        public void RefreshSpellList()
        {
            LogToConsole("Refresh spells list...");
            SpellTemplate[] spells = SpellManager.Instance.GetSpellTemplates().
                Where(entry => string.IsNullOrEmpty(SpellSearchPattern) || string.Compare(entry.Name, 0, SpellSearchPattern, 0, entry.Name.Length, true) >= 0).ToArray();

            Invoke((Action) (() =>
                                 {
                                     listBoxSpells.Items.Clear();
                                     listBoxSpells.Items.AddRange(spells);
                                     groupBoxSelection.Enabled = true;
                                 }));
        }

        private void OnSearch(object sender, EventArgs e)
        {
            RefreshSpellList();
        }

        private void OnSpellSelected(object sender, EventArgs e)
        {
            if (listBoxSpells.SelectedItem == null)
                return;

            ExploreSpell(listBoxSpells.SelectedItem as SpellTemplate);
        }

        public void ExploreSpell(SpellTemplate spell)
        {
            Invoke((Action)(() =>
            {
                groupBoxSpellInfo.Enabled = true;
                propertyGridSpell.SelectedObject = spell.SpellLevels[0];
            }));
        
        }
    }
}