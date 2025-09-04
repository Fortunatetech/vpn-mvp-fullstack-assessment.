using System;
using System.Globalization;
using System.Windows.Data;
using VPNMvp.Models;

namespace VPNMvp.Converters
{
    public class NodeButtonLabelConverter : IMultiValueConverter
    {
        // values[0] = current node (item), values[1] = connected node (from VM)
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var node = values.Length > 0 ? values[0] as Node : null;
            var connected = values.Length > 1 ? values[1] as Node : null;

            if (node == null) return "Connect";
            if (connected != null && connected.Name == node.Name) return "Disconnect";
            return "Connect";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
