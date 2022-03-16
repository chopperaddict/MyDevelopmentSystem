using MyDev . Converts;

using System;
using System . Collections . Generic;
using System . ComponentModel;
using System . Diagnostics;
using System . Globalization;
using System . IO;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Input;
using System . Windows . Markup . Localizer;
using System . Windows . Media;
using System . Windows . Shapes;

using static System . Net . WebRequestMethods;

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

		private static bool isresettingSelection { get; set; } = false;

		#region startup
		public TreeViews ( )
		{
			InitializeComponent ( );
			this . DataContext = this;

			Utils . SetupWindowDrag ( this );
			treeViews = this;
			//MenuItem root = new MenuItem("Parent0" );
			//root . Title = "";
			//MenuItem childItem1 = new MenuItem() { Title = "Child item #1" };
			//childItem1 . Items . Add ( new MenuItem ( ) { Title = "Child item #1.1" } );
			//childItem1 . Items . Add ( new MenuItem ( ) { Title = "Child item #1.2" } );
			//root . Items . Add ( childItem1 );
			//root . Items . Add ( new MenuItem ( ) { Title = "Child item #2" } );
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
		#endregion startup

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
			WrapPanel   name = sender as WrapPanel ;
		}

		private void treeViewModel_SelectedItemChanged ( object sender , RoutedPropertyChangedEventArgs<object> e )
		{
			var v = sender as TreeViewItem;

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

		#endregion other treeviews

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
			#endregion selection changing
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
				// Add Dummy entry so we get an "Can be Opened" triangle icon
				item . Items . Add ( "Loading" );
				// Add Drive to Treeview
				GetDirectories(drive, out List<string>directories );
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
			var normalizedPath = path.Replace('/', '\\');
			var lastindex = normalizedPath.LastIndexOf('\\');
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

		private void treeView4_Selected ( object sender , RoutedEventArgs e )
		{
			int  count = 0;
			string str2 = "";
			string str3 = "";
			string immediateparent = "";
			string path ="";
		
			if ( isresettingSelection == true )
			{
				isresettingSelection = false;
				return;
			}

			TreeViewItem  tvi = treeView4.SelectedItem as TreeViewItem;
			if ( tvi == null )
				return;
			var tag = tvi . Tag;
			//if ( tvi . IsSelected == false )
			//{
			//}
			// fully qualified path to selected item
			var s = tag . ToString ( );
			// This is  the current selection under the cursor !
			string selectedItem  = tvi . Tag.ToString() ;
			//if ( tvi . IsExpanded == true )
			//	tvi . IsExpanded = false;
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
			isresettingSelection = false;
			tvi . IsExpanded = false;
			e . Handled = true;
			return;
		}
		private void treeView4_Collapsed ( object sender , RoutedEventArgs e )
		{
			TreeViewItem item = e.OriginalSource as TreeViewItem;
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
			var item =  e.NewValue as TreeViewItem;
			if ( item != null )//&& item . IsSelected == false )
			{
				item . IsSelected = true;
				item . IsExpanded = false;
			}

			e . Handled = true;

			//int  count = LazyLoading.TreeViewItem_CountFiles ( item, ShowAllfiles);
			//Selection . Text = $"{item . Header.ToString()}, Count = {count}";
		}

		private void treeView4_MouseDoubleClick ( object sender , MouseButtonEventArgs e )
		{
			//TreeView tv = sender as  treeView4.SelectedItem;
			treeView4_Selected ( sender , e );
			TreeViewItem  tvi = treeView4.SelectedItem as TreeViewItem;
			if ( tvi == null )
				return;
			var tag = tvi . Tag;
			TreeViewItem4_Expanded ( tvi , null );
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
			if ( item . IsSelected == true )
				item . IsSelected = false;
			listBox . Items . Clear ( );
			listBox . UpdateLayout ( );
			#endregion Expanding  setup

			#region Expanding Get Folders

			var directories = new List<string>();
			var Allfiles= new List<string>();
			string Fullpath = item.Tag.ToString().ToUpper();

			string InfoMessage="";
			int DirectoryCount = 0;
			int FileCount = 0;
			itemscount = item . Items . Count;
			var tvi = item  as TreeViewItem;
			if ( itemscount == 0 )
				return;
			var itemheader  = item.Items[0] .ToString();
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

		private void SetCurrentSelectedItem(string path)
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
				var subitem = new TreeViewItem();
				subitem . Header = GetFileFolderName ( directoryPath );
				subitem . Tag = directoryPath;
				if ( CheckIsVisible ( directoryPath . ToUpper ( ) , ShowAllfiles ) == true )
				{     // add the dummy entry to each of the subdirectories we are adding to the tree so we get the Expand icons
					subitem . Items . Add ( "Loading" );
					// force it  to iterate  recursively
					TreeViews tvs = new TreeViews();
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
				var subitem = new TreeViewItem()
				{
					Header = GetFileFolderName(filePath),
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
			List<string> directories = new List<string>();
			try
			{
				var directs = Directory . GetDirectories( path);
				if ( directs . Length > 0 )
				{
					foreach ( var item in directs )
					{
						if ( CheckIsVisible ( item . ToUpper ( ) , ShowAllfiles ) == true )
							directories . Add ( item );
					}
				}
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
				{
					foreach ( var item in file )
					{
						if ( CheckIsVisible ( item . ToUpper ( ) , ShowAllfiles ) == true )
							files . Add ( item );
					}
					//					files . AddRange ( file );
				}
			} catch { }
			allfiles = files;
		}
		#endregion Treeview support methods

	}
}
