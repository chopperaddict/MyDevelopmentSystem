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
	/// Interaction logic for DapperTesting.xaml
	/// </summary>
	/// <summary>
	/// Interaction logic for DapperTesting.xaml
	/// </summary>
	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
	public partial class DapperTesting : Window
	{
		//#region DECLARATIONS
		//{
		//	public static ObservableCollection<BankCombinedViewModel> bcvm = new ObservableCollection<BankCombinedViewModel>();
		//	public static ObservableCollection<BankAccountViewModel> bvm = new ObservableCollection<BankAccountViewModel>();
		//	public static ObservableCollection<CustomerViewModel> cvm = new ObservableCollection<CustomerViewModel>();
		//	public static ObservableCollection<GenericClass> selectedgenerics = new ObservableCollection<GenericClass>();
		//	public static ObservableCollection<DetailsViewModel> dvm = new ObservableCollection<DetailsViewModel>();
		//	private bool IsGenericListResult = false;
		//	private List<string> genericlist = new List<string>   ();
		//	private string ActiveLoadMethod = "USESDAPPERSTDPROCEDURES";
		//	public  DispatcherTimer timer1 = new DispatcherTimer();
		//	public  DispatcherTimer timer2 = new DispatcherTimer();
		//	public  DispatcherTimer timer3 = new DispatcherTimer();
		//	private bool UseAsyncLoading = true;
		//	private int startsecs1 = 0;
		//	private int endsecs1 = 0;
		//	private int startsecs2 = 0;
		//	private int endsecs2 = 0;
		//	private int startsecs3 = 0;
		//	private int endsecs3 = 0;
		//	int [ ] args= {0,0,0};

		//	// Temp declaration
		//	bool CreateCombinedDb = true;
		//	static bool UsebackgroundWorker { get; set; }
		//	static private int ToggleBtnStatus { get; set; }
		//	bool[] status = { false , false , false } ;
		//	public struct ToggleFlags
		//	{
		//		public int current;
		//	}
		//	ToggleFlags tFlags = new ToggleFlags();

		//	Msgboxs mbox = new Msgboxs();
		//	public double Checked
		//	{
		//		get
		//		{ return ( double ) GetValue ( CheckedProperty ); }
		//		set { SetValue ( CheckedProperty , value ); }
		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	public static readonly DependencyProperty CheckedProperty =
		//			DependencyProperty . Register ( "Checked",
		//			typeof ( int ),
		//			typeof ( DapperTesting),
		//			new PropertyMetadata ( ( int) 0 ), OnCheckedPropertyChanged );

		//	#endregion DECLARATIONS

		//	public static ObservableCollection<BankAccountViewModel> bvmcollection = new ObservableCollection<BankAccountViewModel>();
		//	public static ObservableCollection<CustomerViewModel> cvmcollection = new ObservableCollection<CustomerViewModel>();
		//	public static ObservableCollection<DetailsViewModel> dvmcollection  = new  ObservableCollection<DetailsViewModel> ();
		//	private static object _lock = new object();

		//	#region BASIC SETUP
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private static bool OnCheckedPropertyChanged ( object value )
		//	{
		//		int x = Convert.ToInt32(value);
		//		ToggleBtnStatus = ( x );

		//		return true;
		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	public DapperTesting ( )
		//	{
		//		Mouse . OverrideCursor = Cursors . Wait;
		//		InitializeComponent ( );
		//	}
		//	public override void OnApplyTemplate ( )
		//	{
		//		base . OnApplyTemplate ( );

		//		if ( Template != null )
		//		{
		//			var v = this . GetTemplateChild ( "MyEllipse" );
		//		}

		//		//UpdateStates ( false ); // Not sure what this does ?
		//		return;
		//	}
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void Timer_Tick ( object sender , EventArgs e )
		//	{
		//		endsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//	}

		//	#endregion BASIC SETUP

		//	#region STARTUP/CLOSEDOWN  METHODS
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void Window_Loaded ( object sender , RoutedEventArgs e )
		//	{
		//		Mouse . OverrideCursor = Cursors . Wait;
		//		// Handle window dragging
		//		Utils . SetupWindowDrag ( this );
		//		UseBackgroundWorker . IsChecked = true;
		//		UsebackgroundWorker = true;
		//		UseStdDapper . IsChecked = false;
		//		UseDapperStoredProc . IsChecked = false;
		//		UseStoredProc . IsChecked = false;

		//		// This code is required to enable BackGroundWorker system for some reason
		//		Utils . SetSynchforDbCollections ( _lock , bvmcollection , cvmcollection , dvmcollection );
		//		LoadBankData ( );
		//		LoadCustomers ( );
		//		LoadDetails ( );

		//		this . Show ( );
		//		args [ 0 ] = 0;
		//		args [ 1 ] = 1700000;
		//		args [ 2 ] = 0;
		//		UseAsync . IsChecked = true;
		//		// Setup toggle button status  to indeterminate (0)
		//		tFlags . current = 0;
		//		Flags . USEADOWITHSTOREDPROCEDURES = true;
		//		EventControl . BankDataLoaded += EventControl_BankDataLoaded;
		//		EventControl . CustDataLoaded += EventControl_CustDataLoaded;
		//		EventControl . DetDataLoaded += EventControl_DetDataLoaded;
		//		ActiveLoadMethod = "USESDAPPERSTOREDPROCEDURE";
		//		BankDb . Text = "BANKACCOUNT";
		//		CustDb . Text = "CUSTOMER";
		//		DetDb . Text = "SECACCOUNTS";
		//		CurrBank . Content = BankDb . Text . ToUpper ( );
		//		CurrCust . Content = CustDb . Text . ToUpper ( );
		//		CurrDet . Content = DetDb . Text . ToUpper ( );
		//		GenericGrid1 . Background = FindResource ( "Red5" ) as SolidColorBrush;
		//		GenericGrid2 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//		GenericGrid3 . Background = FindResource ( "White0" ) as SolidColorBrush;

		//		tFlags . current = 2;
		//		ToggleBtn_Click ( null , null );
		//		SetDummyGridEntries ( );

		//		GenericGrid1 . Focus ( );
		//		string errormsg="";
		//		MouseMove += Grab_MouseMove;
		//		KeyDown += Window_PreviewKeyDown;
		//		Mouse . OverrideCursor = Cursors . Arrow;
		//	}
		//	#endregion STARTUP/CLOSEDOWN  METHODS


		//	// Call BackgroundWorker thread s
		//	private void RunBackgroundProcess ( string dbtype )
		//	{
		//		if ( dbtype == "BANK" )
		//		{
		//			bvmcollection . Clear ( );
		//			this . Dispatcher . BeginInvoke ( new Action ( ( ) =>
		//			{
		//				BackgroundWorker  Bankworker = new BackgroundWorker();
		//				Bankworker . DoWork += new DoWorkEventHandler ( worker_DoWorkBank );
		//				Bankworker . RunWorkerCompleted += new RunWorkerCompletedEventHandler ( worker_RunBankWorkerCompleted );
		//				timer1 . Start ( );
		//				startsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				Bankworker . RunWorkerAsync ( );
		//			} ) );
		//		}
		//		else if ( dbtype == "CUSTOMER" )
		//		{
		//			cvmcollection . Clear ( );
		//			this . Dispatcher . BeginInvoke ( new Action ( ( ) =>
		//			{
		//				BackgroundWorker  Customerworker = new BackgroundWorker();
		//				Customerworker . DoWork += new DoWorkEventHandler ( worker_DoWorkCustomer );
		//				Customerworker . RunWorkerCompleted += new RunWorkerCompletedEventHandler ( worker_RunCustomerWorkerCompleted );
		//				Customerworker . RunWorkerAsync ( );
		//			} ) );
		//			timer2 . Start ( );
		//			startsecs2 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//		}
		//		else if ( dbtype == "DETAILS" )
		//		{
		//			dvmcollection . Clear ( );
		//			this . Dispatcher . BeginInvoke ( new Action ( ( ) =>
		//			{
		//				BackgroundWorker  Detailsworker = new BackgroundWorker();
		//				Detailsworker . DoWork += new DoWorkEventHandler ( worker_DoWorkDetails );
		//				Detailsworker . RunWorkerCompleted += new RunWorkerCompletedEventHandler ( worker_RunDetailsWorkerCompleted );
		//				timer3 . Start ( );
		//				startsecs3 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				Detailsworker . RunWorkerAsync ( );
		//			} ) );
		//		}
		//	}

		//	private void Window_PreviewKeyDown ( object sender , KeyEventArgs e )
		//	{
		//		if ( e . Key == Key . F11 )
		//		{
		//			var pos = Mouse . GetPosition ( this);
		//			Utils . Grab_Object ( sender , pos );
		//			if ( Utils . ControlsHitList . Count == 0 )
		//				return;
		//			Utils . Grabscreen ( this , Utils . ControlsHitList [ 0 ] . VisualHit , null , sender as Control );
		//			e . Handled = true;
		//		}
		//	}

		//	#region Async Data loaded /Load Grids handlers
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void EventControl_BankDataLoaded ( object sender , LoadedEventArgs e )
		//	{
		//		bvm = e . DataSource as ObservableCollection<BankAccountViewModel>;
		//		GenericGrid1 . ItemsSource = bvm;
		//		GenericGrid1 . UpdateLayout ( );
		//		GenericGrid1 . Background = FindResource ( "Red5" ) as SolidColorBrush;
		//		timer1 . Stop ( );
		//		endsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//		//if ( endsecs1 - startsecs1 < 0 )
		//		//	Debugger . Break ( );
		//		//LoadTime . Text = ( endsecs1 - startsecs1 ) . ToString ( ) + " Milliseconds";
		//		BankCount . Text = bvm . Count . ToString ( );
		//		//			CustCount . Text = cvm . Count . ToString ( );
		//		//			DetCount . Text = dvm . Count . ToString ( );
		//		CurrBank . Content = BankDb . Text . ToUpper ( );
		//	}
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void EventControl_CustDataLoaded ( object sender , LoadedEventArgs e )
		//	{
		//		cvm = e . DataSource as ObservableCollection<CustomerViewModel>;
		//		GenericGrid2 . ItemsSource = cvm;
		//		GenericGrid2 . UpdateLayout ( );
		//		GenericGrid2 . Background = FindResource ( "Red5" ) as SolidColorBrush;
		//		timer2 . Stop ( );
		//		endsecs2 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//		//if ( endsecs2 - startsecs2 < 0 )
		//		//	Debugger . Break ( );
		//		LoadTime . Text = ( endsecs2 - startsecs2 ) . ToString ( ) + " Milliseconds";
		//		CustCount . Text = cvm . Count . ToString ( );
		//		//			DetCount . Text = cvm . Count . ToString ( );
		//		CurrCust . Content = CustDb . Text . ToUpper ( );
		//	}
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void EventControl_DetDataLoaded ( object sender , LoadedEventArgs e )
		//	{
		//		dvm = e . DataSource as ObservableCollection<DetailsViewModel>;
		//		GenericGrid3 . ItemsSource = dvm;
		//		GenericGrid3 . UpdateLayout ( );
		//		GenericGrid3 . Background = FindResource ( "Red5" ) as SolidColorBrush;
		//		timer3 . Stop ( );
		//		endsecs3 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//		//if ( endsecs3 - startsecs3 < 0 )
		//		//	Debugger . Break ( );
		//		LoadTime . Text = ( endsecs3 - startsecs3 ) . ToString ( ) + " Milliseconds";
		//		DetCount . Text = dvm . Count . ToString ( );
		//		CurrDet . Content = DetDb . Text . ToUpper ( );
		//	}
		//	#endregion Async Data loaded /Load Grids handlers

		//	#region BUTTON HANDLERS
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void Standardbutton_Click ( object sender , RoutedEventArgs e )
		//	{
		//		Mouse . OverrideCursor = Cursors . Wait;
		//		Flags . USECOPYDATA = false;
		//		LoadDbs ( );
		//		LoadGrids ( );
		//		Mouse . OverrideCursor = Cursors . Arrow;
		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void Backupbutton_Click ( object sender , RoutedEventArgs e )
		//	{
		//		Mouse . OverrideCursor = Cursors . Wait;
		//		Flags . USECOPYDATA = true;
		//		//			Stopwatch sw = new Stopwatch();
		//		startsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;

		//		timer1 . Start ( );
		//		LoadDbs ( );
		//		LoadGrids ( );
		//		timer1 . Stop ( );
		//		endsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//		//if ( endsecs - startsecs < 0 )
		//		//	Debugger . Break ( );
		//		LoadTime . Text = ( endsecs1 - startsecs1 ) . ToString ( ) + " Milliseconds";
		//		Mouse . OverrideCursor = Cursors . Arrow;
		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void button4_Click ( object sender , RoutedEventArgs e )
		//	{
		//		this . Close ( );
		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void button5_Click ( object sender , RoutedEventArgs e )
		//	{
		//		// Reload ALL Dbs button
		//		Mouse . OverrideCursor = Cursors . Wait;
		//		//			Stopwatch sw = new Stopwatch();

		//		timer1 . Start ( );
		//		startsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//		GenericGrid1 . ItemsSource = null;
		//		GenericGrid2 . ItemsSource = null;
		//		GenericGrid3 . ItemsSource = null;
		//		GenericGrid1 . Items . Clear ( );
		//		GenericGrid2 . Items . Clear ( );
		//		GenericGrid3 . Items . Clear ( );
		//		GenericGrid1 . UpdateLayout ( );
		//		GenericGrid2 . UpdateLayout ( );
		//		GenericGrid3 . UpdateLayout ( );
		//		GenericGrid1 . Refresh ( );
		//		GenericGrid2 . Refresh ( );
		//		GenericGrid3 . Refresh ( );
		//		LoadDbs ( );
		//		LoadGrids ( );
		//		if ( UseAsyncLoading == false )
		//		{
		//			timer1 . Stop ( );
		//			endsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//			//if ( endsecs - startsecs < 0 )
		//			//	Debugger . Break ( );
		//			LoadTime . Text = ( endsecs1 - startsecs1 ) . ToString ( ) + " Milliseconds";
		//			BankCount . Text = bvm . Count . ToString ( );
		//			CustCount . Text = cvm . Count . ToString ( );
		//			DetCount . Text = dvm . Count . ToString ( );
		//		}
		//		Mouse . OverrideCursor = Cursors . Arrow;
		//		GenericGrid1 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//		GenericGrid2 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//		GenericGrid3 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//		GenericGrid1 . Focus ( );
		//		CurrBank . Content = BankDb . Text . ToUpper ( );
		//		CurrCust . Content = CustDb . Text . ToUpper ( );
		//		CurrDet . Content = DetDb . Text . ToUpper ( );
		//	}

		//	#endregion BUTTON HANDLERS

		//	#region Db load Db / Grids methods
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private async void LoadDbs ( )
		//	{
		//		//**************************************************************************//
		//		// only  use  these if we are NOT using BackgroundWorker threads
		//		//**************************************************************************//
		//		if ( UsebackgroundWorker == false )
		//		{
		//			args [ 0 ] = 0;
		//			args [ 1 ] = 0;
		//			args [ 2 ] = 0;
		//			if ( MaxRecords . Text != "*" && MaxRecordsToLoad . IsChecked == true )
		//			{
		//				if ( MaxRecords . Text == "*" )
		//				{
		//					MaxRecords . Text = "";
		//					args [ 2 ] = 0;
		//				}
		//				else
		//				{
		//					if ( MaxRecords . Text == "" )
		//						args [ 2 ] = 0;
		//					else
		//						args [ 2 ] = int . Parse ( MaxRecords . Text );
		//				}
		//			}
		//			if ( UseAsyncLoading )
		//			{
		//				bool resut  = await DapperSupport . GetBankObsCollectionAsync ( bvm ,
		//				"" ,
		//				BankDb . Text ,
		//				UseSort . IsChecked == true ? OrderString . Text : "" ,
		//				UseConditions . IsChecked == true ? Conditions . Text : "" ,
		//				true ,
		//				true ,
		//				  false ,
		//				"DAPPERTESTING" ,
		//				args );
		//			}
		//			else
		//			{
		//				bool resut  = await DapperSupport . GetBankObsCollectionAsync ( bvm ,
		//				"" ,
		//				BankDb . Text ,
		//				UseSort . IsChecked == true ? OrderString . Text : "" ,
		//				UseConditions . IsChecked == true ? Conditions . Text : "" ,
		//				true ,
		//				true ,
		//				  false ,
		//				"DAPPERTESTING" ,
		//				args );
		//			}
		//			if ( UseAsyncLoading )
		//			{
		//				cvm = DapperSupport . GetCustObsCollection ( cvm ,
		//				"" ,
		//				CustDb . Text ,
		//				     UseSort . IsChecked == true ? OrderString . Text : "" ,
		//				     UseConditions . IsChecked == true ? Conditions . Text : "" ,
		//				true ,
		//					true ,
		//					false ,
		//					"DAPPERTESTING" ,
		//					args );
		//			}
		//			else
		//			{
		//				cvm = DapperSupport . GetCustObsCollection ( cvm ,
		//				"" ,
		//				CustDb . Text ,
		//				     UseSort . IsChecked == true ? OrderString . Text : "" ,
		//				     UseConditions . IsChecked == true ? Conditions . Text : "" ,
		//				true ,
		//					true ,
		//					false ,
		//					"DAPPERTESTING" ,
		//					args );
		//			}
		//			if ( UseAsyncLoading )
		//			{
		//				bool resut  = await DapperSupport . GetDetailsObsCollectionAsync ( dvm , "" , DetDb . Text ,
		//			   UseSort . IsChecked == true ? OrderString . Text : "" ,
		//			UseConditions . IsChecked == true ? Conditions . Text : "" ,
		//			true ,
		//			true ,
		//			false ,
		//			"DAPPERTESTING" ,
		//			args );
		//			}
		//			else
		//			{
		//				dvm = DapperSupport . GetDetailsObsCollection ( dvm , "" , DetDb . Text ,
		//				   UseSort . IsChecked == true ? OrderString . Text : "" ,
		//				UseConditions . IsChecked == true ? Conditions . Text : "" ,
		//				true ,
		//				true ,
		//				false ,
		//				"DAPPERTESTING" ,
		//				args );
		//			}
		//		}
		//		else
		//		{
		//			//*************************************************//
		//			//We ARE using BackgroundWorker system
		//			//*************************************************//
		//			LoadBankData ( );
		//			LoadCustomers ( );
		//			LoadDetails ( );
		//		}
		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void LoadGrids ( )
		//	{
		//		GenericGrid1 . ItemsSource = bvm;
		//		GenericGrid2 . ItemsSource = cvm;
		//		GenericGrid3 . ItemsSource = dvm;
		//		GenericGrid1 . SelectedIndex = 0;
		//		GenericGrid2 . SelectedIndex = 0;
		//		GenericGrid3 . SelectedIndex = 0;
		//		GenericGrid1 . UpdateLayout ( );
		//		GenericGrid2 . UpdateLayout ( );
		//		GenericGrid3 . UpdateLayout ( );
		//		BankCount . Text = bvm . Count . ToString ( );
		//		CustCount . Text = cvm . Count . ToString ( );
		//		DetCount . Text = dvm . Count . ToString ( );
		//	}

		//	#region Data loading Main Methods
		//	private async void LoadBankData ( )
		//	{
		//		bvmcollection . Clear ( );
		//		if ( UsebackgroundWorker )
		//		{
		//			// Create a background process and  run it to load data
		//			RunBackgroundProcess ( "BANK" );
		//			//bvmcollection . Clear ( );
		//			//BackgroundWorker  Bankworker = new BackgroundWorker();
		//			//Bankworker . DoWork += new DoWorkEventHandler ( worker_DoWorkBank );
		//			//Bankworker . RunWorkerCompleted += new RunWorkerCompletedEventHandler ( worker_RunBankWorkerCompleted );
		//			//timer1 . Start ( );
		//			//startsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//			//Bankworker . RunWorkerAsync ( );
		//		}
		//		else
		//		{
		//			if ( UseAsyncLoading )
		//			{
		//				timer1 . Start ( );
		//				startsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				bool resut  = await DapperSupport . GetBankObsCollectionAsync ( bvm ,
		//				"" ,
		//				BankDb . Text ,
		//				UseSort . IsChecked == true ? OrderString . Text : "" ,
		//				UseConditions . IsChecked == true ? Conditions . Text : "" ,
		//				true ,
		//				true ,
		//				  false ,
		//				"DAPPERTESTING" ,
		//				args );
		//			}
		//			else
		//			{
		//				timer1 . Start ( );
		//				startsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				bool resut  = await DapperSupport . GetBankObsCollectionAsync ( bvm ,
		//				"" ,
		//				BankDb . Text ,
		//				UseSort . IsChecked == true ? OrderString . Text : "" ,
		//				UseConditions . IsChecked == true ? Conditions . Text : "" ,
		//				true ,
		//				true ,
		//				  false ,
		//				"DAPPERTESTING" ,
		//				args );
		//			}

		//		}
		//	}
		//	private void LoadCustomers ( )
		//	{
		//		cvmcollection . Clear ( );
		//		if ( UsebackgroundWorker )
		//		{
		//			RunBackgroundProcess ( "CUSTOMER" );
		//			//cvmcollection . Clear ( );
		//			//BackgroundWorker  Customerworker = new BackgroundWorker();
		//			//Customerworker . DoWork += new DoWorkEventHandler ( worker_DoWorkCustomer );
		//			//Customerworker . RunWorkerCompleted += new RunWorkerCompletedEventHandler ( worker_RunCustomerWorkerCompleted );
		//			//Customerworker . RunWorkerAsync ( );
		//			//timer2 . Start ( );
		//			//startsecs2 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//		}
		//		else
		//		{
		//			if ( UseAsyncLoading )
		//			{
		//				timer2 . Start ( );
		//				startsecs2 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				cvm = DapperSupport . GetCustObsCollection ( cvm ,
		//				"" ,
		//				CustDb . Text ,
		//				     UseSort . IsChecked == true ? OrderString . Text : "" ,
		//				     UseConditions . IsChecked == true ? Conditions . Text : "" ,
		//				true ,
		//					true ,
		//					true ,
		//					"DAPPERTESTING" ,
		//					args );
		//			}
		//			else
		//			{
		//				timer2 . Start ( );
		//				startsecs2 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				cvm = DapperSupport . GetCustObsCollection ( cvm ,
		//				"" ,
		//				CustDb . Text ,
		//				     UseSort . IsChecked == true ? OrderString . Text : "" ,
		//				     UseConditions . IsChecked == true ? Conditions . Text : "" ,
		//				true ,
		//					true ,
		//					false ,
		//					"DAPPERTESTING" ,
		//					args );
		//			}
		//		}
		//	}
		//	private async void LoadDetails ( )
		//	{
		//		dvmcollection . Clear ( );
		//		if ( UsebackgroundWorker )
		//		{
		//			RunBackgroundProcess ( "DETAILS" );
		//			//BackgroundWorker  Detailsworker = new BackgroundWorker();
		//			//Detailsworker . DoWork += new DoWorkEventHandler ( worker_DoWorkDetails );
		//			//Detailsworker . RunWorkerCompleted += new RunWorkerCompletedEventHandler ( worker_RunDetailsWorkerCompleted );
		//			//timer3 . Start ( );
		//			//startsecs3 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//			//Detailsworker . RunWorkerAsync ( );
		//		}
		//		else
		//		{
		//			if ( UseAsyncLoading )
		//			{
		//				timer3 . Start ( );
		//				startsecs3 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				bool resut  = await DapperSupport . GetDetailsObsCollectionAsync ( dvm , "" , DetDb . Text ,
		//					UseSort . IsChecked == true ? OrderString . Text : "" ,
		//					UseConditions . IsChecked == true ? Conditions . Text : "" ,
		//					true ,
		//					true ,
		//					false ,
		//					"DAPPERTESTING" ,
		//			args );
		//			}
		//			else
		//			{
		//				timer3 . Start ( );
		//				startsecs3 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				dvm = DapperSupport . GetDetailsObsCollection ( dvm , "" , DetDb . Text ,
		//					UseSort . IsChecked == true ? OrderString . Text : "" ,
		//					UseConditions . IsChecked == true ? Conditions . Text : "" ,
		//					true ,
		//					true ,
		//					false ,
		//					"DAPPERTESTING" ,
		//					args );
		//			}
		//		}
		//	}
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	#endregion Data loading Main Methods

		//	#endregion Db load Db / Grids methods
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	#region grid  row  entry methods
		//	private void GenericGrid1_RowEditEnding ( object sender , DataGridRowEditEndingEventArgs e )
		//	{
		//		SQLHandlers sqlh = new SQLHandlers ( );
		//		sqlh . UpdateAllDb ( "BANKACCOUNT" , e );
		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void GenericGrid2_RowEditEnding ( object sender , DataGridRowEditEndingEventArgs e )
		//	{
		//		SQLHandlers sqlh = new SQLHandlers ( );
		//		sqlh . UpdateAllDb ( "CUSTOMER" , e );
		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void GenericGrid3_RowEditEnding ( object sender , DataGridRowEditEndingEventArgs e )
		//	{
		//		SQLHandlers sqlh = new SQLHandlers ( );
		//		sqlh . UpdateAllDb ( "DETAILS" , e );
		//	}
		//	#endregion grid  row  entry methods
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	#region Toggle load methods
		//	private void UseBackgroundWorker_Click ( object sender , RoutedEventArgs e )
		//	{
		//		Flags . USESDAPPERSTDPROCEDURES = false;
		//		Flags . USEADOWITHSTOREDPROCEDURES = false;
		//		Flags . USEDAPPERWITHSTOREDPROCEDURE = false;
		//		UseStoredProc . IsChecked = false;
		//		UseStdDapper . IsChecked = false;
		//		UsebackgroundWorker = false;
		//		UsebackgroundWorker = true;
		//	}
		//	private void UseStdDapper_Click ( object sender , RoutedEventArgs e )
		//	{
		//		Flags . USESDAPPERSTDPROCEDURES = false;
		//		Flags . USEDAPPERWITHSTOREDPROCEDURE = false;
		//		Flags . USEADOWITHSTOREDPROCEDURES = true;
		//		UseDapperStoredProc . IsChecked = false;
		//		UseStoredProc . IsChecked = false;
		//		UsebackgroundWorker = false;
		//		e . Handled = true;
		//	}
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void UseStoredProc_Click ( object sender , RoutedEventArgs e )
		//	{
		//		Flags . USESDAPPERSTDPROCEDURES = false;
		//		Flags . USEDAPPERWITHSTOREDPROCEDURE = true;
		//		Flags . USEADOWITHSTOREDPROCEDURES = false;
		//		UseDapperStoredProc . IsChecked = false;
		//		UseStdDapper . IsChecked = false;
		//		UsebackgroundWorker = false;
		//		e . Handled = true;
		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void UseDapperStoredProc_Click ( object sender , RoutedEventArgs e )
		//	{
		//		Flags . USESDAPPERSTDPROCEDURES = true;
		//		Flags . USEADOWITHSTOREDPROCEDURES = false;
		//		Flags . USEDAPPERWITHSTOREDPROCEDURE = false;
		//		UseStoredProc . IsChecked = false;
		//		UseStdDapper . IsChecked = false;
		//		UsebackgroundWorker = false;
		//		e . Handled = true;
		//	}
		//	#endregion Toggle load methods

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

		//	#region mouse handlers
		//	private void ChecksMouseMove ( object sender , MouseEventArgs e )
		//	{
		//		e . Handled = true;
		//		if ( e . RightButton == MouseButtonState . Pressed )
		//			return;
		//	}
		//	private void Grab_MouseMove ( object sender , MouseEventArgs e )
		//	{
		//		if ( e . LeftButton == MouseButtonState . Pressed )
		//			Utils . Grab_MouseMove ( sender , e );
		//		e . Handled = true;
		//	}

		//	private void BankDb_MouseEnter ( object sender , MouseEventArgs e )
		//	{
		//		BankDb . SelectAll ( );
		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void CustDb_MouseEnter ( object sender , MouseEventArgs e )
		//	{
		//		CustDb . SelectAll ( );
		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void DetDb_MouseEnter ( object sender , MouseEventArgs e )
		//	{
		//		DetDb . SelectAll ( );
		//	}
		//	#endregion mouse handling

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void DetDb_KeyDown ( object sender , KeyEventArgs e )
		//	{
		//		//	if(e.Key == Key.Enter)

		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void MaxRecordsToLoad_Click ( object sender , RoutedEventArgs e )
		//	{
		//		if ( MaxRecords . Text != "*" )
		//		{
		//			try
		//			{
		//				if ( int . Parse ( MaxRecords . Text ) > 0 )
		//					args [ 2 ] = int . Parse ( MaxRecords . Text );
		//				else
		//					args [ 2 ] = 0;
		//			}
		//			catch ( Exception ex )
		//			{
		//				Console . WriteLine ( $"SQL Error {ex . Message}, {ex . Data}" );
		//			}
		//		}
		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	#region reload buttons
		//	private void ReloadBank_Click ( object sender , RoutedEventArgs e )
		//	{
		//		args [ 0 ] = 0;
		//		args [ 1 ] = 0;
		//		args [ 2 ] = 0;
		//		if ( MaxRecords . Text != "*" && MaxRecordsToLoad . IsChecked == true )
		//		{
		//			if ( MaxRecords . Text == "*" )
		//			{
		//				MaxRecords . Text = "";
		//				args [ 2 ] = 0;
		//			}
		//			else
		//			{
		//				if ( MaxRecords . Text == "" )
		//					args [ 2 ] = 0;
		//				else
		//					args [ 2 ] = int . Parse ( MaxRecords . Text );
		//			}
		//		}

		//		GenericGrid1 . ItemsSource = null;
		//		GenericGrid1 . Items . Clear ( );
		//		GenericGrid1 . UpdateLayout ( );
		//		GenericGrid1 . Refresh ( );
		//		timer1 . Start ( );
		//		startsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//		LoadBankData ( );
		//		CurrBank . Content = BankDb . Text . ToUpper ( );
		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private  void ReloadCust_Click ( object sender , RoutedEventArgs e )
		//	{
		//		args [ 0 ] = 0;
		//		args [ 1 ] = 0;
		//		args [ 2 ] = 0;
		//		if ( MaxRecords . Text != "*" && MaxRecordsToLoad . IsChecked == true )
		//		{
		//			if ( MaxRecords . Text == "*" )
		//			{
		//				MaxRecords . Text = "";
		//				args [ 2 ] = 0;
		//			}
		//			else
		//			{
		//				if ( MaxRecords . Text == "" )
		//					args [ 2 ] = 0;
		//				else
		//					args [ 2 ] = int . Parse ( MaxRecords . Text );
		//			}
		//		}

		//		timer2 . Start ( );
		//		startsecs2 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//		GenericGrid2 . ItemsSource = null;
		//		GenericGrid2 . Items . Clear ( );
		//		GenericGrid2 . UpdateLayout ( );
		//		GenericGrid2 . Refresh ( );
		//		LoadCustomers ( );
		//	}

		//	private  void ReloadDet_Click ( object sender , RoutedEventArgs e )
		//	{
		//		args [ 0 ] = 0;
		//		args [ 1 ] = 0;
		//		args [ 2 ] = 0;
		//		if ( MaxRecords . Text != "*" && MaxRecordsToLoad . IsChecked == true )
		//		{
		//			if ( MaxRecords . Text == "*" )
		//			{
		//				MaxRecords . Text = "";
		//				args [ 2 ] = 0;
		//			}
		//			else
		//			{
		//				if ( MaxRecords . Text == "" )
		//					args [ 2 ] = 0;
		//				else
		//					args [ 2 ] = int . Parse ( MaxRecords . Text );
		//			}
		//		}

		//		GenericGrid3 . ItemsSource = null;
		//		GenericGrid3 . Items . Clear ( );
		//		GenericGrid3 . UpdateLayout ( );
		//		GenericGrid3 . Refresh ( );
		//		timer3 . Start ( );
		//		startsecs3 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//		LoadDetails ( );
		//	}
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private async void LoadMulti_Click ( object sender , RoutedEventArgs e )
		//	{
		//		// Load the Multi Account holder ALONE 
		//		args [ 0 ] = 0;
		//		args [ 1 ] = 0;
		//		args [ 2 ] = 0;
		//		if ( MaxRecords . Text != "*" && MaxRecordsToLoad . IsChecked == true )
		//		{
		//			if ( MaxRecords . Text == "*" )
		//			{
		//				MaxRecords . Text = "";
		//				args [ 2 ] = 0;
		//			}
		//			else
		//			{
		//				if ( MaxRecords . Text != "" )
		//					args [ 2 ] = int . Parse ( MaxRecords . Text );
		//			}
		//		}
		//		GenericGrid1 . ItemsSource = null;
		//		GenericGrid1 . Items . Clear ( );
		//		GenericGrid1 . UpdateLayout ( );
		//		GenericGrid1 . Refresh ( );
		//		timer1 . Start ( );
		//		startsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//		if ( UseAsyncLoading )
		//		{

		//			DapperSupport . GetMultiBankCollectionAsync ( bvm ,
		//				"" ,
		//				"Bankaccount" ,
		//				UseSort . IsChecked == true ? OrderString . Text : "" ,
		//				UseConditions . IsChecked == true ? Conditions . Text : "" ,
		//				true ,
		//				"DAPPERTESTING" ,
		//				args
		//			);
		//		}
		//		else
		//		{
		//			bvm = DapperSupport . GetMultiBankCollection ( bvm ,
		//			"" ,
		//			"Bankaccount" ,
		//			UseSort . IsChecked == true ? OrderString . Text : "" ,
		//			UseConditions . IsChecked == true ? Conditions . Text : "" ,
		//			false ,
		//			"DAPPERTESTING" ,
		//			args
		//				);
		//			GenericGrid1 . ItemsSource = bvm;
		//			GenericGrid1 . SelectedIndex = 0;
		//			GenericGrid1 . UpdateLayout ( );
		//			BankCount . Text = bvm . Count . ToString ( );
		//			Flags . USEDAPPERWITHSTOREDPROCEDURE = false;
		//			GenericGrid1 . Background = FindResource ( "Red5" ) as SolidColorBrush;
		//			GenericGrid1 . Focus ( );
		//			timer1 . Stop ( );
		//			endsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//			//if ( endsecs - startsecs < 0 )
		//			//	Debugger . Break ( );
		//			LoadTime . Text = ( endsecs1 - startsecs1 ) . ToString ( ) + " Milliseconds";
		//		}
		//		CurrBank . Content = BankDb . Text . ToUpper ( );
		//	}
		//	#endregion reload buttons

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void UseAsync_Click ( object sender , RoutedEventArgs e )
		//	{
		//		if ( UseAsync . IsChecked == true )
		//			UseAsyncLoading = true;
		//		else
		//			UseAsyncLoading = false;
		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private async void UseSelectClause ( object sender , RoutedEventArgs e )
		//	{
		//		/* 
		//		 * Large method that handles processing of "normal program created" queries and user created SQL queries
		//		 * 	It calls various different methods, both normal and Async to process these requests for speed comparisons
		//		 */
		//		string command= ManualSelect.Text.ToUpper().Trim();
		//		if ( ManualSelect . Text == "" )
		//		{
		//			MessageBox . Show ( "Please enter a valid SQL query statement before trying to execute it..." , "Input Error" );
		//			ManualSelect . Focus ( );
		//			return;
		//		}

		//		Dictionary <string, CustomerViewModel> dict = new Dictionary<string, CustomerViewModel>();
		//		foreach ( var item in cvm )
		//		{
		//			dict . Add ( item . Id . ToString ( ) , item );
		//		}

		//		if ( ManualBtnText . Text == "Create Sql Query :-" )
		//		{
		//			ManualBtnText . Text = "      Perform  Query :-";
		//			ManualBtnText . Foreground = FindResource ( "White0" ) as SolidColorBrush;
		//			ManualSelect . IsEnabled = true;
		//			ManualSelect . Text = "Enter valid SQL Query here...";
		//			//Push text to top to allow for wrapping
		//			ManualSelect . Padding = new Thickness ( 0 , 0 , 0 , 0 );
		//			ManualSelect . SelectionLength = ManualSelect . Text . Length;
		//			ManualSelect . Focus ( );
		//			ManualSelect . Refresh ( );
		//			return;
		//		}
		//		if ( command . Contains ( "USER:" ) || command . Contains ( "ENTER VALID SQL" ) || command != "" )
		//		{
		//			string errormsg="";
		//			if ( command . Length < 8 || command . Contains ( "ENTER VALID SQL" ) )
		//			{
		//				if ( command . Contains ( "ENTER VALID SQL" ) )
		//				{
		//					ManualSelect . IsEnabled = true;
		//					ManualSelect . Padding = new Thickness ( 0 , 0 , 0 , 0 );
		//					ManualSelect . Focus ( );
		//				}
		//				else
		//				{
		//					MessageBox . Show ( "Your query does not appear to be valid..." , "Input Error" );
		//					ManualSelect . Focus ( );
		//				}
		//				Mouse . OverrideCursor = Cursors . Arrow;
		//				return;
		//			}
		//			Mouse . OverrideCursor = Cursors . Wait;
		//			if ( UseAsyncLoading )
		//			{
		//				timer1 . Start ( );
		//				startsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				if ( command . Contains ( "USER:" ) )
		//					command = command . Substring ( 5 );
		//				UniversalGrid . Visibility = Visibility . Visible;
		//				UniversalGrid . ItemsSource = null;
		//				UniversalGrid . Refresh ( );
		//				ObservableCollection<GenericClass> generics = new();

		//				try
		//				{
		//					if ( command . ToUpper ( ) . Contains ( "SELECT " ) )
		//					{
		//						// Generic call that wil return the results of any valid SQL select command as an Observable colection<GenericClass>
		//						Dictionary < string, string > dic = new Dictionary<string, string>();
		//						GenericClass gcc = new GenericClass();
		//						string errmsg="";
		//						generics = DapperGeneric<Dictionary<string , string> , GenericClass , bool> . CreateFromDictionary (
		//							dic ,
		//							gcc ,
		//							command ,
		//							ref errmsg
		//							);
		//						if ( errmsg != "" )
		//						{
		//							MessageBox . Show ( $"The SQL Query you entered returned the following Error ?\n\n[{errmsg . ToUpper ( )}]" , "SQL error?" );
		//							Mouse . OverrideCursor = Cursors . Arrow;
		//							return;
		//						}
		//					}
		//					else if ( SqlServerCommands . CheckforStoredProcedure ( command ) )
		//					{
		//						//We have checked for this SP above and  it does exist
		//						string args = "";
		//						string[] fields = command.Split(' ');
		//						if ( fields . Length > 1 )
		//							args = fields [ 1 ] . Trim ( );
		//						SqlServerCommands  sqc = new SqlServerCommands ( );
		//						// call with false to STOP the method flling fields and datagrids in dappersupport/SqlServerCommans
		//						generics = sqc . ExecuteStoredProcedure ( command , null , "" , args , e , false );
		//						if ( generics . Count == 0 )
		//						{
		//							MessageBox . Show ( $"The Stored procedure [{command . ToUpper ( )}]\n\nyou entered has been executed successfully !\nbut sadly it returned ZERO data records?\n\nPerhaps the procedure performs other tasks\\nand is NOT supposed to return any data ?" , "Stored Procedure Information?" );
		//							Mouse . OverrideCursor = Cursors . Arrow;
		//							return;
		//						}
		//					}
		//					else
		//					{
		//						MessageBox . Show ( $"The query you have entered entered has not been recognised !\n\n[{command . ToUpper ( )}]\n\nif it is not an exisiting Stored Procedure, so please Check/Correct the query you entered ?" , "Manual Query Error ?" );
		//						Mouse . OverrideCursor = Cursors . Arrow;
		//						return;
		//					}
		//					//Display the data
		//					UniversalGrid . ItemsSource = null;
		//					UniversalGrid . Items . Clear ( );
		//					SqlServerCommands sqlc = new SqlServerCommands();

		//					// Caution : This loads the data into the Datarid with only the selected rows
		//					// //visible in the grid so do NOT repopulate the grid after making this call
		//					sqlc . LoadActiveRowsOnlyInGrid ( UniversalGrid , generics , DapperSupport . GetGenericColumnCount ( generics ) );
		//					BankCombinedGrid . Visibility = Visibility . Collapsed;
		//					UniversalGrid . Visibility = Visibility . Visible;
		//					UniversalGrid . UpdateLayout ( );
		//					UniversalGrid . SelectedIndex = 0;
		//					UniversalGrid . Focus ( );
		//					CloseGenGridBtn . Opacity = 1;
		//				}
		//				catch ( Exception ex )
		//				{
		//					MessageBox . Show ( $"The error below (DapperTesting:913) was returned by the Dappersupport library !\n\n[{ex . Message}]\n{ex . Data}\n\nPlease Check the syntax of the query entered ?" , "Dapper Call Error ?" );
		//					Console . WriteLine ( $"The error below was returned by the Dappersupport library !\n\n[{errormsg}]\n\nPlease Check the path taken?" );
		//					ManualSelect . IsEnabled = true;
		//					ManualSelect . Focus ( );
		//				}
		//				timer1 . Stop ( );
		//				endsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				LoadTime . Text = ( endsecs1 - startsecs1 ) . ToString ( ) + " Milliseconds";
		//				Mouse . OverrideCursor = Cursors . Arrow;
		//				if ( generics . Count == 0 )
		//				{
		//					if ( errormsg . ToUpper ( ) . Contains ( "SQLERROR" ) )
		//						MessageBox . Show ( $"The error below was returned by the query entered !\n\n[{errormsg}]\n\nPlease Check/Correct the query you entered ?" , "Manual Query Error ?" );
		//					else
		//						MessageBox . Show ( "No records were returned by the query entered !\n\nPlease check the content & syntax of the query entered ?" , "Manual Query Error ?" );
		//					ManualSelect . IsEnabled = true;
		//					ManualSelect . Focus ( );
		//					Mouse . OverrideCursor = Cursors . Arrow;
		//					return;
		//				}
		//				else
		//					ExtractData . IsEnabled = true;

		//				Mouse . OverrideCursor = Cursors . Arrow;
		//				return;
		//			}
		//			else
		//			{
		//				timer1 . Start ( );
		//				startsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				command = command . Substring ( 5 );
		//				UniversalGrid . Visibility = Visibility . Visible;
		//				UniversalGrid . ItemsSource = null;
		//				UniversalGrid . Refresh ( );
		//				List<Dictionary<string, string>> OutData = new List<Dictionary<string, string>>();
		//				List<string> DbData = new List<string>(0);
		//				List<string> ReceivedDbData = new List<string>();
		//				try
		//				{
		//					ReceivedDbData = DapperSupport . GetGenericCollection ( DbData , command , true , "" );
		//					Console . WriteLine ( $"ReceivedDbData contains {ReceivedDbData . Count} records" );
		//					CreateDatabase ( UniversalGrid , ReceivedDbData );
		//				}
		//				catch ( Exception ex )
		//				{
		//					MessageBox . Show ( $"The error below (DapperTesting:953) was returned by the Dappersupport library !\n\n[{ex . Message}]\n{ex . Data}\n\nPlease Check the path taken?" , "Dapper Call Error ?" );
		//					Console . WriteLine ( $"The error below was returned by the Dappersupport library !\n\n[{errormsg}]\n\nPlease Check the path taken?" );
		//				}
		//				CloseGenGridBtn . Opacity = 1;
		//				timer1 . Stop ( );
		//				endsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				LoadTime . Text = ( endsecs1 - startsecs1 ) . ToString ( ) + " Milliseconds";
		//				Mouse . OverrideCursor = Cursors . Arrow;
		//				return;

		//			}
		//		}
		//		if ( Flags . USEDAPPERWITHSTOREDPROCEDURE == true )
		//		{
		//			MessageBox . Show ( $"Manual Commands cannot be processed by Stored Procedures, \nplease change the Mode in use to ONE of the other 2 options..." );
		//			return;
		//		}
		//		if ( command . Contains ( "FROM" ) && command . Contains ( "BANK" ) )
		//		{
		//			Mouse . OverrideCursor = Cursors . Wait;
		//			GenericGrid1 . ItemsSource = null;
		//			GenericGrid1 . Items . Clear ( );
		//			GenericGrid1 . UpdateLayout ( );
		//			GenericGrid1 . Refresh ( );
		//			if ( UseAsyncLoading )
		//			{
		//				timer1 . Start ( );
		//				startsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				try
		//				{
		//					bool result = await DapperSupport . GetBankObsCollectionAsync ( bvm ,
		//					ManualSelect . Text , DetDb . Text ,
		//				   "" ,
		//				"" ,
		//				false ,
		//				false ,
		//				true ,
		//				"DAPPERTESTING" ,
		//				args );
		//					if ( result == false )
		//					{
		//						GenericGrid1 . Background = FindResource ( "Red5" ) as SolidColorBrush;
		//						GenericGrid2 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//						GenericGrid3 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//						MessageBox . Show ( $"The Phrase you have entered for the BankAccount Db was NOT valid, Please correct and try again... " );
		//					}
		//				}
		//				catch ( Exception ex )
		//				{
		//					MessageBox . Show ( $"The error below (DapperTesting:997) was returned by the Dappersupport library !\n\n[{ex . Message}]\n{ex . Data}\n\nPlease Check the path taken?" , "Dapper Call Error ?" );
		//					Console . WriteLine ( $"The error below was returned by the Dappersupport library !\n\n[[{ex . Message}]\n\nPlease Check the path taken?" );
		//				}
		//			}
		//			else
		//			{
		//				timer1 . Start ( );
		//				startsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				bvm = DapperSupport . GetBankObsCollection ( bvm ,
		//				ManualSelect . Text , DetDb . Text ,
		//				  "" ,
		//				"" ,
		//				false ,
		//				false ,
		//				false ,
		//				"DAPPERTESTING" ,
		//				args );
		//				GenericGrid1 . ItemsSource = bvm;
		//				GenericGrid1 . SelectedIndex = 0;
		//				GenericGrid1 . UpdateLayout ( );
		//				BankCount . Text = bvm . Count . ToString ( );
		//				Flags . USEDAPPERWITHSTOREDPROCEDURE = false;
		//				GenericGrid1 . Background = FindResource ( "Red5" ) as SolidColorBrush;
		//				GenericGrid1 . Focus ( );
		//				timer1 . Stop ( );
		//				endsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				//if ( endsecs - startsecs < 0 )
		//				//	Debugger . Break ( );
		//				LoadTime . Text = ( endsecs1 - startsecs1 ) . ToString ( ) + " Milliseconds";
		//			}
		//			Mouse . OverrideCursor = Cursors . Arrow;
		//		}
		//		else if ( command . Contains ( "FROM" ) && command . Contains ( "CUST" ) )
		//		{
		//			if ( Flags . USEDAPPERWITHSTOREDPROCEDURE == true )
		//			{
		//				MessageBox . Show ( $"Manual Commands cannot be processed by Stored Procdeures, \nplease change the Mode in use to 1 of the other 2 options..." );
		//				return;
		//			}
		//			Mouse . OverrideCursor = Cursors . Wait;
		//			GenericGrid2 . ItemsSource = null;
		//			GenericGrid2 . Items . Clear ( );
		//			GenericGrid2 . UpdateLayout ( );
		//			GenericGrid2 . Refresh ( );
		//			if ( UseAsyncLoading )
		//			{
		//				timer1 . Start ( );
		//				startsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				GenericGrid2 . ItemsSource = null;
		//				GenericGrid2 . Items . Clear ( );
		//				GenericGrid2 . UpdateLayout ( );
		//				GenericGrid2 . Refresh ( );
		//				bool result = await DapperSupport . GetCustObsCollectionAsync ( cvm ,
		//				ManualSelect . Text , DetDb . Text ,
		//				   "" ,
		//				"" ,
		//				false ,
		//				false ,
		//				false ,
		//				"DAPPERTESTING" ,
		//				args );
		//				if ( result == false )
		//				{
		//					GenericGrid1 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//					GenericGrid2 . Background = FindResource ( "Red0" ) as SolidColorBrush;
		//					GenericGrid3 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//					MessageBox . Show ( $"The Phrase you have entered for the Customer Db was NOT valid, Please correct and try again... " );
		//				}
		//			}
		//			else
		//			{
		//				timer1 . Start ( );
		//				startsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				cvm = DapperSupport . GetCustObsCollection ( cvm ,
		//				ManualSelect . Text , DetDb . Text ,
		//				   "" ,
		//				"" ,
		//				false ,
		//				false ,
		//				false ,
		//				"DAPPERTESTING" ,
		//				args );
		//				GenericGrid2 . ItemsSource = null;
		//				GenericGrid2 . Items . Clear ( );
		//				GenericGrid2 . UpdateLayout ( );
		//				GenericGrid2 . Refresh ( );
		//				GenericGrid2 . ItemsSource = cvm;
		//				GenericGrid2 . SelectedIndex = 0;
		//				GenericGrid2 . UpdateLayout ( );
		//				CustCount . Text = cvm . Count . ToString ( );
		//				timer1 . Stop ( );
		//				endsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				//if ( endsecs - startsecs < 0 )
		//				//	Debugger . Break ( );
		//				LoadTime . Text = ( endsecs1 - startsecs1 ) . ToString ( ) + " Milliseconds";
		//				GenericGrid1 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//				GenericGrid2 . Background = FindResource ( "Red5" ) as SolidColorBrush;
		//				GenericGrid3 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//				GenericGrid2 . Focus ( );
		//			}
		//			Mouse . OverrideCursor = Cursors . Arrow;
		//		}
		//		else if ( command . Contains ( "FROM" ) && ( command . Contains ( "DET" ) || command . Contains ( "SECACCOUNT" ) ) )
		//		{
		//			if ( Flags . USEDAPPERWITHSTOREDPROCEDURE == true )
		//			{
		//				MessageBox . Show ( $"Manual Commands cannot be processed by Stored Procdeures, \nplease change the Mode in use to 1 of the other 2 options..." );
		//				return;
		//			}
		//			Mouse . OverrideCursor = Cursors . Wait;
		//			GenericGrid3 . ItemsSource = null;
		//			GenericGrid3 . Items . Clear ( );
		//			GenericGrid3 . UpdateLayout ( );
		//			GenericGrid3 . Refresh ( );
		//			if ( UseAsyncLoading )
		//			{
		//				timer1 . Start ( );
		//				startsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				bool result  = await DapperSupport . GetDetailsObsCollectionAsync ( dvm ,
		//				ManualSelect . Text , DetDb . Text ,
		//				   "" ,
		//				"" ,
		//				false ,
		//				false ,
		//				false ,
		//				"DAPPERTESTING" ,
		//				args );
		//				if ( result == false )
		//				{
		//					GenericGrid1 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//					GenericGrid2 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//					GenericGrid3 . Background = FindResource ( "Red5" ) as SolidColorBrush;
		//					MessageBox . Show ( $"The Phrase you have entered for the Details Db was NOT valid, Please correct and try again... " );
		//				}
		//			}
		//			else
		//			{
		//				timer1 . Start ( );
		//				startsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				dvm = DapperSupport . GetDetailsObsCollection ( dvm ,
		//				ManualSelect . Text , DetDb . Text ,
		//				   "" ,
		//				"" ,
		//				false ,
		//				false ,
		//				false ,
		//				"DAPPERTESTING" ,
		//				args );
		//				GenericGrid3 . ItemsSource = null;
		//				GenericGrid3 . Items . Clear ( );
		//				GenericGrid3 . UpdateLayout ( );
		//				GenericGrid3 . Refresh ( );
		//				GenericGrid3 . ItemsSource = dvm;
		//				GenericGrid3 . SelectedIndex = 0;
		//				GenericGrid3 . UpdateLayout ( );
		//				DetCount . Text = dvm . Count . ToString ( );
		//				timer1 . Stop ( );
		//				endsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//				LoadTime . Text = ( endsecs1 - startsecs1 ) . ToString ( ) + " Milliseconds";
		//				GenericGrid1 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//				GenericGrid2 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//				GenericGrid3 . Background = FindResource ( "Red5" ) as SolidColorBrush;
		//				GenericGrid3 . Focus ( );
		//			}
		//		}
		//		Mouse . OverrideCursor = Cursors . Arrow;
		//	}
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void CloseGenericWindow ( object sender , RoutedEventArgs e )
		//	{
		//		// Close Generid selection Grid
		//		UniversalGrid . Visibility = Visibility . Collapsed;
		//		UniversalGrid . ItemsSource = null;
		//		UniversalGrid . Refresh ( );
		//		BankCombinedGrid . Visibility = Visibility . Collapsed;
		//		BankCombinedGrid . ItemsSource = null;
		//		BankCombinedGrid . Refresh ( );
		//		// rest Db entries to dummy entries
		//		SetDummyGridEntries ( );
		//		tFlags . current = 2;
		//		ToggleBtn_Click ( null , null );
		//		CloseGenGridBtn . Opacity = 0.6;
		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void UseDapper_MouseEnter ( object sender , MouseButtonEventArgs e )
		//	{
		//		UseManualDapper . IsEnabled = true;
		//	}

		//	// Toggle Button  handlers
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void Button_Indeterminate ( object sender , RoutedEventArgs e )
		//	{
		//		// Blue ellipse visible	  - startup condition - Both grids closed
		//		// NO special grids visible
		//		UniversalGrid . Visibility = Visibility . Collapsed;
		//		UniversalGrid . Refresh ( );
		//		BankCombinedGrid . Visibility = Visibility . Collapsed;
		//		BankCombinedGrid . Refresh ( );
		//		GridsLabel . Text = "Standard view\nClick for Bank+Customer grid";
		//		GenericGrid1 . Focus ( );
		//		ToggleBtn . IsChecked = null;
		//		//ToggleBtn . Background = FindResource ( "EllipseBluegradientbackground" ) as LinearGradientBrush;
		//		ToggleBtn . Refresh ( );
		//	}
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void Button_Checked ( object sender , RoutedEventArgs e )
		//	{
		//		// Green ellipse visible  - Combined Grid visible
		//		// 2nd in sequence Blue - Green - Red
		//		UniversalGrid . Visibility = Visibility . Collapsed;
		//		UniversalGrid . Refresh ( );
		//		BankCombinedGrid . Visibility = Visibility . Visible;
		//		BankCombinedGrid . Refresh ( );
		//		GridsLabel . Text = "Combined data Grid \nClick again for standard grids";
		//		BankCombinedGrid . Focus ( );
		//		ToggleBtn . IsChecked = true;
		//		//ToggleBtn . Background = FindResource ( "EllipseGreengradientbackground" ) as LinearGradientBrush;
		//		ToggleBtn . Refresh ( );
		//	}
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void Button_Unchecked ( object sender , RoutedEventArgs e )
		//	{
		//		// Red  ellipse visible	 - Universal Grid visible
		//		// 3rd in sequence
		//		BankCombinedGrid . Visibility = Visibility . Collapsed;
		//		BankCombinedGrid . Refresh ( );
		//		UniversalGrid . Visibility = Visibility . Visible;
		//		UniversalGrid . Refresh ( );
		//		GridsLabel . Text = "Manual Query result\nClick button to show Combined Db Grid";
		//		UniversalGrid . Focus ( );
		//		ToggleBtn . IsChecked = false;
		//		//ToggleBtn . Background = FindResource ( "EllipseRedgradientbackground" ) as LinearGradientBrush;
		//		ToggleBtn . Refresh ( );
		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private async void LoadCombined_Click ( object sender , RoutedEventArgs e )
		//	{
		//		timer1 . Start ( );
		//		startsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//		bcvm = await DapperSupport . CreateBankCombinedAsync ( bcvm ,
		//		 "" ,
		//	     false );
		//		if ( bcvm != null )
		//		{
		//			Console . WriteLine ( "BankCombined Db Created/Recreated successfully..." );
		//			BankCombinedGrid . ItemsSource = bcvm;
		//			BankCombinedGrid . UpdateLayout ( );
		//			BankCombinedGrid . Refresh ( );
		//			BankCombinedGrid . Visibility = Visibility . Visible;
		//			GridsLabel . Text = "Combined data Grid \nClick button to hide special grids";
		//			timer1 . Stop ( );
		//			endsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//			LoadTime . Text = ( endsecs1 - startsecs1 ) . ToString ( ) + " Milliseconds";
		//			tFlags . current = 1;
		//			ToggleBtn_Click ( null , null );
		//		}
		//		else
		//		{
		//			BankCombinedGrid . ItemsSource = bcvm;
		//			BankCombinedGrid . UpdateLayout ( );
		//			BankCombinedGrid . Refresh ( );
		//			BankCombinedGrid . Visibility = Visibility . Visible;
		//			timer1 . Stop ( );
		//			endsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//			LoadTime . Text = ( endsecs1 - startsecs1 ) . ToString ( ) + " Milliseconds";
		//			tFlags . current = 1;
		//			ToggleBtn_Click ( null , null );
		//		}

		//	}
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void ToggleBtn_Click ( object sender , RoutedEventArgs e )
		//	{
		//		if ( tFlags . current == 0 )      // None
		//		{
		//			tFlags . current = 1;        // move to Universal
		//			Button_Unchecked ( null , null );
		//		}
		//		else if ( tFlags . current == 1 )      // Generic/Universal
		//		{
		//			tFlags . current = 2;         // move to Combined
		//			Button_Checked ( null , null );
		//		}
		//		else if ( tFlags . current == 2 ) // Combined
		//		{
		//			tFlags . current = 0;          // move to standard
		//			Button_Indeterminate ( null , null );
		//		}

		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	void SetDummyGridEntries ( )
		//	{
		//		BankCombinedGrid . ItemsSource = null;
		//		BankCombinedViewModel bcv = new BankCombinedViewModel();
		//		bcv . FName = "Combined Db has not been loaded ....";
		//		bcvm . Clear ( );
		//		bcvm . Add ( bcv );
		//		BankCombinedGrid . ItemsSource = bcvm;

		//		UniversalGrid . ItemsSource = null;
		//		GenericClass  gc = new GenericClass ( );
		//		gc . field5 = "No Generic Query has been performed....";
		//		ObservableCollection<GenericClass> gcollection = new ObservableCollection<GenericClass>();
		//		gcollection . Clear ( );
		//		gcollection . Add ( gc );
		//		UniversalGrid . ItemsSource = gcollection;
		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void Extract_Click ( object sender , RoutedEventArgs e )
		//	{
		//		int index = 0;
		//		string output = "";
		//		GenericClass gc = new GenericClass();
		//		if ( IsGenericListResult )
		//		{

		//			var dg =MessageBox . Show ( $"The Generic table already contains a set of extracted records.\n\n"
		//				+ "You cannot perform an extract again from these records, but \nyou can use the Save button to Save/Resave the contents to\na new Sql Table.\n\n"
		//				+ "Would you like to Clear the contents of the data grid instead?" , "User selectio Error !" , MessageBoxButton . YesNo);
		//			if ( dg == MessageBoxResult . Yes )
		//			{
		//				UniversalGrid . ItemsSource = null;
		//				UniversalGrid . Items . Clear ( );
		//				UniversalGrid . Refresh ( );
		//				UniversalGrid . Visibility = Visibility . Collapsed;

		//				IsGenericListResult = false;
		//				SaveData . IsEnabled = false;
		//				ExtractBankDbSaveName . Text = "Db Name ...";
		//				ExtractBankDbSaveName . Opacity = 0.4;
		//				ExtractBankDbSaveName . Foreground = FindResource ( "Black4" ) as SolidColorBrush;

		//			}
		//			return;
		//		}
		//		selectedgenerics . Clear ( );
		//		// Create a new generic Db containing selected records only
		//		if ( UniversalGrid . SelectedItems . Count > 0 )
		//		{
		//			foreach ( var data in UniversalGrid . SelectedItems )
		//			{
		//				string[] temp = data.ToString().Split(',');
		//				if ( temp . Length > 0 )
		//					output = ParseDataGridRowToString ( temp );
		//				else
		//				{
		//					DataGridRow row = UniversalGrid.ItemContainerGenerator.ContainerFromIndex (index++) as DataGridRow;
		//					string s = row.Item.ToString();
		//					string[] fields = row.Item.ToString().Split(',');
		//					output = ParseDataGridRowToString ( fields );
		//				}
		//				//Add data to the generic collection
		//				CreateAddGenericRecord ( selectedgenerics , output );
		//			}
		//			UniversalGrid . ItemsSource = null;
		//			UniversalGrid . ItemsSource = selectedgenerics;
		//			UniversalGrid . Refresh ( );
		//			SaveData . IsEnabled = true;
		//			IsGenericListResult = true;
		//			ExtractBankDbSaveName . Opacity = 1.0;
		//			ExtractBankDbSaveName . Foreground = FindResource ( "Red5" ) as SolidColorBrush;
		//		}
		//	}
		//	#region KEY HANDLERS
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void ManualSelect_PreviewKeyDown ( object sender , KeyEventArgs e )
		//	{
		//		if ( e . Key == Key . Enter )
		//			UseSelectClause ( sender , null );
		//		if ( e . Key == Key . F11 )
		//		{
		//			var pos = Mouse . GetPosition ( this);
		//			Utils . Grab_Object ( sender , pos );
		//			if ( Utils . ControlsHitList . Count == 0 )
		//				return;
		//			Utils . Grabscreen ( this , Utils . ControlsHitList [ 0 ] . VisualHit , null , sender as Control );
		//			e . Handled = true;
		//		}
		//	}
		//	#endregion KEY HANDLERS

		//	#region FOCUS HANDLERS
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void GenericGrid1_GotFocus ( object sender , RoutedEventArgs e )
		//	{
		//		GenericGrid1 . Background = FindResource ( "Red5" ) as SolidColorBrush;
		//		GenericGrid2 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//		GenericGrid3 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//		BankCombinedGrid . Background = FindResource ( "White0" ) as SolidColorBrush;
		//		UniversalGrid . Background = FindResource ( "White0" ) as SolidColorBrush;
		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void GenericGrid2_GotFocus ( object sender , RoutedEventArgs e )
		//	{
		//		GenericGrid1 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//		GenericGrid2 . Background = FindResource ( "Red5" ) as SolidColorBrush;
		//		GenericGrid3 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//		BankCombinedGrid . Background = FindResource ( "White0" ) as SolidColorBrush;
		//		UniversalGrid . Background = FindResource ( "White0" ) as SolidColorBrush;
		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void GenericGrid3_GotFocus ( object sender , RoutedEventArgs e )
		//	{
		//		GenericGrid1 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//		GenericGrid2 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//		GenericGrid3 . Background = FindResource ( "Red5" ) as SolidColorBrush;
		//		BankCombinedGrid . Background = FindResource ( "White0" ) as SolidColorBrush;
		//		UniversalGrid . Background = FindResource ( "White0" ) as SolidColorBrush;
		//	}
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void UniversalGrid_GotFocus ( object sender , RoutedEventArgs e )
		//	{
		//		GenericGrid1 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//		GenericGrid2 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//		GenericGrid3 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//		BankCombinedGrid . Background = FindResource ( "White0" ) as SolidColorBrush;
		//		UniversalGrid . Background = FindResource ( "Red5" ) as SolidColorBrush;

		//	}

		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void BankCombinedGrid_GotFocus ( object sender , RoutedEventArgs e )
		//	{
		//		GenericGrid1 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//		GenericGrid2 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//		GenericGrid3 . Background = FindResource ( "White0" ) as SolidColorBrush;
		//		BankCombinedGrid . Background = FindResource ( "Red5" ) as SolidColorBrush;
		//		UniversalGrid . Background = FindResource ( "White0" ) as SolidColorBrush;
		//	}
		//	#endregion FOCUS HANDLERS

		//	#region Generic data parsing methods
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	public string ParseDataGridRowToString ( string [ ] fields )
		//	{
		//		String output="";
		//		int index = 0;
		//		foreach ( var item in fields )
		//		{
		//			string[] temp= item . Split ( '=' );
		//			if ( temp . Length > 0 )
		//			{
		//				if ( temp [ 1 ] . Trim ( ) . Contains ( "}" ) )
		//				{
		//					if ( ( temp [ 1 ] . Trim ( ) . Contains ( " " ) ) || ( temp [ 1 ] . Trim ( ) . Contains ( "/" ) && temp [ 1 ] . Trim ( ) . Contains ( ":" ) ) ) // Date/Time
		//						output += "'" + temp [ 1 ] . Substring ( 0 , temp [ 1 ] . Length - 1 ) . Trim ( ) + "',";
		//					else
		//						output += temp [ 1 ] . Substring ( 0 , temp [ 1 ] . Length - 1 ) . Trim ( ) + ",";
		//				}
		//				else
		//				{
		//					if ( ( temp [ 1 ] . Trim ( ) . Contains ( " " ) ) || ( temp [ 1 ] . Trim ( ) . Contains ( "/" ) && temp [ 1 ] . Trim ( ) . Contains ( ":" ) ) ) // Date/Time
		//						output += "'" + temp [ 1 ] + "',";
		//					else
		//						output += temp [ 1 ] + ",";
		//				}
		//			}
		//			else
		//			{
		//				if ( item . Trim ( ) . Contains ( "{" ) )
		//				{
		//					if ( ( item . Contains ( " " ) ) || ( item . Contains ( "/" ) && item . Contains ( ":" ) ) ) // Date/Time
		//						output += "'" + item . Substring ( 1 , item . Length - 1 ) . Trim ( ) + "',";
		//					else
		//						output += item . Substring ( 1 , item . Length - 1 ) . Trim ( ) + ",";
		//				}
		//				else if ( item . Contains ( "}" ) )
		//				{
		//					if ( ( item . Contains ( " " ) ) || ( item . Contains ( "/" ) && item . Contains ( ":" ) ) ) // Date/Time
		//						output += "'" + item . Substring ( 0 , item . Length - 1 ) . Trim ( ) + "',";
		//					else
		//						output += item . Substring ( 0 , item . Length - 1 ) . Trim ( ) + ",";
		//				}
		//				else
		//				{
		//					if ( ( item . Contains ( " " ) ) || ( item . Contains ( "/" ) && item . Contains ( ":" ) ) ) // Date/Time
		//						output += "'" + item . Trim ( ) + "',";
		//					else
		//						output += item . Trim ( ) + ",";
		//				}
		//			}
		//		}
		//		output = output . Substring ( 0 , output . Length - 1 ) . Trim ( );
		//		return output;
		//	}
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	private void SaveDb_Click ( object sender , RoutedEventArgs e )
		//	{
		//		// save data we have selected from another db in 'selectedgenerics'
		//		bool success=true;
		//		string dbname = ExtractBankDbSaveName.Text;
		//		string errorstring="";
		//		ExtractBankDbSaveName . Opacity = 1.0;
		//		if ( dbname == "" || ExtractBankDbSaveName . Text . ToUpper ( ) . Contains ( "DB NAME" ) )
		//		{
		//			MessageBox . Show ( "You MUST provide the name of the new table in the field to the right..." , "Query syntax error" );
		//			return;
		//		}
		//		try
		//		{
		//			string[] args ={ "","",""};
		//			string createcommand=$" (Id INT IDENTITY(1,1) NOT NULL,";

		//			// 1st we need to DROP any existing table
		//			args [ 0 ] = dbname;
		//			Task<int>  result = DapperSupport . PerformSqlExecuteCommandAsync ( "spDropTable",args);
		//			if ( errorstring != "" )
		//			{
		//				MessageBox . Show ( $"An error was encounterd as shown below...\n\n[{errorstring}]" , "SQL Query Error" );
		//				success = false;
		//			}

		//			// Now we  need to create our fields list for the CREATE command
		//			int columnstotal = DapperSupport. GetGenericColumnCount ( selectedgenerics );
		//			for ( int i = 1 ; i <= columnstotal ; i++ )
		//			{
		//				createcommand += $" field{i} NVARCHAR(100), ";
		//			}

		//			// Now we can CREATE a new Db to match our new Db data structure
		//			string tmp = createcommand.Trim().Substring(0, createcommand.Trim().Length-1);
		//			createcommand = tmp += " )";
		//			args [ 0 ] = dbname;
		//			args [ 1 ] = createcommand;
		//			result = DapperSupport . PerformSqlExecuteCommandAsync ( "spCreateTable" , args );
		//			if ( errorstring != "" )
		//			{
		//				MessageBox . Show ( $"An error was encountered as shown below...\n\n[{errorstring}]" , "SQL Query Error" );
		//				success = false;
		//			}
		//			// Now INSERT DATA
		//			//First, we create the(fields clause) of the Insert command for the new table 
		//			createcommand = "( ";
		//			for ( int i = 1 ; i <= columnstotal ; i++ )
		//			{
		//				createcommand += $" field{i}, ";
		//			}
		//			tmp = createcommand . Trim ( ) . Substring ( 0 , createcommand . Trim ( ) . Length - 1 );
		//			createcommand = tmp += " )  ";
		//			args [ 1 ] = createcommand;

		//			// now create the VALUES clause
		//			GenericClass gc = new GenericClass();
		//			foreach ( var item in selectedgenerics )
		//			{
		//				string fldvalue="";
		//				gc = item;
		//				createcommand = "";
		//				for ( int i = 1 ; i <= columnstotal ; i++ )
		//				{
		//					fldvalue = GetDataValue ( gc , i ) . Trim ( );
		//					if ( fldvalue . Contains ( "}" ) )
		//						fldvalue = fldvalue . Substring ( 0 , fldvalue . Length - 1 );
		//					else if ( fldvalue . Contains ( "'" ) )
		//						fldvalue = fldvalue . Substring ( 1 , fldvalue . Length - 2 );
		//					// We need t warp data in quotes as it can be anything, including having spaces in it !!
		//					createcommand += $" '{fldvalue}', ";
		//				}

		//				tmp = createcommand . Trim ( ) . Substring ( 0 , createcommand . Trim ( ) . Length - 1 );
		//				args [ 2 ] = tmp;
		//				result = DapperSupport . PerformSqlExecuteCommandAsync ( "spInsertSpecifiedRow" , args );
		//				if ( errorstring != "" )
		//				{
		//					MessageBox . Show ( $"An error was encounterd as shown below...\n\n[{errorstring}]" , "SQL Query Error" );
		//					success = false;
		//				}
		//			}
		//			if ( success )
		//			{
		//				MessageBox . Show ( $"The data in the table above has been saved successfully as\n\nTable [{dbname . ToUpper ( )}]" , "SQL Processing System" );
		//				//					SaveData . Content = "Finished...";
		//			}
		//		}
		//		catch ( Exception ex )
		//		{
		//			// This catches any exceptions that dapper suppport may return as well as we "throw" them down there
		//			if ( errorstring == "" )
		//			{
		//				Console . WriteLine ( $"Error in saving data : [{ex . Message}]" );
		//				MessageBox . Show ( $"The data has NOT been saved. Error reported is :\n\n[{ex . Message}]" , "SQL Processing Error" );
		//			}
		//			else
		//			{
		//				Console . WriteLine ( $"Error in saving data : \n\n[{errorstring}]\n\n[{ex . Message}]" );
		//				MessageBox . Show ( $"The data has NOT been saved. Error reported is :\n\n[{ex . Message}]" , "SQL Processing Error" );
		//			}
		//		}

		//	}
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	public string GetDataValue ( GenericClass gc , int index )
		//	{
		//		string fldval = "";
		//		switch ( index )
		//		{
		//			case 1:
		//				fldval = gc . field1;
		//				break;
		//			case 2:
		//				fldval = gc . field2;
		//				break;
		//			case 3:
		//				fldval = gc . field3;
		//				break;
		//			case 4:
		//				fldval = gc . field4;
		//				break;
		//			case 5:
		//				fldval = gc . field5;
		//				break;
		//			case 6:
		//				fldval = gc . field6;
		//				break;
		//			case 7:
		//				fldval = gc . field7;
		//				break;
		//			case 8:
		//				fldval = gc . field8;
		//				break;
		//			case 9:
		//				fldval = gc . field9;
		//				break;
		//			case 10:
		//				fldval = gc . field10;
		//				break;
		//			case 11:
		//				fldval = gc . field11;
		//				break;
		//			case 12:
		//				fldval = gc . field12;
		//				break;
		//			case 13:
		//				fldval = gc . field13;
		//				break;
		//			case 14:
		//				fldval = gc . field14;
		//				break;
		//			case 15:
		//				fldval = gc . field15;
		//				break;
		//			case 16:
		//				fldval = gc . field16;
		//				break;
		//			case 17:
		//				fldval = gc . field17;
		//				break;
		//			case 18:
		//				fldval = gc . field18;
		//				break;
		//			case 19:
		//				fldval = gc . field19;
		//				break;
		//			case 20:
		//				fldval = gc . field20;
		//				break;
		//		}
		//		return fldval;
		//	}
		//	/// <summary>
		//	/// Generic method to create an  observableCollection<GenericClass> for an unkown data set retreived via Dapper sql Query
		//	/// The GenericClass Class has 20 fields so data can be parsed from a string into the relevant fields
		//	/// and used as ItemsSource for a datagrid
		//	/// </summary>
		//	/// <param name="dgrid"></param>
		//	/// <param name="ReceivedDbData"></param>
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	public ObservableCollection<GenericClass> CreateDatabase ( DataGrid dgrid , List<string> ReceivedDbData , int returnDb = 1 )
		//	{
		//		string datain="";
		//		// Post process data string received 
		//		ObservableCollection<GenericClass> genericcollection = new ObservableCollection<GenericClass>();
		//		for ( int x = 0 ; x < ReceivedDbData . Count ; x++ )
		//		{
		//			datain = ReceivedDbData [ x ];
		//			string[] fields = datain.Split(',');
		//			GenericClass genclass = new GenericClass();
		//			for ( int z = 0 ; z < fields . Length - 1 ; z++ )
		//			{
		//				string[] inner = fields[z].Split('=');
		//				try
		//				{
		//					switch ( z + 1 )
		//					{
		//						case 1:
		//							genclass . field1 = inner [ 1 ];
		//							break;
		//						case 2:
		//							genclass . field2 = inner [ 1 ];
		//							break;
		//						case 3:
		//							genclass . field3 = inner [ 1 ];
		//							break;
		//						case 4:
		//							genclass . field4 = inner [ 1 ];
		//							break;
		//						case 5:
		//							genclass . field5 = inner [ 1 ];
		//							break;
		//						case 6:
		//							genclass . field6 = inner [ 1 ];
		//							break;
		//						case 7:
		//							genclass . field7 = inner [ 1 ];
		//							break;
		//						case 8:
		//							genclass . field8 = inner [ 1 ];
		//							break;
		//						case 9:
		//							genclass . field9 = inner [ 1 ];
		//							break;
		//						case 10:
		//							genclass . field10 = inner [ 1 ];
		//							break;
		//						case 11:
		//							genclass . field11 = inner [ 1 ];
		//							break;
		//						case 12:
		//							genclass . field12 = inner [ 1 ];
		//							break;
		//						case 13:
		//							genclass . field13 = inner [ 1 ];
		//							break;
		//						case 14:
		//							genclass . field14 = inner [ 1 ];
		//							break;
		//						case 15:
		//							genclass . field15 = inner [ 1 ];
		//							break;
		//						case 16:
		//							genclass . field16 = inner [ 1 ];
		//							break;
		//						case 17:
		//							genclass . field17 = inner [ 1 ];
		//							break;
		//						case 18:
		//							genclass . field18 = inner [ 1 ];
		//							break;
		//						case 19:
		//							genclass . field19 = inner [ 1 ];
		//							break;
		//						case 20:
		//							genclass . field20 = inner [ 1 ];
		//							break;
		//					}
		//				}
		//				catch ( Exception ex )
		//				{
		//					Console . WriteLine ( $"createBd error : - {ex . Message}" );
		//				}

		//			}
		//			genericcollection . Add ( genclass );
		//		}
		//		tFlags . current = 1;
		//		ToggleBtn_Click ( null , null );

		//		if ( returnDb <= 2 )
		//		{
		//			UniversalGrid . ItemsSource = null;
		//			UniversalGrid . Items . Clear ( );
		//			UniversalGrid . ItemsSource = genericcollection;
		//			UniversalGrid . SelectedIndex = 0;
		//			UniversalGrid . Visibility = Visibility . Visible;
		//			UniversalGrid . Refresh ( );
		//			UniversalGrid . Focus ( );
		//			if ( returnDb == 2 )
		//				return genericcollection;
		//			else
		//				return null;
		//		}
		//		else if ( returnDb == 3 )
		//			return genericcollection;
		//		else
		//			return null;
		//		// *** NB: *** This uses the StringWrapper Class at bottom fo this file to get the content of the input List<string> so they display in a datagrid
		//		//foreach ( var item in collection )
		//		//{}
		//		//				UniversalGrid . ItemsSource = DbData . Select ( s => new { Value = s } ) . ToList ( );
		//	}
		//	//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		//	public ObservableCollection<GenericClass> CreateAddGenericRecord ( ObservableCollection<GenericClass> generics , string data )
		//	{
		//		string[] fields;
		//		int indx=1;
		//		GenericClass gc = new GenericClass();
		//		fields = data . Split ( ',' );
		//		indx = 1;
		//		foreach ( var field in fields )
		//		{
		//			string [] tmp;
		//			tmp = field . Split ( '=' );
		//			if ( tmp . Length == 2 )
		//			{
		//				switch ( indx++ )
		//				{
		//					case 1:
		//						gc . field1 = tmp [ 1 ] . Trim ( );
		//						break;
		//					case 2:
		//						gc . field2 = tmp [ 1 ] . Trim ( );
		//						break;
		//					case 3:
		//						gc . field3 = tmp [ 1 ] . Trim ( );
		//						break;
		//					case 4:
		//						gc . field4 = tmp [ 1 ] . Trim ( );
		//						break;
		//					case 5:
		//						gc . field5 = tmp [ 1 ] . Trim ( );
		//						break;
		//					case 6:
		//						gc . field6 = tmp [ 1 ] . Trim ( );
		//						break;
		//					case 7:
		//						gc . field7 = tmp [ 1 ] . Trim ( );
		//						break;
		//					case 8:
		//						gc . field8 = tmp [ 1 ] . Trim ( );
		//						break;
		//					case 9:
		//						gc . field9 = tmp [ 1 ] . Trim ( );
		//						break;
		//					case 10:
		//						gc . field10 = tmp [ 1 ] . Trim ( );
		//						break;
		//					case 11:
		//						gc . field11 = tmp [ 1 ] . Trim ( );
		//						break;
		//					case 12:
		//						gc . field12 = tmp [ 1 ] . Trim ( );
		//						break;
		//					case 13:
		//						gc . field13 = tmp [ 1 ] . Trim ( );
		//						break;
		//					case 14:
		//						gc . field14 = tmp [ 1 ] . Trim ( );
		//						break;
		//					case 15:
		//						gc . field15 = tmp [ 1 ] . Trim ( );
		//						break;
		//					case 16:
		//						gc . field16 = tmp [ 1 ] . Trim ( );
		//						break;
		//					case 17:
		//						gc . field17 = tmp [ 1 ] . Trim ( );
		//						break;
		//					case 18:
		//						gc . field18 = tmp [ 1 ] . Trim ( );
		//						break;
		//					case 19:
		//						gc . field19 = tmp [ 1 ] . Trim ( );
		//						break;
		//					case 20:
		//						gc . field20 = tmp [ 1 ] . Trim ( );
		//						break;
		//				}
		//			}
		//			else
		//			{
		//				switch ( indx++ )
		//				{
		//					case 1:
		//						gc . field1 = field . Trim ( );
		//						break;
		//					case 2:
		//						gc . field2 = field . Trim ( );
		//						break;
		//					case 3:
		//						gc . field3 = field . Trim ( );
		//						break;
		//					case 4:
		//						gc . field4 = field . Trim ( );
		//						break;
		//					case 5:
		//						gc . field5 = field . Trim ( );
		//						break;
		//					case 6:
		//						gc . field6 = field . Trim ( );
		//						break;
		//					case 7:
		//						gc . field7 = field . Trim ( );
		//						break;
		//					case 8:
		//						gc . field8 = field . Trim ( );
		//						break;
		//					case 9:
		//						gc . field9 = field . Trim ( );
		//						break;
		//					case 10:
		//						gc . field10 = field . Trim ( );
		//						break;
		//					case 11:
		//						gc . field11 = field . Trim ( );
		//						break;
		//					case 12:
		//						gc . field12 = field . Trim ( );
		//						break;
		//					case 13:
		//						gc . field13 = field . Trim ( );
		//						break;
		//					case 14:
		//						gc . field14 = field . Trim ( );
		//						break;
		//					case 15:
		//						gc . field15 = field . Trim ( );
		//						break;
		//					case 16:
		//						gc . field16 = field . Trim ( );
		//						break;
		//					case 17:
		//						gc . field17 = field . Trim ( );
		//						break;
		//					case 18:
		//						gc . field18 = field . Trim ( );
		//						break;
		//					case 19:
		//						gc . field19 = field . Trim ( );
		//						break;
		//					case 20:
		//						gc . field20 = field . Trim ( );
		//						break;
		//				}
		//			}
		//		}
		//		generics . Add ( gc );
		//		return generics;
		//	}


		//	#endregion Generic data parsing methods

		//	private void CloseGrids ( object sender , RoutedEventArgs e )
		//	{
		//		UniversalGrid . Visibility = Visibility . Collapsed;
		//		BankCombinedGrid . Visibility = Visibility . Collapsed;
		//	}

		//	private void ManualSelect_LostFocus ( object sender , RoutedEventArgs e )
		//	{
		//		if ( ManualSelect . Text == "Enter valid SQL Query here..." || ManualSelect . Text == "" )
		//		{
		//			ManualSelect . IsEnabled = false;
		//			UseManualDapper . Content = "Enter Manual Command :-";
		//			ManualBtnText . Text = "Enter valid SQL Query here...";
		//			ManualSelect . Padding = new Thickness ( 0 , 15 , 0 , 0 );

		//		}
		//	}

		//	private void OntopChkbox_Click ( object sender , RoutedEventArgs e )
		//	{
		//		if ( OntopChkbox . IsChecked == true )
		//			this . Topmost = true;
		//		else
		//			this . Topmost = false;
		//	}

		//	private void DbList_Loaded ( object sender , RoutedEventArgs e )
		//	{
		//		//	int currindex = 0;
		//		//	ObservableCollection <GenericClass>Generics= new ObservableCollection<GenericClass>();
		//		//	SqlServerCommands  ssc = new  SqlServerCommands();
		//		//	ssc . ExecuteStoredProcedure ( "spGetTablesList" , Generics , "" , "" , null );
		//		//	DbList . Items . Clear ( );
		//		//	foreach ( var item in Generics )
		//		//	{
		//		//		DbList . Items . Add ( item . field1 );
		//		//	}
		//		//	DbList . SelectedIndex = currindex == -1 ? 0 : currindex;
		//		//	DbList . SelectedItem = DbList . SelectedIndex;
		//		//	DbList . AllowDrop = true;
		//		//	DbList . IsEditable = true;
		//		//	DbList . MaxHeight = 120;
		//		//	//DbCopiedResult . Text = $"SQL Command [{SqlCommand}] completed successfully...";
		//	}

		//	private void DbList_MouseRtBtnUp ( object sender , MouseButtonEventArgs e )
		//	{
		//		// Get the data from the selected Db and display it in generic grid
		//		// Generic call that wil return the results of any valid SQL select command as an Observable colection<GenericClass>
		//		//e . Handled = true;
		//		//Dictionary < string, string > dic = new Dictionary<string, string>();
		//		//GenericClass gcc = new GenericClass();
		//		//ObservableCollection< GenericClass > generic = new ObservableCollection<GenericClass> ( );
		//		//string errmsg="";
		//		//generic = DapperGeneric<Dictionary<string , string> , GenericClass , bool> . CreateFromDictionary (
		//		//	 dic ,
		//		//	gcc ,
		//		//	$"select * from {DbList . SelectedItem . ToString ( )}" ,
		//		//	 ref errmsg );

		//		//if ( errmsg != "" )
		//		//{
		//		//	MessageBox . Show ( $"The SQL Query you entered returned the following Error ?\n\n[{errmsg . ToUpper ( )}]" , "SQL error?" );
		//		//	Mouse . OverrideCursor = Cursors . Arrow;
		//		//	return;
		//		//}
		//		//if ( generic . Count == 0 )
		//		//{
		//		//	Utils . Mbox ( this ,
		//		//		string1: $"The selected Data table \n\n[{DbList . SelectedItem . ToString ( ) . ToUpper ( )}] \n\nwas read successfully but returned Zero records" ,
		//		//		caption: "Sql Error" ,
		//		//		Btn1: mb . OK ,
		//		//		Btn2: mb . NNULL ,
		//		//		defButton: mb . OK );

		//		//	Mouse . OverrideCursor = Cursors . Arrow;
		//		//}
		//		//UniversalGrid . ItemsSource = null;
		//		//UniversalGrid . Items . Clear ( );
		//		//UniversalGrid . ItemsSource = generic;
		//		//UniversalGrid . SelectedIndex = 0;
		//		//UniversalGrid . Visibility = Visibility . Visible;
		//		//UniversalGrid . Refresh ( );
		//		//UniversalGrid . Focus ( );
		//		//tFlags . current = 0;
		//		//ToggleBtn_Click ( null , null );
		//		//return;
		//	}

		//	private void DbList_MouseDoubleClick ( object sender , MouseButtonEventArgs e )
		//	{
		//		//			DbList . IsDropDownOpen = true;

		//	}

		//	private void DbList_MouseRightButtonDown ( object sender , MouseButtonEventArgs e )
		//	{
		//		string s = e . OriginalSource . ToString ( );
		//	}


		//	private void worker_DoWorkBank ( object sender , DoWorkEventArgs e )
		//	{
		//		int [ ] args = null;
		//		SqlBackgroundLoad . LoadBackground_Bank(
		//		bvmcollection ,
		//		"" ,
		//		"BANKACCOUNT" ,
		//		"" ,
		//		"" ,
		//		false ,
		//		false ,
		//		false ,
		//		"" ,
		//		args);

		//		return;
		//		{
		//			//int [ ] args = null ;
		//			//string ConString = ( string ) Properties . Settings . Default [ "BankSysConnectionString" ];
		//			//string[] ValidFields=
		//			//{
		//			//	"ID",
		//			//	"CUSTNO",
		//			//	"BANKNO",
		//			//	"ACTYPE",
		//			//	"INTRATE" ,
		//			//	"BALANCE" ,
		//			//	"ODATE" ,
		//			//	"CDATE"
		//			//	};
		//			//string[] errorcolumns;
		//			//int [ ] dummyargs= {0,0,0};

		//			//// Use defaullt Db if none received frm caller
		//			//if ( DbNameToLoad == "" )
		//			//	DbNameToLoad = "BankAccount";

		//			//// Utility Support Methods to validate data
		//			//if ( DapperSupport . ValidateSortConditionColumns ( ValidFields , "Bank" , Orderby , Conditions , out errorcolumns ) == false )
		//			//{
		//			//	if ( Orderby . ToUpper ( ) . Contains ( errorcolumns [ 0 ] ) )
		//			//	{
		//			//		MessageBox . Show ( $"BANKACCOUNT dB\nSorry, but an invalid Column name has been \nidentified in the Sorting Clause provided.\n\nThe Invalid Column identified was :\n{errorcolumns [ 0 ]}.\n\nTherefore No Sort will be performed for this Db" );
		//			//		Orderby = "";
		//			//	}
		//			//	else if ( Conditions . ToUpper ( ) . Contains ( errorcolumns [ 0 ] ) )
		//			//	{
		//			//		MessageBox . Show ( $"BANKACCOUNT dB\nSorry, but an invalid Match clause or Column Name \nhas been identified in the Data Selection Clause.\n\nThe Invalid item identified was :\n\t{errorcolumns [ 0 ]}\n\nTherefore No Data Matching will be performed for this Db" );
		//			//		Conditions = "";
		//			//	}
		//			//	else
		//			//	{
		//			//		MessageBox . Show ( $"BANKACCOUNT dB\nSorry, but the Loading of the BankAccount Db is being aborted due to \na non existent Column name.\nThe Invalid Column identified was :\n{errorcolumns [ 0 ]}" );
		//			//		return;
		//			//	}
		//			//}
		//			////====================================================
		//			//// Use standard ADO.Net to to load Bank data to run Stored Procedure
		//			////====================================================
		//			//BankAccountViewModel bvm= new BankAccountViewModel();
		//			//string Con = ( string ) Properties . Settings . Default [ "BankSysConnectionString" ];
		//			//SqlConnection sqlCon=null;

		//			//// Works with default command 31/10/21
		//			//// works with Records limited 31/10/21
		//			//// works with Selection conditions limited 31/10/21
		//			//// works with Sort conditions 31/10/21
		//			//try
		//			//{
		//			//	using ( sqlCon = new SqlConnection ( Con ) )
		//			//	{
		//			//		SqlCommand sql_cmnd;
		//			//		sqlCon . Open ( );
		//			//		Orderby = Orderby . Contains ( "Order by" ) ? Orderby . Substring ( 9 ) : Orderby;
		//			//		Conditions = Conditions . Contains ( "where " ) ? Conditions . Substring ( 6 ) : Conditions;
		//			//		if ( SqlCommand != "" )
		//			//			sql_cmnd = new SqlCommand ( SqlCommand , sqlCon );
		//			//		else
		//			//		{
		//			//			sql_cmnd = new SqlCommand ( "dbo.spLoadBankAccountComplex " , sqlCon );
		//			//			sql_cmnd . CommandType = CommandType . StoredProcedure;
		//			//			// Now handle parameters
		//			//			sql_cmnd . Parameters . AddWithValue ( "@Arg1" , SqlDbType . NVarChar ) . Value = DbNameToLoad;
		//			//			if ( args == null )
		//			//				args = dummyargs;
		//			//			if ( args . Length > 0 )
		//			//			{
		//			//				if ( args [ 2 ] > 0 )
		//			//				{
		//			//					string limits = $" Top ({args[2]}) ";
		//			//					sql_cmnd . Parameters . AddWithValue ( "@Arg2" , SqlDbType . NVarChar ) . Value = limits;
		//			//				}
		//			//			}
		//			//			Orderby = Orderby . Contains ( "Order by" ) ? Orderby . Substring ( 9 ) : Orderby;
		//			//			if ( Conditions != "" )
		//			//				sql_cmnd . Parameters . AddWithValue ( "@Arg3" , SqlDbType . NVarChar ) . Value = Conditions;
		//			//			if ( Orderby != "" )
		//			//				sql_cmnd . Parameters . AddWithValue ( "@Arg4" , SqlDbType . NVarChar ) . Value = Orderby . Trim ( );
		//			//		}
		//			//		var sqlDr = sql_cmnd . ExecuteReader ( );
		//			//		while ( sqlDr . Read ( ) )
		//			//		{
		//			//			bvm . Id = int . Parse ( sqlDr [ "ID" ] . ToString ( ) );
		//			//			bvm . CustNo = sqlDr [ "CustNo" ] . ToString ( );
		//			//			bvm . BankNo = sqlDr [ "BankNo" ] . ToString ( );
		//			//			bvm . AcType = int . Parse ( sqlDr [ "ACTYPE" ] . ToString ( ) );
		//			//			bvm . Balance = Decimal . Parse ( sqlDr [ "BALANCE" ] . ToString ( ) );
		//			//			bvm . IntRate = Decimal . Parse ( sqlDr [ "INTRATE" ] . ToString ( ) );
		//			//			bvm . ODate = DateTime . Parse ( sqlDr [ "ODATE" ] . ToString ( ) );
		//			//			bvm . CDate = DateTime . Parse ( sqlDr [ "CDATE" ] . ToString ( ) );
		//			//			bvmcollection . Add ( bvm );
		//			//			bvm = new BankAccountViewModel ( );
		//			//		}
		//			//		sqlDr . Close ( );
		//			//		Console . WriteLine ( $"SQL DAPPER {DbNameToLoad}  loaded : {bvmcollection . Count} records successfuly" );
		//			//	}
		//			//	sqlCon . Close ( );
		//			//}
		//			//catch ( Exception ex )
		//			//{
		//			//	Console . WriteLine ( $"Sql Error, {ex . Message}, {ex . Data}" );
		//			//}

		//			//			Utils . DoErrorBeep ( 250 , 50 , 1 );
		//			//return bvmcollection;
		//		}
		//	}

		//	private void worker_DoWorkCustomer ( object sender , DoWorkEventArgs e )
		//	{
		//		int [ ] args = null;

		//		SqlBackgroundLoad.LoadBackground_Customer (
		//		cvmcollection,
		//		"" ,
		//		"Customer" ,
		//		"",
		//		"" ,
		//		false ,
		//		false ,
		//		false ,
		//		"" ,
		//		args);

		//		return;

		//		{//IEnumerable<CustomerViewModel> cvm ;
		//		 //string[] ValidFields=
		//		 //{
		//		 //	"ID",
		//		 //	"CUSTNO",
		//		 //	"BANKNO",
		//		 //	"ACTYPE",
		//		 //	"FNAME" ,
		//		 //	"LNAME" ,
		//		 //	"ADDR1" ,
		//		 //	"ADDR2" ,
		//		 //	"TOWN" ,
		//		 //	"COUNTY",
		//		 //	"PCODE" ,
		//		 //	"PHONE" ,
		//		 //	"MOBILE",
		//		 //	"ODATE" ,
		//		 //	"CDATE"
		//		 //	};
		//		 //string[] errorcolumns;
		//		 //string ConString = ( string ) Properties . Settings . Default [ "BankSysConnectionString" ];

		//			//if ( DbNameToLoad == "" )
		//			//	DbNameToLoad = "Customer";


		//			//// Utility Support Methods to validate data
		//			//if ( DapperSupport . ValidateSortConditionColumns ( ValidFields , "Bank" , Orderby , Conditions , out errorcolumns ) == false )
		//			//{
		//			//	if ( Orderby . ToUpper ( ) . Contains ( errorcolumns [ 0 ] ) )
		//			//	{
		//			//		MessageBox . Show ( $"BANKACCOUNT dB\nSorry, but an invalid Column name has been \nidentified in the Sorting Clause provided.\n\nThe Invalid Column identified was :\n{errorcolumns [ 0 ]}.\n\nTherefore No Sort will be performed for this Db" );
		//			//		Orderby = "";
		//			//	}
		//			//	else if ( Conditions . ToUpper ( ) . Contains ( errorcolumns [ 0 ] ) )
		//			//	{
		//			//		MessageBox . Show ( $"BANKACCOUNT dB\nSorry, but an invalid Match clause or Column Name \nhas been identified in the Data Selection Clause.\n\nThe Invalid item identified was :\n\t{errorcolumns [ 0 ]}\n\nTherefore No Data Matching will be performed for this Db" );
		//			//		Conditions = "";
		//			//	}
		//			//	else
		//			//	{
		//			//		MessageBox . Show ( $"BANKACCOUNT dB\nSorry, but the Loading of the BankAccount Db is being aborted due to \na non existent Column name.\nThe Invalid Column identified was :\n{errorcolumns [ 0 ]}" );
		//			//		return;
		//			//	}
		//			//}

		//			////====================================================
		//			//// Use standard ADO.Net to to load Bank data to run Stored Procedure
		//			////====================================================
		//			//CustomerViewModel cvmi = new CustomerViewModel ( );
		//			//string Con = ( string ) Properties . Settings . Default [ "BankSysConnectionString" ];
		//			//SqlConnection sqlCon=null;
		//			//Orderby = Orderby . Contains ( "Order by" ) ? Orderby . Substring ( 9 ) : Orderby;
		//			//Conditions = Conditions . Contains ( "where " ) ? Conditions . Substring ( 6 ) : Conditions;

		//			//// Works with default command 31/10/21
		//			//// works with Records limited 31/10/21
		//			//// works with Selection conditions limited 31/10/21
		//			//// works with Sort conditions 31/10/21
		//			//try
		//			//{
		//			//	using ( sqlCon = new SqlConnection ( Con ) )
		//			//	{
		//			//		SqlCommand sql_cmnd;
		//			//		sqlCon . Open ( );
		//			//		if ( SqlCommand != "" )
		//			//			sql_cmnd = new SqlCommand ( SqlCommand , sqlCon );
		//			//		else
		//			//		{
		//			//			sql_cmnd = new SqlCommand ( "dbo.spLoadCustomersComplex " , sqlCon );
		//			//			sql_cmnd . CommandType = CommandType . StoredProcedure;
		//			//			sql_cmnd . Parameters . AddWithValue ( "@Arg1" , SqlDbType . NVarChar ) . Value = DbNameToLoad;
		//			//			if ( args == null )
		//			//				args = dummyargs;
		//			//			if ( args . Length > 0 )
		//			//			{
		//			//				if ( args [ 2 ] > 0 )
		//			//				{
		//			//					string limits = $" Top ({args[2]}) ";
		//			//					sql_cmnd . Parameters . AddWithValue ( "@Arg2" , SqlDbType . NVarChar ) . Value = limits;
		//			//				}
		//			//				//else
		//			//				//	sql_cmnd . Parameters . AddWithValue ( "@Arg2" , SqlDbType . NVarChar ) . Value = " * ";
		//			//			}
		//			//			//else
		//			//			//	sql_cmnd . Parameters . AddWithValue ( "@Arg2" , SqlDbType . NVarChar ) . Value = " * ";
		//			//			sql_cmnd . Parameters . AddWithValue ( "@Arg3" , SqlDbType . NVarChar ) . Value = Conditions;
		//			//			sql_cmnd . Parameters . AddWithValue ( "@Arg4" , SqlDbType . NVarChar ) . Value = Orderby;
		//			//		}
		//			//		// Handle  max records, if any
		//			//		var sqlDr = sql_cmnd . ExecuteReader ( );
		//			//		while ( sqlDr . Read ( ) )
		//			//		{
		//			//			cvmi . Id = int . Parse ( sqlDr [ "ID" ] . ToString ( ) );
		//			//			cvmi . CustNo = sqlDr [ "CUSTNO" ] . ToString ( );
		//			//			cvmi . BankNo = sqlDr [ "BANKNO" ] . ToString ( );
		//			//			cvmi . AcType = int . Parse ( sqlDr [ "ACTYPE" ] . ToString ( ) );
		//			//			cvmi . FName = sqlDr [ "FNAME" ] . ToString ( );
		//			//			cvmi . LName = sqlDr [ "LNAME" ] . ToString ( );
		//			//			cvmi . Addr1 = sqlDr [ "ADDR1" ] . ToString ( );
		//			//			cvmi . Addr2 = sqlDr [ "ADDR2" ] . ToString ( );
		//			//			cvmi . Town = sqlDr [ "TOWN" ] . ToString ( );
		//			//			cvmi . County = sqlDr [ "COUNTY" ] . ToString ( );
		//			//			cvmi . PCode = sqlDr [ "PCODE" ] . ToString ( );
		//			//			cvmi . Phone = sqlDr [ "PHONE" ] . ToString ( );
		//			//			cvmi . Mobile = sqlDr [ "MOBILE" ] . ToString ( );
		//			//			cvmi . ODate = DateTime . Parse ( sqlDr [ "ODATE" ] . ToString ( ) );
		//			//			cvmi . CDate = DateTime . Parse ( sqlDr [ "CDATE" ] . ToString ( ) );
		//			//			cvmcollection . Add ( cvmi );
		//			//			cvmi = new CustomerViewModel ( );
		//			//		}
		//			//		sqlDr . Close ( );
		//			//		Console . WriteLine ( $"SQL DAPPER {DbNameToLoad}  loaded : {cvmcollection . Count} records successfuly" );
		//			//	}
		//			//	sqlCon . Close ( );
		//			//}
		//			//catch ( Exception ex )
		//			//{
		//			//	Console . WriteLine ( $"Sql Error, {ex . Message}, {ex . Data}" );
		//			//}
		//			//			Utils . DoErrorBeep ( 270 , 50, 2 );
		//		}
		//	}
		//	private void worker_DoWorkDetails ( object sender , DoWorkEventArgs e )
		//	{
		//		int [ ] args = null;
		//		SqlBackgroundLoad . LoadBackground_Details (
		//		dvmcollection ,
		//		"" ,
		//		"SECACCOUNTS" ,
		//		"" ,
		//		"" ,
		//		false ,
		//		false ,
		//		false ,
		//		"" ,
		//		args );

		//		return;

		//		{	//string SqlCommand = "";
		//		//string DbNameToLoad = "Secaccounts";
		//		//string Orderby = "";
		//		//string Conditions = "";
		//		//bool wantSort = false;
		//		//bool wantDictionary = false;
		//		//bool Notify = false;
		//		//string Caller = "";
		//		//int[] dummyargs = {0,0,0,0};
		//		//IEnumerable<DetailsViewModel> dvm ;
		//		//string ConString = ( string ) Properties . Settings . Default [ "BankSysConnectionString" ];

		//		//string[] ValidFields=
		//		//{
		//		//	"ID",
		//		//	"CUSTNO",
		//		//	"BANKNO",
		//		//	"ACTYPE",
		//		//	"INTRATE" ,
		//		//	"BALANCE" ,
		//		//	"ODATE" ,
		//		//	"CDATE"
		//		//	};
		//		//string[] errorcolumns;

		//		//// Use defaullt Db if none received frm caller
		//		//if ( DbNameToLoad == "" )
		//		//	DbNameToLoad = "SecAccounts";


		//		//// Utility Support Methods to validate data
		//		//if ( DapperSupport . ValidateSortConditionColumns ( ValidFields , "Bank" , Orderby , Conditions , out errorcolumns ) == false )
		//		//{
		//		//	if ( Orderby . ToUpper ( ) . Contains ( errorcolumns [ 0 ] ) )
		//		//	{
		//		//		MessageBox . Show ( $"BANKACCOUNT dB\nSorry, but an invalid Column name has been \nidentified in the Sorting Clause provided.\n\nThe Invalid Column identified was :\n{errorcolumns [ 0 ]}.\n\nTherefore No Sort will be performed for this Db" );
		//		//		Orderby = "";
		//		//	}
		//		//	else if ( Conditions . ToUpper ( ) . Contains ( errorcolumns [ 0 ] ) )
		//		//	{
		//		//		MessageBox . Show ( $"BANKACCOUNT dB\nSorry, but an invalid Match clause or Column Name \nhas been identified in the Data Selection Clause.\n\nThe Invalid item identified was :\n\t{errorcolumns [ 0 ]}\n\nTherefore No Data Matching will be performed for this Db" );
		//		//		Conditions = "";
		//		//	}
		//		//	else
		//		//	{
		//		//		MessageBox . Show ( $"BANKACCOUNT dB\nSorry, but the Loading of the BankAccount Db is being aborted due to \na non existent Column name.\nThe Invalid Column identified was :\n{errorcolumns [ 0 ]}" );
		//		//		return;
		//		//	}
		//		//}
		//		//if ( DbNameToLoad == "" )
		//		//	DbNameToLoad = "SecAccounts";

		//		//Orderby = Orderby . Contains ( "Order by" ) ? Orderby . Substring ( 9 ) : Orderby;
		//		//Conditions = Conditions . Contains ( "where " ) ? Conditions . Substring ( 6 ) : Conditions;
		//		////====================================================
		//		//// Use standard ADO.Net to to load Bank data to run Stored Procedure
		//		////====================================================
		//		//DetailsViewModel dvmi = new DetailsViewModel();
		//		//string Con = ( string ) Properties . Settings . Default [ "BankSysConnectionString" ];
		//		//SqlConnection sqlCon=null;

		//		//// Works with default command 31/10/21
		//		//// works with Records limited 31/10/21
		//		//// works with Selection conditions limited 31/10/21
		//		//// works with Sort conditions 31/10/21
		//		//try
		//		//{
		//		//	using ( sqlCon = new SqlConnection ( Con ) )
		//		//	{
		//		//		SqlCommand sql_cmnd;
		//		//		sqlCon . Open ( );
		//		//		if ( SqlCommand != "" )
		//		//			sql_cmnd = new SqlCommand ( SqlCommand , sqlCon );
		//		//		else
		//		//		{
		//		//			sql_cmnd = new SqlCommand ( "dbo.spLoadDetailsComplex " , sqlCon );
		//		//			sql_cmnd . CommandType = CommandType . StoredProcedure;
		//		//			sql_cmnd . Parameters . AddWithValue ( "@Arg1" , SqlDbType . NVarChar ) . Value = DbNameToLoad;
		//		//			if ( args == null )
		//		//				args = dummyargs;
		//		//			if ( args . Length > 0 )
		//		//			{
		//		//				if ( args [ 2 ] > 0 )
		//		//				{
		//		//					string limits = $" Top ({args[2]}) ";
		//		//					sql_cmnd . Parameters . AddWithValue ( "@Arg2" , SqlDbType . NVarChar ) . Value = limits;
		//		//				}
		//		//				else
		//		//					sql_cmnd . Parameters . AddWithValue ( "@Arg2" , SqlDbType . NVarChar ) . Value = " * ";
		//		//			}
		//		//			else
		//		//				sql_cmnd . Parameters . AddWithValue ( "@Arg2" , SqlDbType . NVarChar ) . Value = " * ";

		//		//			sql_cmnd . Parameters . AddWithValue ( "@Arg3" , SqlDbType . NVarChar ) . Value = Conditions;
		//		//			sql_cmnd . Parameters . AddWithValue ( "@Arg4" , SqlDbType . NVarChar ) . Value = Orderby;
		//		//		}
		//		//		// Handle  max records, if any
		//		//		var sqlDr = sql_cmnd . ExecuteReader ( );
		//		//		while ( sqlDr . Read ( ) )
		//		//		{
		//		//			dvmi . Id = int . Parse ( sqlDr [ "ID" ] . ToString ( ) );
		//		//			dvmi . CustNo = sqlDr [ "CustNo" ] . ToString ( );
		//		//			dvmi . BankNo = sqlDr [ "BankNo" ] . ToString ( );
		//		//			dvmi . AcType = int . Parse ( sqlDr [ "ACTYPE" ] . ToString ( ) );
		//		//			dvmi . Balance = Decimal . Parse ( sqlDr [ "BALANCE" ] . ToString ( ) );
		//		//			dvmi . IntRate = Decimal . Parse ( sqlDr [ "INTRATE" ] . ToString ( ) );
		//		//			dvmi . ODate = DateTime . Parse ( sqlDr [ "ODATE" ] . ToString ( ) );
		//		//			dvmi . CDate = DateTime . Parse ( sqlDr [ "CDATE" ] . ToString ( ) );
		//		//			dvmcollection . Add ( dvmi );
		//		//			dvmi = new DetailsViewModel ( );
		//		//		}
		//		//		sqlDr . Close ( );
		//		//		Console . WriteLine ( $"SQL DAPPER {DbNameToLoad}  loaded : {dvmcollection . Count} records successfuly" );
		//		//	}
		//		//	sqlCon . Close ( );
		//		//}
		//		//catch ( Exception ex )

		//		//{
		//		//	Console . WriteLine ( $"Sql Error, {ex . Message}, {ex . Data}" );
		//		//}
		//		//			Utils . DoErrorBeep ( 290 , 50 , 3 );
		//	}
		//	}

		//	private void worker_RunBankWorkerCompleted ( object sender , RunWorkerCompletedEventArgs e )
		//	{
		//		var res = e.Result;
		//		Console . WriteLine ( $" Bank e.Result = {e . Result}" );
		//		//bvm . Clear ( );

		//		bvm = bvmcollection;
		//		Console . WriteLine ( $" Bank e.Result = {bvmcollection . Count}" );
		//		GenericGrid1 . ItemsSource = bvm;
		//		GenericGrid1 . SelectedIndex = 0;
		//		GenericGrid1 . UpdateLayout ( );
		//		BankCount . Text = bvm . Count . ToString ( );
		//		CurrBank . Content = BankDb . Text . ToUpper ( );
		//		timer1 . Stop ( );
		//		endsecs1 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//		LoadTime . Text = ( endsecs1 - startsecs1 ) . ToString ( ) + " Milliseconds";

		//	}
		//	private void worker_RunCustomerWorkerCompleted ( object sender , RunWorkerCompletedEventArgs e )
		//	{
		//		var res = e.Result;
		//		Console . WriteLine ( $" Customer e.Result = {e . Result}" );
		//		//cvm . Clear ( );
		//		cvm = cvmcollection;
		//		Console . WriteLine ( $" Customer e.Result = {cvmcollection . Count}" );
		//		GenericGrid2 . ItemsSource = cvm;
		//		GenericGrid2 . SelectedIndex = 0;
		//		GenericGrid2 . UpdateLayout ( );
		//		CustCount . Text = cvm . Count . ToString ( );
		//		CurrCust . Content = CustDb . Text . ToUpper ( );
		//		timer2 . Stop ( );
		//		endsecs2 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//		LoadTime . Text = ( endsecs2 - startsecs2 ) . ToString ( ) + " Milliseconds";
		//	}
		//	private void worker_RunDetailsWorkerCompleted ( object sender , RunWorkerCompletedEventArgs e )
		//	{
		//		//dvm . Clear ( );
		//		dvm = dvmcollection;
		//		Console . WriteLine ( $" Details e.Result = {dvmcollection . Count}" );
		//		//dvm = e.Result as ObservableCollection<DetailsViewModel>;
		//		Console . WriteLine ( $" Details e.Result = {e . Result}" );
		//		if ( dvm == null )
		//			return;
		//		GenericGrid3 . ItemsSource = dvm;
		//		GenericGrid3 . SelectedIndex = 0;
		//		GenericGrid3 . UpdateLayout ( );
		//		DetCount . Text = dvm . Count . ToString ( );
		//		CurrDet . Content = DetDb . Text . ToUpper ( );
		//		timer3 . Stop ( );
		//		endsecs3 = DateTime . Now . Second * 1000 + DateTime . Now . Millisecond;
		//		LoadTime . Text = ( endsecs3 - startsecs3 ) . ToString ( ) + " Milliseconds";
		//	}

	}



}
