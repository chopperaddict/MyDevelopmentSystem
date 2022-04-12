using MyDev . Converts;
using MyDev . Models;
using MyDev . UserControls;
using MyDev . ViewModels;

using Newtonsoft . Json . Linq;

using System;
using System . Collections . Generic;
using System . ComponentModel;
using System . Diagnostics;
using System . IO;
using System . Linq;
using System . Runtime . InteropServices . Expando;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Input;
using System . Windows . Markup . Localizer;
using System . Windows . Media;
using System . Windows . Media . Imaging;
using System . Windows . Shapes;


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

        #endregion full Props

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
            treeViews = this;
            listBox . Items . Clear ( );
            treeView4 . Items . Clear ( );
            fdl = new FlowdocLib ( );
            FdMargin . Left = Flowdoc . Margin . Left;
            FdMargin . Top = Flowdoc . Margin . Top;
            TvExplorer = new ExplorerClass ( );
            treeViewModel . SelectedValuePath = "C:\\";
            TreeViewItem tvi = new TreeViewItem ( );
            tvi . Header = @"C:\";
            treeViewModel . SetValue ( SelectedItemProperty , tvi );
          }
        private void Window_Loaded ( object sender , RoutedEventArgs e )
        {
            string output = "";
            this . SetValue ( FontsizeProperty , InfoList . FontSize );
            canvas . Visibility = Visibility . Visible;
            CreateStaticData ( );
            LoadDrives ( );
            Flowdoc . ExecuteFlowDocMaxmizeMethod += new EventHandler ( MaximizeFlowDoc );
            List<String> errors = new List<string> ( );
            LazyLoadingTreeview . LazyLoadTreeview ( treeViewModel , this , ref errors );
            if ( errors . Count > 0 )
            {
                foreach ( var item in errors )
                {
                    listBox . Items . Add ( "<-- " + item );
                    output += item + "\n";
                }
            }
            //Grid1 . RowDefinitions [ 1 ] . Height = new GridLength ( 3 , GridUnitType . Star );
            //Grid1 . RowDefinitions [ 2 ] . Height = new GridLength ( 25 , GridUnitType . Pixel );
            Grid1 . RowDefinitions [ 3 ] . Height = new GridLength ( 155 , GridUnitType . Pixel );
            LsplitterImage = new BitmapImage ( new Uri ( @"\icons\Lrg updown arrow red copy.png" , UriKind . Relative ) );
            VsplitterImage = new BitmapImage ( new Uri ( @"\icons\Lrg ltrt arrow red copy.png" , UriKind . Relative ) );
           ShowDriveInfo ( sender , e );
            //fdl . FdMsg ( Flowdoc , canvas , "Full Drive Information :-" , output , "" );
        }
        #endregion startup        

        #region close dwn
        private void App_Close ( object sender , RoutedEventArgs e )
        {
            this . Close ( );
            Application . Current . Shutdown ( );
        }

        private void Close_Btn ( object sender , RoutedEventArgs e )
        {
            this . Close ( );
        }
        #endregion close dwn

        #region Expanding
        void ExpandAll ( ItemsControl items , bool expand )
        {
            //TreeViewItem[] items = tv.Items;
            // Handle Expand / Contract for buttons
            if ( ExpandAllFolders )
            {
                foreach ( object obj in items . Items )
                {
                    ItemsControl childControl = items . ItemContainerGenerator . ContainerFromItem ( obj ) as ItemsControl;
                    if ( childControl != null )
                    {
                        ExpandAll ( childControl , expand );
                    }
                    TreeViewItem item = childControl as TreeViewItem;
                    if ( item != null )
                        item . IsExpanded = true;
                }
            }
            else
            {
                //ItemsControl childControl = items . ItemContainerGenerator . ContainerFromItem ( obj ) as ItemsControl;
                //if ( childControl != null )
                //{
                //    ExpandAll ( childControl , expand );
                //}
                //TreeViewItem item = childControl as TreeViewItem;
                //if ( item != null )
                //    item . IsExpanded = true;

            }
        }
        #endregion Expanding

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

        private void treeViewModel_SelectedItemChanged ( object sender , RoutedPropertyChangedEventArgs<object> e )
        {
            //			var tree =( TreeViewItem ) treeViewModel . ItemContainerGenerator . ContainerFromItem ( e . OriginalSource ) as TreeViewItem;
            //           var v = e . OriginalSource as TreeViewItem;
            // This  gets the Current selectedItem Correctly !
            var tvItem = treeViewModel . ItemContainerGenerator . ContainerFromItem ( ( ( TreeView ) sender ) . SelectedItem );

        }

        private void treeViewModel_Expanded ( object sender , RoutedEventArgs e )
        {
            var sel = treeViewModel . SelectedItem as TreeViewItem;
            if ( sel == null )
            {
                if ( CurrentTreeItem == null )
                    return;
            }
            LazyLoadingTreeview . TreeViewItem4_Expanded ( sender , e , CurrentTreeItem );
        }

        private void treeViewModel_Collapsed ( object sender , RoutedEventArgs e )
        {

        }

        private void treeViewModel_Selected ( object sender , RoutedEventArgs e )
        {

        }

        private void TesViewModel ( object sender , RoutedEventArgs e )
        {

        }

        /// <summary>
        /// This workks and Expands the currently seleted level AND ALL the folders in this  folder
        ///  but does not iterate firther down the tree.....
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpandAll ( object sender , RoutedEventArgs e )
        {
            TreeView tv = treeView4;
            string stre = tv . SelectedValuePath;
            Console . WriteLine ( $"Expanding {stre}" );
            treeView4 . UpdateLayout ( );
            // Get   the currently selected node
            var rootTreeViewItem = treeView4 . SelectedItem as TreeViewItem;
            if ( rootTreeViewItem != null && ExpandAllFolders == true )
            {
                ExpandAll3 ( rootTreeViewItem , true );
            }
            else
            {
                ExpandAll3 ( rootTreeViewItem , true );
            }

            return;

            //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%//

            //for ( int x = 0 ; x < tv.Items.Count ; x++ )
            //{
            //    TreeViewItem tvi = new TreeViewItem ( );
            //    tvi = tv . Items [ x ] as TreeViewItem;
            //        if(tvi != null)
            //    tvi.IsExpanded = true;

            //    ItemsControl ic = new ItemsControl ( );
            //    ic = tvi;
            //    Console . WriteLine ( $" ic.Count = {ic.Items.Count},HasItems = {ic . HasItems . ToString ( )}");
            //    ExpandAll ( ic, true );
            //}
            //if(true)
            //{
            //    foreach ( object item in this . treeView4 . Items )
            //    {
            //        TreeViewItem treeItem = this . treeView4 . ItemContainerGenerator . ContainerFromItem ( item ) as TreeViewItem;
            //        if ( treeItem != null )
            //        {
            //            //   ExpandAll ( treeItem , true );
            //                 foreach ( object obj in treeItem . Items )
            //                {
            //                    ItemsControl childControl = treeItem . ItemContainerGenerator . ContainerFromItem ( obj ) as ItemsControl;
            //                    if ( childControl != null )
            //                    {
            //                        ExpandAll ( childControl , true );
            //                    }
            //                    TreeViewItem item2 = childControl as TreeViewItem;
            //                    if ( item2 != null )
            //                        item2 . IsExpanded = true;
            //                }
            //        }
            //        treeItem . IsExpanded = true;
            //    }

            //}
        }

        private void TestViewModel ( object sender , RoutedEventArgs e )
        {
            //var v = DirectoryStructure . GetLogicalDrives ( );
            //foreach ( var item in v)
            //{
            //	treeViewModel . Items . Add ( item.FullPath );
            //}
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
        private void LoadDrives ( )
        {
            treeView4 . Items . Clear ( );
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
                    treeView4 . Items . Add ( item );
                    // Add ot listbox so we can check what has ben added (Debug)
                    listBox . Items . Add ( item . Tag . ToString ( ) );
                }
            }
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
            treeView4 . Items . Clear ( );
            LoadDrives ( );
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
            DependencyProperty . Register ( "LsplitterImage" , typeof ( BitmapImage ) , typeof ( TreeViews) , new PropertyMetadata ( (BitmapImage)null ) );

        public BitmapImage VsplitterImage
        {
            get
            { return ( BitmapImage ) GetValue ( VsplitterImageProperty ); }
            set { SetValue (VsplitterImageProperty , value ); }
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
            DependencyProperty . Register ( "LeftSplitterText" , typeof ( string ) , typeof ( TreeViews ) , new PropertyMetadata ( ( string ) "Drag Up/Down " ) );
        public string RightSplitterText
        {
            get { return ( string ) GetValue ( RightSplitterTextProperty ); }
            set { SetValue ( RightSplitterTextProperty , value ); }
        }

        // Using a DependencyProperty as the backing store for LeftSplitterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightSplitterTextProperty =
            DependencyProperty . Register ( "RightSplitterText" , typeof ( string ) , typeof ( TreeViews ) , new PropertyMetadata ( ( string ) "to View Drive Information." ) );


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
            DependencyProperty . RegisterAttached ( "tvselection" , typeof ( bool ) , typeof ( TreeViews ) , new PropertyMetadata ( ( bool ) false ) );

        #endregion Attached Properties

        public bool TvSelectedItem { get; set; }
        //        public DependencyProperty SelectedItemProperty { get; set; }


        //public bool TvSelectedItem
        //{
        //	get { return ( bool ) GetValue ( TvSelectedItemProperty ); }
        //			set { SetValue ( TvSelectedItemProperty , value ); }
        //}
        //public static readonly DependencyProperty TvSelectedItemProperty =
        //          DependencyProperty . Register( "TvSelectedItem" , typeof ( bool ) , typeof ( TreeViews ) , new PropertyMetadata ( false ) );



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

        private void treeViewModel_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
        {
            if ( Utils . HitTestScrollBar ( sender , e ) )
            {
                return;
            }
            if ( treeViewModel . Items . CurrentItem == null )
                CurrentTreeItem = treeViewModel . Items . GetItemAt ( 0 ) as TreeViewItem;
            else
            {
            }
            var indx = treeViewModel . Items . IndexOf ( CurrentTreeItem );
            var v = ( TreeViewItem ) treeViewModel . ItemContainerGenerator . ContainerFromItem ( e . OriginalSource ) as TreeViewItem;
            var v2 = ( TreeViewItem ) treeViewModel . ItemContainerGenerator . ContainerFromItem ( CurrentTreeItem ) as TreeViewItem;
            Console . WriteLine ( $"Left Button Down returns ? index={indx},OriginalSource={e . OriginalSource}, CurrentTreeItem={CurrentTreeItem}" );
            LazyLoadingTreeview . TreeViewItem4_Expanded ( sender , null , CurrentTreeItem );
            //if(v == null)
            //{
            //    treeViewModel . Items . MoveCurrentTo ( itm);
            //    var pos = treeViewModel . Items . CurrentPosition;
            //    treeViewModel . Items . Refresh ( );
            //    treeViewModel_Expanded ( sender , null);
            //   treeViewModel_SelectedItemChanged ( sender , null );
            //    treeViewModel_Expanded ( sender , null );
            //}
            //treeViewModel . SelectedItem= v;
        }

        private void treeViewModel_Loaded ( object sender , RoutedEventArgs e )
        {
            // This  gets the Current selectedItem Correctly !
            TreeViewItem tvi = new TreeViewItem ( );
            tvi . Header = @"C:\";
            treeViewModel . SetValue ( SelectedItemProperty , tvi );
            treeViewModel . SetCurrentValue ( SelectedItemProperty , tvi );
            var sel = treeViewModel . SelectedItem;
            //. SetCurrentSelectedItem(@"C:\\" );
            var tvItem = treeViewModel . ItemContainerGenerator . ContainerFromItem ( ( ( TreeView ) sender ) . SelectedItem );
            var curr = TvExplorer . CurrentDrive;
            treeViewModel . Focus ( );
        }

        private void ShowDriveInfo ( object sender , RoutedEventArgs e )
        {
            string output = "";
            ExplorerClass Texplorer = new ExplorerClass ( );
            Texplorer . GetDrives ( "C:\\" );
            List<lbitemtemplate> lbtmplates = new List<lbitemtemplate> ( );
            //         DriveInfo [ ] drives = DriveInfo . GetDrives ( );
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
                //DriveType}, Format = {driveInfo . DriveFormat}, Volume = {driveInfo . VolumeLabel}, {sizestr}, SubDirectories = {Texplorer . Directories.Count}, Files = {Texplorer . Files. Count} \n";
                continue;

                //if ( driveInfo . IsReady == true )                //{
                //    tmp = driveInfo . TotalSize . ToString ( );
                //    if (  tmp . Length <= 4 )
                //        sizestr = driveInfo . TotalSize . ToString ( ) + "Mb";
                //    else
                //    {
                //        long totalsize = ( driveInfo . TotalSize ) / ( 1024 * 1024 );
                //         tmp = totalsize . ToString ( );
                //        if(tmp.Length >= 6)
                //            sizestr = tmp.Substring(0,3)+"," + tmp . Substring ( 3 , 3) + " Gb";
                //        else
                //            sizestr = tmp  + " Mb's";
                //    }
                //    DirectoryInfo di = new DirectoryInfo ( driveInfo.Name );

                //    var dirs = new List<DirectoryInfo> ( );
                //    dirs = Texplorer . GetDirectories ( driveInfo . Name );
                //    List<FileInfo> AllFiles = Texplorer . GetFiles ( driveInfo . Name );

                //    output += $"Drive [{driveInfo . Name}, Type = {driveInfo . DriveType}, Format = {driveInfo . DriveFormat}, Volume = {driveInfo . VolumeLabel}, {sizestr}, SubDirectories = {dirs.Count}, Files = {AllFiles.Count} \n";
                //}
                //        foreach ( var item in treeViewModel . Items )
                //{
                //    TreeViewItem itm = item as TreeViewItem;
            }
            //    output += itm.Name + ", " + itm.HasItems.ToString() + ", " + itm.Tag.ToString() + "\n";
            InfoList . ItemsSource = null;
            InfoList . ItemsSource = lbtmplates;
            // fdl . FdMsg ( Flowdoc , canvas , "Treeview Drives Information" , $"{output}" );

        }

        //        public static void ExpandAll ( this TreeViewItem treeViewItem , bool isExpanded = true )
        //        public static void ExpandAll2 ( TreeView tv, TreeViewItem treeViewItem , bool isExpanded = true )
        //        {
        ////            var stack = new Stack<TreeViewItem> ( treeViewItem . Items . Cast<TreeViewItem> ( ) );
        //            var stack = new Stack<TreeViewItem> ( tv . Items.Cast<TreeViewItem>());
        //            while ( stack . Count > 0 )
        //            {
        //                TreeViewItem item = stack . Pop ( );

        //                foreach ( var child in item . Items )
        //                {
        //                    var childContainer = child as TreeViewItem;
        //                    if ( childContainer == null )
        //                    {
        //                        childContainer = item . ItemContainerGenerator . ContainerFromItem ( child ) as TreeViewItem;
        //                    }

        //                    stack . Push ( childContainer );
        //                }

        //                item . IsExpanded = isExpanded;
        //            }
        //        }

        #region Expand // Collapse
        private void ExpandAll3 ( TreeViewItem items , bool expand )
        {
            if ( items == null)
                return;
            TreeViewItem topfolder = items? . Tag as TreeViewItem;
            items . IsExpanded = expand;
            //            if ( !expand )
            //            {
            //                items . IsExpanded = false;
            //                return;
            //            }
            ////            TreeViewItem tvi2 = treeView4 . SelectedItem as TreeViewItem;
            if ( expand && ExpandAllFolders == true )
            {
                // Expanding
                foreach ( object obj in items . Items )
                {
                    ItemsControl childControl = items . ItemContainerGenerator . ContainerFromItem ( obj ) as ItemsControl;
                    if ( childControl != null )
                    {
                        Console . WriteLine ( $"Collapsing {childControl . DisplayMemberPath}" );
                        ExpandAll3 ( childControl as TreeViewItem , expand );

                    }
                    TreeViewItem item = childControl as TreeViewItem;
                    TreeViewItem tvi = obj as TreeViewItem;
                    if ( tvi != null )
                        tvi . IsExpanded = true;
                }
            }
            else
            {
                // Ezpanding  a single layer, or Collapsing currently open folder
                TreeViewItem tvi2 = treeView4 . SelectedItem as TreeViewItem;
                if ( tvi2 != null )
                {
                    if ( expand )
                        tvi2 . IsExpanded = true;
                    else
                        tvi2 . IsExpanded = false;
                    treeView4 . UpdateLayout ( );
                }
            }
        }

        private void CollapseCurrentNode ( object sender , RoutedEventArgs e )
        {
            TreeView tv = treeView4;
            string stre = tv . SelectedValuePath;
            treeView4 . UpdateLayout ( );
            // Get   the currently selected node
            var rootTreeViewItem = treeView4 . SelectedItem as TreeViewItem;
            //            Console . WriteLine ( $"Collapsing {rootTreeViewItem.DisplayMemberPath}" );
            if ( rootTreeViewItem != null )
            {
                //CollapseAll ( TreeView tv , TreeViewItem treeViewItem )
                ExpandAll3 ( rootTreeViewItem , false );
            }
        }
        #endregion Expand // Collapse



        #region Treeview support methods
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
            if ( showall == false
                //( s . ToUpper ( ) . Contains ( "BOOTMGR" ) == false
                //&& s . ToUpper ( ) . Contains ( "BOOTNXT" ) == false
                //&& s . ToUpper ( ) . Contains ( "BOOTSTAT" ) == false
                //&& s . ToUpper ( ) . Contains ( "BOOTSECT" ) == false )
                && ( entry . Contains ( "BOOTMGR" ) == true
                || entry . Contains ( "BOOTNXT" ) == true
                || entry . Contains ( "BOOTSTAT" ) == true
                || entry . Contains ( "RECOVERY" ) == true
                || entry . Contains ( "BOOTNXT" ) == true
                || entry . Contains ( "BACKUP_PARTITION" ) == true
                || entry . Contains ( "BOOTSECT" ) == true ) )
            {
                //Console . WriteLine ($"{entry} NOT listed");
                return false;
            }
            else
                return true;
        }
        public static int AddDirectoriesToTreeview ( List<string> directories , TreeViewItem item , ListBox lBox )
        {
            int added = 0;
            directories . ForEach ( directoryPath =>
            {
                var subitem = new TreeViewItem ( );
                subitem . Header = GetFileFolderName ( directoryPath );
                subitem . Tag = directoryPath;
                if ( CheckIsVisible ( directoryPath . ToUpper ( ) , ShowAllfiles ) == true )
                {     // add the dummy entry to each of the subdirectories we are adding to the tree so we get the Expand icons
                    subitem . Items . Add ( "Loading" );
                    // force it  to iterate  recursively
                    TreeViews tvs = new TreeViews ( );
                    item . Items . Add ( subitem );
                    lBox . Items . Add ( subitem . Header . ToString ( ) );
                    // Add item to parent
                    subitem . Expanded += tvs . TreeViewItem4_Expanded;
                    added++;
                }
            } );
            return added;
        }
        public static int AddFilesToTreeview ( List<string> Allfiles , TreeViewItem item , ListBox lBox )
        {
            int count = 0;
            Allfiles . ForEach ( filePath =>
            {
                var subitem = new TreeViewItem ( )
                {
                    Header = GetFileFolderName ( filePath ) ,
                    Tag = filePath
                };
                if ( CheckIsVisible ( filePath . ToUpper ( ) , ShowAllfiles ) == true )
                {
                    item . Items . Add ( subitem );
                    lBox . Items . Add ( filePath );
                    count++;
                }
            } );
            return count;
        }

        public static void GetDirectories ( string path , out List<string> dirs )
        {
            List<string> directories = new List<string> ( );
            try
            {
                var directs = Directory . GetDirectories ( path );
                if ( directs . Length > 0 )
                {
                    foreach ( var item in directs )
                    {
                        if ( CheckIsVisible ( item . ToUpper ( ) , ShowAllfiles ) == true )
                            directories . Add ( item );
                    }
                }
            }
            catch { }
            dirs = directories;
        }
        public static void GetFiles ( string path , out List<string> allfiles )
        {
            var files = new List<string> ( );
            // Get a list of all items in the current folder
            try
            {
                var file = Directory . GetFiles ( path );
                if ( file . Length > 0 )
                {
                    foreach ( var item in file )
                    {
                        if ( CheckIsVisible ( item . ToUpper ( ) , ShowAllfiles ) == true )
                            files . Add ( item );
                    }
                    //					files . AddRange ( file );
                }
            }
            catch { }
            allfiles = files;
        }

        #endregion Treeview support methods


        #region Treeview4 Mouse selection of items
        private void treeView4_Selected ( object sender , RoutedEventArgs e )
        {
            bool isCollapsing = false;

            //if ( isresettingSelection == true )
            //{
            //    isresettingSelection = false;
            //    return;
            //}

            TreeViewItem tvi = treeView4 . SelectedItem as TreeViewItem;
            if ( tvi == null )
                return;
            CurrentItem = tvi;
            var tag = tvi . Tag;
            if ( tvi . IsExpanded == true )
            {
                isCollapsing = true;
                isresettingSelection = true;
                tvi . IsSelected = true;
                return;
            }
            else
                tvi . IsExpanded = true;
            //           else
            //             isCollapsing = true;
            //if ( tvi . IsSelected == false )
            //{
            //}
            // fully qualified path to selected item
            var s = tag . ToString ( );
            // This is  the current selection under the cursor !
            string selectedItem = tvi . Tag . ToString ( );
            //           if ( tvi . IsExpanded == true )
            //            	tvi . IsExpanded = false;
            //TreeViewItem4_Expanded  ( tvi , e );
            //if ( tvi . IsExpanded )
            //{
            //tvi . IsSelected = true;
            GetItemCounts ( selectedItem , out int Dircount , out int Filecount );
            isresettingSelection = true;
            CurrentFolder . Text = $"Current Folder Content(s) for : {selectedItem}";
            //tvi . Items . Clear ( );
            //tvi . Items . Add ( "Loading" );
            //TreeViewItem4_Expanded ( tvi , null );
            //}
            //return;
            isresettingSelection = true;
            tvi . IsSelected = true;
            if ( tvi . IsExpanded == false )
            {
                //e . Handled = true;
            }
            else
            {
                isresettingSelection = true;
            }
            return;
        }
        private void treeView4_Collapsed ( object sender , RoutedEventArgs e )
        {
            TreeViewItem item = e . OriginalSource as TreeViewItem;
            CurrentItem = item;
            //item . IsExpanded = false;
            item . Items . Clear ( );
            // Add Dummy entry just so we do get the expand icon
            item . Items . Add ( "Loading" );
            string header = item . Header . ToString ( );
            listBox . Items . Clear ( );
            listBox . Refresh ( );
            Mouse . SetCursor ( Cursors . Arrow );
            GetItemCounts ( header , out int Dircount , out int Filecount );
        }

        private void treeView4_SelectedItemChanged ( object sender , RoutedPropertyChangedEventArgs<object> e )
        {
            //if ( isresettingSelection == true )
            //{
            //    // Ensures that any open folder that is clicked on remains open when just being selected (not dblclicked)
            //    isresettingSelection = false;
            //    return;
            //}
            var item = e . NewValue as TreeViewItem;
            if ( item != null )//&& item . IsSelected == false )
            {
                item . IsSelected = true;
                item . IsExpanded = true;
            }

            //           e . Handled = true;

            // This  gets the Current selectedItem Correctly, but  as a DependencyObject - not sure how to use that !
            var tvItem = treeView4 . ItemContainerGenerator . ContainerFromItem ( ( ( TreeView ) sender ) . SelectedItem );
            SetValue ( tv4SelectedItemProperty , item );
            CurrentItem = item;
        }
        private void treeView4_PreviewMouseRightButtonDown ( object sender , MouseButtonEventArgs e )
        {
            this . Flowdoc . Height = 200;

            //treeView4.GetValue(H)
            fdl . FdMsg ( Flowdoc , canvas , "Testing FdMsg in Treeview" , $"{treeView4 . DisplayMemberPath}" );
        }

        private void TreeViewItem4_Expanded ( object sender , RoutedEventArgs e )
        {
            #region Expanding  setup
            TreeViewItem item = null;
            int itemscount = 0;
            if ( e != null )
                item = e . Source as TreeViewItem;
            else
                item = sender as TreeViewItem;
            Mouse . SetCursor ( Cursors . Wait );
            if ( item == null )
                return;

            // This is CRITICAL to get any drive that is currently selected to open when the expand icon is clicked
            //if ( item . IsSelected == true )
            //item . IsSelected = false;
            listBox . Items . Clear ( );
            listBox . UpdateLayout ( );

            item . IsSelected = true;
            #endregion Expanding  setup

            #region Expanding Get Folders

            var directories = new List<string> ( );
            var Allfiles = new List<string> ( );
            string Fullpath = item . Tag . ToString ( ) . ToUpper ( );

            string InfoMessage = "";
            int DirectoryCount = 0;
            int FileCount = 0;
            itemscount = item . Items . Count;
            var tvi = item as TreeViewItem;
            if ( itemscount == 0 )
                return;
            var itemheader = item . Items [ 0 ] . ToString ( );
            if ( itemheader == "Loading" )
                item . Items . Clear ( );
            // Get a list of all items in the current folder
            GetDirectories ( Fullpath , out directories );
            if ( directories == null )
                return;
            DirectoryCount = directories . Count;
            if ( directories . Count >= 1 )
            {
                DirectoryCount = AddDirectoriesToTreeview ( directories , item , listBox );
            }
            else
                DirectoryCount = 0;
            //// Check to see if there any file items in the current folder
            if ( DirectoryCount > 0 )
                InfoMessage = $"Current Item : {Fullpath} -  {DirectoryCount} SubDirectory(s)";
            else
            {
                if ( ShowAllfiles )
                    InfoMessage = $"Current Item : {Fullpath} -  No SubDirectories ";
                else
                    InfoMessage = $"Current Item : {Fullpath} -  No valid SubDirectories ";
            }
            GetFiles ( Fullpath , out Allfiles );
            FileCount = Allfiles . Count;
            if ( FileCount > 0 )
            {
                int added = AddFilesToTreeview ( Allfiles , item , listBox );
                if ( added == 0 )
                    InfoMessage += $",  No Files";
                else
                    InfoMessage += $", {added} Files";
            }
            else
                InfoMessage += $",  No Files";
            Selection . Text = InfoMessage;
            treeView4 . UpdateLayout ( );
            CurrentFolder . Text = $"Current Folder Content(s) for : {Fullpath}";

            Mouse . SetCursor ( Cursors . Arrow );
            //			isresettingSelection = true;
            //			item . IsSelected = true;
            //			isresettingSelection = false;
            return;

            #endregion Expanding Get Folders

            #region Expanding Get Files	  (UNUSED)

            //var files= new List<string>();
            //Fullpath = item . Tag . ToString ( );
            //// Get a list of all (file) items in the current folder
            //GetFiles ( Fullpath , out files );

            //// Add them to our treeview
            //if ( files . Count > 0 )
            //{
            //	int added = AddFilesToTreeview ( files , item , listBox );

            //	//#endregion Expanding Get Files
            //	if ( item . Items . Count <= 1 )
            //	{
            //		FileInfo fi = new FileInfo(Fullpath);
            //		FileAttributes fa =  fi . Attributes;
            //		string attr = fa . ToString ( );
            //		try
            //		{
            //			double len = fi.Length;
            //			if ( len > 1024 * 1024 )
            //			{
            //				len = len / ( 1024 * 1024 );
            //				Selection . Text = $"File : {Fullpath}, {len } M/Bytes, ({attr})";
            //			}
            //			else if ( len > 1024 )
            //			{
            //				len = len / 1024;
            //				Selection . Text = $"File : {Fullpath}, {len } K/Bytes, ({attr})";
            //			}
            //			else
            //				Selection . Text = $"File : {Fullpath}, {len} Bytes, ({attr})";
            //		} catch
            //		{
            //			if ( attr . Contains ( "Directory" ) )
            //				Selection . Text = $"File : {Fullpath},  (Empty Directory)";
            //			else
            //				Selection . Text = $"File : {Fullpath},  ({attr})";
            //		}
            //	}
            //	else
            //	{

            //	}
            //}
            //listBox . Refresh ( );
            //Mouse . SetCursor ( Cursors . Arrow );

            #endregion Expanding Get Files	  (UNUSED)
        }

        private void treeView4_MouseDoubleClick ( object sender , MouseButtonEventArgs e )
        {
            TreeView tv = treeView4;
            string stre = tv . SelectedValuePath;
            treeView4 . UpdateLayout ( );
            // Get   the currently selected node
            var rootTreeViewItem = treeView4 . SelectedItem as TreeViewItem;
            //            Console . WriteLine ( $"Collapsing {rootTreeViewItem.DisplayMemberPath}" );
            //            if( rootTreeViewItem .IsExpanded)
            if ( rootTreeViewItem != null && rootTreeViewItem . IsExpanded == false )
            {
                ExpandAll3 ( rootTreeViewItem , true );
                // ExpandAll3 ( rootTreeViewItem , false );
                rootTreeViewItem . IsExpanded = true;
            }
            else
                ExpandAll3 ( rootTreeViewItem , false );


            return;

            treeView4_PreviewMouseDoubleClick ( sender , e );
            //TreeView tv = sender as  treeView4.SelectedItem;
            //treeView4_Selected ( sender , e );
            TreeViewItem tvi = treeView4 . SelectedItem as TreeViewItem;
            if ( tvi == null )
                return;
            //var tag = tvi . Tag;
            TreeViewItem4_Expanded ( tvi , null );
        }
        private void treeView4_PreviewMouseDoubleClick ( object sender , MouseButtonEventArgs e )
        {
            TreeView tv = treeView4;
            string stre = tv . SelectedValuePath;
            treeView4 . UpdateLayout ( );
            // Get   the currently selected node
            var rootTreeViewItem = treeView4 . SelectedItem as TreeViewItem;
            //            Console . WriteLine ( $"Collapsing {rootTreeViewItem.DisplayMemberPath}" );
            //            if( rootTreeViewItem .IsExpanded)
            if ( rootTreeViewItem != null && rootTreeViewItem . IsExpanded == false )
            {
                ExpandAll3 ( rootTreeViewItem , true );
                // ExpandAll3 ( rootTreeViewItem , false );
                rootTreeViewItem . IsExpanded = true;
            }
            else
                ExpandAll3 ( rootTreeViewItem , false );
            //rootTreeViewItem . UpdateLayout ( );
        }
        private void treeView4_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
        {
            var rootTreeViewItem = treeView4 . SelectedItem as TreeViewItem;
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

        #endregion Treeview4 Mouse selection of items

        private void SetCurrentSelectedItem ( string path )
        {
            //foreach ( var dir in treeView4 . Items )
            //{
            //	var tvi = dir as TreeViewItem;
            //	if ( tvi . Header . ToString ( ) == path )
            //	{
            //		tvi . IsSelected = true;
            //		break;
            //	}
            //}

        }

        private void LeftSplitter_DragStarted ( object sender , System . Windows . Controls . Primitives . DragStartedEventArgs e )
        {
            if ( row1 . ActualHeight <= 10 )
            {
                LeftSplitterText = "Drag Down";
                LsplitterImage = new BitmapImage ( new Uri ( @"\icons\down arroiw red.png" , UriKind . Relative ) );
                RightSplitterText= "to View Tree Directory  Information.";
            }
            else if ( row2. ActualHeight <= 10 )
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
    } // End of CLASS TreeViews

    internal class TreeViewItems
    {
    }
}
