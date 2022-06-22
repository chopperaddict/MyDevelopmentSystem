using MyDev . Views;

using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Data;
using System . Windows . Documents;
using System . Windows . Input;
using System . Windows . Media;
using System . Windows . Media . Imaging;
using System . Windows . Navigation;
using System . Windows . Shapes;

namespace MyDev . UserControls
{
	public partial class Colorpicker : UserControl
	{
		// Events
		public static event EventHandler ExecuteMoveMethod;
		public static event EventHandler<ColorpickerArgs> ExecuteSaveToClipboardMethod;

		public bool Loading = true;
		#region Full Propeties for Binding
		private double  opacityValue;
		public double OpacityValue
		{
			get { return opacityValue; }
			set
			{
				opacityValue = value;
				Console . WriteLine ( $"Opacity set to {value}" );
			}
		}

		private  double redValue;
		public double RedValue
		{
			get { return redValue; }
			set
			{
				redValue = value;
				Console . WriteLine ( $"Red set to {value}" );
			}
		}

		private  double greenValue;
		public double GreenValue
		{
			get { return greenValue; }
			set { greenValue = value; }
		}

		private double   blueValue;
		public double BlueValue
		{
			get { return blueValue; }
			set { blueValue = value; }
		}

		private bool mouseCaptured;
		public bool MouseCaptured
		{
			get { return mouseCaptured; }
			set { mouseCaptured = value; }
		}

		public string RGBValueString
		{
			get { return ( string ) GetValue ( RGBValueStringProperty ); }
			set { SetValue ( RGBValueStringProperty , value ); }
		}
		public static readonly DependencyProperty RGBValueStringProperty=
			DependencyProperty.Register("RGBValueString", typeof(string), typeof(Colorpicker), new PropertyMetadata(""), RGBset);

		private static bool RGBset ( object value )
		{
			Console . WriteLine ( $"RGB is set to {value}" );
			return true;
		}
		#endregion  Full Properties for Binding

		public string OpacityValueString="";
		public string RedValueString="";
		public string GreenValueString="";
		public string BlueValueString="";
		//=============================================================================================================================================//
		public Colorpicker ( )
		{
			InitializeComponent ( );
			OpacitySlider . Value = 255;
			this . DataContext = this;
			RedSlider . Value = 210;
			GreenSlider . Value = 121;
			BlueSlider . Value = 161;
			Loading = false;
			Output . Refresh ( );
			Output . UpdateLayout ( );
			RedSlider . Focus ( );
			PopulateListbox ( );
			UpdateColorBlocks ( );
		}
		private void UserControl_Loaded ( object sender , RoutedEventArgs e )
		{
		}

		#region Sliders Handling
		private void RedSlider_ValueChanged ( object sender , RoutedPropertyChangedEventArgs<double> e )
		{
			if ( Loading )
				return;
			Color fill = Color . FromArgb ( Convert . ToByte ( OpacitySlider . Value ),
				Convert . ToByte ( RedSlider . Value ),
				Convert . ToByte ( GreenSlider . Value ),
				Convert . ToByte ( BlueSlider . Value ) );
			Output . Fill = new SolidColorBrush ( fill );
			Output . Refresh ( );
			RedValue = Convert . ToInt16 ( RedSlider . Value );
			RedValueString = RedValue . ToString ( );
			RGBValueString = "#" + ShowHexValues ( );
			fill = Color . FromArgb ( Convert . ToByte ( OpacitySlider . Value ) ,
				Convert . ToByte ( RedSlider . Value ) ,
				Convert . ToByte ( 0 ) ,
				Convert . ToByte ( 0 ) );
			RedOutput . Fill = new SolidColorBrush ( fill );
		}
		private void GreenSlider_ValueChanged ( object sender , RoutedPropertyChangedEventArgs<double> e )
		{
			if ( Loading )
				return;
			Color fill = Color . FromArgb ( Convert . ToByte ( OpacitySlider . Value ),
				Convert . ToByte ( RedSlider . Value ),
				Convert . ToByte ( GreenSlider . Value ),
				Convert . ToByte ( BlueSlider . Value ) );
			Output . Fill = new SolidColorBrush ( fill );
			Output . Refresh ( );
			GreenValue = Convert . ToInt16 ( GreenSlider . Value );
			GreenValueString = GreenValue . ToString ( );
			RGBValueString = "#" + ShowHexValues ( );
			fill = Color . FromArgb ( Convert . ToByte ( OpacitySlider . Value ) ,
				Convert . ToByte ( 0 ) ,
				Convert . ToByte ( GreenSlider . Value ) ,
				Convert . ToByte ( 0 ) );
			GreenOutput . Fill = new SolidColorBrush ( fill );
		}
		private void BlueSlider_ValueChanged ( object sender , RoutedPropertyChangedEventArgs<double> e )
		{
			if ( Loading )
				return;
			Color fill = Color . FromArgb ( Convert . ToByte ( OpacitySlider . Value ),
				Convert . ToByte ( RedSlider . Value ),
				Convert . ToByte ( GreenSlider . Value ),
				Convert . ToByte ( BlueSlider . Value ) );
			Output . Fill = new SolidColorBrush ( fill );
			Output . Refresh ( );
			BlueValue = Convert . ToInt16 ( BlueSlider . Value );
			BlueValueString = BlueValue . ToString ( );
			RGBValueString = "#" + ShowHexValues ( );
			fill = Color . FromArgb ( Convert . ToByte ( OpacitySlider . Value ) ,
				Convert . ToByte ( 0 ) ,
				Convert . ToByte ( 0 ) ,
				Convert . ToByte ( BlueSlider . Value ) );
			BlueOutput . Fill = new SolidColorBrush ( fill );
		}
		private void OpacitySlider_ValueChanged ( object sender , RoutedPropertyChangedEventArgs<double> e )
		{
			if ( Loading )
				return;
			Color fill = Color . FromArgb ( 
				Convert . ToByte ( OpacitySlider.Value),
				Convert . ToByte ( RedSlider . Value ),
				Convert . ToByte ( GreenSlider . Value ),
				Convert . ToByte ( BlueSlider . Value ) );
			if ( Output != null )
			{
				Output . Fill = new SolidColorBrush ( fill );
				Output . Refresh ( );
			}
			OpacityValue = Convert . ToInt16 ( OpacitySlider . Value );
			OpacityValueString = OpacityValue . ToString ( );
		RGBValueString = "#" + ShowHexValues ( );
  		}
		#endregion SlidersHandling
	
		#region External Hook to implement CopyToCllpBoard
		//code to allow this button to be handled by the parent window
		// Clever stuff really

		// Allows any other (External) window to control this via  the control and get called back to do whatever it wants with it. 
		//public static event EventHandler ExecuteSaveToClipboardMethod;
		protected virtual void OnExecuteMethod ( )
		{
			ColorpickerArgs args = new ColorpickerArgs();
			args . RgbString = RGBValueString;
			if ( ExecuteSaveToClipboardMethod != null )
				ExecuteSaveToClipboardMethod ( this , args );
		}

		private void ClipboardSave_Click ( object sender , RoutedEventArgs e )
		{
			//This is  the internal Click metho that the triggers the Event above to pass it on to the Parent window
			ColorpickerArgs args = new ColorpickerArgs();
			args . RgbString = RGBValueString;
			if ( ExecuteSaveToClipboardMethod != null )
				ExecuteSaveToClipboardMethod?.Invoke ( sender , args );

		}

		// Allow exit by hitting ESC
		private void colorPicker_PreviewKeyDown ( object sender , KeyEventArgs e )
		{
			if ( e . Key == Key . Escape )
				colorPicker . Visibility = Visibility . Hidden;
		}

		private void Output_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
		{
			//if ( sender != RedSlider && sender != GreenSlider && sender != BlueSlider && sender != OpacitySlider && sender != ClipboardSave )
			//{
			//	//				MouseCaptured = Output . CaptureMouse ( );
			//	//				Console . WriteLine ( "Mouse CAPTURED...Colorpicker_PreviewMouseLeftButtonDown()" );
			//	//				var v =e.Source;
			//	//				Slider s = v as Slider;
			//}
		}

		private void Output_PreviewMouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
		{
			//			ReleaseMouseCapture ( );
		}
		#endregion External Hook to implement CopyToCllpBoard

		#region External Hook
		//code to allow this action in flowdocto be handled by an external window
		// Clever stuff really

		// Allows any other (External) window to control this via  the control 
		protected virtual void OnExecuteMove ( )
		{
			if ( ExecuteMoveMethod != null )
				ExecuteMoveMethod ( this , EventArgs . Empty );
		}
		private void Image_PreviewMouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
		{
			//allows remote window to maximize /resize  this control ?
			OnExecuteMethod ( );
		}
		#endregion RemoteClipBoardEvent

		#region RemoteClipBoardEvent
		private void colorPicker_PreviewMouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
		{
			// Window wide method
			Output . ReleaseMouseCapture ( );
			//flowdoc . ReleaseMouseCapture ( );
//			Console . WriteLine ( "Mouse RELEASED...(Colorpicker_PreviewMouseLeftButtonUp" );

		}

		#endregion External Hook

		private void exitbtn_Click ( object sender , RoutedEventArgs e )
		{
			this . Visibility = Visibility . Hidden;
		}

		private void Image_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
		{
			this . Visibility = Visibility . Hidden;
		}

		#region setup support methods
		private string ShowHexValues ( )
		{
			// Called when a slider chages a value
			//Save colors on a per panel basis
			int r = ( int ) RedValue;
			int g = ( int ) GreenValue;
			int b = ( int ) BlueValue;
			if ( OpacityValue == 0 )
				OpacityValue = 255;
			int o = (int) OpacityValue ;
			string output = "";
			output = o . ToString ( "X2" );
			output += r . ToString ( "X2" );
			output += g . ToString ( "X2" );
			output += b . ToString ( "X2" );
			//opVal . Text = Convert . ToInt32 ( o ) . ToString ( "X2" );
			//			opRed . Text = Convert . ToInt32 ( r ) . ToString ( "X2" );
			//opGreen . Text = Convert . ToInt32 ( g ) . ToString ( "X2" );
			//opBlue . Text = Convert . ToInt32 ( b ) . ToString ( "X2" );
			return output;
		}
		private void UpdateColorBlocks ( )
		{
			var str = "#FF" + ((int)(RedSlider . Value)) . ToString ( "X2" ) + "0000";
			RedOutput . Fill = str . ToSolidBrush ( );
			str = "#FF00" + ( ( int ) ( GreenSlider . Value ) ) . ToString ( "X2" ) + "00";
			GreenOutput . Fill = str . ToSolidBrush ( );
			str = "#FF0000" + ( ( int ) ( BlueSlider . Value ) ) . ToString ( "X2" );
			BlueOutput . Fill = str . ToSolidBrush ( );
			RedOutput . Refresh ( );
			GreenOutput . Refresh ( );
			BlueOutput . Refresh ( );
			str = "#FF" + ( ( int ) ( RedSlider . Value ) ) . ToString ( "X2" );
			str += ( ( int ) ( GreenSlider . Value ) ) . ToString ( "X2" );
			str += ( ( int ) ( BlueSlider . Value ) ) . ToString ( "X2" );
			RGBValueString = str;
		}
		private void PopulateListbox ( )
		{
			listbox . Items . Add ( "Menu Background" );
			listbox . Items . Add ( "Menu Foreground" );
			listbox . Items . Add ( "Background - Mouseover" );
			listbox . Items . Add ( "Foreground - Mouseover" );
			listbox . SelectedIndex = 0;
			listbox . SelectedItem = 0;
		}
		#endregion setup support methods

		#region  Dependency properties for listbox
		#region Fontsize
		public double Fontsize
		{
			get { return ( double ) GetValue ( FontsizeProperty ); }
			set { SetValue ( FontsizeProperty , value ); }
		}
		public static readonly DependencyProperty FontsizeProperty =
			DependencyProperty.Register("Fontsize", typeof(double), typeof(Colorpicker), new PropertyMetadata((double)10));
		#endregion Fontsize

		#endregion  Dependency properties for listbox
	}
	// Event Arguments for the ClipBoard copy method used by Listviews
	public class ColorpickerArgs : EventArgs
	{
		public string RgbString
		{
			get; set;
		}

	}
}
