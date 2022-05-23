using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows . Threading;
using System . Windows;
using MyDev . ViewModels;
using System . Windows . Media;

namespace MyDev
{
	public static class ExtensionMethods
	{
		private static Action EmptyDelegate = delegate ( ) { };
		public static void CW ( this string message )
		{
			Console . WriteLine ( message );
		}
		public static void cwerror ( this string message )
		{
			Console . WriteLine ( $"ERROR : {message}" );
		}
		public static void cwwarn( this string message )
		{
			Console . WriteLine ( $"WARNING: {message}" );
		}
		public static void cwinfo( this string message )
		{
			Console . WriteLine ( $"INFO : {message}" );
		}
		public static void Refresh ( this UIElement uiElement )
			{
				try
				{
				uiElement . Dispatcher . Invoke ( DispatcherPriority . Render , EmptyDelegate );
			}
			catch
			{

			}
		}
		public static Brush ToSolidColorBrush ( this string HexColorString )
		{
			if ( HexColorString . Length < 9 )
			{
				//				MessageBox.Show( "The Hex value entered is invalid. It needs to be # + 4 hex pairs\n\neg: [#FF0000FF] = BLUE ");
				return null;
			}
			try
			{
				if ( HexColorString != null && HexColorString != "" )
					return ( SolidColorBrush ) System . Windows . Application . Current . FindResource(HexColorString);
				else
					return null;
			}
			catch ( Exception ex )
			{
				Console . WriteLine ( $"ToSolidColorBrush failed - input = {HexColorString}" );
				return null;
			}
		}

		public static Brush ToSolidBrush ( this string HexColorString )
		{
			if ( HexColorString . Length < 9 )
			{
				//				MessageBox.Show( "The Hex value entered is invalid. It needs to be # + 4 hex pairs\n\neg: [#FF0000FF] = BLUE ");
				return null;
			}
			try
			{
				if ( HexColorString != null && HexColorString != "" )
					return ( Brush ) ( new BrushConverter ( ) . ConvertFrom ( HexColorString ) );
				else
					return null;
			} catch ( Exception ex )
			{
				Console . WriteLine ( $"ToSolidbrush failed - input = {HexColorString}" );
				return null;
			}
		}
		public static LinearGradientBrush ToLinearGradientBrush ( this string Colorstring )
		{
			try
			{
				return Application . Current . FindResource ( Colorstring ) as LinearGradientBrush;
			} catch ( Exception ex )
			{
				Console . WriteLine ( $"ToLinearGradientbrush failed - input = {Colorstring}" );
				return null;
			}
			//return ( LinearGradientBrush ) ( new BrushConverter ( ) . ConvertFrom ( color ) );
		}
		public static string BrushtoText ( this Brush brush )
		{
			try
			{
				if ( brush != null )
					return ( string ) brush . ToString ( );
				else
					return null;
			} catch ( Exception ex )
			{
				Console . WriteLine ( $"BrushtoText failed - input = {brush }" );
				return null;
			}
		}
		public static string ToBankRecordCommaDelimited ( this BankAccountViewModel record )
		{
			BankAccountViewModel bvm = new  BankAccountViewModel();
			string [] fields ={ "","","","","","","","",""};
			fields [ 0 ] = record . Id . ToString ( );
			fields [ 1 ] = record . BankNo . ToString ( );
			fields [ 2 ] = record . CustNo . ToString ( );
			fields [ 3 ] = record . Balance . ToString ( );
			fields [ 4 ] = record . IntRate . ToString ( );
			fields [ 5 ] = record . AcType . ToString ( );
			fields [ 6 ] = record . ODate . ToString ( );
			fields [ 7 ] = record . CDate . ToString ( );
			return fields [ 0 ] + "," + fields [ 1 ] + "," + fields [ 2 ] + "," + fields [ 3 ] + "," + fields [ 4 ] + "," + fields [ 5 ] + "," + fields [ 6 ] + "," + fields [ 7 ] + "\n";
		}
	}

}