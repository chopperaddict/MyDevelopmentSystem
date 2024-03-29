﻿using System;
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

    public partial class DgUserControl : UserControl
    {
        const string FileName = @"DgUserControl.bin"; 
        DatagridUserControlViewModel dgridctrlvm { get; set; }

        #region header block
        public ObservableCollection<ViewModels . BankAccountViewModel> Bvm { get; private set; }
        public ObservableCollection<ViewModels . CustomerViewModel> Cvm { get; private set; }
        //       private static TabWinViewModel Controller { get; set; }
//        public static object HostViewModel { get; set; }
//        public static Tabview tabviewWin { get; set; }
        public string CurrentType { set; get; } = "CUSTOMER";
        //private static DgUserControl ThisWin { get; set; }
        new private bool IsLoaded { get; set; } = false;
        private bool SelectionInAction { get; set; } = false;
        public static bool TrackselectionChanges { get; set; } = false;
        private int CurrentIndex { get; set; } = 0;
        #endregion header block

        #region Serialization
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
        public void WriteSerializedObject ( )
        {
            Stream SaveFileStream = File . Create ( FileName );
            BinaryFormatter serializer = new BinaryFormatter ( );
            serializer . Serialize ( SaveFileStream , grid1 );
            SaveFileStream . Close ( );
        }
        #endregion Serialization

        public DgUserControl ( )
        {
            InitializeComponent ( );
            Console . WriteLine ( $"DataGrid Control Loading ......" );
            //ThisWin = this;

            // setup DP pointer in Tabview to DgUserControl using shortcut command line !
            Tabview . GetTabview(). Dgusercontrol = this; 
            
            //Set Datagrid AP pointer in Tabview
            Tabview . SetDGControl ( this , this.grid1 );

            // setup local data collections
            Bvm = new ObservableCollection<BankAccountViewModel> ( );
            Cvm = new ObservableCollection<CustomerViewModel> ( );

            //setup DataContext
            dgridctrlvm = new DatagridUserControlViewModel ( this );
            this . DataContext = dgridctrlvm;

            
            // setup required Hooks
            this . grid1 . SelectionChanged += dgridctrlvm . grid1_SelectionChanged;
            
            // allow this  to broadcast
            EventControl . TriggerWindowMessage ( this , new InterWindowArgs { message = $"DgUIserControl loaded, ViewMode is DATAGRIDUSERCONTROLVIEWMODEL..." } );
            IsLoaded = false;
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
            }
            return;
        }


        public static void SetListSelectionChanged ( bool arg )
        {
            TrackselectionChanges = arg;
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
            Task task = Task . Run ( ( ) =>
            {
                // This is pretty fast - uses Dapper and Linq
                this . Dispatcher . Invoke ( ( ) =>
                {
                    Cvm = ( ObservableCollection<CustomerViewModel> ) UserControlDataAccess . GetCustObsCollection ( Cvm , "" , true , "DgUserControl" );
                } );
            } );
        }

        #region Hilite TabItem header on mouse Entry / Exit
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
        #endregion Hilite TabItem header on mouse Entry / Exit

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
 
        //#endregion sundry grid focus stuff
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
