using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Messenger.Client.Utils
{
    public class BooleanToBrushConverter : DependencyObject, IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool && (bool)value) 
            { 
                return TrueBrush;
            }
            return FalseBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { 
            throw new NotImplementedException(); 
        }

        public Brush FalseBrush 
        {
            get
            {
                return (Brush)GetValue(FalseBrushProperty);
            } 
            set
            { 
                SetValue(FalseBrushProperty, value);
            }
        }

        public static readonly DependencyProperty FalseBrushProperty = DependencyProperty.Register("FalseBrush",
            typeof(Brush), typeof(BooleanToBrushConverter), new PropertyMetadata(null));

        public Brush TrueBrush 
        {
            get
            {
                return (Brush)GetValue(TrueBrushProperty);
            }
            set
            { 
                SetValue(TrueBrushProperty, value); 
            }
        }

        public static readonly DependencyProperty TrueBrushProperty = DependencyProperty.Register("TrueBrush", 
            typeof(Brush), typeof(BooleanToBrushConverter), new PropertyMetadata(null));
    }
}
