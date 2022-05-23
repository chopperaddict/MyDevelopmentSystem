using System;
using System . Collections . Generic;
using System . Diagnostics;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows . Data;

namespace MyDev . Converts
{

	// Call this when a binding is causing werid issues
	public class DebugConverter : IValueConverter
	{
		public object Convert ( object value , Type targetType , object parameter , System . Globalization . CultureInfo culture )
		{
//            Console . WriteLine ($"value = {value}, Parameter = {parameter}, TargetType={targetType}" );
			Debugger . Break ( );
			return value;
		}

		public object ConvertBack ( object value , Type targetType , object parameter , System . Globalization . CultureInfo culture )
		{
//			Console . WriteLine ( $"value = {value}, Parameter = {parameter}, TargetType={targetType}" );
			Debugger . Break ( );
			return value;
		}
	}
}
