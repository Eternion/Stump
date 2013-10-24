using System.Collections.Generic;
using System.Windows;
using DBSynchroniser.Records;
using Stump.DofusProtocol.D2oClasses;

namespace WorldEditor.Editors.Items
{
    /// <summary>
    ///     Interaction logic for EffectEditorDialog.xaml
    /// </summary>
    public partial class EffectEditorDialog : Window
    {
        public static readonly DependencyProperty EffectToEditProperty =
            DependencyProperty.Register("EffectToEdit", typeof (EffectWrapper), typeof (EffectEditorDialog),
                                        new PropertyMetadata(default(EffectWrapper)));

        public static readonly DependencyProperty EffectsSourceProperty =
            DependencyProperty.Register("EffectsSource", typeof(IEnumerable<EffectRecord>), typeof(EffectEditorDialog),
                                        new PropertyMetadata(default(IEnumerable<EffectRecord>)));

        public EffectEditorDialog()
        {
            InitializeComponent();
        }

        public EffectWrapper EffectToEdit
        {
            get { return (EffectWrapper) GetValue(EffectToEditProperty); }
            set { SetValue(EffectToEditProperty, value); }
        }

        public IEnumerable<EffectRecord> EffectsSource
        {
            get { return (IEnumerable<EffectRecord>) GetValue(EffectsSourceProperty); }
            set { SetValue(EffectsSourceProperty, value); }
        }

        private void OnButtonOKClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnButtonCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}