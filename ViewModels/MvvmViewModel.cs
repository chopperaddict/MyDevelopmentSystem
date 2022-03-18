﻿using MyDev . Models;
using MyDev . UserControls;
using MyDev . Views;

using System;
using System . Collections . Generic;
using System . Data . SqlClient;
using System . Diagnostics;
using System . Linq;
using System . Security . Policy;
using System . Text;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Data;
using System . Windows . Input;
using System . Xml . Linq;

namespace MyDev . ViewModels
{
	/// <summary>
	/// VIEWMODEL for MvvmDataGrid.
	///  Also sends lower level requests to MvvmGridView for data access etc
	///   This only handles ICommands and Binding variables for the View itself
	/// </summary>
	public class MvvmViewModel : BaseViewModel
	{
		internal MvvmGenericModel mvvm { get; set; }
		internal MvvmGridModel mvgm { get; set; }
		public MvvmDataGrid ParentBGView { get; set; }
		public FlowDoc Flowdoc { get; set; }
		public FlowdocLib fdl { get; set; }
		public Canvas canvas { get; set; }
		public bool IsBankActive { get; set; } = true;

		public ICommand debugger { get; set; }
		public ICommand CloseWindow { get; set; }
		public ICommand LoadData { get; set; }

		private  bool UseFlowdoc = true;
		public object MovingObject { get; set; }

		#region Full Properties
		private string fillterlabel;
		public string FilterLabel
		{
			get { return fillterlabel; }
			set { fillterlabel = value; OnPropertyChanged ( FilterLabel ); }
		}
		private string  acFilterLabel;
		public string AcFilterLabel
		{
			get { return acFilterLabel; }
			set { acFilterLabel = value; OnPropertyChanged ( AcFilterLabel ); }
		}
		#region Filter TextBoxes
		private string  filtertextbox;
		public string FilterTextBox
		{
			get { return filtertextbox; }
			set { filtertextbox = value; OnPropertyChanged ( FilterTextBox ); }
		}
		private string  acFilterTextBox;
		public string ACFilterTextBox
		{
			get { return acFilterTextBox; }
			set { acFilterTextBox = value; OnPropertyChanged ( ACFilterTextBox ); }
		}
		#endregion Filter TextBoxes

		private string  loadbuttontext;
		public string LoadButtonText
		{
			get { return loadbuttontext; }
			set { loadbuttontext = value; OnPropertyChanged ( LoadButtonText ); }
		}
		private string activeTable;
		public string ActiveTable
		{
			get { return activeTable; }
			set { activeTable = value; OnPropertyChanged ( ActiveTable ); }
		}

		#endregion Full Properties
		public MvvmViewModel ( )
		{
			CloseWindow = new RelayCommand ( ExecuteCloseWindow , CanExecuteCloseWindow );

		}
		public MvvmViewModel ( object caller )
		{
			// Get pointers  to  View and Model Class
			ParentBGView = caller as MvvmDataGrid;
			mvgm = new MvvmGridModel ( this );

			// Handle ICommands
			debugger = new RelayCommand ( ExecuteDebugger , CanExecuteDebugger );
			LoadData = new RelayCommand ( ExecuteLoadData , CanExecuteLoadData );
			//Setup Edit label annd button text as it switches from Bank to Customer
			FilterLabel = "Filter Bank A/c's Col : CustNo";
			LoadButtonText = "Load Customer A/cs";
			ActiveTable = "All Bank Accounts";
			// Handle  filtering by calling MvvmGridModel to process it
			ParentBGView . filtertext . TextChanged += mvgm . filter_TextChanged;
			ParentBGView . acfiltertext . TextChanged += mvgm . acfilter_TextChanged;

			fdl = new FlowdocLib();
		}
		#region ICommand Methods 
		// ICommand CanExecute's
		private bool CanExecuteLoadData ( object arg )
		{ return true; }
		public bool CanExecuteDebugger ( object arg )
		{
			return true;
		}
		public bool CanExecuteCloseWindow ( object parameter )
		{ return true; }

		// IComand Handelrs
		public void ExecuteDebugger ( object obj )
		{ mvgm . ExecuteDebugger ( null ); }
		public void ExecuteCloseWindow ( object parameter )
		{
			//			Console . WriteLine ( "We have Hit the close ICommand ..." );
			WindowCollection  v = Application .Current.Windows;
			foreach ( Window item in v )
			{
				if ( item . ToString ( ) . Contains ( "MvvmDataGrid" ) )
				{
					MessageBoxResult res = MessageBox . Show ( "Close App down entirely ?" , "Application Closedown Options" , MessageBoxButton . YesNoCancel , MessageBoxImage. Question , MessageBoxResult . Yes );
					if ( res == MessageBoxResult . Yes )
						Application . Current . Shutdown ( );
					else if ( res == MessageBoxResult . No )
						item . Close ( );

					break;
				}
			}
			//Application . Current . Shutdown ( );
		}

		private void ExecuteLoadData ( object obj )
		{
			ParentBGView . filtertext . Text = "";
			if ( IsBankActive == true )
			{
				// Call MvvmGridModel to actually get the data from SQL Db
				mvgm . LoadData ( false );
				IsBankActive = false;
				// Reset labels & button text in filter box
				FilterLabel = "Filter Customer A/c's Col : CustNo";
				LoadButtonText = "Load Bank A/cs";
				ActiveTable = "All Customer Details";
			}
			else
			{
				// Call MvvmGridModel to actually get the data from SQL Db
				mvgm . LoadData ( true );
				IsBankActive = true;
				FilterLabel = "Filter Bank A/c's Col : CustNo";
				LoadButtonText = "Load Customer A/cs";
				ActiveTable = "All Bank Accounts";
			}
			ShowInfo (Flowdoc, canvas, line1: $"The requ" , clr1: "Black0" ,
				line2: $"The command line used was" , clr2: "Red2" ,
				header: "Generic style data table" , clr4: "Red5" );
		}
		#endregion ICommand Methods EXECUTEDEBUGGER, EXECUTECLOSEWINDOW

		public void ShowRecordData ( DataGrid dgrid)
		{
			if ( IsBankActive )
			{
				BankAccountViewModel bvm = dgrid.SelectedItem as   BankAccountViewModel;
				string data = "Record Contents :-\n";
				data += "Customer # :	" + bvm . CustNo+ "\n";
				data += "Bank A/C  # :	" + bvm . BankNo + "\n";
				data += "A/c Type :	" + bvm . AcType+ "\n";
				data +="Balance :	" +  bvm .Balance + "\n";
				data +="Interest  rate :	" +  bvm .IntRate + "\n";
				data +="Date opened :	" +  bvm .ODate + "\n";
				data += "Date Closed:	" + bvm .CDate + "\n";
				fdmsg ( "testing flowdoc from MvvmViewModel", data);
			}

		}
		public void fdmsg ( string line1 , string line2 = "" , string line3 = "" )
		{
			//We have to pass the Flowdoc.Name, and Canvas.Name as well as up   to 3 strings of message
			//  you can  just provie one if required
			// eg fdmsg("message text");
			fdl . FdMsg ( Flowdoc , canvas , line1 , line2 , line3 );
		}

		private void ShowInfo (FlowDoc Flowdoc, Canvas canvas, string line1 = "" , string clr1 = "" , string line2 = "" , string clr2 = "" , string line3 = "" , string clr3 = "" , string header = "" , string clr4 = "" , bool beep = false )
		{
			if ( UseFlowdoc == false )
				return;
			ShowInfo (	Flowdoc ,canvas ,line1 ,clr1,line2,clr2,line3,clr3,header ,clr4,beep );

		}
	}
}