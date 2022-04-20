#define DEBUGEXPAND
#undef DEBUGEXPAND

using MyDev . Converts;
using MyDev . Models;
using MyDev . UserControls;
using MyDev . ViewModels;
using System . IO;
using Newtonsoft . Json . Linq;

using System;
using System . Collections . Generic;
using System . ComponentModel;
using System . Diagnostics;
using System . IO;
using System . Linq;
using System . Runtime . InteropServices . Expando;
using System . Security . Policy;
using System . Text;
using System . Threading;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Documents;
using System . Windows . Input;
using System . Windows . Markup . Localizer;
using System . Windows . Media;
using System . Windows . Media . Imaging;
using System . Windows . Shapes;
using System . Windows . Threading;
using static MyDev . Views . TreeViews;


namespace MyDev . Views
{

    public partial class TreeViews : Window, INotifyPropertyChanged
    {
        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged ( string PropertyName )
        {
            if ( null != PropertyChanged )
            {
                PropertyChanged ( this ,
                      new PropertyChangedEventArgs ( PropertyName ) );
            }
        }
        #endregion OnPropertyChanged
        public static int PROGRESSWRAPVALUE = 48;
        public static TreeViewItem CurrentTreeItem { get; set; }
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
        public ICommand WalkTreeViewItem { get; set; }
        public struct ExpandArgs
        {
            public int Levels;
            public TreeView tv;
            public TreeViewItem tvitem;
        };
        //public  static ExpandArgs ExpanderArgs { get; set; }
        public static bool BreakExpand { get; set; } = false;

        #region Public dclarations
        //		public static LazyLoading Lazytree = null;
        public List<Family> families = new List<Family> ( );
        public Family family1 = new Family ( );
        public static List<string> LbStrings = new List<string> ( );
        public bool LoggingToListbox { get; set; }
        //		public static ListBox Tvlistbox = new ListBox();
        public static bool ShowAllfiles = false;
        public string CurrentDrive { set; get; }
        TreeViewItem CurrentItem { get; set; }
        DirectoryInfo DirInfo = new DirectoryInfo ( @"C:\\" );
        public static TreeViews treeViews { get; set; }
        public static bool ExpandAllFolders { get; set; } = false;
        private static double TimeElapsed { get; set; }
        private static DispatcherTimer sw = new DispatcherTimer ( );
        private static List<TreeViewItem> nextfolder { get; set; }
        //        private TreeNodeViewModel tnvm = new TreeNodeViewModel ( );
        public List<String> DirOptions { get; set; }
        public bool Loading = true;
        public struct lbitemtemplate
        {
            public string Colm1 { get; set; }
            public string Colm2 { get; set; }
            public string Colm3 { get; set; }
            public string Colm4 { get; set; }
            public string Colm5 { get; set; }
            public string Colm6 { get; set; }
        };
        public static lbitemtemplate lbtmp { get; set; }

        List<ComboBoxItem> DirectoryOptions2 = new List<ComboBoxItem> ( );
        public static List<string> ValidFiles = new List<string> ( );
        public int maxlevels { get; set; }
        private static double startmin = 0;
        private static double startsec = 0;
        private static double startmsec = 0;
        private TreeViewItem startitem { get; set; }

        #endregion Public dclarations

        private static bool isresettingSelection { get; set; } = false;
        private static FlowdocLib fdl;
        #region startup
        public TreeViews ( )
        {
            InitializeComponent ( );
            this . DataContext = this;

            // Cannot use  this with FlowDoc cos of dragging/Resizing
            //Utils . SetupWindowDrag ( this );
            WalkTreeViewItem = new RelayCommand ( ExecuteWalkTreeViewItem , CanExecuteWalkTreeViewItem );
            ActiveTree = TestTree;
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
            //            treeViewModel . SetValue ( SelectedItemProperty , tvi );
        }

        private void ExecuteWalkTreeViewItem ( object obj )
        {
            if ( maxlevels == 99 && obj == null )
            {
                var sel = DirectoryOptions . SelectedIndex;
                ProgressString = "";
                ProgressCount = 0;
                if ( sel < 5 )
                    maxlevels = sel + 2;

                //else if ( sel == 1 )
                //    maxlevels = 3;
                //else if ( sel == 2 )
                //    maxlevels = 4;
                //else if ( sel == 3 )
                //    maxlevels = 5;
                //else if ( sel == 4 )
                //    maxlevels = 6;
                else if ( sel == 5 )
                {
                    object [ ] Args = new object [ 3 ];
                    TreeViewItem tvisel = new TreeViewItem ( );
                    tvisel = TestTree . SelectedItem as TreeViewItem;
                    Args [ 0 ] = ( object ) tvisel;

                    WalkTestTree ( Args );
                    ProgressCount = 0;
                    ProgressString = "Done ...";
                    return;
                }
                else if ( sel == 6 )
                {
                    object [ ] Args = new object [ 3 ];
                    Args [ 0 ] = TestTree;
                    CollapseAllDrives ( Args );
                    return;
                }
            }
            else
            {
                int levels = Convert . ToInt32 ( obj );
                maxlevels = levels;
            }
            TestTree . Focus ( );
            Mouse . OverrideCursor = Cursors . Wait;
            TreeViewItem tvi = TestTree . SelectedItem as TreeViewItem;
            if ( tvi != null )
            {
                testtreebanner . Text = $"Please wait, Expanding {tvi . Tag . ToString ( )} ...";
                testtreebanner . Refresh ( );
                //                Thread . Sleep ( 100 );
            }
            ProgressString = "";
            ProgressCount = 0;

            //            WalkTestTree ( Args );
            Mouse . OverrideCursor = Cursors . Arrow;
            if ( tvi != null )
            {
                testtreebanner . Text = $"{tvi . Tag . ToString ( )} Fully Expanded...";
                //enditem . IsSelected = true;
                TestTree . Focus ( );
                TestTree . SetCurrentValue ( SelectedItemProperty , tvi );
            }
            ProgressCount = 0;
            ProgressString = "Done ...";
            maxlevels = 99;
        }

        private bool CanExecuteWalkTreeViewItem ( object arg )
        {
            return true;
        }

        private void Window_Loaded ( object sender , RoutedEventArgs e )
        {
            string output = "";
            this . SetValue ( FontsizeProperty , InfoList . FontSize );
            canvas . Visibility = Visibility . Visible;
            CreateStaticData ( );
            LoadDrives ( TestTree );
            //            LoadDrives ( treeView4 );
            Flowdoc . ExecuteFlowDocMaxmizeMethod += new EventHandler ( MaximizeFlowDoc );
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

            // Set Horizontal Splitter FULLY DOWN at startup
            TopGrid . RowDefinitions [ 2 ] . Height = new GridLength ( 0 , GridUnitType . Pixel );
            //Grid1 . RowDefinitions [ 4 ] . Height = new GridLength ( 0 , GridUnitType . Pixel );
            //Grid1 . RowDefinitions [ 3 ] . Height = new GridLength ( 155 , GridUnitType . Pixel );
            // orow2 . Height = new GridLength ( 0 , GridUnitType . Pixel ); 
            LsplitterImage = new BitmapImage ( new Uri ( @"\icons\Lrg updown arrow red copy.png" , UriKind . Relative ) );
            VsplitterImage = new BitmapImage ( new Uri ( @"\icons\Lrg ltrt arrow red copy.png" , UriKind . Relative ) );
            ShowDriveInfo ( sender , e );
            loadExpandOptions ( );
            DrivesCombo . Items . Add ( "ALL" );
            foreach ( var drive in Directory . GetLogicalDrives ( ) )
                DrivesCombo . Items . Add ( drive . ToString ( ) );
            DrivesCombo . Items . Add ( "ALL" );
            DrivesCombo . SelectedIndex = 0;
            sw . Tick += Sw_Tick;
            sw . Interval = TimeSpan . FromMilliseconds ( 1 );
            sw . Start ( );
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

        #region other treeviews
        private void treeView3_MouseDoubleClick ( object sender , MouseButtonEventArgs e )
        {

        }
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
        private void tv3Item_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
        {
            WrapPanel name = sender as WrapPanel;
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
            CreateTreeViewData ( $"{selecteddrive}" , dirs , files );
        }

        #endregion other treeviews

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

            tv . Items . Clear ( );
            listBox . Items . Clear ( );
            listBox . UpdateLayout ( );
            foreach ( var drive in Directory . GetLogicalDrives ( ) )
            {
                var item = new TreeViewItem ( );
                item . Header = drive;
                item . Tag = drive;
                // Add Dummy entry so we get an "Can be Opened" triangle icon
                item . Items . Add ( "Loading" );

                // Add Drive to Treeview
                GetDirectories ( drive , out List<string> directories );
                if ( directories . Count > 0 )
                {   // avoid empty CD drive etc
                    tv . Items . Add ( item );
                    // Add ot listbox so we can check what has ben added (Debug)
                    listBox . Items . Add ( item . Tag . ToString ( ) );
                }
                LoadValidFiles ( );
            }
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
        public void ReloadListBox ( )
        {
            //listBox . ItemsSource = null;
            //listBox . Items . Clear ( );
            //listBox . Refresh ( );
            //foreach ( var items in LbStrings )
            //{
            //	listBox . Items . Add ( items );
            //}
            //listBox . UpdateLayout ( );
            //listBox . Refresh ( );
        }
        private void ShowallFiles_Click ( object sender , RoutedEventArgs e )
        {
            CheckBox cb = sender as CheckBox;
            if ( cb . IsChecked == true )
                ShowAllfiles = true;
            else
                ShowAllfiles = false;
        }
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

        #region utilities
        private void Refresh_Btn ( object sender , RoutedEventArgs e )
        {
            ActiveTree . Items . Clear ( );
            //            LoadDrives ( treeView4 );
        }
        private static TreeViewItem FindParentTreeViewItem ( object child )
        {
            try
            {
                var parent = VisualTreeHelper . GetParent ( child as DependencyObject );
                while ( ( parent as TreeViewItem ) == null )
                {
                    parent = VisualTreeHelper . GetParent ( parent );
                }
                return parent as TreeViewItem;
            }
            catch ( Exception e )
            {
                //could not find a parent of type TreeViewItem
                Console . WriteLine ( e . Message );
                return null;
            }
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
        //    DependencyProperty . Register ( "ExpandDuration" , typeof ( string ) , typeof ( TreeViews ) , new PropertyMetadata ( "0:00" ) );

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
            DependencyProperty . Register ( "tv1SelectedItem" , typeof ( bool ) , typeof ( TreeViews ) , new PropertyMetadata ( false ) );
        public bool tv2SelectedItem
        {
            get { return ( bool ) GetValue ( tv2SelectedItemProperty ); }
            set { SetValue ( tv2SelectedItemProperty , value ); }
        }
        public static readonly DependencyProperty tv2SelectedItemProperty =
            DependencyProperty . Register ( "tv2SelectedItem" , typeof ( bool ) , typeof ( TreeViews ) , new PropertyMetadata ( false ) );
        public bool tv3SelectedItem
        {
            get { return ( bool ) GetValue ( tv3SelectedItemProperty ); }
            set { SetValue ( tv3SelectedItemProperty , value ); }
        }
        public static readonly DependencyProperty tv3SelectedItemProperty =
            DependencyProperty . Register ( "tv3SelectedItem" , typeof ( bool ) , typeof ( TreeViews ) , new PropertyMetadata ( false ) );
        public TreeViewItem tv4SelectedItem
        {
            get { return ( TreeViewItem ) GetValue ( tv4SelectedItemProperty ); }
            set { SetValue ( tv4SelectedItemProperty , value ); }
        }
        public static readonly DependencyProperty tv4SelectedItemProperty =
            DependencyProperty . Register ( "tv4SelectedItem" , typeof ( TreeViewItem ) , typeof ( TreeViews ) , new PropertyMetadata ( ( TreeViewItem ) null ) );
        public TreeViewItem SelectedItem
        {
            get { return ( TreeViewItem ) GetValue ( SelectedItemProperty ); }
            set { SetValue ( SelectedItemProperty , value ); }
        }
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty . Register ( "SelectedItem" , typeof ( TreeViewItem ) , typeof ( TreeViews ) , new PropertyMetadata ( ( TreeViewItem ) null ) );
        public double Fontsize
        {
            get { return ( double ) GetValue ( FontsizeProperty ); }
            set { SetValue ( FontsizeProperty , value ); }
        }
        public static readonly DependencyProperty FontsizeProperty =
            DependencyProperty . Register ( "Fontsize" , typeof ( double ) , typeof ( TreeViews ) , new PropertyMetadata ( ( double ) 12 ) );
        public BitmapImage LsplitterImage
        {
            get
            { return ( BitmapImage ) GetValue ( LsplitterImageProperty ); }
            set { SetValue ( LsplitterImageProperty , value ); }
        }
        public static readonly DependencyProperty LsplitterImageProperty =
            DependencyProperty . Register ( "LsplitterImage" , typeof ( BitmapImage ) , typeof ( TreeViews ) , new PropertyMetadata ( ( BitmapImage ) null ) );
        public BitmapImage VsplitterImage
        {
            get
            { return ( BitmapImage ) GetValue ( VsplitterImageProperty ); }
            set { SetValue ( VsplitterImageProperty , value ); }
        }
        public static readonly DependencyProperty VsplitterImageProperty =
            DependencyProperty . Register ( "VsplitterImage" , typeof ( BitmapImage ) , typeof ( TreeViews ) , new PropertyMetadata ( ( BitmapImage ) null ) );
        public string LeftSplitterText
        {
            get { return ( string ) GetValue ( LeftSplitterTextProperty ); }
            set { SetValue ( LeftSplitterTextProperty , value ); }
        }
        // Using a DependencyProperty as the backing store for LeftSplitterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftSplitterTextProperty =
            DependencyProperty . Register ( "LeftSplitterText" , typeof ( string ) , typeof ( TreeViews ) , new PropertyMetadata ( ( string ) "Drag Up or Down" ) );
        public string RightSplitterText
        {
            get { return ( string ) GetValue ( RightSplitterTextProperty ); }
            set { SetValue ( RightSplitterTextProperty , value ); }
        }
        // Using a DependencyProperty as the backing store for LeftSplitterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightSplitterTextProperty =
            DependencyProperty . Register ( "RightSplitterText" , typeof ( string ) , typeof ( TreeViews ) , new PropertyMetadata ( ( string ) "to View Directory Tree / Drive Technical Information." ) );


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
            DependencyProperty . RegisterAttached ( "ExpandDuration" , typeof ( string ) , typeof ( TreeViews ) , new PropertyMetadata ( "0:00" ) );


        public static bool Gettvselection ( DependencyObject obj )
        {
            return ( bool ) obj . GetValue ( tvselectionProperty );
        }
        public static void Settvselection ( DependencyObject obj , bool value )
        {
            obj . SetValue ( tvselectionProperty , value );
        }
        public static readonly DependencyProperty tvselectionProperty =
            DependencyProperty . RegisterAttached ( "tvselection" , typeof ( bool ) , typeof ( TreeViews ) , new PropertyMetadata ( ( bool ) false ) );

        #endregion Attached Properties

        public bool TvSelectedItem { get; set; }

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


        #endregion Flowdoc support via library

        private void TREEViews_Closing ( object sender , CancelEventArgs e )
        {
            Flowdoc . ExecuteFlowDocMaxmizeMethod -= new EventHandler ( MaximizeFlowDoc );
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
        private void GetItemCounts ( string path , out int Dircount , out int Filecount )
        {
            int dirs = 0;
            int files = 0;
            GetDirectories ( path , out List<string> results );
            Dircount = results . Count;
            GetFiles ( path , out List<string> fileresults );

            Filecount = fileresults . Count;

            if ( Dircount > 0 )
                Selection . Text = $"Current Item : {path} -  {Dircount } SubDirectory(s)";
            else
            {
                if ( ShowAllfiles )
                    Selection . Text = $"Current Item : {path} -  No SubDirectories ";
                else
                    Selection . Text = $"Current Item : {path} -  No valid SubDirectories ";
            }
            if ( Filecount > 0 )
                Selection . Text += $", {Filecount} Files";
            else
                Selection . Text += $",  No Files";
            listBox . Items . Clear ( );
            foreach ( var item in results )
                listBox . Items . Add ( $"Directory : {item}" );
            foreach ( var item in fileresults )
                listBox . Items . Add ( $"File : {item}" );

        }
        private static bool CheckIsVisible ( string entry , bool showall )
        {
            entry = entry . ToUpper ( );
            if ( showall == false )
            {
                foreach ( var item in ValidFiles )
                {
                    if ( entry.Contains( item . ToUpper ( ) ) )
                        return false;
                }
                return true;
            }
            return true;
        }
        public int AddDirectoriesToTreeview ( List<string> directories , TreeViewItem item , ListBox lBox = null , bool UseExpand = true )
        {
            int added = 0;
            directories . ForEach ( directoryPath =>
            {
                var subitem = new TreeViewItem ( );
                subitem . Header = GetFileFolderName ( directoryPath );
                subitem . Tag = subitem . Header;
                //                subitem . Tag = directoryPath;
                if ( CheckIsVisible ( directoryPath . ToUpper ( ) , ShowAllfiles ) == true )
                {     // add the dummy entry to each of the subdirectories we are adding to the tree so we get the Expand icons
                    List<string> allfiles = new List<string> ( );
                    if ( GetFiles ( directoryPath , out allfiles ) > 0 )
                        subitem . Items . Add ( "Loading" );
                    // force it  to iterate  recursively
                    TreeViews tvs = new TreeViews ( );
                    item . Items . Add ( subitem );
                    // Add item to parent
                    if ( UseExpand )
                        subitem . Expanded += tvs . TreeViewItem4_Expanded;
                    added++;
                }
            } );
            return added;
        }
        public int AddDirectoriesToTestTreeview ( List<string> directories , TreeViewItem item , ListBox lBox = null , bool UseExpand = true )
        {
            int added = 0;
            foreach ( var dir in directories )
            {
                var subitem = new TreeViewItem ( );
                try
                {
                    subitem . Header = GetFileFolderName ( dir );
                    subitem . Tag = dir;
                    item . Items . Add ( subitem );
                    ShowProgress ( );
                    if ( CheckIsVisible ( dir . ToUpper ( ) , ShowAllfiles ) == true )
                    {
                        // add the dummy entry to each of the subdirectories we are adding to the tree so we get the Expand icons
                        // need to protect against invalid folder access from crashing us!!!!
                        string [ ] dirs = Directory . GetDirectories ( dir );
                        if ( dirs . Length > 0 )
                        {
                            subitem . Items . Add ( "Loading" );
                        }
                        else
                        {
                            if ( GetFilesCount ( dir ) > 0 )
                                subitem . Items . Add ( "Loading" );
                        }
                        ShowProgress ( );
                        added++;
                    }
                    else
                         ShowProgress ( );
                }
                catch ( Exception ex )
                {
                    Console . WriteLine ( $"Invalid  directory accessed {ex . Message}" );
                    ShowProgress ( );
                }
            }
            return added;
        }
        public int AddFilesToTreeview ( List<string> Allfiles , TreeViewItem item )
        {
            int count = 0;
            foreach ( var itm in Allfiles )
            {
                ShowProgress ( );
                var subitem = new TreeViewItem ( )
                {
                    Header = GetFileFolderName ( itm ) ,
                    Tag = itm
                };
                if ( CheckIsVisible ( itm . ToUpper ( ) , ShowAllfiles ) == true )
                {
                    item . Items . Add ( subitem );
                    count++;
                }
                ShowProgress ( );
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
                        // working correctly
                        UpdateListBox ( item);
                        ActiveTree . Refresh ( );
                        count++;
                        ShowProgress ( );
                    }
                }
            }
            catch { }
            dirs = directories;
            return count;
        }
        public int GetFiles ( string path , out List<string> allfiles )
        {
            int count = 0;
            var files = new List<string> ( );
            // Get a list of all items in the current folder
            ActiveTree . Refresh ( );
            try
            {
                var file = Directory . EnumerateFiles ( path , "*.*" );
                //var file = Directory . GetFiles ( path , "*.*");
                if ( file . Count ( ) > 0 )
                {
                    foreach ( var item in file )
                    {
                        ShowProgress ( );
                        if ( CheckIsVisible ( item . ToUpper ( ) , ShowAllfiles ) == true )
                        {
                            files . Add ( item );
                            count++;
                            // working correctly
                            UpdateListBox ( item );
                        }
                        ShowProgress ( );
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
        }
        private void Hsplitter_DragOver ( object sender , DragEventArgs e )
        {
            //double totalheight = ( row1 . ActualHeight + orow2 . ActualHeight ) - 20;
            //double maxheight = Grid1 . ActualHeight - btngrid . ActualHeight;
            //double topheight = row1 . ActualHeight - 10;
            //double lowerheight = row2 . ActualHeight - 10;
            ////innergrid . Height = row1 . ActualHeight;
            //InfoList . UpdateLayout ( );
        }
        private void hsplitter_DragDelta ( object sender , System . Windows . Controls . Primitives . DragDeltaEventArgs e )
        {
            double totalheight = ( row1 . ActualHeight + orow2 . ActualHeight ) - 20;
            double maxheight = Grid1 . ActualHeight - btngrid . ActualHeight;
            double topheight = row1 . ActualHeight - 10;
            double lowerheight = row2 . ActualHeight - 10;
            //            innergrid . Height = row1 . ActualHeight;
            //            InfoList . UpdateLayout ( );
        }

        private void VLeftSplitter_DragStarted ( object sender , System . Windows . Controls . Primitives . DragStartedEventArgs e )
        {

        }

        private void VLeftSplitter_DragCompleted ( object sender , System . Windows . Controls . Primitives . DragCompletedEventArgs e )
        {

        }

        private void VLeftSplitter_DragOver ( object sender , DragEventArgs e )
        {

        }

        private void vLeftSplitter_DragDelta ( object sender , System . Windows . Controls . Primitives . DragDeltaEventArgs e )
        {

        }
        private void VRightSplitter_DragStarted ( object sender , System . Windows . Controls . Primitives . DragStartedEventArgs e )
        {

        }

        private void VRightSplitter_DragCompleted ( object sender , System . Windows . Controls . Primitives . DragCompletedEventArgs e )
        {

        }

        private void VRightSplitter_DragOver ( object sender , DragEventArgs e )
        {

        }

        private void vRightSplitter_DragDelta ( object sender , System . Windows . Controls . Primitives . DragDeltaEventArgs e )
        {

        }

        #endregion Splitter handlers

        private void TestTree_Expanded ( object sender , RoutedEventArgs e )
        {
            // All working when clicking on any folder !!!!
            TreeViewItem item = null;
            int itemscount = 0;
            if ( e != null )
                item = e . Source as TreeViewItem;
            else
                item = sender as TreeViewItem;
            if ( item == null )
                return;
            // This is CRITICAL to get any drive that is currently selected to open when the expand icon is clicked
            item . IsSelected = true;
            Selection . Text = $"{item . Tag . ToString ( )}";
            Console . WriteLine ( Selection . Text );

            var directories = new List<string> ( );
            var Allfiles = new List<string> ( );
            string Fullpath = item . Tag . ToString ( ) . ToUpper ( );
            //            string InfoMessage = "";
            int DirectoryCount = 0;
            //listboxtotal = 0;
            //           int FileCount = 0;
            itemscount = item . Items . Count;
            var tvi = item as TreeViewItem;
            if ( itemscount == 0 )
                return;
            var itemheader = item . Items [ 0 ] . ToString ( );
            // Get a list of all items in the current folder
            int count = GetDirectories ( Fullpath , out directories );
            if ( count > 0 )
            {
                if ( itemheader == "Loading" )
                    item . Items . Clear ( );
                DirectoryCount = count;
                ShowProgress ( );
                DirectoryCount = AddDirectoriesToTestTreeview ( directories , item , listBox );
                //// Check to see if there any file items in the current folder
                //if ( DirectoryCount > 0 )
                //    InfoMessage = $"Current Item : {Fullpath} -  {DirectoryCount} SubDirectory(s)";
            }
            else
            {
                DirectoryCount = 0;
                ShowProgress ( );
                //                InfoMessage = $"Current Item : {Fullpath} -  No valid SubDirectories ";
            }
            GetFiles ( Fullpath , out Allfiles );
            if ( Allfiles . Count > 0 )
            {
                if ( count == 0 && itemheader == "Loading" )
                    item . Items . Clear ( );
                AddFilesToTreeview ( Allfiles , item );
            }
            TestTree . UpdateLayout ( );
            Selection . Text = $"Expanding : {Fullpath} ...";
            ShowProgress ( );
            return;
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

        #region Expand // Collapse

        #region TestTree Expanding Handling methods
        int iterations = 0;
        private void ExpandGo_Click ( object sender , RoutedEventArgs e )
        {
            double temp = 0;
            ComboBox cb = DirectoryOptions;
            int selindex = DirectoryOptions . SelectedIndex;
            object [ ] Args = new object [ ] { new object ( ) , new object ( ) , new object ( ) };
            string original = "";
            iterations = 0;
            listboxtotal = 0;
            TreeViewItem root = ActiveTree . SelectedItem as TreeViewItem;
            if ( root != null )
            {
                Args [ 0 ] = root as object;
                original = root . Header . ToString ( );
            }
            InfoList . ItemsSource = null;
            listBox . Items . Clear ( );
            if ( selindex < 4 )
            {
                if ( Args [ 0 ] == null )
                {
                    Mouse . OverrideCursor = Cursors . Arrow;
                    MessageBox . Show ( "Please select a Drive in the TreeView befre using these options...." );
                    BusyLabel . Visibility = Visibility . Hidden;
                    return;
                }
                if ( selindex == 0 )
                    Args [ 1 ] = 2;
                else if ( selindex == 1 )
                    Args [ 1 ] = 3;
                else if ( selindex == 2 )
                {
                    Args [ 1 ] = 4;
                    TreeViewItem tvi = ( TreeViewItem ) Args [ 0 ];
                    string str = tvi . Tag . ToString ( );
                    if ( MessageBox . Show ( $"Expanding {str} down FOUR levels may take some time, Are you  sure you want  to continue ?" , "Expansion System Warning !" ,
                        MessageBoxButton . YesNo ) == MessageBoxResult . No )
                    {
                        Mouse . OverrideCursor = Cursors . Arrow;
                        BusyLabel . Visibility = Visibility . Hidden;
                        return;
                    }
                }
                else if ( selindex == 3 )
                    Args [ 1 ] = 90;

                //Expand current 2 levels
                listBox . Items . Add ( $"Expanding {original} {Args [ 1 ]} levels...." );
                InfoList . Items . Add ( $"Expanding {original} {Args [ 1 ]} levels...." );
                ExpandSetup ( true );

                Dispatcher . BeginInvoke ( new Action ( ( ) =>
                {
                    Mouse . OverrideCursor = Cursors . Wait;
                    ExpandCurrentAllLevels ( Args );
                } ) , DispatcherPriority . SystemIdle );
                InfoList . Items . Add ( $"{Args [ 0 ] . ToString ( )} Expanded {Args [ 1 ]} levels......." );
                Mouse . OverrideCursor = Cursors . Arrow;
                ScrollCurrentTvItemIntoView ( ( TreeViewItem ) Args [ 0 ] );
                //ExpandSetup ( false );
                return;
            }
            else if ( selindex == 4 || selindex == 5 )
            {
                // Fully Expand Currently selected drive
                Args [ 0 ] = ( object ) ActiveTree . SelectedItem as TreeViewItem;
                // Set level to match selection made by user
                Args [ 1 ] = selindex - 2;
                ExpandSetup ( true );
                InfoList . Items . Add ( $"Fully Expanding {Args [ 0 ] . ToString ( )} ALL levels...." );
                Dispatcher . BeginInvoke ( new Action ( ( ) =>
                {
                    Mouse . OverrideCursor = Cursors . Wait;
                    ExpandCurrentAllLevels ( Args );
                } ) , DispatcherPriority . ApplicationIdle );
                InfoList . Items . Add ( $"{Args [ 0 ] . ToString ( )} Fully Expanded ......." );
                Mouse . OverrideCursor = Cursors . Arrow;
                ExpandSetup ( false );
                ScrollCurrentTvItemIntoView ( ( TreeViewItem ) Args [ 0 ] );
                return;
            }
            else if ( selindex == 6 )
            {
                InfoList . Items . Add ( "Calling EXPANDALLRECURSIVE" );
                Args [ 0 ] = ( object ) ActiveTree . SelectedItem as TreeViewItem;
                Args [ 1 ] = ( object ) 90;
                ExpandSetup ( true );
                Dispatcher . BeginInvoke ( new Action ( ( ) =>
                {
                    Mouse . OverrideCursor = Cursors . Wait;
                    ExpandCurrentAllLevels ( Args );
                } ) , DispatcherPriority . ApplicationIdle );
                ExpandSetup ( false );
                ScrollCurrentTvItemIntoView ( ( TreeViewItem ) Args [ 0 ] );
                return;
            }
            else if ( selindex == 7 )
            {
                Args [ 0 ] = ActiveTree as object;
                ExpandSetup ( true );
                Dispatcher . BeginInvoke ( new Action ( ( ) =>
                {
                    CollapseAllDrives ( Args );
                } ) , DispatcherPriority . ApplicationIdle );
                Mouse . OverrideCursor = Cursors . Arrow;
                ExpandSetup ( false );
                ScrollCurrentTvItemIntoView ( ( TreeViewItem ) Args [ 0 ] );
                return;
            }
        }
        private void ExpandAll3 ( TreeViewItem items , bool expand , int levels )
        {
            if ( items == null )
                return;
            foreach ( object obj in items . Items )
            {
                iterations++;
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
                    catch ( Exception ex ) { }
                    ShowProgress ( );
                    if ( CalculateLevel ( childControl . Tag . ToString ( ) ) > levels )
                        break;
                    UpdateListBox ( childControl . Tag . ToString ( ) );

                    ActiveTree . Refresh ( );
                    if ( childControl . Items . Count > 1 )
                    {
                        if ( childControl . Items [ 0 ] . ToString ( ) != "Loading" )
                        {
                            if ( levels == 1 )
                            {
                                ShowProgress ( );
                                Selection . Text = $"Calling ExpandFolder for {childControl . Tag . ToString ( )}";
                                Console . WriteLine ( Selection . Text );
                                ExpandFolder ( childControl , true ); // Expand ALL Contents (true)
                                ShowProgress ( );
                                ActiveTree . Refresh ( );
                            }
                            else
                            {
                                ShowProgress ( );
                                Selection . Text = $"Calling ExpandAll3 for {childControl . Tag . ToString ( )}";
                                Console . WriteLine ( Selection . Text );
                                ExpandAll3 ( childControl as TreeViewItem , expand , levels );
                                ShowProgress ( );
                                ActiveTree . Refresh ( );
                            }
                        }
                        else
                        {
                            ShowProgress ( );
                            try
                            {
                                childControl . IsExpanded = true;
                            }
                            catch ( Exception ex ) { }
                            ShowProgress ( );
                            ActiveTree . Refresh ( );
                        }
                    }
                    else
                    {
                        ShowProgress ( );
                        try
                        {
                            childControl . IsExpanded = true;
                        }
                        catch ( Exception ex ) { }
                    }
                    ActiveTree . Refresh ( );
                    ShowExpandTime ( );
                    ShowProgress ( );
                }
                ActiveTree . Refresh ( );
            }
            ShowProgress ( );
            ActiveTree . Refresh ( );
        }

        private void CollapseAllDrives ( object [ ] Args )
        {
            Mouse . OverrideCursor = Cursors . Wait;
            TreeView tv = Args [ 0 ] as TreeView;
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
                    if ( nextdrive . Tag . ToString ( ) == validdrive )
                    {
                        nextdrive . IsExpanded = true;
                        ShowProgress ( );
                        foreach ( TreeViewItem item2 in nextdrive . Items )
                        {
                            item2 . IsExpanded = true;
                            ShowProgress ( );
                            item2 . UpdateLayout ( );
                            ActiveTree . Refresh ( );
                            if ( item2 . Items . Count > 0 )
                            {
                                foreach ( TreeViewItem item3 in item2 . Items )
                                {
                                    item3 . IsExpanded = true;
                                    item3 . UpdateLayout ( );
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

        public void TriggerExpand0 ( object sender , RoutedEventArgs e )
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
            Dispatcher . BeginInvoke ( new Action ( ( ) =>
            {
                Mouse . OverrideCursor = Cursors . Wait;
                ExpandCurrentAllLevels ( Args );
                Mouse . OverrideCursor = Cursors . Arrow;
            } ) , DispatcherPriority . ApplicationIdle );


            //            ExpandCurrentAllLevels ( Args );
            //            ExpandSetup ( false );
            int indx = TestTree . Items . IndexOf ( ( object ) TestTree . SelectedItem );
            ScrollCurrentTvItemIntoView ( startitem );
        }
         public void TriggerExpand1 ( object sender , RoutedEventArgs e )
        {
            if ( TestTree . SelectedItem == null )
            {
                MessageBox . Show ( $"Please select a drive or subfolder before using  these options...." , "No Drive Selected" );
                return;
            }
            object [ ] Args = { TestTree . SelectedItem as TreeViewItem , ( object ) 2 , null };
            startitem = TestTree . SelectedItem as TreeViewItem;
            ExpandSetup ( true );
            DirectoryOptions . Focus ( );
            Dispatcher . BeginInvoke ( new Action ( ( ) =>
            {
                Mouse . OverrideCursor = Cursors . Wait;
                ExpandCurrentAllLevels ( Args );
                Mouse . OverrideCursor = Cursors . Arrow;
            } ) , DispatcherPriority . ApplicationIdle );
//            ExpandSetup ( false );
            ScrollCurrentTvItemIntoView ( startitem );
        }
        public void TriggerExpand2 ( object sender , RoutedEventArgs e )
        {
            if ( TestTree . SelectedItem == null )
            {
                MessageBox . Show ( $"Please select a drive or subfolder before using  these options...." , "No Drive Selected" );
                return;
            }
            object [ ] Args = { TestTree . SelectedItem as TreeViewItem , ( object ) 3 , null };
            startitem = TestTree . SelectedItem as TreeViewItem;
            ExpandSetup ( true );
            DirectoryOptions . Focus ( );
            Dispatcher . BeginInvoke ( new Action ( ( ) =>
            {
                Mouse . OverrideCursor = Cursors . Wait;
                ExpandCurrentAllLevels ( Args );
                Mouse . OverrideCursor = Cursors . Arrow;
            } ) , DispatcherPriority . ApplicationIdle );
//           ExpandSetup ( false );
            ScrollCurrentTvItemIntoView ( startitem );
        }
        public void TriggerExpand3 ( object sender , RoutedEventArgs e )
        {
            if ( TestTree . SelectedItem == null )
            {
                MessageBox . Show ( $"Please select a drive or subfolder before using  these options...." , "No Drive Selected" );
                return;
            }
            object [ ] Args = { TestTree . SelectedItem as TreeViewItem , ( object ) 4 , null };
            startitem = TestTree . SelectedItem as TreeViewItem;
            ExpandSetup ( true );
            DirectoryOptions . Focus ( );
            Dispatcher . BeginInvoke ( new Action ( ( ) =>
            {
                Mouse . OverrideCursor = Cursors . Wait;
                ExpandCurrentAllLevels ( Args );
                Mouse . OverrideCursor = Cursors . Arrow;
            } ) , DispatcherPriority . ApplicationIdle );
 //           ExpandSetup ( false );
            ScrollCurrentTvItemIntoView ( startitem );
        }
        public void TriggerExpand4 ( object sender , RoutedEventArgs e )
        {
            if ( TestTree . SelectedItem == null )
            {
                MessageBox . Show ( $"Please select a drive or subfolder before using  these options...." , "No Drive Selected" );
                return;
            }
            object [ ] Args = { TestTree . SelectedItem as TreeViewItem , ( object ) 90 , null };
            startitem = TestTree . SelectedItem as TreeViewItem;
            ExpandSetup ( true );
            DirectoryOptions . Focus ( );
            Dispatcher . BeginInvoke ( new Action ( ( ) =>
            {
                Mouse . OverrideCursor = Cursors . Wait;
                ExpandCurrentAllLevels ( Args );
                Mouse . OverrideCursor = Cursors . Arrow;
            } ) , DispatcherPriority . ApplicationIdle );
//            ExpandSetup ( false );
            ScrollCurrentTvItemIntoView ( startitem );
        }
        public void ExpandCurrentAllLevels ( object [ ] Args )
        {
            int iterations = 0;
            int itemcount = 0;
            int levelscount = 0;
            int levels = ( int ) Args [ 1 ];
            TreeViewItem items = Args [ 0 ] as TreeViewItem;
            startitem = items;
            if ( items == null )
                return;
            ProgressCount = 0;
            // Essential to force root to be expanded, else nothing happens
            try
            {
                items . IsExpanded = true;
            }
            catch ( Exception ex ) { }
            ActiveTree . Refresh ( );
            if ( ( int ) CalculateLevel ( items . Tag . ToString ( ) ) >= levels )
                return;
            UpdateListBox ( items . Tag . ToString ( ) );
            ShowProgress ( );
            foreach ( TreeViewItem obj in items . Items )
            {
                ShowProgress ( );
                levelscount = CalculateLevel ( obj . Tag . ToString ( ) );
                if ( levelscount >= levels )
                    break;
                //                Selection . Text = $"Expanding {obj . Tag . ToString ( )}";
                TreeViewItem childControl = obj as TreeViewItem;
                // working correctly
                UpdateListBox ( childControl . Tag . ToString ( ) );
                ShowProgress ( );

                try
                {
                    childControl . IsExpanded = true;
                }
                catch ( Exception ex ) { }
                //                Console . WriteLine ( $"2 Level={levelscount}, Outer loop {childControl . Tag . ToString ( )}" );
                ShowProgress ( );
                if ( childControl != null && levels > 2 )
                {
                    string entry = childControl . Header . ToString ( ) . ToString ( ) . ToUpper ( );
                    itemcount = childControl . Items . Count;

                    levelscount = CalculateLevel ( childControl . Tag . ToString ( ) );
                    if ( levelscount >= levels )
                        break;
                    //                    Console . WriteLine ( $"3 Level={levelscount}, About to Loop thru: {childControl . Tag . ToString ( )}" );

                    ShowProgress ( );
 
                    //                   UpdateListBox ( childControl. Tag . ToString ( ) . ToUpper ( ) );
                    
                    ActiveTree . Refresh ( );
                    iterations++;
                    if ( levels >= 3 )
                    {
                        foreach ( TreeViewItem nextitem in childControl . Items )
                        {
                            ShowProgress ( );
                            if ( CheckIsVisible ( nextitem . Tag . ToString ( ) . ToUpper ( ) , ShowAllfiles ) == false )
                            {
                                Console . WriteLine ($"System file : {nextitem . Tag . ToString ( ) . ToUpper ( )}");
                                continue;
                            }
 
                            try
                            {
                                nextitem . IsExpanded = true;
                            }
                            catch ( Exception ex ) { }
                            ShowProgress ( );
                            // working correctly
                            UpdateListBox ( nextitem . Tag . ToString ( ) );
                            ActiveTree . Refresh ( );

                            //                            Selection . Text = $"Expanding {nextitem . Tag . ToString ( )}";
                            Console . WriteLine ( Selection . Text );
                            levelscount = CalculateLevel ( nextitem . Tag . ToString ( ) );
                            if ( levelscount >= levels )
                                break;
                            //                          Console . WriteLine ( $"3 Level={levelscount}, Inner loop EXPANDING : {nextitem . Tag . ToString ( )}" );
                            UpdateListBox ( nextitem . Tag . ToString ( ) );
                            if ( levels >= 4 )
                            {
                                if ( CheckIsVisible ( nextitem . Tag . ToString ( ) . ToUpper ( ) , ShowAllfiles ) == false )
                                {
                                    Console . WriteLine ( $"System file : {nextitem . Tag . ToString ( ) . ToUpper ( )}" );
                                    continue;
                                }
                                ExpandAll3 ( nextitem , true , levels );
                            }
                            ShowExpandTime ( );
                            ShowProgress ( );
                            ActiveTree . Refresh ( );
                        }
                    }
                    ShowExpandTime ( );
                }
                ActiveTree . Refresh ( );
                ShowProgress ( );
            }
            ShowExpandTime ( );
            ExpandSetup ( false );
            Expandprogress . Refresh ( );
            ScrollCurrentTvItemIntoView ( Args [ 0 ] as TreeViewItem );
        }
        //******************************************************************************************************//
        // Utility method called recursively to open folders by  other expannd methods
        //******************************************************************************************************//
        private void ExpandFolder ( TreeViewItem item , bool ExpandContent = false )
        {
            string fullpath = "";
            if ( item . Items . Count > 0 )
            {
                if ( item . Items [ 0 ] != "Loading" )
                {
                    foreach ( TreeViewItem item2 in item . Items )
                    {
                        ShowProgress ( );
                        fullpath = item2 . Tag . ToString ( ) . ToUpper ( );
                        try
                        {
                            item2 . IsExpanded = true;
                        }
                        catch ( Exception ex ) { }
                        UpdateListBox ( item2 . Tag . ToString ( ) );
                        ShowProgress ( );
                        ShowExpandTime ( );
                        ActiveTree . Refresh ( );
                    }
                    UpdateExpandprogress ( );
                }
            }
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
            {//List<ComboBoxItem> DirOptions2 = new List<ComboBoxItem> ( );
             //ComboBoxItem cbitem = new ComboBoxItem ( );
             //ComboBoxItem cbitem1 = new ComboBoxItem ( );
             //ComboBoxItem cbitem2 = new ComboBoxItem ( );
             //ComboBoxItem cbitem3 = new ComboBoxItem ( );
             //ComboBoxItem cbitem4 = new ComboBoxItem ( );
             //cbitem . Content = "Expand current Item down 2 levels \n-> Root \n->    SubFolders\n->       Subfolders\n            Files\n->.Files";
             //cbitem . Background = FindResource ( "Red0" ) as SolidColorBrush;
             //cbitem . Foreground = FindResource ( "White0" ) as SolidColorBrush;//
             //DirOptions2 . Add ( cbitem);
             //cbitem1 . Content = "Expand current Item down 3 levels\n-> Root \n->    SubFolders\n->       Subfolders\n                 Subfolders\n                      Files\n->.Files";
             //cbitem1 . Background = FindResource ( "Red2" ) as SolidColorBrush;//
             //cbitem1 . Foreground = FindResource ( "White0" ) as SolidColorBrush;//
             //DirOptions2 . Add ( cbitem1 );
             //cbitem2 . Content = "Expand ALL Drives one level\n -> Root Drive\n         Folders\n       Drive\n         Folders\n        .....\n    Files";
             //cbitem2 . Background = FindResource ( "Green4" ) as SolidColorBrush;
             //cbitem2 . Foreground = FindResource ( "White0" ) as SolidColorBrush;//
             //DirOptions2 . Add ( cbitem2 );
             //cbitem3 . Content = "Fully Expand ALL levels of current Item \n-> ALL contents\nMay take a while !...";
             //cbitem3 . Background = FindResource ( "Blue3" ) as SolidColorBrush;
             //cbitem3 . Foreground = FindResource ( "White0" ) as SolidColorBrush;//
             //DirOptions2 . Add ( cbitem3 );
             //cbitem4 . Content = "Collapse All Drives";
             //cbitem4 . Background = FindResource ( "Orange3" ) as SolidColorBrush;
             //cbitem4 . Foreground = FindResource ( "Black0" ) as SolidColorBrush;//
             //DirOptions2 . Add ( cbitem4 );
             //DirectoryOptions . Items . Clear ( );
             //DirectoryOptions . ItemsSource = DirOptions2;
             //DirectoryOptions.  SelectedIndex = 0;
             //DirectoryOptions . SelectedItem = 0;
            }
        }

        #endregion Expanding support mmethods


        #region BackgroundWorker
        private void worker_DoWork ( object sender , DoWorkEventArgs e )
        {
            int result = 0;
            BackgroundWorker worker = sender as BackgroundWorker;
            //ExpandCurrentTwoLevels ( treeView4 . SelectedItem as TreeViewItem );
            //result =
            //int max = ( int ) e . Argument;
            //for ( int i = 0 ; i < max ; i++ )
            //{
            //    int progressPercentage = Convert . ToInt32 ( ( ( double ) i / max ) * 100 );
            //    if ( i % 42 == 0 )
            //    {
            //        result++;
            //        ( sender as BackgroundWorker ) . ReportProgress ( progressPercentage , i );
            //    }
            //    else
            //        ( sender as BackgroundWorker ) . ReportProgress ( progressPercentage );
            //    System . Threading . Thread . Sleep ( 1 );

            //}
            e . Result = result;
        }
        private void worker_RunWorkerCompleted ( object sender , RunWorkerCompletedEventArgs e )
        {
            //pbCalculationProgress . Value = e . ProgressPercentage;
            //if ( e . UserState != null )
            //    lbResults . Items . Add ( e . UserState );
        }
        private void worker_ProgressChanged ( object sender , ProgressChangedEventArgs e )
        {
            //pbCalculationProgress . Value = e . ProgressPercentage;
            //if ( e . UserState != null )
            //    lbResults . Items . Add ( e . UserState );
        }
        #endregion BackgroundWorker

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

        private void ReloadTreeView ( string drive )
        {
            List<string> directories = new List<string> ( );
            List<string> files = new List<string> ( );
            int count = GetDirectories ( drive , out directories );
            CreateTreeViewData ( @"M:\\" , directories , files );
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
                // Add Dummy entry so we get an "Can be Opened" triangle icon
                int dircount = GetDirectories ( drive , out List<string> directories );
                if ( dircount > 0 )
                    item . Items . Add ( "Loading" );
                // Add Drive to Treeview with dummy "Loading" item
                TestTree . Items . Add ( item );

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
                            if ( CheckIsVisible ( file . ToUpper ( ) , ShowAllfiles ) == true )
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
        public int AddDirectoriesToTestTree ( List<string> directories , TreeViewItem item , ListBox lBox = null , bool UseExpand = true )
        {
            int added = 0;
            directories . ForEach ( directoryPath =>
            {
                var subitem = new TreeViewItem ( );
                subitem . Header = GetFileFolderName ( directoryPath );
                subitem . Tag = directoryPath;
               UpdateListBox ( directoryPath . ToUpper ( ) );
                if ( CheckIsVisible ( directoryPath . ToUpper ( ) , ShowAllfiles ) == true )
                {     // add the dummy entry to each of the subdirectories we are adding to the tree so we get the Expand icons
                    subitem . Items . Add ( "Loading" );
                    added++;
                }
                ActiveTree . Refresh ( );
            } );
            return added;
        }
        private void SearchTree ( )
        {
            StringBuilder TreeNodes = new StringBuilder ( );

            foreach ( TreeViewItem l_item in TestTree . Items )
            {
                ProcessNodes ( l_item , TreeNodes , 0 );
            }
            MessageBox . Show ( TreeNodes . ToString ( ) ); ;
        }
        private void ProcessNodes ( TreeViewItem node , StringBuilder builder , int level )
        {

            builder . Append ( new string ( '\t' , level ) + node . Header . ToString ( ) + Environment . NewLine );
            for ( int x = 0 ; x < node . Items . Count ; x++ )
            {
                TreeViewItem tvi = new TreeViewItem ( );
                var vvvvv = node . Items [ x ] . ToString ( );
                if ( vvvvv == "Loading" )
                    continue;
                string [ ] str1 = vvvvv . Split ( ':' );
                string [ ] str2 = str1 [ 1 ] . Split ( ' ' );
                Console . WriteLine ( $"{str2 [ 0 ]}" );
                if ( str2 [ 0 ] . Contains ( " " ) )
                {
                    tvi . Header = str2 [ 0 ]; ;
                }
                else
                    tvi . Header = str2 [ 0 ];
                if ( tvi . IsExpanded )
                {
                    ProcessNodes ( tvi , builder , level + 1 );
                }
                else
                    ProcessNodes ( tvi , builder , level + 1 );
            }
        }
        private void iterateitems ( TreeViewItem item )
        {
            item . IsExpanded = true;
            item . UpdateLayout ( );
        }
        // All working just fine - expands  any  folder FULLY
        private void WalkTestTree ( object [ ] Args )
        {
            string current = "";
            bool maxexceeded = false;
            //            int currentitemlevel = 0;
            if ( BreakExpand )// || maxlevels >= 3)
                return;
            ShowProgress ( );
            TreeViewItem tvitem = TestTree . SelectedItem as TreeViewItem;
            if ( maxlevels == 99 )
            {
                current = tvitem . Tag . ToString ( );
                string [ ] levels = current . Split ( '\\' );
                maxlevels += levels . Length - 1;
            }
            if ( startitem == null && tvitem == null )
            {
                MessageBox . Show ( $"Please select an item in the Tree Viewer to use this option" );
                return;
            }
            else if ( startitem == null )
            {
                //                Console . WriteLine ( $"0: {tvitem . Tag . ToString ( )}" );
                startitem = tvitem;
            }
            try
            {
                if ( tvitem . IsExpanded == false )
                    tvitem . IsExpanded = true;
                //          Console . WriteLine ( $"1: tvitem = {tvitem . Tag . ToString ( )}" );
                //tvitem . UpdateLayout ( );
                //                ActiveTree . Refresh ( );
                ShowProgress ( );
                if ( maxlevels > 1 )
                {
                    foreach ( var item in tvitem . Items )
                    {
                        // This loop handles opening of TWO levels down
                        TreeViewItem tvit = item as TreeViewItem;
                        if ( maxlevels != 99 )
                        {
                            current = tvit . Tag . ToString ( );
                            string [ ] levels = current . Split ( '\\' );
                            if ( levels . Length > maxlevels )
                            {
                                maxexceeded = true;
                                ShowProgress ( );
                                //                              ActiveTree . Refresh ( );
                                continue;
                            }
                        }
                        if ( tvit . IsExpanded == false )
                        {
                            try
                            {
                                tvit . IsExpanded = true;
                            }
                            catch ( Exception ex ) { }
                            ProgressCount++;
                            ShowProgress ( );
                        }
                    }
                    object [ ] args = { ActiveTree . SelectedItem };
                    WalkTestTree ( ( object [ ] ) args );
                }
            }
            catch ( Exception ex )
            {
                ShowProgress ( );
                object [ ] args = { ActiveTree . SelectedItem };
                WalkTestTree ( ( object [ ] ) args );
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
        private void DrivesCombo_SelectionChanged ( object sender , SelectionChangedEventArgs e )
        {
            if ( Loading )
                return;
            ComboBox cb = sender as ComboBox;
            string seldrive = cb . SelectedItem . ToString ( );
            if ( seldrive . ToString ( ) == "ALL" )
            {
                //cb . Items . Clear ( );
                LoadDrives ( TestTree );
            }
            else
            {
                List<string> dirs = new List<string> ( );
                List<string> files = new List<string> ( );
                CreateTreeViewData ( $@"{seldrive}" , dirs , files );
            }
        }
        private void TestTree_PreviewMouseRightButtonDown ( object sender , MouseButtonEventArgs e )
        {

            return;

            string output = "";
            TreeViewItem tvi = TestTree . SelectedItem as TreeViewItem;
            string selection = tvi . Tag . ToString ( );
            var fi = new FileInfo ( selection );
            if ( fi . Attributes . ToString ( ) . Contains ( "Directory" ) )
            {
                output += $"Sub Directory : {selection . ToUpper ( )}\n";
                try
                {
                    string [ ] allcontent = Directory . GetFileSystemEntries ( selection );
                    output += $"Contents : \n";
                    foreach ( var item in allcontent )
                    {
                        fi = new FileInfo ( item );
                        if ( fi . Attributes . ToString ( ) . Contains ( "Directory" ) == false )
                        {
                            string fmt = String . Format ( "{0:#,###,###,0}" , fi . Length );
                            output += $"{item}, {fmt} bytes\n";
                        }
                        //else
                        //    output += $"{item}, {fmt} bytes\n";
                    }
                }
                catch ( Exception ex ) { }
            }
            else
            {
                FileAttributes fa = File . GetAttributes ( selection );
                output += $"Length={ fi . Length}";
            }
            listBox . Items . Clear ( );
            listBox . Items . Add ( output );
            //MessageBox . Show (output );
        }
        private void DirectoryOptions_Selected ( object sender , SelectionChangedEventArgs e )
        {
            ExpanderMenuOption . Text = $"{DirectoryOptions . SelectedItem . ToString ( )}";

        }

        private void WalkTestTree ( object sender , RoutedEventArgs e )
        {
            //object [ ] Args = { ActiveTree . SelectedItem };
            TriggerExpand4 ( sender , e );
            //WalkTestTree ( Args );
        }

        #region Expand Utility methods
        public void ShowProgress ( )
        {
            if ( ProgressCount < PROGRESSWRAPVALUE ) ProgressCount++;
            else ProgressCount = 0;
            if ( BusyLabelColor == FindResource ( "Red5" ) as SolidColorBrush )
            { BusyLabelColor = FindResource ( "Yellow0" ) as SolidColorBrush;
                BusyLabelBkgrn = FindResource ( "Black0" ) as SolidColorBrush;
            } else
            { BusyLabelColor = FindResource ( "Red5" ) as SolidColorBrush;
                BusyLabelBkgrn = FindResource ( "White2" ) as SolidColorBrush;
            }
            
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
                len = 1;
            else len = levels . Length - 1;
            return len;
        }
        public void UpdateListBox ( string entry )
        {
            listBox . Items . Add ( entry );
            listBox . ScrollIntoView ( entry );
            // Bound in xaml
            Listboxtotal++;
//            Expandcounter . Text = $"{Listboxtotal}";
        }
        private void ExpandSetup ( bool direction )
        {
            if ( direction )
            {
                ProgressCount = 0;
                ProgressString = "";
                StartTimer ( );
                listBox . Items . Clear ( );
                Thickness th = new Thickness ( 2 , 2 , 2 , 2 );
                Expandprogress . BorderThickness = th;
                Expandcounter . BorderThickness = th;
                Expandprogress . BorderBrush = FindResource ( "Red5" ) as SolidColorBrush;
                Expandcounter . Foreground = FindResource ( "Yellow1" ) as SolidColorBrush;
                Expandprogress . UpdateLayout ( );
                Expandcounter . BorderBrush = FindResource ( "Red5" ) as SolidColorBrush;
                Expandcounter . UpdateLayout ( );
                BusyLabel . Visibility = Visibility . Visible;
                Mouse . OverrideCursor = Cursors . Wait;
            }
            else
            {
                Expandprogress . BorderBrush = FindResource ( "White0" ) as SolidColorBrush;
                Expandcounter . BorderBrush = FindResource ( "White0" ) as SolidColorBrush;
                Expandcounter . Foreground = FindResource ( "White4" ) as SolidColorBrush;
                Thickness th = new Thickness ( 0 , 0 , 0 , 0 );
                Expandprogress . BorderThickness = th;
                Expandcounter . BorderThickness = th;
                Expandprogress . Text = "Finished ...";
                BusyLabel . Visibility = Visibility . Hidden;
                listBox . Items . Add ( $"\nExpanded to {listboxtotal } items ..." );
                listBox . ScrollIntoView ( $"\nExpanded to {listboxtotal } items ..." );
                //Expandcounter . Text = "";
                ShowExpandTime ( );
                Expandprogress . Refresh ( );
                Expandcounter . Refresh ( );
                listBox . Items.Add("");
                Mouse . OverrideCursor = Cursors . Arrow;
            }
        }
        public void ScrollCurrentTvItemIntoView ( TreeViewItem item )
        {
            // Brings selected item  into view as selected item
            var count = VisualTreeHelper . GetChildrenCount ( item );

            for ( int i = count - 1 ; i >= 0 ; --i )
            {
                var childItem = VisualTreeHelper . GetChild ( item , i );
                ( ( FrameworkElement ) childItem ) . BringIntoView ( );
            }
        }
        #endregion Expand Utility methods
    } // End of CLASS TreeViews
}
