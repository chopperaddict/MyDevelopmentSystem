using System;
using System . Collections . Generic;
using System . Globalization;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows . Controls;
using System . Windows . Data;

using static MyDev . Views . TreeViews;

namespace MyDev . Converts
{
    public class ExpanderArgumentsConverter : IMultiValueConverter
    {
        public object Convert ( object [ ] values , Type targetType , object parameter , CultureInfo culture )
        {
#pragma warning disable CS0219 // The variable 'eargs' is assigned but its value is never used
            ExpandArgs eargs = new ExpandArgs ( );
#pragma warning restore CS0219 // The variable 'eargs' is assigned but its value is never used
            return values;
            //eargs . tvitem = values [ 0 ] as TreeViewItem;
            //eargs . Levels = System.Convert . ToInt16 ( values [ 1 ] );
            //return ( object ) eargs;
        }

        public object [ ] ConvertBack ( object value , Type [ ] targetTypes , object parameter , CultureInfo culture )
        {
            throw new NotImplementedException ( );
        }
    }
}
