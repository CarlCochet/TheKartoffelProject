using System;
using System.Windows.Markup;

namespace WorldEditor.Helpers.Markups
{
    public class EnumValuesExtension : MarkupExtension
    {
        private readonly Type m_enumType;

        public EnumValuesExtension(Type enumType)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Argument enumType must derive from type Enum.");
            }
            m_enumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(m_enumType);
        }
    }
}
