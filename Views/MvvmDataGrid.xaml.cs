using MyDev . Dapper;
using MyDev . Models;
using MyDev . Sql;
using MyDev . SQL;
using MyDev . ViewModels;

using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . Configuration;
using System . Data . SqlClient;
using System . Diagnostics;
using System . Runtime . CompilerServices;
using System . Runtime . InteropServices . WindowsRuntime;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Data;
using System . Windows . Input;
using System . Windows . Media;

namespace MyDev . Views
{
	/// <summary>
	/// Interaction logic for BankGridView.xaml
	/// This is my FIRST MVVM project so this only contains the most BASIC
	/// Code Behind.
	/// All the actual work is performed in the VIEWMODEL file - MVVMGRIDMODEL.CS
	/// </summary>
	public partial class MvvmDataGrid : Window
	{
		// Variables  for the Classes in use
		public static MvvmGridModel MvvmGridmodel { get; set; }
		private MvvmViewModel MvvM { get; set; }
		public static MvvmDataGrid BankGridViewWindow { get; set; }

		public MvvmDataGrid ( )
		{
			// WORKS WELL   			
			InitializeComponent ( );

			// Get  the base class loaded into variable
			MvvM = new MvvmViewModel ( this );
			this . DataContext = MvvM;

			MvvmGridmodel = MvvM . mvgm;
			//this . DataContext = MvvmGridmodel;

			// Sets it to x:Name "BankGV"
			BankGridViewWindow = this;
			Utils . SetupWindowDrag ( this );
		}


		private void BankGV_KeyDown ( object sender , KeyEventArgs e )
		{
			if ( e . Key == Key . F8 )
				Debugger . Break ( );
		}

		private void acfiltertext_TextChanged ( object sender , TextChangedEventArgs e )
		{

		}

		private void GetColumnNamesBtn_Click ( object sender , RoutedEventArgs e )
		{
			List<string> list = new List<string>();
			ObservableCollection<GenericClass> GenericClass = new ObservableCollection<GenericClass>(); 
			Dictionary<string, string> dict = new Dictionary<string, string>();
			// This returns a Dictionary<sting,string> PLUS a collection  and a List<string> passed by ref....
			dict = GenericDbHandlers . GetDbTableColumns ( ref GenericClass,  ref list, "BankAccount" , "IAN1");
			
			// Process data
			//foreach ( var item in dict )
			//{
			//	list . Add ( item . Key.ToString() );
			//	GenericClass gc = new GenericClass();
			//	gc . field1 = item . Key . ToString ( );
			//	gc . field2 = item . Value. ToString ( );
			//	GenericClass . Add ( gc );
			//}
			SqlServerCommands . LoadActiveRowsOnlyInGrid ( dataGrid2 , GenericClass , DapperSupport . GetGenericColumnCount ( GenericClass ) );
			//dataGrid2 . ItemsSource = GenericClass;
		}

		private void Flowdoc_LostFocus ( object sender , RoutedEventArgs e )
		{

		}

		private void Flowdoc_MouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
		{

		}

		private void Flowdoc_MouseMove ( object sender , MouseEventArgs e )
		{

		}

		private void LvFlowdoc_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
		{

		}
	}
}

