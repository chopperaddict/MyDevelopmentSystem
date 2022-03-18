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
	/// Interaction logic for SysConfig.xaml
	/// </summary>
	public partial class SysConfig : Window
	{
		public SysConfig ( )
		{
			InitializeComponent ( );
		}
		private void Setup()
		{
			chkbox1 . IsChecked = Flags . UseScrollView;
			chkbox2 . IsChecked = Flags . ReplaceFldNames ;
		}

		private void chkbox1_Checked ( object sender , RoutedEventArgs e )
		{
				Flags . UseScrollView = (bool)chkbox1 . IsChecked;
		}

		private void chkbox2_Checked ( object sender , RoutedEventArgs e )
		{
			Flags . ReplaceFldNames = ( bool ) chkbox2 . IsChecked;
		}

		private void Closbtn_Click ( object sender , RoutedEventArgs e )
		{
			this . Close ( );
		}
	}
}
