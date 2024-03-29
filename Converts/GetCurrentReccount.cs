﻿using System;
using System. Collections. Generic;
using System. Globalization;
using System. Linq;
using System. Text;
using System. Threading. Tasks;
using System. Windows;
using System. Windows. Controls;
using System. Windows. Data;

using MyDev. Views;

namespace MyDev. Converts
{
    public class GetCurrentReccount : IValueConverter
    {
        public object Convert ( object value, Type targetType, object parameter, CultureInfo culture )
        {
			double currentvalue = 0;
#pragma warning disable CS0219 // The variable 'offset' is assigned but its value is never used
			double offset = 0;
#pragma warning restore CS0219 // The variable 'offset' is assigned but its value is never used
			double d = 0;
			Type t = targetType;

			d = ( int) value;
            if ( parameter != null && value != null )
            {
                ModernViews win = parameter as ModernViews;
                if ( d == 0 || win == null )
                    return value;
                ListView lv = new ListView ( );
                if ( lv == null )
                    return value;
                else
                {
                    lv = win. lview3;
                    if ( win. lview3. Visibility == Visibility. Visible )
                        currentvalue = win. RecCountlv;
                    else if ( win. lbox1. Visibility == Visibility. Visible )
                        currentvalue = win. RecCountlb;
                    else if ( win. Dgrid1. Visibility == Visibility. Visible )
                        currentvalue = win. RecCountdg;

                    return currentvalue;
                }
            }
            //else
            return d;
		}

		public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException ( );
        }
    }
}
