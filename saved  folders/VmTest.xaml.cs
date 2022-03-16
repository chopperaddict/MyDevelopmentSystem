using MyDev . ViewModels;

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
	/// Interaction logic for VmTest.xaml
	/// </summary>
	public partial class VmTest : Window
	{
		public VmTest ( )
		{
			InitializeComponent ( );
			this . DataContext = new Class1 ( );
		}

		private void TestViewModel ( object sender , RoutedEventArgs e )
		{

		}

		private void Close_Btn ( object sender , RoutedEventArgs e )
		{
			this . Close ( );
		}

		private void App_Close ( object sender , RoutedEventArgs e )
		{
			Application . Current . Shutdown ( );
		}

		private void treeViewModel_SelectedItemChanged ( object sender , RoutedPropertyChangedEventArgs<object> e )
		{

		}

		private void treeViewModel_Expanded ( object sender , RoutedEventArgs e )
		{

		}

		private void treeViewModel_Collapsed ( object sender , RoutedEventArgs e )
		{

		}

		private void treeViewModel_Selected ( object sender , RoutedEventArgs e )
		{

		}
	}
}
