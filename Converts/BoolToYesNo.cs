﻿using System;
using System . Collections . Generic;
using System . Globalization;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows . Data;

namespace MyDev . Converts
{
    public class BoolToYesNo : IValueConverter
	{
		string result = "";
		public object Convert ( object value , Type targetType , object parameter , CultureInfo culture )
		{
			string result = "";
			if ( ( bool ) value == true )
				result  = "Yes";
			else
				result  = "No";
			if ( parameter != null && parameter . ToString ( ) == "NOT" )
            {
				if ( result == "Yes" )
					result  = "No";
				else
					result = "Yes";
			}
			value = (object)result;
			return value;
		}

		public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture )
		{
			throw new NotImplementedException ( );
		}
	}
}
