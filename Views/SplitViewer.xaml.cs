using System;
using System . Collections . Generic;
using System . ComponentModel;
using System . Data . SqlClient;
using System . Data;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Controls . Primitives;
using System . Windows . Data;
using System . Windows . Documents;
using System . Windows . Input;
using System . Windows . Media;
using System . Windows . Media . Imaging;
using System . Windows . Shapes;

using MyDev . Models;
using MyDev . UserControls;
using MyDev . ViewModels;
using MyDev . Dapper;
using MyDev . SQL;
using System . Collections . ObjectModel;
using System . Xml . Linq;
using static System . Windows . Forms . VisualStyles . VisualStyleElement . ProgressBar;
using MyDev . Sql;
using static System . Windows . Forms . LinkLabel;
using System . Windows . Media . Animation;


namespace MyDev . Views
{
    /// <summary>
    /// Interaction logic for SplitViewer.xaml
    /// </summary>
    public partial class SplitViewer : Window
    {
        public BankAccountViewModel bvm = new BankAccountViewModel ( );
        public CustomerViewModel cvm = new CustomerViewModel ( );
        public DetailsViewModel dvm = new DetailsViewModel ( );
        public GenericClass gvm = new GenericClass ( );

        // Collections
        public ObservableCollection<BankAccountViewModel> bankaccts = new ObservableCollection<BankAccountViewModel> ( );
        public ObservableCollection<CustomerViewModel> custaccts = new ObservableCollection<CustomerViewModel> ( );
        public ObservableCollection<DetailsViewModel> detaccts = new ObservableCollection<DetailsViewModel> ( );
        public ObservableCollection<GenericClass> genaccts = new ObservableCollection<GenericClass> ( );

        #region All Template setup stuff

        #region Variables used for splitters
        private double MaxColWidth1 { get; set; }
        private double MinRowHeight1 { get; set; }
        private double MaxRowHeight { get; set; }
        #endregion Variables used for splitters

        private string SqlCommand = "";
        private string DefaultSqlCommand = "Select * from BankAccount";
        private bool UseBGThread = false;
        private bool LoadDirect = true;

        #region Binding full props
        // Full properties used in Binding to I/f objects

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

        private double FirstXPos = 0;
        private double FirstYPos = 0;

        #endregion Binding full props

        #region Flowdoc file wide variables
        public FlowdocLib fdl = new FlowdocLib ( );
        private double XLeft = 0;
        private double YTop = 0;
        #endregion Flowdoc file wide variables

        #endregion All Template setup stuff

        #region Startup
        public SplitViewer ( )
        {
            InitializeComponent ( );
            // MOST important line !!!!
            this . DataContext = this;
            MaxColWidth1 = 340;
            MinRowHeight1 = 255;
            MaxRowHeight = 275;
        }
        private void Window_Loaded ( object sender , RoutedEventArgs e )
        {
            // Startup settings
            #region Template settings
            #region  Handling for splitters
            imgup = new BitmapImage ( new Uri ( @"\icons\down arroiw red.png" , UriKind . Relative ) );
            vimgmove = new BitmapImage ( new Uri ( @"\icons\Lrg ltrt arrow red copy.png" , UriKind . Relative ) );
            LhHsplitter = new BitmapImage ( new Uri ( @"\icons\down arroiw red.png" , UriKind . Relative ) );
            // This sets the relative height of t Grid'Left hand side row heights - works  too
            //LeftPanelgrid . RowDefinitions [ 0 ] . Height = new GridLength ( 20 , GridUnitType . Star );
            //LeftPanelgrid . RowDefinitions [ 1 ] . Height = new GridLength ( 20 , GridUnitType . Pixel );
            //LeftPanelgrid . RowDefinitions [ 2 ] . Height = new GridLength ( 20 , GridUnitType . Star );

            //LeftSideContainer . RowDefinitions [ 0 ] . Height = new GridLength ( 0.00 , GridUnitType . Pixel );
            //LeftSideContainer . RowDefinitions [ 1 ] . Height = new GridLength ( 0 , GridUnitType . Star );
            //LeftSideContainer . RowDefinitions [ 2 ] . Height = new GridLength ( 20 , GridUnitType . Pixel );
            //LeftSideContainer . RowDefinitions [ 3 ] . Height = new GridLength ( 300 , GridUnitType . Star );
            //LeftSideContainer . RowDefinitions [ 4 ] . Height = new GridLength ( 75 , GridUnitType . Pixel );

            //RightPanelGrid . RowDefinitions [ 0 ] . Height = new GridLength ( 20 , GridUnitType . Star );
            //RightPanelGrid . RowDefinitions [ 1 ] . Height = new GridLength ( 20 , GridUnitType . Pixel );
            //RightPanelGrid . RowDefinitions [ 2 ] . Height = new GridLength ( 20 , GridUnitType . Star );
            #endregion  Handling for splitters
            #endregion Template settings
            #region Template settings
            Flowdoc . ExecuteFlowDocMaxmizeMethod += new EventHandler ( MaximizeFlowDoc );
            ShowdragText = "Drag ";
            ShowText = "Adjust splitter Panel's size";
            imgup = new BitmapImage ( new Uri ( @"\icons\Lrg updown arrow red copy.png" , UriKind . Relative ) );

            this . SetValue ( MaxSplitterWidthProperty , this . ActualWidth );
            this . SetValue ( MaxSplitterHeightProperty , this . ActualHeight );
            #endregion Template   settings

            DataGrid1 . ItemsSource = null;
            DataGrid1 . Items . Clear ( );
            DataGrid2 . ItemsSource = null;
            DataGrid2 . Items . Clear ( );
            LoadTablesList ( );
            SqlCommand = DefaultSqlCommand;
            LoadData ( DataGrid1 );
            LoadData ( DataGrid2 );
            //if ( bankaccts != null )
            //{
            //    DataGrid1 . ItemsSource = bankaccts;
            //   DataGrid2 . ItemsSource = bankaccts;
            //}

            //LeftSideContainer . RowDefinitions [ 0 ] . Height = new GridLength ( 1 , GridUnitType . Star );
            //LeftSideContainer . RowDefinitions [ 1 ] . Height = new GridLength ( 65 , GridUnitType . Pixel );

            //LeftSideContainer . ColumnDefinitions [ 0 ] . Width = new GridLength ( 4 , GridUnitType . Star );
            //LeftSideContainer . ColumnDefinitions [ 1 ] . Width = new GridLength ( 20 , GridUnitType . Pixel );
            //LeftSideContainer . ColumnDefinitions [ 2 ] . Width= new GridLength ( 480 , GridUnitType . Pixel );

            //RightInsidegrid . RowDefinitions [ 0 ] . Height = new GridLength ( 0.01 , GridUnitType . Star );
            //RightInsidegrid . RowDefinitions [ 1 ] . Height = new GridLength ( 20 , GridUnitType . Pixel);
            //RightInsidegrid . RowDefinitions [ 2 ] . Height = new GridLength ( 1 , GridUnitType . Star );
        }
        #endregion Startup

        #region Data Loading
        private void LoadData ( DataGrid dgrid )
        {
            if ( UseBGThread )
            {
                //    // This calls various methods that run on a Background Thread
                //    if ( SqlCommand . Contains ( " " ) == false || SqlCommand . ToUpper ( ) . Trim ( ) . Substring ( 0 , 2 ) == "SP" )
                //    {
                //        // process a Stored procedure
                //         DataLoadControl . GetDataTable ( SqlCommand );
                //    }
                //    else
                //    {     //process any other type of cmomand
                //        SqlCommand = CheckLimits ( );
                //        BackgroundWorkerLoad ( );
                //    }
            }
            if ( LoadDirect )
            {
                // WORKING 5.2.22
                // This creates and loads a GenericClass table if data is found in the selected table
                DataTable dt = DataLoadControl . GetDataTable ( SqlCommand );
                genaccts = SqlSupport . LoadGenericCollection ( dt );
                dgrid . ItemsSource = null;
                dgrid . Items . Clear ( );
                SqlServerCommands . LoadActiveRowsOnlyInGrid ( dgrid , genaccts , SqlServerCommands . GetGenericColumnCount ( genaccts ) );
            }
        }

        public void LoadTablesList ( )
        {
            int bankindex = 0, count = 0;
            List<string> list = new List<string> ( );
            SqlCommand = "spGetTablesList";
            dbName . Items . Clear ( );
            CallStoredProcedure ( list , SqlCommand );
            //This call returns us a DataTable
            DataTable dt = DataLoadControl . GetDataTable ( SqlCommand );
            // This how to access  Row data from  a grid the easiest way.... parsed into a List <xxxxx>
            // using :- list = Utils . GetDataDridRowsAsListOfStrings ( dt );
            bankindex = AddDtEntriesToCombo ( dbName , dt );
            // how to Sort Combo/Listbox contents
            dbName . Items . SortDescriptions . Add ( new SortDescription ( "" , ListSortDirection . Ascending ) );
            dbName . SelectedIndex = bankindex;
            bankindex = AddDtEntriesToCombo ( dbName2 , dt );
            // how to Sort Combo/Listbox contents
            dbName2 . Items . SortDescriptions . Add ( new SortDescription ( "" , ListSortDirection . Ascending ) );
            dbName2 . SelectedIndex = bankindex;
        }
        public static int AddDtEntriesToCombo ( ComboBox cb , DataTable dt )
        {
            int bankindex = 0, count = 0;
            List<string> list = Utils . GetDataDridRowsAsListOfStrings ( dt );
            foreach ( string row in list )
            {
                cb . Items . Add ( row );
                if ( row . ToUpper ( ) == "BANKACCOUNT" )
                    bankindex = count;
                count++;
            }
            return bankindex;
        }

        #endregion Data Loading

        #region Lower level data support methods

        public static List<string> CallStoredProcedure ( List<string> list , string sqlcommand )
        {
            //This call returns us a DataTable
            DataTable dt = DataLoadControl . GetDataTable ( sqlcommand );
            if ( dt != null )
                //				list = GenericDbHandlers.GetDataDridRowsWithSizes ( dt );
                list = Utils . GetDataDridRowsAsListOfStrings ( dt );
            return list;
        }

        #endregion Lower level data support methods

        #region DP.s
        public double LeftTopSplitterOffset
        {
            get { return ( double ) GetValue ( LeftTopSplitterOffsetProperty ); }
            set { SetValue ( LeftTopSplitterOffsetProperty , value ); }
        }
        public static readonly DependencyProperty LeftTopSplitterOffsetProperty =
            DependencyProperty . Register ( "LeftTopSplitterOffset" , typeof ( double ) , typeof ( SplitViewer ) , new PropertyMetadata ( ( double ) 0 ) );
        public string LeftSplitterText
        {
            get { return ( string ) GetValue ( LeftSplitterTextProperty ); }
            set { SetValue ( LeftSplitterTextProperty , value ); }
        }
        public static readonly DependencyProperty LeftSplitterTextProperty =
           DependencyProperty . Register ( "LeftSplitterText" , typeof ( string ) , typeof ( SplitViewer ) , new PropertyMetadata ( "Drag Down  " ) );
        public string ShowText
        {
            get { return ( string ) GetValue ( ShowTextProperty ); }
            set { SetValue ( ShowTextProperty , value ); }
        }
        public static readonly DependencyProperty ShowTextProperty =
            DependencyProperty . Register ( "ShowText" , typeof ( string ) , typeof ( SplitViewer ) , new PropertyMetadata ( "Show More Records" ) );
        public string ShowdragText
        {
            get { return ( string ) GetValue ( ShowdragTextProperty ); }
            set { SetValue ( ShowdragTextProperty , value ); }
        }
        public static readonly DependencyProperty ShowdragTextProperty =
            DependencyProperty . Register ( "ShowdragText" , typeof ( string ) , typeof ( SplitViewer ) , new PropertyMetadata ( "Drag Up/Down to  " ) );
        public BitmapImage imgup
        {
            get { return ( BitmapImage ) GetValue ( imgupProperty ); }
            set { SetValue ( imgupProperty , value ); }
        }
        public static readonly DependencyProperty imgupProperty =
            DependencyProperty . Register ( "imgup" , typeof ( BitmapImage ) ,
    typeof ( SplitViewer ) ,
                 new PropertyMetadata ( null ) );
        public BitmapImage imgdn
        {
            get { return ( BitmapImage ) GetValue ( imgdnProperty ); }
            set { SetValue ( imgdnProperty , value ); }
        }
        public static readonly DependencyProperty imgdnProperty =
            DependencyProperty . Register ( "imgdn" ,
                 typeof ( BitmapImage ) ,
                   typeof ( SplitViewer ) ,
                new PropertyMetadata ( null ) );
        public BitmapImage imgmv
        {
            get { return ( BitmapImage ) GetValue ( imgmvProperty ); }
            set { SetValue ( imgmvProperty , value ); }
        }
        public static readonly DependencyProperty imgmvProperty =
             DependencyProperty . Register ( "imgmv" ,
                  typeof ( BitmapImage ) ,
                    typeof ( SplitViewer ) ,
                 new PropertyMetadata ( null ) );
        public BitmapImage vimgleft
        {
            get { return ( BitmapImage ) GetValue ( vimgleftProperty ); }
            set { SetValue ( vimgleftProperty , value ); }
        }
        public static readonly DependencyProperty vimgleftProperty =
            DependencyProperty . Register ( "vimgleft" , typeof ( BitmapImage ) , typeof ( SplitViewer ) , new PropertyMetadata ( null ) );
        public BitmapImage vimgright
        {
            get { return ( BitmapImage ) GetValue ( vimgrightProperty ); }
            set { SetValue ( vimgrightProperty , value ); }
        }
        public static readonly DependencyProperty vimgrightProperty =
           DependencyProperty . Register ( "vimgright" , typeof ( BitmapImage ) , typeof ( SplitViewer ) , new PropertyMetadata ( null ) );
        public BitmapImage vimgmove
        {
            get { return ( BitmapImage ) GetValue ( vimgmoveProperty ); }
            set { SetValue ( vimgmoveProperty , value ); }
        }
        public static readonly DependencyProperty vimgmoveProperty =
            DependencyProperty . Register ( "vimgmove" , typeof ( BitmapImage ) , typeof ( SplitViewer ) , new PropertyMetadata ( null ) );
        public int DbCount
        {
            get { return ( int ) GetValue ( DbCountProperty ); }
            set { SetValue ( DbCountProperty , value ); }
        }
        public BitmapImage LhHsplitter
        {
            get { return ( BitmapImage ) GetValue ( LhHsplitterProperty ); }
            set { SetValue ( LhHsplitterProperty , value ); }
        }
        public static readonly DependencyProperty LhHsplitterProperty =
            DependencyProperty . Register ( "LhHsplitter" , typeof ( BitmapImage ) , typeof ( SplitViewer ) , new PropertyMetadata ( null ) );

        // Using a DependencyProperty as the backing store for DbCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DbCountProperty =
            DependencyProperty . Register ( "DbCount" , typeof ( int ) , typeof ( SplitViewer ) , new PropertyMetadata ( 0 ) );
        public double MaxSplitterHeight
        {
            get { return ( double ) GetValue ( MaxSplitterHeightProperty ); }
            set { SetValue ( MaxSplitterHeightProperty , value ); }
        }
        public static readonly DependencyProperty MaxSplitterHeightProperty =
            DependencyProperty . Register ( "MaxSplitterHeight" , typeof ( double ) , typeof ( SplitViewer ) , new PropertyMetadata ( ( double ) 500 ) );
        public double MaxSplitterWidth
        {
            get { return ( double ) GetValue ( MaxSplitterWidthProperty ); }
            set { SetValue ( MaxSplitterWidthProperty , value ); }
        }
        public static readonly DependencyProperty MaxSplitterWidthProperty =
            DependencyProperty . Register ( "MaxSplitterWidth" , typeof ( double ) , typeof ( SplitViewer ) , new PropertyMetadata ( ( double ) 500 ) );
        #endregion DP.s

        #region FlowDoc support
        /// <summary>
        ///  These are the only methods any window needs ot provide support for my FlowDoc system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void MaximizeFlowDoc ( object sender , EventArgs e )
        {
            // Clever "Hook" method that Allows the flowdoc to be resized to fill window
            // or return to its original size and position courtesy of the Event declard in FlowDoc
            fdl . MaximizeFlowDoc ( Flowdoc , canvas , e );
        }
        private void Flowdoc_MouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
        {
            // Window wide  !!
            // Called  when a Flowdoc MOVE has ended
            MovingObject = fdl . Flowdoc_MouseLeftButtonUp ( sender , Flowdoc , MovingObject , e );
            VSplitDown = false;
            ReleaseMouseCapture ( );
        }
        private void Flowdoc_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
        {
            //In this event, we get current mouse position on the control to use it in the MouseMove event.
            MovingObject = fdl . Flowdoc_PreviewMouseLeftButtonDown ( sender , Flowdoc , e );
        }
        private void Flowdoc_MouseMove ( object sender , MouseEventArgs e )
        {
            // We are Resizing the Flowdoc using the mouse on the border  (Border.Name=FdBorder)
            fdl . Flowdoc_MouseMove ( Flowdoc , canvas , MovingObject , e );
        }
        // Shortened version proxy call		
        private void Flowdoc_LostFocus ( object sender , RoutedEventArgs e )
        {
            Flowdoc . BorderClicked = false;
        }
        public void FlowDoc_ExecuteFlowDocBorderMethod ( object sender , EventArgs e )
        {
            // EVENTHANDLER to Handle resizing
            FlowDoc fd = sender as FlowDoc;
            Point pt = Mouse . GetPosition ( canvas );
            double dLeft = pt . X;
            double dTop = pt . Y;
        }
        public void fdmsg ( string line1 , string line2 = "" , string line3 = "" )
        {
            //We have to pass the Flowdoc.Name, and Canvas.Name as well as up   to 3 strings of message
            //  you can  just provie one if required
            // eg fdmsg("message text");
            fdl . FdMsg ( Flowdoc , canvas , line1 , line2 , line3 );
        }
        #endregion FlowDoc support  

        #region Splitter support methods

        #region Left Horizontal Splitter

        private void LeftSplitter_DragStarted ( object sender , DragStartedEventArgs e )
        {
            if ( slideupPanel . ActualHeight <= 3 )
            {
                LeftSplitterText = "Drag Up   ";
                LhHsplitter = new BitmapImage ( new Uri ( @"\icons\up arroiw red.png" , UriKind . Relative ) );
                ShowText = "To view lower Datagrid";
            }
            else if ( LeftHorzintalSplitter . ActualHeight <= 10 )
            {
                LeftSplitterText = "Drag Down ";
                LhHsplitter = new BitmapImage ( new Uri ( @"\icons\down arroiw red.png" , UriKind . Relative ) );
                ShowText = "To change view proportions";
            }
            else
            {
                LeftSplitterText = "Drag";
                LhHsplitter = new BitmapImage ( new Uri ( @"\icons\Lrg updown arrow red copy.png" , UriKind . Relative ) );
                ShowText = "To change Datagrids view position split";
            }
        }
        private void LeftSplitter_DragCompleted ( object sender , DragCompletedEventArgs e )
        {
            if ( slidedownPanel . ActualHeight <= 10 )
            {
                LeftSplitterText = "Drag Down   ";
                LhHsplitter = new BitmapImage ( new Uri ( @"\icons\down arroiw red.png" , UriKind . Relative ) );
                ShowText = "To display more of upper Datagrid";
                LeftTopSplitterOffset = slidedownPanel . ActualHeight;
                Thickness th = new Thickness ( 0 , 0 , 0 , 0 );
                th . Top = LeftTopSplitterOffset;
                lowerleftpanel . Margin = th;
            }
            else if ( slideupPanel . ActualHeight <= 10 )
            {
                LeftSplitterText = "Drag Up  ";
                LhHsplitter = new BitmapImage ( new Uri ( @"\icons\up arroiw red.png" , UriKind . Relative ) );
                ShowText = "To display more of lower DataGrid.";
                LeftTopSplitterOffset = slidedownPanel . ActualHeight;
                Thickness th = new Thickness ( 0 , 0 , 0 , 0 );
                lowerleftpanel . Margin = th;
            }
            else
            {
                LeftSplitterText = "Drag";
                LhHsplitter = new BitmapImage ( new Uri ( @"\icons\Lrg updown arrow red copy.png" , UriKind . Relative ) );
                ShowText = "To change Datagrids view position split";
                // Green row grid
                LeftTopSplitterOffset = slidedownPanel . ActualHeight;
                Thickness th = new Thickness ( 0 , 0 , 0 , 0 );
                lowerleftpanel . Margin = th;
                // Yellow  row grid
                Thickness th2 = new Thickness ( 0 , 0 , 0 , 0 );
                th2 . Top = LeftTopSplitterOffset;
                lowerleftpanel . Margin = th2;
            }
        }
        #endregion Left Horizontal Splitter

        #region Right Horizontal splitter resize handlers
        //All working well 31/3/2022 - MY BIRTHDAY !!
        private void RtSplitter_DragStarted ( object sender , System . Windows . Controls . Primitives . DragStartedEventArgs e )
        {
            if ( Middlerow1 . ActualHeight <= 3 )
            {
                imgup = new BitmapImage ( new Uri ( @"\icons\down arroiw red.png" , UriKind . Relative ) );
                ShowdragText = "Drag ";
                ShowText = "show Upper Options Pane";
            }
            else if ( Middlerow2 . ActualHeight <= 10 )
            {
                // At bottom of screen
                imgup = new BitmapImage ( new Uri ( @"\icons\up arroiw red.png" , UriKind . Relative ) );
                ShowdragText = "Drag ";
                ShowText = "show lower Options panel";
            }
            else
            {
                imgup = new BitmapImage ( new Uri ( @"\icons\Lrg updown arrow red copy.png" , UriKind . Relative ) );
                ShowdragText = "Drag ";
                ShowText = "Adjust Splitter Panels size";
            }
        }
        private void RtSplitter_DragCompleted ( object sender , System . Windows . Controls . Primitives . DragCompletedEventArgs e )
        {
            if ( Middlerow2 . ActualHeight <= 10 )
            {
                imgup = new BitmapImage ( new Uri ( @"\icons\up arroiw red.png" , UriKind . Relative ) );
                ShowdragText = "Drag";
                ShowText = "show lower Options Pane";
            }
            else if ( Middlerow1 . ActualHeight <= 10 )
            {
                imgup = new BitmapImage ( new Uri ( @"\icons\down arroiw red.png" , UriKind . Relative ) );
                ShowdragText = "Drag ";
                ShowText = "show upper Options Pane";
            }
            else
            {
                imgup = new BitmapImage ( new Uri ( @"\icons\Lrg updown arrow red copy.png" , UriKind . Relative ) );
                //                imgmv = new BitmapImage ( new Uri ( @"\icons\sync.ico" , UriKind . Relative ) );
                ShowdragText = "Drag ";
                ShowText = "Adjust Splitter Panel's size";
            }
        }
        #endregion Horizontal splitter resize handlers

        #region Vertical splitter resize handlers
        //All working well 31/3/2022 - MY BIRTHDAY !!
        private void VSplitter_DragStarted ( object sender , DragStartedEventArgs e )
        {

            //if ( Col0 . ActualWidth >= MaxSplitterWidth )
            //{
            //    vimgmove = new BitmapImage ( new Uri ( @"\icons\left arroiw red.png" , UriKind . Relative ) );
            //}
            //else if ( Col0 . ActualWidth <= 11 )
            //{
            //    vimgmove = new BitmapImage ( new Uri ( @"\icons\right arroiw red.png" , UriKind . Relative ) );
            //}
            //else
            //{
            //    vimgmove = new BitmapImage ( new Uri ( @"\icons\Lrg ltrt arrow red copy.png" , UriKind . Relative ) );
            //}
        }

        private void VSplitter_DragCompleted ( object sender , DragCompletedEventArgs e )
        {
            //double width = Col0 . ActualWidth;
            //double rwidth = Col2 . ActualWidth;
            //Console . WriteLine ( $"{width}, {rwidth}, {MaxSplitterWidth}" );
            //if ( Col0 . ActualWidth <= 10 )
            //{
            //    vimgmove = new BitmapImage ( new Uri ( @"\icons\right arroiw red.png" , UriKind . Relative ) );
            //}
            //else if ( rwidth <= 10 )
            //{
            //    vimgmove = new BitmapImage ( new Uri ( @"\icons\left arroiw red.png" , UriKind . Relative ) );
            //}
            //else
            //{
            //    vimgmove = new BitmapImage ( new Uri ( @"\icons\Lrg ltrt arrow red copy.png" , UriKind . Relative ) );
            //}
        }
        #endregion Vertical splitter resize handlers

        #endregion Splitter support methods

        #region Closedown
        private void Window_Closing ( object sender , System . ComponentModel . CancelEventArgs e )
        {

        }
        private void App_Close ( object sender , RoutedEventArgs e )
        {
            // Close down Application
            this . Close ( );
            Application . Current . Shutdown ( );
        }
        private void Datagrids_Close ( object sender , RoutedEventArgs e )
        {
            this . Close ( );
        }

        #endregion Closedown

        private void Window_PreviewKeyDown ( object sender , KeyEventArgs e )
        {

        }

        private void Window_PreviewMouseMove ( object sender , MouseEventArgs e )
        {

        }

        private void splttermplatet_SizeChanged ( object sender , SizeChangedEventArgs e )
        {
            Size size = e . NewSize;

            LeftTopSplitterOffset = slidedownPanel . ActualHeight;
            //Thickness th1 = new Thickness ( 0 , 0 , 0 , 0 );
            //th1 . Top = LeftTopSplitterOffset;
            //lowerleftpanel . Margin = th1;

            Thickness th2 = new Thickness ( 0 , 0 , 0 , 0 );
            th2 . Top = LeftTopSplitterOffset;
            lowerleftpanel . Margin = th2;

            //Thickness th = new Thickness ( 0 , 0 , 0 , 0 );
            //th = vsplitterbar . Margin;
            //th . Left = lowerleftpanel . ActualWidth - 120;
            //vsplitterbar . Margin = th;
        }

        private void dbName_SelectionChanged ( object sender , SelectionChangedEventArgs e )
        {
            string selection = dbName . SelectedItem . ToString ( );
            DataGrid1 . ItemsSource = null;
            DataGrid1 . Items . Clear ( );
            SqlCommand = $"Select * from {selection}";
            LoadData ( DataGrid1 );
        }

        private void dbName2_SelectionChanged ( object sender , SelectionChangedEventArgs e )
        {
            string selection = dbName2 . SelectedItem . ToString ( );
            DataGrid2 . ItemsSource = null;
            DataGrid2 . Items . Clear ( );
            SqlCommand = $"Select * from {selection}";
            LoadData ( DataGrid2 );
        }

        private bool VSplitDown = false;
        private double LeftWidth = 0;
        private double RightWidth = 0;
        private void vsplitterbar_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
        {
            VSplitDown = true;
        }

        private void vsplitterbar_PreviewMouseMove ( object sender , MouseEventArgs e )
        {
            //if( VSplitDown )
            //{
            //    double dbl = 0, diff=0;
            //    var conv = new GridLengthConverter ( );
            //    LeftWidth = outer0 . ActualWidth;
            //    RightWidth = outer2 . ActualWidth;
            //    Border bd = sender as Border;
            //    Point pt = e . GetPosition ( outer0 );
            //    diff = pt . X - LeftWidth - 10;
            //    LeftWidth += diff;
            //    RightWidth -= diff;

            //    Thickness th = new Thickness ( 0 , 0 , 0 , 0 );
            //    //if ( RightWidth > 350 )
            //{
            //    outer0 . Width = ( GridLength ) conv . ConvertFrom ( LeftWidth );
            //    outer2 . Width = ( GridLength ) conv . ConvertFrom ( RightWidth );
            //}
            //}
            //else
            //    Console . WriteLine ( $"missed {LeftWidth}, {RightWidth}" );

        }

        private void vsplitterbar_PreviewMouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
        {
            //VSplitDown = false;
            //ReleaseMouseCapture ( );
        }

        #region Search methods
        private void Searchbtn_Click ( object sender , RoutedEventArgs e )
        {
            string srchstring = Filtertext . Text;
            string check = DataGrid1 . SelectedItem . ToString ( );
            if ( ValidateEntries ( srchstring , check , Filtercolumn . Text ) == false )
                return;
            GenericSearch ( DataGrid1 , Convert . ToInt16 ( Filtercolumn . Text ) , srchstring );
            return;
        }
        private void Searchbtn2_Click ( object sender , RoutedEventArgs e )
        {
            string srchstring = Filtertext2 . Text;
            string check = DataGrid2 . SelectedItem . ToString ( );
            if ( ValidateEntries ( srchstring , check , Filtercolumn2 . Text ) == false )
                return;
            GenericSearch ( DataGrid2 , Convert . ToInt16 ( Filtercolumn2 . Text ) , srchstring );
            return;
        }
        private int GenericSearch ( DataGrid dgrid , int col , string srchstring )
        {
            int indx = -1;
            List<string> gc = new List<string> ( );
            Mouse . SetCursor ( Cursors . Wait );
            for ( int x = 0 ; x < dgrid . Items . Count ; x++ )
            {
                dgrid . SelectedIndex = x;
                dgrid . SelectedItem = x;
                //gvm = new GenericClass ( );
                gc . Add ( dgrid . SelectedItem . ToString ( ) );
                if ( gc == null )
                    return -1;
                if ( CheckFields ( gc [ 0 ] , col , srchstring ) )
                {
                    indx = x;
                    break;
                }
                gc . Clear ( );
            }
            dgrid . SelectedIndex = indx;
            dgrid . SelectedItem = indx;
            Utils . ScrollRecordIntoView ( dgrid , indx );
            //            Utils . SetGridRowSelectionOn ( dgrid , indx );
            //           dgrid . BringIntoView ( );
            if ( indx >= 0 )
                infopanel . Text = "Search completed successfully....";
            else
                infopanel . Text = $"Sorry, but the search of {dgrid . Items . Count} records Failed to match '{srchstring . ToUpper ( )}'...";
            Mouse . SetCursor ( Cursors . Arrow );
            return indx;
        }
        private bool CheckFields ( string gc , int col , string Srchstring )
        {
            bool res = true;
            string srch = "";
            string srchstring = Srchstring . ToUpper ( );
            if ( srchstring == "" )
            {
                fdl . FdMsg ( Flowdoc , canvas , "The search term field is empty, Search cancelled" , "" , "" );
                infopanel . Text = "The search term field is empty, Search cancelled";
                return false;
            }
            string line = gc . ToUpper ( );
            string findstr = $"FIELD{col} = ";
            int x = line . LastIndexOf ( findstr );
            if ( x == -1 )
            {
                fdl . FdMsg ( Flowdoc , canvas , $"There is no column such as '{findstr}' , Search cancelled" , "You need to enter one of the 'field'#'s shown in the column headers in this field" , "" );
                infopanel . Text = "The column index field is invalid, Search cancelled";
                return false;
            }
            string line1 = line . Substring ( x , line . Length - x );
            int y = line1 . IndexOf ( "," );
            if ( y == -1 )
                srch = line1 . Substring ( 8 , line1 . Length - 9 ) . Trim ( );
            else
                srch = line1 . Substring ( 8 , y - 8 ) . Trim ( );
            res = srch . Contains ( srchstring );
            return res;
        }
        private bool ValidateEntries ( string FilterText , string srchstring , string colstr )
        {
            if ( FilterText == "" )
            {
                fdl . FdMsg ( Flowdoc , canvas , "The column # identifier field is empty, Search cancelled" , "You need to enter the # shown in the column headers in this field" , "" );
                infopanel . Text = "The column # identifier field is empty, Search cancelled";
                return false;
            }
            string check = DataGrid2 . SelectedItem . ToString ( );
            int col = Convert . ToInt16 ( colstr );
            if ( col <= 0 || check . Contains ( $"field{col}" ) == false )
            {
                fdl . FdMsg ( Flowdoc , canvas , "The column # identifier field is invalid, Search cancelled" , $"There is no column numbered {col} in the column headers of this table" , "" );
                infopanel . Text = "The column # identifier field is invalid, Search cancelled";
                return false;
            }
            if ( srchstring == "" )
            {
                fdl . FdMsg ( Flowdoc , canvas , "The search term field is empty, Search cancelled" , "" , "" );
                return false;
            }
            return true;
        }
        #endregion Search methods

        private void ShowTableStructure_Click ( object sender , RoutedEventArgs e )
        {
            // Show table fields with nvarchar sizes in FlowDoc
            string output = "";
            List<string> list = new List<string> ( );
            List<string> fldnameslist = new List<string> ( );
            SqlCommand = $"spGetTableColumnWithSize {dbName . SelectedItem . ToString ( )}";
            fldnameslist = Datagrids . CallStoredProcedureWithSizes ( list , SqlCommand );
            output = Utils . ParseTableColumnData ( fldnameslist );

           fdl . ShowInfo ( Flowdoc , canvas , header: "Table Columns informaton accessed successfully" , clr4: "Red5" ,
           line1: $"Request made was completed succesfully!" , clr1: "Red3" ,
           line2: $"the structure of the table [{dbName . SelectedItem . ToString ( ) }] is listed below : \n{output}" ,
           line3: $"Results created by Stored Procedure : \n({SqlCommand . ToUpper ( )})" , clr3: "Blue4"
           );
        }

   
        private void LeftHorzintalSplitter_LayoutUpdated ( object sender , EventArgs e )
        {
            //upperleftpanel . UpdateLayout ( );
            //lowerleftpanel . UpdateLayout ( );

        }

        private void upperleftpanel_LayoutUpdated ( object sender , EventArgs e )
        {
            //upperleftpanel . UpdateLayout( );
            //lowerleftpanel . UpdateLayout( );
        }

        private void TopWrapper_LayoutUpdated ( object sender , EventArgs e )
        {
            //upperleftpanel . UpdateLayout ( );
            //lowerleftpanel . UpdateLayout ( );
        }

        private void ShowTableStructure2_Click ( object sender , RoutedEventArgs e )
        {
            // Show table fields with nvarchar sizes in FlowDoc
            string output = "";
            List<string> list = new List<string> ( );
            List<string> fldnameslist = new List<string> ( );
            SqlCommand = $"spGetTableColumnWithSize {dbName2 . SelectedItem . ToString ( )}";
            fldnameslist = Datagrids . CallStoredProcedureWithSizes ( list , SqlCommand );
            output = Utils . ParseTableColumnData ( fldnameslist );

            fdl . ShowInfo ( Flowdoc , canvas , header: "Table Columns informaton accessed successfully" , clr4: "Red5" ,
            line1: $"Request made was completed succesfully!" , clr1: "Red3" ,
            line2: $"the structure of the table [{dbName2 . SelectedItem . ToString ( ) }] is listed below : \n{output}" ,
            line3: $"Results created by Stored Procedure : \n({SqlCommand . ToUpper ( )})" , clr3: "Blue4"
            );
        }
    }
}
