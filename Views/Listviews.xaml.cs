using MyDev . Sql;
using MyDev . SQL;
using MyDev . UserControls;
using MyDev . ViewModels;

using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . ComponentModel;
using System . Configuration;
using System . Data;
using System . Diagnostics;
using System . Runtime . InteropServices . WindowsRuntime;
using System . Windows;
using System . Windows . Automation . Peers;
using System . Windows . Controls;
using System . Windows . Documents;
using System . Windows . Input;
using System . Windows . Media;
using System . Xml . Linq;

using static Dapper . SqlMapper;

namespace MyDev . Views
{
	public partial class Listviews : Window, INotifyPropertyChanged
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

		// Northwind Records / collections
		public nwcustomer nwc = new nwcustomer();
		public NwOrderCollection  nwo = new NwOrderCollection( );
		public ObservableCollection<nwcustomer> nwcustomeraccts = new ObservableCollection<nwcustomer>();
		public ObservableCollection<nworder> nworderaccts = new ObservableCollection<nworder>();

		// Pubs
		public PubAuthors pubAuthor= new PubAuthors ();
		static public ObservableCollection<PubAuthors > pubauthors= new   ObservableCollection<PubAuthors >();

		// supporting sources
		public List<string> TablesList = new List<string>();

		// internal Flag data
		//		private string CurrentType= "BANKACCOUNT";
		//		private string [ ] ACTypes = {"BANK", "CUSTOMER", "DETAILS", "NWCUSTOMER", "NWCUSTLIMITED", "GENERIC"};
		//		private string [ ] DefaultTables = {"BANKACCOUNT", "CUSTOMER", "SECACCOUNTS", "CUSTOMERS", "GENERICS"};
		private string SqlCommand="";
		private string DefaultSqlCommand="Select * from BankAccount";
		string Nwconnection = "NorthwindConnectionString";
		string CurrentSqlConnection = "BankSysConnectionString";
		string CurrentDbName = "IAN1";
		string CurrentTableName="BANKACCOUNT";
		string CurrentDataTable = "";
		Dictionary <string, string> ConnectionStringsDict = new Dictionary<string, string>();
		Dictionary <string, string> DefaultSqlCommands = new Dictionary<string, string>();

		#endregion  Public variables

		#region private variables
		private bool UseDirectLoad = true;
		private bool UseBGThread= true;
		private bool LoadDirect=false;
		private bool alldone=false;
		// pro temp variables
		// Flowdoc flags
		private bool UseFlowdoc=false;
		private bool UseFlowdocBeep=false;
		private bool UseScrollViewer= false;

		private  double flowdocHeight=0;
		private  double flowdocWidth=0;
		private  double flowdocTop=0;
		private  double flowdocLeft=0;
		private bool showall=false;
		private  bool ShowFullScript = false;
		private bool Startup = true;
		private bool LoadAll = true;
		private bool Usetimer = true;
		private bool ComboSelectionActive = false;
		private static Stopwatch timer = new Stopwatch();
		Datagrids dGrids = new Datagrids();
		#endregion private variables

		#region Private layout variables

		private double row2Height;
		public double Row2Height
		{
			get { return row2Height; }
			set { row2Height = value; }
		}
		private double col2Width;
		public double Col2Width
		{
			get { return col2Width; }
			set { col2Width = value; }
		}
		private double col3Width;
		public double Col3Width
		{
			get { return col3Width; }
			set { col3Width = value; }
		}

		#endregion Private layout variables

		#region Full Properties

		// Full properties used in Binding to I/f objects
		private int dbCountlb;
		public int DbCountlb
		{
			get { return dbCountlb; }
			set { dbCountlb = value; OnPropertyChanged ( "DbCountlb" ); Console . WriteLine ( $"DbCountlb set to {value}" ); }
		}
		private int dbCountlv;
		public int DbCountlv
		{
			get { return dbCountlv; }
			set { dbCountlv = value; OnPropertyChanged ( "DbCountlv" ); Console . WriteLine ( $"DbCountlv set to {value}" ); }
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
		private object Colorpickerobject;

		public object ColorpickerObject
		{
			get { return Colorpickerobject; }
			set { Colorpickerobject = value; }
		}


		private double FirstXPos=0;
		private double FirstYPos=0;

		private double CpFirstXPos=0;
		private double CpFirstYPos=0;

		#endregion Full Properties

		//Data  for font size/rowheight
		private List<int> fontsizes = new List<int>( );
		private List<int> rowsizes = new List<int>( );

		#region Startup / close
		public Listviews ( )
		{
			InitializeComponent ( );
			this . DataContext = this;
			// Setup default connection
			//			BgWorker . IsChecked = true;
			UseBGThread = true;
			UseFlowdoc = false;
			UseScrollViewer = false;
			Usetimer = true;
			//			UseTimer . IsChecked = true;
			Flags . UseScrollView = false;
			// Set flags so we get the right SQL command method used...
			UseDirectLoad = true;
			SqlCommand = DefaultSqlCommand;

		}
		//*************************************************************//
		// Initial startup - load Db = Ian1 / Table = Bankaccount 
		//*************************************************************//
		private void ListViewWindow_Loaded ( object sender , RoutedEventArgs e )
		{
			this . DataContext = this;
			canvas . Visibility = Visibility . Visible;
			// Initialize all connection strings
			LoadConnectionStrings ( );

			// Get list of Dbs (just 3 right now)
			LoadAllDbNames ( );

			// Initialize all default Sql command strings
			LoadDefaultSqlCommands ( );
			DefaultSqlCommand = GetDefaultSqlCommand ( "BANKACCOUNT" );
			if ( DefaultSqlCommand == "" )
			{
				Console . WriteLine ( "Unable to load default Select string for BANKACOUNT Db " );
				Utils . DoErrorBeep ( 250 , 50 , 1 );
			}
			else
				SqlCommand = DefaultSqlCommand;
			if ( SetConnectionString ( "IAN1" ) == false )
			{
				Console . WriteLine ( "Unable to load connection string for BANKACOUNT Db from System Properties" );
				Utils . DoErrorBeep ( 250 , 50 , 1 );
			}
			//Default to Bankaccount as we are strting up
			CurrentDbName = "IAN1";
			CurrentTableName = "BANKACCOUNT";

			// Now open Ian1, load data and display in both viewers
			// 1st = connect to IAN1.MDF
			// this also loads list of tables in Ian1/mdf
			OpenIan1Db ( );
			// used to access Dictionary of DataTemplates  - load into both Combos and selects 1st entry
			//LoadDataTemplates_Ian1 ( CurrentTableName , "" );

			// now load list of tables in IAN1 Db
			LoadDbTables ( "IAN1" );
			SelectCurrentDbInCombo ( "BANKACCOUNT" , "" );
			//			LoadTablesList_Ian1 ( );

			// Load Bankaccount into both viewers
			CurrentTableName = "BANKACCOUNT";
			LoadData_Ian1 ( "" );
			// Set selection of datatemplate to 1st one (selection  used in load method)
			CurrentDataTable = DataTemplatesLv . Items [ 0 ] . ToString ( );

			// Hook into our Flowdoc so we can resize it in  the canvas !!!
			// Flowdoc has an Event declared (ExecuteFlowDocSizeMethod ) that we are  hooking into
			Flowdoc . ExecuteFlowDocSizeMethod += new EventHandler ( ParentWPF_method );
			Colorpicker . ExecuteSaveToClipboardMethod += Colorpicker_ExecuteSaveToClipboardMethod;
//			Colorpicker . ExecuteMoveMethod += Colorpicker_ExecuteMoveMethod;


			// LOAD BOTH VIEWERS (NO PARAMETER)
			LoadGrid_IAN1 ( );
			Startup = false;
			ShowInfo ( header: "Load completed successfully" , clr4: "Red5" ,
				line1: $"Request made was completed succesfully!" , clr1: "Red3" ,
				line2: $"" ,
				line3: $"{SqlCommand . ToUpper ( )}" , clr3: "Blue4"
				);
			LoadFontsizes ( );
			LoadRowsizes ( );
		}


		private void LoadRowsizes ( )
		{
			rowsizes . Add ( 10 );
			rowsizes . Add ( 12 );
			rowsizes . Add ( 14 );
			rowsizes . Add ( 15 );
			rowsizes . Add ( 16 );
			rowsizes . Add ( 17 );
			rowsizes . Add ( 18 );
			rowsizes . Add ( 20 );
			rowsizes . Add ( 22 );
			rowsizes . Add ( 24 );
			rowsizes . Add ( 26 );
			rowsizes . Add ( 28 );
			rowsizes . Add ( 30 );
			rowsizes . Add ( 34 );
			rowsizes . Add ( 36 );
			rowsizes . Add ( 38 );
			rowsizes . Add ( 40 );
			rowsizes . Add ( 45 );
			rowheight . ItemsSource = rowsizes;
		}
		private void LoadFontsizes ( )
		{
			fontsizes . Add ( 11 );
			fontsizes . Add ( 12 );
			fontsizes . Add ( 13 );
			fontsizes . Add ( 14 );
			fontsizes . Add ( 15 );
			fontsizes . Add ( 16 );
			fontsizes . Add ( 17 );
			fontsizes . Add ( 18 );
			fontsizes . Add ( 19 );
			fontsizes . Add ( 20 );
			fontSize . ItemsSource = fontsizes;
		}
		private void Close_Btn ( object sender , RoutedEventArgs e )
		{
			this . Close ( );
		}

		private void App_Close ( object sender , RoutedEventArgs e )
		{
			this . Close ( );
			Application . Current . Shutdown ( );
		}
		#endregion Startup / close

		#region Load data for the 3 different Db's
		private void LoadData_Ian1 ( string viewertype )
		{
			if ( Usetimer )
				timer . Start ( );
			if ( CurrentTableName == "BANKACCOUNT" )
			{
				//// Looad available DataTemplates into combo(s) ??
				LoadDataTemplates_Ian1 ( "BANKACCOUNT" , viewertype );
				bankaccts = new ObservableCollection<BankAccountViewModel> ( );
				SqlCommand = GetDefaultSqlCommand ( CurrentTableName );
				// need to do this cos the SQL command is changed to load the tables list....
				if ( Startup )
					SqlCommand = DefaultSqlCommand;
				bankaccts = SqlSupport . LoadBank ( SqlCommand , 0 , true );
			}
			else if ( CurrentTableName == "CUSTOMER" )
			{
				LoadDataTemplates_Ian1 ( "CUSTOMER" , viewertype );
				custaccts = new ObservableCollection<CustomerViewModel> ( );
				SqlCommand = GetDefaultSqlCommand ( CurrentTableName );
				// need to do this cos the SQL command is changed to load the tables list....
				if ( Startup )
					SqlCommand = DefaultSqlCommand;
				custaccts = SqlSupport . LoadCustomer ( SqlCommand , 0 , true );
			}
			else if ( CurrentTableName == "SECACCOUNTS" )
			{
				LoadDataTemplates_Ian1 ( "SECACCOUNTS" , viewertype );
				detaccts = new ObservableCollection<DetailsViewModel> ( );
				SqlCommand = GetDefaultSqlCommand ( CurrentTableName );
				// need to do this cos the SQL command is changed to load the tables list....
				if ( Startup )
					SqlCommand = DefaultSqlCommand;
				detaccts = SqlSupport . LoadDetails ( SqlCommand , 0 , true );
			}
			else
			{
				string tablename="";
				LoadDataTemplates_Ian1 ( "GENERIC" , viewertype );
				genaccts = new ObservableCollection<GenericClass> ( );
				// need to do this cos the SQL command is changed to load the tables list....
				if ( viewertype == "VIEW" )
					tablename = dbNameLv . SelectedItem . ToString ( );
				else
					tablename = dbNameLb . SelectedItem . ToString ( );
				SqlCommand = $"Select *from {tablename}";
				SqlCommand = GetDefaultSqlCommand ( CurrentTableName );
				if ( SqlCommand == "" )
					SqlCommand = $"Select * from {tablename}";
				// need to do this cos the SQL command is changed to load the tables list....
				if ( Startup )
					SqlCommand = DefaultSqlCommand;
				string ResultString="";
				genaccts = SqlSupport . LoadGeneric ( SqlCommand , out ResultString , 0 , true );
				if ( genaccts . Count > 0 )
				{
					ShowInfo ( line1: $"The requested table [{CurrentTableName }] was loaded successfully, and {genaccts . Count} records were returned,\nThe data is shown in  the viewer below" , clr1: "Black0" ,
						line2: $"The command line used was" , clr2: "Red2" ,
						line3: $"{SqlCommand . ToUpper ( )}" , clr3: "Blue4" ,
						header: "Generic style data table" , clr4: "Red5" );
				}
				else
				{
					ShowInfo ( line1: $"The requested table [{CurrentTableName }] was loaded successfully, but ZERO records were returned,\nThe Table is  " , clr1: "Black0" ,
						line2: $"The command line used was" , clr2: "Red2" ,
						line3: $"{SqlCommand . ToUpper ( )}" , clr3: "Blue4" ,
						header: "Generic style data table" , clr4: "Red5" );


				}
				//else
				//{
				//	// MORE COMPLEX METHODS !!
				//	if ( Usetimer )
				//		timer . Start ( );
				//	if ( CurrentType == "BANKACCOUNT" )
				//		DapperSupport . GetBankObsCollection ( bankaccts , DbNameToLoad: "BankAccount" , Orderby: "Custno, BankNo" , wantSort: true , Caller: "DATAGRIDS" , Notify: true );
				//	else if ( CurrentType == "CUSTOMER" )
				//		DapperSupport . GetCustObsCollection ( custaccts , DbNameToLoad: "Customer" , Orderby: "Custno, BankNo" , wantSort: true , Caller: "DATAGRIDS" , Notify: true );
				//	else if ( CurrentType == "SECACCOUNTS" )
				//		DapperSupport . GetDetailsObsCollection ( detaccts , DbNameToLoad: "SecAccounts" , Orderby: "Custno, BankNo" , wantSort: true , Caller: "DATAGRIDS" , Notify: true );
				//}
			}
		}
		public void LoadData_NorthWind ( string viewertype )
		{
			if ( Usetimer )
				timer . Start ( );
			if ( CurrentTableName == "CUSTOMERS" )
			{

				LoadDataTemplates_NorthWind ( "CUSTOMERS" , viewertype );
				nwcustomeraccts = new ObservableCollection<nwcustomer> ( );
				nwcustomeraccts = nwc . GetNwCustomers ( );
				// need to do this cos the SQL command is changed to load the tables list....
				if ( Startup )
					SqlCommand = DefaultSqlCommand;
			}
			else if ( CurrentTableName == "ORDERS" )
			{

				LoadDataTemplates_NorthWind ( "ORDERS" , viewertype );
				nworderaccts = new ObservableCollection<nworder> ( );
				nworderaccts = nwo . LoadOrders ( );
				// need to do this cos the SQL command is changed to load the tables list....
				if ( Startup )
					SqlCommand = DefaultSqlCommand;
			}
			else
			{
				string tablename="";
				LoadDataTemplates_Ian1 ( "GENERIC" , viewertype );
				genaccts = new ObservableCollection<GenericClass> ( );
				// need to do this cos the SQL command is changed to load the tables list....
				if ( viewertype == "VIEW" )
					tablename = dbNameLv . SelectedItem . ToString ( );
				else
					tablename = dbNameLb . SelectedItem . ToString ( );
				SqlCommand = GetDefaultSqlCommand ( CurrentTableName );
				if ( SqlCommand == "" )
					SqlCommand = $"Select * from {tablename}";
				// need to do this cos the SQL command is changed to load the tables list....
				if ( Startup )
					SqlCommand = DefaultSqlCommand;
				string ResultString="";
				genaccts = SqlSupport . LoadGeneric ( SqlCommand , out ResultString , 0 , true );
				if ( genaccts . Count > 0 )
				{
					ShowInfo ( line1: $"The requested table [{CurrentTableName }] was loaded successfully, and {genaccts . Count} records were returned,\nThe data is shown in  the viewer below" , clr1: "Black0" ,
						line2: $"The command line used was" , clr2: "Red2" ,
						line3: $"{SqlCommand . ToUpper ( )}" , clr3: "Blue4" ,
						header: "Generic style data table" , clr4: "Red5" );
				}
				else
				{
					ShowInfo ( line1: $"The requested table [{CurrentTableName }] was loaded successfully, but ZERO records were returned,\nThe Table is  " , clr1: "Black0" ,
						line2: $"The command line used was" , clr2: "Red2" ,
						line3: $"{SqlCommand . ToUpper ( )}" , clr3: "Blue4" ,
						header: "Generic style data table" , clr4: "Red5" );
				}
			}
		}
		public void LoadData_Publishers ( string viewertype , out ObservableCollection<GenericClass> generics )
		{
			generics = null;
			//ObservableCollection<GenericClass> generics = new ObservableCollection<GenericClass>();
			if ( Usetimer )
				timer . Start ( );
			if ( CurrentTableName == "AUTHORS" )
			{
				LoadDataTemplates_PubAuthors ( "AUTHORS" , viewertype );
				pubauthors = new ObservableCollection<PubAuthors> ( );
				// need to do this cos the SQL command is changed to load the tables list....
				if ( Startup )
					SqlCommand = DefaultSqlCommand;
				//				nwcustomeraccts = SqlSupport . LoadBank ( SqlCommand , 0 , true );
			}
			else if ( CurrentTableName == "ORDERS" )
			{

				LoadDataTemplates_NorthWind ( "ORDERS" , viewertype );
				nworderaccts = new ObservableCollection<nworder> ( );
				// need to do this cos the SQL command is changed to load the tables list....
				if ( Startup )
					SqlCommand = DefaultSqlCommand;
				//				nwcustomeraccts = SqlSupport . LoadBank ( SqlCommand , 0 , true );
			}
			else
			{
				string tablename="";
				LoadDataTemplates_Ian1 ( "GENERIC" , viewertype );
				genaccts = new ObservableCollection<GenericClass> ( );
				// need to do this cos the SQL command is changed to load the tables list....
				if ( viewertype == "VIEW" )
					tablename = dbNameLv . SelectedItem . ToString ( );
				else
					tablename = dbNameLb . SelectedItem . ToString ( );
				SqlCommand = $"Select *from {tablename}";
				SqlCommand = GetDefaultSqlCommand ( CurrentTableName );
				if ( SqlCommand == "" )
					SqlCommand = $"Select * from {tablename}";
				// need to do this cos the SQL command is changed to load the tables list....
				if ( Startup )
					SqlCommand = DefaultSqlCommand;
				string ResultString="";
				genaccts = SqlSupport . LoadGeneric ( SqlCommand , out ResultString , 0 , true );
				if ( genaccts . Count > 0 )
				{
					ShowInfo ( line1: $"The requested table [{CurrentTableName }] was loaded successfully, and {genaccts . Count} records were returned,\nThe data is shown in  the viewer below" , clr1: "Black0" ,
						line2: $"The command line used was" , clr2: "Red2" ,
						line3: $"{SqlCommand . ToUpper ( )}" , clr3: "Blue4" ,
						header: "Generic style data table" , clr4: "Red5" );
				}
				else
				{
					ShowInfo ( line1: $"The requested table [{CurrentTableName }] was loaded successfully, but ZERO records were returned,\nThe Table is  " , clr1: "Black0" ,
						line2: $"The command line used was" , clr2: "Red2" ,
						line3: $"{SqlCommand . ToUpper ( )}" , clr3: "Blue4" ,
						header: "Generic style data table" , clr4: "Red5" );
				}
				generics = genaccts;
			}
		}

		#endregion Load data for the 3 different Db's

		// Just Assign data to grids to display it
		#region Load Viewer for the 3 different Db's
		private void LoadGrid_IAN1 ( string type = "" )
		{
			// Load whatever data we have received into DataGrid
			if ( CurrentTableName . ToUpper ( ) == "BANKACCOUNT" )
			{
				if ( bankaccts == null )
					return;
				if ( type == "VIEW" )
				{
					listView . ItemsSource = bankaccts;
					dGrid . ItemsSource = bankaccts;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					HandleCaption ( "VIEW" , bankaccts . Count );
					listView . Focus ( );
				}
				else if ( type == "BOX" )
				{
					listBox . ItemsSource = bankaccts;
					dGrid . ItemsSource = bankaccts;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					HandleCaption ( "BOX" , bankaccts . Count );
					listBox . Focus ( );
				}
				else
				{
					listView . ItemsSource = bankaccts;
					listBox . ItemsSource = bankaccts;
					dGrid . ItemsSource = bankaccts;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					HandleCaption ( "" , bankaccts . Count );
					listBox . Focus ( );
					listView . Focus ( );
				}
				ShowInfo ( line1: $"The requested table [{CurrentTableName }] was loaded successfully, and the {DbCountlb} records returned are displayed in the table below" , clr1: "Black0" ,
					line2: $"The command line used was" , clr2: "Red2" ,
					line3: $"{SqlCommand . ToUpper ( )}" , clr3: "Blue4" ,
					header: "Bank Accounts data table" , clr4: "Red5" );
			}
			else if ( CurrentTableName . ToUpper ( ) == "CUSTOMER" )
			{
				if ( custaccts == null )
					return;
				if ( type == "VIEW" )
				{
					listView . ItemsSource = custaccts;
					dGrid . ItemsSource = custaccts;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					HandleCaption ( "VIEW" , custaccts . Count );
					listView . SelectedIndex = 0;
					listView . Focus ( );
				}
				else if ( type == "BOX" )
				{
					listBox . ItemsSource = custaccts;
					dGrid . ItemsSource = custaccts;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					HandleCaption ( "BOX" , custaccts . Count );
					listBox . SelectedIndex = 0;
				}
				else
				{
					listView . ItemsSource = custaccts;
					listBox . ItemsSource = custaccts;
					dGrid . ItemsSource = custaccts;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					HandleCaption ( "" , custaccts . Count );
					listBox . SelectedIndex = 0;
					listView . SelectedIndex = 0;
				}
				listBox . SelectedIndex = 0;
				listBox . Focus ( );
			}
			else if ( CurrentTableName . ToUpper ( ) == "SECACCOUNTS" )
			{
				if ( detaccts == null )
					return;
				if ( type == "VIEW" )
				{
					listView . ItemsSource = detaccts;
					dGrid . ItemsSource = detaccts;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					HandleCaption ( "VIEW" , detaccts . Count );
				}
				else if ( type == "BOX" )
				{
					listBox . ItemsSource = detaccts;
					dGrid . ItemsSource = detaccts;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					HandleCaption ( "BOX" , detaccts . Count );
					listBox . SelectedIndex = 0;
				}
				else
				{
					listView . ItemsSource = detaccts;
					listBox . ItemsSource = detaccts;
					dGrid . ItemsSource = detaccts;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					HandleCaption ( "" , detaccts . Count );
					listBox . SelectedIndex = 0;
					listView . SelectedIndex = 0;
					listView . Focus ( );
				}
				ShowInfo ( line1: $"The requested table [{CurrentTableName }] was loaded successfully, and the {DbCountlb} records returned are displayed in the table below" , clr1: "Black0" ,
					line2: $"The command line used was" , clr2: "Red2" ,
					line3: $"{SqlCommand . ToUpper ( )}" , clr3: "Blue4" ,
					header: "Secondary Accounts data table" );
				listBox . Focus ( );
			}
			else
			{
				if ( genaccts . Count > 0 )
				{
					//Generic with >= 1 records
					if ( type == "VIEW" )
					{
						listView . ItemsSource = genaccts;
						SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
						dGrid . SelectedItem = dGrid . SelectedIndex = 0;
						HandleCaption ( "VIEW" , genaccts . Count );
						listView . SelectedIndex = 0;
					}
					else if ( type == "BOX" )
					{
						listBox . ItemsSource = genaccts;
						SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
						dGrid . SelectedItem = dGrid . SelectedIndex = 0;
						HandleCaption ( "BOX" , genaccts . Count );
						listBox . SelectedIndex = 0;
						listBox . Focus ( );
					}
					else
					{
						listView . ItemsSource = genaccts;
						listBox . ItemsSource = genaccts;
						SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
						dGrid . SelectedItem = dGrid . SelectedIndex = 0;
						HandleCaption ( "" , genaccts . Count );
						listBox . SelectedIndex = 0;
						listView . SelectedIndex = 0;
					}
					listBox . Refresh ( );
					return;
				}
				else
				{
					// Empty  Generic table
					if ( type == "VIEW" )
					{
						listView . ItemsSource = genaccts;
						SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
						dGrid . SelectedItem = dGrid . SelectedIndex = 0;
						HandleCaption ( "VIEW" , genaccts . Count );
					}
					else if ( type == "BOX" )
					{
						listBox . ItemsSource = genaccts;
						SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
						dGrid . SelectedItem = dGrid . SelectedIndex = 0;
						HandleCaption ( "BOX" , genaccts . Count );
					}
					else
					{
						//Generic table type
						listView . ItemsSource = genaccts;
						listBox . ItemsSource = genaccts;
						SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
						dGrid . SelectedItem = dGrid . SelectedIndex = 0;
						HandleCaption ( "" , genaccts . Count );
					}
					ShowInfo ( header: "Unrecognised table accessed successfully" , clr4: "Red5" ,
						line1: $"Request made was completed succesfully!" , clr1: "Red3" ,
						line2: $"the table [{CurrentTableName }] that was queried returned a record count of {genaccts . Count}.\nThe structure of this data is not recognised, so a generic structure has been used..." ,
						line3: $"{SqlCommand . ToUpper ( )}" , clr3: "Blue4"
						);
				}
			}
			ShowLoadtime ( );
		}
		private void LoadGrid_NORTHWIND ( string type = "" )
		{
			// Load whatever data we have received into DataGrid
			if ( CurrentTableName . ToUpper ( ) == "CUSTOMERS" )
			{
				if ( nwcustomeraccts == null )
					return;
				if ( type == "VIEW" )
				{
					listView . ItemsSource = nwcustomeraccts;
					if ( nwcustomeraccts . Count == 0 )
						return;
					dGrid . ItemsSource = nwcustomeraccts;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					HandleCaption ( "VIEW" , nwcustomeraccts . Count );
					listView . SelectedIndex = 0;
				}
				else if ( type == "BOX" )
				{
					listBox . ItemsSource = nwcustomeraccts;
					if ( nwcustomeraccts . Count == 0 )
						return;
					dGrid . ItemsSource = nwcustomeraccts;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					HandleCaption ( "BOX" , nwcustomeraccts . Count );
					listBox . SelectedIndex = 0;
				}
				else
				{
					listView . ItemsSource = nwcustomeraccts;
					if ( nwcustomeraccts . Count == 0 )
						return;
					listBox . ItemsSource = nwcustomeraccts;
					dGrid . ItemsSource = nwcustomeraccts;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					HandleCaption ( "" , nwcustomeraccts . Count );
					listBox . SelectedIndex = 0;
					listView . SelectedIndex = 0;
				}
				ShowInfo ( line1: $"The requested table [{CurrentTableName }] was loaded successfully, and the {nwcustomeraccts . Count} records returned are displayed in the table below" , clr1: "Black0" ,
					line2: $"The command line used was" , clr2: "Red2" ,
					line3: $"{SqlCommand . ToUpper ( )}" , clr3: "Blue4" ,
					header: "Bank Accounts data table" , clr4: "Red5" );
			}
			else if ( CurrentTableName . ToUpper ( ) == "ORDERS" )
			{
				if ( nworderaccts == null )
					return;
				if ( type == "VIEW" )
				{
					listView . ItemsSource = nworderaccts;
					if ( nworderaccts . Count == 0 )
						return;
					dGrid . ItemsSource = nworderaccts;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					HandleCaption ( "VIEW" , nworderaccts . Count );
					listView . SelectedIndex = 0;
				}
				else if ( type == "BOX" )
				{
					listBox . ItemsSource = nworderaccts;
					if ( nworderaccts . Count == 0 )
						return;
					dGrid . ItemsSource = nworderaccts;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					HandleCaption ( "BOX" , nworderaccts . Count );
					listBox . SelectedIndex = 0;
				}
				else
				{
					listView . ItemsSource = nworderaccts;
					if ( nworderaccts . Count == 0 )
						return;
					listBox . ItemsSource = nworderaccts;
					dGrid . ItemsSource = nworderaccts;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					HandleCaption ( "" , nworderaccts . Count );
					listBox . SelectedIndex = 0;
					listView . SelectedIndex = 0;
				}
				listBox . SelectedIndex = 0;
				listBox . Focus ( );
			}
			else
			{
				if ( genaccts . Count > 0 )
				{
					//ShowInfo ( line1: $"The requested table [ {CurrentTableName } ] succeeded, but returned Zero rows of data." , clr1: "Green5" , header: "It is quite likely that the table is actually empty !" , clr4: "Cyan1" );
					if ( type == "VIEW" )
					{
						listView . ItemsSource = genaccts;
						SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
						dGrid . SelectedItem = dGrid . SelectedIndex = 0;
						HandleCaption ( "VIEW" , genaccts . Count );
						listView . SelectedIndex = 0;
					}
					else if ( type == "BOX" )
					{
						listBox . ItemsSource = genaccts;
						SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
						dGrid . SelectedItem = dGrid . SelectedIndex = 0;
						HandleCaption ( "BOX" , genaccts . Count );
						listBox . SelectedIndex = 0;
					}
					else
					{
						listView . ItemsSource = genaccts;
						listBox . ItemsSource = genaccts;
						SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
						dGrid . SelectedItem = dGrid . SelectedIndex = 0;
						HandleCaption ( "" , genaccts . Count );
						listBox . SelectedIndex = 0;
						listView . SelectedIndex = 0;
					}
					listBox . Refresh ( );
					return;
				}
				else
				{
					if ( type == "VIEW" )
					{
						// Caution : This loads the data into the DataGrid with only the selected rows
						// //visible in the grid so do NOT repopulate the grid after making this call
						//SqlSupport . LoadActiveRowsOnlyInLView ( listView , genaccts , DapperSupport . GetGenericColumnCount ( genaccts ) );
						listView . ItemsSource = genaccts;
						SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
						dGrid . SelectedItem = dGrid . SelectedIndex = 0;
						if ( genaccts . Count == 0 )
							return;
						listView . SelectedIndex = 0;
						listView . Focus ( );
						HandleCaption ( "VIEW" , genaccts . Count );
						listView . SelectedIndex = 0;
					}
					else if ( type == "BOX" )
					{
						listBox . ItemsSource = genaccts;
						if ( genaccts . Count == 0 )
							return;


						// Caution : This loads the data into the Datarid with only the selected rows
						// //visible in the grid so do NOT repopulate the grid after making this call
						//SqlSupport . LoadActiveRowsOnlyInLBox ( listBox , genaccts , DapperSupport . GetGenericColumnCount ( genaccts ) );
						SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
						dGrid . SelectedItem = dGrid . SelectedIndex = 0;
						listBox . SelectedIndex = 0;
						HandleCaption ( "BOX" , genaccts . Count );
					}
					else
					{
						//SqlSupport . LoadActiveRowsOnlyInLView ( listView , genaccts , DapperSupport . GetGenericColumnCount ( genaccts ) );
						listView . ItemsSource = genaccts;
						if ( genaccts . Count == 0 )
							return;
						listBox . ItemsSource = genaccts;
						SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
						dGrid . SelectedItem = dGrid . SelectedIndex = 0;
						listView . SelectedIndex = 0;
						HandleCaption ( "" , genaccts . Count );
						listBox . SelectedIndex = 0;
					}
				}
				ShowInfo ( header: "Unrecognised table accessed successfully" , clr4: "Red5" ,
					line1: $"Request made was completed succesfully!" , clr1: "Red3" ,
					line2: $"the table [{CurrentTableName }] that was queried returned a record count of {genaccts . Count}.\nThe structure of this data is not recognised, so a generic structure has been used..." ,
					line3: $"{SqlCommand . ToUpper ( )}" , clr3: "Blue4"
					);

				ShowLoadtime ( );
			}
		}
		private void LoadGrid_PUBS ( string type = "" )
		{
			// Load whatever data we have received into DataGrid
			if ( CurrentTableName . ToUpper ( ) == "AUTHORS" )
			{
				if ( pubauthors == null )
					return;
				if ( type == "VIEW" )
				{
					listView . ItemsSource = pubauthors;
					if ( pubauthors . Count == 0 )
						return;
					dGrid . ItemsSource = pubauthors;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					HandleCaption ( "VIEW" , pubauthors . Count );
					FrameworkElement elemnt = listView as FrameworkElement;
				}
				else if ( type == "BOX" )
				{
					listBox . ItemsSource = pubauthors;
					if ( pubauthors . Count == 0 )
						return;
					dGrid . ItemsSource = pubauthors;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					HandleCaption ( "BOX" , pubauthors . Count );
					listBox . SelectedIndex = 0;
				}
				else
				{
					listView . ItemsSource = pubauthors;
					if ( pubauthors . Count == 0 )
						return;
					listBox . ItemsSource = pubauthors;
					dGrid . ItemsSource = pubauthors;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					HandleCaption ( "" , pubauthors . Count );
					listBox . SelectedIndex = 0;
					listView . SelectedIndex = 0;
				}
				ShowInfo ( line1: $"The requested table [{CurrentTableName }] was loaded successfully, and the {pubauthors . Count} records returned are displayed in the table below" , clr1: "Black0" ,
					line2: $"The command line used was" , clr2: "Red2" ,
					line3: $"{SqlCommand . ToUpper ( )}" , clr3: "Blue4" ,
					header: "Bank Accounts data table" , clr4: "Red5" );
			}
			else if ( CurrentTableName . ToUpper ( ) == "ORDERS" )
			{
				if ( nworderaccts == null )
					return;
				if ( type == "VIEW" )
				{
					listView . ItemsSource = pubauthors;
					if ( pubauthors . Count == 0 )
						return;
					HandleCaption ( "VIEW" , pubauthors . Count );
					listView . SelectedIndex = 0;
				}
				else if ( type == "BOX" )
				{
					listBox . ItemsSource = pubauthors;
					if ( pubauthors . Count == 0 )
						return;
					dGrid . ItemsSource = pubauthors;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					FrameworkElement elemnt = listBox as FrameworkElement;
					HandleCaption ( "BOX" , pubauthors . Count );
					listBox . SelectedIndex = 0;
				}
				else
				{
					listView . ItemsSource = pubauthors;
					if ( pubauthors . Count == 0 )
						return;
					listBox . ItemsSource = pubauthors;
					dGrid . ItemsSource = pubauthors;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					HandleCaption ( "" , pubauthors . Count );
					listBox . SelectedIndex = 0;
					listView . SelectedIndex = 0;
				}
				listBox . SelectedIndex = 0;
				listBox . Focus ( );
			}
			else
			{
				if ( genaccts . Count > 0 )
				{
					//ShowInfo ( line1: $"The requested table [ {CurrentTableName } ] succeeded, but returned Zero rows of data." , clr1: "Green5" , header: "It is quite likely that the table is actually empty !" , clr4: "Cyan1" );
					if ( type == "VIEW" )
					{
						listView . ItemsSource = genaccts;
						if ( genaccts . Count == 0 )
							return;
						SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
						dGrid . SelectedItem = dGrid . SelectedIndex = 0;
						HandleCaption ( "VIEW" , genaccts . Count );
						listView . SelectedIndex = 0;
					}
					else if ( type == "BOX" )
					{
						listBox . ItemsSource = genaccts;
						if ( genaccts . Count == 0 )
							return;
						SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
						dGrid . SelectedItem = dGrid . SelectedIndex = 0;
						HandleCaption ( "BOX" , genaccts . Count );
						listBox . SelectedIndex = 0;
					}
					else
					{
						if ( type == "VIEW" )
						{
							// Caution : This loads the data into the DataGrid with only the selected rows
							// //visible in the grid so do NOT repopulate the grid after making this call
							//SqlSupport . LoadActiveRowsOnlyInLView ( listView , genaccts , DapperSupport . GetGenericColumnCount ( genaccts ) );
							listView . ItemsSource = genaccts;
							if ( genaccts . Count == 0 )
								return;
							SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
							dGrid . SelectedItem = dGrid . SelectedIndex = 0;
							listView . SelectedIndex = 0;
							HandleCaption ( "" , genaccts . Count );
						}
						else if ( type == "BOX" )
						{
							// Caution : This loads the data into the Datarid with only the selected rows
							// //visible in the grid so do NOT repopulate the grid after making this call
							//SqlSupport . LoadActiveRowsOnlyInLBox ( listBox , genaccts , DapperSupport . GetGenericColumnCount ( genaccts ) );
							listBox . ItemsSource = genaccts;
							if ( genaccts . Count == 0 )
								return;
							SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
							dGrid . SelectedItem = dGrid . SelectedIndex = 0;
							listBox . SelectedIndex = 0;
							HandleCaption ( "BOX" , genaccts . Count );
						}
						else
						{
							//SqlSupport . LoadActiveRowsOnlyInLView ( listView , genaccts , DapperSupport . GetGenericColumnCount ( genaccts ) );
							listView . ItemsSource = genaccts;
							if ( genaccts . Count == 0 )
								return;
							listBox . ItemsSource = genaccts;
							SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
							dGrid . SelectedItem = dGrid . SelectedIndex = 0;
							listView . SelectedIndex = 0;
							HandleCaption ( "" , genaccts . Count );
							listBox . SelectedIndex = 0;
						}
						ShowInfo ( header: "Unrecognised table accessed successfully" , clr4: "Red5" ,
							line1: $"Request made was completed succesfully!" , clr1: "Red3" ,
							line2: $"the table [{CurrentTableName }] that was queried returned a record count of {genaccts . Count}.\nThe structure of this data is not recognised, so a generic structure has been used..." ,
							line3: $"{SqlCommand . ToUpper ( )}" , clr3: "Blue4"
							);
					}
					ShowLoadtime ( );
				}
				else
				{
					// no genaccts = its empty
					DbCountlv = genaccts . Count;
					if ( type == "VIEW" )
					{
						listView . ItemsSource = genaccts;
						SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
						dGrid . SelectedItem = dGrid . SelectedIndex = 0;
						lvHeader . Text = $"List View Display :  No records returned...";
					}
					else if ( type == "BOX" )
					{
						listBox . ItemsSource = genaccts;
						SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
						dGrid . SelectedItem = dGrid . SelectedIndex = 0;
						lbHeader . Text = $"List Box Display :  No records returned...";
						dGridHeader . Text = $"List Box Display :  No records returned...";
					}
					else
					{
						listView . ItemsSource = genaccts;
						listBox . ItemsSource = genaccts;
						SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
						dGrid . SelectedItem = dGrid . SelectedIndex = 0;
						lvHeader . Text = $"List View Display :  No records returned...";
						listBox . ItemsSource = genaccts;
						lbHeader . Text = $"List Box Display :  No records returned...";
						dGridHeader . Text = $"List Box Display :  No records returned...";
					}
				}
			}
		}
		#endregion Load Viewer for the 3 different Db's

		// Load list of databases in MSSQL
		private void LoadAllDbNames ( )
		{
			int bankindex = 0, count=0;
			List<string> list = new List<string>      ();
			SqlCommand = "spGetAllDatabaseNames";
			Datagrids . CallStoredProcedure ( list , SqlCommand );
			//This call returns us a DataTable
			DataTable dt = DataLoadControl . GetDataTable ( SqlCommand );
			//			Grid2 . ItemsSource = dt . DefaultView;
			//			Grid2 . Refresh ( );
			// This how to access  Row data from  a grid the easiest way.... parsed into a List <xxxxx>
			list = Utils . GetDataDridRowsAsListOfStrings ( dt );
			foreach ( string row in list )
			{
				string entry = row.ToUpper();
				// ONLY ALLOW THE TRHEE MAIN dB'S WE ARE LIKELY TO USE.
				if ( entry . Contains ( "IAN" ) || entry . Contains ( "NORTHWIND" ) || entry . Contains ( "PUBS" ) )
				{
					DbMain . Items . Add ( row );
					if ( row . ToUpper ( ) . Contains ( "IAN1" ) )
						bankindex = count;
					count++;
				}
			}
			// how to Sort Combo/Listbox contents
			DbMain . Items . SortDescriptions . Add ( new SortDescription ( "" , ListSortDirection . Ascending ) );
			Startup = true;
			DbMain . SelectedIndex = bankindex;
			Startup = false;
			SqlCommand = DefaultSqlCommand;
		}

		//Reload  contents of table names on right click
		private void dbName_PreviewMouseRightButtonUp ( object sender , MouseButtonEventArgs e )
		{
			if ( LoadDbTables ( DbMain . SelectedItem . ToString ( ) ) == true )
			{
				ShowInfo ( header: "List of Tables in current Db reloaded successfully" , clr4: "Red5" ,
					line1: $"Request made was completed succesfully!" , clr1: "Red3"
					);
			}
			else
			{
				ShowInfo ( header: "List of Tables in current Db could not be reloaded" , clr4: "Red5" ,
					line1: $"Request failed...!" , clr1: "Red3"
					);
			}
		}

		private void ResetViewers ( string type )
		{
			// clear data collections
			if ( CurrentTableName == "BANKACCOUNT" )
				bankaccts = null;
			else if ( CurrentTableName == "CUSTOMER" )
				custaccts = null;
			else if ( CurrentTableName == "SECACCOUNTS" )
				detaccts = null;
			else if ( CurrentTableName == "GENERICTABLE" )
				genaccts = null;
		}
		private string GetCurrentDatabase ( )
		{
			string   result="";
			result = DbMain . SelectedItem . ToString ( );
			return result . ToUpper ( );
		}

		#region Load data templates
		private void LoadDataTemplates_Ian1 ( string type , string viewertype )
		{
			if ( viewertype == "VIEW" )
			{
				DataTemplatesLv . Items . Clear ( );
			}
			else if ( viewertype == "BOX" )
			{
				DataTemplatesLb . Items . Clear ( );
			}
			else
			{
				DataTemplatesLv . Items . Clear ( );
				DataTemplatesLb . Items . Clear ( );
			}

			if ( type == "BANKACCOUNT" )
			{
				if ( viewertype == "VIEW" )
				{
					DataTemplatesLv . Items . Add ( "BankDataTemplate1" );
					DataTemplatesLv . Items . Add ( "BankDataTemplate2" );
					DataTemplatesLv . SelectedIndex = 0;
					DataTemplatesLv . SelectedItem = 0;
				}
				else if ( viewertype == "BOX" )
				{
					DataTemplatesLb . Items . Add ( "BankDataTemplate1" );
					DataTemplatesLb . Items . Add ( "BankDataTemplate2" );
					DataTemplatesLb . SelectedIndex = 0;
					DataTemplatesLb . SelectedItem = 0;
				}
				else
				{
					DataTemplatesLv . Items . Add ( "BankDataTemplate1" );
					DataTemplatesLv . Items . Add ( "BankDataTemplate2" );
					DataTemplatesLv . SelectedIndex = 0;
					DataTemplatesLv . SelectedItem = 0;
					DataTemplatesLb . Items . Add ( "BankDataTemplate1" );
					DataTemplatesLb . Items . Add ( "BankDataTemplate2" );
					DataTemplatesLb . SelectedIndex = 0;
					DataTemplatesLb . SelectedItem = 0;
				}

			}
			else if ( type == "CUSTOMER" )
			{
				if ( viewertype == "VIEW" )
				{
					DataTemplatesLv . Items . Add ( "CustomersDbTemplate1" );
					DataTemplatesLv . Items . Add ( "CustomersDbTemplate2" );
					DataTemplatesLv . SelectedIndex = 0;
					DataTemplatesLv . SelectedItem = 0;
				}
				else if ( viewertype == "BOX" )
				{
					DataTemplatesLb . Items . Add ( "CustomersDbTemplate1" );
					DataTemplatesLb . Items . Add ( "CustomersDbTemplate2" );
					DataTemplatesLb . SelectedIndex = 0;
					DataTemplatesLb . SelectedItem = 0;
				}
				else
				{
					DataTemplatesLv . Items . Add ( "CustomersDbTemplate1" );
					DataTemplatesLv . Items . Add ( "CustomersDbTemplate2" );
					DataTemplatesLv . SelectedIndex = 0;
					DataTemplatesLv . SelectedItem = 0;
					DataTemplatesLb . Items . Add ( "CustomersDbTemplate1" );
					DataTemplatesLb . Items . Add ( "CustomersDbTemplate2" );
					DataTemplatesLb . SelectedIndex = 0;
					DataTemplatesLb . SelectedItem = 0;
				}
			}
			else if ( type == "SECACCOUNTS" )
			{
				DataTemplatesLv . Items . Add ( "DetailsDataTemplate1" );
				DataTemplatesLv . Items . Add ( "DetailsDataTemplate2" );
				DataTemplatesLv . SelectedIndex = 0;
				DataTemplatesLv . SelectedItem = 0;
				DataTemplatesLb . Items . Add ( "DetailsDataTemplate1" );
				DataTemplatesLb . Items . Add ( "DetailsDataTemplate2" );
				DataTemplatesLb . SelectedIndex = 0;
				DataTemplatesLb . SelectedItem = 0;
			}
			else
			{
				if ( viewertype == "VIEW" )
				{
					DataTemplatesLv . Items . Add ( "GenDataTemplate1" );
					DataTemplatesLv . Items . Add ( "GenDataTemplate2" );
					DataTemplatesLv . SelectedIndex = 0;
					DataTemplatesLv . SelectedItem = 0;
				}
				else if ( viewertype == "BOX" )
				{
					DataTemplatesLb . Items . Add ( "GenDataTemplate1" );
					DataTemplatesLb . Items . Add ( "GenDataTemplate2" );
					DataTemplatesLb . SelectedIndex = 0;
					DataTemplatesLb . SelectedItem = 0;
				}
				else
				{
					DataTemplatesLv . Items . Add ( "GenDataTemplate1" );
					DataTemplatesLv . Items . Add ( "GenDataTemplate2" );
					DataTemplatesLv . SelectedIndex = 0;
					DataTemplatesLv . SelectedItem = 0;
					DataTemplatesLb . Items . Add ( "GenDataTemplate1" );
					DataTemplatesLb . Items . Add ( "GenDataTemplate2" );
					DataTemplatesLb . SelectedIndex = 0;
					DataTemplatesLb . SelectedItem = 0;
				}
			}
		}
		private void LoadDataTemplates_NorthWind ( string type , string viewertype )
		{
			if ( viewertype == "VIEW" )
			{
				DataTemplatesLv . Items . Clear ( );
			}
			else if ( viewertype == "BOX" )
			{
				DataTemplatesLb . Items . Clear ( );
			}
			else
			{
				DataTemplatesLv . Items . Clear ( );
				DataTemplatesLb . Items . Clear ( );
			}
			if ( type == "CUSTOMERS" )
			{
				if ( viewertype == "VIEW" )
				{
					DataTemplatesLv . Items . Add ( "NwCustomersDataTemplate1" );
					DataTemplatesLv . Items . Add ( "NwCustomersDataTemplate3" );
					DataTemplatesLv . Items . Add ( "NwCustomersDataTemplate5" );
					DataTemplatesLv . Items . Add ( "NwCustomersLVDataTemplate5" );
				}
				else if ( viewertype == "BOX" )
				{
					DataTemplatesLv . Items . Add ( "NwCustomersDataTemplate1" );
					DataTemplatesLv . Items . Add ( "NwCustomersDataTemplate3" );
					DataTemplatesLv . Items . Add ( "NwCustomersDataTemplate5" );
					DataTemplatesLv . Items . Add ( "NwCustomersLVDataTemplate5" );
				}
				else
				{
					DataTemplatesLv . Items . Add ( "NwCustomersDataTemplate1" );
					DataTemplatesLv . Items . Add ( "NwCustomersDataTemplate3" );
					DataTemplatesLv . Items . Add ( "NwCustomersDataTemplate5" );
					DataTemplatesLv . Items . Add ( "NwCustomersLVDataTemplate5" );
					DataTemplatesLb . Items . Add ( "NwCustomersDataTemplate1" );
					DataTemplatesLb . Items . Add ( "NwCustomersDataTemplate3" );
					DataTemplatesLb . Items . Add ( "NwCustomersDataTemplate5" );
					DataTemplatesLb . Items . Add ( "NwCustomersLVDataTemplate5" );
				}
			}
			else if ( type == "ORDERS" )
			{
				if ( viewertype == "VIEW" )
				{
					DataTemplatesLv . Items . Add ( "NwordersComplexTemplate1" );
					DataTemplatesLv . Items . Add ( "NwordersDataTemplate1" );
					DataTemplatesLv . Items . Add ( "NwordersDataTemplate2" );
					DataTemplatesLv . Items . Add ( "NwordersDataTemplate4" );
					DataTemplatesLv . Items . Add ( "NwOrdersDataGridTemplate1" );
				}
				else if ( viewertype == "BOX" )
				{
					DataTemplatesLb . Items . Add ( "NwordersComplexTemplate1" );
					DataTemplatesLb . Items . Add ( "NwordersDataTemplate1" );
					DataTemplatesLb . Items . Add ( "NwordersDataTemplate2" );
					DataTemplatesLb . Items . Add ( "NwordersDataTemplate4" );
					DataTemplatesLb . Items . Add ( "NwOrdersDataGridTemplate1" );
				}
				else
				{
					DataTemplatesLv . Items . Add ( "NwordersComplexTemplate1" );
					DataTemplatesLv . Items . Add ( "NwordersDataTemplate1" );
					DataTemplatesLv . Items . Add ( "NwordersDataTemplate2" );
					DataTemplatesLv . Items . Add ( "NwordersDataTemplate4" );
					DataTemplatesLv . Items . Add ( "NwOrdersDataGridTemplate1" );
					DataTemplatesLb . Items . Add ( "NwordersComplexTemplate1" );
					DataTemplatesLb . Items . Add ( "NwordersDataTemplate1" );
					DataTemplatesLb . Items . Add ( "NwordersDataTemplate2" );
					DataTemplatesLb . Items . Add ( "NwordersDataTemplate4" );
					DataTemplatesLb . Items . Add ( "NwOrdersDataGridTemplate1" );
				}
			}
			else
			{
				//Generic   type
				if ( viewertype == "VIEW" )
				{
					DataTemplatesLv . Items . Add ( "GenDataTemplate1" );
					DataTemplatesLv . Items . Add ( "GenDataTemplate2" );
				}
				else if ( viewertype == "BOX" )
				{
					DataTemplatesLb . Items . Add ( "GenDataTemplate1" );
					DataTemplatesLb . Items . Add ( "GenDataTemplate2" );
				}
				else
				{
					DataTemplatesLv . Items . Add ( "GenDataTemplate1" );
					DataTemplatesLv . Items . Add ( "GenDataTemplate2" );
					DataTemplatesLb . Items . Add ( "GenDataTemplate1" );
					DataTemplatesLb . Items . Add ( "GenDataTemplate2" );
				}
			}
			if ( viewertype == "VIEW" )
			{
				DataTemplatesLv . SelectedIndex = 0;
				DataTemplatesLv . SelectedItem = 0;
			}
			else if ( viewertype == "BOX" )
			{
				DataTemplatesLb . SelectedIndex = 0;
				DataTemplatesLb . SelectedItem = 0;
			}
			else
			{
				DataTemplatesLv . SelectedIndex = 0;
				DataTemplatesLv . SelectedItem = 0;
				DataTemplatesLb . SelectedIndex = 0;
				DataTemplatesLb . SelectedItem = 0;
			}
		}
		private void LoadDataTemplates_PubAuthors ( string type , string viewertype )
		{
			if ( viewertype == "VIEW" )
			{
				DataTemplatesLv . Items . Clear ( );
			}
			else if ( viewertype == "BOX" )
			{
				DataTemplatesLb . Items . Clear ( );
			}
			else
			{
				DataTemplatesLv . Items . Clear ( );
				DataTemplatesLb . Items . Clear ( );
			}
			if ( type == "AUTHORS" )
			{
				if ( viewertype == "VIEW" )
				{
					DataTemplatesLv . Items . Add ( "PubsAuthorTemplate1" );
					DataTemplatesLv . Items . Add ( "PubsAuthorTemplate2" );
				}
				else if ( viewertype == "BOX" )
				{
					DataTemplatesLb . Items . Add ( "PubsAuthorTemplate1" );
					DataTemplatesLb . Items . Add ( "PubsAuthorTemplate2" );
				}
				else
				{
					DataTemplatesLv . Items . Add ( "PubsAuthorTemplate1" );
					DataTemplatesLv . Items . Add ( "PubsAuthorTemplate2" );
					DataTemplatesLb . Items . Add ( "PubsAuthorTemplate1" );
					DataTemplatesLb . Items . Add ( "PubsAuthorTemplate2" );
				}
			}
			else
			{
				//Generic   type
				if ( viewertype == "VIEW" )
				{
					DataTemplatesLv . Items . Add ( "GenDataTemplate1" );
					DataTemplatesLv . Items . Add ( "GenDataTemplate2" );
				}
				else if ( viewertype == "BOX" )
				{
					DataTemplatesLb . Items . Add ( "GenDataTemplate1" );
					DataTemplatesLb . Items . Add ( "GenDataTemplate2" );
				}
				else
				{
					DataTemplatesLv . Items . Add ( "GenDataTemplate1" );
					DataTemplatesLv . Items . Add ( "GenDataTemplate2" );
					DataTemplatesLb . Items . Add ( "GenDataTemplate1" );
					DataTemplatesLb . Items . Add ( "GenDataTemplate2" );
				}
			}
			if ( viewertype == "VIEW" )
			{
				DataTemplatesLv . SelectedIndex = 0;
				DataTemplatesLv . SelectedItem = 0;
			}
			else if ( viewertype == "BOX" )
			{
				DataTemplatesLb . SelectedIndex = 0;
				DataTemplatesLb . SelectedItem = 0;
			}
			else
			{
				DataTemplatesLv . SelectedIndex = 0;
				DataTemplatesLv . SelectedItem = 0;
				DataTemplatesLb . SelectedIndex = 0;
				DataTemplatesLb . SelectedItem = 0;
			}
		}
		#endregion Load data templates

		#region ALL Combo box handlers
		private void LVDataTemplate_SelectionChanged ( object sender , SelectionChangedEventArgs e )
		{
			if ( DataTemplatesLv . Items . Count == 0 )
				return;
			string selection = DataTemplatesLv.SelectedItem.ToString();
			FrameworkElement elemnt = listView as FrameworkElement;
			listView . ItemTemplate = elemnt . FindResource ( selection ) as DataTemplate;
			listView . Refresh ( );
		}
		private void LBDataTemplate_SelectionChanged ( object sender , SelectionChangedEventArgs e )
		{
			if ( DataTemplatesLb . Items . Count == 0 )
				return;
			ComboBox cb = sender as ComboBox;
			string selection = cb.SelectedItem.ToString();
			FrameworkElement elemnt = listBox as FrameworkElement;
			listBox . ItemTemplate = elemnt . FindResource ( selection ) as DataTemplate;
			listBox . Refresh ( );
		}

		private void fontsize_SelectionChanged ( object sender , SelectionChangedEventArgs e )
		{
			// Font size changing
			ComboBox cb = sender as ComboBox;
			ComboBox cbrow = rowheight as ComboBox;
			if ( cb . SelectedItem == null )
				cb . SelectedIndex = 0;
			double fontsze = 0;
			fontsze = Convert . ToDouble ( cb . SelectedItem );
			double newitemheightrequired= 0;
			double currentItemHeight = 0;
			newitemheightrequired = Fontsize + 6;
			currentItemHeight = Convert . ToDouble ( GetValue ( ItemsHeightProperty ) );
			SetValue ( FontsizeProperty , fontsze );
			dGrid . FontSize = fontsze;
		}
		private void rowheight_SelectionChanged ( object sender , SelectionChangedEventArgs e )
		{
			// Row Height changing
			ComboBox cb = sender as ComboBox;
			if ( cb . SelectedItem == null )
				cb . SelectedIndex = 0;
			double rwheight = 0;
			rwheight = Convert . ToDouble ( cb . SelectedItem );
			ItemsHeight = rwheight;
			SetValue ( ItemsHeightProperty , rwheight );
			dGrid . RowHeight = rwheight;
		}

		private void SelectCurrentDbInCombo ( string dbname , string viewertype )
		{
			ComboBoxItem cbi = new ComboBoxItem();
			string entry="";
			int indx=0;
			for ( int x = 0 ; x < dbNameLv . Items . Count ; x++ )
			{
				entry = dbNameLv . Items [ x ] . ToString ( );
				if ( entry . ToUpper ( ) == dbname )
					break;
				indx++;
			}
			ComboSelectionActive = true;
			if ( viewertype == "VIEW" )
				dbNameLv . SelectedIndex = indx;
			if ( viewertype == "BOX" )
				dbNameLb . SelectedIndex = indx;
			else
			{
				dbNameLb . SelectedIndex = indx;
				dbNameLv . SelectedIndex = indx;
			}

			ComboSelectionActive = false;
		}
		private void dbNameLb_SelectionChanged ( object sender , SelectionChangedEventArgs e )
		{
			string ResultString="";
			if ( alldone )
				return;
			if ( ComboSelectionActive || dbNameLb . Items . Count == 0 )
				return;
			string tablename = dbNameLb . SelectedItem.ToString();
			SqlCommand = $"Select *from {tablename}";
			CurrentTableName = tablename . ToUpper ( );
		}
		private void dbNameLv_SelectionChanged ( object sender , SelectionChangedEventArgs e )
		{
			string ResultString="";
			if ( alldone )
				return;
			if ( ComboSelectionActive || dbNameLv . Items . Count == 0 )
				return;
			string tablename = dbNameLv . SelectedItem.ToString();
			SqlCommand = $"Select *from {tablename}";
			CurrentTableName = tablename . ToUpper ( );
		}

		#endregion ALL Combo box handlers

		#region NorthWind	 UNUSED customer load
		//NorthWind methods    - UNUSED
		private ObservableCollection<nwcustomer> LoadNwCustomers ( string type )
		{
			if ( Startup )
				return null;
			// Set correct connection string
			if ( SetConnectionString ( "NORTHWIND" ) == false )
			{
				Console . WriteLine ( "Failed to set connection string for NorthWind Db" );
				return null;
			}
			{
				OpenNorthWindDb ( );
				nwcustomeraccts = nwc . GetNwCustomers ( );
				listView . ItemsSource = nwcustomeraccts;
				dGrid . ItemsSource = nwcustomeraccts;
				FrameworkElement elemnt = listView as FrameworkElement;
				listView . ItemTemplate = elemnt . FindResource ( "NwCustomersDataTemplate1" ) as DataTemplate;
				dGrid . SelectedItem = dGrid . SelectedIndex = 0;
				lvHeader . Text = $"List View Display : {dbNameLv?.SelectedItem?.ToString ( ) . ToUpper ( )}";
				listView . SelectedIndex = 0;
				listView . Focus ( );
				listBox . ItemTemplate = elemnt . FindResource ( "NwCustomersDataTemplate1" ) as DataTemplate;
				listBox . ItemsSource = nwcustomeraccts;
				lbHeader . Text = $"List Box Display : {dbNameLb?.SelectedItem?.ToString ( ) . ToUpper ( )}";
				dGridHeader . Text = $"DataGrid Display : {dbNameLb?.SelectedItem?.ToString ( ) . ToUpper ( )} Records = {nwcustomeraccts . Count}";
				listBox . SelectedIndex = 0;
				listBox . Focus ( );
				DbCountlb = nwcustomeraccts . Count;
				DbCountlv = nwcustomeraccts . Count;
				return nwcustomeraccts;
			}
		}
		#endregion NorthWind

		#region Make main Db Connectiions
		private bool OpenIan1Db ( )
		{
			//Set Sql Connection string up first
			if ( SetConnectionString ( "IAN1" ) == false )
			{
				Console . WriteLine ( "Failed to set connection string for Ian1 Db" );
				return false;
			}
			// Open the Ian1 Db first off
			SqlCommand = "spOpenDb_Ian1";
			if ( SqlSupport . Executestoredproc ( SqlCommand , Flags . CurrentConnectionString ) == false )
			{
				Console . WriteLine ( $"Stored procedure {SqlCommand} Failed to open IAN1.MDF" );
				Utils . DoErrorBeep ( 250 , 75 , 1 );
				SqlCommand = DefaultSqlCommand;
				return false;
			}
			SqlCommand = DefaultSqlCommand;
			return true;
		}
		private void OpenNorthWindDb ( )
		{
			//Set Sql Connectoin string up first
			if ( SetConnectionString ( "NORTHWIND" ) == false )
			{
				Console . WriteLine ( "Failed to set connection string for NorthWind Db" );
				return;
			}
			// Open the NorthWind Db first off
			SqlCommand = "spOpenDb_NorthWind";
			SqlSupport . Executestoredproc ( SqlCommand , Flags . CurrentConnectionString );
			// now load list of tabels in Northwind Db into dbMain Combo
			LoadDbTables ( "NORTHWIND" );
			SqlCommand = DefaultSqlCommand;
		}
		private void OpenPublishers ( )
		{
			//Set Sql Connectoin string up first
			if ( SetConnectionString ( "PUBS" ) == false )
			{
				Console . WriteLine ( "Failed to set connection string for Adventure WorksDb" );
				return;
			}
			// Open the Adventureworks Db first off
			SqlCommand = "spOpenDb_Publishers";
			SqlSupport . Executestoredproc ( SqlCommand , Flags . CurrentConnectionString );
			// now load list of tabels in Northwind Db
			LoadDbTables ( "PUBS" );
			SqlCommand = "Select * fom Authors order by au_fname";
		}
		#endregion Make main Db Connectiions

		#region  Load Current Db's Tables lists
		//Get list of all Tables in our Db (Ian1.MDF)
		public void LoadTablesList_Ian1 ( )
		{
			int bankindex = 0, count=0;
			List<string> list = new List<string>      ();
			dbNameLv . Items . Clear ( );
			dbNameLb . Items . Clear ( );
			SqlCommand = "spGetTablesList";
			Datagrids . CallStoredProcedure ( list , SqlCommand );
			//This call returns us a DataTable
			DataTable dt = DataLoadControl . GetDataTable ( SqlCommand );
			// This how to access  Row data from  a grid the easiest way.... parsed into a List <xxxxx>
			list = Utils . GetDataDridRowsAsListOfStrings ( dt );
			foreach ( string row in list )
			{
				dbNameLb . Items . Add ( row );
				dbNameLv . Items . Add ( row );
				if ( row . ToUpper ( ) == "BANKACCOUNT" )
					bankindex = count;
				count++;
			}
			// how to Sort Combo/Listbox contents
			dbNameLb . Items . SortDescriptions . Add ( new SortDescription ( "" , ListSortDirection . Ascending ) );
			dbNameLb . SelectedIndex = bankindex;
			dbNameLv . Items . SortDescriptions . Add ( new SortDescription ( "" , ListSortDirection . Ascending ) );
			dbNameLv . SelectedIndex = bankindex;
			SqlCommand = DefaultSqlCommand;
		}
		#endregion  Load Current Db's Tables lists

		#region Load List of Tables in current Db

		//Get list of all Tables in currently selected Db 
		public bool LoadDbTables ( string DbName )
		{
			int listindex = 0, count=0;
			List<string> list = new List<string>      ();
			DbName = DbName . ToUpper ( );
			if ( SetConnectionString ( DbName ) == false )
			{
				Console . WriteLine ( $"Failed to set connection string for {DbName} Db" );
				return false;
			}
			// All Db's have their own version of this SP.....
			SqlCommand = "spGetTablesList";

			Datagrids . CallStoredProcedure ( list , SqlCommand );
			//This call returns us a DataTable
			DataTable dt = DataLoadControl . GetDataTable ( SqlCommand );
			// This how to access Row data from  a grid the easiest way.... parsed into a List <xxxxx>
			if ( dt != null )
			{
				dbNameLb . Items . Clear ( );
				dbNameLv . Items . Clear ( );
				list = Utils . GetDataDridRowsAsListOfStrings ( dt );
				if ( DbName == "NORTHWIND" )
				{
					foreach ( string row in list )
					{
						dbNameLb . Items . Add ( row );
						dbNameLv . Items . Add ( row );
						if ( row . ToUpper ( ) == CurrentTableName )
							listindex = count;
						count++;
					}
				}
				else if ( DbName == "IAN1" )
				{
					foreach ( string row in list )
					{
						dbNameLb . Items . Add ( row );
						dbNameLv . Items . Add ( row );
						if ( row . ToUpper ( ) == CurrentTableName )
							listindex = count;
						count++;
					}
				}
				else if ( DbName == "PUBS" )
				{
					foreach ( string row in list )
					{
						dbNameLb . Items . Add ( row );
						dbNameLv . Items . Add ( row );
						if ( row . ToUpper ( ) == CurrentTableName )
							listindex = count;
						count++;
					}
				}
				// how to Sort Combo/Listbox contents
				//dbNameLv . Items . SortDescriptions . Add ( new SortDescription ( "" , ListSortDirection . Ascending ) );
				alldone = true;
				dbNameLb . SelectedIndex = listindex;
				dbNameLv . SelectedIndex = listindex;
				alldone = false;
				if ( count > 0 )
					return true;
				else
					return false;
			}
			else
			{
				MessageBox . Show ( $"SQL comand {SqlCommand} Failed..." );
				Utils . DoErrorBeep ( 125 , 55 , 1 );
				return false;
			}
			return true;
			//SqlCommand = DefaultSqlCommand;
		}

		#endregion Load List of Tables in current Db

		#region FlowDoc support
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

		#region  Dependency properties
		public double Fontsize
		{
			get { return ( double ) GetValue ( FontsizeProperty ); }
			set { SetValue ( FontsizeProperty , value ); }
		}
		public static readonly DependencyProperty FontsizeProperty =
			DependencyProperty.Register("Fontsize", typeof(double), typeof(Listviews), new PropertyMetadata((double)12));

		public double ItemsHeight
		{
			get { return ( double ) GetValue ( ItemsHeightProperty ); }
			set { SetValue ( ItemsHeightProperty , value ); }
		}
		public static readonly DependencyProperty ItemsHeightProperty =
			DependencyProperty.Register("ItemsHeight", typeof(double), typeof(Listviews), new PropertyMetadata((double)20));

		#endregion  Dependency properties

		private void Flowdoc_MouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
		{
			MovingObject = null;
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

		#endregion FlowDoc support

		#region  initialize data
		// Create dictionary of ALL Sql Connection strings
		public void LoadConnectionStrings ( )
		{
			ConnectionStringsDict . Add ( "IAN1" , ( string ) Properties . Settings . Default [ "BankSysConnectionString" ] );
			ConnectionStringsDict . Add ( "NORTHWIND" , ( string ) Properties . Settings . Default [ "NorthwindConnectionString" ] );
			//ConnectionStringsDict . Add ( "NEWBANKACCOUNT" , ( string ) Properties . Settings . Default [ "NewBanksys" ] );
			ConnectionStringsDict . Add ( "PUBS" , ( string ) Properties . Settings . Default [ "PubsConnectionString" ] );
		}
		public void LoadDefaultSqlCommands ( )
		{
			DefaultSqlCommands . Add ( "BANKACCOUNT" , "Select * from BankAccount" );
			DefaultSqlCommands . Add ( "CUSTOMER" , "Select * from Customer" );
			DefaultSqlCommands . Add ( "SECACCOUNTS" , "Select * from secAccounts" );
			DefaultSqlCommands . Add ( "NORTHWIND" , "Select * from Customers" );
			DefaultSqlCommands . Add ( "AUTHORS" , "Select * from Authors" );
			//DefaultSqlCommands . Add ( "AW.SALES.CREDITCARD" , "Select * from Sales.CreditCard" );
			//DefaultSqlCommands . Add ( "AW.SALES.SALESPERSON" , "Select * from Sales.Salesperson" );
			//DefaultSqlCommands . Add ( "AW.SALES.SALESTERRITORY" , "Select * from Sales.SalesTerritory" );
			//DefaultSqlCommands . Add ( "AW.PRODUCTION.PRODUCTREVIEW" , "Select * from Production.ProductReview" );
			//DefaultSqlCommands . Add ( "AW.PRODUCTION.PRODUCTPHOTO" , "Select * from Production.ProductPhoto" );
		}                 // Set up the connection string fo rthe approriate Db
		public bool SetConnectionString ( string key )
		{
			//string connstring = "";
			//	if ( ConnectionStringsDict . TryGetValue ( key , out connstring ) )
			if ( Utils . GetDictionaryEntry ( ConnectionStringsDict , key , out string connstring ) != "" )
			{
				CurrentSqlConnection = connstring;
				Flags . CurrentConnectionString = CurrentSqlConnection;
				return true;
			}
			else
				return false;
		}

		#endregion  initialize data

		#region utility  support methods
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
		#endregion utility  support methods

		#region Database switching
		/// <summary>
		///----------------------------------------------------------------------------------//
		///  Switching DataBases, so gotta change all other lookup tables
		///----------------------------------------------------------------------------------//
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dbMain_SelectionChanged ( object sender , SelectionChangedEventArgs e )
		{
			if ( Startup )
				return;
			if ( DbMain . Items . Count == 0 )
				return;
			// clear down viewers 1st
			listView . ItemsSource = null;
			listBox . ItemsSource = null;
			listView . Refresh ( );
			listBox . Refresh ( );
			ComboBox cb = sender as ComboBox;
			string selection = cb.SelectedItem.ToString();
			if ( selection . ToUpper ( ) == "IAN1" )
			{
				// initial access of this Db, so load BankAccount table
				if ( SetConnectionString ( "IAN1" ) == false )
				{
					Console . WriteLine ( "Failed to set connection string for IAN1 Db" );
					return;
				}
				// used to access Dictionary of DataTemplates
				OpenIan1Db ( );
				CurrentDbName = "IAN1";
				LoadDbTables ( CurrentDbName );
				CurrentTableName = "BANKACCOUNT";
				SelectCurrentDbInCombo ( "BANKACCOUNT" , "" );
				LoadDataTemplates_Ian1 ( CurrentTableName , "VIEW" );
				DefaultSqlCommand = "Select * from Bankaccount";
				SqlCommand = DefaultSqlCommand;
				CurrentDataTable = DataTemplatesLv . Items [ 0 ] . ToString ( );

				//Now setup the UI as needed
				listView . ItemsSource = null;
				// Load  the data from the Table via SQL
				LoadData_Ian1 ( "VIEW" );

				FrameworkElement elemnt = listView as FrameworkElement;
				listView . ItemTemplate = elemnt . FindResource ( CurrentDataTable ) as DataTemplate;
				listView . ItemsSource = bankaccts;
				dGrid . ItemsSource = bankaccts;
				dGrid . SelectedItem = dGrid . SelectedIndex = 0;
				lvHeader . Text = $"List View Display : {DbMain . SelectedItem . ToString ( )} : {dbNameLv?.SelectedItem?.ToString ( ) . ToUpper ( )}";
				listView . SelectedIndex = 0;
				listView . Focus ( );
				listBox . ItemTemplate = elemnt . FindResource ( CurrentDataTable ) as DataTemplate;
				listBox . ItemsSource = bankaccts;
				lbHeader . Text = $"List Box Display : {DbMain . SelectedItem . ToString ( )} : {dbNameLb?.SelectedItem?.ToString ( ) . ToUpper ( )}";
				dGridHeader . Text = $"DataGrid Display : {DbMain . SelectedItem . ToString ( )} : {dbNameLb?.SelectedItem?.ToString ( ) . ToUpper ( )} Records = {bankaccts . Count}";
				listBox . SelectedIndex = 0;
				DbCountlb = bankaccts . Count;
				DbCountlv = bankaccts . Count;
			}
			else if ( selection . ToUpper ( ) == "NORTHWIND" )
			{
				// Just open the New DB
				if ( SetConnectionString ( "NORTHWIND" ) == false )
				{
					Console . WriteLine ( "Failed to set connection string for NorthWind Db" );
					return;
				}
				OpenNorthWindDb ( );
				CurrentDbName = "NORTHWIND";
				CurrentTableName = "CUSTOMERS";
				LoadDbTables ( CurrentDbName );

				LoadDataTemplates_NorthWind ( CurrentTableName , "" );
				SelectCurrentDbInCombo ( CurrentTableName , "" );
				DefaultSqlCommand = "Select * from Customers";
				SqlCommand = DefaultSqlCommand;
				listView . ItemsSource = null;
				// Load  the data from the Table via SQL
				nwcustomeraccts = nwc . GetNwCustomers ( );

				//Now setup the UI as needed
				FrameworkElement elemnt = listView as FrameworkElement;
				listView . ItemTemplate = elemnt . FindResource ( "NwCustomersDataTemplate1" ) as DataTemplate;
				listView . ItemsSource = nwcustomeraccts;
				dGrid . ItemsSource = nwcustomeraccts;
				dGrid . SelectedItem = dGrid . SelectedIndex = 0;
				lvHeader . Text = $"List View Display : {DbMain . SelectedItem . ToString ( )} : {dbNameLv?.SelectedItem?.ToString ( ) . ToUpper ( )}";
				listView . SelectedIndex = 0;
				listView . Focus ( );
				listBox . ItemTemplate = elemnt . FindResource ( "NwCustomersDataTemplate1" ) as DataTemplate;
				listBox . ItemsSource = nwcustomeraccts;
				lbHeader . Text = $"List Box Display : {DbMain . SelectedItem . ToString ( )} : {dbNameLb?.SelectedItem?.ToString ( ) . ToUpper ( )}";
				dGridHeader . Text = $"DataGrid Display : {DbMain . SelectedItem . ToString ( )} : {dbNameLb?.SelectedItem?.ToString ( ) . ToUpper ( )} Records = {nwcustomeraccts . Count}";
				listBox . SelectedIndex = 0;
				listBox . Focus ( );
				DbCountlb = nwcustomeraccts . Count;
				DbCountlv = nwcustomeraccts . Count;
			}
			else if ( selection . ToUpper ( ) == "PUBS" )
			{
				listView . ItemsSource = null;
				OpenPublishers ( );
				CurrentDbName = "PUBS";
				CurrentTableName = "AUTHORS";

				LoadDataTemplates_PubAuthors ( CurrentTableName , "" );

				SelectCurrentDbInCombo ( CurrentTableName , "" );
				DefaultSqlCommand = "Select * from Authors ";
				SqlCommand = DefaultSqlCommand;
				listView . ItemsSource = null;
				// Load  the data from the Table via SQL
				pubauthors = PubAuthors . LoadPubAuthors ( pubauthors , false );

				//Now setup the UI as needed
				FrameworkElement elemnt = listView as FrameworkElement;
				listView . ItemTemplate = elemnt . FindResource ( "PubsAuthorTemplate1" ) as DataTemplate;
				listView . ItemsSource = pubauthors;
				dGrid . ItemsSource = pubauthors;
				dGrid . SelectedItem = dGrid . SelectedIndex = 0;

				lvHeader . Text = $"List View Display : {DbMain . SelectedItem . ToString ( )} : {dbNameLv?.SelectedItem?.ToString ( ) . ToUpper ( )}";
				listView . SelectedIndex = 0;
				listView . Focus ( );
				listBox . ItemTemplate = elemnt . FindResource ( "PubsAuthorTemplate1" ) as DataTemplate;
				listBox . ItemsSource = pubauthors;
				lbHeader . Text = $"List Box Display : {DbMain . SelectedItem . ToString ( )} : {dbNameLb?.SelectedItem?.ToString ( ) . ToUpper ( )}";
				dGridHeader . Text = $"DataGrid Display : {DbMain . SelectedItem . ToString ( )} : {dbNameLb?.SelectedItem?.ToString ( ) . ToUpper ( )} Records = {pubauthors . Count}";
				listBox . SelectedIndex = 0;
				listBox . Focus ( );
				DbCountlb = pubauthors . Count;
				DbCountlv = pubauthors . Count;
			}
		}

		#endregion Databse switching

		#region  Reload Data viewers
		private void ReloadListview ( object sender , RoutedEventArgs e )
		{
			ResetViewers ( "VIEW" );
			listView . ItemsSource = null;
			DbCountlv = 0;
			listView . Refresh ( );
			// Set flag  to ignore limits check
			LoadAll = true;
			string currdb = GetCurrentDatabase ( );
			CurrentTableName = dbNameLv . SelectedItem . ToString ( ) . ToUpper ( );

			if ( currdb == "IAN1" )
			{
				LoadData_Ian1 ( "VIEW" );
				LoadGrid_IAN1 ( "VIEW" );
			}
			else if ( currdb == "NORTHWIND" )
			{
				LoadData_NorthWind ( "VIEW" );
				LoadGrid_NORTHWIND ( "VIEW" );
			}
			else if ( currdb == "PUBS" )
			{
				genaccts = null;
				LoadData_Publishers ( "VIEW" , out genaccts );
				if ( genaccts != null )
				{
					listView . ItemsSource = genaccts;
					SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					DbCountlv = genaccts . Count;
				}
				else
				{
					pubauthors = PubAuthors . LoadPubAuthors ( pubauthors , false );
					listView . ItemsSource = pubauthors;
					dGrid . ItemsSource = pubauthors;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					DbCountlv = pubauthors . Count;
				}
				LoadGrid_PUBS ( "VIEW" );
			}
			listView . SelectedIndex = 0;
			listView . SelectedItem = 0;
			// Clear flag again
			LoadAll = false;
		}

		private void ReloadListbox ( object sender , RoutedEventArgs e )
		{
			ResetViewers ( "BOX" );
			listBox . ItemsSource = null;
			listBox . Refresh ( );
			DbCountlb = 0;
			// Set flag  to ignore limits check
			LoadAll = true;
			string currdb = GetCurrentDatabase ( );
			CurrentTableName = dbNameLb . SelectedItem . ToString ( ) . ToUpper ( );

			if ( currdb == "IAN1" )
			{
				LoadData_Ian1 ( "BOX" );
				LoadGrid_IAN1 ( "BOX" );
			}
			else if ( currdb == "NORTHWIND" )
			{
				LoadData_NorthWind ( "BOX" );
				LoadGrid_NORTHWIND ( "BOX" );
			}
			else if ( currdb == "PUBS" )
			{
				genaccts = null;
				LoadData_Publishers ( "BOX" , out genaccts );
				if ( genaccts != null )
				{
					listBox . ItemsSource = genaccts;
					SqlServerCommands . LoadActiveRowsOnlyInGrid ( dGrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					DbCountlb = genaccts . Count;
				}
				else
				{
					pubauthors = PubAuthors . LoadPubAuthors ( pubauthors , false );
					listBox . ItemsSource = pubauthors;
					dGrid . ItemsSource = pubauthors;
					dGrid . SelectedItem = dGrid . SelectedIndex = 0;
					DbCountlb = pubauthors . Count;
				}
				LoadGrid_PUBS ( "BOX" );
			}
			listBox . SelectedIndex = 0;
			listBox . SelectedItem = 0;
			LoadAll = false;
		}
		#endregion  Reload Data viewers
		private string GetDefaultSqlCommand ( string CurrentType )
		{
			string result="";
			foreach ( KeyValuePair<string , string> pair in DefaultSqlCommands )
			{
				if ( pair . Key == CurrentType . ToUpper ( ) )
				{
					result = pair . Value;
					SqlCommand = result;
					break;
				}
			}
			return result;
		}

		#region Checkboxes
		private void ShowInfo_Click ( object sender , RoutedEventArgs e )
		{
			UseFlowdoc = !UseFlowdoc;
		}

		private void scrollview_Click ( object sender , RoutedEventArgs e )
		{
			UseScrollViewer = !UseScrollViewer;
			Flags . UseScrollView = UseScrollViewer;
		}
		#endregion Checkboxes

		private void ViewTableColumnsLb ( object sender , RoutedEventArgs e )
		{
			bool flowdocswitch = false;
			int count = 0;
			List<string> list = new List<string>      ();
			string output="";
			SqlCommand = $"spGetTableColumns {dbNameLb . SelectedItem . ToString ( )}";
			Datagrids . CallStoredProcedure ( list , SqlCommand );
			//This call returns us a DataTable
			DataTable dt = DataLoadControl . GetDataTable ( SqlCommand );
			// This how to access  Row data from  a grid the easiest way.... parsed into a List <xxxxx>
			list = Utils . GetTableColumnsList ( dt );
			foreach ( string row in list )
			{
				string entry = row.ToUpper();
				output += row + "\n";
				count++;
			}
			// Fiddle  to allow Flowdoc  to show Field info even though Flowdoc use is disabled
			if ( UseFlowdoc == false )
			{
				flowdocswitch = true;
				UseFlowdoc = true;
			}
			Console . WriteLine ( $"loaded {count} records for table columns" );
			ShowInfo ( header: "Table Columns informaton accessed successfully" , clr4: "Red5" ,
				line1: $"Request made was completed succesfully!" , clr1: "Red3" ,
				line2: $"the structure of the table [{dbNameLb . SelectedItem . ToString ( ) }] is listed below : \n{output}" ,
				line3: $"Results created by Stored Procedure : \n({SqlCommand . ToUpper ( )})" , clr3: "Blue4"
				);
			if ( flowdocswitch == true )
			{
				flowdocswitch = false;
				UseFlowdoc = false;
			}
		}
		private void ViewTableColumnsLv ( object sender , RoutedEventArgs e )
		{
			bool flowdocswitch = false;
			int count = 0;
			List<string> list = new List<string>      ();
			string output="";
			SqlCommand = $"spGetTableColumns {dbNameLv . SelectedItem . ToString ( )}";
			Datagrids . CallStoredProcedure ( list , SqlCommand );
			//This call returns us a DataTable
			DataTable dt = DataLoadControl . GetDataTable ( SqlCommand );
			// This how to access  Row data from  a grid the easiest way.... parsed into a List <xxxxx>
			list = Utils . GetTableColumnsList ( dt );
			foreach ( string row in list )
			{
				string entry = row.ToUpper();
				output += row + "\n";
				count++;
			}
			Console . WriteLine ( $"loaded {count} records for table columns" );
			// Fiddle  to allow Flowdoc  to show Field info even though Flowdoc use is disabled
			if ( UseFlowdoc == false )
			{
				flowdocswitch = true;
				UseFlowdoc = true;
			}
			ShowInfo ( header: "Table Columns informaton accessed successfully" , clr4: "Red5" ,
				line1: $"Request made was completed succesfully!" , clr1: "Red3" ,
				line2: $"the structure of the table [{dbNameLv . SelectedItem . ToString ( ) }] is listed below : \n{output}" ,
				line3: $"Results created by Stored Procedure : \n({SqlCommand . ToUpper ( )})" , clr3: "Blue4"
				);
			if ( flowdocswitch == true )
			{
				flowdocswitch = false;
				UseFlowdoc = false;
			}
		}

		// display relevant info when a different table is selected
		private void HandleCaption ( string viewertype , int reccount )
		{
			FrameworkElement elemnt;
			if ( viewertype == "VIEW" || viewertype == "" )
			{
				elemnt = listView as FrameworkElement;
				listView . ItemTemplate = elemnt . FindResource ( DataTemplatesLv . SelectedItem ) as DataTemplate;
				lvHeader . Text = $"List View Display : {DbMain . SelectedItem . ToString ( )} : {dbNameLv?.SelectedItem?.ToString ( ) . ToUpper ( )}";
				dGridHeader . Text = $"DataGrid Display : {DbMain . SelectedItem . ToString ( )} : {dbNameLv?.SelectedItem?.ToString ( ) . ToUpper ( )} Records = {reccount}";
				DbCountlv = reccount;
			}
			else if ( viewertype == "BOX" || viewertype == "" )
			{
				elemnt = listBox as FrameworkElement;
				listBox . ItemTemplate = elemnt . FindResource ( DataTemplatesLb?.SelectedItem ) as DataTemplate;
				lbHeader . Text = $"List Box Display : {DbMain . SelectedItem . ToString ( )} : {dbNameLb?.SelectedItem?.ToString ( ) . ToUpper ( )}";
				dGridHeader . Text = $"DataGrid Display : {DbMain . SelectedItem . ToString ( )} : {dbNameLb?.SelectedItem?.ToString ( ) . ToUpper ( )} Records = {reccount}";
				DbCountlb = reccount;
			}
			else
			{
				elemnt = listView as FrameworkElement;
				listView . ItemTemplate = elemnt . FindResource ( DataTemplatesLv?.SelectedItem ) as DataTemplate;
				lvHeader . Text = $"List View Display : {DbMain . SelectedItem . ToString ( )} : {dbNameLv?.SelectedItem?.ToString ( ) . ToUpper ( )}";
				dGridHeader . Text = $"DataGrid Display : {DbMain . SelectedItem . ToString ( )} : {dbNameLv?.SelectedItem?.ToString ( ) . ToUpper ( )} Records = {reccount}";
				DbCountlv = reccount;
				elemnt = listBox as FrameworkElement;
				listBox . ItemTemplate = elemnt . FindResource ( DataTemplatesLb?.SelectedItem ) as DataTemplate;
				lbHeader . Text = $"List Box Display : {DbMain . SelectedItem . ToString ( )} : {dbNameLb?.SelectedItem?.ToString ( ) . ToUpper ( )}";
				dGridHeader . Text = $"DataGrid Display : {DbMain . SelectedItem . ToString ( )} : {dbNameLb?.SelectedItem?.ToString ( ) . ToUpper ( )} Records = {reccount}";
				DbCountlb = reccount;
			}
		}

		#region Hook to Flowdoc Maximize buttoin
		protected void ParentWPF_method ( object sender , EventArgs e )
		{
			// Clever "Hook" method that Allows the flowdoc to be resized to fill window
			// or return to its original size and position courtesy of the Event declard in FlowDoc
			double height = canvas . Height;
			double width = canvas . Width;
			if ( Flowdoc . Height < canvas . Height && Flowdoc . Width < canvas . Width )
			{
				// it is in NORMAL moode right now
				// Set flowdoc size into variables for later use
				flowdocHeight = Flowdoc . Height;
				flowdocWidth = Flowdoc . Width;
				flowdocLeft = ( double ) GetValue ( Canvas . LeftProperty );
				flowdocTop = ( double ) GetValue ( Canvas . TopProperty );
				( Flowdoc as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) 0 );
				( Flowdoc as FrameworkElement ) . SetValue ( Canvas . TopProperty , ( double ) 0 );
				Flowdoc . Height = height;
				Flowdoc . Width = width;
			}
			else
			{
				// it is MAXIMIZED right now
				// We re returning it to normal position/Size
				Flowdoc . Height = flowdocHeight;
				Flowdoc . Width = flowdocWidth;
				if ( Flags . PinToBorder )
				{
					( Flowdoc as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) 0 );
					( Flowdoc as FrameworkElement ) . SetValue ( Canvas . TopProperty , ( double ) 0 );
				}
				else
				{
					( Flowdoc as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) 150 );
					( Flowdoc as FrameworkElement ) . SetValue ( Canvas . TopProperty , ( double ) 100 );
				}
				//if ( ParkFlowDoc )
				//{
				//	flowdocLeft = 0;
				//	flowdocTop = 0;
				//} else
				//{
				//	flowdocLeft = ( double ) GetValue ( Canvas . LeftProperty );
				//	flowdocTop = ( double ) GetValue ( Canvas . TopProperty );
				//}
			}
		}
		#endregion Hook to Flowdoc Maximize buttoin

		private void Park_Click ( object sender , RoutedEventArgs e )
		{
			Flags . PinToBorder = !Flags . PinToBorder;
		}

		private void Arrow1_PreviewMouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
		{
			if ( PickColors . Visibility == Visibility . Visible )
			{

				PickColors . Visibility = Visibility . Hidden;
				PickColors . RedSlider . Focus ( );
			}
			else
			{
				PickColors . Visibility = Visibility . Visible;

				PickColors . Focus ( );
			}
		}

		#region Event handler rom ColorPicker button - save to clipboard
		private void Colorpicker_ExecuteSaveToClipboardMethod ( object sender , ColorpickerArgs e )
		{
			Clipboard . SetText ( e.RgbString );
		}
		#endregion Event handler rom ColorPicker button - save to clipboard

		#region Move ColorPicker
		// NOT USED
		private void Colorpicker_ExecuteMoveMethod ( object sender , EventArgs e )
		{
			// Clever "Hook" method that Allows the flowdoc to be resized to fill window
			// or return to its original size and position courtesy of the Event declard in FlowDoc
			//double height = canvas . Height;
			//double width = canvas . Width;
			//if ( PickColors . Height < canvas . Height && Flowdoc . Width < canvas . Width )
			//{
			//	// it is in NORMAL moode right now
			//	// Set flowdoc size into variables for later use
			//	//flowdocHeight = Flowdoc . Height;
			//	//flowdocWidth = Flowdoc . Width;
			//	CpFirstXPos = ( double ) GetValue ( Canvas . LeftProperty );
			//	CpFirstYPos = ( double ) GetValue ( Canvas . TopProperty );
			//	( PickColors as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) CpFirstXPos );
			//	( PickColors  as FrameworkElement ) . SetValue ( Canvas . TopProperty , ( double ) CpFirstYPos );
			//	//Flowdoc . Height = height;
			//	//Flowdoc . Width = width;
			//}
			//else
			//{
			//	// it is MAXIMIZED right now
			//	// We re returning it to normal position/Size
			//	Flowdoc . Height = flowdocHeight;
			//	Flowdoc . Width = flowdocWidth;
			//	if ( Flags . PinToBorder )
			//	{
			//		( Flowdoc as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) 0 );
			//		( Flowdoc as FrameworkElement ) . SetValue ( Canvas . TopProperty , ( double ) 0 );
			//	}
			//	else
			//	{
			//		( Flowdoc as FrameworkElement ) . SetValue ( Canvas . LeftProperty , ( double ) 150 );
			//		( Flowdoc as FrameworkElement ) . SetValue ( Canvas . TopProperty , ( double ) 100 );
			//	}
			//	//if ( ParkFlowDoc )
			//	//{
			//	//	flowdocLeft = 0;
			//	//	flowdocTop = 0;
			//	//} else
			//	//{
			//	//	flowdocLeft = ( double ) GetValue ( Canvas . LeftProperty );
			//	//	flowdocTop = ( double ) GetValue ( Canvas . TopProperty );
			//	//}
			//}
		}
		private void PickColors_MouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
		{
			ColorpickerObject = null;
		}   	
		private void PickColors_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
		{
			//In this event, we get current mouse position on the control to use it in the MouseMove event.
			if ( PickColors.RedSlider.IsMouseOver == true 
				|| PickColors . GreenSlider . IsMouseOver == true 
				|| PickColors . BlueSlider . IsMouseOver == true 
				|| PickColors . OpacitySlider . IsMouseOver == true 
				|| PickColors . exitbtn. IsMouseOver == true 
				|| PickColors . ClipboardSave . IsMouseOver == true )
			{
				// Dont capture mouse in our cotrols
				return;
			}  			
			CpFirstXPos = e . GetPosition ( sender as Control ) . X;
			CpFirstYPos = e . GetPosition ( sender as Control ) . Y;
			double FirstArrowXPos = e . GetPosition ( ( sender as Control ) . Parent as Control ) . X - CpFirstXPos;
			double FirstArrowYPos = e . GetPosition ( ( sender as Control ) . Parent as Control ) . Y - CpFirstYPos;
			ColorpickerObject = sender;
		}
		private void PickColors_MouseMove ( object sender , MouseEventArgs e )
		{
			if ( ColorpickerObject != null && e . LeftButton == MouseButtonState . Pressed )
			{
				///var v = e . Source;
				// Get mouse position IN PickColor !!
				double left = e . GetPosition ( ( ColorpickerObject  as FrameworkElement ) . Parent as FrameworkElement ) . X - CpFirstXPos ;
				double top = e . GetPosition ( ( ColorpickerObject as FrameworkElement ) . Parent as FrameworkElement ) . Y - CpFirstYPos ;
				double trueleft = left - CpFirstXPos;
				double truetop = left - CpFirstYPos;
				if ( left >= 0 ) // && left <= canvas.ActualWidth - Flowdoc.ActualWidth)
					( ColorpickerObject as FrameworkElement ) . SetValue ( Canvas . LeftProperty , left );
				if ( top >= 0 ) //&& top <= canvas . ActualHeight- Flowdoc. ActualHeight)
					( ColorpickerObject as FrameworkElement ) . SetValue ( Canvas . TopProperty , top );
				Console . WriteLine ($"left={left}, Top = {top}");	 			
			}
		}
		#endregion Move ColorPicker
		private void ListViewWindow_Closed ( object sender , EventArgs e )
		{											  
			Colorpicker . ExecuteSaveToClipboardMethod -= Colorpicker_ExecuteSaveToClipboardMethod;
//			Colorpicker . ExecuteMoveMethod -= Colorpicker_ExecuteMoveMethod;
			Flowdoc . ExecuteFlowDocSizeMethod -= new EventHandler ( ParentWPF_method );

		}

		private void LoadColorpicker ( object sender , RoutedEventArgs e )
		{
			if ( PickColors . Visibility == Visibility . Visible )
			{

				PickColors . Visibility = Visibility . Hidden;
				PickColors . RedSlider . Focus ( );
			}
			else
			{
				PickColors . Visibility = Visibility . Visible;

				PickColors . Focus ( );
			}
		}
	}
}
