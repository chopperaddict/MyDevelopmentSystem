﻿
using MyDev . Dapper;
using MyDev . Sql;
using MyDev . SQL;
using MyDev . ViewModels;

using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . ComponentModel;
using System . Data;
using System . Data . SqlClient;
using System . Diagnostics;
using System . Linq;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Data;
using System . Windows . Input;
using System . Windows . Media;
using System . Windows . Threading;

namespace MyDev . Views
{
	public partial class Datagrids : Window, INotifyPropertyChanged
	{

		#region OnPropertyChanged
		new public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged ( string PropertyName )
		{
			if ( this . PropertyChanged != null )
			{
				var e =  new PropertyChangedEventArgs ( PropertyName );
				this . PropertyChanged ( this , e );
			}
		}
		/// <summary>
		/// Warns the developer if this object does not have
		/// a public property with the specified name. This
		/// method does not exist in a Release build.
		/// </summary>
		[Conditional ( "DEBUG" )]
		[DebuggerStepThrough]
		public virtual void VerifyPropertyName ( string propertyName )
		{
			// Verify that the property name matches a real,
			// public, instance property on this object.
			if ( TypeDescriptor . GetProperties ( this ) [ propertyName ] == null )
			{
				string msg = "Invalid property name: " + propertyName;

				if ( this . ThrowOnInvalidPropertyName )
					throw new Exception ( msg );
				else
					Debug . Fail ( msg );
			}
		}

		/// <summary>
		/// Returns whether an exception is thrown, or if a Debug.Fail() is used
		/// when an invalid property name is passed to the VerifyPropertyName method.
		/// The default value is false, but subclasses used by unit tests might
		/// override this property's getter to return true.
		/// </summary>
		protected virtual bool ThrowOnInvalidPropertyName
		{
			get; private set;
		}

		#endregion OnPropertyChanged

		#region  Public variables
		// Set  up our data collections

		// Individual records
		public BankAccountViewModel bvm = new BankAccountViewModel();
		public CustomerViewModel cvm = new CustomerViewModel ();
		public DetailsViewModel dvm = new DetailsViewModel();
		public GenericClass  gvm = new GenericClass  ();

		// Collections
		public ObservableCollection<BankAccountViewModel> bankaccts = new ObservableCollection<BankAccountViewModel>();
		public ObservableCollection<CustomerViewModel> custaccts = new ObservableCollection<CustomerViewModel>();
		public ObservableCollection<DetailsViewModel> detaccts = new ObservableCollection<DetailsViewModel>();
		public ObservableCollection<GenericClass> genaccts = new ObservableCollection<GenericClass>();

		// supporting sources
		public List<string> TablesList = new List<string>();

		// internal Flag data
		private string CurrentType= "BANKACCOUNT";
		//		private string [ ] ACTypes = {"BANK", "CUSTOMER", "DETAILS", "NWCUSTOMER", "NWCUSTLIMITED", "GENERIC"};
		//		private string [ ] DefaultTables = {"BANKACCOUNT", "CUSTOMER", "SECACCOUNTS", "CUSTOMERS", "GENERICS"};
		private string SqlCommand="";
		private string DefaultSqlCommand="Select * from BankAccount";
		string Nwconnection = "NorthwindConnectionString";

		#endregion  Public variables

		#region private variables
		private bool UseDirectLoad = true;
		private bool UseBGThread= true;
		private bool LoadDirect=false;
		// pro temp variables
		private bool UseFlowdoc=true;
		private bool UseFlowdocBeep=false;
		private bool showall=false;
		private  bool ShowFullScript = false;
		private bool LoadAll = false;
		private bool Usetimer = true;
		private bool UseScrollViewer= true;
		private static Stopwatch timer = new Stopwatch();
		#endregion private variables

		#region Binding full props
		// Full properties used in Binding to I/f objects
		private int dbcount;
		public int DbCount
		{
			get { return dbcount; }
			set { dbcount = value; OnPropertyChanged ( "DbCount" ); }
		}

		private bool ismouseDown;
		public bool isMouseDown
		{
			get { return ismouseDown; }
			set { ismouseDown = value; }
		}
		private object movingobject;
		public object MovingObject
		{
			get { return movingobject; }
			set { movingobject = value; }
		}

		private double FirstXPos=0;
		private double FirstYPos=0;

		#endregion Binding full props

		#region Startup/Close
		public Datagrids ( )
		{
			InitializeComponent ( );
			this . DataContext = this;
			// setup check boxes & ListBox
			LoadViaSqlCmd . IsChecked = false;
			BgWorker . IsChecked = true;
			UseBGThread = true;
			UseFlowdoc = true;
			Usetimer = true;
			UseTimer . IsChecked = true;
			Flags . UseScrollView = false;
			// Set flags so we get the right SQL command method used...
			UseDirectLoad = true;
			SqlCommand = DefaultSqlCommand;
		}

		private void Window_Loaded ( object sender , RoutedEventArgs e )
		{
			// Set up notification from "Normal" Db Loading system
			EventControl . BankDataLoaded += EventControl_BankDataLoaded;
			EventControl . CustDataLoaded += EventControl_CustDataLoaded;
			EventControl . DetDataLoaded += EventControl_DetDataLoaded;
			EventControl . GenDataLoaded += EventControl_GenDataLoaded;
			this . SizeChanged += Datagrids_SizeChanged;

			// set global flag so we can access it via this pointer
			Flags . dataGrids = this;
			Showinfo . IsChecked = true;

			LoadTablesList ( );
			LoadSpList ( );
			SqlCommand = "Select * from BankAccount";
			LoadData ( );
			Datagrids_SizeChanged ( sender , null );
		}
		private void Datagrids_SizeChanged ( object sender , SizeChangedEventArgs e )
		{
			//Info . Width = Col2 . ActualWidth + Col3 . ActualWidth;

		}

		private void Window_Closing ( object sender , CancelEventArgs e )
		{
			EventControl . BankDataLoaded -= EventControl_BankDataLoaded;
			EventControl . CustDataLoaded -= EventControl_CustDataLoaded;
			EventControl . DetDataLoaded -= EventControl_DetDataLoaded;
			EventControl . GenDataLoaded -= EventControl_GenDataLoaded;
		}
		private void App_Close ( object sender , RoutedEventArgs e )
		{
			this . Close ( );
			Application . Current . Shutdown ( );
		}
		private void Datagrids_Close ( object sender , RoutedEventArgs e )
		{
			this . Close ( );
		}
		#endregion Startup/Close

		#region EventControl data loaded methods
		private void EventControl_BankDataLoaded ( object sender , LoadedEventArgs e )
		{
			// Works - 2/2/22
			// Received notification from Bank Load system via an Event
			bankaccts = e . DataSource as ObservableCollection<BankAccountViewModel>;
			DbCount = bankaccts . Count;
			LoadGrid ( );
			ShowInfo ( $"The request for the default Bank Accounts table [{CurrentType}] was successful, and the {DbCount} results returned are shown in the datagrid ..." ,
				  "Blue3" ,
				  "" ,
				  "" ,
				  "" ,
				  "" ,
				   "Default Bank Account data table" ,
				    "Red3" );
			Grid1 . SelectedIndex = 0;
			Grid1 . Focus ( );
			//ShowLoadtime ( timer . ElapsedMilliseconds );
		}
		private void EventControl_CustDataLoaded ( object sender , LoadedEventArgs e )
		{
			// Works - 2/2/22
			// Received notification from Bank Load system via an Event
			custaccts = e . DataSource as ObservableCollection<CustomerViewModel>;
			DbCount = custaccts . Count;
			LoadGrid ( );
			ShowInfo ( line1: $"The request for the default Customer Accounts table [{CurrentType}] was successful, and the {DbCount} results returned are shown in the datagrid..." , "Orange0" , header: "Default Customers data table" );
			Grid1 . SelectedIndex = 0;
			Grid1 . Focus ( );
			//ShowLoadtime ();
		}
		private void EventControl_DetDataLoaded ( object sender , LoadedEventArgs e )
		{
			// Works - 2/2/22
			// Received notification from Bank Load system via an Event
			detaccts = e . DataSource as ObservableCollection<DetailsViewModel>;
			DbCount = detaccts . Count;
			LoadGrid ( );
			ShowInfo ( line1: $"The request for the default Secondary Accounts table [{CurrentType}] was successful, and the {DbCount} results returned are shown in the datagrid ..." , clr1: "Green0" , header: "Default Secondary Bank Accounts data table" );
			Grid1 . SelectedIndex = 0;
			Grid1 . Focus ( );
			//ShowLoadtime ( );
		}
		private void EventControl_GenDataLoaded ( object sender , LoadedEventArgs e )
		{
			// Works - 2/2/22
			// Received notification from Bank Load system via an Event
			genaccts = e . DataSource as ObservableCollection<GenericClass>;
			DbCount = genaccts . Count;
			LoadGrid ( );
			ShowInfo ( line1: $"The requested Generic table type [{CurrentType}] request succeeded, and the results are shown in the datagrid ..." , "header:Generic data table" );
			Grid1 . SelectedIndex = 0;
			Grid1 . Focus ( );
			//ShowLoadtime ( );
		}
		#endregion EventControl data loaded methods

		#region Checkbox/Combo handlers

		private void Autoload_Click ( object sender , RoutedEventArgs e )
		{
			UseDirectLoad = LoadViaSqlCmd . IsChecked == true ? true : false;
			if ( UseDirectLoad )
			{
				BgWorker . IsChecked = false;
				UseBGThread = false;
			}
			else
			{
				BgWorker . IsChecked = true;
				UseBGThread = true;
			}
		}
		private void BgWorker_Click ( object sender , RoutedEventArgs e )
		{
			UseBGThread = BgWorker . IsChecked == true ? true : false;
			if ( UseBGThread )
			{
				UseDirectLoad = false;
				LoadViaSqlCmd . IsChecked = false;
			}
			else
			{
				UseDirectLoad = true;
				LoadViaSqlCmd . IsChecked = true;
			}
		}
		private void dbName_SelectionChanged ( object sender , SelectionChangedEventArgs e )
		{
			ComboBox cb = sender as ComboBox;
			if ( cb . Items . Count == 0 )
				return;
			CurrentType = cb . SelectedItem . ToString ( ) . ToUpper ( );
		}
		private void dbName_SelectionChanged_1 ( object sender , SelectionChangedEventArgs e )
		{

		}

		#endregion Checkbox/Combo handlers

		#region Data Handling

		// TRIGGER method for all load requests
		private void ReloadDatagrids ( object sender , RoutedEventArgs e )
		{
			int max=0;
			//ShowInfo ( "" );
			// Load Db based on Parameters entered by user
			var result = int . TryParse ( RecCount . Text , out max);
			SqlCommand = GetSqlCommand ( max , dbName . SelectedIndex , "" , "" );
			bankaccts = new ObservableCollection<BankAccountViewModel> ( );
			Grid1 . ItemsSource = null;
			Grid1 . Refresh ( );
			//ShowInfo ( info: $"Processing command [{SqlCommand}] ..." );
			LoadData ( );
		}
		private void LoadData ( )
		{
			if ( UseBGThread )
			{
				// This calls various methods that run on a Background Thread
				if ( SqlCommand . Contains ( " " ) == false || SqlCommand . ToUpper ( ) . Trim ( ) . Substring ( 0 , 2 ) == "SP" )
				{
					// process a Stored procedure
					DataLoadControl . GetDataTable ( SqlCommand );
				}
				else
				{     //process any other type of cmomand
					SqlCommand = CheckLimits ( );
					BackgroundWorkerLoad ( );
				}
			}
			else
			{
				if ( LoadDirect )
				{
					if ( Usetimer )
						timer . Start ( );
					if ( CurrentType == "BANK" )
					{
						DataTable dt = DataLoadControl . GetDataTable (SqlCommand );
						SqlSupport . LoadBankCollection ( dt , true );
					}
					if ( CurrentType == "CUSTOMER" )
					{
						DataTable dt = DataLoadControl . GetDataTable (SqlCommand );
						SqlSupport . LoadCustomerCollection ( dt , true );
					}
					if ( CurrentType == "DETAILS" )
					{
						DataTable dt = DataLoadControl . GetDataTable (SqlCommand );
						SqlSupport . LoadDetailsCollection ( dt , true );
					}
					else
					{
						// WORKING 5.2.22
						// This creates and loads a GenericClass table if data is found in the selected table
						DataTable dt = DataLoadControl . GetDataTable (SqlCommand );
						SqlSupport . LoadGenericCollection ( dt );
					}
				}
				if ( UseDirectLoad )
				{
					// CheckBox (UseBackgound Worker) is set to Unchecked for this branch to be activated

					// This method is called on the UI Thread, and require the Events system to be notified when data is ready for us
					// It accepts a fully qualified Sql Command line string to process, a maximum # of recrods to load, and a Notify Event completed flag
					// SIMPLER METHODS !!
					if ( Usetimer )
						timer . Start ( );
					if ( CurrentType == "BANKACCOUNT" )
					{
						bankaccts = new ObservableCollection<BankAccountViewModel> ( );
						bankaccts = SqlSupport . LoadBank ( SqlCommand , 0 , true );
					}
					else if ( CurrentType == "CUSTOMER" )
					{
						custaccts = new ObservableCollection<CustomerViewModel> ( );
						custaccts = SqlSupport . LoadCustomer ( SqlCommand , 0 , true );
					}
					else if ( CurrentType == "SECACCOUNTS" )
					{
						detaccts = new ObservableCollection<DetailsViewModel> ( );
						detaccts = SqlSupport . LoadDetails ( SqlCommand , 0 , true );
					}
					else
					{
						// WORKING 5.2.22
						// This creates and loads a GenericClass table if data is found in the selected table
						string ResultString="";
						string tablename = dbName . SelectedItem . ToString ( );
						SqlCommand = $"Select *from {tablename}";
						genaccts = SqlSupport . LoadGeneric ( SqlCommand , out ResultString , 0 , true );
						if ( genaccts . Count > 0 )
						{
							LoadGrid ( genaccts );
						}
					}
				}
				else
				{
					// MORE COMPLEX METHODS !!
					if ( Usetimer )
						timer . Start ( );
					if ( CurrentType == "BANKACCOUNT" )
						DapperSupport . GetBankObsCollection ( bankaccts , DbNameToLoad: "BankAccount" , Orderby: "Custno, BankNo" , wantSort: true , Caller: "DATAGRIDS" , Notify: true );
					else if ( CurrentType == "CUSTOMER" )
						DapperSupport . GetCustObsCollection ( custaccts , DbNameToLoad: "Customer" , Orderby: "Custno, BankNo" , wantSort: true , Caller: "DATAGRIDS" , Notify: true );
					else if ( CurrentType == "SECACCOUNTS" )
						DapperSupport . GetDetailsObsCollection ( detaccts , DbNameToLoad: "SecAccounts" , Orderby: "Custno, BankNo" , wantSort: true , Caller: "DATAGRIDS" , Notify: true );
				}
			}
		}
		private void ReloadAll ( object sender , RoutedEventArgs e )
		{
			int max=0;
			// Load ALL records
			SqlCommand = GetSqlCommand ( max , dbName . SelectedIndex , "" , "" );
			if ( CurrentType == "BANKACCOUNT" )
				bankaccts = null;
			else if ( CurrentType == "CUSTOMER" )
				custaccts = null;
			else if ( CurrentType == "SECACCOUNTS" )
				detaccts = null;
			else
				genaccts = null;
			Grid1 . ItemsSource = null;
			Grid1 . Refresh ( );
			// Set flag  to ignore limits check
			LoadAll = true;
			LoadData ( );
			// Clear flag again
			LoadAll = false;
		}
		private void ShowSPArgs ( object sender , RoutedEventArgs e )
		{
			//Preview  SP arguments  info in TextBox for current item in Combo
			Mouse . OverrideCursor = Cursors . Wait;
			//showall = false;
			if ( Usetimer )
				timer . Start ( );
			string str = GetSpArgs ( Storedprocs . SelectedItem . ToString ( ) );
			//DbCopiedResult . Text = $"Display selected Stored Procedure Command completed successfully ...";
			Mouse . OverrideCursor = Cursors . Arrow;
		}
		public string GetSpArgs ( string spName , bool showfull = false )
		{
			string output = "";
			string errormsg="";
			int columncount = 0;
			DataTable dt = new DataTable();
			ObservableCollection<GenericClass> Generics = new ObservableCollection<GenericClass>();

			ObservableCollection<BankAccountViewModel> bvmparam = new ObservableCollection<BankAccountViewModel>();
			List<string> genericlist = new List<string>();
			try
			{
				DapperSupport . CreateGenericCollection (
					ref Generics ,
					"spGetSpecificSchema  " ,
					$"{Storedprocs . SelectedItem . ToString ( )}" ,
					"" ,
					"" ,
					ref genericlist ,
					ref errormsg );
				dt = ProcessSqlCommand ( "spGetSpecificSchema  " + spName );
				if ( dt . Rows . Count == 0 )
				{
					if ( errormsg == "" )
						MessageBox . Show ( $"No Argument information is available" , $"[{spName }] SP Script Information" , MessageBoxButton . OK , MessageBoxImage . Warning );
					return "";
				}
			}
			catch ( Exception ex )
			{
				MessageBox . Show ( $"SQL ERROR 1125 - {ex . Message}" );
				return "";
			}
			int  count = 0;
			columncount = 0;
			//			Generics . Clear ( );
			foreach ( var item in dt . Rows )
			{
				GenericClass  gc = new GenericClass ( );
				string  store="";
				DataRow dr = item as DataRow;
				columncount = dr . ItemArray . Count ( );
				if ( columncount == 0 )
					columncount = 1;
				// we only need max cols - 1 here !!!
				for ( int x = 0 ; x < columncount ; x++ )
					store += dr . ItemArray [ x ] . ToString ( ) + ",";
				output += store;
				//CreateGenericRecord ( store , gc , Generics );
			}
			if ( showfull == false )
			{
				// we now have the result, so lets process them
				string buffer = output;
				string[] lines = buffer.Split('\n');
				output = "";
				//output = $"Procedure Name : \n{SPCombo . SelectedItem . ToString ( ) . ToUpper ( )}\n\n";
				foreach ( var item in lines )
				{
					if ( ShowFullScript )
					{
						output += item;
					}
					else
					{
						if ( item . ToUpper ( ) . Contains ( "@" ) )
						{
							if ( item [ 0 ] == '@' && item . ToUpper ( ) . Contains ( "@SQL" ) == false )
								output += item;
						}
						if ( showall == false && item . ToUpper ( ) == "AS\r" )
							break;
					}
				}
				// we now have a list of the Args for the selected SP in output
				// Show it in a TextBox if it takes 1 or more args
				if ( output != "" )
				{
					string fdinput = $"Procedure Name : {Storedprocs . SelectedItem . ToString ( ) . ToUpper ( )}\n\n";
					fdinput += output;
					fdinput += $"\n\nPress ESCAPE to close this window...\n";

					ShowInfo ( line1: fdinput , clr1: "Black0" , line2: "" , clr2: "Black0" , line3: "" , clr3: "Black0" , header: "" , clr4: "Black0" );
					//GridData_Display . Visibility = Visibility . Visible;
					//SetViewButtons ( 2 , ( GridData_Display . Visibility == Visibility . Visible ? true : false ) , ( DisplayGrid . Visibility == Visibility . Visible ? true : false ) );
					//GridData_Display . Focus ( );
				}
				else
				{
					Mouse . OverrideCursor = Cursors . Arrow;
					//Utils . Mbox ( this , string1: $"Procedure [{Storedprocs . SelectedItem . ToString ( ) . ToUpper ( )}] \ndoes not Support / Require any arguments" , string2: "" , caption: "" , iconstring: "\\icons\\Information.png" , Btn1: MB . OK , Btn2: MB . NNULL , defButton: MB . OK );
					ShowInfo ( line1: $"Procedure [{Storedprocs . SelectedItem . ToString ( ) . ToUpper ( )}] \ndoes not Support / Require any arguments" , clr1: "Black0" , line2: "" , clr2: "Black0" , line3: "" , clr3: "Black0" , header: "" , clr4: "Black0" );
				}
			}
			ShowLoadtime ( );
			return output;
		}
		public static DataTable ProcessSqlCommand ( string SqlCommand )
		{
			SqlConnection con;
			DataTable dt = new DataTable();
			string filterline = "";
			string ConString = Flags . CurrentConnectionString;
			//			string ConString = ( string ) Properties . Settings . Default [ "BankSysConnectionString" ];
			//Debug . WriteLine ( $"Making new SQL connection in DETAILSCOLLECTION,  Time elapsed = {timer . ElapsedMilliseconds}" );
			//SqlCommand += " TempDb";
			con = new SqlConnection ( ConString );
			try
			{
				Debug . WriteLine ( $"Using new SQL connection in PROCESSSQLCOMMAND" );
				using ( con )
				{
					SqlCommand cmd = new SqlCommand ( SqlCommand , con );
					SqlDataAdapter sda = new SqlDataAdapter ( cmd );
					sda . Fill ( dt );
				}
			}
			catch ( Exception ex )
			{
				Debug . WriteLine ( $"ERROR in PROCESSSQLCOMMAND(): Failed to load Datatable :\n {ex . Message}, {ex . Data}" );
				MessageBox . Show ( $"ERROR in PROCESSSQLCOMMAND(): Failed to load datatable\n{ex . Message}" );
			}
			finally
			{
				Console . WriteLine ( $" SQL data loaded from SQLCommand [{SqlCommand . ToUpper ( )}]" );
				con . Close ( );
			}
			return dt;
		}
		private void ShowSPScript ( object sender , RoutedEventArgs e )
		{
			ShowFullScript = true;
			ShowSPArgs ( sender , e );
			ShowFullScript = false;

		}
		private void ExecuteSP ( object sender , RoutedEventArgs e )
		{
			Storedprocs_MouseRightButtonUp ( null , null );
		}

		#endregion Data Handling

		#region Background Worker loading
		//Only Triggers a Worker thread
		public void BackgroundWorkerLoad ( )
		{
			// Instantiate the Background Worker system, and then Run it.
			BackgroundWorker  worker = new BackgroundWorker();
			worker . RunWorkerCompleted += new RunWorkerCompletedEventHandler ( worker_RunWorkerCompleted );
			worker . DoWork += new DoWorkEventHandler ( worker_DoWork );
			worker . RunWorkerAsync ( );
		}

		// The real Meat & Potatoes are here !
		private void worker_DoWork ( object sender , DoWorkEventArgs e )
		{
			// Use the background worker system to execute either
			// my Background worker class ( SqlBackgroundLoad Class) methods, or
			// the Methods in the BankCollection Class
			int[] args={0,0,0,0 };

			if ( UseDirectLoad )
			{
				// using our own SQLCOMMAND string to call
				// our Background support class using a DELEGATE declared in the DataLoadController Class
				DataLoadControl dlc = new DataLoadControl();
				LoadTableDelegate glc = dlc.LoadTableInBackground ;
				if ( Usetimer )
					timer . Start ( );
				if ( CurrentType == "BANKACCOUNT" )
				{
					bankaccts = new ObservableCollection<BankAccountViewModel> ( );
					glc ( SqlCommand , "BANKACCOUNT" , bankaccts );
				}
				else if ( CurrentType == "CUSTOMER" )
				{
					custaccts = new ObservableCollection<CustomerViewModel> ( );
					glc ( SqlCommand , "CUSTOMER" , custaccts );
				}
				else if ( CurrentType == "SECACCOUNTS" )
				{
					detaccts = new ObservableCollection<DetailsViewModel> ( );
					glc ( SqlCommand , "SECACCOUNTS" , detaccts );
				}
				else
				{
					// WORKING 5.2.22
					// This creates and loads a GenericClass table(genaccts)  if data is found in the selected table
					DataTable dt = DataLoadControl . GetDataTable (SqlCommand );
					Application . Current . Dispatcher . Invoke ( ( ) =>
					{
						genaccts = new ObservableCollection<GenericClass> ( );
						genaccts = SqlSupport . LoadGenericCollection ( dt );
						if ( genaccts . Count == 0 )
							ShowInfo ( line1: $"Although the request you made was completed succesfully " , line2: $"the table [{CurrentType}] that was queried returned a zero record count, so it\nappears that it does not contain any records" , header: "Unrecognised table type Accessed" , clr4: "Red5" );
					} );
				}
			}
			else
			{
				// default table loading methods (
				DataLoadControl dlc = new DataLoadControl();
				LoadTableWithDapperDelegate glc = dlc.LoadTablewithDapper ;
				DbLoadArgs dbla = new DbLoadArgs();
				if ( Usetimer )
					timer . Start ( );
				if ( CurrentType == "BANKACCOUNT" )
				{
					dbla . dbname = "BANKACCOOUNT";
					dbla . Notify = true;
					glc ( SqlCommand , "BANKACCOUNT" , bankaccts , dbla );
				}
				else if ( CurrentType == "CUSTOMER" )
				{
					dbla . dbname = "CUSTOMERS";
					dbla . Notify = true;
					glc ( SqlCommand , "CUSTOMER" , custaccts , dbla );
				}
				else if ( CurrentType == "DETAILS" )
				{
					dbla . dbname = "SECACCOUNTS";
					dbla . Notify = true;
					glc ( SqlCommand , "SECACCOUNTS" , detaccts , dbla );
				}
				else
				{
					// WORKING 5.2.22
					// This creates and loads a GenericClass table(genaccts)  if data is found in the selected table
					DataTable dt = DataLoadControl . GetDataTable (SqlCommand );
					genaccts = SqlSupport . LoadGenericCollection ( dt );
				}
			}
			{
				//}
				//else
				//{
				//if ( UseDirectLoad)
				//{
				//	Application . Current . Dispatcher . Invoke ( ( ) =>
				//	{
				//		bankaccts = SqlBackgroundLoad . LoadBackground_Bank (
				//		bankaccts ,
				//		SqlCommand ,
				//		"" ,
				//		"" ,
				//		"" ,
				//		false ,
				//		false ,
				//		false ,
				//		"" ,
				//		args );
				//	} );
				//}
				//else
				//{
				//	Application . Current . Dispatcher . Invoke ( ( ) =>
				//	{
				//		bankaccts = SqlBackgroundLoad . LoadBackground_Bank (
				//		bankaccts ,
				//		"Select top (50) * from bankaccount" ,
				//		"BANKACCOUNT" ,
				//		"" ,
				//		"" ,
				//		false ,
				//		false ,
				//		false ,
				//		"" ,
				//		args );
				//	} );
				//}
				//}
			}
		}

		public   string CheckLimits ( )
		{
			int val = 0;
			string [] fields={"","","","","","","","","","" };
			DataLoadControl dlc = new DataLoadControl();
			LoadTableDelegate glc = dlc.LoadTableInBackground ;
			fields [ 0 ] = "select";
			if ( LoadAll == false && RecCount . Text != "" && RecCount . Text != "*" )
			{
				if ( int . TryParse ( RecCount . Text , out val ) == true )
					fields [ 1 ] = $" top ({RecCount . Text }) * ";
				else
					fields [ 1 ] = $" *";
			}
			else
				fields [ 1 ] = $" *";
			if ( dbName . Text != "" )
				fields [ 2 ] = $" from {dbName . Text} ";
			else
				return "";      // no DbName to select from, so abort
			if ( LoadAll == false )
			{
				if ( Conditions . Text != "" && Conditions . Text != "limits..." )
					fields [ 3 ] = $" where {Conditions . Text} ";
				if ( orderby . Text != "" && orderby . Text != "Order by..." )
					fields [ 4 ] = $" order by {orderby . Text}";
			}
			SqlCommand = fields [ 0 ] + fields [ 1 ] + fields [ 2 ] + fields [ 3 ] + fields [ 4 ];
			return SqlCommand;

		}

		// This handles the return value from a background thread, BUT it is running on the main UI thread,
		// so we can access controls normally
		// It is called automatically by the Background Worker system
		private void worker_RunWorkerCompleted ( object sender , RunWorkerCompletedEventArgs e )
		{
			LoadGrid ( );
		}

		#endregion Background Worker loading

		#region Utility  support Methods
		// Create SQLCommand string from fields on UI
		public  string GetSqlCommand ( int count = 0 , int table = 0 , string condition = "" , string sortorder = "" )
		{
			// Parse fields into a valid SQL Command string
			string output = "Select  ";
			output += count == 0 ? " * From " : $"top ({count}) * From ";
			output += dbName . SelectedItem . ToString ( );
			output += condition . Trim ( ) != "" ? " Where " + condition + " " : "";
			output += sortorder . Trim ( ) != "" ? " Order by " + sortorder . Trim ( ) : "";
			CurrentType = dbName . Items [ table ] . ToString ( ) . ToUpper ( );
			return output;
		}

		// Just Assign data to grids to display it
		private void LoadGrid ( object obj = null )
		{

			ShowLoadtime ( );

			// Load whatever data we have received into DataGrid
			if ( CurrentType . ToUpper ( ) == "BANKACCOUNT" )
			{
				if ( bankaccts == null )
					return;
				Grid1 . ItemsSource = bankaccts;
				DbCount = bankaccts . Count;
				ShowInfo ( line1: $"The requested table [{CurrentType}] was loaded successfully, and the {DbCount} records returned are displayed in the table below" , clr1: "Black0" ,
					line2: $"The command line used was" , clr2: "Red2" ,
					line3: $"{SqlCommand . ToUpper ( )}" , clr3: "Blue4" ,
					header: "Bank Accounts data table" , clr4: "Red5" );
				Grid1 . SelectedIndex = 0;
				Grid1 . Focus ( );
			}
			else if ( CurrentType . ToUpper ( ) == "CUSTOMER" )
			{
				if ( custaccts == null )
					return;
				Grid1 . ItemsSource = custaccts;
				DbCount = custaccts . Count;
				ShowInfo ( line1: $"The requested table [{CurrentType}] was loaded successfully, and the {DbCount} records returned are displayed in the table below" , clr1: "Black0" ,
					line2: $"The command line used was" , clr2: "Red2" ,
					line3: $"{SqlCommand . ToUpper ( )}" , clr3: "Blue4" ,
					header: "All Customers data table" , clr4: "Red5" );
				Grid1 . SelectedIndex = 0;
				Grid1 . Focus ( );
			}
			else if ( CurrentType . ToUpper ( ) == "SECACCOUNTS" )
			{
				if ( detaccts == null )
					return;
				Grid1 . ItemsSource = detaccts;
				DbCount = detaccts . Count;
				ShowInfo ( line1: $"The requested table [{CurrentType}] was loaded successfully, and the {DbCount} records returned are displayed in the table below" , clr1: "Black0" ,
					line2: $"The command line used was" , clr2: "Red2" ,
					line3: $"{SqlCommand . ToUpper ( )}" , clr3: "Blue4" ,
					header: "Secondary Accounts data table" );
				Grid1 . SelectedIndex = 0;
				Grid1 . Focus ( );
			}
			else
			{
				if ( genaccts . Count == 0 )
				{
					ShowInfo ( line1: $"The requested table [ {CurrentType} ] succeeded, but returned Zero rows of data." , clr1: "Green5" , header: "It is quite likely that the table is actually empty !" , clr4: "Cyan1" );
					Grid1 . ItemsSource = null;
					Grid1 . Refresh ( );
					return;
				}
				// Caution : This loads the data into the Datarid with only the selected rows
				// //visible in the grid so do NOT repopulate the grid after making this call
				//				SqlServerCommands sqlc = new SqlServerCommands();
				SqlServerCommands . LoadActiveRowsOnlyInGrid ( Grid1 , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
//				Grid1 . ItemsSource = genaccts;
				DbCount = genaccts . Count;
				ShowInfo ( header: "Unrecognised table accessed successfully" , clr4: "Red5" ,
					line1: $"Request made was completed succesfully!" , clr1: "Red3" ,
					line2: $"the table [{CurrentType}] that was queried returned a record count of {DbCount}.\nThe structure of this data is not recognised, so a generic structure has been used..." ,
					line3: $"{SqlCommand . ToUpper ( )}" , clr3: "Blue4"
					);
				Grid1 . SelectedIndex = 0;
				Grid1 . Focus ( );
			}
		}


		//Get list of all Tables in our Db (Ian1.MDF)
		public void LoadTablesList ( )
		{
			int bankindex = 0, count=0;
			List<string> list = new List<string>      ();
			SqlCommand = "spGetTablesList";
			CallStoredProcedure ( list , SqlCommand );
			//This call returns us a DataTable
			DataTable dt = DataLoadControl . GetDataTable ( SqlCommand );
			//			Grid2 . ItemsSource = dt . DefaultView;
			//			Grid2 . Refresh ( );
			// This how to access  Row data from  a grid the easiest way.... parsed into a List <xxxxx>
			list = Utils . GetDataDridRowsAsListOfStrings ( dt );
			foreach ( string row in list )
			{
				dbName . Items . Add ( row );
				if ( row . ToUpper ( ) == "BANKACCOUNT" )
					bankindex = count;
				count++;
			}
			// how to Sort Combo/Listbox contents
			dbName . Items . SortDescriptions . Add ( new SortDescription ( "" , ListSortDirection . Ascending ) );
			dbName . SelectedIndex = bankindex;
		}

		// load a list of all SP's
		private void LoadSpList ( )
		{
			List<string> SpList = new List<string>();
			SpList = CallStoredProcedure ( SpList , "spGetStoredProcs" );
			Storedprocs . ItemsSource = SpList;
			Storedprocs . Items . SortDescriptions . Add ( new SortDescription ( "" , ListSortDirection . Ascending ) );
			Storedprocs . SelectedIndex = 0;
			Storedprocs . SelectedItem = 0;
			Storedprocs . Refresh ( );

		}


		#endregion Utility  support Methods

		#region Trigger methods  for Stored Procedures (string, Int, Double, Decimal) that return a List<xxxxx>
		// These all return just a single column from any table by calling a Stored Procedure  in MSSQL Server
		public static List<string> CallStoredProcedure ( List<string> list , string sqlcommand )
		{
			//This call returns us a DataTable
			DataTable dt = DataLoadControl . GetDataTable ( sqlcommand );
			if(dt != null)
				list = Utils . GetDataDridRowsAsListOfStrings ( dt );
			return list;
		}
		public static List<int> CallStoredProcedure ( List<int> list , string sqlcommand )
		{
			//This call returns us a DataTable
			DataTable dt = DataLoadControl . GetDataTable ( sqlcommand );
			list = Utils . GetDataDridRowsAsListOfInts ( dt );
			return list;
		}
		public static List<double> CallStoredProcedure ( List<double> list , string sqlcommand )
		{
			//This call returns us a DataTable
			DataTable dt = DataLoadControl . GetDataTable ( sqlcommand );
			list = Utils . GetDataDridRowsAsListOfDoubles ( dt );
			return list;
		}
		public static List<decimal> CallStoredProcedure ( List<decimal> list , string sqlcommand )
		{
			//This call returns us a DataTable
			DataTable dt = DataLoadControl . GetDataTable ( sqlcommand );
			list = Utils . GetDataDridRowsAsListOfDecimals ( dt );
			return list;
		}
		public static List<DateTime> CallStoredProcedure ( List<DateTime> list , string sqlcommand )
		{
			//This call returns us a DataTable
			DataTable dt = DataLoadControl . GetDataTable ( sqlcommand );
			list = Utils . GetDataDridRowsAsListOfDateTime ( dt );
			return list;
		}
		#endregion Trigger methods  for Stored Procedures

		//private void sp_SelectionChanged ( object sender , SelectionChangedEventArgs e )
		//{

		//}

		// Executes any SP successfully when it is right clicked in the Combo list
		// If any rows are returned ,they are shown in Grid1 DataGrid
		//otherwise a Message box appears
	
		#region FlowDoc Drag methods
		// All working perfectly
		private void Flowdoc_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
		{
			//In this event, we get current mouse position on the control to use it in the MouseMove event.
			if ( Utils . HitTestScrollBar ( sender , e ) == true )
			{
				if ( e . OriginalSource . ToString ( ) . Contains ( ".Run" ) == false )
				{
					return;
				}
			}
			FirstXPos = e . GetPosition ( sender as Control ) . X;
			FirstYPos = e . GetPosition ( sender as Control ) . Y;
			double FirstArrowXPos = e . GetPosition ( ( sender as Control ) . Parent as Control ) . X - FirstXPos;
			double FirstArrowYPos = e . GetPosition ( ( sender as Control ) . Parent as Control ) . Y - FirstYPos;
			MovingObject = sender;
		}

		private void Flowdoc_MouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
		{
			MovingObject = null;
			//ReleaseMouseCapture ( );
			//Console . WriteLine ( "Mouse RELEASED..(Flowdoc_MouseLeftButtonUp." );
		}

		private void Flowdoc_MouseMove ( object sender , MouseEventArgs e )
		{
			if ( MovingObject != null && e . LeftButton == MouseButtonState . Pressed )
			{
				// Get mouse position IN FlowDoc !!
				double left = e . GetPosition ( ( MovingObject as FrameworkElement ) . Parent as FrameworkElement ) . X - FirstXPos ;
				double top = e . GetPosition ( ( MovingObject as FrameworkElement ) . Parent as FrameworkElement ) . Y - FirstYPos ;
				double trueleft = left - FirstXPos;
				double truetop = left - FirstYPos;
				if ( left >= 0 ) // && left <= canvas.ActualWidth - Flowdoc.ActualWidth)
					( MovingObject as FrameworkElement ) . SetValue ( Canvas . LeftProperty , left );
				if ( top >= 0 ) //&& top <= canvas . ActualHeight- Flowdoc. ActualHeight)
					( MovingObject as FrameworkElement ) . SetValue ( Canvas . TopProperty , top );
			}
		}
		#endregion FlowDoc Drag methods

		#region Mouse handlers
		private void dbName_PreviewMouseRightButtonUp ( object sender , MouseButtonEventArgs e )
		{
			string currsel = dbName.SelectedItem.ToString();
			e . Handled = true;
			dbName . Items . Clear ( );
			LoadTablesList ( );
			for (int x = 0; x < dbName.Items.Count ; x++ )
			{
				if ( dbName . Items [ x ].ToString().ToUpper() == currsel )
				{
					dbName . SelectedIndex = x;
					break;
				}
			}
		}
		private void scrollview_Click ( object sender , RoutedEventArgs e )
		{
			Flags . UseScrollView = !Flags . UseScrollView;
		}
		private void Storedprocs_MouseRightButtonUp ( object sender , MouseButtonEventArgs e )
		{
			string errmsg="";
			string args="";
			Grid1 . ItemsSource = null;
			Grid1 . Refresh ( );
			string cmd = Storedprocs.SelectedItem.ToString();
			if ( SpArgs . Text != "" && SpArgs . Text . Contains ( "Enter Arg" ) == false )
				args += $" {SpArgs . Text }";
			genaccts = SqlSupport . ExecuteStoredProcedure ( cmd , genaccts , out errmsg , Arguments: args );
			DbCount = genaccts . Count;
			if ( DbCount > 0 )
			{
				SqlServerCommands . LoadActiveRowsOnlyInGrid ( Grid1 , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
				//				Grid1 . ItemsSource = genaccts;
				ShowInfo ( line1: $"Stored Procedure was completed successfully, and returned the {DbCount} records shown in the Grid below !" , "Black0" ,
					    line2: $"Procedure executed was :" , clr2: "Black0" , line3: $"{cmd . ToUpper ( )}" , clr3: "" , header: "Stored Procedure execution" , "Orange1" );
			}
			else if ( errmsg != "" )
				ShowInfo ( line1: $"Stored Procedure [{cmd . ToUpper ( )}] returned the following information !\n\n[{errmsg}]\n " , "Red4" , "Stored Procedure execution" , "Orange1" );
		}
		#endregion Mouse handlers

		#region keyboard handlers
		private void Window_PreviewKeyDown ( object sender , System . Windows . Input . KeyEventArgs e )
		{
			// Allow quick window close, (but only close FlowDoc if it is currently open)
			if ( e . Key == Key . Escape && Flowdoc . Visibility == Visibility . Visible )
				Flowdoc . Visibility = Visibility . Hidden;       // Just hide  the FlowDoc
			else if ( e . Key == Key . Escape )
				this . Close ( );           // Close the window
			else if ( e . Key == Key . F8 )
			{
				//				ReleaseMouseCapture ( );
				//				Console . WriteLine ( "Mouse RELEASED ...Window_PreviewKeyDown()" );
			}
		}
		private void Flowdoc_PreviewKeyDown ( object sender , KeyEventArgs e )
		{
			if ( e . Key == Key . Escape )
				Flowdoc . Visibility = Visibility . Hidden;
		}
		#endregion keyboard handlers

		#region GotFocus
		private void Args_GotFocus ( object sender , RoutedEventArgs e )
		{
			SpArgs . Background = FindResource ( "White0" ) as SolidColorBrush;
			SpArgs . Foreground = FindResource ( "Black0" ) as SolidColorBrush;
			if(SpArgs.Text == "Enter Arguments  for current S.P" )
				SpArgs . Text = "";
		}
		//Handle gray text in text fields on field entry
		private void tb_GotFocus ( object sender , RoutedEventArgs e )
		{
			TextBox tb = sender as TextBox;
			tb . Background = FindResource ( "White0" ) as SolidColorBrush;
			tb . Foreground = FindResource ( "Black0" ) as SolidColorBrush;
			tb . CaretBrush = FindResource ( "Red0" ) as SolidColorBrush;
			//tb . Foreground = FindResource ( "Black0" ) as SolidColorBrush;
			tb . Text = "";
		}
		#endregion GotFocus

		#region LostFocus
		private void SpArgs_LostFocus ( object sender , RoutedEventArgs e )
		{
			SpArgs . Background = FindResource ( "Gray2" ) as SolidColorBrush;
			if ( SpArgs . Text == "" )
			{
				SpArgs . Text = "Enter Arguments  for current S.P";
			}
		}

		private void Conditions_LostFocus ( object sender , RoutedEventArgs e )
		{
			if ( Conditions . Text == "" )
				Conditions . Background = FindResource ( "Gray2" ) as SolidColorBrush;

		}

		private void orderby_LostFocus ( object sender , RoutedEventArgs e )
		{
			if ( orderby . Text == "" )
				orderby . Background = FindResource ( "Gray2" ) as SolidColorBrush;
		}
		#endregion LostFocus	

		#region Checkbox handlers
		private void ShowInfo_Click ( object sender , RoutedEventArgs e )
		{
			UseFlowdoc = !UseFlowdoc;
		}
		private void Timer_Click ( object sender , RoutedEventArgs e )
		{
			Usetimer = !Usetimer;
			LoadTime . Text = "xxx";
		}
		private void ShowLoadtime ( )
		{
			if ( Usetimer )
			{
				timer . Stop ( );
				if ( timer . ElapsedMilliseconds != 0 )
					LoadTime . Text = timer . ElapsedMilliseconds . ToString ( ) + " m/secs";
				timer . Reset ( );
			}
		}
		private void checkBox_Click ( object sender , RoutedEventArgs e )
		{
			var v = sender as CheckBox;
			if ( v . IsChecked == true )
			{
				Flags . PinToBorder = true;
				/// Move it to top left corbner
				( Flowdoc as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) 0 );
				( Flowdoc as FrameworkElement ) . SetValue ( Canvas . TopProperty , ( double ) 0 );
			}
			else
				Flags . PinToBorder = false;
		}

		#endregion Checkbox handlers

		private void ShowInfo ( string line1 = "" , string clr1 = "" , string line2 = "" , string clr2 = "" , string line3 = "" , string clr3 = "" , string header = "" , string clr4 = "" , bool beep = false )
		{
			if ( UseFlowdoc )
			{
				if ( UseFlowdocBeep == false )
					beep = false;
				Flowdoc . ShowInfo ( line1 , clr1 , line2 , clr2 , line3 , clr3 , header , clr4 , beep );
				canvas . Visibility = Visibility . Visible;
				canvas . BringIntoView ( );
				Flowdoc . Visibility = Visibility . Visible;
				Flowdoc . BringIntoView ( );
				if ( Flags . PinToBorder == true )
				{
					( Flowdoc as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) 0 );
					( Flowdoc as FrameworkElement ) . SetValue ( Canvas . TopProperty , ( double ) 0 );
				}
			}
		}

	}
}
