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
			Flags . UseFlowdoc = Properties . Settings . Default . UseFlowDoc . ToUpper ( ) == "TRUE" ? true : false;
			Flags . UseScrollView = Properties. Settings. Default. UseScrollViewer. ToUpper ( ) == "TRUE" ? true : false;
			Flags. ReplaceFldNames = Properties. Settings. Default. ReplaceFldNames. ToUpper ( ) == "TRUE" ? true : false;
			chkbox1. IsChecked = Flags. UseFlowdoc;
			chkbox2 . IsChecked = Flags . ReplaceFldNames;
			chkbox3 . IsChecked = Flags . UseScrollView;
			Flags . UseMagnify = Properties . Settings . Default . UseMagnify ;
			chkbox4 . IsChecked = Flags . UseMagnify;
		}
		private void Setup()
		{
			//chkbox1_Checked  (null, null);
			//chkbox2_Checked  (null, null);
			//chkbox3_Checked  (null, null);
		}
		private void chkbox1_Click ( object sender , RoutedEventArgs e )
		{
		}
		private void chkbox3_Click ( object sender, RoutedEventArgs e )
		{
		}
		private void chkbox2_Click ( object sender , RoutedEventArgs e )
		{
		}

		private void chkbox1_Checked ( object sender , RoutedEventArgs e )
        {
			//Flags . UseFlowdoc = Properties . Settings . Default . UseFlowDoc == "TRUE" ? true : false;
			//if(Flags . UseFlowdoc)
			//	chkbox1 . IsChecked = true;
			//else
			//	chkbox1 . IsChecked = false;
		}
		private void chkbox3_Checked ( object sender , RoutedEventArgs e )
		{
			//Flags . UseScrollView = Properties . Settings . Default . UseScrollViewer == "TRUE" ? true : false;
			//if ( Flags . UseScrollView )
			//	chkbox3 . IsChecked = true;
			//else
			//	chkbox3 . IsChecked = false;
		}

		private void chkbox2_Checked ( object sender , RoutedEventArgs e )
		{
			//Flags . ReplaceFldNames = Properties . Settings . Default . ReplaceFldNames == "TRUE" ? true : false;
			//if ( Flags . ReplaceFldNames )
			//	chkbox2 . IsChecked = true;
			//else
			//	chkbox2 . IsChecked = false;
		}

		private void Closbtn_Click ( object sender , RoutedEventArgs e )
		{
			Properties . Settings . Default . UseFlowDoc = chkbox1 . IsChecked == true ? "TRUE" : "FALSE";
			Flags . UseFlowdoc = ( bool ) chkbox1 . IsChecked;
			Properties . Settings . Default . Save ( );
			Properties . Settings . Default . UseScrollViewer = chkbox3 . IsChecked == true ? "TRUE" : "FALSE";
			Flags . UseScrollView = ( bool ) chkbox3 . IsChecked;
			Properties . Settings . Default . Save ( );
			Properties . Settings . Default . ReplaceFldNames = chkbox2 . IsChecked == true ? "TRUE" : "FALSE";
			Flags . ReplaceFldNames = ( bool ) chkbox2 . IsChecked;
			Properties . Settings . Default . Save ( );
			Properties . Settings . Default . UseMagnify = ( bool ) chkbox4 . IsChecked;
			Properties . Settings . Default . Save ( );
			Flags . UseMagnify = (bool)chkbox4 . IsChecked;
			this . Close ( );
		}

        private void chkbox4_Checked ( object sender , RoutedEventArgs e )
        {
			Flags . UseMagnify = (bool)chkbox4 . IsChecked;
        }

        private void chkbox4_Click ( object sender , RoutedEventArgs e )
        {

        }
    }
}
