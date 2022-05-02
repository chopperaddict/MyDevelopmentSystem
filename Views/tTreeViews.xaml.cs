
#define DEBUGEXPAND
#undef DEBUGEXPAND

using System;
using System . Collections . Generic;
using System . ComponentModel;
using System . Diagnostics;
using System . IO;
using System . Linq;
using System . Runtime . InteropServices;
using System . Threading;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Controls . Primitives;
using System . Windows . Input;
using System . Windows . Media;
using System . Windows . Media . Imaging;
using System . Windows . Threading;

using MyDev . Models;
using MyDev . UserControls;
using MyDev . ViewModels;
namespace MyDev . Views
{
    public partial class tTreeViews : Window, INotifyPropertyChanged
    {
        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public int SLEEPTIME { get; set; } = 100;

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

        BackgroundWorker worker = null;
        #region full Props
        private string t1SelectedItem;
        public string T1SelectedItem
        {
            get { return t1SelectedItem; }
            set { t1SelectedItem = value; }// OnPropertyChanged ( T1SelectedItem ); }
        }

        private string t2SelectedItem;
        public string T2SelectedItem
        {
            get { return t2SelectedItem; }
            set { t2SelectedItem = value; OnPropertyChanged ( T2SelectedItem ); }
        }

        private string t3SelectedItem;
        public string T3SelectedItem
        {
            get { return t3SelectedItem; }
            set { t3SelectedItem = value; OnPropertyChanged ( T3SelectedItem ); }
        }
        private string t4SelectedItem;
        public string T4SelectedItem
        {
            get { return t4SelectedItem; }
            set { t4SelectedItem = value; OnPropertyChanged ( T4SelectedItem ); }
        }

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
        public ExplorerClass TvExplorer = null;
        public TreeView ActiveTree { get; set; }
        #endregion full Props
        //        public ICommand WalkTreeViewItem { get; set; }
        public struct ExpandArgs
        {
            public TreeView tv;
            public TreeViewItem tvitem;
            public TreeViewItem SearchSuccessItem;
            public int ExpandLevels;
            public int MaxItems;
            public string SearchTerm;
            //            public bool ClosePrevious;
            public bool SearchActive;
            public int Selection;
            public bool SearchSuccess;
            public bool ListResults;
            public TreeViewItem Parent;
        };
        public bool LISTRESULTS { get; set; } = true;

        // Global flag to control auto closing of searched folders (only)
        public bool ClosePreviousNode { get; set; }

        public static ExpandArgs ExpArgs = new ExpandArgs ( );
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
        public List<Family> families = new List<Family> ( );
        public static List<string> LbStrings = new List<string> ( );
        public static List<string> ValidFiles = new List<string> ( );
        public static List<TreeViewItem> AllCheckedFolders = new List<TreeViewItem> ( );
        public int ExpandSelection { get; set; } = -1;
        public TreeViewItem SelectedTVItem { get; set; }
        //        public bool ClosePreviousFolder { get; set; } = false;
        public bool Exactmatch { get; set; } = false;
        public string SearchString { get; set; } = "";
        public static bool BreakExpand { get; set; } = false;
        private static bool isresettingSelection { get; set; } = false;
        public int maxlevels { get; set; }
        public int TotalItemsExpanded { get; set; }
        public static lbitemtemplate lbtmp { get; set; }
        public TextBlock LbTextblock { get; set; }
        public string TextToSearchFor { set; get; } = "";
        public static tTreeViews treeViews { get; set; }
        private TreeViewItem startitem { get; set; }

        #region Public dclarations
        public bool HasHidden = false;
        public bool ShowVolumeLabels { get; set; } = true;
        private bool FullExpandinProgress = false;
        public bool Loading = true;
        public static bool ShowAllfiles = false;
        private static double startmin = 0;
        private static double startsec = 0;
        private static double startmsec = 0;
        //		public static LazyLoading Lazytree = null;
        public Family family1 = new Family ( );
        public DirectoryInfo DirInfo = new DirectoryInfo ( @"C:\\" );
        private static DispatcherTimer sw = new DispatcherTimer ( );

        #endregion Public dclarations

        private static FlowdocLib fdl;
        #region startup
        public tTreeViews ( )
        {
            InitializeComponent ( );
            this . DataContext = this;

            // Cannot use  this with FlowDoc cos of dragging/Resizing
            //Utils . SetupWindowDrag ( this );
            //            WalkTreeViewItem = new RelayCommand ( ExecuteWalkTreeViewItem , CanExecuteWalkTreeViewItem );
            ActiveTree = TestTree;
            OptionsPanel . Visibility = Visibility . Hidden;
            treeViews = this;
            listBox . Items . Clear ( );
            TestTree . Items . Clear ( );
            fdl = new FlowdocLib ( );
            FdMargin . Left = Flowdoc . Margin . Left;
            FdMargin . Top = Flowdoc . Margin . Top;
            TvExplorer = new ExplorerClass ( );
            //            treeViewModel . SelectedValuePath = "C:\\";
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
            //            treeViewModel . SetValue ( SelectedItemProperty , tvi );
        }
        private void Window_Loaded ( object sender , RoutedEventArgs e )
        {
            string output = "";
            ShowAllfiles = true;
            this . SetValue ( FontsizeProperty , InfoList . FontSize );
            canvas . Visibility = Visibility . Visible;
            CreateStaticData ( );
            CreateBrushes ( );
            LoadDrives ( TestTree );
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
            DrivesCombo . SelectedIndex = 0;
            maxlevels = 99;
            Loading = false;
            this . DataContext = this;

        }

        #endregion startup        

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

        private void CreateStaticData ( )
        {
            //			treeView3 . ItemsSource = null;
            //			treeView3 . Items . Clear ( );
            //family1 = new Family ( ) { Name = "The Doe's" };
            //family1 . Members . Add ( new FamilyMember ( ) { Name = "John Doe" , Age = 42 } );
            //family1 . Members . Add ( new FamilyMember ( ) { Name = "Jane Doe" , Age = 39 } );
            //family1 . Members . Add ( new FamilyMember ( ) { Name = "Sammy Doe" , Age = 13 } );
            //families . Add ( family1 );

            //Family family2 = new Family() { Name = "The Moe's" };
            //family2 . Members . Add ( new FamilyMember ( ) { Name = "Mark Moe" , Age = 31 , Gender = "Male" , Employed = true } );
            //family2 . Members . Add ( new FamilyMember ( ) { Name = "Norma Moe" , Age = 28 , Gender = "Female" , Employed = false } );
            //families . Add ( family2 );

            //treeView3 . ItemsSource = families;
        }
        //private void treeViewModel_SelectedItemChanged ( object sender , RoutedPropertyChangedEventArgs<object> e )
        //{
        //    //			var tree =( TreeViewItem ) treeViewModel . ItemContainerGenerator . ContainerFromItem ( e . OriginalSource ) as TreeViewItem;
        //    //           var v = e . OriginalSource as TreeViewItem;
        //    // This  gets the Current selectedItem Correctly !
        //    var tvItem = treeViewModel . ItemContainerGenerator . ContainerFromItem ( ( ( TreeView ) sender ) . SelectedItem );

        //}

        //private void treeViewModel_Expanded ( object sender , RoutedEventArgs e )
        //{
        //    var sel = treeViewModel . SelectedItem as TreeViewItem;
        //    if ( sel == null )
        //    {
        //        if ( CurrentTreeItem == null )
        //            return;
        //    }
        //    else
        //        LazyLoadingTreeview . TreeViewItem4_Expanded ( sender , e , CurrentTreeItem );
        //}

        //private void treeViewModel_Collapsed ( object sender , RoutedEventArgs e )
        //{

        //}

        //private void treeViewModel_Selected ( object sender , RoutedEventArgs e )
        //{

        //}

        //private void TesViewModel ( object sender , RoutedEventArgs e )
        //{

        //}

        private void TestViewModel ( object sender , RoutedEventArgs e )
        {
            List<string> dirs = new List<string> ( );
            List<string> files = new List<string> ( );
            string selecteddrive = DrivesCombo . SelectedItem . ToString ( );
            //CreateTreeViewData ( $"{selecteddrive}" , dirs , files );
            LoadDrives ( ActiveTree );
        }

        #region selection changing
        private void treeView1_SelectedItemChanged ( object sender , RoutedPropertyChangedEventArgs<object> e )
        {
            CurrentTree = 1;
            CurrentLevel = 1;
            Selection . Text = "";
            // How to get the currently seleted item's "Header" string
            var entry = e . NewValue as TreeViewItem;
            T1SelectedItem = entry?.Header . ToString ( );
            Selection . Text = T1SelectedItem;
        }
        private void treeView2_SelectedItemChanged ( object sender , RoutedPropertyChangedEventArgs<object> e )
        {
            CurrentTree = 2;
            CurrentLevel = 1;
            // How to get the currently seleted item's "Header" string
            var entry = e . NewValue as TreeViewItem;
            T2SelectedItem = entry?.Header . ToString ( );
            Selection . Text = T2SelectedItem;
        }
        private void treeView3_SelectedItemChanged ( object sender , RoutedPropertyChangedEventArgs<object> e )
        {
            CurrentTree = 3;
            // How to get the currently seleted item's data when the Treeiew is Dynamic
            // We parse down the levels in the tree to find out what level we have selected
            var entry = e . NewValue as Family;
            if ( entry != null )
            {
                T3SelectedItem = entry?.Name;
                Selection . Text = T3SelectedItem;
                CurrentLevel = 1;
                return;
            }
            else
            {
                var entry2 = e . NewValue as FamilyMember;
                entry2 = e . NewValue as FamilyMember;
                if ( entry2 != null )
                {
                    CurrentLevel = 2;
                    T3SelectedItem = entry2 . Name;
                    string Empstatus = entry2 . Employed == true ? "Yes" : "No";
                    FullDetail = entry2 . Name + ", " + entry2 . Age + " years, Employed : " + Empstatus;
                    Selection . Text = FullDetail;
                    return;
                }
            }
        }
        #endregion selection changing
        private void LoadDrives ( TreeView tv )
        {
            bool ValidDrive = false;
            bool HasHiddenItems = false;
            string volabel = "";
            string DriveHeader = "", Padding = "                 ";
            bool isvalid = false;
            tv . Items . Clear ( );
            listBox . Items . Clear ( );
            listBox . UpdateLayout ( );
            DrivesCombo . Items . Add ( "ALL" );
            LoadValidFiles ( );
            //     TestTree . FontFamily = "Consolas";
            foreach ( var drive in Directory . GetLogicalDrives ( ) )
            {
                ValidDrive = false;
                DriveHeader = "";
                //Add Drive to Treeview
                DriveInfo [ ] di = DriveInfo . GetDrives ( );
                foreach ( var item in di )
                {
                    if ( item . Name == drive )
                    {
                        if ( item . DriveType == DriveType . CDRom )
                        {
                            ValidDrive = false;
                            DriveHeader = Padding . Substring ( 0 , 10 );
                            DriveHeader += "CdRom Drive";
                        }
                        else
                        {
                            List<string> directories = new List<string> ( );
                            GetDirectories ( item . ToString ( ) , out directories );
                            foreach ( var dir in directories )
                            {
                                if ( CheckIsVisible ( dir . ToUpper ( ) , ShowAllfiles , out HasHidden ) == true )
                                {
                                    isvalid = true;
                                    if ( ShowVolumeLabels == true )
                                        DriveHeader = $"    [{item . VolumeLabel}]";
                                    break;
                                }
                            }
                            if ( isvalid )
                            {
                                if ( ShowVolumeLabels == true )
                                    DriveHeader = $"   [{item . VolumeLabel}]";
                                ValidDrive = true;
                            }
                            else
                                volabel = $"    [{item . VolumeLabel}]";
                        }
                        break;
                    }
                }
                if ( ValidDrive == true )
                {
                    var item = new TreeViewItem ( );
                    item . Header = drive + DriveHeader;
                    item . Tag = drive;
                    tv . Items . Add ( item );
                    // Add Dummy entry so we get an "Can be Opened" triangle icon
                    item . Items . Add ( "Loading" );
                    UpdateListBox ( item . Tag . ToString ( ) );
                    DrivesCombo . Items . Add ( drive . ToString ( ) );
                }
                else
                {
                    var item = new TreeViewItem ( );
                    if ( ShowVolumeLabels == true )
                        item . Header = drive + volabel;
                    else
                        item . Header = drive + DriveHeader;
                    item . Tag = drive;
                    tv . Items . Add ( item );
                    UpdateListBox ( item . Tag . ToString ( ) );
                    DrivesCombo . Items . Add ( drive . ToString ( ) );
                }
                //break;
            }
            DrivesCombo . Items . Add ( "ALL" );
            DrivesCombo . SelectedIndex = 0;
            DrivesCombo . SelectedItem = 0;
            tv . UpdateLayout ( );
        }
        public void LoadValidFiles ( )
        {
            ValidFiles . Add ( "BOOTMGR" );
            ValidFiles . Add ( "BOOTNXT" );
            ValidFiles . Add ( "BOOTSTAT" );
            ValidFiles . Add ( "RECOVERY" );
            ValidFiles . Add ( "BOOTNXT" );
            ValidFiles . Add ( "MEMTEST" );
            ValidFiles . Add ( "BOOTUWF" );
            ValidFiles . Add ( "BOOTVHD" );
            ValidFiles . Add ( "MEMTEST" );
            ValidFiles . Add ( "BOOT" );
            ValidFiles . Add ( "$GETCURRENT" );
            ValidFiles . Add ( "$WINDOWS" );
            ValidFiles . Add ( "$WINREAGENT" );
            ValidFiles . Add ( "CONFIG.MSI" );
            ValidFiles . Add ( "WINDOWS.OLD" );
            ValidFiles . Add ( ".BIN" );
            ValidFiles . Add ( "$WINRE_BACKUP" );
            ValidFiles . Add ( "RECYCLE" );
            ValidFiles . Add ( "SYSTEM VOLUME INFORMATION" );
            ValidFiles . Add ( "BACKUP_PARTITION" );
            ValidFiles . Add ( "BOOTSECT" );
        }
        private void Window_KeyDown ( object sender , KeyEventArgs e )
        {
            if ( e . Key == Key . F8 )
                Debugger . Break ( );
        }
        private void ShowallFiles_Click ( object sender , RoutedEventArgs e )
        {
            CheckBox cb = sender as CheckBox;
            if ( cb . IsChecked == true )
                ShowAllfiles = true;
            else
                ShowAllfiles = false;
            LoadDrives ( ActiveTree );
        }

        #region utilities
        public static string GetFileFolderName ( string path )
        {
            if ( string . IsNullOrEmpty ( path ) )
                return String . Empty;
            var normalizedPath = path . Replace ( '/' , '\\' );
            var lastindex = normalizedPath . LastIndexOf ( '\\' );
            if ( lastindex <= 0 )
                return path;
            return path . Substring ( lastindex + 1 );
        }
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

        #region Dependency Properties
        //public string ExpandDuration
        //{
        //    get { return ( string ) GetValue ( ExpandDurationProperty ); }
        //    set { SetValue ( ExpandDurationProperty , value ); }
        //}
        //public static readonly DependencyProperty ExpandDurationProperty =
        //    DependencyProperty . Register ( "ExpandDuration" , typeof ( string ) , typeof ( tTreeViews ) , new PropertyMetadata ( "0:00" ) );

        public string def = ".....................................................";
        private int progressCount;
        public int ProgressCount
        {
            get { return progressCount; }
            set
            {
                if ( value != 0 && value % PROGRESSWRAPVALUE == 0 )
                {
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
                //               Console . WriteLine ($": [{ProgressString}]");
                OnPropertyChanged ( ProgressString );
            }
        }

        private int listboxtotal;
        public int Listboxtotal
        {
            get { return listboxtotal; }
            set { listboxtotal = value; OnPropertyChanged ( Listboxtotal . ToString ( ) ); }
        }

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
            DependencyProperty . Register ( "tv1SelectedItem" , typeof ( bool ) , typeof ( tTreeViews ) , new PropertyMetadata ( false ) );
        public bool tv2SelectedItem
        {
            get { return ( bool ) GetValue ( tv2SelectedItemProperty ); }
            set { SetValue ( tv2SelectedItemProperty , value ); }
        }
        public static readonly DependencyProperty tv2SelectedItemProperty =
            DependencyProperty . Register ( "tv2SelectedItem" , typeof ( bool ) , typeof ( tTreeViews ) , new PropertyMetadata ( false ) );
        public bool tv3SelectedItem
        {
            get { return ( bool ) GetValue ( tv3SelectedItemProperty ); }
            set { SetValue ( tv3SelectedItemProperty , value ); }
        }
        public static readonly DependencyProperty tv3SelectedItemProperty =
            DependencyProperty . Register ( "tv3SelectedItem" , typeof ( bool ) , typeof ( tTreeViews ) , new PropertyMetadata ( false ) );
        public TreeViewItem tv4SelectedItem
        {
            get { return ( TreeViewItem ) GetValue ( tv4SelectedItemProperty ); }
            set { SetValue ( tv4SelectedItemProperty , value ); }
        }
        public static readonly DependencyProperty tv4SelectedItemProperty =
            DependencyProperty . Register ( "tv4SelectedItem" , typeof ( TreeViewItem ) , typeof ( tTreeViews ) , new PropertyMetadata ( ( TreeViewItem ) null ) );
        public TreeViewItem SelectedItem
        {
            get { return ( TreeViewItem ) GetValue ( SelectedItemProperty ); }
            set { SetValue ( SelectedItemProperty , value ); }
        }
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty . Register ( "SelectedItem" , typeof ( TreeViewItem ) , typeof ( tTreeViews ) , new PropertyMetadata ( ( TreeViewItem ) null ) );
        public double Fontsize
        {
            get { return ( double ) GetValue ( FontsizeProperty ); }
            set { SetValue ( FontsizeProperty , value ); }
        }
        public static readonly DependencyProperty FontsizeProperty =
            DependencyProperty . Register ( "Fontsize" , typeof ( double ) , typeof ( tTreeViews ) , new PropertyMetadata ( ( double ) 12 ) );
        public BitmapImage LsplitterImage
        {
            get
            { return ( BitmapImage ) GetValue ( LsplitterImageProperty ); }
            set { SetValue ( LsplitterImageProperty , value ); }
        }
        public static readonly DependencyProperty LsplitterImageProperty =
            DependencyProperty . Register ( "LsplitterImage" , typeof ( BitmapImage ) , typeof ( tTreeViews ) , new PropertyMetadata ( ( BitmapImage ) null ) );
        public BitmapImage VsplitterImage
        {
            get
            { return ( BitmapImage ) GetValue ( VsplitterImageProperty ); }
            set { SetValue ( VsplitterImageProperty , value ); }
        }
        public static readonly DependencyProperty VsplitterImageProperty =
            DependencyProperty . Register ( "VsplitterImage" , typeof ( BitmapImage ) , typeof ( tTreeViews ) , new PropertyMetadata ( ( BitmapImage ) null ) );
        public string LeftSplitterText
        {
            get { return ( string ) GetValue ( LeftSplitterTextProperty ); }
            set { SetValue ( LeftSplitterTextProperty , value ); }
        }
        // Using a DependencyProperty as the backing store for LeftSplitterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftSplitterTextProperty =
            DependencyProperty . Register ( "LeftSplitterText" , typeof ( string ) , typeof ( tTreeViews ) , new PropertyMetadata ( ( string ) "Drag Up or Down" ) );
        public string RightSplitterText
        {
            get { return ( string ) GetValue ( RightSplitterTextProperty ); }
            set { SetValue ( RightSplitterTextProperty , value ); }
        }
        // Using a DependencyProperty as the backing store for LeftSplitterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightSplitterTextProperty =
            DependencyProperty . Register ( "RightSplitterText" , typeof ( string ) , typeof ( tTreeViews ) , new PropertyMetadata ( ( string ) "to View Directory Tree / Drive Technical Information." ) );


        public SolidColorBrush LbTextColor
        {
            get { return ( SolidColorBrush ) GetValue ( LbTextColorProperty ); }
            set { SetValue ( LbTextColorProperty , value ); }
        }
        public static readonly DependencyProperty LbTextColorProperty =
            DependencyProperty . Register ( "LbTextColor" , typeof ( SolidColorBrush ) , typeof ( tTreeViews ) ,
                new PropertyMetadata ( new SolidColorBrush ( Colors . Black ) ) );

        #endregion Dependency Properties

        #region Attached Properties

        public static string GetExpandDuration ( DependencyObject obj )
        {
            return ( string ) obj . GetValue ( ExpandDurationProperty );
        }
        public static void SetExpandDuration ( DependencyObject obj , string value )
        {
            obj . SetValue ( ExpandDurationProperty , value );
        }
        public static readonly DependencyProperty ExpandDurationProperty =
            DependencyProperty . RegisterAttached ( "ExpandDuration" , typeof ( string ) , typeof ( tTreeViews ) , new PropertyMetadata ( "0:00" ) );


        public static bool Gettvselection ( DependencyObject obj )
        {
            return ( bool ) obj . GetValue ( tvselectionProperty );
        }
        public static void Settvselection ( DependencyObject obj , bool value )
        {
            obj . SetValue ( tvselectionProperty , value );
        }
        public static readonly DependencyProperty tvselectionProperty =
            DependencyProperty . RegisterAttached ( "tvselection" , typeof ( bool ) , typeof ( tTreeViews ) , new PropertyMetadata ( ( bool ) false ) );

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
            // NB Flowdoc remebers its last position automatically
        }
        private void Flowdoc_MouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
        {
            // Window wide  !!
            // Called  when a Flowdoc MOVE has ended
            MovingObject = fdl . Flowdoc_MouseLeftButtonUp ( sender , Flowdoc , MovingObject , e );
            ReleaseMouseCapture ( );
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
        }


        #endregion Flowdoc support via library

        private void TREEViews_Closing ( object sender , CancelEventArgs e )
        {
            Flowdoc . ExecuteFlowDocMaxmizeMethod -= new EventHandler ( MaximizeFlowDoc );
            Flowdoc . HandleKeyEvents -= new KeyEventHandler ( Flowdoc_HandleKeyEvents );
        }
        //private void treeViewModel_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
        //{
        //    if ( Utils . HitTestScrollBar ( sender , e ) )
        //    {
        //        return;
        //    }
        //    if ( treeViewModel . Items . CurrentItem == null )
        //        CurrentTreeItem = treeViewModel . Items . GetItemAt ( 0 ) as TreeViewItem;
        //    else
        //    {
        //    }
        //    //var indx = treeViewModel . Items . IndexOf ( CurrentTreeItem );
        //    //            var v = ( TreeViewItem ) treeViewModel . ItemContainerGenerator . ContainerFromItem ( e . OriginalSource ) as TreeViewItem;
        //    //var v2 = ( TreeViewItem ) treeViewModel . ItemContainerGenerator . ContainerFromItem ( CurrentTreeItem ) as TreeViewItem;
        //    Console . WriteLine ( $"Left Button Down returns ? CurrentTreeItem={CurrentTreeItem}" );
        //    LazyLoadingTreeview . TreeViewItem4_Expanded ( sender , null , CurrentTreeItem );
        //}
        //private void treeViewModel_Loaded ( object sender , RoutedEventArgs e )
        //{
        //    // This  gets the Current selectedItem Correctly !
        //    TreeViewItem tvi = new TreeViewItem ( );
        //    tvi . Header = @"C:\";
        //    treeViewModel . SetValue ( SelectedItemProperty , tvi );
        //    treeViewModel . SetCurrentValue ( SelectedItemProperty , tvi );
        //    var sel = treeViewModel . SelectedItem;
        //    //. SetCurrentSelectedItem(@"C:\\" );
        //    var tvItem = treeViewModel . ItemContainerGenerator . ContainerFromItem ( ( ( TreeView ) sender ) . SelectedItem );
        //    var curr = TvExplorer . CurrentDrive;
        //    treeViewModel . Focus ( );
        //}
        private void ShowDriveInfo ( object sender , RoutedEventArgs e )
        {
            string output = "";
            ExplorerClass Texplorer = new ExplorerClass ( );
            Texplorer . GetDrives ( "C:\\" );
            List<lbitemtemplate> lbtmplates = new List<lbitemtemplate> ( );
            InfoList . ItemsSource = null;
            InfoList . Items . Clear ( );
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
        private static bool CheckIsVisible ( string entry , bool showall , out bool HasHidden )
        {
            HasHidden = false;
            entry = entry . ToUpper ( );
            if ( showall == false )
            {
                foreach ( var item in ValidFiles )
                {
                    if ( entry . Contains ( item . ToUpper ( ) ) )
                    {
                        HasHidden = true; ;
                        return false;
                    }
                }
                return true;
            }
            return true;
        }
        public int AddDirectoriesToTestTreeview ( List<string> directories , TreeViewItem item , ListBox lBox = null , bool UseExpand = true )
        {
            int added = 0;
            if ( directories . Count == 0 )
                return 0;
            item . Items . Clear ( );
            foreach ( var dir in directories )
            {
                var subitem = new TreeViewItem ( );
                //                if ( subitem . Items . Count == 1 && subitem . Items [ 0 ] == "Loading" )
                //                    subitem . Items . Clear ( );
                ShowProgress ( );

                if ( CheckIsVisible ( dir . ToUpper ( ) , ShowAllfiles , out HasHidden ) == true )
                {
                    try
                    {
                        subitem . Header = GetFileFolderName ( dir );
                        subitem . Tag = dir;
                        item . Items . Add ( subitem );
                        item . IsExpanded = true;
                        ActiveTree . HorizontalContentAlignment = HorizontalAlignment . Left;
                        ScrollCurrentTvItemIntoView ( subitem );
                        TestTree . Refresh ( );
                        //UpdateListBox ( subitem . Tag . ToString ( ) );

                        int count = GetDirectories ( dir , out directories );
                        if ( count > 0 )
                        {
                            var tv = new TreeViewItem ( );
                            tv . Header = "Loading";
                            //                      tv . Tag = "Loading";
                            subitem . Items . Add ( tv );
                            //ScrollCurrentTvItemIntoView (subitem );

                            //AddDirectoriesToTestTreeview ( directories , subitem , listBox );
                        }
                        else
                        {
                            var dirfile = Directory . GetFiles ( dir , "*.*" );
                            count = ( int ) dirfile . Length;
                            if ( count > 0 )
                            {
                                //foreach ( var temp in dirfile )
                                //{
                                var tv = new TreeViewItem ( );
                                tv . Header = "Loading";
                                //                            tv . Tag = temp;
                                subitem . Items . Add ( tv );
                                //                            subitem . IsExpanded = true;
                                ActiveTree . HorizontalContentAlignment = HorizontalAlignment . Left;
                                ScrollCurrentTvItemIntoView ( subitem );
                                TestTree . Refresh ( );

                                //}
                            }
                        }
                    }
                    catch ( Exception ex )
                    {
                        Console . WriteLine ( $"Invalid  directory accessed {ex . Message}" );
                        //                    continue;
                    }
                }
                ShowProgress ( );
                //if ( CheckIsVisible ( dir . ToUpper ( ) , ShowAllfiles ) == true )
                //{
                //    // add the dummy entry to each of the subdirectories we are adding to the tree so we get the Expand icons
                // need to protect against invalid folder access from crashing us!!!!
                //int totdirs = GetDirectoryCount ( dir );
                // if ( totdirs == 0 )
                // {
                //     //int count = GetDirectories ( dir , out directories );
                //     //if ( count > 0 )
                //     //    AddDirectoriesToTestTreeview ( directories , subitem , listBox );
                // }
                // else
                // {
                //     int count = GetDirectories ( dir, out directories );
                //     if(count > 0)
                //        AddDirectoriesToTestTreeview ( directories , subitem , listBox );
                // }
                //  ShowProgress ( );
                added++;
                //}
                //else
                //    ShowProgress ( );
            }
            return added;
        }
        public int AddFilesToTreeview ( List<string> Allfiles , TreeViewItem item )
        {
            int count = 0;
            if ( item . Items . Count == 1 )
            {
                var tmp = item . Items [ 0 ] . ToString ( );
                if ( tmp == "Loading" )
                    item . Items . Clear ( );

            }
            item . IsExpanded = true;
            foreach ( var itm in Allfiles )
            {
                ShowProgress ( );
                var subitem = new TreeViewItem ( )
                {
                    Header = GetFileFolderName ( itm ) ,
                    Tag = itm
                };
                if ( CheckIsVisible ( itm . ToUpper ( ) , ShowAllfiles , out HasHidden ) == true )
                {
                    item . Items . Add ( subitem );
                    item . IsSelected = true;
                    ActiveTree . HorizontalContentAlignment = HorizontalAlignment . Left;
                    ScrollCurrentTvItemIntoView ( subitem );
                    TestTree . Refresh ( );
                    count++;
                }
                ShowProgress ( );
                if ( FullExpandinProgress == false )
                    ActiveTree . Refresh ( );
            }
            return count;
        }
        public int GetDirectories ( string path , out List<string> dirs )
        {
            bool filterSysfiles = false;
            int count = 0;
            List<string> directories = new List<string> ( );
            try
            {
                var directs = Directory . GetDirectories ( path , "*.*" );
                if ( directs . Length > 0 )
                {
                    foreach ( var item in directs )
                    {
                        if ( filterSysfiles )
                        {
                            ShowProgress ( );
                            if ( IsSystemFile ( item . ToUpper ( ) ) == true )
                            {
                                continue;
                            }
                        }
                        //if ( CheckIsVisible ( item . ToUpper ( ) , ShowAllfiles ) == true )
                        directories . Add ( item );

                        //                        UpdateListBox ( item );


                        ShowProgress ( );
                        if ( FullExpandinProgress == false )
                            ActiveTree . Refresh ( );
                        count++;
                    }
                }
            }
            catch { }
            dirs = directories;
            return count;
        }
        public int GetDirectoryCount ( string path )
        {
            int count = 0;
            List<string> directories = new List<string> ( );
            try
            {
                string [ ] directs = Directory . GetDirectories ( path , "*.*" );
                count = directs . Length;
            }
            catch { }
            return count;
        }
        public int GetFiles ( string path , out List<string> allfiles )
        {
            int count = 0;
            var files = new List<string> ( );
            // Get a list of all items in the current folder
            ShowProgress ( );
            if ( FullExpandinProgress == false )
                ActiveTree . Refresh ( );
            try
            {
                var file = Directory . EnumerateFiles ( path , "*.*" );
                var fil = Directory . GetFiles ( path , "*.*" );
                if ( file . Count ( ) > 0 )
                {
                    foreach ( var item in file )
                    {
                        ShowProgress ( );
                        if ( CheckIsVisible ( item . ToUpper ( ) , ShowAllfiles , out HasHidden ) == true )
                        {
                            files . Add ( item );
                            count++;
                            // working correctly
                            UpdateListBox ( item );
                        }
                        ShowProgress ( );
                        if ( FullExpandinProgress == false )
                            ActiveTree . Refresh ( );
                    }
                }
            }
            catch { }
            allfiles = files;
            return count;
        }
        public int GetFilesCount ( string path )
        {
            int count = 0;
            var files = new List<string> ( );
            // Get a list of all items in the current folder
            try
            {
                //var file = Directory . EnumerateFiles ( path , "*.*" );
                var dirfile = Directory . GetFiles ( path , "*.*" );
                count = ( int ) dirfile . Length;
                ShowProgress ( );
                //    //var file = Directory . GetFiles ( path , "*.*");
                //    if ( file . Count ( ) > 0 )
                //    {
                //        foreach ( var item in file )
                //        {
                //            if ( CheckIsVisible ( item . ToUpper ( ) , ShowAllfiles ) == true )
                //            {
                //                files . Add ( item );
                //                count++;
                //            }
                //        }
                //        //					files . AddRange ( file );
                //    }
            }
            catch { }
            //allfiles = files;
            return count;
        }

        #endregion Treeview support methods

        private void treeView4_Selected ( object sender , RoutedEventArgs e )
        {
            //bool isCollapsing = false;
            //return;

            //TreeViewItem tvi = treeView4 . SelectedItem as TreeViewItem;
            //if ( tvi == null )
            //    return;
            //CurrentItem = tvi;
            //var tag = tvi . Tag;
            //if ( tvi . IsExpanded == true )
            //{
            //    isCollapsing = true;
            //    isresettingSelection = true;
            //    tvi . IsSelected = true;
            //    return;
            //}
            //else
            //    tvi . IsExpanded = false;
            //// fully qualified path to selected item
            //var s = tag . ToString ( );
            //// This is  the current selection under the cursor !
            //string selectedItem = tvi . Tag . ToString ( );
            //GetItemCounts ( selectedItem , out int Dircount , out int Filecount );
            //isresettingSelection = true;
            //testtreebanner . Text = $"Current Folder Content(s) for : {selectedItem}";
            //isresettingSelection = true;
            //// tvi . IsSelected = true;
            //if ( tvi . IsExpanded == false )
            //{
            //    //e . Handled = true;
            //}
            //else
            //{
            //    isresettingSelection = true;
            //}
            //return;
        }
        private void treeView4_Collapsed ( object sender , RoutedEventArgs e )
        {
            //Mouse . OverrideCursor = Cursors . Wait;
            //TreeViewItem item = e . OriginalSource as TreeViewItem;
            //CurrentItem = item;
            //item . IsExpanded = false;
            //item . Items . Clear ( );
            //// Add Dummy entry just so we do get the expand icon
            //item . Items . Add ( "Loading" );
            //string header = item . Header . ToString ( );
            //listBox . Items . Clear ( );
            //listBox . Refresh ( );
            ////            Mouse . SetCursor ( Cursors . Arrow );
            //GetItemCounts ( header , out int Dircount , out int Filecount );
            //Mouse . OverrideCursor = Cursors . Arrow;
        }
        private void treeView4_SelectedItemChanged ( object sender , RoutedPropertyChangedEventArgs<object> e )
        {
            //var item = e . NewValue as TreeViewItem;
            //if ( item != null )//&& item . IsSelected == false )
            //{
            //    item . IsSelected = true;
            //}
            //// This  gets the Current selectedItem Correctly, but  as a DependencyObject - not sure how to use that !
            //var tvItem = treeView4 . ItemContainerGenerator . ContainerFromItem ( ( ( TreeView ) sender ) . SelectedItem );
            //SetValue ( tv4SelectedItemProperty , item );
            //CurrentItem = item;
        }
        private void treeView4_PreviewMouseRightButtonDown ( object sender , MouseButtonEventArgs e )
        {
            //this . Flowdoc . Height = 200;

            ////treeView4.GetValue(H)
            //fdl . FdMsg ( Flowdoc , canvas , "Testing FdMsg in Treeview" , $"{treeView4 . DisplayMemberPath}" );
        }
        private void TreeViewItem4_Expanded ( object sender , RoutedEventArgs e )
        {
            //#region Expanding  setup
            //TreeViewItem item = null;
            //int itemscount = 0;
            //if ( e != null )
            //    item = e . Source as TreeViewItem;
            //else
            //    item = sender as TreeViewItem;
            ////            Mouse . SetCursor ( Cursors . Wait );
            //if ( item == null )
            //    return;

            //// This is CRITICAL to get any drive that is currently selected to open when the expand icon is clicked
            ////if ( item . IsSelected == true )
            ////item . IsSelected = false;
            //listBox . Items . Clear ( );
            //listBox . UpdateLayout ( );

            //item . IsSelected = true;
            //#endregion Expanding  setup

            //#region Expanding Get Folders

            //var directories = new List<string> ( );
            //var Allfiles = new List<string> ( );
            //string Fullpath = item . Tag . ToString ( ) . ToUpper ( );

            //string InfoMessage = "";
            //int DirectoryCount = 0;
            //int FileCount = 0;
            //itemscount = item . Items . Count;
            //var tvi = item as TreeViewItem;
            //if ( itemscount == 0 )
            //    return;
            //var itemheader = item . Items [ 0 ] . ToString ( );
            //if ( itemheader == "Loading" )
            //    item . Items . Clear ( );
            //// Get a list of all items in the current folder
            //int count = GetDirectories ( Fullpath , out directories );
            //if ( count == 0 || directories == null )
            //    return;
            //DirectoryCount = count;
            //if ( directories . Count >= 1 )
            //{
            //    DirectoryCount = AddDirectoriesToTreeview ( directories , item , listBox );
            //}
            //else
            //    DirectoryCount = 0;
            ////// Check to see if there any file items in the current folder
            //if ( DirectoryCount > 0 )
            //    InfoMessage = $"Current Item : {Fullpath} -  {DirectoryCount} SubDirectory(s)";
            //else
            //{
            //    if ( ShowAllfiles )
            //        InfoMessage = $"Current Item : {Fullpath} -  No SubDirectories ";
            //    else
            //        InfoMessage = $"Current Item : {Fullpath} -  No valid SubDirectories ";
            //}
            //GetFiles ( Fullpath , out Allfiles );
            //FileCount = Allfiles . Count;
            //if ( FileCount > 0 )
            //{
            //    int added = AddFilesToTreeview ( Allfiles , item );
            //    if ( added == 0 )
            //        InfoMessage += $",  No Files";
            //    else
            //        InfoMessage += $", {added} Files";
            //}
            //else
            //    InfoMessage += $",  No Files";
            //Selection . Text = InfoMessage;
            //treeView4 . UpdateLayout ( );
            //testtreebanner . Text = $"Current Folder Content(s) for : {Fullpath}";
            //return;
            //#region Expanding Get Files	  (UNUSED)
            //#endregion Expanding Get Files	  (UNUSED)
        }
        private void treeView4_MouseDoubleClick ( object sender , MouseButtonEventArgs e )
        {
            //TreeView tv = treeView4;
            //var rootTreeViewItem = treeView4 . SelectedItem as TreeViewItem;
            //listBox . Items . Add ( rootTreeViewItem . Tag . ToString ( ) );
            //return;

        }
        private void treeView4_PreviewMouseDoubleClick ( object sender , MouseButtonEventArgs e )
        {
            //TreeView tv = treeView4;
            //string stre = tv . SelectedValuePath;
            //treeView4 . UpdateLayout ( );
            //// Get   the currently selected node
            //var rootTreeViewItem = treeView4 . SelectedItem as TreeViewItem;
            //if ( rootTreeViewItem != null && rootTreeViewItem . IsExpanded == false )
            //{
            //    ExpandAll3 ( rootTreeViewItem , true );
            //    // ExpandAll3 ( rootTreeViewItem , false );
            //    rootTreeViewItem . IsExpanded = true;
            //}
            //else
            //    ExpandAll3 ( rootTreeViewItem , false );
            ////rootTreeViewItem . UpdateLayout ( );
        }
        private void treeView4_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
        {
            //            var rootTreeViewItem = treeView4 . SelectedItem as TreeViewItem;
            //if ( rootTreeViewItem != null )
            //    CurrentItem = rootTreeViewItem;
            //return;


            //ExpandAll3 ( rootTreeViewItem , true );

            //if ( rootTreeViewItem . IsExpanded )
            //{
            //    rootTreeViewItem . IsExpanded = false;
            //    rootTreeViewItem . IsSelected = true;
            //    rootTreeViewItem . UpdateLayout ( );
            //    CurrentItem = rootTreeViewItem;
            //}
        }


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
            //Console . WriteLine ($"{TestTree . ActualWidth }" );
            //if ( TestTree . ActualWidth < 155 )
            //{
            //    DragCompletedEventArgs dce = new DragCompletedEventArgs ( 0 , 0 , true );
            //    VRightSplitter_DragCompleted ( sender , dce );
            //    e . Handled = true;
            //}
        }

        #endregion Splitter handlers

        #region Expand // Collapse

        #region TestTree Expanding Handling methods
        int iterations = 0;


        public Task t1;
        public DispatcherOperation operation;
        private bool ExpandAll3 ( TreeViewItem items , bool expand , int levels )
        {
            if ( items == null )
                return false;

            levels = ExpArgs . ExpandLevels;
            foreach ( object obj in items . Items )
            {
                if ( AbortExpand )
                    return false;
                iterations++;
                //                Thread . Sleep ( SLEEPTIME );

                ShowProgress ( );
                TreeViewItem childControl = obj as TreeViewItem;
                //stack . Push ( childControl . Tag . ToString ( ) );
                if ( childControl != null )
                {
                    UpdateExpandprogress ( );
                    if ( BreakExpand )
                        break;
                    try
                    {
                        childControl . IsExpanded = true;
                        //stack . Push ( childControl . Tag . ToString ( ) );
                    }
                    catch ( Exception ex ) { }

                    if ( CheckSearchSuccess ( childControl . Tag . ToString ( ) ) == true )
                    {
                        UpdateListBox ( $"Search for {Searchtext . Text} found  as [" + childControl . Header . ToString ( ) + $"]\nin {childControl . Tag . ToString ( )}" );
                        ActiveTree . HorizontalContentAlignment = HorizontalAlignment . Left;
                        ScrollCurrentTvItemIntoView ( childControl );
                        ExpArgs . SearchSuccessItem = childControl;
                        ExpArgs . SearchSuccess = true;
                        childControl . IsSelected = true;
                        fdl . ShowInfo ( Flowdoc , canvas , "Match found !" );
                        return true;
                    }


                    ShowProgress ( );
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
                                    ActiveTree . HorizontalContentAlignment = HorizontalAlignment . Left;
                                    ScrollCurrentTvItemIntoView ( childControl );
                                    childControl . IsSelected = true;
                                    //SearchSuccess = true;
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
                                //                                Console . WriteLine ( Selection . Text );
                                //                                Thread . Sleep ( SLEEPTIME );

                                UpdateListBox ( childControl . Tag . ToString ( ) );
                                if ( ExpandAll3 ( childControl as TreeViewItem , expand , levels ) == true )
                                {
                                    //ActiveTree . HorizontalContentAlignment = HorizontalAlignment . Left;
                                    ScrollCurrentTvItemIntoView ( childControl );
                                    childControl . IsSelected = true;
                                    //SearchSuccess = true;
                                    ExpArgs . SearchSuccess = true;
                                    return true;
                                }
                                ShowProgress ( );
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
                            catch ( Exception ex ) { }
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
                        catch ( Exception ex ) { }
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
            ProgressCount = 0;
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
                    }
                }
            }
            ActiveTree . Refresh ( );
            ProgressCount = 0;
            ProgressString = "Done ...";
            ShowExpandTime ( );
            Mouse . OverrideCursor = Cursors . Arrow;
        }

        // ALL WORKING  REASONABLY CORRECTLY IT APPEARS 14/4/22
        // BUT SOME FILE ENTRIES SEEM TO BE DUPLICATED

        public async void TriggerExpand0 ( object sender , RoutedEventArgs e )
        {
            if ( TestTree . SelectedItem == null )
            {
                MessageBox . Show ( $"Please select a drive or subfolder before using  these options...." , "No Drive Selected" );
                return;
            }
            object [ ] Args = { TestTree . SelectedItem as TreeViewItem , ( object ) 1 , null };
            startitem = TestTree . SelectedItem as TreeViewItem;
            ExpandSetup ( true );
            DirectoryOptions . Focus ( );
            ExpanderMenuOption . Text = "Expand Top Level only of Selected Item only";

            ClearExpandArgs ( );
            ExpArgs . tvitem = ActiveTree . SelectedItem as TreeViewItem;
            ExpArgs . Selection = 0;
            ExpArgs . ExpandLevels = 1;
            await RunExpandSystem ( null , null );
            return;
        }
        public async void TriggerExpand1 ( object sender , RoutedEventArgs e )
        {
            if ( TestTree . SelectedItem == null )
            {
                //MessageBox . Show ( $"Please select a drive or subfolder before using  these options...." , "No Drive Selected" );
                fdl . ShowInfo ( Flowdoc , canvas ,
                      $"Please select a drive or subfolder before using  this option...." ,
                      "Blue1" ,
                      "TreeView Search Sytem" );
                //fdl . SetFocus ( );
                return;
            }
            object [ ] Args = { TestTree . SelectedItem as TreeViewItem , ( object ) 2 , null };
            startitem = TestTree . SelectedItem as TreeViewItem;
            ExpandSetup ( true );
            DirectoryOptions . Focus ( );
            ExpanderMenuOption . Text = "Fully Expand Selected Item 2 levels";
            ClearExpandArgs ( );
            ExpArgs . tvitem = ActiveTree . SelectedItem as TreeViewItem;
            ExpArgs . Selection = 1;
            ExpArgs . ExpandLevels = 2;
            await RunExpandSystem ( null , null );
            return;

            //if ( ExpandCurrentAllLevels ( Args ) == true && TextToSearchFor != "" )
            //    MessageBox . Show ( $"[{Searchtext . Text}] FOUND ...." , "Search System" );
            //Mouse . OverrideCursor = Cursors . Arrow;
            //  ActiveTree . HorizontalContentAlignment = HorizontalAlignment . Left;
            //ScrollCurrentTvItemIntoView ( startitem );
        }
        public async void TriggerExpand2 ( object sender , RoutedEventArgs e )
        {
            if ( TestTree . SelectedItem == null )
            {
                //                MessageBox . Show ( $"Please select a drive or subfolder before using  these options...." , "No Drive Selected" );
                fdl . ShowInfo ( Flowdoc , canvas ,
                      $"Please select a drive or subfolder before using  this option...." ,
                      "Blue1" ,
                      "TreeView Search Sytem" );
                return;
            }
            object [ ] Args = { TestTree . SelectedItem as TreeViewItem , ( object ) 3 , null };
            startitem = TestTree . SelectedItem as TreeViewItem;
            ExpandSetup ( true );
            DirectoryOptions . Focus ( );
            ExpanderMenuOption . Text = "Fully Expand Selected Item 3 levels";
            ClearExpandArgs ( );
            ExpArgs . tvitem = ActiveTree . SelectedItem as TreeViewItem;
            ExpArgs . Selection = 2;
            ExpArgs . ExpandLevels = 3;
            await RunExpandSystem ( null , null );
            return;
        }
        public async void TriggerExpand3 ( object sender , RoutedEventArgs e )
        {
            if ( TestTree . SelectedItem == null )
            {
                //                MessageBox . Show ( $"Please select a drive or subfolder before using  these options...." , "No Drive Selected" );
                fdl . ShowInfo ( Flowdoc , canvas ,
                      $"Please select a drive or subfolder before using  this option...." ,
                      "Blue1" ,
                      "TreeView Search Sytem" );
                return;
            }
            object [ ] Args = { TestTree . SelectedItem as TreeViewItem , ( object ) 4 , null };
            startitem = TestTree . SelectedItem as TreeViewItem;
            ExpandSetup ( true );
            DirectoryOptions . Focus ( );
            ExpanderMenuOption . Text = "Fully Expand Selected Item 4 levels";
            ClearExpandArgs ( );
            ExpArgs . tvitem = ActiveTree . SelectedItem as TreeViewItem;
            ExpArgs . Selection = 3;
            ExpArgs . ExpandLevels = 4;
            await RunExpandSystem ( null , null );
            return;
        }
        public async void TriggerExpand4 ( object sender , RoutedEventArgs e )
        {
            // Open ALL levels
            if ( TestTree . SelectedItem == null )
            {
                //MessageBox . Show ( $"Please select a drive or subfolder before using  these options...." , "No Drive Selected" );
                fdl . ShowInfo ( Flowdoc , canvas ,
                     $"Please select a drive or subfolder before using  this option...." ,
                     "Blue1" ,
                     "TreeView Search Sytem" );
                return;
            }
            if ( MessageBox . Show ( $"This  can take a *** considerable *** time to complete, and access to the application will not be available until it has completed"
                + ".\n\nAre you sure you want to fully expand the current item ?\n\n" , "Potentially Lengthy Expansion request !" , MessageBoxButton . YesNo , MessageBoxImage . Hand , MessageBoxResult . No ) == MessageBoxResult . No )
                return;
            object [ ] Args = { TestTree . SelectedItem as TreeViewItem , ( object ) 90 , null };
            startitem = TestTree . SelectedItem as TreeViewItem;
            ExpandSetup ( true );
            DirectoryOptions . Focus ( );
            ExpanderMenuOption . Text = "Fully Expand Selected Item ALL levels";
            ClearExpandArgs ( );
            ExpArgs . tvitem = ActiveTree . SelectedItem as TreeViewItem;
            ExpArgs . Selection = 4;
            ExpArgs . ExpandLevels = 90;
            ExpArgs . ListResults = false;
            LISTRESULTS = true;

            await RunExpandSystem ( null , null );
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
                        catch ( Exception ex ) { }
                        UpdateListBox ( item2 . Tag . ToString ( ) );
                        ShowProgress ( );
                        ShowExpandTime ( );
                        if ( FullExpandinProgress == false )
                            ActiveTree . Refresh ( );
                    }
                    UpdateExpandprogress ( );
                }
            }
            return false;
        }
        #endregion Expanding

        #region Expanding support methods
        private void UpdateExpandprogress ( )
        {
            if ( Expandprogress . Text . Length >= 12 )
                Expandprogress . Text = ".";
            else
                Expandprogress . Text += ".";
            Thread . Sleep ( 25 );
            Expandprogress . UpdateLayout ( );
            Thread . Sleep ( 25 );
        }
        private static bool IsSystemFile ( string entry )
        {
            if ( entry . Contains ( "BOOT" )
                || entry . Contains ( "SYSTEM VOLUME INFORMATION" )
                || entry . Contains ( "$WINDOWS" )
                || entry . Contains ( "PAGEFILE.SYS" )
                || entry . Contains ( "HIBERFIL.SYS" )
                || entry . Contains ( "DUMPSTACK" )
                || entry . Contains ( ".RND" )
                || entry . Contains ( "$GETCURRENT" )
                || entry . Contains ( "$WINREAGENT" )
                || entry . Contains ( "WINDOWS.OLD" )
                || entry . Contains ( "CONFIG.MSI" )
                || entry . Contains ( "RECOVERY.TXT" )
                || entry . Contains ( "$RECYCLE.BIN" ) == true )
            {
                return true;
            }
            else
                return false;
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
            TestTree . Items . Clear ( );

            foreach ( var drive in Directory . GetLogicalDrives ( ) )
            {
                if ( drive != DriveToLoad && DriveToLoad != "ALL" )
                    continue;
                //                if ( iterator > 1 )
                //                  break;
                iterator++;
                var item = new TreeViewItem ( );
                item . Header = drive;
                item . Tag = drive;
                //                item . Items . Add ( "Loading" );
                //                continue;

                // Add Dummy entry so we get an "Can be Opened" triangle icon
                int dircount = GetDirectories ( drive , out List<string> directories );
                if ( dircount > 0 )
                {
                    item . Items . Add ( "Loading" );

                    // Add Drive to Treeview with dummy "Loading" item
                    TestTree . Items . Add ( item );
                }
                continue;

                dirs = directories;

                foreach ( var directory in dirs )
                {
                    var diritem = new TreeViewItem ( );
                    diritem . Header = directory;
                    diritem . Tag = directory;
                    // Add Dummy entry so we get an "Can be Opened" triangle icon
                    int filecount = GetFiles ( directory , out files );
                    if ( filecount > 0 )
                        diritem . Items . Add ( "Loading" );
                    // Add directory to treeview
                    item . Items . Add ( diritem );
                    //int DirectoryCount = AddDirectoriesToTestTree ( directories , diritem , listBox, true);
                    continue;

                    iterator++;
                    currentdir . Clear ( );
                    currentdir . Add ( directory );

                    if ( filecount > 0 )
                    {
                        foreach ( var file in files )
                        {
                            // Add Files to treeview
                            var subitem = new TreeViewItem ( );
                            subitem . Header = GetFileFolderName ( file );
                            subitem . Tag = file;
                            if ( CheckIsVisible ( file . ToUpper ( ) , ShowAllfiles , out HasHidden ) == true )
                            {     // add the dummy entry to each of the subdirectories we are adding to the tree so we get the Expand icons
                                subitem . Items . Add ( "Loading" );
                                // force it  to iterate  recursively
                                //                                TreeViews tvs = new TreeViews ( );
                                TestTree . Items . Add ( subitem );
                                // Add item to parent

                                //subitem . Expanded += tvs . TreeViewItem4_Expanded;
                            }
                        }
                    }
                    //foreach ( var file in files )
                    //{
                    //    TestTree . Items . Add ( file );
                    //}
                }
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
        }
        public void StartTimer ( )
        {
            startmin = DateTime . Now . Minute;
            startsec = DateTime . Now . Second;
            startmsec = DateTime . Now . Millisecond;
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
            SetExpandDuration ( this , restime );
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
                ProgressCount = 0;
                ProgressString = "";
                Listboxtotal = 0;
                StartTimer ( );
                listBox . Items . Clear ( );
                listBox . Refresh ( );
                Thickness th = new Thickness ( 2 , 2 , 2 , 2 );
                Expandprogress . Text = ".";
                Expandprogress . BorderThickness = th;
                Expandcounter . BorderThickness = th;
                Expandprogress . BorderBrush = FindResource ( "Red5" ) as SolidColorBrush;
                Expandcounter . Foreground = FindResource ( "Yellow1" ) as SolidColorBrush;
                Expandprogress . UpdateLayout ( );
                Expandcounter . BorderBrush = FindResource ( "Red5" ) as SolidColorBrush;
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
                BusyLabel . Visibility = Visibility . Hidden;

                if ( ExpArgs . SearchSuccess )
                    UpdateListBox ( $"\nSearch for {ExpArgs . SearchTerm} completed successfully ..." );
                else
                    UpdateListBox ( $"\nExpansion completed successfully ..." );
                //UpdateListBox ( $"All SubDirectories below {startitem.Tag.ToString()} have been expanded" );
                //UpdateListBox ( $"are shown as White on a Red Backgroud ..." );
                //listBox . ScrollIntoView ( $"All SubDirectories below {startitem . Tag . ToString ( )} have been expanded" );
                TotalItemsExpanded = 0;
                //                Listboxtotal= 0;
                ProgressCount = 0;
                //ProgressString = "";
                ShowExpandTime ( );
                Expandprogress . Refresh ( );
                Expandcounter . Refresh ( );
                UpdateListBox ( "" );
                //if ( TotalItemsExpanded > 0 )
                if ( ExpArgs . SearchSuccess )
                    UpdateListBox ( $"\nAround {Expandcounter . Text } objects have been searched..." );
                else
                    UpdateListBox ( $"\nAround {Expandcounter . Text } objects have been identified..." );
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
                if ( childItem != null )
                    ( ( FrameworkElement ) childItem ) . BringIntoView ( );
                //item.BringIntoView ( );
            }
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
            GetFiles ( folder , out files );
            if ( CheckFileForMatch ( files , upperstring , out resultstring ) == true )
            {
                tvfound . Tag = resultstring;
                tvfound . IsSelected = true;
                return true;
            }
            GetDirectories ( folder , out subfolders );
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
            tv . Refresh ( );
            Mouse . OverrideCursor = Cursors . Arrow;
        }
        private void CollapseDrive (TreeViewItem tv )
        {
            Mouse . OverrideCursor = Cursors . Wait;
            if(tv == null )
                return;
            tv . IsExpanded = false;
            Mouse . OverrideCursor = Cursors . Arrow;
        }
        public bool CheckSearchSuccess ( string currentitem )
        {
            bool result = false;
            currentitem = currentitem . ToUpper ( );
            if ( ExpArgs . SearchSuccess == true || ExpArgs . SearchTerm == "" )
                return false;
            if ( currentitem . Contains ( "BG-BG" ) )
                Console . WriteLine ( $"" );
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
            //        int count = GetDirectories ( item . Tag . ToString ( ) , out directories );

            //        AddDirectoriesToTestTreeview ( directories , item , listBox );
            //        AddFilesToSubdirectory ( item );
            //        item . IsExpanded = true;
            //        TestTree . Refresh ( );
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
            GetFiles ( item . Tag . ToString ( ) , out files );
            if ( item . Items [ 0 ] . ToString ( ) == "Loading" )
                item . Items . Clear ( );
            if ( files . Count > 0 )
            {
                for ( int y = 0 ; y < files . Count ; y++ )
                {
                    item . Items . Add ( files [ y ] );
                    item . IsExpanded = true;
                    ScrollCurrentTvItemIntoView ( item );
                    TestTree . Refresh ( );
                }
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
                    UpdateListBox ( $"\nSearch for {Searchtext . Text} found as [\" + { itms . Header . ToString ( ) }\"] \nin {itms . Tag . ToString ( )}" );
                    itms . IsSelected = true;
                    ScrollCurrentTvItemIntoView ( itms );
                    //                    SearchSuccess = true;
                    ExpArgs . SearchSuccess = true;
                    TestTree . Refresh ( );
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
                            UpdateListBox ( $"Search for {Searchtext . Text} found  as [\" + { itm . Header . ToString ( ) }\"] \nin {itm . Tag . ToString ( )}" );
                            itm . IsSelected = true;
                            ScrollCurrentTvItemIntoView ( itm );
                            //SearchSuccess = true;
                            ExpArgs . SearchSuccess = true;
                            TestTree . Refresh ( );
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
            if ( ExpArgs . ListResults == false )
            {
                if ( listBox . Items . Count == 0 )
                {
                    if ( ExpArgs . SearchActive )
                        listBox . Items . Add ( "Logging to this List is automatically disabled\nfor all Search operations to improve the\nspeed of  the search ..." );
                    else
                    {
                        listBox . Items . Add ( "Logging to this List is currently disabled" );
                        listBox . Items . Add ( "Right click to access Options to change  this..." );
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
            //lbtextblock = tblk;
            int currindex = listBox . Items . Add ( tblk );
            listBox . HorizontalContentAlignment = HorizontalAlignment . Left;
            // Bound in xaml
            Listboxtotal++;
            ShowExpandTime ( );
            if ( entry != "" && entry . Length > 1 )
            {
                if ( entry . Substring ( 1 , 1 ) == ":" )
                    TotalItemsExpanded++;
            }
            listBox . Refresh ( );
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
            int levels = ( int ) Args [ 1 ];
            TreeViewItem items = Args [ 0 ] as TreeViewItem;
            startitem = items;
            fail = false;
            if ( items == null )
                return false;
            ProgressCount = 0;
            string TagString = items . Tag . ToString ( ) . ToUpper ( );
            if ( TagString . Contains ( items . Header . ToString ( ) . ToUpper ( ) ) == false )
                items . Header = items . Tag . ToString ( );
            // Essential to force root to be expanded, else nothing happens
            if ( CheckSearchSuccess ( TagString ) == true )
            {
                UpdateListBox ( $"Search for {Searchtext . Text} found  as [" + items . Header . ToString ( ) + $"]\nin {TagString }" );
                ScrollCurrentTvItemIntoView ( items );
                items . IsSelected = true;
                ExpArgs . SearchSuccessItem = items;
                ExpArgs . SearchSuccess = true;
                TestTree . Refresh ( );
                return true;
            }

            try
            {
                items . IsExpanded = true;
            }
            catch ( Exception ex )
            {
                Console . WriteLine ( $"Expanded=true failed...{ex . Message}" );
            }

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
                if ( CheckSearchSuccess ( childControl . Tag . ToString ( ) ) == true )
                {
                    UpdateListBox ( $"Search for {Searchtext . Text} found  as [" + childControl . Header . ToString ( ) + $"]\nin {childControl . Tag . ToString ( )}" );
                    ScrollCurrentTvItemIntoView ( childControl );
                    childControl . IsSelected = true;
                    ExpArgs . SearchSuccessItem = childControl;
                    ExpArgs . SearchSuccess = true;
                    Returnval = true;
                    IsComplete = true;
                    TestTree . Refresh ( );
                    break;
                }

                try
                {
                    childControl . IsExpanded = true;
                }
                catch ( Exception ex ) { }

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
                            if ( CheckIsVisible ( nextitem . Tag . ToString ( ) . ToUpper ( ) , ShowAllfiles , out HasHidden ) == false )
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
                                TestTree . Refresh ( );
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
                            catch ( Exception ex ) { }
                            ShowProgress ( );
                            // working correctly
                            if ( FullExpandinProgress == false )
                                ActiveTree . Refresh ( );
                            Console . WriteLine ( Selection . Text );
                            levelscount = CalculateLevel ( nextitem . Tag . ToString ( ) );
                            if ( levelscount >= ExpArgs . ExpandLevels )
                            {
                                continue;
                            }
                            UpdateListBox ( nextitem . Tag . ToString ( ) );
                            if ( ExpArgs . ExpandLevels >= 4 )
                            {
                                if ( CheckIsVisible ( nextitem . Tag . ToString ( ) . ToUpper ( ) , ShowAllfiles , out HasHidden ) == false )
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
            testtreebanner . Text = $"{startitem . Tag . ToString ( )} Expanded {ExpArgs . ExpandLevels} levels Successfully...";
            if ( ExpArgs . SearchSuccess == false )
            {
                startitem . IsSelected = true;
                ScrollCurrentTvItemIntoView ( Args [ 0 ] as TreeViewItem );
                TestTree . Refresh ( );
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
            ProgressCount = 0;
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
                TestTree . Refresh ( );
                return Task . FromResult ( success );
            }

            try
            {
                items . IsExpanded = true;
            }
            catch ( Exception ex ) { }

            ShowProgress ( );
            if ( FullExpandinProgress == false )
                TestTree . Refresh ( );
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
                catch ( Exception ex ) { }
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
                        TestTree . Refresh ( );
                    iterations++;
                    if ( ExpArgs . ExpandLevels >= 3 )
                    {
                        //******************
                        // INNER LOOP
                        //******************
                        foreach ( TreeViewItem nextitem in childControl . Items )
                        {
                            ShowProgress ( );
                            if ( CheckIsVisible ( nextitem . Tag . ToString ( ) . ToUpper ( ) , ShowAllfiles , out HasHidden ) == false )
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
                            catch ( Exception ex ) { }
                            ShowProgress ( );
                            if ( FullExpandinProgress == false )
                                TestTree . Refresh ( );

                            Console . WriteLine ( Selection . Text );
                            levelscount = CalculateLevel ( nextitem . Tag . ToString ( ) );
                            if ( levelscount >= ExpArgs . ExpandLevels )
                            {
                                continue;
                            }
                            UpdateListBox ( nextitem . Tag . ToString ( ) );
                            if ( ExpArgs . ExpandLevels >= 4 )
                            {
                                if ( CheckIsVisible ( nextitem . Tag . ToString ( ) . ToUpper ( ) , ShowAllfiles , out HasHidden ) == false )
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
                                TestTree . Refresh ( );
                        }   // End INNER FOREACH

                        if ( IsComplete )
                            break;
                    }
                    ShowExpandTime ( );
                }
                if ( FullExpandinProgress == false )
                    TestTree . Refresh ( );
                ShowProgress ( );
                if ( IsComplete )
                    break;
            }   // End FOREACH

            ShowExpandTime ( );
            ExpandSetup ( false );
            Expandprogress . Refresh ( );
            testtreebanner . Text = $"{startitem . Tag . ToString ( )} Expanded {ExpArgs . ExpandLevels} levels Successfully...";
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
            TreeViewItem item = e . Source as TreeViewItem;
            item . Items . Clear ( );
            item . Items . Add ( "Loading" );
            //            item . Items . Add ( "Loading" );
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
            catch ( Exception ex )
            { }
            return null;
        }

        private void worker_ProgressChanged ( object sender , ProgressChangedEventArgs e )
        {
            TreeViewItem tvnew = GetParentNode ( ExpArgs . tvitem );

            RunRecurse ( e );

            //// All done;
            Mouse . OverrideCursor = Cursors . Arrow;
            if ( ExpArgs . SearchActive && ExpArgs . SearchSuccess )
                //MessageBox . Show ( $"{ExpArgs . SearchTerm} has been identified" , "TreeView Search facilitiy" );
                fdl . ShowInfo ( Flowdoc , canvas , $"The 1st instance of the Search term shown below has been identified successfully, and is highlighted for you ..." , "Red5" , $"[{ExpArgs . SearchTerm}] " , "Black0" , "Match found !" , "Cyan5" , "TreeView Search Sytem" );
            else if ( ExpArgs . SearchActive )
                fdl . ShowInfo ( Flowdoc , canvas , $"Sorry, but the Search term shown below could not be identified for you ..." , "Black0" , $"[{ExpArgs . SearchTerm}] " , "Red5" , "NO Match found !" , "Cyan5" , "TreeView Search Sytem" );

            // Called once recurse methods have completed
            if ( worker . IsBusy == false )
            {
                TreeViewItem tvi = new TreeViewItem ( );
                Console . WriteLine ( $"1 Recurse finished" );
                try
                {
                    if ( ShowVolumeLabels == true )
                    {
                        if ( ExpArgs . Parent != null )
                            tvi = ExpArgs . Parent;
                        else
                            tvi = ExpArgs . tvitem;
                        TreeViewItem caller = tvi as TreeViewItem;
                        if ( tvi . Tag . ToString ( ) . Length > 3 )
                        {
                            caller = tvi . Parent as TreeViewItem;
                        }
                        if ( caller . Tag . ToString ( ) . Length == 3 && caller . Tag . ToString ( ) . Contains ( "\\" ) )
                        {
                            string s = GetDriveInfo ( $"{caller . Header . ToString ( )}" );
                            caller . Header = $"{caller . Header}  [{s}]";
                        }
                    }
                }
                catch ( Exception ex ) { Console . WriteLine ( $"Parent name parsing of {tvi . Tag . ToString ( )} failed :-\n{ex.Message}" ); }
            }
            //Tidy up after ourselves
            ClearExpandArgs ( );
        }
        public void RunRecurse ( ProgressChangedEventArgs e )
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

            // Force search to be upper case
            ExpArgs . SearchTerm = ExpArgs . SearchTerm . ToUpper ( );
            //ExpandArgs eargs = e . UserState as ExpandArgs;
            //int levels = ( int ) Args [ 1 ];
            int levels = ExpArgs . ExpandLevels;

            //TreeViewItem items = TestTree . SelectedItem as TreeViewItem;
            TreeViewItem items = ExpArgs . tvitem;
            startitem = items;
            fail = false;
            if ( items == null )
                return;
            ProgressCount = 0;
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
            catch ( Exception ex ) { }
            Thread . Sleep ( SLEEPTIME );
            ShowProgress ( );
            if ( FullExpandinProgress == false )
                items . Refresh ( );
            levelscount = CalculateLevel ( items . Tag . ToString ( ) );
            if ( levelscount >= ExpArgs . ExpandLevels )
            {
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
                catch ( Exception ex ) { }
                ShowProgress ( );
                levelscount = CalculateLevel ( childControl . Tag . ToString ( ) );
                if ( levelscount >= ExpArgs . ExpandLevels )
                {
                    if ( ExpArgs . SearchTerm != "" && ClosePreviousNode )
                    {
                        childControl . IsExpanded = false;
                        TestTree . Refresh ( );
                    }
                    continue;
                }
                if ( childControl != null )//&& ExpArgs . ExpandLevels >= 2 )
                {
                    string entry = childControl . Header . ToString ( ) . ToString ( ) . ToUpper ( );
                    itemcount = childControl . Items . Count;
                    ShowProgress ( );
                    if ( FullExpandinProgress == false )
                        TestTree . Refresh ( );
                    iterations++;
                    if ( ExpArgs . ExpandLevels >= 3 )
                    {
                        //******************
                        // INNER LOOP
                        //******************
                        //UpdateListBox ( childControl. Tag . ToString ( ) );

                        UpdateListBox ( childControl . Tag . ToString ( ) );
                        foreach ( TreeViewItem nextitem in childControl . Items )
                        {
                            ShowProgress ( );
                            if ( CheckIsVisible ( nextitem . Tag . ToString ( ) . ToUpper ( ) , ShowAllfiles , out HasHidden ) == false )
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
                                nextitem . IsExpanded = true;
                                if ( AbortExpand )
                                    return;
                            }
                            catch ( Exception ex ) { }
                            ShowProgress ( );
                            // working correctly
                            if ( FullExpandinProgress == false )
                                TestTree . Refresh ( );
                            Console . WriteLine ( Selection . Text );
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
                                if ( CheckIsVisible ( nextitem . Tag . ToString ( ) . ToUpper ( ) , ShowAllfiles , out HasHidden ) == false )
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
                                            TestTree . Refresh ( );
                                        }
                                        //              UpdateListBox ( nextitem . Tag . ToString ( ) );
                                    }
                                }
                            }
                            else
                                UpdateListBox ( nextitem . Tag . ToString ( ) );

                            ShowExpandTime ( );
                            ShowProgress ( );
                            if ( FullExpandinProgress == false )
                                TestTree . Refresh ( );
                        }   // End INNER FOREACH

                        if ( ClosePreviousNode && ExpArgs . SearchActive == true && ExpArgs . SearchSuccess == false )
                        {
                            // ONLY If Searching , Close the subdir we have just finished prcessing
                            childControl . IsExpanded = false;
                            TestTree . Refresh ( );
                        }
                        if ( IsComplete )
                            break;
                    }
                    ShowExpandTime ( );
                }
                if ( FullExpandinProgress == false )
                    TestTree . Refresh ( );
                ShowProgress ( );
                if ( IsComplete )
                    break;
            }   // End FOREACH

            ShowExpandTime ( );
            ExpandSetup ( false );
            Expandprogress . Refresh ( );
            testtreebanner . Text = $"{startitem . Tag . ToString ( )} Expanded {ExpArgs . ExpandLevels} levels Successfully...";
            if ( ExpArgs . SearchSuccess == false )
            {
                startitem . IsSelected = true;
                ScrollCurrentTvItemIntoView ( Args [ 0 ] as TreeViewItem );
            }
            //if(worker.IsBusy == false)
            //    Console . WriteLine ($"1 Recurse finished");
            //else
            //    Console . WriteLine ( $"2 Recurse finished" );
            return;


        }
        #endregion BackgroundWorker
        public int AddDirectoriesToTestTree ( List<string> directories , TreeViewItem item , ListBox lBox = null , bool UseExpand = true )
        {
            int added = 0;
            item . Items . Clear ( );
            foreach ( var directoryPath in directories )
            {
                //directories . ForEach ( directoryPath =>
                //{
                var dummy = new TreeViewItem ( );
                var subitem = new TreeViewItem ( );
                subitem . Header = GetFileFolderName ( directoryPath );
                subitem . Tag = directoryPath;
                UpdateListBox ( directoryPath . ToUpper ( ) );
                if ( CheckIsVisible ( directoryPath . ToUpper ( ) , ShowAllfiles , out HasHidden ) == true )
                {     // add the dummy entry to each of the subdirectories we are adding to the tree so we get the Expand icons
                    if ( item . Tag . ToString ( ) . ToUpper ( ) . Contains ( "BIGFONT" ) )
                        Console . WriteLine ( $"" );
                    item . Items . Add ( subitem );

                    dummy . Header = "Loading";
                    subitem . Items . Add ( dummy );
                    //                    AllCheckedFolders . Add ( subitem );
                    item . IsExpanded = true;
                    ScrollCurrentTvItemIntoView ( subitem );
                    if ( FullExpandinProgress == false )
                        ActiveTree . Refresh ( );
                    //                    Console . WriteLine ( $"3 - ADTT : Added Subdir {subitem . Tag . ToString ( )} to expanded {item . Tag . ToString ( )}" );
                    added++;
                    if ( CheckSearchSuccess ( item . Tag . ToString ( ) ) == true )
                    {
                        UpdateListBox ( $"Search for {Searchtext . Text} found  as [" + item . Header . ToString ( ) + $"]\nin {item . Tag . ToString ( )}" );
                        ScrollCurrentTvItemIntoView ( item );
                        item . IsSelected = true;
                        ExpArgs . SearchSuccessItem = item;
                        //SearchSuccess = true;
                        ExpArgs . SearchSuccess = true;
                        TestTree . Refresh ( );
                        break;
                    }
                }
                ShowProgress ( );
                //});
            }
            return added;
        }
        public int AddFilesToRecurse ( List<string> Allfiles , TreeViewItem item )
        {
            int count = 0;
            //            TreeViewItem tmp = item . Items [ 0 ] as TreeViewItem;
            var subitemctrl = new TreeViewItem ( );
            if ( item . Items [ 0 ] . ToString ( ) == "Loading" )
                item . Items . Clear ( );
            foreach ( var itm in Allfiles )
            {
                ShowProgress ( );
                var subitem = new TreeViewItem ( )
                {
                    Header = GetFileFolderName ( itm ) ,
                    Tag = itm
                };
                if ( CheckIsVisible ( itm . ToUpper ( ) , ShowAllfiles , out HasHidden ) == true )
                {
                    item . Items . Add ( subitem );
                    item . IsExpanded = true;
                    subitem . IsSelected = true;
                    ScrollCurrentTvItemIntoView ( subitem );
                    TestTree . Refresh ( );
                    subitemctrl = subitem;                    //item . IsExpanded = true;
                                                              //                    Console . WriteLine ( $"3 - ADTR : Added {subitem . Tag . ToString ( )} to {item . Tag . ToString ( )} &  scrolled" );
                    count++;
                    if ( CheckSearchSuccess ( subitem . Tag . ToString ( ) ) == true )
                    {
                        UpdateListBox ( $"\nSearch for {Searchtext . Text} found  as [" + subitem . Header . ToString ( ) + $"]\nin {subitem . Tag . ToString ( )}" );
                        if ( subitem . IsSelected == false )
                            subitem . IsSelected = true;
                        ScrollCurrentTvItemIntoView ( subitem );
                        TestTree . Refresh ( );
                        ExpArgs . SearchSuccessItem = subitem;
                        //SearchSuccess = true;
                        ExpArgs . SearchSuccess = true;
                        Mouse . OverrideCursor = Cursors . Arrow;
                        break;
                    }

                }
                if ( Allfiles . Count > 0 )
                {
                    item . IsExpanded = true;
                    subitemctrl . IsSelected = true;
                    ScrollCurrentTvItemIntoView ( subitemctrl );
                    if ( FullExpandinProgress == false )
                        TestTree . Refresh ( );
                }
                ShowProgress ( );
            }
            return count;
        }
        private void
         SearchTree ( object sender , RoutedEventArgs e )
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
                ExpArgs . Selection = 4;
                ExpArgs . ExpandLevels = LevelsCombo . SelectedIndex + 3;
                ExpArgs . SearchTerm = Searchtext . Text . ToUpper ( );
                ExpArgs . ListResults = false;
                ExpArgs . SearchActive = true;

                RunExpandSystem ( null , null );
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
            // TreeView tv = ExpArgs . tv;
            //SearchTerm = ExpArgs . SearchTerm;
            // RunRecurseItem ( ExpArgs . tvitem , SearchTerm , ClosePrevious );
            // SearchTerm = "";
            // ExpArgs . SearchSuccess = false;
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


            if ( ExpArgs . SearchTerm == "SEARCH FOR...." )
            {
                MessageBox . Show ( "No search term entered, so Search has been aborted" , "User Error" );
                return null;
            }
            // Allow it  to unwind gracefully
            if ( ExpArgs . SearchSuccess )
                return ExpArgs . SearchSuccessItem;
            List<String> directories = new List<string> ( );
            List<String> AllFiles = new List<string> ( );
            TreeViewItem currentitem = new TreeViewItem ( );
            currentitem = tvitem;
            // Add all content to current folder
            // Get root level Subdirs next, it may provde a search match
            int count = GetDirectories ( currentitem . Tag . ToString ( ) , out directories );
            if ( count > 0 )
            {
                AddDirectoriesToTestTree ( directories , tvitem );
                if ( ExpArgs . SearchSuccess )
                    return ExpArgs . SearchSuccessItem;
                if ( ClosePreviousNode )
                    tvitem . IsExpanded = false;
            }
            // Get root level files 1st, it may provde a search match
            GetFiles ( currentitem . Tag . ToString ( ) , out AllFiles );
            if ( AllFiles . Count > 0 )
            {
                AddFilesToRecurse ( AllFiles , currentitem );
                if ( ExpArgs . SearchSuccess )
                {
                    currentitem . IsSelected = true;
                    return ExpArgs . SearchSuccessItem;
                }
            }
            //Finally, iterate thru subdirs 
            currentitem . IsExpanded = true;
            foreach ( var subItem in currentitem . Items )
            {
                TreeViewItem tvo = new TreeViewItem ( );
                tvo = subItem as TreeViewItem;
                if ( tvo . HasItems == true && CheckSearchSuccess ( tvo . Tag . ToString ( ) ) == true )
                {
                    UpdateListBox ( $"\"nSearch for {Searchtext . Text} found  as [" + tvo . Header . ToString ( ) + $"]\nin {tvo . Tag . ToString ( )}" );
                    tvo . IsSelected = true;
                    //    SearchSuccess = true;
                    ExpArgs . SearchSuccess = true;
                    ExpArgs . SearchSuccessItem = tvo;
                    ScrollCurrentTvItemIntoView ( tvo );
                    TestTree . Refresh ( );
                    Mouse . OverrideCursor = Cursors . Arrow;
                    break;
                }
                else
                {
                    if ( ClosePrevious )
                        tvo . IsExpanded = false;
                    if ( tvo . HasItems == false )
                        continue;
                    //                  Console . WriteLine ( $"RI Expanded :{tvo . Tag . ToString ( )} - calling Ri for item {tvo . Header . ToString ( )}...." );

                    RunRecurseItem ( tvo , SearchTerm , ClosePrevious );
                    if ( ExpArgs . SearchSuccess == true )
                        return ExpArgs . SearchSuccessItem;
                    //                    AllCheckedFolders . Add ( tvo );
                }
            }
            if ( ExpArgs . SearchSuccess )
                return ExpArgs . SearchSuccessItem;
            // Close the subdirectory we have completed processing on - for neatness - Works well 25/4/22
            if ( ClosePrevious )
                currentitem . IsExpanded = false;
            return null;
        }
        private void ExactMatch_Click ( object sender , RoutedEventArgs e )
        {
            Exactmatch = ( bool ) ExactMatch . IsChecked;
        }
        private async void Expand_Click ( object sender , RoutedEventArgs e )
        {
            ClearExpandArgs ( );
            ExpArgs . Selection = DirectoryOptions . SelectedIndex;
            await RunExpandSystem ( sender , e );
        }
        /// <summary>
        /// Process Expannd request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task<bool> RunExpandSystem ( object sender , RoutedEventArgs e )
        {
            double temp = 0;
            ComboBox cb = DirectoryOptions;
            //int selindex = DirectoryOptions . SelectedIndex;
            int selindex = ExpArgs . Selection;
            string original = "";
            iterations = 0;
            listboxtotal = 0;
            TreeViewItem root = ActiveTree . SelectedItem as TreeViewItem;
            TreeViewItem caller = ActiveTree . SelectedItem as TreeViewItem;
            if ( caller == null )
                caller = root;
            if ( root != null )
            {
                if ( root . HasItems == false )
                {
                    Mouse . OverrideCursor = Cursors . Arrow;
                    MessageBox . Show ( "The current item is only a file (or contains only Hidden items)\nand cannot threfore be expanded.\n\nPlease select a Valid Drive or Folder in the TreeView before using these options...." ,
                        "Invalid Current Selection" );
                    BusyLabel . Visibility = Visibility . Hidden;
                    return false;
                }
                Args [ 0 ] = root as object;
                //TreeViewObject = (object)root;
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
            listBox . Items . Clear ( );

            Console . WriteLine ( $"User Selection in use = {ExpArgs . Selection }" );
            if ( ExpArgs . tvitem == null )
            {
                Mouse . OverrideCursor = Cursors . Arrow;
                //MessageBox . Show ( "Please a Drive or Folder in the TreeView before using these options...." );
                fdl . ShowInfo ( Flowdoc , canvas ,
                      $"Please select a drive or subfolder before using  this option...." ,
                      "Blue1" ,
                      "TreeView Search Sytem" );
                BusyLabel . Visibility = Visibility . Hidden;
                return false;
            }

            if ( selindex == 0 )   // Expand  2  levels
            {
                //Args [ 1 ] = 2;
                if ( ExpArgs . ExpandLevels == 0 )
                    ExpArgs . ExpandLevels = 2;

                UpdateListBox ( $"Expanding {original} {ExpArgs . ExpandLevels} levels...." );
                //InfoList . Items . Add ( $"Expanding {original} {Args [ 1 ]} levels...." );
            }
            else if ( selindex == 1 )
            {
                //Args [ 1 ] = 3;   // Expand  3  levels
                if ( ExpArgs . ExpandLevels == 0 )
                    ExpArgs . ExpandLevels = 3;
                UpdateListBox ( $"Expanding {original} {ExpArgs . ExpandLevels } levels...." );
                //InfoList . Items . Add ( $"Expanding {original} {ExpArgs . ExpandLevels } levels...." );
            }
            else if ( selindex == 2 )
            {
                //Args [ 1 ] = 4;   // Expand  4  levels
                if ( ExpArgs . ExpandLevels == 0 )
                    ExpArgs . ExpandLevels = 4;
                TreeViewItem tvi = ( TreeViewItem ) ExpArgs . tvitem;
                //                TreeViewItem tvi = ( TreeViewItem ) Args [ 0 ];
                string str = tvi . Tag . ToString ( );
                if ( MessageBox . Show ( $"Expanding {str} down {ExpArgs . ExpandLevels } levels may take some time, Are you  sure you want  to continue ?" , "Expansion System Warning !" ,
                    MessageBoxButton . YesNo ) == MessageBoxResult . No )
                {
                    Mouse . OverrideCursor = Cursors . Arrow;
                    BusyLabel . Visibility = Visibility . Hidden;
                    return false;
                }
                UpdateListBox ( $"Expanding {original} {Args [ 1 ]} levels...." );
                //InfoList . Items . Add ( $"Expanding {original} {Args [ 1 ]} levels...." );
                // inhibit listbox
                ExpArgs . ListResults = LISTRESULTS;
            }
            else if ( selindex == 3 )
            {
                //Args [ 1 ] = 90;
                if ( ExpArgs . ExpandLevels == 0 )
                    ExpArgs . ExpandLevels = 90;
                UpdateListBox ( $"Expanding {original} {Args [ 1 ]} levels...." );
                //InfoList . Items . Add ( $"Expanding ALL  levels...." );
                // inhibit listbox
                ExpArgs . ListResults = LISTRESULTS;
            }
            else if ( selindex == 6 )
            {
                // Expand ALL below  current drive
                //                InfoList . Items . Add ( "Calling EXPANDALLRECURSIVE" );
                ExpArgs . tvitem = ActiveTree . SelectedItem as TreeViewItem;
                if ( ExpArgs . ExpandLevels == 0 )
                    ExpArgs . ExpandLevels = 90;
                // inhibit listbox
                ExpArgs . ListResults = LISTRESULTS;

                ExpandSetup ( true );
                UpdateListBox ( $"Expanding ALL items BELOW current...." );
                //                InfoList . Items . Add ( $"Expanding ALL items BELOW current..." );
                if ( ExpandCurrentAllLevels ( Args ) == true && TextToSearchFor != "" )
                    MessageBox . Show ( $"[{Searchtext . Text}] FOUND ...." , "Search System" );
                ExpandSetup ( false );
                ScrollCurrentTvItemIntoView ( ( TreeViewItem ) Args [ 0 ] );
                return true;
            }
            else if ( selindex == 7 )
            {
                // Collapse All Drives
                ExpArgs . tv = ActiveTree;
                ExpandSetup ( true );
                //Dispatcher . BeginInvoke ( new Action ( ( ) =>
                //{
                CollapseAllDrives ( );
                //} ) , DispatcherPriority . ApplicationIdle );
                Mouse . OverrideCursor = Cursors . Arrow;
                ExpandSetup ( false );
                ScrollCurrentTvItemIntoView ( ( TreeViewItem ) ActiveTree . Items [ 0 ] );
                return true;
            }

            // go ahead
            TreeViewItem tview = new TreeViewItem ( );
            tview = ExpArgs . tvitem;
            tview . IsExpanded = false;
            tview . IsSelected = true;
            ActiveTree . Refresh ( );
            //Expand current 2 levels
            ExpandSetup ( true );

            worker = new BackgroundWorker ( );
            worker . WorkerSupportsCancellation = true;
            worker . WorkerReportsProgress = true;
            //new DoWorkEventArgs ( Args);
            //object TreeViewObject = Args [ 0 ];
            worker . DoWork += worker_DoWork;
            worker . ProgressChanged += worker_ProgressChanged;
            worker . RunWorkerCompleted += worker_RunWorkerCompleted;
            //args [ 0 ] = TreeViewObject as TreeViewItem;
            worker . RunWorkerAsync ( ExpArgs );
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
            catch ( Exception ex )
            { }
            return true;
        }
        private void TestTree_Expanded ( object sender , RoutedEventArgs e )
        {
            // All working when clicking on any folder !!!!
            string currentHeader = "";
            // Needed to let us show the volume label if the option is checked
            TreeViewItem Caller = new TreeViewItem ( );
            TreeViewItem item = null;
            int itemscount = 0;
            if ( e != null )
                item = e . Source as TreeViewItem;
            else
                item = sender as TreeViewItem;
            if ( item == null )
                return;
            Caller = item;
            currentHeader = item . Header . ToString ( );
            item . Header = item . Tag . ToString ( );

            // This is CRITICAL to get any drive that is currently selected to open when the expand icon is clicked

            item . IsSelected = true;
            if ( item . Header . ToString ( ) == "Loading" )
            {
                Caller . Header = currentHeader;
                return;
            }
            Selection . Text = $"{item . Tag . ToString ( )}";
            ActiveTree . HorizontalContentAlignment = HorizontalAlignment . Left;
            ScrollCurrentTvItemIntoView ( item );
            ActiveTree . Refresh ( );

            var directories = new List<string> ( );
            var Allfiles = new List<string> ( );
            string Fullpath = item . Tag . ToString ( ) . ToUpper ( );
            int DirectoryCount = 0, filescount = 0;
            itemscount = item . Items . Count;
            var tvi = item as TreeViewItem;
            if ( itemscount == 0 )
            {
                Caller . Header = currentHeader;
                return;
            }
            //TreeViewItem tmp = item. Items [ 0 ] as TreeViewItem;
            var itemheader = item . Items [ 0 ] . ToString ( );
            //  UpdateListBox ( $"{item . Tag . ToString ( )}" );

            GetFiles ( Fullpath , out Allfiles );
            if ( Allfiles . Count > 0 )
            {
                filescount = Allfiles . Count;
                if ( filescount > 500 )
                {
                    MessageBoxResult result = MessageBox . Show ( $"Directory {Fullpath} contains {filescount} Files\nExpanding these will take a considerable time...\n\nAre you sure you want to continue ?" ,
                     "Potential long delay" , MessageBoxButton . YesNoCancel , MessageBoxImage . Warning , MessageBoxResult . Cancel );
                    if ( result == MessageBoxResult . Yes )
                    {
                        item . Items . Clear ( );
                        AddFilesToTreeview ( Allfiles , item );
                    }
                    else if ( result == MessageBoxResult . Cancel )
                    {
                        AbortExpand = true;
                        {
                            Caller . Header = currentHeader;
                            return;
                        }
                    }
                    else
                    {
                        ExpandLimited = true;
                        {
                            Caller . Header = currentHeader;
                            return;
                        }
                    }
                }
                else
                {
                    item . Items . Clear ( );
                    AddFilesToTreeview ( Allfiles , item );
                }
            }
            //            Console . WriteLine ( $"TT.Added - {filescount} Files to {item . Tag . ToString ( )}" );
            // Get a list of all items in the current folder
            int count = GetDirectories ( Fullpath , out directories );
            if ( count > 0 )
            {
                if ( count > 250 )
                {
                    MessageBoxResult result = MessageBox . Show ( $"Directory {Fullpath} contains {count} Files\nExpanding these will take a considerable time...\n\nAre you sure you want to continue ?" ,
                     "Potential long delay" , MessageBoxButton . YesNoCancel , MessageBoxImage . Warning , MessageBoxResult . Cancel );
                    if ( result == MessageBoxResult . Yes )
                    {
                        // Remove DUMMY entry
                        if ( itemheader == "Loading" )
                            item . Items . Clear ( );
                        DirectoryCount = count;
                        ShowProgress ( );
                        DirectoryCount = AddDirectoriesToTestTreeview ( directories , item , listBox );
                    }
                    else if ( result == MessageBoxResult . Cancel )
                    {
                        AbortExpand = true;
                        {
                            Caller . Header = currentHeader;
                            return;
                        }
                    }
                    else
                    {
                        ExpandLimited = true;
                        {
                            Caller . Header = currentHeader;
                            return;
                        }
                    }
                }
                else
                {
                    DirectoryCount = count;
                    ShowProgress ( );
                    if ( directories . Count > 0 )
                        if ( item . Items [ 0 ] . ToString ( ) == "Loading" )
                        {
                            //                       if ( tmp . Header . ToString ( ) == "Loading" )
                            item . Items . Clear ( );
                        }
                    {
                        item . IsExpanded = true;
                        ActiveTree . HorizontalContentAlignment = HorizontalAlignment . Left;
                        ScrollCurrentTvItemIntoView ( item );
                        TestTree . Refresh ( );
                        DirectoryCount = AddDirectoriesToTestTreeview ( directories , item , listBox );
                        item . IsExpanded = true;
                    }
                }
                //                Console . WriteLine ( $"TT.Expanded - {count} Subdirs to {item . Tag . ToString ( )}" );
            }
            else
            {
                DirectoryCount = 0;
                ShowProgress ( );
            }
            TestTree . UpdateLayout ( );
            //            Selection . Text = $"Expanding : {Fullpath} ...";
            Caller . Header = currentHeader;
            TestTree . Refresh ( );
            ShowProgress ( );
            return;
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
            ExpArgs . Parent = null;
        }

        private void TreeOptions ( object sender , RoutedEventArgs e )
        {
            OptionsPanel . Visibility = Visibility . Visible;
        }
        private void Image_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
        {
            if ( Opt1cbox . IsChecked == true )
                ClosePreviousNode = true;
            else
                ClosePreviousNode = false;
            if ( Opt2cbox . IsChecked == true )
                LISTRESULTS = true;
            else
                LISTRESULTS = false;
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
            CheckBox cb = sender as CheckBox;
            ShowVolumeLabels = ( bool ) cb . IsChecked;
            LoadDrives ( ActiveTree );
        }
        private void MenuItem_Click ( object sender , RoutedEventArgs e )
        {
            OptionsPanel . Visibility = Visibility . Visible;
        }

        private void CollapseAll ( object sender , RoutedEventArgs e )
        {
            CollapseAllDrives ( );
        }

        private void CollapseCurrent ( object sender , RoutedEventArgs e )
        {
            if ( ExpArgs . Parent != null )
                CollapseTree (ExpArgs.Parent, e );
        }
    }
} // End of CLASS TreeViews
  //public class MyVirtualizingStackPanel : VirtualizingStackPanel
  //{
  //    /// <summary>
  //    /// Publically expose BringIndexIntoView.
  //    /// </summary>
  //    public void BringIntoView ( int index )
  //    {

//        this . BringIndexIntoView ( index );
//    }
//}

/*
ExpArgs . tv =
ExpArgs . tvitem =
ExpArgs . ExpandLevels =
ExpArgs . SearchTerm =
ExpArgs . ClosePrevious =
ExpArgs . Selection =
*/