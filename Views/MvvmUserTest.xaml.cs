using MyDev . UserControls;
using MyDev . ViewModels;

using System;
using System . Collections . Generic;
using System . Data;
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
	/// Interaction logic for MvvmUserTest.xaml
	/// </summary>
	public partial class MvvmUserTest : Window
	{
		// Host window
		// Must declare the event
		public event RoutedEventHandler Click;
		StdDataUserControl SduCtrl { get; set; }
		MulltiDbUserControl MduCtrl { get; set; }
		Ucontrol1 U1ctrl { get; set; }
		public double Col1width=260;
		private double ButtonPanelMaxOffset = 155;
		private double ButtonPanelLeftOffset = 265;
		private double GridTopOffset = 300;
		string CurrentClient = "STD";
		//private double ButtonPanelLeftOffset = 245;
		public MvvmUserTest ( )
		{
			InitializeComponent ( );
			Utils . SetupWindowDrag ( this );

			this . DataContext = this;
			SduCtrl = stddatagrid;
			SduCtrl . SetParent ( this );
			MduCtrl = Multigrid;
			U1ctrl = Ucontrol1;
			this . DataContext = this;
			// setup the user control
			//SduCtrl . Visibility = Visibility . Visible;
			OpenStdControl ( this , null );
			//this . SizeChanged += MvvmUserTest_SizeChanged;
			UserTestWindow . SizeChanged += MvvmUserTest_SizeChanged;
			//this . Height += 1;
			//this . Width += 10;
			//bguv . canvas . Width = canvas . Width;
			//Setup handler to handle click event  from UserControl
			//			Click += new RoutedEventHandler ( CloseThisWindow );
			this . Title = "User Control Demonstratioin System - Standard Db's Viewer";

		}

		private void MvvmUserTest_SizeChanged ( object sender , SizeChangedEventArgs e )
		{
			// Handlle resizing of client user controls
			double height =  e . NewSize . Height;
			this . Grid1 . Height = height;
			this . canvas . Height=height;
			double width=(double) e . NewSize . Width;
			this . canvas . Width = width;

			// Std Viewer - HEIGHT - All Working
			if ( CurrentClient == "STD")
			{
				SduCtrl . Height = height - 70;
				SduCtrl . MAINgrid_Grid1 . Height = height;
				SduCtrl . dgcanvas . Height = height - 30;
				SduCtrl . dataGrid . Height = height - 120;
				// Std Viewer - WIDTH
				SduCtrl . Width = width - ButtonPanelLeftOffset - 30 ;
				SduCtrl . dgcanvas . Width = SduCtrl . Width + 60;// - ButtonPanelLeftOffset;
				SduCtrl . MAINgrid_Grid1 . Width = this . canvas . Width +160;
				SduCtrl . dataGrid . Width = SduCtrl . dgcanvas . Width - ButtonPanelLeftOffset;
				( SduCtrl . Top_rightcol as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) SduCtrl . dataGrid . Width);
				SduCtrl . Currentdb . Width = SduCtrl . dgcanvas . Width - ButtonPanelLeftOffset;
			}
			else if ( CurrentClient == "MULTI" )
			{
				////Handle Multi grid positioning
				Multigrid . Height = height - 70;
				Multigrid . MAIN_Grid1 . Height = height;
				Multigrid . bgcanvas . Height = height - 30;
				Multigrid . BankDataGrid . Height = height - 90;
				// Std Viewer - WIDTH
				Multigrid . Width = width - ButtonPanelLeftOffset - 30;
				Multigrid . bgcanvas . Width = Multigrid . Width;// - ButtonPanelLeftOffset;
				Multigrid . MAIN_Grid1 . Width = this . canvas . Width - ButtonPanelLeftOffset;
				Multigrid . BankDataGrid . Width = Multigrid . bgcanvas . Width - ButtonPanelLeftOffset;
				( Multigrid . bgcanvas as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) 0 );
				( Multigrid . BankDataGrid as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) 0 );
				( Multigrid . ButtonGroup as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) Multigrid . BankDataGrid . Width );
				Console . WriteLine ( $"MG Cv.Width {Multigrid . bgcanvas . Width },  " );
			}
			else
			{
				U1ctrl. Height = height - 70;
				U1ctrl . MainGrid . Height = height;
				U1ctrl . uccanvas . Height = height - 30;
				U1ctrl . listbox1 . Height = height - 140;
				// Std Viewer - WIDTH
				U1ctrl . Width = width - ButtonPanelLeftOffset - 30;
				U1ctrl . uccanvas . Width = U1ctrl . Width;// - ButtonPanelLeftOffset;
				U1ctrl . MainGrid . Width = this . canvas . Width - ButtonPanelLeftOffset;
				U1ctrl . listbox1 . Width = U1ctrl . uccanvas . Width - ButtonPanelLeftOffset;
				( U1ctrl . uccanvas as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) 0 );
				( U1ctrl . listbox1 as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) 0 );
				( U1ctrl . UiButtons as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) U1ctrl . listbox1 . Width );
				Console . WriteLine ( $"MG Cv.Width {U1ctrl . uccanvas . Width },  " );
			}
			// Handle the windows own buttons
			ButtonPanel . VerticalAlignment = VerticalAlignment . Bottom;
			if ( this . WindowState == WindowState . Maximized )
			{
				Console . WriteLine ( $"Canvas Max panel={canvas . Width}, Button AWidth={ButtonPanel . ActualWidth}" );
				canvas . Width = this . Width;
				double cheight = canvas.ActualHeight;
				( ButtonPanel as FrameworkElement ) . SetValue ( Canvas . TopProperty , ( double ) cheight +( ButtonPanel.Height - ButtonPanelMaxOffset ) );
				( ButtonPanel as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) this . ActualWidth - ButtonPanelLeftOffset );
			}
			else
			{
				( ButtonPanel as FrameworkElement ) . SetValue ( Canvas . TopProperty , ( double ) Height -310 );
				( ButtonPanel as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) Width - ButtonPanelLeftOffset );
			}

		}

		//private void OnClick ( object sender , MouseButtonEventArgs e )
		//{
		//	if ( this . Click != null )
		//	{
		//		this . Click ( this , e );
		//	}
		//}


		private void OpenStdControl ( object sender , RoutedEventArgs e )
		{
			CurrentClient = "STD";
			if ( canvas . Height == 0 )
				return;
			MduCtrl . Visibility = Visibility . Hidden;
			U1ctrl . Visibility = Visibility . Hidden;
			this . Height += 1;
			this . UpdateLayout ( );
			this. Refresh ( );
			if ( SduCtrl . Visibility == Visibility . Hidden )
			{
				SduCtrl . Visibility = Visibility . Visible;
				SduCtrl . Refresh ( );
				SduCtrl . Width -= 1;
				SduCtrl . UpdateLayout ( );
				SduCtrl . Height = this . canvas . Height - 70;//= canvas . Width;
				SduCtrl . Width = this . canvas . Width - 300;
				SduCtrl . UpdateLayout ( );
				this . Title = "User Control Demonstratioin System - Standard Db's Viewer";
			}
			else
				SduCtrl . Visibility = Visibility . Hidden;
		}

		private void OpenMultiDbControl( object sender , RoutedEventArgs e )
		{
			CurrentClient = "MULTI";
			U1ctrl . Visibility = Visibility . Hidden;
			SduCtrl . Visibility = Visibility . Hidden;
			MduCtrl . Width = 860;
			this . Height += 1;
			this . UpdateLayout ( );
			if ( MduCtrl . Visibility == Visibility . Hidden )
			{
				MduCtrl . Visibility = Visibility . Visible;
				MduCtrl . Width -= 1;
				MduCtrl . UpdateLayout ( );
				MduCtrl . Refresh ( );
				MduCtrl . Height= this . canvas . Height - 70;
				MduCtrl . Width = this . canvas . Width - 300;
				if ( MduCtrl . DbMain . Items . Count == 0 )
					MduCtrl . InitialLoad ( );
				MduCtrl . Width += 1;
				MduCtrl . UpdateLayout();
				this . Title = "User Control Demonstratioin System - Multi Db Viewer";
			}
			else
				MduCtrl . Visibility = Visibility . Hidden;
		}

		private void OpenDummy ( object sender , RoutedEventArgs e )
		{
			CurrentClient = "DUMMY";
			MduCtrl . Visibility = Visibility . Hidden;
			SduCtrl . Visibility = Visibility . Hidden;
			U1ctrl . Width = 860;
			this . Height += 1;
			this . UpdateLayout ( );
			if ( U1ctrl . Visibility == Visibility . Hidden )
			{
				U1ctrl . Visibility = Visibility . Visible;
				U1ctrl . Width -= 1;
				U1ctrl . UpdateLayout ( );
				U1ctrl . Refresh();
				U1ctrl . Height = this . canvas . Height - 70;//= canvas . Width;
				U1ctrl . Width = canvas . Width - 300;
				//				U1ctrl . uccanvas . Width = this.canvas.Width - 220;
				//UiButtons.
				U1ctrl . UpdateLayout ( );
				this . Title = "User Control Demonstratioin System - Dummy Grid Viewer";
			}
			else
			{
				U1ctrl . Visibility = Visibility . Hidden;
			}
		}

		private void UserTestWindow_ContentRendered ( object sender , EventArgs e )
		{
		}

		private void CloseThisWindow ( object sender , RoutedEventArgs e )
		{
			WindowCollection  v = Application .Current.Windows;
			foreach ( Window item in v )
			{
				if ( item . ToString ( ) . Contains ( "MvvmUserTest" ) )
				{
					MessageBoxResult res = MessageBox . Show ( "Close App down entirely ?" , "Application Closedown Options" ,
						MessageBoxButton .YesNoCancel, MessageBoxImage.Question, MessageBoxResult . Yes );
					if ( res == MessageBoxResult . Cancel )
						return;
					if ( res == MessageBoxResult . Yes )
						Application . Current . Shutdown ( );
					else
						item . Close ( );
					break;
				}
			}
		}
	}
}
