using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using WorldEditor.Loaders.Data;
using WorldEditor.Loaders.Database;
using WorldEditor.Loaders.I18N;
using WorldEditor.Loaders.Objetcs;

namespace WorldEditor.Helpers.Converters
{
    public class ItemTypeIdToTypeNameConverter : IValueConverter
    {
        private static bool m_initialized;
        private static Dictionary<int, ItemTypeRecord> m_types;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ItemTypeIdToTypeNameConverter.m_initialized)
            {
                m_initialized = true;
                ItemTypeIdToTypeNameConverter.Initialize();
            }
            if (value is uint)
            {
                value = (int)((uint)value);
            }
            ItemTypeRecord type;
            return ItemTypeIdToTypeNameConverter.m_types.TryGetValue((int)value, out type) ? I18NDataManager.Instance.ReadText(type.NameId) : value.ToString();
        }

        private static void Initialize()
        {
            ItemTypeIdToTypeNameConverter.m_types = ObjectDataManager.Instance.Query<ItemTypeRecord>("SELECT * FROM items_types").ToDictionary(entry => entry.Id);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
