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
using System . Windows . Shapes;

namespace MyDev . Views
{
	/// <summary>
	/// Interaction logic for Setup.xaml
	/// </summary>
	public partial class Setup : Window
	{
		public Setup ( )
		{
			InitializeComponent ( );
			//			this . MouseDown += delegate { DoDragMove ( ); };
			Utils . SetupWindowDrag ( this );
			string ConnString = Flags . CurrentConnectionString;
			//			Connstring . Text = ( string ) Properties . Settings . Default [ "BankSysConnectionString" ];
			Connstring . Text = Connstring . Text . Trim ( );
			StartupWindow . Text = ( string ) Properties . Settings . Default [ "StartupWindow" ];

		}
		private void DoDragMove ( )
		{
			//Handle the button NOT being the left mouse button
			// which will crash the DragMove Fn.....
			try
			{
				this . DragMove ( );
			}
			catch
			{
				return;
			}
		}

		private void Close_Click ( object sender , RoutedEventArgs e )
		{
			Close ( );
		}

		private void Save_Click ( object sender , RoutedEventArgs e )
		{
			// save settings
		}

        private void UseMagnify_Click ( object sender , RoutedEventArgs e )
        {
			Flags . UseMagnify = (bool)UseMagnify . IsChecked;
			Properties . Settings . Default . UseMagnify = Flags . UseMagnify;
			Properties . Settings . Default . Save ( );
		}
	}
}
