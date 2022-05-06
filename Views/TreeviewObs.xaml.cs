﻿
#define DEBUGEXPAND
#undef DEBUGEXPAND

using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . ComponentModel;
using System . Diagnostics;
using System . IO;
using System . Linq;
using System . Threading;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Controls . Primitives;
using System . Windows . Input;
using System . Windows . Media;
using System . Windows . Media . Imaging;
using System . Windows . Threading;

using Microsoft . Xaml . Behaviors;

using MyDev . AttachedProperties;
using MyDev . Models;
using MyDev . UserControls;
using MyDev . ViewModels;

namespace MyDev . Views
{
    /// <summary>
    /// Interaction logic for TreeviewObs.xaml
    /// </summary>
    public partial class TreeviewObs : Window
    {
        #region Declarations

        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public int SLEEPTIME { get; set; } = 100;
        public bool KeepSubdirs { get; set; } = true;

        public static object [ ] Args = new object [ ] { new object ( ) , new object ( ) , new object ( ) };
        public static object TreeViewObject;
        public static bool AbortExpand = false;
        public static bool ExpandLimited = false;
        protected void OnPropertyChanged ( string PropertyName )
        {
            if ( null != PropertyChanged )
            {
                PropertyChanged ( this ,
                      new PropertyChangedEventArgs ( PropertyName ) );
            }
        }
        #endregion OnPropertyChanged

        public static int PROGRESSWRAPVALUE = 24;
        public static int MAXSEARCHLEVELS = 99;

        #region Brushes
        public SolidColorBrush YellowBrush;
        public SolidColorBrush WhiteBrush;
        public SolidColorBrush BlackBrush;
        public SolidColorBrush RedBrush;
        public SolidColorBrush Brush0;
        public SolidColorBrush Brush1;
        public SolidColorBrush Brush2;
        public SolidColorBrush Brush3;
        public SolidColorBrush Brush4;
        public SolidColorBrush Brush5;
        public SolidColorBrush Brush6;
        public SolidColorBrush Brush7;
        public SolidColorBrush Brush8;
        #endregion Brushes

        BackgroundWorker worker = null;
        public ExplorerClass TvExplorer = null;
        public TreeView ActiveTree { get; set; }
        public static TvItemClass tvitemclass { get; set; }
        public static TreeViewCollection tvcollection = new TreeViewCollection ( );
        public static ObservableCollection<TreeViewItem> TvItems = new ObservableCollection<TreeViewItem> ( );
        public TreeViewItem Mouseovertvitem { get; set; }


        //        public ICommand WalkTreeViewItem { get; set; }
        #region Expansion Items
        public struct ExpandArgs
        {
            public TreeView tv;
            public TreeViewItem tvitem;
            public TreeViewItem SearchSuccessItem;
            public int ExpandLevels;
            public int MaxItems;
            public string SearchTerm;
            public bool SearchActive;
            public int Selection;
            public bool SearchSuccess;
            public bool ListResults;
            public bool IsFullExpand;
            public TreeViewItem Parent;
        };
        public static ExpandArgs ExpArgs = new ExpandArgs ( );

        public static Dictionary<string , string> VolumeLabelsDict = new Dictionary<string , string> ( );

        #endregion Expansion Items

        public struct lbitemtemplate
        {
            public string Colm1 { get; set; }
            public string Colm2 { get; set; }
            public string Colm3 { get; set; }
            public string Colm4 { get; set; }
            public string Colm5 { get; set; }
            public string Colm6 { get; set; }
        };

        public List<String> DirOptions { get; set; }

        public List<ComboBoxItem> DirectoryOptions2 = new List<ComboBoxItem> ( );
        //        public List<Family> families = new List<Family> ( );
        public static List<string> LbStrings = new List<string> ( );
        public static List<TreeViewItem> AllCheckedFolders = new List<TreeViewItem> ( );
        public int ExpandSelection { get; set; } = -1;
        public TreeViewItem SelectedTVItem { get; set; }
        //        public bool ClosePreviousFolder { get; set; } = false;

        #region Full Properties
        private string fullDetail;
        public string FullDetail
        {
            get { return fullDetail; }
            set { fullDetail = value; OnPropertyChanged ( FullDetail ); }
        }
        private int currentTree;
        public int CurrentTree
        {
            get { return currentTree; }
            set { currentTree = value; }
        }
        private int currentLevel;
        public int CurrentLevel
        {
            get { return currentLevel; }
            set { currentLevel = value; }
        }
        private string defaultDrive;
        public string DefaultDrive
        {
            get { return defaultDrive; }
            set { defaultDrive = value; }
        }
        private TreeViews treeviewsclass;
        public TreeViews Treeviewsclass
        {
            get { return treeviewsclass; }
            set { treeviewsclass = value; }
        }
        private ExplorerClass explorer;
        public ExplorerClass Explorer
        {
            get { return explorer; }
            set { explorer = value; }
        }
        private bool exactmatch;
        public bool Exactmatch
        {
            get { return exactmatch; }
            set { exactmatch = value; OnPropertyChanged ( "Exactmatch" ); }
        }
        private bool listresults;
        public bool LISTRESULTS
        {
            get { return listresults; }
            set { listresults = value; OnPropertyChanged ( "LISTRESULTS" ); }
        }
        // Global flag to control auto closing of searched folders (only)
        private bool closePreviousNode;
        public bool ClosePreviousNode
        {
            get { return closePreviousNode; }
            set { closePreviousNode = value; OnPropertyChanged ( "ClosePreviousNode" ); }
        }
        private bool showVolumeLabels;
        public bool ShowVolumeLabels
        {
            get { return showVolumeLabels; }
            set { showVolumeLabels = value; OnPropertyChanged ( "ShowVolumeLabels " ); }
        }
        private bool showallfiles;
        public bool ShowAllFiles
        {
            get { return showallfiles; }
            set { showallfiles = value; OnPropertyChanged ( "ShowAllFiles " ); }
        }

        #endregion Full Properties

        #region General  dclarations
        public string def = ".....................................................";
        public string SearchString { get; set; } = "";
        public string ExceptionMessage { get; set; }
        public static bool BreakExpand { get; set; } = false;
        private static bool isresettingSelection { get; set; } = false;
        public int maxlevels { get; set; }
        public int TotalItemsExpanded { get; set; }
        public static lbitemtemplate lbtmp { get; set; }
        public TextBlock LbTextblock { get; set; }
        public string TextToSearchFor { set; get; } = "";
        public static TreeviewObs treeViews { get; set; }
        private TreeViewItem startitem { get; set; }

        public bool HasHidden = false;
        private bool FullExpandinProgress = false;
        public bool Loading = true;
        private static double startmin = 0;
        private static double startsec = 0;
        private static double startmsec = 0;
        //		public static LazyLoading Lazytree = null;
        public Family family1 = new Family ( );
        public DirectoryInfo DirInfo = new DirectoryInfo ( @"C:\\" );
        private static DispatcherTimer sw = new DispatcherTimer ( );

        #endregion General declarations

        private static FlowdocLib fdl;
        #endregion Declarations

        #region startup  items

        public TreeviewObs ( )
        {
            int count = 0;
            InitializeComponent ( );
            ReadSettings ( );
            tvitemclass = new TvItemClass ( );
            TvItems = TreeViewCollection . tvitems;
            this . DataContext = tvitemclass;
            // Get ObsCollection
            // Cannot use  this with FlowDoc cos of dragging/Resizing
            //Utils . SetupWindowDrag ( this );
            ActiveTree = TestTree;
            OptionsPanel . Visibility = Visibility . Hidden;
            treeViews = this;
            listBox . Items . Clear ( );
            ActiveTree . Items . Clear ( );
            fdl = new FlowdocLib ( );
            FdMargin . Left = Flowdoc . Margin . Left;
            FdMargin . Top = Flowdoc . Margin . Top;
            TvExplorer = new ExplorerClass ( );
            TreeViewItem tvi = new TreeViewItem ( );
            tvi . Header = @"C:\";
            BusyLabelColor = FindResource ( "Yellow0" ) as SolidColorBrush;
            BusyLabelBkgrn = FindResource ( "Black0" ) as SolidColorBrush;

            YellowBrush = FindResource ( "Yellow0" ) as SolidColorBrush;
            BlackBrush = FindResource ( "Black0" ) as SolidColorBrush;
            RedBrush = FindResource ( "Red5" ) as SolidColorBrush;
            WhiteBrush = FindResource ( "White2" ) as SolidColorBrush;
            LoadSearchLevels ( );
            // Set Horizontal Splitter FULLY DOWN at startup
            TopGrid . RowDefinitions [ 2 ] . Height = new GridLength ( 0 , GridUnitType . Pixel );
            Col3 . Width = new GridLength ( 350 , GridUnitType . Pixel );
            testtreebanner . Text = "Manual Directories System, TestTree";

        }
        private void Window_Loaded ( object sender , RoutedEventArgs e )
        {
            string output = "";
            this . SetValue ( FontsizeProperty , InfoList . FontSize );
            canvas . Visibility = Visibility . Visible;
            CreateBrushes ( );
            VolumeLabelsDict . Clear ( );
            //tvitemclass . LoadDrives ( "ALL" );
            TestTree . ItemsSource = TvItems;
            ExpArgs . SearchSuccessItem = new TreeViewItem ( );
            Flowdoc . ExecuteFlowDocMaxmizeMethod += new EventHandler ( MaximizeFlowDoc );
            Flowdoc . HandleKeyEvents += new KeyEventHandler ( Flowdoc_HandleKeyEvents );
            List<String> errors = new List<string> ( );
            //            LazyLoadingTreeview . LazyLoadTreeview ( treeViewModel , this , ref errors );
            if ( errors . Count > 0 )
            {
                foreach ( var item in errors )
                {

                    listBox . Items . Add ( "<-- " + item );
                    output += item + "\n";
                }
            }
            //Grid1 . RowDefinitions [ 1 ] . Height = new GridLength ( 3 , GridUnitType . Star );
            //Grid1 . RowDefinitions [ 3 ] . Height = new GridLength ( 155 , GridUnitType . Pixel );
            // orow2 . Height = new GridLength ( 0 , GridUnitType . Pixel ); 
            LsplitterImage = new BitmapImage ( new Uri ( @"\icons\Lrg updown arrow red copy.png" , UriKind . Relative ) );
            VsplitterImage = new BitmapImage ( new Uri ( @"\icons\Lrg ltrt arrow red copy.png" , UriKind . Relative ) );
            ShowDriveInfo ( sender , e );
            loadExpandOptions ( );
            ExpandSetup ( false );
            DrivesCombo . SelectedIndex = 0;
            maxlevels = 99;
            Loading = false;
            if ( LISTRESULTS )
                CurrentFolder . Text = "Information / Log  Panel : ENABLED";
            else
                CurrentFolder . Text = "Information / Log  Panel : DISABLED";
            Duration . Text = "";
            ProgressCount = 0;
            Expandcounter . Text = "";
            this . DataContext = this;
            var newsel = TestTree . FindName ( $"C:\\" );

            if ( newsel != null )
                TestTree . SetProperty ( SelectedItemProperty );
            else
            {
                foreach ( var item in TvItems )
                {
                    if ( item . Tag . ToString ( ) == $"C:\\" )
                    {
                        item . IsSelected = true;
                        break;
                    }
                }
            }
            //fdl . ShowInfo ( Flowdoc, canvas, "This Version is here to demonstrate the use of a Templated style that handles all coloring", "Black0", "Information Idiot !!", "Red5");
        }
        private void TREEViews_Closing ( object sender , CancelEventArgs e )
        {
            Flowdoc . ExecuteFlowDocMaxmizeMethod -= new EventHandler ( MaximizeFlowDoc );
            Flowdoc . HandleKeyEvents -= new KeyEventHandler ( Flowdoc_HandleKeyEvents );
            SaveSettings ( );
        }
        private void ReadSettings ( )
        {
            string output = "";
            output = File . ReadAllText ( @"TreeViewSettings.dat" );
            string [ ] input = output . Split ( ',' );
            for ( int x = 0 ; x < input . Length ; x++ )
            {
                switch ( x )
                {
                    case 0:
                        Exactmatch = input [ x ] == "T" ? true : false;
                        break;
                    case 1:
                        LISTRESULTS = input [ x ] == "T" ? true : false;
                        break;
                    case 2:
                        ClosePreviousNode = input [ x ] == "T" ? true : false;
                        break;
                    case 3:
                        ShowVolumeLabels = input [ x ] == "T" ? true : false;
                        break;
                    case 4:
                        ShowAllFiles = input [ x ] == "T" ? true : false;
                        break;
                }
            }
        }
        private void SaveSettings ( )
        {
            string output = "";
            output = Exactmatch ? "T," : "F,";
            output += LISTRESULTS ? "T," : "F,";
            output += ClosePreviousNode ? "T," : "F,";
            output += ShowVolumeLabels ? "T," : "F,";
            output += ShowAllFiles ? "T," : "F,";
            output += "\n";
            File . WriteAllText ( @"TreeViewSettings.dat" , output );
        }

        #endregion startup    items     

        #region close dwn
        private void App_Close ( object sender , RoutedEventArgs e )
        {
            sw . Stop ( );
            this . Close ( );
            Application . Current . Shutdown ( );
        }

        private void Close_Btn ( object sender , RoutedEventArgs e )
        {
            sw . Stop ( );
            this . Close ( );
        }
        #endregion close dwn

        #region Initialization methods

        private void TestViewModel ( object sender , RoutedEventArgs e )
        {
            List<string> dirs = new List<string> ( );
            List<string> files = new List<string> ( );
            string selecteddrive = DrivesCombo . SelectedItem . ToString ( );
            ActiveTree . ItemsSource = tvcollection . LoadDrives ( selecteddrive );
        }

        //private void LoadDrives ( TreeView tv , string drivetoload = "" )
        //{
        //    bool ValidDrive = false;
        //    //            bool HasHiddenItems = false;
        //    string volabel = "";
        //    string DriveHeader = "";
        //    string Padding = "                 ";
        //    bool isvalid = false;
        //    tv . Items . Clear ( );
        //    //            listBox . Items . Clear ( );
        //    listBox . UpdateLayout ( );
        //    DrivesCombo . Items . Add ( "ALL" );
        //    VolumeLabelsDict . Clear ( );
        //    tvitemclass . LoadValidFiles ( );
        //    if ( drivetoload == "ALL" )
        //        drivetoload = "";
        //    foreach ( var drive in Directory . GetLogicalDrives ( ) )
        //    {
        //        ValidDrive = false;
        //        DriveHeader = "";
        //        if ( drivetoload . ToUpper ( ) != "" )
        //        {
        //            if ( drive . ToUpper ( ) != drivetoload . ToUpper ( ) )
        //                continue;
        //        }
        //        //Add Drive to Treeview
        //        DriveInfo [ ] di = DriveInfo . GetDrives ( );
        //        foreach ( var item in di )
        //        {
        //            if ( item . Name == drive )
        //            {
        //                if ( item . DriveType == DriveType . CDRom )
        //                {
        //                    ValidDrive = false;
        //                    DriveHeader = Padding . Substring ( 0 , 10 );
        //                    DriveHeader += "CdRom Drive";
        //                    string newlabel = " " + DriveHeader;
        //                    VolumeLabelsDict . Add ( drive , newlabel );
        //                }
        //                else
        //                {
        //                    List<string> directories = new List<string> ( );
        //                    tvitemclass . GetDirectories ( item . ToString ( ) , out directories );
        //                    foreach ( var dir in directories )
        //                    {
        //                        if ( TreeViewCollection. CheckIsVisible ( dir . ToUpper ( ) , ShowAllFiles , out HasHidden ) == true )
        //                        {
        //                            isvalid = true;
        //                            string newlabel = " " + item . VolumeLabel;
        //                            VolumeLabelsDict . Add ( drive , newlabel );
        //                            if ( ShowVolumeLabels == true )
        //                            {
        //                                DriveHeader = $"    [{newlabel}]";
        //                            }
        //                            break;
        //                        }
        //                    }
        //                    if ( isvalid )
        //                    {
        //                        if ( ShowVolumeLabels == true )
        //                            DriveHeader = $"   [{item . VolumeLabel}]";
        //                        ValidDrive = true;
        //                    }
        //                    else
        //                        volabel = $"    [{item . VolumeLabel}]";
        //                }
        //                break;
        //            }
        //        }
        //        if ( ValidDrive == true )
        //        {
        //            var item = new TreeViewItem ( );
        //            item . Header = drive + DriveHeader;
        //            item . Tag = drive;
        //            //                   tv . Items . Add ( item );
        //            // Add Dummy entry so we get an "Can be Opened" triangle icon
        //            item . Items . Add ( "Loading" );
        //            DrivesCombo . Items . Add ( drive . ToString ( ) );
        //            tvitems . Add ( item );
        //        }
        //        else
        //        {
        //            var item = new TreeViewItem ( );
        //            if ( ShowVolumeLabels == true )
        //                item . Header = drive + volabel;
        //            else
        //                item . Header = drive + DriveHeader;
        //            item . Tag = drive;
        //            //                    tv . Items . Add ( item );
        //            DrivesCombo . Items . Add ( drive . ToString ( ) );
        //            tvitems . Add ( item );
        //        }
        //    }
        //    DrivesCombo . Items . Add ( "ALL" );
        //    DrivesCombo . SelectedIndex = 0;
        //    DrivesCombo . SelectedItem = 0;
        //    //           tv . UpdateLayout ( );
        //}

        // Stored list of all Hidden/System file names so we can handle not showing the,

        #endregion Initialization methods
        private void Window_KeyDown ( object sender , KeyEventArgs e )
        {
            if ( e . Key == Key . F8 )
                Debugger . Break ( );
        }
        private void ShowallFiles_Click ( object sender , RoutedEventArgs e )
        {
            CheckBox cb = sender as CheckBox;
            if ( cb . IsChecked == true )
                ShowAllFiles = true;
            else
                ShowAllFiles = false;
            ActiveTree . ItemsSource = tvcollection . LoadDrives ( "" );
        }

        #region utilities
        private static T FindAnchestor<T> ( DependencyObject current )
         where T : DependencyObject
        {
            do
            {
                if ( current is T )
                {
                    return ( T ) current;
                }
                current = VisualTreeHelper . GetParent ( current );
            }
            while ( current != null );
            return null;
        }
        #endregion utilities

        #region FULL (get; set; )  Properties


        private string expandDuration;
        public string ExpandDuration
        {
            get { return expandDuration; }
            set { expandDuration = value; OnPropertyChanged ( "ExpandDuration" ); }
        }

        private int progressCount;
        public int ProgressCount
        {
            get { return progressCount; }
            set
            {
                if ( value != 0 && value % PROGRESSWRAPVALUE == 0 )
                {
                    if ( BusyLabel . Visibility == Visibility . Hidden )
                        BusyLabel . Visibility = Visibility . Visible;
                    progressCount = 0;
                    ProgressString = ".";
                    if ( BusyLabelColor == RedBrush )
                    {
                        BusyLabelColor = YellowBrush;
                        BusyLabelBkgrn = BlackBrush;
                    }
                    else
                    {
                        BusyLabelColor = RedBrush;
                        BusyLabelBkgrn = WhiteBrush;
                    }

                }
                else
                {
                    progressCount = value;
                    ProgressString = def . Substring ( 0 , value );
                    OnPropertyChanged ( ProgressCount . ToString ( ) );
                }
            }
        }
        private string progressString;
        public string ProgressString
        {
            get { return progressString; }
            set
            {
                progressString = value;
                OnPropertyChanged ( ProgressString );
            }
        }

        private int listboxtotal;
        public int Listboxtotal
        {
            get { return listboxtotal; }
            set { listboxtotal = value; OnPropertyChanged ( Listboxtotal . ToString ( ) ); }
        }

        #endregion FULL (get; set; )  Properties

        #region Dependency Properties

        private SolidColorBrush busyLabelColor;
        public SolidColorBrush BusyLabelColor
        {
            get { return busyLabelColor; }
            set { busyLabelColor = value; OnPropertyChanged ( BusyLabelColor . ToString ( ) ); }
        }

        private SolidColorBrush busyLabelBkgrn;
        public SolidColorBrush BusyLabelBkgrn
        {
            get { return busyLabelBkgrn; }
            set { busyLabelBkgrn = value; OnPropertyChanged ( BusyLabelBkgrn . ToString ( ) ); }
        }

        public bool tv1SelectedItem
        {
            get { return ( bool ) GetValue ( tv1SelectedItemProperty ); }
            set { SetValue ( tv1SelectedItemProperty , value ); }
        }
        public static readonly DependencyProperty tv1SelectedItemProperty =
            DependencyProperty . Register ( "tv1SelectedItem" , typeof ( bool ) , typeof ( TreeviewObs ) , new PropertyMetadata ( false ) );
        public bool tv2SelectedItem
        {
            get { return ( bool ) GetValue ( tv2SelectedItemProperty ); }
            set { SetValue ( tv2SelectedItemProperty , value ); }
        }
        public static readonly DependencyProperty tv2SelectedItemProperty =
            DependencyProperty . Register ( "tv2SelectedItem" , typeof ( bool ) , typeof ( TreeviewObs ) , new PropertyMetadata ( false ) );
        public bool tv3SelectedItem
        {
            get { return ( bool ) GetValue ( tv3SelectedItemProperty ); }
            set { SetValue ( tv3SelectedItemProperty , value ); }
        }
        public static readonly DependencyProperty tv3SelectedItemProperty =
            DependencyProperty . Register ( "tv3SelectedItem" , typeof ( bool ) , typeof ( TreeviewObs ) , new PropertyMetadata ( false ) );
        public TreeViewItem tv4SelectedItem
        {
            get { return ( TreeViewItem ) GetValue ( tv4SelectedItemProperty ); }
            set { SetValue ( tv4SelectedItemProperty , value ); }
        }
        public static readonly DependencyProperty tv4SelectedItemProperty =
            DependencyProperty . Register ( "tv4SelectedItem" , typeof ( TreeViewItem ) , typeof ( TreeviewObs ) , new PropertyMetadata ( ( TreeViewItem ) null ) );
        public TreeViewItem SelectedItem
        {
            get { return ( TreeViewItem ) GetValue ( SelectedItemProperty ); }
            set { SetValue ( SelectedItemProperty , value ); }
        }
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty . Register ( "SelectedItem" , typeof ( TreeViewItem ) , typeof ( TreeviewObs ) , new PropertyMetadata ( ( TreeViewItem ) null ) );
        public double Fontsize
        {
            get { return ( double ) GetValue ( FontsizeProperty ); }
            set { SetValue ( FontsizeProperty , value ); }
        }
        public static readonly DependencyProperty FontsizeProperty =
            DependencyProperty . Register ( "Fontsize" , typeof ( double ) , typeof ( TreeviewObs ) , new PropertyMetadata ( ( double ) 12 ) );
        public BitmapImage LsplitterImage
        {
            get
            { return ( BitmapImage ) GetValue ( LsplitterImageProperty ); }
            set { SetValue ( LsplitterImageProperty , value ); }
        }
        public static readonly DependencyProperty LsplitterImageProperty =
            DependencyProperty . Register ( "LsplitterImage" , typeof ( BitmapImage ) , typeof ( TreeviewObs ) , new PropertyMetadata ( ( BitmapImage ) null ) );
        public BitmapImage VsplitterImage
        {
            get
            { return ( BitmapImage ) GetValue ( VsplitterImageProperty ); }
            set { SetValue ( VsplitterImageProperty , value ); }
        }
        public static readonly DependencyProperty VsplitterImageProperty =
            DependencyProperty . Register ( "VsplitterImage" , typeof ( BitmapImage ) , typeof ( TreeviewObs ) , new PropertyMetadata ( ( BitmapImage ) null ) );
        public string LeftSplitterText
        {
            get { return ( string ) GetValue ( LeftSplitterTextProperty ); }
            set { SetValue ( LeftSplitterTextProperty , value ); }
        }
        // Using a DependencyProperty as the backing store for LeftSplitterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftSplitterTextProperty =
            DependencyProperty . Register ( "LeftSplitterText" , typeof ( string ) , typeof ( TreeviewObs ) , new PropertyMetadata ( ( string ) "Drag Up or Down" ) );
        public string RightSplitterText
        {
            get { return ( string ) GetValue ( RightSplitterTextProperty ); }
            set { SetValue ( RightSplitterTextProperty , value ); }
        }
        // Using a DependencyProperty as the backing store for LeftSplitterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightSplitterTextProperty =
            DependencyProperty . Register ( "RightSplitterText" , typeof ( string ) , typeof ( TreeviewObs ) , new PropertyMetadata ( ( string ) "to View Directory Tree / Drive Technical Information." ) );


        public SolidColorBrush LbTextColor
        {
            get { return ( SolidColorBrush ) GetValue ( LbTextColorProperty ); }
            set { SetValue ( LbTextColorProperty , value ); }
        }
        public static readonly DependencyProperty LbTextColorProperty =
            DependencyProperty . Register ( "LbTextColor" , typeof ( SolidColorBrush ) , typeof ( TreeviewObs ) ,
                new PropertyMetadata ( new SolidColorBrush ( Colors . Black ) ) );

        #endregion Dependency Properties

        #region Attached Properties

        public static bool Gettvselection ( DependencyObject obj )
        {
            return ( bool ) obj . GetValue ( tvselectionProperty );
        }
        public static void Settvselection ( DependencyObject obj , bool value )
        {
            obj . SetValue ( tvselectionProperty , value );
        }
        public static readonly DependencyProperty tvselectionProperty =
            DependencyProperty . RegisterAttached ( "tvselection" , typeof ( bool ) , typeof ( TreeviewObs ) , new PropertyMetadata ( ( bool ) false ) );

        public static bool GetIsMouseDirectlyOverItem ( DependencyObject obj )
        {
            return ( bool ) obj . GetValue ( IsMouseDirectlyOverItemProperty );
        }
        public static void SetIsMouseDirectlyOverItem ( DependencyObject obj , bool value )
        {
            obj . SetValue ( IsMouseDirectlyOverItemProperty , value );
        }
        public static readonly DependencyProperty IsMouseDirectlyOverItemProperty =
            DependencyProperty . RegisterAttached ( "IsMouseDirectlyOverItem" , typeof ( bool ) , typeof ( TreeviewObs ) , new PropertyMetadata ( ( bool ) false ) );

        #endregion Attached Properties

        #region Flowdoc support via library
        /// <summary>
        /// These methods are needed to allow FLowdoc  to work via FlowDocLib
        ///  We also Need to declare an object :
        ///  object MovingObject ;
        ///  in the heade area just worksj
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        // Variables Required for FlowDoc
        object MovingObject;
        private static double fdTop = 100;
        private static double fdLeft = 100;
        private static Thickness FdMargin = new Thickness ( );

        /*  
		 *  Add these  to the FlowDoc in XAML
  				PreviewMouseLeftButtonDown="Flowdoc_PreviewMouseLeftButtonDown"
				MouseLeftButtonUp="Flowdoc_MouseLeftButtonUp"
				MouseMove= "Flowdoc_MouseMove"
				LostFocus="Flowdoc_LostFocus"
*/

        // Add this startup :-			Flowdoc . ExecuteFlowDocMaxmizeMethod += new EventHandler ( MaximizeFlowDoc );
        // & of course  on closing :-	Flowdoc . ExecuteFlowDocMaxmizeMethod -= new EventHandler ( MaximizeFlowDoc );


        protected void MaximizeFlowDoc ( object sender , EventArgs e )
        {
            // Clever "Hook" method that Allows the flowdoc to be resized to fill window
            // or return to its original size and position courtesy of the Event declard in FlowDoc
            //Need to ensure the wrapping canvas is sized to its containing element (Wiindow outer Grid in this case)
            canvas . Height = Grid1 . ActualHeight;
            canvas . Width = Grid1 . ActualWidth;
            fdl . MaximizeFlowDoc ( Flowdoc , canvas , e );
        }
        // CALLED WHEN  LEFT BUTTON PRESSED
        private void Flowdoc_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
        {
            //In this event, we get current mouse position on the control to use it in the MouseMove event.
            MovingObject = fdl . Flowdoc_PreviewMouseLeftButtonDown ( sender , Flowdoc , e );
            Mouse . OverrideCursor = Cursors . Arrow;
            // NB Flowdoc remebers its last position automatically
        }
        private void Flowdoc_MouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
        {
            // Window wide  !!
            // Called  when a Flowdoc MOVE has ended
            MovingObject = fdl . Flowdoc_MouseLeftButtonUp ( sender , Flowdoc , MovingObject , e );
            ReleaseMouseCapture ( );
            Mouse . OverrideCursor = Cursors . Arrow;
        }
        private void Flowdoc_MouseMove ( object sender , MouseEventArgs e )
        {
            FlowDoc fd = sender as FlowDoc;
            // We are Resizing the Flowdoc using the mouse on the border  (Border.Name=FdBorder)
            fdl . Flowdoc_MouseMove ( Flowdoc , canvas , MovingObject , e );
            fd . Focus ( );
        }
        // Shortened version proxy call		
        private void Flowdoc_LostFocus ( object sender , RoutedEventArgs e )
        {
            Flowdoc . BorderClicked = false;
            Mouse . OverrideCursor = Cursors . Arrow;
        }
        public void Flowdoc_HandleKeyEvents ( object sender , KeyEventArgs e )
        {
            if ( e . Key == Key . Escape )
                this . Visibility = Visibility . Hidden;
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
            Mouse . OverrideCursor = Cursors . Arrow;
        }


        #endregion Flowdoc support via library

        private void ShowDriveInfo ( object sender , RoutedEventArgs e )
        {
            string output = "";
            ExplorerClass Texplorer = new ExplorerClass ( );
            Texplorer . GetDrives ( "C:\\" );
            List<lbitemtemplate> lbtmplates = new List<lbitemtemplate> ( );
            InfoList . ItemsSource = null;
            InfoList . Items . Clear ( );

            // Create list of drive info in individual Templates & add to array of templates
            for ( int x = 0 ; x < Texplorer . Drives . Count ; x++ )
            {
                DriveInfo [ ] driveinfo = DriveInfo . GetDrives ( );
                List<string> drives = Texplorer . GetDrives ( Texplorer . Drives [ x ] );
                Texplorer . GetDirectories ( drives [ x ] );
                Texplorer . GetFiles ( drives [ x ] );
                lbitemtemplate lbtmp = new lbitemtemplate ( );
                if ( driveinfo [ x ] . IsReady == true )
                {
                    output += $"Drive [{Texplorer . Name}, Volume Label = {driveinfo [ x ] . VolumeLabel}, Type = {driveinfo [ x ] . DriveType}, Format = {driveinfo [ x ] . DriveFormat}, " +
                        $"Directories = {Texplorer . Directories . Count}, Files = {Texplorer . Files . Count}\n";
                    lbtmp . Colm1 = $"Drive [{Texplorer . Name}]";
                    lbtmp . Colm2 = $"Drive Type = [{driveinfo [ x ] . DriveType}]";
                    lbtmp . Colm3 = $"Volume label = [{driveinfo [ x ] . VolumeLabel}]";
                    lbtmp . Colm4 = $"Format = [{driveinfo [ x ] . DriveFormat}]";
                    lbtmp . Colm5 = $"Directories = [{Texplorer . Directories . Count}]";
                    lbtmp . Colm6 = $"Files[{Texplorer . Files . Count}]";
                }
                else
                {
                    output += $"NOT READY : Drive [{Texplorer . Name}, Type = {driveinfo [ x ] . DriveType}\n";
                    lbtmp . Colm1 = $"Drive [{Texplorer . Name} ";
                    lbtmp . Colm2 = $"Drive Type = {driveinfo [ x ] . DriveType}";
                    lbtmp . Colm3 = $" DRIVE NOT READY!!";
                }
                lbtmplates . Add ( lbtmp );
                continue;

            }
            InfoList . ItemsSource = null;
            InfoList . ItemsSource = lbtmplates;
        }

        #region Treeview lower level  support methods

        #endregion Treeview support methods

        #region Splitter handlers
        private void LeftSplitter_DragStarted ( object sender , System . Windows . Controls . Primitives . DragStartedEventArgs e )
        {
            if ( row1 . ActualHeight <= 10 )
            {
                LeftSplitterText = "Drag Down";
                LsplitterImage = new BitmapImage ( new Uri ( @"\icons\down arroiw red.png" , UriKind . Relative ) );
                RightSplitterText = "to View Tree Directory  Information.";
            }
            else if ( row2 . ActualHeight <= 10 )
            {
                LeftSplitterText = "Drag Up";
                LsplitterImage = new BitmapImage ( new Uri ( @"\icons\up arroiw red.png" , UriKind . Relative ) );
                RightSplitterText = "to view detailed Drive technical Information.";
            }
            else
            {
                LeftSplitterText = "Drag Up or Down";
                LsplitterImage = new BitmapImage ( new Uri ( @"\icons\Lrg updown arrow red copy.png" , UriKind . Relative ) );
                RightSplitterText = "to view Directory Tree / Drive technical Information.";
            }
            //vsplitterleft. Cursor = Cursors . SizeWE;
        }

        private void LeftSplitter_DragCompleted ( object sender , System . Windows . Controls . Primitives . DragCompletedEventArgs e )
        {
            //double totalheight = ( row1 . ActualHeight + orow2 . ActualHeight ) - 20;
            //double maxheight = Grid1 . ActualHeight - btngrid . ActualHeight;
            //double topheight = row1 . ActualHeight - 10;
            //double lowerheight = row2 . ActualHeight - 10;
            //     innergrid . Height = row1.ActualHeight;
            //     InfoList . UpdateLayout ( );
            //            Console . WriteLine ($"treeView4.ActualHeight = {treeView4 . ActualHeight }, row1.AH = {row1.ActualHeight}" );
            if ( row1 . ActualHeight <= 10 )
            {
                LeftSplitterText = "Drag Down";
                LsplitterImage = new BitmapImage ( new Uri ( @"\icons\down arroiw red.png" , UriKind . Relative ) );
                RightSplitterText = "to view Tree Directory information.";
            }
            else if ( row2 . ActualHeight <= 10 )
            {
                LeftSplitterText = "Drag Up";
                LsplitterImage = new BitmapImage ( new Uri ( @"\icons\up arroiw red.png" , UriKind . Relative ) );
                RightSplitterText = "to view detailed Drive technical Information.";
            }
            else
            {
                LeftSplitterText = "Drag Up or Down";
                LsplitterImage = new BitmapImage ( new Uri ( @"\icons\Lrg updown arrow red copy.png" , UriKind . Relative ) );
                RightSplitterText = "to view Directory Tree / Drive technical Information.";
            }
            vsplitterright . Cursor = Cursors . SizeWE;
        }
        private void Hsplitter_DragOver ( object sender , DragEventArgs e )
        {
            //double totalheight = ( row1 . ActualHeight + orow2 . ActualHeight ) - 20;
            //double maxheight = Grid1 . ActualHeight - btngrid . ActualHeight;
            //double topheight = row1 . ActualHeight - 10;
            //double lowerheight = row2 . ActualHeight - 10;
            ////innergrid . Height = row1 . ActualHeight;
            //InfoList . UpdateLayout ( );
            hsplitter . Cursor = Cursors . SizeNS;
        }
        private void hsplitter_DragDelta ( object sender , System . Windows . Controls . Primitives . DragDeltaEventArgs e )
        {
            //double totalheight = ( row1 . ActualHeight + orow2 . ActualHeight ) - 20;
            //double maxheight = Grid1 . ActualHeight - btngrid . ActualHeight;
            //double topheight = row1 . ActualHeight - 10;
            //double lowerheight = row2 . ActualHeight - 10;
            ////            innergrid . Height = row1 . ActualHeight;
            //            InfoList . UpdateLayout ( );
            hsplitter . Cursor = Cursors . SizeNS;

        }
        private void VRightSplitter_DragStarted ( object sender , System . Windows . Controls . Primitives . DragStartedEventArgs e )
        {
            vsplitterright . Cursor = Cursors . SizeWE;

        }

        private void VRightSplitter_DragCompleted ( object sender , System . Windows . Controls . Primitives . DragCompletedEventArgs e )
        {
            vsplitterright . Cursor = Cursors . SizeWE;
            //if(e.Canceled)
            //    e . Handled = true;

        }

        private void VRightSplitter_DragOver ( object sender , DragEventArgs e )
        {
            vsplitterright . Cursor = Cursors . SizeWE;

        }

        private void vRightSplitter_DragDelta ( object sender , System . Windows . Controls . Primitives . DragDeltaEventArgs e )
        {
            vsplitterright . Cursor = Cursors . SizeWE;
            //Console . WriteLine ($"{ActiveTree .ActualWidth }" );
            //if ( ActiveTree .ActualWidth < 155 )
            //{
            //    DragCompletedEventArgs dce = new DragCompletedEventArgs ( 0 , 0 , true );
            //    VRightSplitter_DragCompleted ( sender , dce );
            //    e . Handled = true;
            //}
        }

        #endregion Splitter handlers

        #region TestTree Expanding Handling methods

        //        int iterations = 0;
        //        public Task t1;
        //        public DispatcherOperation operation;
        private bool ExpandAll3 ( TreeViewItem items , bool expand , int levels )
        {
            if ( items == null )
                return false;

            //          levels = ExpArgs . ExpandLevels;
            foreach ( object obj in items . Items )
            {
                if ( AbortExpand )
                    return false;
                //iterations++;
                ShowProgress ( );
                TreeViewItem childControl = obj as TreeViewItem;
                if ( childControl != null )
                {
                    UpdateExpandprogress ( );
                    if ( BreakExpand )
                        break;
                    try
                    {
                        childControl . IsExpanded = true;
                    }
                    catch ( Exception ex ) { Console . WriteLine ( $"ExpandAll3: 1427 : {ex . Message}" ); }
                    if ( childControl . Header . ToString ( ) == "Loading" )
                        continue;

                    if ( TreeViewCollection . CheckIsVisible ( childControl . Header . ToString ( ) . ToUpper ( ) , ShowAllFiles , out HasHidden ) == false )
                    {
                        continue;
                    }
                    if ( CheckSearchSuccess ( childControl . Tag . ToString ( ) ) == true )
                    {
                        UpdateListBox ( $"\nSearch for {Searchtext . Text} found  as [" + childControl . Header . ToString ( ) + $"]\nin {childControl . Tag . ToString ( )}" );
                        //ActiveTree . HorizontalAlignment = HorizontalAlignment . Left;
                        ScrollCurrentTvItemIntoView ( childControl );
                        ExpArgs . SearchSuccessItem = childControl;
                        ExpArgs . SearchSuccess = true;
                        childControl . IsSelected = true;
                        fdl . ShowInfo ( Flowdoc , canvas , "Match found !" );
                        return true;
                    }

                    ShowProgress ( );
                    ShowExpandTime ( );
                    if ( CalculateLevel ( childControl . Tag . ToString ( ) ) > levels )
                        break;

                    if ( FullExpandinProgress == false )
                        ActiveTree . Refresh ( );
                    if ( childControl . Items . Count > 1 )
                    {
                        UpdateListBox ( childControl . Tag . ToString ( ) );
                        TreeViewItem tmp = childControl . Items [ 0 ] as TreeViewItem;
                        if ( tmp . ToString ( ) != "Loading" )
                        {
                            if ( levels == 1 )
                            {
                                ShowProgress ( );
                                Selection . Text = $"Calling ExpandFolder for {childControl . Tag . ToString ( )}";
                                //                                Console . WriteLine ( Selection . Text );
                                if ( ExpandFolder ( childControl , true ) == true ) // Expand ALL Contents (true)
                                {
                                    //ActiveTree . HorizontalAlignment = HorizontalAlignment . Left;
                                    ScrollCurrentTvItemIntoView ( childControl );
                                    childControl . IsSelected = true;
                                    ExpArgs . SearchSuccess = true;
                                    return true;
                                }
                                ShowProgress ( );
                                if ( FullExpandinProgress == false )
                                    ActiveTree . Refresh ( );
                            }
                            else
                            {
                                ShowProgress ( );
                                Selection . Text = $"Calling ExpandAll3 for {childControl . Tag . ToString ( )}";
                                UpdateListBox ( childControl . Tag . ToString ( ) );
                                ShowExpandTime ( );
                                if ( ExpandAll3 ( childControl as TreeViewItem , expand , levels ) == true )
                                {
                                    //ActiveTree . HorizontalContentAlignment = HorizontalAlignment . Left;
                                    //                                    ScrollCurrentTvItemIntoView ( childControl );
                                    childControl . IsSelected = true;
                                    //SearchSuccess = true;
                                    ExpArgs . SearchSuccess = true;
                                    return true;
                                }
                                ShowProgress ( );
                                ShowExpandTime ( );
                                if ( FullExpandinProgress == false )
                                    ActiveTree . Refresh ( );
                            }
                        }
                        else
                        {
                            UpdateListBox ( childControl . Tag . ToString ( ) );
                            ShowProgress ( );
                            try
                            {
                                childControl . IsExpanded = true;
                                //stack . Push ( childControl . Tag . ToString ( ) );
                            }
                            catch ( Exception ex ) { Console . WriteLine ( $"ExpandAll3: 1503 : {ex . Message}" ); }
                            ShowProgress ( );
                            if ( FullExpandinProgress == false )
                                ActiveTree . Refresh ( );
                        }
                    }
                    else
                    {
                        ShowProgress ( );
                        try
                        {
                            childControl . IsExpanded = true;
                            //stack . Push ( childControl . Tag . ToString ( ) );
                        }
                        catch ( Exception ex ) { Console . WriteLine ( $"ExpandAll3: 1517 : {ex . Message}" ); }
                    }
                    ShowProgress ( );
                    if ( FullExpandinProgress == false )
                        ActiveTree . Refresh ( );
                    ShowExpandTime ( );
                }
                ShowProgress ( );
                if ( FullExpandinProgress == false )
                    ActiveTree . Refresh ( );
            }
            ShowProgress ( );
            ShowExpandTime ( );
            if ( FullExpandinProgress == false )
                ActiveTree . Refresh ( );
            return false;
        }
        private void ExpandAllDrivesBelowCurrent ( object [ ] Args )
        {
            // WORKING for TWO levels onlly
            bool go = false;
            int levels = ( int ) Args [ 1 ];
            TreeViewItem tv = Args [ 0 ] as TreeViewItem;
            if ( tv == null )
                return;
            List<TreeViewItem> allsubsequent = new List<TreeViewItem> ( );
            List<string> list = new List<string> ( );
            string [ ] drives = Directory . GetLogicalDrives ( );
            bool islaterdrive = false;
            //            ProgressCount = 0;
            ProgressString = "";
            foreach ( var validdrive in drives )
            {
                if ( islaterdrive == false && validdrive != tv . Header . ToString ( ) )
                {
                    ShowProgress ( );
                    continue;
                }
                islaterdrive = true;
                ShowProgress ( );
                ShowExpandTime ( );
                foreach ( TreeViewItem nextdrive in ActiveTree . Items )
                {
                    nextdrive . IsExpanded = true;
                    //stack . Push ( nextdrive . Tag . ToString ( ) );
                    if ( nextdrive . Tag . ToString ( ) == validdrive )
                    {
                        //nextdrive . IsExpanded = true;
                        //stack . Push ( nextdrive.Tag.ToString() );
                        ShowProgress ( );
                        foreach ( TreeViewItem item2 in nextdrive . Items )
                        {
                            item2 . IsExpanded = true;
                            //stack . Push ( item2 . Tag . ToString ( ) );
                            ShowProgress ( );
                            item2 . UpdateLayout ( );
                            ActiveTree . Refresh ( );
                            if ( item2 . Items . Count > 0 )
                            {
                                foreach ( TreeViewItem item3 in item2 . Items )
                                {
                                    item3 . IsExpanded = true;
                                    //stack . Push ( item3 . Tag . ToString ( ) );

                                    //item3 . UpdateLayout ( );
                                    ShowProgress ( );
                                    ActiveTree . Refresh ( );
                                }
                            }
                        }
                        ActiveTree . Refresh ( );
                        ShowExpandTime ( );
                    }
                }
            }
            ActiveTree . Refresh ( );
            //            ProgressCount = 0;
            ProgressString = "Done ...";
            ShowExpandTime ( );
            Mouse . OverrideCursor = Cursors . Arrow;
        }

        // ALL WORKING  REASONABLY CORRECTLY IT APPEARS 14/4/22

        public void TriggerExpand0 ( object sender , RoutedEventArgs e )
        {
            if ( ActiveTree . SelectedItem == null )
            {
                MessageBox . Show ( $"Please select a drive or subfolder before using  these options...." , "No Drive Selected" );
                return;
            }
            object [ ] Args = { ActiveTree . SelectedItem as TreeViewItem , ( object ) 1 , null };
            startitem = ActiveTree . SelectedItem as TreeViewItem;
            //            ExpandSetup ( true );
            DirectoryOptions . Focus ( );
            ExpanderMenuOption . Text = "Expand Top Level only of Selected Item.";

            // ClearExpandArgs ( );
            ExpArgs . tvitem = ActiveTree . SelectedItem as TreeViewItem;
            ExpArgs . Selection = 0;
            //            ExpArgs . ExpandLevels = 1;
            RunExpandSystem ( null , null );
            return;
        }
        public async void TriggerExpand1 ( object sender , RoutedEventArgs e )
        {
            if ( ActiveTree . SelectedItem == null )
            {
                //MessageBox . Show ( $"Please select a drive or subfolder before using  these options...." , "No Drive Selected" );
                fdl . ShowInfo ( Flowdoc , canvas ,
                      $"Please select a drive or subfolder before using  this option...." ,
                      "Blue1" ,
                      "TreeView Search Sytem" );
                //fdl . SetFocus ( );
                return;
            }
            object [ ] Args = { ActiveTree . SelectedItem as TreeViewItem , ( object ) 2 , null };
            startitem = ActiveTree . SelectedItem as TreeViewItem;
            ExpandSetup ( true );
            DirectoryOptions . Focus ( );
            if ( ExpArgs . SearchActive == false )
            {
                ExpanderMenuOption . Text = "Fully Expand Selected Item 2 levels";
                ClearExpandArgs ( );
                ExpArgs . tvitem = ActiveTree . SelectedItem as TreeViewItem;
                ExpArgs . Selection = 1;
                ExpArgs . ExpandLevels = 2;
            }
            else
            {
                ExpanderMenuOption . Text = "Search for Item down up to 2 levels";
                ExpArgs . Selection = 1;
            }

            RunExpandSystem ( null , null );
            return;

            //if ( ExpandCurrentAllLevels ( Args ) == true && TextToSearchFor != "" )
            //    MessageBox . Show ( $"[{Searchtext . Text}] FOUND ...." , "Search System" );
            //Mouse . OverrideCursor = Cursors . Arrow;
            //  ActiveTree . HorizontalContentAlignment = HorizontalAlignment . Left;
            //ScrollCurrentTvItemIntoView ( startitem );
        }
        public async void TriggerExpand2 ( object sender , RoutedEventArgs e )
        {
            if ( ActiveTree . SelectedItem == null )
            {
                //                MessageBox . Show ( $"Please select a drive or subfolder before using  these options...." , "No Drive Selected" );
                fdl . ShowInfo ( Flowdoc , canvas ,
                      $"Please select a drive or subfolder before using  this option...." ,
                      "Blue1" ,
                      "TreeView Search Sytem" );
                return;
            }
            object [ ] Args = { ActiveTree . SelectedItem as TreeViewItem , ( object ) 3 , null };
            startitem = ActiveTree . SelectedItem as TreeViewItem;
            ExpandSetup ( true );
            DirectoryOptions . Focus ( );
            if ( ExpArgs . SearchActive == false )
            {
                ExpanderMenuOption . Text = "Fully Expand Selected Item 3 levels";
                ClearExpandArgs ( );
                ExpArgs . tvitem = ActiveTree . SelectedItem as TreeViewItem;
                ExpArgs . Selection = 2;
                ExpArgs . ExpandLevels = 3;
            }
            else
            {
                ExpanderMenuOption . Text = "Search for item down up to 3 levels";
                ExpArgs . Selection = 2;
            }
            RunExpandSystem ( null , null );
            return;
        }
        public async void TriggerExpand3 ( object sender , RoutedEventArgs e )
        {
            if ( ActiveTree . SelectedItem == null )
            {
                //                MessageBox . Show ( $"Please select a drive or subfolder before using  these options...." , "No Drive Selected" );
                fdl . ShowInfo ( Flowdoc , canvas ,
                      $"Please select a drive or subfolder before using  this option...." ,
                      "Blue1" ,
                      "TreeView Search Sytem" );
                return;
            }
            object [ ] Args = { ActiveTree . SelectedItem as TreeViewItem , ( object ) 4 , null };
            DirectoryOptions . Focus ( );
            if ( ExpArgs . SearchActive == false )
            {
                startitem = ActiveTree . SelectedItem as TreeViewItem;
                ExpandSetup ( true );
                ExpanderMenuOption . Text = "Fully Expand Selected Item 4 levels";
                ClearExpandArgs ( );
                ExpArgs . tvitem = ActiveTree . SelectedItem as TreeViewItem;
                ExpArgs . Selection = 3;
                ExpArgs . ExpandLevels = 4;
            }
            else
            {
                ExpanderMenuOption . Text = "Search for Item down to 4 levels";
                ExpArgs . Selection = 3;
            }
            RunExpandSystem ( null , null );
            return;
        }
        public async void TriggerExpand4 ( object sender , RoutedEventArgs e )
        {
            // Open ALL levels
            if ( ActiveTree . SelectedItem == null )
            {
                //MessageBox . Show ( $"Please select a drive or subfolder before using  these options...." , "No Drive Selected" );
                fdl . ShowInfo ( Flowdoc , canvas ,
                     $"Please select a drive or subfolder before using  this option...." ,
                     "Blue1" ,
                     "TreeView Search Sytem" );
                return;
            }
            if ( ExpArgs . SearchActive == false )
            {
                if ( MessageBox . Show ( $"This  can take a *** considerable *** time to complete, and access to the application will not be available until it has completed"
                + ".\n\nAre you sure you want to fully expand the current item ?\n\n" , "Potentially Lengthy Expansion request !" , MessageBoxButton . YesNo , MessageBoxImage . Hand , MessageBoxResult . No ) == MessageBoxResult . No )
                    return;
            }
            object [ ] Args = { ActiveTree . SelectedItem as TreeViewItem , ( object ) 90 , null };
            startitem = ActiveTree . SelectedItem as TreeViewItem;
            ExpandSetup ( true );
            DirectoryOptions . Focus ( );
            if ( ExpArgs . SearchActive == false )
            {
                ExpanderMenuOption . Text = $"Fully Expand Selected Item down {ExpArgs . ExpandLevels} levels";
                ClearExpandArgs ( );
                ExpArgs . tvitem = ActiveTree . SelectedItem as TreeViewItem;
                ExpArgs . Selection = 4;
                ExpArgs . ExpandLevels = 90;
                ExpArgs . ListResults = false;
                LISTRESULTS = true;
            }
            else
            {
                ExpanderMenuOption . Text = $"Search for Item in {ExpArgs . ExpandLevels} levels";
                ExpArgs . Selection = 4;
            }
            RunExpandSystem ( null , null );
            return;

        }

        //******************************************************************************************************//
        // Utility method called recursively to open folders by  other expand methods
        //******************************************************************************************************//
        private bool ExpandFolder ( TreeViewItem item , bool ExpandContent = false )
        {
            string fullpath = "";
            if ( AbortExpand )
                return false;
            if ( item . Items . Count > 0 )
            {
                TreeViewItem tmp = item . Items [ 0 ] as TreeViewItem;
                if ( tmp . ToString ( ) != "Loading" )
                {
                    foreach ( TreeViewItem item2 in item . Items )
                    {
                        Thread . Sleep ( SLEEPTIME );

                        ShowProgress ( );
                        if ( CheckSearchSuccess ( item2 . Tag . ToString ( ) ) == true )
                        {
                            UpdateListBox ( $"Search for {Searchtext . Text} found  as [" + item2 . Header . ToString ( ) + $"]\nin {item2 . Tag . ToString ( )}" );
                            ScrollCurrentTvItemIntoView ( item2 );
                            item2 . IsSelected = true;
                            ExpArgs . SearchSuccessItem = item2;
                            //SearchSuccess = true;
                            ExpArgs . SearchSuccess = true;
                            return true;
                        }
                        fullpath = item2 . Tag . ToString ( ) . ToUpper ( );
                        try
                        {
                            item2 . IsExpanded = true;
                            //stack . Push ( item2 . Tag . ToString ( ) );
                        }
                        catch ( Exception ex ) { Console . WriteLine ( $"ExpandFolder : 1797 : {ex . Message}" ); }
                        UpdateListBox ( item2 . Tag . ToString ( ) );
                        ShowProgress ( );
                        ShowExpandTime ( );
                        if ( FullExpandinProgress == false )
                            ActiveTree . Refresh ( );
                    }
                    UpdateExpandprogress ( );
                    ShowExpandTime ( );
                }
            }
            return false;
        }

        #region Expanding support methods
        private void UpdateExpandprogress ( )
        {
            if ( Expandprogress . Text . Length >= 12 )
                Expandprogress . Text = ".";
            else
                Expandprogress . Text += ".";
            Thread . Sleep ( 10 );
            Expandprogress . UpdateLayout ( );
            Thread . Sleep ( 10 );
        }
        private void loadExpandOptions ( )
        {
            DirOptions = new List<string> ( );
            //           DirOptions . Add ( "Expand current Item down 2 levels \n-> Root \n->       Subfolders\n           ....  Subfolders\n           ....  Files\n-               ....  Subfolders\n                 ....Files\n        ....  Files\n (Reasonably Fast...)" );
            //            DirOptions . Add ( "Expand current Item down 3 levels\n-> Root \n->    SubFolders\n->       Subfolders\n             ....  Subfolders\n                   ....  Subfolders\n                   ....  Files\n-                 ....  Subfolders\n                   ....Files\n             ....  Files\n (Can take  while...)" );
            DirOptions . Add ( "Fully Expand Current Drive 2 Levels" );
            DirOptions . Add ( "Fully Expand Current Drive 3 Levels" );
            DirOptions . Add ( "Fully Expand Current Drive 4 Levels" );
            DirOptions . Add ( "Fully Expand Current Drive ALL Levels" );
            DirOptions . Add ( "Expand ALL Drives down 1 level\n-> Root \n->    SubFolders\n->       ....  Subfolders\n            ....  Files\n->.Files\n (Quite fast...)" );
            DirOptions . Add ( "Expand ALL Drives down 2 levels \n-> Root \n->    SubFolders\n         Subfolders\n            Files\n      ....  .Files \n (WARNIING - May take some time....)" );
            DirOptions . Add ( "Fully Expand All Drives below current ? levels\n  (WARNING - Can take quite some time !" );
            DirOptions . Add ( "Collapse All Drives" );
            DirectoryOptions . Items . Clear ( );
            DirectoryOptions . ItemsSource = DirOptions;
            DirectoryOptions . SelectedIndex = 0;
            DirectoryOptions . SelectedItem = 0;
        }
        private void LoadSearchLevels ( )
        {
            LevelsCombo . Items . Add ( "2" );
            LevelsCombo . Items . Add ( "3" );
            LevelsCombo . Items . Add ( "4" );
            LevelsCombo . Items . Add ( "5" );
            LevelsCombo . Items . Add ( "90" );
        }

        #endregion Expanding support mmethods


        private void Sw_Tick ( object sender , EventArgs e )
        {
            //TimeElapsed = temp;
        }

        #endregion Expand // Collapse

        private void TREEViews_KeyDown ( object sender , KeyEventArgs e )
        {
            if ( e . Key == Key . F5 )
                BreakExpand = true;
        }
        private void DirectoryOptions_Selected ( object sender , RoutedEventArgs e )
        {
            ComboBox cb = sender as ComboBox;
            ExpanderMenuOption . Text = cb . SelectedItem . ToString ( );
        }
        private void CreateTreeViewData ( string DriveToLoad , List<string> dirs , List<string> files )
        {
            List<string> currentdir = new List<string> ( );
            List<string> currentfile = new List<string> ( );
            int iterator = 0;
            ActiveTree . Items . Clear ( );

            foreach ( var drive in Directory . GetLogicalDrives ( ) )
            {
                if ( drive != DriveToLoad && DriveToLoad != "ALL" )
                    continue;
                iterator++;
                var item = new TreeViewItem ( );
                item . Header = drive;
                item . Tag = drive;

                // Add Dummy entry so we get an "Can be Opened" triangle icon
                int dircount = tvcollection . GetDirectories ( drive , out List<string> directories );
                if ( dircount > 0 )
                {
                    item . Items . Add ( "Loading" );

                    // Add Drive to Treeview with dummy "Loading" item
                    ActiveTree . Items . Add ( item );
                }
                continue;
            }
        }
        private void listBox_PreviewMouseRightButtonDown ( object sender , MouseButtonEventArgs e )
        {

            //ListBox tv = sender as ListBox;
            //if ( tv . Visibility == Visibility . Visible )
            //{
            //    tv . Visibility = Visibility . Hidden;
            //    xtreeView4 . Visibility = Visibility . Visible;
            //}
            //else
            //{
            //    tv . Visibility = Visibility . Visible;
            //    xtreeView4 . Visibility = Visibility . Hidden;
            //}
        }
        private void TestTree_PreviewMouseRightButtonDown ( object sender , MouseButtonEventArgs e )
        {
            return;
        }
        private void DirectoryOptions_Selected ( object sender , SelectionChangedEventArgs e )
        {
            ExpanderMenuOption . Text = $"{DirectoryOptions . SelectedItem . ToString ( )}";

        }
        private void WalkTestTree ( object sender , RoutedEventArgs e )
        {
            //        TriggerExpand4 ( sender , e );
            //object [ ] Args = new object [ 3 ] { null , null , null };
            Args [ 0 ] = ActiveTree . SelectedItem as TreeViewItem;
            Args [ 1 ] = 90;
            //SelectedTVItem = ActiveTree . SelectedItem as TreeViewItem;
            SearchString = "";
            ExpandSelection = 4;
            ClearExpandArgs ( );
            ExpArgs . tvitem = ActiveTree . SelectedItem as TreeViewItem;
            ExpArgs . Selection = 4;
            ExpArgs . ExpandLevels = 90;
            RunExpandSystem ( sender , e );
        }

        #region Expand Utility methods
        public void ShowProgress ( )
        {
            if ( ProgressCount < PROGRESSWRAPVALUE )
                ProgressCount++;
            else
                ProgressCount = 0;
        }
        public void StartTimer ( )
        {
            startmin = DateTime . Now . Minute;
            startsec = DateTime . Now . Second;
            startmsec = DateTime . Now . Millisecond;
        }
        public void StopTimer ( )
        {
            startmin = 0;
            startsec = 0;
            startmsec = 0;
        }
        public void ShowExpandTime ( )
        {
            //Working fine, updates an Att.Prop that is Binded  to display field
            double endmin = DateTime . Now . Minute;
            double endsec = DateTime . Now . Second;
            double endmsec = DateTime . Now . Millisecond;
            double total = 0;
            double rsecs = 0, rmin = 0, rmsecs = 0;
            //Console . WriteLine ( $"{startmin}, {startsec}, {startmsec}" );
            //Console . WriteLine ( $"{endmin}, {endsec}, {endmsec}" );
            if ( endmin >= startmin )
                rmin = endmin - startmin;
            else
                rmin = endmin + ( 60 - startmin );

            if ( endsec > startsec )
                rsecs = endsec - startsec;// > 0 ? ( endsec - startsec ) : 0;
            else
            {
                // end Seconds less then start secs
                rsecs = ( 60 - startsec ) + endsec;
                if ( rmin == ( double ) 1 )
                {
                    rmin = rmin - 1;
                    rsecs = ( rmin * 60 ) + rsecs;
                }
                else
                {
                    rsecs = endsec - startsec;
                }
            }
            if ( endmsec > startmsec )
                rmsecs = endmsec - startmsec; //> 0 ? ( endmsec - startmsec ) : 0;
            else
                rmsecs = ( 1000 - startmsec ) + endmsec;
            //            Console . WriteLine ($"{rmin}, {rsecs}, {rmsecs}\n");
            BreakExpand = false;
            total = ( rmin * 60 );
            total += rsecs;
            string restime = String . Format ( "{0:0.00}" , ( ( total * 1000 ) + rmsecs ) / 1000 ) + " secs";
            //            SetExpandDuration ( this , restime );
            ExpandDuration = restime;
        }
        public int CalculateLevel ( string currentitem )
        {
            int len = 0;
            string [ ] levels = currentitem . Split ( '\\' );
            if ( levels [ 1 ] == "" )
                return 1;
            else len = levels . Length;
            //            else len = levels . Length - 1;
            return len;
        }
        private void ExpandSetup ( bool direction )
        {
            if ( direction )
            {
                //                ProgressCount = 0;
                Duration . Text = "";
                ProgressCount = 0;
                ProgressString = "";
                Listboxtotal = 0;
                StartTimer ( );
                //                listBox . Items . Clear ( );
                //                listBox . Refresh ( );
                Thickness th = new Thickness ( 2 , 2 , 2 , 2 );
                Expandprogress . Text = ".";
                Expandprogress . BorderThickness = th;
                Expandcounter . BorderThickness = th;
                Expandprogress . BorderBrush = FindResource ( "Red5" ) as SolidColorBrush;
                Expandcounter . Foreground = FindResource ( "Yellow1" ) as SolidColorBrush;
                Expandprogress . UpdateLayout ( );
                Expandcounter . BorderBrush = FindResource ( "Red5" ) as SolidColorBrush;
                Expandcounter . Text = "";
                Expandcounter . UpdateLayout ( );
                BusyLabel . Visibility = Visibility . Visible;
                treeViews . Cursor = Cursors . Wait;
            }
            else
            {
                Expandprogress . BorderBrush = FindResource ( "White0" ) as SolidColorBrush;
                Expandcounter . BorderBrush = FindResource ( "White0" ) as SolidColorBrush;
                Expandcounter . Foreground = FindResource ( "White4" ) as SolidColorBrush;
                Thickness th = new Thickness ( 0 , 0 , 0 , 0 );
                Expandprogress . BorderThickness = th;
                Expandcounter . BorderThickness = th;
                //                BusyLabel . Visibility = Visibility . Hidden;

                if ( ExpArgs . SearchSuccess == false )
                    UpdateListBox ( $"\nExpansion completed successfully ..." );
                TotalItemsExpanded = 0;
                ShowExpandTime ( );
                Expandprogress . Refresh ( );
                Expandcounter . Refresh ( );
                UpdateListBox ( "" );
                if ( ExpArgs . SearchSuccess )
                    UpdateListBox ( $"Around {Expandcounter . Text } objects have been\nOpened & Searched..." );
                else
                    UpdateListBox ( $"Around {Expandcounter . Text } objects have been Expanded..." );
                Expandprogress . Text = "Finished ...";
                vsplitterright . Cursor = Cursors . SizeWE;
                hsplitter . Cursor = Cursors . SizeNS;
                treeViews . Cursor = Cursors . Arrow;
            }
        }
        public void ScrollCurrentTvItemIntoView ( TreeViewItem item )
        {
            // Brings selected item  into view as selected item
            var count = VisualTreeHelper . GetChildrenCount ( item );

            for ( int i = count - 1 ; i >= 0 ; --i )
            {
                var childItem = VisualTreeHelper . GetChild ( item , i );
                if ( childItem != ( DependencyObject ) null )
                    ( ( FrameworkElement ) childItem ) . BringIntoView ( );
                //item.BringIntoView ( );
            }
        }
        public void ScrollTvItemIntoView ( TreeViewItem item )
        {
            item . BringIntoView ( );
        }

        #endregion Expand Utility methods
        private ScrollEventType ScrollToLeftEnd;

        #region Failed Search fom M$$$$$$

        /// <summary>
        /// Recursively search for an item in this subtree.
        /// </summary>
        /// <param name="container">
        /// The parent ItemsControl. This can be a TreeView or a TreeViewItem.
        /// </param>
        /// <param name="item">
        /// The item to search for.
        /// </param>
        /// <returns>
        /// The TreeViewItem that contains the specified item.
        /// </returns>
        private TreeViewItem GetTreeViewItem ( ItemsControl container , object item )
        {
            if ( container != null )
            {
                if ( container . DataContext == item )
                {
                    return container as TreeViewItem;
                }

                // Expand the current container
                if ( container is TreeViewItem && !( ( TreeViewItem ) container ) . IsExpanded )
                {
                    container . SetValue ( TreeViewItem . IsExpandedProperty , true );
                }

                // Try to generate the ItemsPresenter and the ItemsPanel.
                // by calling ApplyTemplate.  Note that in the
                // virtualizing case even if the item is marked
                // expanded we still need to do this step in order to
                // regenerate the visuals because they may have been virtualized away.

                container . ApplyTemplate ( );
                ItemsPresenter itemsPresenter =
                    ( ItemsPresenter ) container . Template . FindName ( "ItemsHost" , container );
                if ( itemsPresenter != null )
                {
                    itemsPresenter . ApplyTemplate ( );
                }
                else
                {
                    // The Tree template has not named the ItemsPresenter,
                    // so walk the descendents and find the child.
                    itemsPresenter = FindVisualChild<ItemsPresenter> ( container );
                    if ( itemsPresenter == null )
                    {
                        container . UpdateLayout ( );

                        itemsPresenter = FindVisualChild<ItemsPresenter> ( container );
                    }
                }

                Panel itemsHostPanel = ( Panel ) VisualTreeHelper . GetChild ( itemsPresenter , 0 );

                // Ensure that the generator for this panel has been created.
                UIElementCollection children = itemsHostPanel . Children;

                //                MyVirtualizingStackPanel virtualizingPanel =
                //                    itemsHostPanel as MyVirtualizingStackPanel;

                for ( int i = 0, count = container . Items . Count ; i < count ; i++ )
                {
                    TreeViewItem subContainer;
                    //if ( virtualizingPanel != null )
                    //{
                    //    // Bring the item into view so
                    //    // that the container will be generated.
                    //    virtualizingPanel . BringIntoView ( i );

                    //    subContainer =
                    //        ( TreeViewItem ) container . ItemContainerGenerator .
                    //        ContainerFromIndex ( i );
                    //}
                    //else
                    {
                        subContainer = ( TreeViewItem ) container . ItemContainerGenerator . ContainerFromIndex ( i );

                        // Bring the item into view to maintain the
                        // same behavior as with a virtualizing panel.
                        subContainer?.BringIntoView ( );
                    }

                    if ( subContainer != null )
                    {
                        // Search the next level for the object.
                        TreeViewItem resultContainer = GetTreeViewItem ( subContainer , item );
                        if ( resultContainer != null )
                        {
                            return resultContainer;
                        }
                        else
                        {
                            // The object is not under this TreeViewItem
                            // so collapse it.
                            if ( subContainer != null )
                                subContainer . IsExpanded = false;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Search for an element of a certain type in the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of element to find.</typeparam>
        /// <param name="visual">The parent element.</param>
        /// <returns></returns>
        private T FindVisualChild<T> ( Visual visual ) where T : Visual
        {
            for ( int i = 0 ; i < VisualTreeHelper . GetChildrenCount ( visual ) ; i++ )
            {
                Visual child = ( Visual ) VisualTreeHelper . GetChild ( visual , i );
                if ( child != null )
                {
                    T correctlyTyped = child as T;
                    if ( correctlyTyped != null )
                    {
                        return correctlyTyped;
                    }

                    T descendent = FindVisualChild<T> ( child );
                    if ( descendent != null )
                    {
                        return descendent;
                    }
                }
            }

            return null;
        }
        #endregion Failed Search fom M$$$$$$

        private void listBox_SelectionChanged ( object sender , SelectionChangedEventArgs e )
        {
            string str = "";
            bool iterate = false;
            ListBox tb = sender as ListBox;
            tb . ScrollIntoView ( tb . SelectedItem );
            tb . HorizontalAlignment = HorizontalAlignment . Left;
            //foreach ( var item in tb.Items )
            //{
            //    if(item.)
            //}
            //if ( tb . SelectedIndex != -1 )
            //{ 
            //    if ( Selindex != -1 && iterate == false)
            //    {
            //        var  lastone = tb . Items [ Selindex ];
            //        iterate = true;
            //        tb . SelectedItem = lastone;
            //        iterate = false;
            //        (lastone as TextBlock).Background = FindResource ( "White3" ) as SolidColorBrush;
            //        Selindex = -1;
            //    }

            //    Selindex = tb . SelectedIndex;
            //    TextBlock lbi = tb . SelectedItem as TextBlock;
            //    lbi . Background = FindResource ( "Black0" ) as SolidColorBrush;
            //    return;
            //}
        }
        private bool CheckFileForMatch ( List<string> files , string upperstring , out string resstring )
        {
            bool result = false;
            resstring = "";
            foreach ( var filename in files )
            {
                Console . WriteLine ( $"? FILE match[{filename . ToUpper ( )}]" );
                if ( filename . ToUpper ( ) . Contains ( upperstring ) == true )
                {
                    resstring = filename;
                    return true;
                }
            }
            return false;
        }
        private bool CheckFolderForMatch ( string folder , string upperstring , out string resultstring )
        {
            bool result = false;
            resultstring = "";
            List<string> subfolders = new List<string> ( );
            List<string> files = new List<string> ( );
            string resstring = "";

            Console . WriteLine ( $"? FOLDER match [{folder}]" );
            TreeViewItem tvfound = new TreeViewItem ( );
            //tvfound . Tag = folder;
            //tvfound . IsExpanded = true;
            if ( folder . Contains ( upperstring ) == true )
            {
                tvfound . IsExpanded = true;
                return true;
            }
            tvcollection . GetFiles ( folder , out files );
            if ( CheckFileForMatch ( files , upperstring , out resultstring ) == true )
            {
                tvfound . Tag = resultstring;
                tvfound . IsSelected = true;
                return true;
            }
            tvcollection . GetDirectories ( folder , out subfolders );
            foreach ( var filename in subfolders )
            {
                Console . WriteLine ( $"? ? FOLDER match [{filename}]" );
                if ( filename . ToUpper ( ) . Contains ( upperstring ) == true )
                {
                    result = true;
                    break;
                }
                else
                {
                    Console . WriteLine ( $"? ? iterating inner FOLDER match [{filename}]" );
                    if ( CheckFolderForMatch ( filename , upperstring , out resultstring ) == true )
                    {
                        tvfound . Tag = resultstring;
                        tvfound . IsSelected = true;
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
        private void Searchtext_PreviewMouseDown ( object sender , MouseButtonEventArgs e )
        {
            if ( Searchtext . Text == "Search for...." )
                Searchtext . Text = "";
        }
        private string GetParentPath ( string tag )
        {
            string [ ] items = tag . Split ( '\\' );
            string newpath = "";
            int max = items . Count ( ) - 1;
            for ( int x = 0 ; x <= max ; x++ )
            {
                if ( x < max )
                    newpath += items [ x ] + "\\";
                else if ( x == max - 1 )
                    newpath += items [ x ];
            }
            return items [ 0 ];
        }
        private void CollapseAllDrives ( )
        {
            Mouse . OverrideCursor = Cursors . Wait;
            TreeView tv = new TreeView ( );
            if ( ExpArgs . tv != null )
                tv = ExpArgs . tv;
            else
                tv = ActiveTree;
            //    TreeView tv = treeView4;
            foreach ( TreeViewItem item in tv . Items )
            {
                if ( item . IsExpanded )
                    item . IsExpanded = false;
                //ExpandAll3 ( rootTreeViewItem , false );
            }
            //ShowExpandTime ( );
            //            ActiveTree . HorizontalContentAlignment = HorizontalAlignment . Left;
            tv . Refresh ( );
            Mouse . OverrideCursor = Cursors . Arrow;
        }
        public bool CheckSearchSuccess ( string currentitem )
        {
            bool result = false;
            currentitem = currentitem . ToUpper ( );
            if ( ExpArgs . SearchSuccess == true || ExpArgs . SearchTerm == "" )
                return false;
            //if ( currentitem . Contains ( "BG-BG" ) )
            //    Console . WriteLine ( $"" );
            if ( Exactmatch )
            {
                result = currentitem == ExpArgs . SearchTerm;
            }

            else
            {
                if ( currentitem . Contains ( ExpArgs . SearchTerm ) )
                    result = true;
                else
                    result = false;
            }

            return result;
        }
        private void ShowFullPath ( object sender , RoutedEventArgs e )
        {
            TreeViewItem tvi = ActiveTree . SelectedItem as TreeViewItem;
            MessageBoxResult result = MessageBox . Show ( $"{tvi . Tag . ToString ( )}\n\nClick YES to save to ClipBoard ..." , "Full path of current  item"
             , MessageBoxButton . YesNo , MessageBoxImage . Question , MessageBoxResult . No );
            if ( result == MessageBoxResult . Yes )
                Clipboard . SetText ( tvi . Tag . ToString ( ) );
        }
        private void CollapseTree ( object sender , RoutedEventArgs e )
        {
            object [ ] Args = { "C:\\" , null , null };
            Args [ 0 ] = ActiveTree as object;
            ExpArgs . tv = ActiveTree;
            ExpandSetup ( true );
            CollapseAllDrives ( );
            Mouse . OverrideCursor = Cursors . Arrow;
            ExpandSetup ( false );
        }
        private void InfoList_Scroll ( object sender , System . Windows . Controls . Primitives . ScrollEventArgs e )
        {
            ListBox lb = sender as ListBox;
            lb . HorizontalContentAlignment = HorizontalAlignment . Left;
        }

        private void CreateBrushes ( )
        {
            Brush0 = FindResource ( "White0" ) as SolidColorBrush;
            Brush1 = FindResource ( "White3" ) as SolidColorBrush;
            Brush2 = FindResource ( "Red6" ) as SolidColorBrush;
            Brush3 = FindResource ( "Black0" ) as SolidColorBrush;
            Brush4 = FindResource ( "Blue3" ) as SolidColorBrush;
            Brush5 = FindResource ( "Orange1" ) as SolidColorBrush;
            Brush6 = FindResource ( "Magenta3" ) as SolidColorBrush;
            Brush7 = FindResource ( "Black7" ) as SolidColorBrush;
            Brush8 = FindResource ( "Green2" ) as SolidColorBrush;
        }

        #region Search
        private TreeViewItem [ ] SearchSubDir ( TreeViewItem item , string SearchTerm , out TreeViewItem MatchingItem , bool AddFolders )
        {
            //    bool result = false;
            //    bool found = false;
            //    // Save main calling item so we can get back  to it later on
            //    TreeViewItem CallingItem = item;
            //    TreeViewItem MatchItem = null;
            MatchingItem = null;

            //    // see if the parent item matches ?
            //    if ( CheckForMatchingItem ( item , SearchTerm ) )
            //    {
            //        SearchSuccess = true;
            //        return null;
            //    }
            //    if ( AddFolders )
            //    {
            //        List<string> directories = new List<string> ( );
            //        int count = tvitemclass.GetDirectories ( item . Tag . ToString ( ) , out directories );

            //        AddDirectoriesToTestTreeview ( directories , item , listBox );
            //        AddFilesToSubdirectory ( item );
            //        item . IsExpanded = true;
            //        ActiveTree .Refresh ( );
            //    }
            //    foreach ( TreeViewItem itms in item . Items )
            //    {
            //        if ( CheckForMatchingItem ( itms , SearchTerm ) )
            //        {
            //            SearchSuccess = true;
            //            return null;
            //        }
            //        if ( itms . HasItems )
            //        {
            //            itms . IsExpanded = true;
            //            AddFilesToSubdirectory ( itms );
            //            if ( SearchIterate ( itms , SearchTerm ) == null )
            //                continue;
            //        }
            //    }
            return null;
        }
        private void AddFilesToSubdirectory ( TreeViewItem item )
        {
            List<string> files = new List<string> ( );
            tvcollection . GetFiles ( item . Tag . ToString ( ) , out files );
            if ( item . Items [ 0 ] . ToString ( ) == "Loading" )
                item . Items . Clear ( );
            if ( files . Count > 0 )
            {
                for ( int y = 0 ; y < files . Count ; y++ )
                {
                    item . Items . Add ( files [ y ] );
                    item . IsExpanded = true;
                    ScrollCurrentTvItemIntoView ( item );
                    ActiveTree . Refresh ( );
                }
            }
            else
            {
                // No Directories or files, so remove "Loading" Dummy
                if ( item . Items . Count == 0 )
                    item . Items . Clear ( );
            }
        }
        // NOT USED
        private TreeViewItem SearchIterate ( TreeViewItem item , string SearchTerm )
        {
            bool result = false;
            bool found = false;
            // Save main calling item so we can get back  to it later on
            TreeViewItem CallingItem = item;
            TreeViewItem MatchItem = null;

            // Is this a match ?
            if ( CheckForMatchingItem ( item , SearchTerm ) )
            {
                //                SearchSuccess = true;
                ExpArgs . SearchSuccess = true;
                return null;
            }

            // no, so list all remaining subdirs
            foreach ( TreeViewItem itms in item . Items )
            {
                if ( itms . Header . ToString ( ) == "Loading" )
                    return null;
                if ( itms . Tag . ToString ( ) . ToUpper ( ) . Contains ( SearchTerm ) )
                {
                    //UpdateListBox ( $"\nSearch for {Searchtext . Text} found as [\" + { itms . Header . ToString ( ) }\"] \nin {itms . Tag . ToString ( )}" );
                    itms . IsSelected = true;
                    ScrollCurrentTvItemIntoView ( itms );
                    //                    SearchSuccess = true;
                    ExpArgs . SearchSuccess = true;
                    ActiveTree . Refresh ( );
                    return itms;
                }
                if ( itms . HasItems )
                {

                }
            }
            return MatchItem;
        }
        private bool CheckForMatchingItem ( TreeViewItem item , string SearchTerm )
        {
            if ( item . HasItems )
            {
                foreach ( TreeViewItem itm in item . Items )
                {
                    if ( itm . HasItems )
                    {
                        if ( itm . Tag . ToString ( ) . ToUpper ( ) . Contains ( SearchTerm ) )
                        {
                            //                            UpdateListBox ( $"Search for {Searchtext . Text} found  as [\" + { itm . Header . ToString ( ) }\"] \nin {itm . Tag . ToString ( )}" );
                            itm . IsSelected = true;
                            ScrollCurrentTvItemIntoView ( itm );
                            //SearchSuccess = true;
                            ExpArgs . SearchSuccess = true;
                            ActiveTree . Refresh ( );
                            return true;

                        }
                    }
                }
            }
            return false;
        }
        #endregion Search

        private int testcount = 0;
        public void UpdateListBox ( string entry )
        {
            testcount++;
            if ( testcount < 25 )
                Console . WriteLine ( $"{entry}" );
            if ( LISTRESULTS == false )
            {
                if ( listBox . Items . Count == 0 )
                {
                    if ( ExpArgs . SearchActive )
                        listBox . Items . Add ( "Logging to this List is automatically disabled\nfor all Search operations to improve the\nspeed of  the search ..." );
                    else if ( LISTRESULTS == false )
                    {
                        listBox . Items . Add ( "Logging to this List is currently disabled" );
                        listBox . Items . Add ( "Right click to access TreeView Options to change  this" );
                        //                        listBox . Items . Add ( "from the TreeView Options to change  this..." );
                    }
                }
                return;
            }
            TextBlock tblk = new TextBlock ( );
            LbTextblock = tblk;
            string [ ] items;
            items = entry . Split ( '\\' );
            if ( entry . Contains ( "." ) && entry . Contains ( "All " ) == false )
            {
                //LIGHT Black on gray- Filles
                tblk . Foreground = Brush3;
                tblk . Background = Brush1;
                tblk . FontWeight = FontWeights . Normal;
                string [ ] dot = entry . Split ( '\\' );
                if ( dot . Length == 2 )
                    entry = dot [ dot . Length - 1 ];
                else if ( dot . Length >= 3 )
                    entry = entry;
                else
                    entry = dot [ dot . Length - 1 ];
            }
            else if ( items . Length == 2 && items [ 1 ] == "" )
            {
                //BOLD White on Red background - Drives
                tblk . Foreground = Brush0;
                tblk . Background = Brush2;
                tblk . FontWeight = FontWeights . DemiBold;
            }
            else if ( items . Length == 2 && items [ 1 ] != "" )
            {
                //NORMAL Red on Gray - C:\\xxxxx
                tblk . Foreground = Brush2;
                tblk . Background = Brush1;
                tblk . FontWeight = FontWeights . Normal;
            }
            else if ( items . Length == 3 )
            {
                //NORMAL Magenta on gray - C:\\xxxx\\xxxx
                tblk . Foreground = Brush6;
                tblk . Background = Brush1;
                tblk . FontWeight = FontWeights . Normal;
            }
            else if ( items . Length == 4 )
            {
                //LIGHTFONT - Orange on gray - C:\\xxx\\xxx\\xxx
                tblk . Foreground = Brush5;
                tblk . Background = Brush1;
                tblk . FontWeight = FontWeights . Light;
            }
            else if ( items . Length == 5 )
            {
                //NORMAL Orange on gray - C:\\xxx\\xxx\\xxx
                tblk . Foreground = Brush5;
                tblk . Background = Brush1;
                tblk . FontWeight = FontWeights . Normal;
                //Console . WriteLine ( );
            }
            else
            {
                //NORMAL Blue on gray
                tblk . Foreground = Brush3;
                tblk . Background = Brush1;
                tblk . FontWeight = FontWeights . Normal;
            }

            tblk . Text = entry;
            // Doing it this way forces the  listbox  to remain LEFT Justified even with long lines
            ListBoxItem lbi = new ListBoxItem ( );
            lbi . Content = tblk;
            lbi . HorizontalAlignment = HorizontalAlignment . Left;
            int currindex = listBox . Items . Add ( lbi );

            //listBox . SelectedIndex = currindex;
            //listBox . SelectedItem = currindex;
            //// Bound in xaml
            Listboxtotal++;
            ShowExpandTime ( );
            if ( entry != "" && entry . Length > 1 )
            {
                if ( entry . Substring ( 1 , 1 ) == ":" )
                    TotalItemsExpanded++;
            }
            //ListBoxItem current = listBox . SelectedItem as ListBoxItem;
            //if ( current != null )
            //{
            //    ListBoxItem lbitem = listBox . Items [ currindex - 1 ] as ListBoxItem;
            //    listBox . Refresh ( );
            //}
            //listBox . HorizontalContentAlignment = HorizontalAlignment . Left;
            //listBox . UpdateLayout ( );
        }

        /// <summary>
        /// Main method for expanding  folders, or searching for items
        /// </summary>
        /// <param name="Args"></param>
        /// <returns></returns>
        public bool ExpandCurrentAllLevels ( object [ ] Args )
        {
            bool Returnval = false;
            bool IsComplete = false;
            int iterations = 0;
            int itemcount = 0;
            int levelscount = 0;
            var fail = false;
            var success = true;
            int levels = ( int ) ExpArgs . ExpandLevels;
            TreeViewItem items = ExpArgs . tvitem;
            startitem = items;
            if ( items == null )
                return false;
            //ProgressCount = 0;
            string TagString = items . Tag . ToString ( ) . ToUpper ( );
            if ( TagString . Contains ( items . Header . ToString ( ) . ToUpper ( ) ) == false )
                items . Header = items . Tag . ToString ( );
            try
            {
                items . IsExpanded = true;
            }
            catch ( Exception ex ) { Console . WriteLine ( $"ExpandCurrentAllLevels: 2736 : {ex . Message}" ); }

            ShowProgress ( );
            if ( FullExpandinProgress == false )
                ActiveTree . Refresh ( );
            levelscount = CalculateLevel ( TagString );
            if ( levelscount >= ExpArgs . ExpandLevels )
            {
                ShowExpandTime ( );
                ExpandSetup ( false );
                Expandprogress . Refresh ( );
                return false;
            }
            //**************
            // Main LOOP
            //**************
            foreach ( object objct in items . Items )
            {
                int currentcount = 0;
                TreeViewItem obj = objct as TreeViewItem;
                currentcount = obj . Items . Count;
                //    TreeViewItem obj = objct as TreeViewItem;
                if ( obj == null || obj . ToString ( ) == "Loading" )
                    break;
                ShowProgress ( );
                //levelscount = CalculateLevel ( obj . Tag . ToString ( ) );
                if ( levelscount > ExpArgs . ExpandLevels )
                {
                    continue;
                    //                    Console . WriteLine ( $"LEVELS 1 - Breaking out where level {levelscount} <= {levels}" );
                    //                    break;
                }
                Selection . Text = $"Expanding {obj . Tag . ToString ( )}";
                TreeViewItem childControl = obj as TreeViewItem;
                // working correctly
                //UpdateListBox ( childControl . Tag . ToString ( ) );
                ShowProgress ( );
                ShowExpandTime ( );
                if ( CheckSearchSuccess ( childControl . Tag . ToString ( ) ) == true )
                {
                    UpdateListBox ( $"Search for {Searchtext . Text} found  as [" + childControl . Header . ToString ( ) + $"]\nin {childControl . Tag . ToString ( )}" );
                    ScrollCurrentTvItemIntoView ( childControl );
                    childControl . IsSelected = true;
                    ExpArgs . SearchSuccessItem = childControl;
                    ExpArgs . SearchSuccess = true;
                    Returnval = true;
                    IsComplete = true;
                    ActiveTree . Refresh ( );
                    break;
                }

                try
                {
                    childControl . IsExpanded = true;
                }
                catch ( Exception ex ) { Console . WriteLine ( $"ExpandCurrentAllLevels: 2790 : {ex . Message}" ); }

                if ( childControl . Items . Count == 1 && childControl . Header . ToString ( ) == "Loading" )
                    continue;
                //                Console . WriteLine ( $"2 Level={levelscount}, Outer loop {childControl . Tag . ToString ( )}" );
                ShowProgress ( );
                levelscount = CalculateLevel ( childControl . Tag . ToString ( ) );
                if ( levelscount >= ExpArgs . ExpandLevels )
                    continue;

                if ( childControl != null )
                {
                    string entry = childControl . Header . ToString ( ) . ToString ( ) . ToUpper ( );
                    itemcount = childControl . Items . Count;
                    ShowProgress ( );
                    if ( FullExpandinProgress == false )
                        ActiveTree . Refresh ( );
                    iterations++;
                    if ( ExpArgs . ExpandLevels >= 3 )
                    {
                        //******************
                        // INNER LOOP
                        //******************
                        UpdateListBox ( childControl . Tag . ToString ( ) );
                        if ( childControl . Items . Count > 0 )
                        {
                            TreeViewItem tmp = childControl . Items [ 0 ] as TreeViewItem;
                            if ( tmp . ToString ( ) == "Loading" && childControl . Items . Count > 1 )
                            {
                                MessageBox . Show ( $"ERROR, {childControl . Tag . ToString ( )} has  a 'Loading' dummy entry" );
                                AbortExpand = true;
                                break;
                            }
                        }
                        foreach ( TreeViewItem nextitem in childControl . Items )
                        {
                            ShowProgress ( );
                            if ( TreeViewCollection . CheckIsVisible ( nextitem . Tag . ToString ( ) . ToUpper ( ) , ShowAllFiles , out HasHidden ) == false )
                            {
                                Console . WriteLine ( $"System file : {nextitem . Tag . ToString ( ) . ToUpper ( )}" );
                                continue;
                            }

                            if ( CheckSearchSuccess ( nextitem . Tag . ToString ( ) ) == true )
                            {
                                UpdateListBox ( $"Search for {Searchtext . Text} found  as [" + nextitem . Header . ToString ( ) + $"]\nin {nextitem . Tag . ToString ( )}" );
                                ScrollCurrentTvItemIntoView ( nextitem );
                                nextitem . IsSelected = true;
                                //SearchSuccess = true;
                                ExpArgs . SearchSuccess = true;
                                ExpArgs . SearchSuccessItem = nextitem;
                                Returnval = true;
                                ActiveTree . Refresh ( );
                                break;
                            }
                            try
                            {
                                nextitem . IsExpanded = true;
                                if ( AbortExpand == true )
                                    break;
                                if ( ExpandLimited == true )
                                    continue;
                            }
                            catch ( Exception ex ) { Console . WriteLine ( $"ExpandCurrentAllLevels: 2853 : {ex . Message}" ); }
                            ShowProgress ( );
                            // working correctly
                            if ( FullExpandinProgress == false )
                                ActiveTree . Refresh ( );
                            //                            Console . WriteLine ( Selection . Text );
                            levelscount = CalculateLevel ( nextitem . Tag . ToString ( ) );
                            if ( levelscount >= ExpArgs . ExpandLevels )
                            {
                                continue;
                            }
                            UpdateListBox ( nextitem . Tag . ToString ( ) );
                            if ( ExpArgs . ExpandLevels >= 4 )
                            {
                                if ( TreeViewCollection . CheckIsVisible ( nextitem . Tag . ToString ( ) . ToUpper ( ) , ShowAllFiles , out HasHidden ) == false )
                                {
                                    Console . WriteLine ( $"System file : {nextitem . Tag . ToString ( ) . ToUpper ( )}" );
                                    continue;
                                }
                                if ( ExpandAll3 ( nextitem , true , ExpArgs . ExpandLevels ) == true )
                                {
                                    //SearchSuccess = true;
                                    ExpArgs . SearchSuccess = true;
                                    Returnval = true;
                                    break;
                                }
                            }
                            ShowExpandTime ( );
                            ShowProgress ( );
                            if ( FullExpandinProgress == false )
                                ActiveTree . Refresh ( );
                        }   // End INNER FOREACH

                        if ( IsComplete )
                            break;
                        if ( AbortExpand == true )
                            break;
                        ShowExpandTime ( );
                    }
                    ShowExpandTime ( );
                }
                if ( FullExpandinProgress == false )
                    ActiveTree . Refresh ( );
                ShowProgress ( );
                if ( IsComplete )
                    break;
            }   // End FOREACH

            ShowExpandTime ( );
            ExpandSetup ( false );
            Expandprogress . Refresh ( );
            if ( AbortExpand == true )
            {
                UpdateListBox ( $"Expansion has been CANCELLED, \n\nNot all expected folders have been expanded..." );
                MessageBox . Show ( $"Current Expansion of {startitem} has been CANCELLED. \n\nNot all expected folders have been expanded..." , "System Information" );
            }
            Selection . Text = $"{startitem . Tag . ToString ( )} Expanded {ExpArgs . ExpandLevels} levels Successfully...";
            if ( ExpArgs . SearchSuccess == false )
            {
                startitem . IsSelected = true;
                ScrollCurrentTvItemIntoView ( Args [ 0 ] as TreeViewItem );
                ActiveTree . Refresh ( );
            }
            // Reset global flag for cancellation
            AbortExpand = false;
            ExpandLimited = false;
            return Returnval;
        }
        public Task<bool> ExpandCurrentAllLevelsTask ( object [ ] Args )
        {
            bool Returnval = false;
            bool IsComplete = false;
            int iterations = 0;
            int itemcount = 0;
            int levelscount = 0;
            var fail = false;
            var success = true;
            //            int levels = ( int ) Args [ 1 ];
            TreeViewItem items = Args [ 0 ] as TreeViewItem;
            startitem = items;
            fail = false;
            if ( items == null )
                return Task . FromResult ( fail );
            //ProgressCount = 0;
            if ( items . Tag . ToString ( ) . ToUpper ( ) . Contains ( items . Header . ToString ( ) . ToUpper ( ) ) == false )
                items . Header = items . Tag . ToString ( );
            // Essential to force root to be expanded, else nothing happens
            if ( CheckSearchSuccess ( items . Tag . ToString ( ) ) == true )
            {
                UpdateListBox ( $"Search for {Searchtext . Text} found  as [" + items . Header . ToString ( ) + $"]\nin {items . Tag . ToString ( )}" );
                ScrollCurrentTvItemIntoView ( items );
                items . IsSelected = true;
                ExpArgs . SearchSuccessItem = items;
                //SearchSuccess = true;
                ExpArgs . SearchSuccess = true;
                ActiveTree . Refresh ( );
                return Task . FromResult ( success );
            }

            try
            {
                items . IsExpanded = true;
            }
            catch ( Exception ex ) { Console . WriteLine ( $"ExpandCurrentAllLevels: 2956 : {ex . Message}" ); }

            ShowProgress ( );
            if ( FullExpandinProgress == false )
                ActiveTree . Refresh ( );
            levelscount = CalculateLevel ( items . Tag . ToString ( ) );
            if ( levelscount >= ExpArgs . ExpandLevels )
            {
                ShowExpandTime ( );
                ExpandSetup ( false );
                Expandprogress . Refresh ( );
                return Task . FromResult ( fail );
            }
            //**************
            // Main LOOP
            //**************
            foreach ( TreeViewItem obj in items . Items )
            {
                ShowProgress ( );
                if ( levelscount > ExpArgs . ExpandLevels )
                {
                    continue;
                }
                Selection . Text = $"Expanding {obj . Tag . ToString ( )}";
                TreeViewItem childControl = obj as TreeViewItem;
                // working correctly
                ShowProgress ( );
                if ( CheckSearchSuccess ( childControl . Tag . ToString ( ) ) == true )
                {
                    UpdateListBox ( $"Search for {Searchtext . Text} found  as [" + childControl . Header . ToString ( ) + $"]\nin {childControl . Tag . ToString ( )}" );
                    ScrollCurrentTvItemIntoView ( childControl );
                    childControl . IsSelected = true;
                    ExpArgs . SearchSuccessItem = childControl;
                    //  SearchSuccess = true;
                    ExpArgs . SearchSuccess = true;
                    Returnval = true;
                    IsComplete = true;
                    break;
                }

                try
                {
                    childControl . IsExpanded = true;
                }
                catch ( Exception ex ) { Console . WriteLine ( $"ExpandCurrentAllLevels: 3000 : {ex . Message}" ); }
                //                Console . WriteLine ( $"2 Level={levelscount}, Outer loop {childControl . Tag . ToString ( )}" );
                ShowProgress ( );
                levelscount = CalculateLevel ( childControl . Tag . ToString ( ) );
                if ( levelscount >= ExpArgs . ExpandLevels )
                    continue;

                if ( childControl != null )//&& levels >= 2 )
                {
                    string entry = childControl . Header . ToString ( ) . ToString ( ) . ToUpper ( );
                    itemcount = childControl . Items . Count;

                    ShowProgress ( );
                    if ( FullExpandinProgress == false )
                        ActiveTree . Refresh ( );
                    iterations++;
                    if ( ExpArgs . ExpandLevels >= 3 )
                    {
                        //******************
                        // INNER LOOP
                        //******************
                        foreach ( TreeViewItem nextitem in childControl . Items )
                        {
                            ShowProgress ( );
                            if ( TreeViewCollection . CheckIsVisible ( nextitem . Tag . ToString ( ) . ToUpper ( ) , ShowAllFiles , out HasHidden ) == false )
                            {
                                Console . WriteLine ( $"System file : {nextitem . Tag . ToString ( ) . ToUpper ( )}" );
                                continue;
                            }

                            if ( CheckSearchSuccess ( nextitem . Tag . ToString ( ) ) == true )
                            {
                                UpdateListBox ( $"Search for {Searchtext . Text} found  as [" + nextitem . Header . ToString ( ) + $"]\nin {nextitem . Tag . ToString ( )}" );
                                ScrollCurrentTvItemIntoView ( nextitem );
                                nextitem . IsSelected = true;
                                ExpArgs . SearchSuccessItem = nextitem;
                                //    SearchSuccess = true;
                                ExpArgs . SearchSuccess = true;
                                Returnval = true;
                                break;
                            }

                            try
                            {
                                nextitem . IsExpanded = true;
                            }
                            catch ( Exception ex ) { Console . WriteLine ( $"ExpandCurrentAllLevels: 3046 : {ex . Message}" ); }
                            ShowProgress ( );
                            if ( FullExpandinProgress == false )
                                ActiveTree . Refresh ( );

                            //                            Console . WriteLine ( Selection . Text );
                            levelscount = CalculateLevel ( nextitem . Tag . ToString ( ) );
                            if ( levelscount >= ExpArgs . ExpandLevels )
                            {
                                continue;
                            }
                            UpdateListBox ( nextitem . Tag . ToString ( ) );
                            if ( ExpArgs . ExpandLevels >= 4 )
                            {
                                if ( TreeViewCollection . CheckIsVisible ( nextitem . Tag . ToString ( ) . ToUpper ( ) , ShowAllFiles , out HasHidden ) == false )
                                {
                                    Console . WriteLine ( $"System file : {nextitem . Tag . ToString ( ) . ToUpper ( )}" );
                                    continue;
                                }
                                if ( ExpandAll3 ( nextitem , true , ExpArgs . ExpandLevels ) == true )
                                {
                                    //SearchSuccess = true;
                                    ExpArgs . SearchSuccess = true;
                                    Returnval = true;
                                    IsComplete = true;
                                    break;
                                }
                            }
                            ShowExpandTime ( );
                            ShowProgress ( );
                            if ( FullExpandinProgress == false )
                                ActiveTree . Refresh ( );
                        }   // End INNER FOREACH

                        if ( IsComplete )
                            break;
                    }
                    ShowExpandTime ( );
                }
                if ( FullExpandinProgress == false )
                    ActiveTree . Refresh ( );
                ShowProgress ( );
                if ( IsComplete )
                    break;
            }   // End FOREACH

            ShowExpandTime ( );
            ExpandSetup ( false );
            Expandprogress . Refresh ( );
            Selection . Text = $"{startitem . Tag . ToString ( )} Expanded {ExpArgs . ExpandLevels} levels Successfully...";
            if ( ExpArgs . SearchSuccess == false )
            {
                startitem . IsSelected = true;
                ScrollCurrentTvItemIntoView ( Args [ 0 ] as TreeViewItem );
            }
            if ( Returnval )
                return Task . FromResult ( success );
            else
                return Task . FromResult ( fail );
        }
        private void TestTree_Collapsed ( object sender , RoutedEventArgs e )
        {
            Mouse . OverrideCursor = Cursors . Wait;
            //if ( ActiveTree == TestTree )
            //{
            //Get current Obs item
            TreeView tv = sender as TreeView;
            TreeViewItem item = TestTree . SelectedItem as TreeViewItem;
            if ( item != null  )
            {
                //if ( KeepSubdirs )
                //{
                //    foreach ( TreeViewItem subitem in item.Items )
                //    {
                //        subitem . IsExpanded = false;
                //    }
                //    item . IsExpanded = false;
                //}
                //else
                //{
                //item . Items . Clear ( );
                //item . Items . Add ( "Loading" );
                //}
                tvcollection . SetExpanded ( item , false );
                tvcollection . SetSelected ( item , true );
                item . IsSelected = true;
                item . IsExpanded = false;
            }
            //    item . Items . Clear ( );
            //    item . Items . Add ( "Loading" );
            //    //            ActiveTree . HorizontalContentAlignment = HorizontalAlignment . Left;
            //}

            // Also select it in TreeView itself - not sure why I need to do so ?
            //            TreeViewItem tvi = ActiveTree . Items . CurrentItem as TreeViewItem;
            //            tvi . IsSelected = true;
            Mouse . OverrideCursor = Cursors . Arrow;
        }

        private void Stopthread ( object sender , RoutedEventArgs e )
        {
            //           Dispatcher . BeginInvokeShutdown ( DispatcherPriority . Normal );

        }

        #region BackgroundWorker
        private void worker_DoWork ( object sender , DoWorkEventArgs e )
        {
            int result = 0;
            BackgroundWorker worker = sender as BackgroundWorker;
            object [ ] Args = e . Argument as object [ ];
            Console . WriteLine ( $"Calling BackGroundWorker Progress thread" );
            worker . ReportProgress ( 0 , ExpArgs );
            e . Result = result;
        }
        private void worker_RunWorkerCompleted ( object sender , RunWorkerCompletedEventArgs e )
        {
            if ( e . Cancelled )
            {
            }
        }
        private TreeViewItem GetParentNode ( TreeViewItem currentItem )
        {
            try
            {
                TreeViewItem tv2 = new TreeViewItem ( );
                tv2 = currentItem;
                if ( tv2 == null )
                    return null;
                string test = tv2 . Tag . ToString ( );
                if ( test . Length == 3 )
                    Console . WriteLine ( $"Starting at parent node of {test}.." );
                else
                {
                    tv2 = currentItem . Parent as TreeViewItem;
                    test = tv2 . Tag . ToString ( );
                    if ( test . Length > 3 )
                    {
                        while ( test . Length > 3 )
                        {
                            TreeViewItem tv3 = new TreeViewItem ( );
                            tv3 = tv2 . Parent as TreeViewItem;
                            test = tv3 . Tag . ToString ( );
                            if ( test . Length > 3 )
                                tv2 = tv3;
                            else
                            {
                                ExpArgs . Parent = tv3;
                                tv2 = tv3;
                                break;
                            }
                        }
                    }
                }
                Console . WriteLine ( $"Parent found is {test}" );
                return tv2;
            }
            catch ( Exception ex ) { Console . WriteLine ( $"GetParentNode :3174 : {ex . Message}" ); }
            return null;
        }

        private void worker_ProgressChanged ( object sender , ProgressChangedEventArgs e )
        {
            TreeViewItem tvnew = GetParentNode ( ExpArgs . tvitem );

            //if ( ExpArgs . Selection == 4 )
            //{
            //TreeView tv = ExpArgs . tv as TreeView;
            //TreeViewItem tvi = new TreeViewItem ( );
            //List<string> dirs = new List<string> ( );
            //List<string> files= new List<string> ( );
            //foreach ( TreeViewItem item in tv. Items )
            //{
            //    tvi. Tag = item . Tag;
            //    tvi . Header = item . Header;
            //    if ( item . Items.Count  > 0 )
            //    {
            //        tvi . Items . Clear ( );
            //       tvitemclass.GetDirectories ( tvi . Tag . ToString ( ) , out   dirs);
            //        AddDirectoriesToTestTree ( dirs , tvi , null , false );
            //        GetFiles (tvi.Tag.ToString(), out files );
            //        AddFilesToTreeview ( files , tvi );
            //        if ( tvi . HasItems )
            //        {
            //            ExpArgs . tvitem = tvi;
            //            RunRecurse ( e );
            //            tvi . Refresh ( );
            //            ActiveTree . Refresh ( );
            //        }
            //    }
            //    continue;

            //        tvi . IsExpanded = true;
            //}
            //}
            //else
            StartTimer ( );

            RunRecurse ( ActiveTree , e );

            //// All done;
            Mouse . OverrideCursor = Cursors . Arrow;
            if ( ExpArgs . SearchActive && ExpArgs . SearchSuccess )
                //MessageBox . Show ( $"{ExpArgs . SearchTerm} has been identified" , "TreeView Search facilitiy" );
                fdl . ShowInfo ( Flowdoc , canvas , $"The 1st instance of the Search term shown below has been identified successfully, and is highlighted for you ..." , "Red5" , $"[{ExpArgs . SearchTerm}] " , "Black0" , "Match found !" , "Cyan5" , "TreeView Search Sytem" );
            else if ( ExpArgs . SearchActive )
            {
                if ( Exactmatch == false )
                    fdl . ShowInfo ( Flowdoc , canvas , $"Sorry, but the Search term of [{ExpArgs . SearchTerm}] could not be identified for you in the {ExpArgs . ExpandLevels - 1} Expansion levels below the initial point of [{ExpArgs . tvitem . Tag . ToString ( )}]..." , "Black0" , $"[{ExpArgs . SearchTerm}] " , "Red5" , "NO Match found !" , "Cyan5" , "TreeView Search Sytem" );
                else
                    fdl . ShowInfo ( Flowdoc , canvas , $"Sorry, but an EXACT match to the Search term of [{ExpArgs . SearchTerm}] could not be identified for you in the {ExpArgs . ExpandLevels - 1} Expansion levels below the initial point of [{ExpArgs . tvitem . Tag . ToString ( )}]..." , "Black0" , $"[{ExpArgs . SearchTerm}] " , "Red5" , "NO Match found !" , "Cyan5" , "TreeView Search Sytem" );
            }
            // Called once recurse methods have completed
            //if ( worker . IsBusy == false )
            //{
            //    TreeViewItem tvi = new TreeViewItem ( );
            //    Console . WriteLine ( $"1 Recurse finished" );
            //    try
            //    {
            //        if ( ShowVolumeLabels == true )
            //        {
            //            if ( ExpArgs . Parent != null )
            //                tvi = ExpArgs . Parent;
            //            else
            //                tvi = ExpArgs . tvitem;
            //            TreeViewItem caller = tvi as TreeViewItem;
            //            if ( tvi . Tag . ToString ( ) . Length > 3 )
            //            {
            //                caller = tvi . Parent as TreeViewItem;
            //            }
            //            if ( caller . Tag . ToString ( ) . Length == 3 && caller . Tag . ToString ( ) . Contains ( "\\" ) )
            //            {
            //                string s = GetDriveInfo ( $"{caller . Header . ToString ( )}" );
            //                caller . Header = $"{caller . Header}  [{s}]";
            //            }
            //        }
            //    }
            //    catch ( Exception ex ) { Console . WriteLine ( $"Parent name parsing of {tvi . Tag . ToString ( )} failed :-\n{ex . Message}" ); }
            //}
            ////Tidy up after ourselves
            //  ClearExpandArgs ( );
        }
        public void RunRecurse ( TreeView ActiveTree , ProgressChangedEventArgs e )
        {
            // e contains current treeviewitem !!!
            // All parameters are in ExpandArgs ExpArgs
            bool Returnval = false;
            bool IsComplete = false;
            int iterations = 0;

            int itemcount = 0;
            int levelscount = 0;
            var fail = false;
            var success = true;

            if ( ExpArgs . SearchTerm != null )
                // Force search to be upper case
                ExpArgs . SearchTerm = ExpArgs . SearchTerm . ToUpper ( );
            else
                ExpArgs . SearchTerm = "";
            //ExpandArgs eargs = e . UserState as ExpandArgs;
            //int levels = ( int ) Args [ 1 ];
            int levels = ExpArgs . ExpandLevels;

            TreeViewItem startup = new TreeViewItem ( );
            TreeViewItem items = ExpArgs . tvitem;
            startitem = items;
            fail = false;
            if ( items == null )
                return;
            //ProgressCount = 0;
            if ( items . Tag . ToString ( ) . ToUpper ( ) . Contains ( items . Header . ToString ( ) . ToUpper ( ) ) == false )
                items . Header = items . Tag . ToString ( );
            // Essential to force root to be expanded, else nothing happens
            if ( CheckSearchSuccess ( items . Tag . ToString ( ) ) == true )
            {
                UpdateListBox ( $"Search for {Searchtext . Text} found  as [" + items . Header . ToString ( ) + $"]\nin {items . Tag . ToString ( )}" );
                ScrollCurrentTvItemIntoView ( items );
                ExpArgs . SearchSuccessItem = items;
                items . IsSelected = true;
                //    SearchSuccess = true;
                ExpArgs . SearchSuccess = true;
                return;
            }

            try
            {
                items . IsSelected = true;
                items . IsExpanded = true;
                if ( AbortExpand )
                    return;
                //stack . Push ( items . Tag . ToString ( ) );
            }
            catch ( Exception ex ) { Console . WriteLine ( $"RunRecurse: 3304 : {ex . Message}" ); }
            Thread . Sleep ( SLEEPTIME );
            ShowProgress ( );
            if ( FullExpandinProgress == false )
                items . Refresh ( );
            levelscount = CalculateLevel ( items . Tag . ToString ( ) );
            if ( levelscount >= ExpArgs . ExpandLevels )
            {
                items . IsSelected = true;
                items . IsExpanded = true;
                Console . WriteLine ( $"{items . Header . ToString ( )} Expanded.... ?" );
                ActiveTree . Refresh ( );
                ShowExpandTime ( );
                ExpandSetup ( false );
                Expandprogress . Refresh ( );
                return;
            }
            //**************
            // Main LOOP
            //**************
            UpdateListBox ( items . Tag . ToString ( ) );

            foreach ( var objct in items . Items )
            {
                if ( objct . ToString ( ) == "Loading" )
                    break;
                startup = objct as TreeViewItem;
                //Thread . Sleep ( SLEEPTIME );
                TreeViewItem obj = objct as TreeViewItem;
                ShowProgress ( );
                //levelscount = CalculateLevel ( obj . Tag . ToString ( ) );
                if ( levelscount > ExpArgs . ExpandLevels )
                {
                    continue;
                }
                Selection . Text = $"Expanding {obj . Tag . ToString ( )}";
                TreeViewItem childControl = obj as TreeViewItem;
                // working correctly
                ShowProgress ( );
                if ( CheckSearchSuccess ( childControl . Tag . ToString ( ) ) == true )
                {
                    UpdateListBox ( $"Search for {Searchtext . Text} found  as [" + childControl . Header . ToString ( ) + $"]\nin {childControl . Tag . ToString ( )}" );
                    ScrollCurrentTvItemIntoView ( childControl );
                    childControl . IsSelected = true;
                    ExpArgs . SearchSuccessItem = childControl;
                    ExpArgs . SearchSuccess = true;
                    Returnval = true;
                    IsComplete = true;
                    ActiveTree . Refresh ( );
                    break;
                }

                try
                {
                    childControl . IsExpanded = true;
                    if ( AbortExpand )
                        return;
                }
                catch ( Exception ex ) { Console . WriteLine ( $"RunRecurse: 3361 : {ex . Message}" ); }
                ShowProgress ( );

                //                if ( ClosePreviousNode )
                //                  Console . WriteLine ( $"Closeprevious 1 : {childControl.Tag.ToString()}" );

                levelscount = CalculateLevel ( childControl . Tag . ToString ( ) );
                if ( levelscount >= ExpArgs . ExpandLevels )
                {
                    if ( ExpArgs . SearchTerm != "" && ClosePreviousNode )
                    {
                        childControl . IsExpanded = false;
                        ActiveTree . Refresh ( );
                    }
                    continue;
                }
                if ( childControl != null )//&& ExpArgs . ExpandLevels >= 2 )
                {
                    string entry = childControl . Header . ToString ( ) . ToString ( ) . ToUpper ( );
                    itemcount = childControl . Items . Count;
                    ShowProgress ( );
                    if ( FullExpandinProgress == false )
                        ActiveTree . Refresh ( );
                    iterations++;
                    if ( ExpArgs . ExpandLevels >= 3 )
                    {
                        bool nofault = false;
                        //******************
                        // INNER LOOP
                        //******************
                        //UpdateListBox ( childControl. Tag . ToString ( ) );

                        UpdateListBox ( childControl . Tag . ToString ( ) );
                        foreach ( var newentry in childControl . Items )
                        {
                            TreeViewItem nextitem = new TreeViewItem ( );
                            try
                            {
                                nextitem = newentry as TreeViewItem;
                            }
                            catch ( Exception ex )
                            {
                                Console . WriteLine ( $"Unable to access {childControl . Tag . ToString ( )}" );
                                nofault = true;
                            }
                            if ( nofault || nextitem == null )
                            {
                                nofault = false;
                                continue;
                            }
                            ShowProgress ( );
                            if ( nextitem . Header . ToString ( ) == "Loading" )
                                continue;
                            if ( TreeViewCollection . CheckIsVisible ( nextitem . Tag . ToString ( ) . ToUpper ( ) , ShowAllFiles , out HasHidden ) == false )
                            {
                                Console . WriteLine ( $"System file : {nextitem . Tag . ToString ( ) . ToUpper ( )}" );
                                continue;
                            }
                            UpdateListBox ( nextitem . Tag . ToString ( ) );

                            if ( CheckSearchSuccess ( nextitem . Tag . ToString ( ) ) == true )
                            {
                                UpdateListBox ( $"Search for {Searchtext . Text} found  as [" + nextitem . Header . ToString ( ) + $"]\nin {nextitem . Tag . ToString ( )}" );
                                ScrollCurrentTvItemIntoView ( nextitem );
                                nextitem . IsSelected = true;
                                //SearchSuccess = true;
                                ExpArgs . SearchSuccess = true;
                                ExpArgs . SearchSuccessItem = nextitem;
                                Returnval = true;
                                IsComplete = true;
                                break;
                            }

                            try
                            {
                                iterations++;
                                nextitem . IsExpanded = true;
                                if ( AbortExpand )
                                    return;
                            }
                            catch ( Exception ex ) { Console . WriteLine ( $"RunRecurse: 3424 : {ex . Message}" ); }
                            ShowProgress ( );
                            // working correctly
                            if ( FullExpandinProgress == false )
                                ActiveTree . Refresh ( );
                            //                            Console . WriteLine ( Selection . Text );
                            levelscount = CalculateLevel ( nextitem . Tag . ToString ( ) );
                            if ( levelscount >= ExpArgs . ExpandLevels )
                            {
                                continue;
                            }
                            if ( nextitem . Tag . ToString ( ) . Contains ( "-500" ) )
                                Returnval = Returnval;
                            if ( ExpArgs . ExpandLevels >= 4 )
                            {
                                //                                UpdateListBox ( nextitem . Tag . ToString ( ) );
                                if ( TreeViewCollection . CheckIsVisible ( nextitem . Tag . ToString ( ) . ToUpper ( ) , ShowAllFiles , out HasHidden ) == false )
                                {
                                    Console . WriteLine ( $"System file : {nextitem . Tag . ToString ( ) . ToUpper ( )}" );
                                    continue;
                                }
                                //Thread . Sleep ( SLEEPTIME );
                                if ( nextitem . HasItems )
                                {
                                    if ( ExpandAll3 ( nextitem , true , ExpArgs . ExpandLevels ) == true )
                                    {
                                        ExpArgs . SearchSuccess = true;
                                        Returnval = true;
                                        IsComplete = true;
                                        break;
                                    }
                                    else
                                    {
                                        if ( ClosePreviousNode && ExpArgs . SearchActive == true && ExpArgs . SearchSuccess == false )
                                        {
                                            // ONLY If Searching , Close the subdir we have just finished prcessing
                                            nextitem . IsExpanded = false;
                                            ActiveTree . Refresh ( );
                                        }
                                    }
                                }
                            }
                            else
                                UpdateListBox ( nextitem . Tag . ToString ( ) );

                            ShowExpandTime ( );
                            ShowProgress ( );
                            if ( FullExpandinProgress == false )
                                ActiveTree . Refresh ( );
                        }   // End INNER FOREACH

                        if ( ClosePreviousNode && ExpArgs . SearchActive == true && ExpArgs . SearchSuccess == false )
                        {
                            // ONLY If Searching , Close the subdir we have just finished prcessing
                            childControl . IsExpanded = false;
                            ActiveTree . Refresh ( );
                        }
                        if ( IsComplete )
                            break;
                    }
                    ShowExpandTime ( );
                }
                if ( FullExpandinProgress == false )
                    ActiveTree . Refresh ( );
                ShowProgress ( );
                if ( IsComplete )
                    break;
            }   // End FOREACH

            ShowExpandTime ( );
            ExpandSetup ( false );
            Expandprogress . Refresh ( );
            Selection . Text = $"{startitem . Tag . ToString ( )} Expanded {ExpArgs . ExpandLevels} levels Successfully...";
            if ( ExpArgs . SearchSuccess == false )
            {
                ScrollCurrentTvItemIntoView ( startup );
                startitem . IsSelected = true;
            }
            UpdateDriveHeader ( ShowVolumeLabels );
            return;
        }
        #endregion BackgroundWorker
        public int AddFilesToRecurse ( List<string> Allfiles , TreeViewItem item )
        {
            //    int count = 0;
            //    var subitemctrl = new TreeViewItem ( );
            //    if ( item . Items [ 0 ] . ToString ( ) == "Loading" )
            //        item . Items . Clear ( );
            //    foreach ( var itm in Allfiles )
            //    {
            //        ShowProgress ( );
            //        var subitem = new TreeViewItem ( )
            //        {
            //            Header = GetFileFolderName ( itm ) ,
            //            Tag = itm
            //        };
            //        if ( TreeViewCollection. CheckIsVisible ( itm . ToUpper ( ) , ShowAllFiles , out HasHidden ) == true )
            //        {
            //            item . Items . Add ( subitem );
            //            item . IsExpanded = true;
            //            subitem . IsSelected = true;
            //            ScrollCurrentTvItemIntoView ( subitem );
            //            ActiveTree . Refresh ( );
            //            subitemctrl = subitem;              
            //                                                      //                    Console . WriteLine ( $"3 - ADTR : Added {subitem . Tag . ToString ( )} to {item . Tag . ToString ( )} &  scrolled" );
            //            count++;
            //            if ( CheckSearchSuccess ( subitem . Tag . ToString ( ) ) == true )
            //            {
            //                UpdateListBox ( $"\nSearch for {Searchtext . Text} found  as [" + subitem . Header . ToString ( ) + $"]\nin {subitem . Tag . ToString ( )}" );
            //                if ( subitem . IsSelected == false )
            //                    subitem . IsSelected = true;
            //                ScrollCurrentTvItemIntoView ( subitem );
            //                ActiveTree . Refresh ( );
            //                ExpArgs . SearchSuccessItem = subitem;
            //                ExpArgs . SearchSuccess = true;
            //                Mouse . OverrideCursor = Cursors . Arrow;
            //                break;
            //            }

            //        }
            //        if ( Allfiles . Count > 0 )
            //        {
            //            item . IsExpanded = true;
            //            subitemctrl . IsSelected = true;
            //            ScrollCurrentTvItemIntoView ( subitemctrl );
            //            if ( FullExpandinProgress == false )
            //                ActiveTree . Refresh ( );
            //        }
            //        ShowProgress ( );
            //    }
            return 0;
        }
        private void SearchTree ( object sender , RoutedEventArgs e )
        {
            // This is the Search button  handler
            //SearchSuccess = false;
            ExpArgs . SearchSuccess = false;
            TreeViewItem tvi = new TreeViewItem ( );
            tvi = ActiveTree . SelectedItem as TreeViewItem;
            TreeViewItem rootitem = new TreeViewItem ( );
            TreeViewItem tvfound = new TreeViewItem ( );
            ExpandSetup ( true );
            TextToSearchFor = Searchtext . Text . ToUpper ( );
            if ( TextToSearchFor . ToUpper ( ) == "" || TextToSearchFor . ToUpper ( ) == "SEARCH FOR...." )
            {
                fdl . ShowInfo ( Flowdoc , canvas , "You have not entered a value to be searched for ? " );
                return;
            }
            SearchString = TextToSearchFor;
            // Set it as a default
            rootitem = tvi;
            if ( tvi != null && tvi . HasItems )
            {
                // call main recursive handler
                ExpandSelection = 4;
                maxlevels = 90;

                ClearExpandArgs ( );
                ExpArgs . tvitem = ActiveTree . SelectedItem as TreeViewItem;
                ExpArgs . Selection = 0;
                ExpArgs . ExpandLevels = LevelsCombo . SelectedIndex + 3;
                ExpArgs . SearchTerm = Searchtext . Text . ToUpper ( );
                ExpArgs . ListResults = false;
                ExpArgs . SearchActive = true;
                ExpanderMenuOption . Text = $"Search for Item down up to {ExpArgs . ExpandLevels - 1} levels";

                RunExpandSystem ( null , null );
                //RecurseItem ( ExpArgs . tvitem , ExpArgs . SearchTerm , ExpArgs . IsFullExpand );
            }
            Mouse . OverrideCursor = Cursors . Arrow;
            return;
        }
        private bool RecurseItem ( TreeViewItem tvitem , string SearchTerm , bool ClosePrevious = true )
        {

            /*
              * ExpandArgs
                       public TreeView tv;
                         public               TreeViewItem tvitem;
                         public  int         ExpandLevels;
                         public string     SearchTerm;
                         public bool        ClosePrevious; 
               */
            // Expand our structure to parameters
            //TreeView tv = ExpArgs . tv;
            SearchTerm = ExpArgs . SearchTerm;
            RunRecurseItem ( ExpArgs . tvitem , SearchTerm , ClosePrevious );
            SearchTerm = "";
            ExpArgs . SearchSuccess = false;
            return true;
        }
        // NOT USED
        // refs are internal (recursive) calls only
        private TreeViewItem RunRecurseItem ( TreeViewItem tvitem , string SearchTerm , bool ClosePrevious = true )
        {
            // MAIN SEARCH HANDLER
            // This recurses through all files and folders
            //and Scrolls to and highlights the item Found, if any!
            return null;
        }
        private void Expand_Click ( object sender , RoutedEventArgs e )
        {
            ClearExpandArgs ( );
            ExpArgs . Selection = DirectoryOptions . SelectedIndex;
            if ( ExpArgs . Selection == 3 )
                ExpArgs . Selection = 90;
            ExpArgs . ExpandLevels = Convert . ToInt16 ( LevelsCombo . SelectedItem );
            if ( ExpArgs . Selection == 0 )
                ExpArgs . ExpandLevels = 2;
            if ( ExpArgs . Selection == 2 )
                ExpArgs . ExpandLevels = 3;
            ExpArgs . tvitem = ActiveTree . SelectedItem as TreeViewItem;
            RunExpandSystem ( sender , e );
            //            ExpandSetup ( false );
        }

        /// <summary>
        /// Process Expannd request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool RunExpandSystem ( object sender , RoutedEventArgs e )
        {
            double temp = 0;
            ComboBox cb = DirectoryOptions;
            int selindex = ExpArgs . Selection;
            string original = "";
            //            int iterations = 0;
            listboxtotal = 0;
            TreeViewItem root = ActiveTree . SelectedItem as TreeViewItem;
            TreeViewItem caller = ActiveTree . SelectedItem as TreeViewItem;
            if ( caller == null )
                caller = root;
            if ( root != null )
            {
                if ( root . HasItems == false && ExpArgs . IsFullExpand == false )
                {
                    Mouse . OverrideCursor = Cursors . Arrow;
                    MessageBox . Show ( "The current item is only a file (or contains only Hidden items)\nand cannot threfore be expanded.\n\nPlease select a Valid Drive or Folder in the TreeView before using these options...." ,
                        "Invalid Current Selection" );
                    return false;
                }
                Args [ 0 ] = root as object;
                original = root . Header . ToString ( );
            }
            else
            {
                if ( Args [ 0 ] != null )
                    root = Args [ 0 ] as TreeViewItem;
            }

            // Prepare Arguments
            ExpArgs . tv = ActiveTree;
            ExpArgs . tvitem = root;
            ExpArgs . Selection = selindex;

            InfoList . ItemsSource = null;

            Console . WriteLine ( $"User Selection in use = {ExpArgs . Selection }" );
            if ( ExpArgs . tvitem == null )
            {
                Mouse . OverrideCursor = Cursors . Arrow;
                fdl . ShowInfo ( Flowdoc , canvas ,
                      $"Please select a drive or subfolder before using  this option...." ,
                      "Blue1" ,
                      "TreeView Search Sytem" );
                BusyLabel . Visibility = Visibility . Hidden;
                return false;
            }

            if ( selindex == 0 )   // Expand  2  levels
            {
                if ( ExpArgs . ExpandLevels == 0 )
                    ExpArgs . ExpandLevels = 2;
                //else
                //    ExpArgs . ExpandLevels = 1;

                UpdateListBox ( $"Expanding {original} {ExpArgs . ExpandLevels} levels...." );
                TestTree_Expanded ( sender , null );
                //ActiveTree .HorizontalAlignment = HorizontalAlignment . Left;
            }
            else if ( selindex == 1 )    // Expand 3 levels
            {
                if ( ExpArgs . ExpandLevels == 0 )
                    ExpArgs . ExpandLevels = 3;
                UpdateListBox ( $"Expanding {original} {ExpArgs . ExpandLevels } levels...." );
            }
            else if ( selindex == 2 )    // Expand 4 levels
            {

                //                if ( ExpArgs . ExpandLevels == 0 )
                ExpArgs . ExpandLevels = 4;
                TreeViewItem tvi = ( TreeViewItem ) ExpArgs . tvitem;
                string str = tvi . Tag . ToString ( );
                if ( ExpArgs . SearchActive == false )
                {
                    if ( MessageBox . Show ( $"Expanding {str} down {ExpArgs . ExpandLevels } levels may take some time, Are you  sure you want  to continue ?" , "Expansion System Warning !" ,
                    MessageBoxButton . YesNo ) == MessageBoxResult . No )
                    {
                        Mouse . OverrideCursor = Cursors . Arrow;
                        BusyLabel . Visibility = Visibility . Hidden;
                        return false;
                    }
                }
                UpdateListBox ( $"Expanding {original} {Args [ 1 ]} levels...." );
                ExpArgs . ListResults = LISTRESULTS;
            }
            else if ( selindex == 3 )
            {
                if ( ExpArgs . ExpandLevels == 0 )
                    ExpArgs . ExpandLevels = 90;
                UpdateListBox ( $"Expanding {original} {Args [ 1 ]} levels...." );
                ExpArgs . ListResults = LISTRESULTS;
            }
            else if ( selindex == 4 )
            {
                // Expand All Drives 1 level 
                ExpArgs . tv = ActiveTree;
                ExpArgs . Selection = 0;
                ExpArgs . ExpandLevels = 1;
                ExpandSetup ( true );
                foreach ( TreeViewItem item in ActiveTree . Items )
                {
                    TreeViewItem tvi = new TreeViewItem ( );
                    tvi = item;
                    tvi . IsSelected = true;
                    ExpArgs . tvitem = tvi;
                    ExpArgs . IsFullExpand = true;
                    TriggerExpand0 ( sender , e );
                    Thread . Sleep ( 10 );
                }
                ExpandSetup ( false );
                ScrollCurrentTvItemIntoView ( ( TreeViewItem ) ActiveTree . Items [ 0 ] );
                return true;
            }
            else if ( selindex == 5 )
            {
                // Expand All Drives 2 levels 
                ExpArgs . tv = ActiveTree;
                ExpArgs . Selection = 0;
                ExpArgs . ExpandLevels = 2;
                ExpandSetup ( true );
                foreach ( TreeViewItem item in ActiveTree . Items )
                {
                    TreeViewItem tvi = new TreeViewItem ( );
                    tvi = item;
                    tvi . IsSelected = true;
                    ExpArgs . tvitem = tvi;
                    ExpArgs . IsFullExpand = true;
                    ExpArgs . ExpandLevels = 2;
                    TriggerExpand0 ( sender , e );
                    Thread . Sleep ( 10 );
                }
                ExpandSetup ( false );
                ScrollCurrentTvItemIntoView ( ( TreeViewItem ) ActiveTree . Items [ 0 ] );
                return true;
            }
            else if ( selindex == 6 )
            {
                bool IterateGo = false;
                // Expand ALL below  current drive
                ExpArgs . tvitem = ActiveTree . SelectedItem as TreeViewItem;
                if ( ExpArgs . ExpandLevels == 0 )
                    ExpArgs . ExpandLevels = 90;
                // inhibit listbox
                ExpArgs . ListResults = LISTRESULTS;
                UpdateListBox ( $"Expanding ALL items BELOW current...." );
                foreach ( TreeViewItem item in ActiveTree . Items )
                {
                    if ( IterateGo || item . Header . ToString ( ) == ExpArgs . tvitem . Header . ToString ( ) )
                    {
                        ExpArgs . tvitem = item;
                        if ( ExpandCurrentAllLevels ( Args ) == true && TextToSearchFor != "" )
                            MessageBox . Show ( $"[{Searchtext . Text}] FOUND ...." , "Search System" );
                        IterateGo = true;
                    }
                }
                ExpandSetup ( false );
                ScrollCurrentTvItemIntoView ( ( TreeViewItem ) Args [ 0 ] );
                return true;
            }
            else if ( selindex == 7 )
            {
                // Collapse All Drives
                ExpArgs . tv = ActiveTree;
                CollapseAllDrives ( );
                Mouse . OverrideCursor = Cursors . Arrow;
                ExpandSetup ( false );
                ScrollCurrentTvItemIntoView ( ( TreeViewItem ) ActiveTree . Items [ 0 ] );
                return true;
            }

            // go ahead
            TreeViewItem tview = new TreeViewItem ( );
            tview = ExpArgs . tvitem;
            tview . IsSelected = true;
            ActiveTree . Refresh ( );
            if ( ExpArgs . IsFullExpand )
                worker_ProgressChanged ( sender , null );
            else
            {
                worker = new BackgroundWorker ( );
                worker . WorkerSupportsCancellation = true;
                worker . WorkerReportsProgress = true;
                worker . DoWork += worker_DoWork;
                worker . ProgressChanged += worker_ProgressChanged;
                worker . RunWorkerCompleted += worker_RunWorkerCompleted;
                worker . RunWorkerAsync ( ExpArgs );
            }
            try
            {
                TreeViewItem tv2 = new TreeViewItem ( );
                tv2 = tview;
                string test = tv2 . Tag . ToString ( );
                if ( test . Length == 3 )
                    Console . WriteLine ( $"Starting at parent node of {test}.." );
                else
                {
                    tv2 = tview . Parent as TreeViewItem;
                    test = tv2 . Tag . ToString ( );
                    if ( test . Length > 3 )
                    {
                        while ( test . Length > 3 )
                        {
                            TreeViewItem tv3 = new TreeViewItem ( );
                            tv3 = tv2 . Parent as TreeViewItem;
                            test = tv3 . Tag . ToString ( );
                            if ( test . Length > 3 )
                                tv2 = tv3;
                            else
                            {
                                ExpArgs . Parent = tv3;
                                break;
                            }
                        }
                    }
                }
                Console . WriteLine ( $"Parent is {test}" );
            }
            catch ( Exception ex ) { Console . WriteLine ( $"RunRecurse: 4020 : {ex . Message}" ); }
            return true;
        }
        public static int iterations { get; set; } = 0;

        public int GetCurrentLevel ( string currentpath )
        {
            int count = 0;
            string [ ] paths = currentpath . Split ( '\\' );
            count = paths . Length - 1;
            return count;
        }
        private void TestTree_Expanded ( object sender , RoutedEventArgs e )
        {
            // All working when clicking on any folder !!!!
            // this gets callled iteratively  as it progress down a tree of subdirectories
            string currentHeader = "";
            int currentlevel = 0;
            BusyLabel . Text = "Busy  ...";
            //tvcollection . FindEntry ( );
            TreeViewItem Caller = ( TreeViewItem ) TestTree . SelectedItem;
            if ( Caller == null )
                return;
            if ( Expandprogress . Text == "Finished ..." || Expandprogress . Text == "" )
                StartTimer ( );
            //            var exp = TestTree.FindResource ( "Expander" );
            tvitemclass . ExpandTreeViewItem ( Caller , Mouseovertvitem );
            // Needed to let us show the volume label if the option is checked
            //TreeViewItem Caller = new TreeViewItem ( );
            //TreeViewItem item = null;
            //int itemscount = 0;
            //if ( e != null )
            //    item = e . Source as TreeViewItem;
            //else
            //    item = sender as TreeViewItem;
            //if ( item == null )
            //{
            //    iterations = 0;
            //    BusyLabel . Text = "";
            //    return;
            //}
            //if ( item . Header . ToString ( ) == "Loading" )
            //{
            //    Caller . Header = currentHeader;
            //    item . Header = currentHeader;
            //    iterations = 0;
            //    BusyLabel . Text = "";
            //    return;
            //}
            //if ( item . Items . Count == 0 )
            //{
            //    // if ( item . Items [ 0 ] == "Loading" )
            //    item . IsExpanded = false;
            //    iterations = 0;
            //    BusyLabel . Text = "";
            //    return;
            //}
            // Caller = item;
            //currentlevel = GetCurrentLevel ( item . Tag . ToString ( ) );
            //currentHeader = item . Header . ToString ( );
            //             Console . WriteLine ( $"Level = {currentlevel} : {item . Header}  ||   {item . Tag}" );


            //REINSTATE LATER 

            //TreeViewItem current = new TreeViewItem ( );
            //current = TestTree . SelectedItem as TreeViewItem;
            //if ( current == null )
            //    current = tvitemclass . SelectedItem;
            //if ( current == null )
            //    return;
            //tvitemclass . SelectedItem = current;
            //tvitemclass . ExpandTreeViewItem ( current );
            ////TestTree . Items . Clear ( );
            TestTree . ItemsSource = null;
            TestTree . ItemsSource = TvItems;

            //item . IsSelected = true;
            //Selection . Text = $"{item . Tag . ToString ( )}";
            //  ScrollCurrentTvItemIntoView ( item );
            //ActiveTree . Refresh ( );

            //var directories = new List<string> ( );
            //var Allfiles = new List<string> ( );
            //string Fullpath = item . Tag . ToString ( ) . ToUpper ( );
            //int DirectoryCount = 0, filescount = 0;
            //itemscount = item . Items . Count;
            //if ( itemscount == 0 )
            //{
            //    iterations = 0;
            //    BusyLabel . Text = "";
            //    return;
            //}
            //var tvi = item as TreeViewItem;
            // Caller . Header = currentHeader;
            // var itemheader = item . Items [ 0 ] . ToString ( );
            ////  UpdateListBox ( $"{item . Tag . ToString ( )}" );
            //// Get a list of all items in the current folder
            //int dircount = tvitemclass . GetDirectoryCount ( Fullpath );
            //if ( dircount > 0 )
            //{
            //    int count = tvitemclass . GetDirectories ( Fullpath , out directories );
            //    if ( count > 250 )
            //    {
            //        MessageBoxResult result = MessageBox . Show ( $"Directory {Fullpath} contains {count} Files\nExpanding these will take a considerable time...\n\nAre you sure you want to continue ?" ,
            //         "Potential long delay" , MessageBoxButton . YesNoCancel , MessageBoxImage . Warning , MessageBoxResult . Cancel );
            //        if ( result == MessageBoxResult . Yes )
            //        {
            //            // Remove DUMMY entry
            //            if ( itemheader != null && itemheader == "Loading" )
            //                item . Items . Clear ( );
            //            DirectoryCount = count;
            //            ShowProgress ( );
            //            DirectoryCount = AddDirectoriesToTestTreeview ( directories , item , listBox );
            //        }
            //        else if ( result == MessageBoxResult . Cancel )
            //        {
            //            AbortExpand = true;
            //            {
            //                Caller . Header = currentHeader;
            //                iterations = 0;
            //                BusyLabel . Text = "";
            //                return;
            //            }
            //        }
            //        else
            //        {
            //            ExpandLimited = true;
            //            {
            //                Caller . Header = currentHeader;
            //                iterations = 0;
            //                BusyLabel . Text = "";
            //                return;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        DirectoryCount = count;
            //        ShowProgress ( );
            //        if ( directories . Count > 0 )
            //        {
            //            if ( item . Items [ 0 ] . ToString ( ) == "Loading" )
            //            {
            //                item . Items . Clear ( );
            //            }
            //            iterations++;
            //            item . IsExpanded = true;
            //              ScrollCurrentTvItemIntoView ( item );
            //            ActiveTree . Refresh ( );
            //            DirectoryCount = AddDirectoriesToTestTree ( directories , item , listBox );
            //          }
            //    }
            //}
            //else
            //{
            //    DirectoryCount = 0;
            //    ShowProgress ( );
            //}
            //// Now Get FILES

            //if ( tvitemclass . GetFilesCount ( Fullpath ) > 0 )
            //{
            //    tvitemclass . GetFiles ( Fullpath , out Allfiles );
            //    filescount = Allfiles . Count;
            //    if ( filescount > 500 )
            //    {
            //        MessageBoxResult result = MessageBox . Show ( $"Directory {Fullpath} contains {filescount} Files\nExpanding these will take a considerable time...\n\nAre you sure you want to expand  thiis  subdirectory?\n\n(Cancel to stop the entire Expansion immediately)" ,
            //         "Potential long delay" , MessageBoxButton . YesNoCancel , MessageBoxImage . Warning , MessageBoxResult . Cancel );
            //        if ( result == MessageBoxResult . Yes )
            //        {
            //            if ( item . Items [ 0 ] . ToString ( ) == "Loading" )
            //            {
            //                item . Items . Clear ( );
            //            }
            //            AddFilesToTreeview ( Allfiles , item );
            //        }
            //        else if ( result == MessageBoxResult . Cancel )
            //        {
            //            AbortExpand = true;
            //            {
            //                Caller . Header = currentHeader;
            //                iterations = 0;
            //                BusyLabel . Text = "";
            //                return;
            //            }
            //        }
            //        else
            //        {
            //            ExpandLimited = true;
            //            {
            //                Caller . Header = currentHeader;
            //                iterations = 0;
            //                BusyLabel . Text = "";
            //                return;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if ( item . Items [ 0 ] . ToString ( ) == "Loading" )
            //        {
            //            item . Items . Clear ( );
            //        }
            //        AddFilesToTreeview ( Allfiles , item );
            //         ScrollCurrentTvItemIntoView ( item );
            //        ActiveTree . Refresh ( );
            //    }
            //}

            //if ( DirectoryCount == 0 && Allfiles . Count == 0 )
            //{
            //    if ( item . Items [ 0 ] . ToString ( ) == "Loading" )
            //    {
            //        item . Items . Clear ( );
            //        item . IsExpanded = false;
            //        item . IsSelected = true;
            //        ActiveTree . Refresh ( );
            //        ShowProgress ( );
            //    }
            //    if ( ExceptionMessage != "" )
            //    {
            //        Selection . Text = ExceptionMessage;
            //        ExceptionMessage = "";
            //    }
            //    else
            //    {
            //        Selection . Text = "This Subdirectory does not contain any Non System or Hidden files, or perhaps Access is denied by Windows ...";
            //    }
            //}
            //else
            //{
            //    iterations++;
            //     ActiveTree . UpdateLayout ( );
            //    ActiveTree . Refresh ( );
            //    Selection . Text = $"{item . Header . ToString ( )} SubDirectories = {DirectoryCount} , Files = {Allfiles . Count}";
            //}

            //Caller . Header = currentHeader;
            ShowProgress ( );
            //ExpandSetup ( false );
            iterations = 0;
            //            ActiveTree . HorizontalContentAlignment = HorizontalAlignment . Left;

            //BusyLabel . Text = "";
            return;
        }
        private void FlashInfopanel ( string text )
        {
            Selection . Text = "";
            Selection . UpdateLayout ( );
            Thread . Sleep ( 500 );
            Selection . Text = text;
            Selection . UpdateLayout ( );
            Thread . Sleep ( 500 );
            Selection . Text = "";
            Selection . UpdateLayout ( );
            Thread . Sleep ( 300 );
            Selection . Text = text; ;
            Selection . UpdateLayout ( );
        }
        private void ClearExpandArgs ( )
        {
            ExpArgs . tv = ActiveTree;
            ExpArgs . tvitem = null;
            ExpArgs . ExpandLevels = 0;
            ExpArgs . SearchTerm = "";
            ExpArgs . SearchActive = false;
            ExpArgs . Selection = 7;    // default to collapse
            ExpArgs . SearchSuccess = false;
            ExpArgs . MaxItems = 250;
            ExpArgs . ListResults = LISTRESULTS;
            ExpArgs . IsFullExpand = false;
            ExpArgs . Parent = null;
        }
        private void TreeOptions ( object sender , RoutedEventArgs e )
        {
            OptionsPanel . Visibility = Visibility . Visible;
            RefreshOptions ( );
        }
        private void Image_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
        {
            ClosePreviousNode = ( bool ) Opt1cbox . IsChecked;
            LISTRESULTS = ( bool ) Opt2cbox . IsChecked;
            Exactmatch = ( bool ) Opt4cbox . IsChecked;
            ShowVolumeLabels = ( bool ) Opt3cbox . IsChecked;
            ShowAllFiles = ( bool ) Opt5cbox . IsChecked;
            OptionsPanel . Visibility = Visibility . Hidden;
        }
        private string GetDriveInfo ( string arg )
        {
            string str = "";
            DriveInfo di = new DriveInfo ( arg );
            str = di . VolumeLabel;
            return str;
        }
        private void LevelsCombo_Selected ( object sender , SelectionChangedEventArgs e )
        {
            ExpArgs . ExpandLevels = LevelsCombo . SelectedIndex;
        }
        private void ShowallVolumes_Click ( object sender , RoutedEventArgs e )
        {
            ShowAllFiles = !ShowAllFiles;
        }
        private void MenuItem_Click ( object sender , RoutedEventArgs e )
        {
            OptionsPanel . Visibility = Visibility . Visible;
            RefreshOptions ( );
        }
        private void CollapseAll ( object sender , RoutedEventArgs e )
        {
            CollapseAllDrives ( );
        }
        private void CollapseCurrent ( object sender , RoutedEventArgs e )
        {
            TreeViewItem tv = sender as TreeViewItem;
            tv = ActiveTree . SelectedItem as TreeViewItem;
            tv = GetParentNode ( tv );
            if ( tv != null )
                tv . IsExpanded = false;
            //            ActiveTree . HorizontalContentAlignment = HorizontalAlignment . Left;
        }
        private void Close_Click ( object sender , RoutedEventArgs e )
        {
            this . Close ( );
        }

        private void CloseApp_Click ( object sender , RoutedEventArgs e )
        {
            Application . Current . Shutdown ( );
        }

        private void TreviewOptions_Click ( object sender , RoutedEventArgs e )
        {
            if ( OptionsPanel . Visibility == Visibility . Hidden )
                OptionsPanel . Visibility = Visibility . Visible;
            else
                OptionsPanel . Visibility = Visibility . Hidden;
            RefreshOptions ( );
        }

        private void searchcurrent_Click ( object sender , RoutedEventArgs e )
        {
            if ( Searchtext . Text == "" || Searchtext . Text == "Search for...." )
            {
                fdl . ShowInfo ( Flowdoc , canvas , "You need to enter a search term in the Search box below before using this option." , "Red0" );
                return;
            }
            TreeViewItem tv = new TreeViewItem ( );
            tv = ActiveTree . SelectedItem as TreeViewItem;
            if ( tv == null )
            {
                fdl . ShowInfo ( Flowdoc , canvas , "You need to select an item in the TreeView before using this option ..." , "Red5" );
                return;
            }
            tv . IsSelected = true;
            ExpArgs . tvitem = tv;
            ExpArgs . SearchActive = true;
            //ExpArgs . SearchTerm = Searchtext . Text . ToUpper ( );
            //ExpArgs . SearchSuccess = false;
            //ExpArgs .ExpandLevels = Convert.ToInt16(LevelsCombo.SelectedItem) ;
            SearchTree ( sender , e );
        }

        private void searchdrive_Click ( object sender , RoutedEventArgs e )
        {

            TreeViewItem tv = new TreeViewItem ( );
            tv = ActiveTree . SelectedItem as TreeViewItem;
            if ( tv != null )
            {
                tv = GetParentNode ( tv );
                tv . IsSelected = true;
                ExpArgs . tvitem = tv;
                ExpArgs . SearchActive = true;
                SearchTree ( sender , e );
            }
            else
            {
                fdl . ShowInfo ( Flowdoc , canvas , "You need to select an item in the TreeView before using this option ..." , "Red5" );
                return;
            }
        }

        private void CboxExactMatch_Click ( object sender , RoutedEventArgs e )
        {
            if ( ( bool ) CboxExactMatch . IsChecked == false )
                Exactmatch = ( bool ) true;
            else
                Exactmatch = ( bool ) false;
            CboxExactMatch . IsChecked = Exactmatch;
            CboxExactMatch . Refresh ( );
            this . Refresh ( );
            RefreshOptions ( );
        }
        private void ExactMatch_Click ( object sender , RoutedEventArgs e )
        {
            //called by main menu option
            if ( ( bool ) CboxExactMatch . IsChecked == false )
                Exactmatch = ( bool ) true;
            else
                Exactmatch = ( bool ) false;
            CboxExactMatch . IsChecked = Exactmatch;
            Opt4cbox_Click ( sender , e );
            this . Refresh ( );
            RefreshOptions ( );
        }

        private void Showlog_Click ( object sender , RoutedEventArgs e )
        {
            LISTRESULTS = !LISTRESULTS;
            UseListBox . IsChecked = LISTRESULTS;
            Opt2cbox_Click ( sender , e );
            this . Refresh ( );
            RefreshOptions ( );
        }

        private void Closenodes_Click ( object sender , RoutedEventArgs e )
        {
            ClosePreviousNode = !ClosePreviousNode;
            Opt1cbox . IsChecked = ClosePreviousNode;
            Opt1cbox_Click ( sender , e );
            this . Refresh ( );

            RefreshOptions ( );
        }
        private void ShowVolumes_Click ( object sender , RoutedEventArgs e )
        {
            ShowVolumeLabels = !ShowVolumeLabels;
            Opt3cbox . IsChecked = ShowVolumeLabels;
            ShowVolumes . IsChecked = ShowVolumeLabels;
            Opt3cbox_Click ( sender , e );
            this . Refresh ( );
            RefreshOptions ( );
            ShowallVolumes_Click ( sender , e );
        }
        private void ShowHidden_Click ( object sender , RoutedEventArgs e )
        {
            ShowAllFiles = !ShowAllFiles;
            Opt5cbox . IsChecked = ShowAllFiles;
            showallFiles . IsChecked = ShowAllFiles;
            Opt5cbox_Click ( sender , e );
            this . Refresh ( );
            RefreshOptions ( );
            ActiveTree . ItemsSource = tvcollection . LoadDrives ( "" );
        }
        private void MainTVMenu_MouseDown ( object sender , MouseButtonEventArgs e )
        {
            MenuItem_GotFocus ( sender , e );
        }

        private void MenuItem_GotFocus ( object sender , RoutedEventArgs e )
        {
            if ( ClosePreviousNode )
                Closenodes . Header = "Do NOT close Searched Nodes if no match";
            else
                Closenodes . Header = "Close Searched Nodes if no match";

            if ( LISTRESULTS )
                showlog . Header = "Do NOT log Search/Expand operations";
            else
                showlog . Header = "Log all Search/Expand operations";

            if ( Exactmatch == false )
                exactmatching . Header = "SEARCH : Require EXACT (full) Match for Success";
            else
                exactmatching . Header = "SEARCH : Allow Partial Matches for Success";

            if ( ShowVolumeLabels == true )
                showVolumes . Header = "Do NOT show Volume labels";
            else
                showVolumes . Header = "Show Volume labels";

            if ( ShowAllFiles == true )
                Showhidden . Header = "Do NOT show Hidden/System files";
            else
                Showhidden . Header = "Show Hidden/System files";
            this . Refresh ( );
            RefreshOptions ( );
        }
        private void RefreshOptions ( )
        {
            if ( OptionsPanel . Visibility == Visibility . Visible )
            {
                Opt1cbox . IsChecked = ClosePreviousNode;
                Opt1cbox . Foreground = ClosePreviousNode ? FindResource ( "Green3" ) as SolidColorBrush : FindResource ( "Red3" ) as SolidColorBrush;
                Opt2cbox . IsChecked = LISTRESULTS;
                Opt2cbox . Foreground = LISTRESULTS ? FindResource ( "Green3" ) as SolidColorBrush : FindResource ( "Red3" ) as SolidColorBrush;
                Opt3cbox . IsChecked = ShowVolumeLabels;
                Opt3cbox . Foreground = ShowVolumeLabels ? FindResource ( "Green3" ) as SolidColorBrush : FindResource ( "Red3" ) as SolidColorBrush;
                Opt4cbox . IsChecked = Exactmatch;
                Opt4cbox . Foreground = Exactmatch ? FindResource ( "Green3" ) as SolidColorBrush : FindResource ( "Red3" ) as SolidColorBrush;
                Opt5cbox . IsChecked = ShowAllFiles;
                Opt5cbox . Foreground = ShowAllFiles ? FindResource ( "Green3" ) as SolidColorBrush : FindResource ( "Red3" ) as SolidColorBrush;
                OptionsPanel . Refresh ( );
            }
        }

        private void TestTree_MouseEnter ( object sender , MouseEventArgs e )
        {
            Console . WriteLine ( $"MouseEnter : {ActiveTree . SelectedItem . ToString ( )}" );
            //ActiveTree . Refresh ( );
        }

        private void TREEViews_IsMouseDirectlyOverChanged ( object sender , DependencyPropertyChangedEventArgs e )
        {
            //  ActiveTree . Refresh ( );
        }

        private void TREEViews_MouseEnter ( object sender , MouseEventArgs e )
        {
            //ActiveTree . Refresh ( );

        }

        private void TREEViews_MouseMove ( object sender , MouseEventArgs e )
        {
            ActiveTree . Refresh ( );
        }
        private void Opt1cbox_Click ( object sender , RoutedEventArgs e )
        {
            ClosePreviousNode = ( bool ) Opt1cbox . IsChecked;
            Opt1cbox . Content = ClosePreviousNode ? "Yes" : "No";
            if ( ClosePreviousNode )
                Opt1cbox . Foreground = FindResource ( "Green3" ) as SolidColorBrush;
            else
                Opt1cbox . Foreground = FindResource ( "Red3" ) as SolidColorBrush;
            if ( LISTRESULTS && ClosePreviousNode )
            {
                Selection . Text = "Previous Nodes will be closed during Searching....";
            }
            else
            {
                Selection . Text = "Previous Nodes will NOT be closed during Searching....";
            }
            OptionsPanel . Refresh ( );
        }

        private void Opt2cbox_Click ( object sender , RoutedEventArgs e )
        {
            LISTRESULTS = ( bool ) Opt2cbox . IsChecked;
            Opt2cbox . Content = LISTRESULTS ? "Yes" : "No";
            if ( LISTRESULTS )
                Opt2cbox . Foreground = FindResource ( "Green3" ) as SolidColorBrush;
            else
                Opt2cbox . Foreground = FindResource ( "Red3" ) as SolidColorBrush;
            if ( LISTRESULTS )
            {
                Selection . Text = "Search/Expansion information will be listed ....";
                listBox . Items . Clear ( );
                listBox . Items . Add ( "Logging of Expansion and Search\nactivity is current ENABLED." );
                CurrentFolder . Text = "Information / Log  Panel : ENABLED";
            }
            else
            {
                Selection . Text = "Listing Search/Expansion is Disabled ...";
                listBox . Items . Clear ( );
                listBox . Items . Add ( "Logging of Expansion and Search\nactivity is current DISABLED." );
                CurrentFolder . Text = "Information / Log  Panel : DISABLED";
            }
            OptionsPanel . Refresh ( );
        }

        private void Opt3cbox_Click ( object sender , RoutedEventArgs e )
        {
            ShowVolumeLabels = ( bool ) Opt3cbox . IsChecked;
            Opt3cbox . Content = ShowVolumeLabels ? "Yes" : "No";
            if ( ShowVolumeLabels )
                Opt3cbox . Foreground = FindResource ( "Green3" ) as SolidColorBrush;
            else
                Opt3cbox . Foreground = FindResource ( "Red3" ) as SolidColorBrush;
            if ( LISTRESULTS && ShowVolumeLabels )
            {
                Selection . Text = "Volume labels are being shown ....";
            }
            else
            {
                Selection . Text = "Volume labels are not being shown ...";
            }
            //LoadDrives ( ActiveTree );
            UpdateDriveHeader ( ShowVolumeLabels );
            OptionsPanel . Refresh ( );
        }
        private void UpdateDriveHeader ( bool ShowVolumeLabels )
        {
            string drivestring = "";
            string dictvalue = "";
            foreach ( TreeViewItem item in ActiveTree . Items )
            {
                drivestring = item . Tag . ToString ( );
                if ( ShowVolumeLabels )
                    item . Header = drivestring + "  " + Utils . GetDictionaryEntry ( VolumeLabelsDict , drivestring , out dictvalue );
                else
                    item . Header = drivestring;
            }
            ActiveTree . Refresh ( );
        }
        private void Opt4cbox_Click ( object sender , RoutedEventArgs e )
        {
            Exactmatch = ( bool ) Opt4cbox . IsChecked;
            Opt4cbox . Content = Exactmatch ? "Yes" : "No";
            if ( Exactmatch )
                Opt4cbox . Foreground = FindResource ( "Green3" ) as SolidColorBrush;
            else
                Opt4cbox . Foreground = FindResource ( "Red3" ) as SolidColorBrush;
            if ( LISTRESULTS && Exactmatch )
            {
                Selection . Text = "Searching will use EXACT matching....";
            }
            else
            {
                Selection . Text = "Searching will use Partial matching....";
            }
            OptionsPanel . Refresh ( );
        }

        private void Opt5cbox_Click ( object sender , RoutedEventArgs e )
        {
            ShowAllFiles = ( bool ) Opt5cbox . IsChecked;
            Opt5cbox . Content = ShowAllFiles ? "Yes" : "No";
            if ( ShowAllFiles )
                Opt5cbox . Foreground = FindResource ( "Green3" ) as SolidColorBrush;
            else
                Opt5cbox . Foreground = FindResource ( "Red3" ) as SolidColorBrush;
            if ( LISTRESULTS && ShowAllFiles )
            {
                Selection . Text = "Hidden/System files will be shown....";
            }
            else
            {
                Selection . Text = "Hidden/System files will NOT be shown....";
            }
            VolumeLabelsDict . Clear ( );
            ActiveTree . ItemsSource = tvcollection . LoadDrives ( "" );
            OptionsPanel . Refresh ( );
        }
        private void Opt1cbox_Click ( object sender , MouseButtonEventArgs e )
        {
            Opt1cbox . IsChecked = !Opt1cbox . IsChecked;
            Opt1cbox_Click ( sender , new RoutedEventArgs ( null ) );
        }
        private void Opt2cbox_Click ( object sender , MouseButtonEventArgs e )
        {
            Opt2cbox . IsChecked = !Opt2cbox . IsChecked;
            Opt2cbox_Click ( sender , new RoutedEventArgs ( null ) );
        }
        private void Opt3cbox_Click ( object sender , MouseButtonEventArgs e )
        {
            Opt3cbox . IsChecked = !Opt3cbox . IsChecked;
            Opt3cbox_Click ( sender , new RoutedEventArgs ( null ) );
        }
        private void Opt4cbox_Click ( object sender , MouseButtonEventArgs e )
        {
            Opt4cbox . IsChecked = !Opt4cbox . IsChecked;
            Opt4cbox_Click ( sender , new RoutedEventArgs ( null ) );
        }
        private void Opt5cbox_Click ( object sender , MouseButtonEventArgs e )
        {
            Opt5cbox . IsChecked = !Opt5cbox . IsChecked;
            Opt5cbox_Click ( sender , new RoutedEventArgs ( null ) );
        }

        private void ExpandNode_Click ( object sender , RoutedEventArgs e )
        {
            TreeViewItem tv = new TreeViewItem ( );
            tv = ActiveTree . SelectedItem as TreeViewItem;
            tv . IsExpanded = !tv . IsExpanded;
        }

        private void TestTree_MouseDoubleClick ( object sender , MouseButtonEventArgs e )
        {
            // Handle dbl click on file type entries correctly
            return;
            bool result = false;
            TreeViewItem tvi = ActiveTree . SelectedItem as TreeViewItem;
            if ( tvi == null )
                return;
            if ( tvi . IsSelected )
            {
                tvitemclass . ExpandTreeViewItem ( tvi , Mouseovertvitem );
                return;
            }
            else
            {
                if ( tvi . Items . Count > 1 )
                    return;
                tvitemclass . ExpandTreeViewItem ( tvi , Mouseovertvitem );
            }
            //if ( tvi == null )
            //    return;
            //int files = tvitemclass . GetFilesCount ( tvi . Tag . ToString ( ) );
            //int dirs = tvitemclass . GetDirectoryCount ( tvi . Tag . ToString ( ) );
            //if ( dirs == 0 && files <= 0 && tvi . HasItems )
            //{
            //    try
            //    {
            //        if ( tvi . Items [ 0 ] . ToString ( ) == "Loading" )
            //        {
            //            tvi . Items . Clear ( );
            //            result = true;
            //        }
            //    }
            //    finally
            //    {
            //        if ( result )
            //        {
            //            tvi . IsExpanded = false;
            //            Selection . Text = $"Unable to access {tvi . Header . ToString ( )}";
            //            e . Handled = true;
            //        }
            //    }
            //}
            //else
            //{
            //    if ( tvi . IsExpanded == false )
            //    {
            //        // StartTimer ( );
            //        TestTree_Expanded ( sender , null );
            //    }
            //}
        }

        private void ToggleListbox ( object sender , MouseButtonEventArgs e )
        {

        }

        private void clearlog_Click ( object sender , RoutedEventArgs e )
        {
            listBox . Items . Clear ( );
        }

        private void TreeViewObs_SelectedItemChanged ( object sender , RoutedPropertyChangedEventArgs<object> e )
        {
            TreeView tv = sender as TreeView;
            if ( tv == null )
                return;
            TreeViewItem item = tv . SelectedItem as TreeViewItem;
            if ( item != null )
            {
                Console . WriteLine ( $"ItemChanged ? : Selection = {item . Tag . ToString ( )}" );
                Mouseovertvitem = item;
                if ( item . IsSelected == false )
                {
                    item . IsSelected = true;
                    //                    item . BringIntoView ( );
                    //                    item . HorizontalAlignment = HorizontalAlignment . Left;
                }
            }
        }
        private void TreeViewItem_RequestBringIntoView ( object sender , RequestBringIntoViewEventArgs e )
        {
            //stop horizontal scrolling when filling TV
            //e . Handled = true;
        }

        private void ToggleTreeview ( object sender , MouseButtonEventArgs e )
        {
            // if ( TreeviewObs .Visibility == Visibility . Visible )
            // {
            // ActiveTree = TreeViewObs;
            // TreeviewObs . Visibility = Visibility . Hidden;
            // TestTree . Visibility = Visibility . Visible;
            // TestTree . ItemsSource = tvitems;
            // TestTree . UpdateLayout ( );
            // TestTree . Refresh ( );
            // testtreebanner.Text= "Manual Directories System, TestTree";
            // TestTree . BringIntoView ( );
            // }
            // else
            // {
            // ActiveTree = TestTree;
            // TestTree . Visibility = Visibility . Hidden;
            // TreeviewObs . UpdateLayout ( );
            // TreeviewObs . Refresh ( );
            // TestTree . ItemsSource = null;
            // TreeviewObs . Visibility = Visibility . Visible;
            // TreeviewObs . BringIntoView ( );
            // testtreebanner . Text = "Manual Directories System, TestTree";
            // }
        }

        private void TestTree_SelectedItemChanged ( object sender , RoutedPropertyChangedEventArgs<object> e )
        {
            TreeView tv = sender as TreeView;
            if ( tv == null )
                return;
            TreeViewItem item = tv . SelectedItem as TreeViewItem;
            if ( item != null )
            {
                if ( tvcollection . SetSelected ( item ) != null )
                {
                    //                    item . BringIntoView ( );
                    //                    item . HorizontalAlignment = HorizontalAlignment . Left;
                }
            }

        }
        public TreeViewItem SelectNewTreeItem ( TreeViewItem item )
        {
            bool Success = false;
            TreeViewItem newitem = new TreeViewItem ( );
            foreach ( TreeViewItem itm in TvItems )
            {
                if ( Success == true)
                    break;
                if ( itm . Tag . ToString ( ) == item . Tag . ToString ( ) && itm . Header . ToString ( ) == item . Header . ToString ( ) )
                {
                    newitem = itm;
                    break;
                }
                itm . IsSelected = true;
                if ( itm . IsExpanded )
                {
                    if ( itm . Tag . ToString ( ) == item . Tag . ToString ( ) && itm . Header . ToString ( ) . ToUpper ( ) == item . Header . ToString ( ) )
                    {
                        newitem = itm;
                        Success = true;
                        break;
                    }
                    if ( itm . Items [ 0 ] . ToString ( ) != "Loading" )
                    {
                        foreach ( TreeViewItem itm2 in itm . Items )
                        {
                            if ( itm2 . Header . ToString ( ) . ToUpper ( ) == item . Header . ToString ( ) )
                            {
                                newitem = itm2;
                                Success = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if ( itm . Tag . ToString ( ) == item . Tag . ToString ( ) && itm . Header . ToString ( ) == item . Header . ToString ( ) )
                    {
                        newitem = itm;
                        break;
                    }
                }
            }
            return newitem;
        }
    private void TestTree_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
    {
            return;
        TreeViewItem currentitem = new TreeViewItem ( );
        Point pt = new Point ( );
        pt = e . GetPosition ( TestTree );

        // This might get us  the Textblock label contents !
        IInputElement dropNode = TestTree . InputHitTest ( pt );
        Type type = dropNode . GetType ( );
        if ( type . FullName . Contains ( "TextBlock" ) == false )
        {
            e . Handled = false;
            TestTree_Expanded ( sender , null );
            return;
        }
        //else if ( type . FullName . Contains ( "TextBlock" ) )
        //{
        //    TextBlock tb = dropNode as TextBlock;
        //    string str = tb . Text;
        //    currentitem . Header = str . ToUpper ( );
        //    currentitem . Tag = str . ToUpper ( );
        //    TreeViewItem newsel = new TreeViewItem ( );

        //    newsel = SelectNewTreeItem ( currentitem );
        //    newsel . IsSelected = true;
        //    return;
        //}

        return;





        IInputElement ie = ActiveTree . InputHitTest ( pt );
        var offset = this . VisualOffset;
        currentitem = TestTree . SelectedItem as TreeViewItem;
        if ( currentitem != null )
            currentitem . IsSelected = false;
        else return;
        if ( currentitem . Tag . ToString ( ) . Length > 3 )
            return;
        TreeViewItem selitem = GetPathAtPos ( pt );
        if ( selitem == null || selitem . Header == null )
            return;
        selitem . IsSelected = true;
        TestTree . Refresh ( );
        Console . WriteLine ( $"Selected item = ={selitem . Tag . ToString ( )}" );
    }
    public TreeViewItem GetPathAtPos ( Point point )
    {
        // Actually Works successfully - gets correct drive
        Point pt = new Point ( );
        Point pt2 = new Point ( );
        Point pt3 = new Point ( );
        int index = 1;

        double itemht = 0;
        TreeViewItem path = new TreeViewItem ( );
        foreach ( TreeViewItem item in TestTree . Items )
        {
            itemht = item . ActualHeight;
            pt3 = item . PointToScreen ( point );
            double diff = ( pt3 . Y / 10 );
            double match = ( index * itemht );
            //               Console . WriteLine ( $"point = {point . Y}, pt3.Y = {pt3 . Y}, match =  {match}, diff = {diff}, result ={match >= diff && match <= diff + itemht} " );
            if ( match >= point . Y && match <= point . Y + itemht )
            {
                path = item;
            }
            index++;
            //                int count = this . VisualChildrenCount;
        }
        return path;
    }
    private void TestTree_MouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
    {
        //var v = TestTree . SelectedItem;
        //if ( v == null )
        //{
        //    e . Handled = true;
        //}
    }

    private void TestTree_MouseDown ( object sender , MouseButtonEventArgs e )
    {
        var v = TestTree . SelectedItem;
        if ( v != null )
            tvcollection . SetSelected ( ( TreeViewItem ) v );

    }

    private void TreeViewItem_IsHitTestVisibleChanged ( object sender , DependencyPropertyChangedEventArgs e )
    {

    }

    private void TreeViewItem_MouseMove ( object sender , MouseEventArgs e )
    {
        Point pt = new Point ( );
        pt = e . GetPosition ( TestTree );
        IInputElement dropNode = TestTree . InputHitTest ( pt );
        IInputElement ie = ActiveTree . InputHitTest ( pt );
        //var res =  ie.GetType();
        //        var parent = Utils . FindVisualParent<TextBlock> ( dropNode.Text as UIElement);
        //            Console . WriteLine ( $"IE : {ie}, {pt . Y} dropNode={dropNode}" );
        //            Console . WriteLine ( $"MOVE : {pt . X}, {pt . Y} Parent=" );
        //VisualTreeHelper . GetParent ( )
    }

    private void TreeViewItem_PreviewMouseMove ( object sender , MouseEventArgs e )
    {

    }

        private void TreeViewItem_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
        {
            TreeViewItem currentitem = new TreeViewItem ( );
            Point pt = new Point ( );
            pt = e . GetPosition ( TestTree );

            // This might get us  the Textblock label contents !
            IInputElement dropNode = TestTree . InputHitTest ( pt );
            Type type = dropNode . GetType ( );
            if ( type . FullName . Contains ( "TextBlock" ) == false )
            {
                e . Handled = false;
                TestTree_Expanded ( sender , null );
                return;
            }

        }
    }
}
// End of CLASS TreeViews

//public class MyVirtualizingStackPanel : VirtualizingStackPanel
//{
//    /// <summary>
//    /// Publically expose BringIndexIntoView.
//    /// </summary>
//    public void BringIntoView ( int index )
//    {

//        this . BringIndexIntoView ( index );
//    }
//
