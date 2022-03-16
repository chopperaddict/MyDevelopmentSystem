using MyDev . Converts;
using MyDev . ViewModels;

using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . ComponentModel;
using System . Diagnostics;
using System . Globalization;
using System . IO;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Input;
using System . Windows . Media;
using System . Windows . Shapes;

using static System . Net . WebRequestMethods;

namespace MyDev . Views
{
	/// <summary>
	/// Interaction logic for TreeViews.xaml
	/// </summary>
	#region MenuItem Class
	public class MenuItem : INotifyPropertyChanged
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

		public ExplorerClass myfolder = new ExplorerClass();
		private bool isSelected;
		public bool IsSelected
		{
			get { return isSelected; }
			set { isSelected = value; OnPropertyChanged ( IsSelected . ToString ( ) ); }
		}
		private bool isExpanded;
		public bool IsExpanded
		{
			get { return isExpanded; }
			set { isExpanded = value; OnPropertyChanged ( IsExpanded . ToString ( ) ); }
		}

		public MenuItem ( )
		{
			this . Items = new ObservableCollection<MenuItem> ( );
		}

		public string Title { get; set; }

		public ObservableCollection<MenuItem> Items { get; set; }
	}
	#endregion MenuItem Class

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

		#region full Props
		private string t1SelectedItem;
		public string T1SelectedItem
		{
			get { return t1SelectedItem; }
			set { t1SelectedItem = value; OnPropertyChanged ( T1SelectedItem ); }
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

		private string  defaultDrive ;
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
		//private ExplorerClass explorer;
		//public ExplorerClass Explorer
		//{
		//	get { return explorer; }
		//	set { explorer = value; }
		//}
		//public ExplorerClass TvExplorer=null;

		#endregion full Props

		#region Public dclarations
		//		public static LazyLoading Lazytree = null;
		public List<Family> families = new List<Family>();
		public Family family1 = new Family();
		public static List<string> LbStrings = new List<string>();
		public bool LoggingToListbox { get; set; }
		//		public static ListBox Tvlistbox = new ListBox();
		public static bool ShowAllfiles=false;
		public string CurrentDrive { set; get; }
		TreeViewItem CurrentItem = new TreeViewItem();
		DirectoryInfo DirInfo = new DirectoryInfo (@"C:\\" );
		public static TreeViews treeViews { get; set; }
		//		public static Directories  Directorys;
		#endregion Public dclarations

		public TreeViews ( )
		{
			InitializeComponent ( );
			this . DataContext = new Class1 ( );
	
			Utils . SetupWindowDrag ( this );
			treeViews = this;
			MenuItem root = new MenuItem() { Title = "Menu" };
			MenuItem childItem1 = new MenuItem() { Title = "Child item #1" };
			childItem1 . Items . Add ( new MenuItem ( ) { Title = "Child item #1.1" } );
			childItem1 . Items . Add ( new MenuItem ( ) { Title = "Child item #1.2" } );
			root . Items . Add ( childItem1 );
			root . Items . Add ( new MenuItem ( ) { Title = "Child item #2" } );
			//			treeView3 . Items . Add ( root );
			listBox . Items . Clear ( );
			treeView4 . Items . Clear ( );
			//			Directorys = new Directories ( );
		}
		private void Window_Loaded ( object sender , RoutedEventArgs e )
		{
			CreateStaticData ( );
			LoadDrives ( );
		}
		private void LoadDrives ( )
		{
			treeView4 . Items . Clear ( );
			listBox . Items . Clear ( );
			listBox . UpdateLayout ( );
			foreach ( var drive in Directory . GetLogicalDrives ( ) )
			{
				var   item = new TreeViewItem();
				item . Header = drive;
				item . Tag = drive;
				item . Items . Add ( "Loading" );
				treeView4 . Items . Add ( item );
				listBox . Items . Add ( item . Tag . ToString ( ) );
			}
		}
		//private void AddItem_Click ( object sender , RoutedEventArgs e )
		//{
		//	TreeViewItem newChild = new TreeViewItem();
		//	if ( CurrentTree <= 2 && CurrentLevel == 1 )
		//	{
		//		newChild . Header = Textbox . Text;
		//		if ( CurrentTree == 1 )
		//			treeView1 . Items . Add ( newChild );
		//		else if ( CurrentTree == 2 )
		//			treeView2 . Items . Add ( newChild );
		//	}
		//	else if ( CurrentTree == 3 && CurrentLevel == 1 )
		//	{
		//		newChild . Header = Textbox . Text;
		//		family1 = new Family ( ) { Name = Textbox . Text };
		//		families . Add ( family1 );
		//		treeView1 . Items . Add ( newChild );
		//	}
		//	else if ( CurrentTree == 3 && CurrentLevel == 2 )
		//	{
		//		newChild . Header = Textbox . Text;
		//		family1 = new Family ( ) { Name = Textbox . Text };
		//		families . Add ( family1 );
		//		//					Members members =. new Members ( );
		//		//				Family . Members . Add (Name=Textbox . Text );
		//		treeView3 . ItemsSource = null;
		//		treeView3 . ItemsSource = families;
		//		//				treeView3 . Item . Add ( newChild );
		//	}
		//}
		//private void DeleteItem_Click ( object sender , RoutedEventArgs e )
		//{
		//	// Removes ROOT items only, not submenu items
		//	treeView1 . Items . RemoveAt ( treeView1 . Items . IndexOf ( treeView1 . SelectedItem ) );
		//}

		private void App_Close ( object sender , RoutedEventArgs e )
		{
			this . Close ( );
			Application . Current . Shutdown ( );
		}

		private void Close_Btn ( object sender , RoutedEventArgs e )
		{
			this . Close ( );
		}

		#region Expanding
		void ExpandAll ( ItemsControl items , bool expand )
		{
			// Handle Expand / Contract for buttons
			foreach ( object obj in items . Items )
			{
				ItemsControl childControl = items.ItemContainerGenerator.ContainerFromItem(obj) as ItemsControl;
				if ( childControl != null )
				{
					ExpandAll ( childControl , expand );
				}
				TreeViewItem item = childControl as TreeViewItem;
				if ( item != null )
					item . IsExpanded = true;
			}
		}
		//private void ContractAll ( object sender , RoutedEventArgs e )
		//{
		//	// Contract all button

		//	TreeView tv = treeView1 as TreeView;
		//	ExpandTreeview ( tv , false );
		//	tv = treeView2 as TreeView;
		//	ExpandTreeview ( tv , false );
		//	tv = treeView3 as TreeView;
		//	ExpandTreeview ( tv , false );
		//}
		//private void ExpandAll ( object sender , RoutedEventArgs e )
		//{
		//	// Expand all button
		//	TreeView tv = treeView1 as TreeView;
		//	ExpandTreeview ( tv , true );
		//	tv = treeView2 as TreeView;
		//	ExpandTreeview ( tv , true );
		//	tv = treeView3 as TreeView;
		//	ExpandTreeview ( tv , true );
		//}
		//private void ExpandTreeview ( TreeView tv , bool direction )
		//{
		//	// Service method  to handle Expand/Contract of any treeview recieved as a parameter
		//	foreach ( var item in tv . Items )
		//	{
		//		TreeViewItem treeItem = tv.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
		//		if ( treeItem != null )
		//		{
		//			ExpandAll ( treeItem , direction );
		//			treeItem . IsExpanded = direction;
		//		}
		//	}
		//}
		#endregion Expanding

		private void treeView3_MouseDoubleClick ( object sender , MouseButtonEventArgs e )
		{

		}

		#region Dependency Properties
		public bool tv1SelectedItem
		{
			get { return ( bool ) GetValue ( tv1SelectedItemProperty ); }
			set { SetValue ( tv1SelectedItemProperty , value ); }
		}
		public static readonly DependencyProperty tv1SelectedItemProperty =
			DependencyProperty.Register("tv1SelectedItem", typeof(bool), typeof(TreeViews), new PropertyMetadata(false));
		public bool tv2SelectedItem
		{
			get { return ( bool ) GetValue ( tv2SelectedItemProperty ); }
			set { SetValue ( tv2SelectedItemProperty , value ); }
		}
		public static readonly DependencyProperty tv2SelectedItemProperty =
			DependencyProperty.Register("tv2SelectedItem", typeof(bool), typeof(TreeViews), new PropertyMetadata(false));
		public bool tv3SelectedItem
		{
			get { return ( bool ) GetValue ( tv3SelectedItemProperty ); }
			set { SetValue ( tv3SelectedItemProperty , value ); }
		}
		public static readonly DependencyProperty tv3SelectedItemProperty =
			DependencyProperty.Register("tv3SelectedItem", typeof(bool), typeof(TreeViews), new PropertyMetadata(false));

		#endregion Dependency Properties

		private void tv3Item_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
		{
			WrapPanel   name = sender as WrapPanel ;
		}
		#region selection changing
		private void treeView1_SelectedItemChanged ( object sender , RoutedPropertyChangedEventArgs<object> e )
		{
			CurrentTree = 1;
			CurrentLevel = 1;
			Selection . Text = "";
			// How to get the currently seleted item's "Header" string
			var entry = e.NewValue as TreeViewItem;
			T1SelectedItem = entry?.Header . ToString ( );
			Selection . Text = T1SelectedItem;
		}
		private void treeView2_SelectedItemChanged ( object sender , RoutedPropertyChangedEventArgs<object> e )
		{
			CurrentTree = 2;
			CurrentLevel = 1;
			// How to get the currently seleted item's "Header" string
			var entry = e.NewValue as TreeViewItem;
			T2SelectedItem = entry?.Header . ToString ( );
			Selection . Text = T2SelectedItem;
		}
		private void treeView3_SelectedItemChanged ( object sender , RoutedPropertyChangedEventArgs<object> e )
		{
			CurrentTree = 3;
			// How to get the currently seleted item's data when the Treeiew is Dynamic
			// We parse down the levels in the tree to find out what level we have selected
			var entry = e.NewValue as Family;
			if ( entry != null )
			{
				T3SelectedItem = entry?.Name;
				Selection . Text = T3SelectedItem;
				CurrentLevel = 1;
				return;
			}
			else
			{
				var entry2 = e.NewValue as FamilyMember;
				entry2 = e . NewValue as FamilyMember;
				if ( entry2 != null )
				{
					CurrentLevel = 2;
					T3SelectedItem = entry2 . Name;
					string Empstatus = entry2.Employed == true ? "Yes" : "No";
					FullDetail = entry2 . Name + ", " + entry2 . Age + " years, Employed : " + Empstatus;
					Selection . Text = FullDetail;
					return;
				}
			}
		}
		private void treeView4_SelectedItemChanged ( object sender , RoutedPropertyChangedEventArgs<object> e )
		{
			var item =  e.NewValue as TreeViewItem;
			//int  count = LazyLoading.TreeViewItem_CountFiles ( item, ShowAllfiles);
			//Selection . Text = $"{item . Header.ToString()}, Count = {count}";
		}
		#endregion selection changing

		private void TreeViewItem_Expanded ( object sender , RoutedEventArgs e )
		{
			#region Expanding  setup
			int itemscount = 0;
			Mouse . SetCursor ( Cursors . Wait );
			TreeViewItem item = e.Source as TreeViewItem;
			//	LazyLoading . TreeViewItem_Expanded (sender, e );
			listBox . Items . Clear ( );
			listBox . UpdateLayout ( );
			//if ( item . IsExpanded )
			//{
			//	StringToImageMultiConverter ( item.Name , typeof ( string) , item.IsExpanded, CultureInfo . CurrentCulture ) );

			//}
			#endregion Expanding  setup

			#region Expanding Get Folders

			var directories = new List<string>();
			var Allfiles= new List<string>();
			string Fullpath = item.Tag.ToString().ToUpper();
			string InfoMessage="";
			int DirectoryCount = 0;
			int FileCount = 0;
			itemscount = item . Items . Count;
			item . Items . Clear ( );
			// Get a list of all items in the current folder
			GetDirectories ( Fullpath , out directories );
			DirectoryCount = directories . Count;
			if ( directories . Count >= 0 )
			{
				AddDirectoriesToTreeview ( directories , item , listBox );
				//// Check to see if there any file items in the current folder

				if ( DirectoryCount > 0 )
					InfoMessage = $"Current Item : {Fullpath} -  {DirectoryCount} SubDirectory(s)";
				else
					InfoMessage = $"Current Item : {Fullpath} -  No SubDirectories ";

				GetFiles ( Fullpath , out Allfiles );
				FileCount = Allfiles . Count;
				if ( FileCount > 0 )
				{
					AddFilesToTreeview ( Allfiles , item , listBox );
					InfoMessage += $", {FileCount} Files";
				}
				else
					InfoMessage += $",  No Files";
				Selection . Text = InfoMessage;

				//FileInfo fi = new FileInfo(Fullpath);
				//FileAttributes fa =  fi . Attributes;
				//string attr = fa . ToString ( );
				//try
				//{
				//	double len = (double)fi.Length;
				//	if ( len > 1024 * 1024 * 1024 )
				//	{
				//		len = len / ( 1024 * 1024 * 1024 );
				//		Selection . Text = $"{InfoMessage}, {String . Format ( "{0:##,###}" , fi . Length / ( 1024 * 1024 ) ) } MBytes, ({attr})";
				//	}
				//	else if ( len > 1024 * 1024 )
				//	{
				//		len = len / ( 1024 * 1024 );
				//		Selection . Text = $"{InfoMessage}, {String . Format ( "{0:##,###}" , fi . Length / 1024 ) } MBytes, ({attr})";
				//	}
				//	else if ( len > 1024 )
				//	{
				//		len = len / 1024;
				//		Selection . Text = $"{InfoMessage}, {String . Format ( "{0:##,###} KBytes" , fi . Length ) }, ({attr})";
				//	}
				//	else
				//		Selection . Text = $"{InfoMessage}, {len} Bytes, ({attr})";
				//} catch
				//{
				//	if ( attr . Contains ( "Directory" ) )
				//		Selection . Text = $"{InfoMessage},  (Empty Directory)";
				//	else
				//		Selection . Text = $"{InfoMessage},  ({attr})";
				//}
				return;
			}
			//else
			//{
			//	// No Directories, but may have files, so check it out
			//	GetFiles ( Fullpath , out Allfiles );

			//	FileInfo fi = new FileInfo(Fullpath);
			//	FileAttributes fa =  fi . Attributes;
			//	string attr = fa . ToString ( );
			//	try
			//	{
			//		double len = (double)fi.Length;
			//		if ( len > 1024 * 1024  * 1024)
			//		{
			//			len = len / ( 1024 * 1024 * 1024);
			//			Selection . Text = $"File : {Fullpath}, {String . Format ( "{0:##,###}" , fi . Length / (1024 * 1024) ) } MBytes, ({attr})";
			//		}
			//		else if ( len > 1024 * 1024 )
			//		{
			//			len = len / ( 1024 * 1024 );
			//			Selection . Text = $"File : {Fullpath}, {String.Format("{0:##,###}", fi.Length/1024) } MBytes, ({attr})";
			//		}
			//		else if ( len > 1024 )
			//		{
			//			len = len / 1024;
			//			Selection . Text = $"File : {Fullpath}, {String.Format("{0:##,###} KBytes", fi.Length) }, ({attr})";
			//		}
			//		else
			//			Selection . Text = $"File : {Fullpath}, {len} Bytes, ({attr})";
			//	} catch 
			//	{
			//		if(attr.Contains("Directory"))
			//			Selection . Text = $"File : {Fullpath},  (Empty Directory)";
			//		else
			//			Selection . Text = $"File : {Fullpath},  ({attr})";
			//	}
			//	return;
			//}

			#endregion Expanding Get Folders

			#region Expanding Get Files

			var files= new List<string>();
			Fullpath = item . Tag . ToString ( );
			// Get a list of all (file) items in the current folder
			GetFiles ( Fullpath , out files );

			// Add them to our treeview
			if ( files . Count > 0 )
			{
				AddFilesToTreeview ( files , item , listBox );

				#endregion Expanding Get Files
				if ( item . Items . Count <= 1 )
				{
					FileInfo fi = new FileInfo(Fullpath);
					FileAttributes fa =  fi . Attributes;
					string attr = fa . ToString ( );
					try
					{
						double len = fi.Length;
						if ( len > 1024 * 1024 )
						{
							len = len / ( 1024 * 1024 );
							Selection . Text = $"File : {Fullpath}, {len } M/Bytes, ({attr})";
						}
						else if ( len > 1024 )
						{
							len = len / 1024;
							Selection . Text = $"File : {Fullpath}, {len } K/Bytes, ({attr})";
						}
						else
							Selection . Text = $"File : {Fullpath}, {len} Bytes, ({attr})";
					} catch
					{
						if ( attr . Contains ( "Directory" ) )
							Selection . Text = $"File : {Fullpath},  (Empty Directory)";
						else
							Selection . Text = $"File : {Fullpath},  ({attr})";
					}
				}
			}
			listBox . Refresh ( );
			Mouse . SetCursor ( Cursors . Arrow );
		}

		public static void GetDirectories ( string path , out List<string> dirs )
		{
			List<string> directories = new List<string>();
			try
			{
				var directs = Directory . GetDirectories( path);
				if ( directs . Length > 0 )
					directories . AddRange ( directs );
			} catch { }
			dirs = directories;
		}
		public static void GetFiles ( string path , out List<string> allfiles )
		{
			var files= new List<string>();
			// Get a list of all items in the current folder
			try
			{
				var file = Directory . GetFiles( path);
				if ( file . Length > 0 )
					files . AddRange ( file );
			} catch { }
			allfiles = files;
		}
		public static void AddDirectoriesToTreeview ( List<string> directories , TreeViewItem item , ListBox lBox )
		{
			directories . ForEach ( directoryPath =>
			{
				var subitem = new TreeViewItem()
				{
					Header = GetFileFolderName(directoryPath),
					Tag = directoryPath
				};
				if ( CheckIsVisible ( directoryPath . ToUpper ( ) , ShowAllfiles ) == true )
				{     // add the dummy entry
					subitem . Items . Add ( "Loading" );
					// force it  to iterate  recursively
					TreeViews tvs = new TreeViews();
					subitem . Expanded += tvs . TreeViewItem_Expanded;
					// Add item to parent
					item . Items . Add ( subitem );
					lBox . Items . Add ( subitem . Header . ToString ( ) );
				}
			} );
		}
		public static void AddFilesToTreeview ( List<string> Allfiles , TreeViewItem item , ListBox lBox )
		{
			Allfiles . ForEach ( filePath =>
			{
				var subitem = new TreeViewItem()
				{
					Header = GetFileFolderName(filePath),
					Tag = filePath
				};
				// Add item to parent
				//				 string entry = filePath.ToUpper();
				//string s = fa.ToString();
				if ( CheckIsVisible ( filePath . ToUpper ( ) , ShowAllfiles ) == true )
				{
					item . Items . Add ( subitem );
					lBox . Items . Add ( filePath );
				}
			} );
		}
		private void treeView4_Selected ( object sender , RoutedEventArgs e )
		{
			int  count = 0;
			string str2 = "";
			string str3 = "";
			string immediateparent = "";
			string path ="";

			TreeViewItem  tvi = treeView4.SelectedItem as TreeViewItem;
			var tag = tvi . Tag;

			// fully qualified path to selected item
			var s = tag . ToString ( );
			// This is  the current selection under the cursor !
			string selectedItem  = tvi . Tag.ToString() ;
			TreeViewItem_Expanded ( sender , e );
			return;
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

		private void Window_KeyDown ( object sender , KeyEventArgs e )
		{
			if ( e . Key == Key . F8 )
				Debugger . Break ( );
		}
		public void ReloadListBox ( )
		{
			listBox . ItemsSource = null;
			listBox . Items . Clear ( );
			listBox . Refresh ( );
			foreach ( var items in LbStrings )
			{
				listBox . Items . Add ( items );
			}
			listBox . UpdateLayout ( );
			listBox . Refresh ( );
		}

		private void ShowallFiles_Click ( object sender , RoutedEventArgs e )
		{
			CheckBox cb = sender as CheckBox;
			if ( cb . IsChecked == true )
				ShowAllfiles = true;
			else
				ShowAllfiles = false;
		}

		private void Refresh_Btn ( object sender , RoutedEventArgs e )
		{
			treeView4 . Items . Clear ( );
			LoadDrives ( );
		}
		private static TreeViewItem FindParentTreeViewItem ( object child )
		{
			try
			{
				var parent = VisualTreeHelper.GetParent(child as DependencyObject);
				while ( ( parent as TreeViewItem ) == null )
				{
					parent = VisualTreeHelper . GetParent ( parent );
				}
				return parent as TreeViewItem;
			} catch ( Exception e )
			{
				//could not find a parent of type TreeViewItem
				Console . WriteLine ( e . Message );
				return null;
			}
		}
		public static string GetFileFolderName ( string path )
		{
			if ( string . IsNullOrEmpty ( path ) )
				return String . Empty;
			var normalizedPath = path.Replace('/', '\\');
			var lastindex = normalizedPath.LastIndexOf('\\');
			if ( lastindex <= 0 )
				return path;
			return path . Substring ( lastindex + 1 );
		}
		private void treeView4_Selected_1 ( object sender , RoutedEventArgs e )
		{

		}
		private void treeView4_Collapsed ( object sender , RoutedEventArgs e )
		{
			TreeViewItem item = e.Source as TreeViewItem;
			CurrentItem = item;
			string header = item . Header . ToString ( );
			//if ( header . Contains ( "\\" ) )
			//{
			//	CurrentDrive = item . Header . ToString ( );
			//	Directorys . CurrentDrive = CurrentDrive;
			//	Directorys . IsExpanded = true;
			//	Directorys . IsCollapsed = false;
			//}
			//else
			//{
			//	Directorys . CurrentDirectory = header;
			//	Directorys . DirectoryInfo = new DirectoryInfo ( Directorys . CurrentDrive + header );
			//	Directorys . IsExpanded = false;
			//	Directorys . IsCollapsed = true;
			//}
			listBox . Refresh ( );
			Mouse . SetCursor ( Cursors . Arrow );
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

		private void treeViewModel_SelectedItemChanged ( object sender , RoutedPropertyChangedEventArgs<object> e )
		{

		}

		private void treeViewModel_Expanded ( object sender , RoutedEventArgs e )
		{

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

		private void ExpandAll ( object sender , RoutedEventArgs e )
		{

		}

		private void TestViewModel ( object sender , RoutedEventArgs e )
		{
			//var v = DirectoryStructure . GetLogicalDrives ( );
			//foreach ( var item in v)
			//{
			//	treeViewModel . Items . Add ( item.FullPath );
			//}
		}
	}
}
