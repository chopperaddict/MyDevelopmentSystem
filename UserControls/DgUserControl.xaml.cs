using System;
using System . Collections . ObjectModel;
using System . Data;
using System . Data . SqlClient;

using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Data;
using System . Windows . Input;
using System . Windows . Media;

using MyDev . SQL;
using MyDev . ViewModels;
using MyDev . Views;
using Cursors = System . Windows . Input . Cursors;

using System . IO;
using System . Runtime . Serialization . Formatters . Binary;

namespace MyDev . UserControls
{
    /// <summary>
    /// Interaction logic for DgUserControl.xaml
    /// </summary>
    public partial class DgUserControl : UserControl
    {
        const string FileName = @"DgUserControl.bin";
        DatagridUserControlViewModel dgridctrlvm { get; set; }  = new DatagridUserControlViewModel ( );   
        #region header block
        public object Viewmodel { get; set; }

        public ObservableCollection<ViewModels . BankAccountViewModel> Bvm { get; private set; }
        public ObservableCollection<ViewModels . CustomerViewModel> Cvm { get; private set; }
        //       private static TabWinViewModel Controller { get; set; }
        public static object HostViewModel { get; set; }
        public static Tabview tabviewWin { get; set; }
        public string CurrentType { set; get; } = "CUSTOMER";
        private static DgUserControl ThisWin { get; set; }
        new private bool IsLoaded { get; set; } = false;
        public bool  SelectionInAction { get; set; } = false;
        public static bool TrackselectionChanges { get; set; } = false;
        private int CurrentIndex { get; set; } = 0;

        public DatagridUserControlViewModel ReadSerializedObject ( )
        {
            Console . WriteLine ( "Reading saved file" );
            Stream openFileStream = File . OpenRead ( FileName );
            BinaryFormatter deserializer = new BinaryFormatter ( );
            DatagridUserControlViewModel dcvm = ( DatagridUserControlViewModel ) deserializer . Deserialize ( openFileStream );
            //TestLoan . TimeLastLoaded = DateTime . Now;
            openFileStream . Close ( );
            return dcvm;
        }
        public void WriteSerializedObject()
        {
            Stream SaveFileStream = File . Create ( FileName );
            BinaryFormatter serializer = new BinaryFormatter ( );
            serializer . Serialize ( SaveFileStream , grid1 );
            SaveFileStream . Close ( );
        }
         #endregion header block

        public static event EventHandler<GotFocusArgs> GridGotFocus;

        public static void SetGridGotFocus ( GotFocusArgs args )
        {
            if ( DgUserControl . GridGotFocus != null )
                DgUserControl . GridGotFocus . Invoke ( null , args );
        }

        public DgUserControl ( )
        {
            InitializeComponent ( );
            Console . WriteLine ( $"DataGrid Control Loading ......" );
            ThisWin = this;
            this . DataContext = dgridctrlvm ;
            TabWinViewModel . LoadDb += DgLoadDb;
            EventControl . BankDataLoaded += EventControl_BankDataLoaded;
            EventControl . CustDataLoaded += EventControl_CustDataLoaded;
            EventControl . ListSelectionChanged += SelectionHasChanged;
            grid1 . PreviewGotKeyboardFocus += Grid1_PreviewGotKeyboardFocus;

            GridGotFocus += DgUserControl_GridGotFocus;
            // allow   this  to broadcast
            EventControl . TriggerWindowMessage ( this , new InterWindowArgs { message = $"DgUIserControl loaded..." } );
            CreateBankColumns ( );
            LoadBank ( );
            IsLoaded = false;
            AddHotKeys ( );
            // Save to our ViewModel repository
            Viewmodel = new ViewModel ( );
            Viewmodel = this;
            ViewModel . SaveViewmodel ( "DgUsercontrol" , Viewmodel );
        }

        private void DgUserControl_GridGotFocus ( object sender , GotFocusArgs e )
        {
            ReceivedFocus ( e );
        }

        private void ReceivedFocus ( GotFocusArgs args )
        {
            // Handle focus being set to this user control
            this . Focus ( );
            Console . WriteLine ( $"Setting DataGrid as Active tab" );
            Mouse . OverrideCursor = Cursors . Wait;
            // setup the current tab Id

            if ( grid1?.Items . Count > 0 )
            {
                grid1 . CancelEdit ( );
                if ( grid1 . IsKeyboardFocusWithin )
                {
                    Console . WriteLine ( "Keyboard IS FOCUSED" );
//                    grid1 . GotKeyboardFocus += Grid1_GotKeyboardFocus;
                }
                else
                    Console . WriteLine ( "Keyboard not  focused" );
                Mouse . OverrideCursor = Cursors . Arrow;
            Console . WriteLine ( $"Current index is {grid1 . SelectedIndex}" );
            }
            else
            {
                Console . WriteLine ( $"Loading DataGrid Control" );
                //Application . Current . Dispatcher . Invoke ( ( ) =>
                //    Tview . LoadName . Text = "Data Grid Loading"
                //);
                if ( args . UseTask )
                {
                    BankCollection bnk = new BankCollection ( );
                    ObservableCollection<BankAccountViewModel> Bvm = new ObservableCollection<BankAccountViewModel> ( );
                    Task task = new Task ( ( ) =>
                    {
                        UserControlDataAccess . GetBankObsCollectionAsync ( Bvm , "" , true , "DgUserControl" );
                    } );
                    task . Start ( );

                }
                else
                {
                    //                    TabWinViewModel. LoadDatagridInBackgroundTask ( null);
                }
            }
            Mouse . OverrideCursor = Cursors . Arrow;
            DbCountArgs cargs = new DbCountArgs ( );
            cargs . Dbcount = grid1 . Items . Count;
            cargs . sender = "dgUserctrl";
            TabWinViewModel . TriggerBankDbCount ( this , cargs );
            //EventControl . TriggerWindowMessage ( this , new InterWindowArgs { message = $"dgUserControl now Active..." , listbox = null } );
            grid1 . CancelEdit ( );
            if ( grid1 . IsKeyboardFocusWithin )
                Console . WriteLine ( "Keyboard IS FOCUSED" );
            else
            {
                Console . WriteLine ( "Keyboard not  focused" );
                SendEscapeToGrid ( );
            }
            return;
        }
        private void SendEscapeToGrid ( )
        {
            Key key = Key . Escape;
            
        }
        private void AddHotKeys ( )
        {
            try
            {
                RoutedCommand firstSettings = new RoutedCommand ( );
                firstSettings . InputGestures . Add ( new KeyGesture ( Key . Escape , ModifierKeys . None ) );
                CommandBindings . Add ( new CommandBinding ( firstSettings , HandleEscKey ) );

                //RoutedCommand secondSettings = new RoutedCommand ( );
                //secondSettings . InputGestures . Add ( new KeyGesture ( Key . B , ModifierKeys . None ) );
                //CommandBindings . Add ( new CommandBinding ( secondSettings , My_second_event_handler ) );
            }
            catch ( Exception err )
            {
                //handle exception error
            }
        }

    
        private void Grid1_PreviewGotKeyboardFocus ( object sender , KeyboardFocusChangedEventArgs e )
        {
            // has no effect, it still focuses on ID fielld on top record
            KeyboardDevice kd = e . KeyboardDevice;
            kd . ClearFocus ( );
            kd . Focus ( TabWinViewModel . dgUserctrl );
        }

        private void UserControl_Loaded ( object sender , System . Windows . RoutedEventArgs e )
        {
            if ( IsLoaded == true ) return;
            IsLoaded = true;
        }
        public static void SetListSelectionChanged ( bool arg )
        {
            TrackselectionChanges = arg;
        }

        private void EventControl_BankDataLoaded ( object sender , LoadedEventArgs e )
        {
            if ( e . CallerType != "DgUserControl" ) return;
            //if ( grid1 . Items . Count > 0 && CurrentType != "BANK" ) return;
            CurrentType = "BANK";
            TabWinViewModel . TriggerDbType ( CurrentType );
            //await Dispatcher . SwitchToUi ( );
            $"Dispatcher on UI thread =  {Dispatcher . CheckAccess ( )}" . CW ( );
            Console . WriteLine ( $"Data requested by : [{e . CallerDb}]" );
            this . grid1 . ItemsSource = null;
            this . grid1 . Items . Clear ( );
            CreateBankColumns ( );
            Bvm = e . DataSource as ObservableCollection<ViewModels . BankAccountViewModel>;
            DataTemplate dt = FindResource ( "BankDataTemplate1" ) as DataTemplate;
            this . grid1 . ItemTemplate = dt;
            this . grid1 . ItemsSource = Bvm;
            this . grid1 . SelectedIndex = 0;
            this . grid1 . SelectedItem = 0;
            Utils . ScrollRecordIntoView ( this . grid1 , 0 );
            this . grid1 . UpdateLayout ( );
            Console . WriteLine ( $"Data Loaded for: [{e . CallerDb}], Records = {this . grid1 . Items . Count}" );
            //TabWinViewModel . dgUserctrl . Refresh ( );
            DbCountArgs args = new DbCountArgs ( );
            args . Dbcount = Bvm?.Count ?? -1;
            args . sender = "dgUserctrl";
            TabWinViewModel . TriggerBankDbCount ( this , args );

        }

        private void EventControl_CustDataLoaded ( object sender , LoadedEventArgs e )
        {
            if ( e . CallerType != "DgUserControl" ) return;
            //            if ( grid1 . Items . Count > 0 && CurrentType != "CUSTOMER" ) return;

            CurrentType = "CUSTOMER";
            TabWinViewModel . TriggerDbType ( CurrentType );
            this . grid1 . ItemsSource = null;
            this . grid1 . Items . Clear ( );
            CreateCustomerColumns ( );
            Cvm = new ObservableCollection<ViewModels . CustomerViewModel> ( );
            Cvm = e . DataSource as ObservableCollection<ViewModels . CustomerViewModel>;
            this . grid1 . ItemsSource = Cvm;
            this . grid1 . CellStyle = FindResource ( "MAINCustomerGridStyle" ) as Style;
            DataTemplate dt = FindResource ( "CustomersDbTemplate1" ) as DataTemplate;
            this . grid1 . ItemTemplate = dt;
            this . grid1 . SelectedIndex = 0;
            this . grid1 . SelectedItem = 0;
            Utils . ScrollRecordIntoView ( grid1 , 0 );
            DbCountArgs args = new DbCountArgs ( );
            args . Dbcount = Cvm?.Count ?? -1;
            args . sender = "dgUserctrl";
            TabWinViewModel . TriggerBankDbCount ( this , args );
        }

        private void DgLoadDb ( object sender , LoadDbArgs e )
        {
            if ( e . dbname == "BANK" )
                LoadBank ( );
            else
                LoadCustomer ( );
        }

        public static DgUserControl SetController ( object ctrl )
        {
            HostViewModel = ctrl ;
            tabviewWin = TabWinViewModel . SendTabview ( );
            return ThisWin;
        }
        public async Task<ObservableCollection<BankAccountViewModel>> LoadBank ( bool update = true )
        {
            BankCollection bankcollection = new BankCollection ( );
            this . grid1 . ItemsSource = null;
            this . grid1 . Items . Clear ( );
            if ( Bvm == null ) Bvm = new ObservableCollection<BankAccountViewModel> ( );
            CurrentType = "BANK";
            TabWinViewModel . TriggerDbType ( CurrentType );

            Task task = Task . Run ( ( ) =>
            {
                // This is pretty fast - uses Dapper and Linq
                this . Dispatcher . Invoke ( ( ) =>
                {
                    Bvm = ( ObservableCollection<BankAccountViewModel> ) UserControlDataAccess . GetBankObsCollectionAsync ( Bvm , "" , true , "DgUserControl" );
                } );
                return Bvm;
            } );
            // This workks  too, but above call is faster
            //Task . Run (async ( ) => {
            //     this . Dispatcher . Invoke ( ( ) =>
            //     {
            //        UserControlDataAccess . GetBankObsCollection ( Bvm , true , "DgUserControl" );
            //    } );
            //} );
            return Bvm;
        }
        public void LoadCustomer ( bool update = true )
        {
            this . grid1 . ItemsSource = null;
            this . grid1 . Items . Clear ( );
            CurrentType = "CUSTOMER";
            TabWinViewModel . TriggerDbType ( CurrentType );
            if ( Cvm == null ) Cvm = new ObservableCollection<CustomerViewModel> ( );
            //            CustomerViewModel . GetCustObsCollectionWithDict ( Cvm );
            Task task = Task . Run ( ( ) =>
            {
                // This is pretty fast - uses Dapper and Linq
                this . Dispatcher . Invoke ( ( ) =>
                {
                    Cvm = ( ObservableCollection<CustomerViewModel> ) UserControlDataAccess . GetCustObsCollection ( Cvm , "" , true , "DgUserControl" );
                } );
            } );
        }

  
        private void grid1_SelectionChanged ( object sender , SelectionChangedEventArgs e )
        {
            if ( this . SelectionInAction )
            {
                this . SelectionInAction = false;
                return;
            }
            //DataGrid dg = sender as DataGrid;
            //if ( dg == null ) return;
            if ( this.grid1. Items . Count == 0 ) return;
            if ( this . grid1 . SelectedIndex == -1 )
            {
                //make sure we have something selected !
                this . grid1 . SelectedIndex = 0;
                this . grid1 . SelectedItem = 0;
            }
            //            DataGrid v = e . OriginalSource as DataGrid;
            DataGrid v = e . OriginalSource as DataGrid;
            if ( v?.SelectedIndex != CurrentIndex )
            {
                (double, int) t1 = (4.5, 3);
                CurrentIndex = v. SelectedIndex;
                
                if ( TrackselectionChanges )
                {
                    this . SelectionInAction = true;
                    SelectionChangedArgs args = new SelectionChangedArgs ( );
                    args . index = this . grid1 . SelectedIndex;
                    args . data = this . grid1 . SelectedItem;
                    args . sendertype = CurrentType;
                    args . sendername = "grid1";
                    Console . WriteLine ( $"DataGrid broadcasting selection set to  {args . index}" );
                    this . SelectionInAction = false;
                    EventControl . TriggerListSelectionChanged ( sender , args );
                }
                else this . SelectionInAction = false;
            }
            else
                this . SelectionInAction = false;

            Mouse . OverrideCursor = Cursors . Arrow;
            CurrentIndex = this . grid1 . SelectedIndex;
            Console . WriteLine ( $"{CurrentIndex} ...." );
        }
        private void SelectionHasChanged ( object sender , SelectionChangedArgs e )
        {
            bool success = false;
            object row = null;
            string custno = "", bankno = "";
            if ( DgUserControl . TrackselectionChanges == false ) return;
            // Another viewer has changed selection
            if ( this . grid1 . ItemsSource == null ) return;
            if ( sender . GetType ( ) == typeof ( DgUserControl ) ) return;

            int newindex = 0;
            if ( e . sendername != "grid1" )
            {
                if ( e . sendertype == "BANK" )
                {
                    // Sender is a BANK
                    BankAccountViewModel sourcerecord = new BankAccountViewModel ( );
                    sourcerecord = e . data as BankAccountViewModel;
                    if ( sourcerecord != null )
                    {
                        custno = sourcerecord . CustNo;
                        bankno = sourcerecord . BankNo;
                    }
                    else return;
                }
                else if ( e . sendertype == "CUSTOMER" )
                {
                    // Sender is a CUSTOMER
                    CustomerViewModel sourcerecord = new CustomerViewModel ( );
                    sourcerecord = e . data as CustomerViewModel;
                    if ( sourcerecord != null )
                    {
                        custno = sourcerecord . CustNo;
                        bankno = sourcerecord . BankNo;
                    }
                    else return;
                }
                if ( this . CurrentType == "CUSTOMER" )
                {
                    try
                    {
                        foreach ( CustomerViewModel item in this . grid1 . Items )
                        {
                            if ( item . CustNo == custno && item . BankNo == bankno )
                            {
                                this . SelectionInAction = true;
                                this . grid1 . SelectedIndex = newindex;
                                this . grid1 . SelectedItem = newindex;
                                CurrentIndex = newindex;

                                Console . WriteLine ( $"DataGrid selection in Customers matched on {custno}:{bankno}, index {newindex}" );
                                //
                                //row = Utils . GetRow ( grid1 , newindex );
                                Utils . ScrollRecordIntoView ( grid1 , newindex );
                                success = true;
                                break;
                            }
                            newindex++;
                        }
                    }
                    catch ( Exception ex ) { Console . WriteLine ( $"DataGrid failed search in Customer for match to {custno} : {bankno} : {ex . Message}" ); }
                }
                else
                {
                    try
                    {
                        foreach ( BankAccountViewModel item in this . grid1 . Items )
                        {
                            if ( item . CustNo == custno && item . BankNo == bankno )
                            {
                                this . SelectionInAction = true;
                                this . grid1 . SelectedIndex = newindex;
                                this . grid1 . SelectedItem = newindex;
                                CurrentIndex = newindex;
                                Console . WriteLine ( $"DataGrid selection in BankAccount matched on {custno}:{bankno}, index {newindex}" );
                                //row = Utils . GetRow ( grid1 , newindex );
                                Utils . ScrollRecordIntoView ( grid1 , newindex );
                                this . grid1 . UpdateLayout ( );
                                success = true;
                                break;
                            }
                            newindex++;
                        }
                    }
                    catch ( Exception ex ) { Console . WriteLine ( $"DataGrid failed search in Bank for match to {custno} : {bankno}" ); }
                }
                if ( success == false )
                    Console . WriteLine ( $"DataGrid failed search in Bank for match to {custno} : {bankno}" );
            }
            if ( success )
                Utils . ScrollRecordIntoView ( this . grid1 , newindex );
            Console . WriteLine ( $"DataGrid : {CurrentIndex} ...." );
        }

        public void PART_MouseLeave ( object sender , MouseEventArgs e )
        {
            var tabview = TabWinViewModel . Tview;
            if ( TabWinViewModel . CurrentTabTextBlock == "Tab1Header" )
            {
                tabview . Tab1Header . FontSize = 14;
                Tabview . TriggerStoryBoardOff ( 1 );
                tabview . Tab1Header . Foreground = FindResource ( "Cyan0" ) as SolidColorBrush;
            }
        }

        public void PART_MouseEnter ( object sender , MouseEventArgs e )
        {
            var tabview = TabWinViewModel . Tview;
            if ( TabWinViewModel . CurrentTabTextBlock == "Tab1Header" )
            {
                tabview . Tab1Header . FontSize = 18;
                Tabview . TriggerStoryBoardOn ( 1 );
                tabview . Tab1Header . Foreground = FindResource ( "Yellow0" ) as SolidColorBrush;
            }
        }

  
        #region DataGrid columns creation
        private void CreateBankColumns ( )
        {
            grid1 . Columns . Clear ( );
            Console . WriteLine ( $"CREATING BANK COLUMNS" );
            DataGridTextColumn c1 = new DataGridTextColumn ( );
            c1 . Header = "Id";
            c1 . Binding = new Binding ( "Id" );
            grid1 . Columns . Add ( c1 );
            DataGridTextColumn c2 = new DataGridTextColumn ( );
            c2 . Header = "Customer #";
            c2 . Binding = new Binding ( "CustNo" );
            grid1 . Columns . Add ( c2 );
            DataGridTextColumn c3 = new DataGridTextColumn ( );
            c3 . Header = "Bank #";
            c3 . Binding = new Binding ( "BankNo" );
            grid1 . Columns . Add ( c3 );
            DataGridTextColumn c4 = new DataGridTextColumn ( );
            c4 . Header = "A/c Type";
            c4 . Binding = new Binding ( "AcType" );
            grid1 . Columns . Add ( c4 );
            DataGridTextColumn c5 = new DataGridTextColumn ( );
            c5 . Header = "Balance";
            c5 . Binding = new Binding ( "Balance" );
            grid1 . Columns . Add ( c5 );
            DataGridTextColumn c6 = new DataGridTextColumn ( );
            c6 . Header = "Opened";
            c6 . Binding = new Binding ( "ODate" );
            grid1 . Columns . Add ( c6 );
            DataGridTextColumn c7 = new DataGridTextColumn ( );
            c7 . Header = "Closed";
            c7 . Binding = new Binding ( "CDate" );
            grid1 . Columns . Add ( c7 );
        }
        private void CreateCustomerColumns ( )
        {
            grid1 . Columns . Clear ( );
            Console . WriteLine ( $"CREATING CUSTOMER COLUMNS" );
            DataGridTextColumn c1 = new DataGridTextColumn ( );
            c1 . Header = "Id";
            c1 . Binding = new Binding ( "Id" );
            grid1 . Columns . Add ( c1 );
            DataGridTextColumn c2 = new DataGridTextColumn ( );
            c2 . Header = "Customer #";
            c2 . Binding = new Binding ( "CustNo" );
            grid1 . Columns . Add ( c2 );
            DataGridTextColumn c3 = new DataGridTextColumn ( );
            c3 . Header = "Bank #";
            c3 . Binding = new Binding ( "BankNo" );
            grid1 . Columns . Add ( c3 );
            DataGridTextColumn c4 = new DataGridTextColumn ( );
            c4 . Header = "A/c Type";
            c4 . Binding = new Binding ( "AcType" );
            grid1 . Columns . Add ( c4 );
            DataGridTextColumn c5 = new DataGridTextColumn ( );
            c5 . Header = "Address1";
            c5 . Binding = new Binding ( "Addr1" );
            grid1 . Columns . Add ( c5 );
            DataGridTextColumn c6 = new DataGridTextColumn ( );
            c6 . Header = "Address2";
            c6 . Binding = new Binding ( "Addr2" );
            grid1 . Columns . Add ( c6 );
            DataGridTextColumn c7 = new DataGridTextColumn ( );
            c7 . Header = "Town";
            c7 . Binding = new Binding ( "Town" );
            grid1 . Columns . Add ( c7 );
            DataGridTextColumn c8 = new DataGridTextColumn ( );
            c8 . Header = "County";
            c8 . Binding = new Binding ( "County" );
            grid1 . Columns . Add ( c8 );
            DataGridTextColumn c9 = new DataGridTextColumn ( );
            c9 . Header = "Zip";
            c9 . Binding = new Binding ( "PCode" );
            grid1 . Columns . Add ( c9 );
            DataGridTextColumn c10 = new DataGridTextColumn ( );
            c10 . Header = "Opened";
            c10 . Binding = new Binding ( "ODate" );
            grid1 . Columns . Add ( c10 );
            DataGridTextColumn c11 = new DataGridTextColumn ( );
            c11 . Header = "Closed";
            c11 . Binding = new Binding ( "CDate" );
            grid1 . Columns . Add ( c11 );
        }
        #endregion DataGrid columns creation

        #region sundry grid focus stuff
        private void grid1_CellEditEnding ( object sender , DataGridCellEditEndingEventArgs e )
        {
            //DataGridRow  row = e.Row; 
        }
        private void HandleEscKey ( object sender , ExecutedRoutedEventArgs e )
        {
            grid1 . SelectedItem = CurrentIndex;
        }
        private void Grid1_GotKeyboardFocus ( object sender , KeyboardFocusChangedEventArgs e )
        {
            bool cancelled = grid1 . CancelEdit ( );
            Console . WriteLine ( $"still in Edit ? {cancelled}" );
            HandleEscKey ( sender , null );
        }
        private void Grid1_PreviewKeyDown ( object sender , KeyEventArgs e )
        {
            //            throw new NotImplementedException ( );
        }
        private void grid1_PreparingCellForEdit ( object sender , DataGridPreparingCellForEditEventArgs e )
        {
            //never gets  here !!!!
            var v = e . EditingEventArgs;
            v . Handled = true;
        }
        private void grid1_PreviewMouseMove1 ( object sender , MouseEventArgs e )
        {
            //DataGrid dgSender = sender as DataGrid;
            //if ( dgSender != null )
            //{
            //    if ( dgSender . Name == "grid1" )
            //    {
            //        TabWinViewModel . CurrentTabIndex = 0;
            //        TabWinViewModel . CurrentTabName = "DgridTab";
            //        TabWinViewModel . CurrentTabTextBlock = "Tab1Header";
            //    }
            //}

        }
        private void grid1_Loaded ( object sender , System . Windows . RoutedEventArgs e )
        {
            this . SelectionInAction = false;
            Console . WriteLine ( $"Datagrid : this . SelectionInAction = false" );
        }
        private void grid1_GotFocus ( object sender , RoutedEventArgs e )
        {
            grid1 . CancelEdit ( );
            ReceivedFocus (null );
        }
        private void grid1_LostFocus ( object sender , RoutedEventArgs e )
        {
            Console . WriteLine ( $"Datagrid lost focus" );
            //            this . SelectionInAction = false;
        }
        private void dg_Editing ( object sender , DataGridBeginningEditEventArgs e )
        {
        }
        private void dg_CellEditEnding ( object sender , DataGridCellEditEndingEventArgs e )
        {
        }
        private void dg_RowEditEnding ( object sender , DataGridRowEditEndingEventArgs e )
        {
        }
        private void grid1_BeginningEdit ( object sender , DataGridBeginningEditEventArgs e )
        {
        }
        
        #endregion sundry grid focus stuff

        private void TabControl_PreviewMouseDown ( object sender , MouseButtonEventArgs e )
        {
            if ( IsUnderTabHeader ( e . OriginalSource as DependencyObject ) )
                CommitTables ( sender as Tabview );
        }
        private bool IsUnderTabHeader ( DependencyObject control )
        {
            if ( control is TabItem ) return true;
            DependencyObject parent = VisualTreeHelper . GetParent ( control );
            if ( parent == null )
                return false;
            return IsUnderTabHeader ( parent );
        }
        private void CommitTables ( DependencyObject control )
        {
            if ( control is DataGrid )
            {
                DataGrid grid = control as DataGrid;
                grid . CommitEdit ( DataGridEditingUnit . Row , true );
                return;
            }
            int childrenCount = VisualTreeHelper . GetChildrenCount ( control );
            for ( int childIndex = 0 ; childIndex < childrenCount ; childIndex++ )
                CommitTables ( VisualTreeHelper . GetChild ( control , childIndex ) );
        }
    }
}
