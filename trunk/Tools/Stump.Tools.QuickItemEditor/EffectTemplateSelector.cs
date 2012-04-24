using System.Windows;
using System.Windows.Controls;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Tools.QuickItemEditor
{
    public class EffectTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EffectBaseTemplate
        {
            get;
            set;
        }

        public DataTemplate EffectDiceTemplate
        {
            get;
            set;
        }

        public DataTemplate EffectIntegerTemplate
        {
            get;
            set;
        }

        public override DataTemplate SelectTemplate(object item,
                                                    DependencyObject container)
        {
            if (item is EffectDice)
            {
                return EffectDiceTemplate;
            }
            else if (item is EffectInteger)
            {
                return EffectIntegerTemplate;
            }

            return EffectBaseTemplate;
        }
    }
}