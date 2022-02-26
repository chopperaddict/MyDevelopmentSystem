using Dapper;

using MyDev . ViewModels;

using Newtonsoft . Json;

using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . ComponentModel;
using System . Diagnostics;
using System . IO;
using System . Linq;
using System . Runtime . InteropServices . Expando;
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
		private ExplorerClass explorer;
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
		public static ListBox Tvlistbox = new ListBox();
		public static bool ShowAllfiles=false;
		TreeViewItem CurrentItem = new TreeViewItem();

		#endregion Public dclarations

		public TreeViews ( )
		{
			InitializeComponent ( );
			this . DataContext = this;
			Utils . SetupWindowDrag ( this );

			//MenuItem root = new MenuItem() { Title = "Menu" };
			//MenuItem childItem1 = new MenuItem() { Title = "Child item #1" };
			//childItem1 . Items . Add ( new MenuItem ( ) { Title = "Child item #1.1" } );
			//childItem1 . Items . Add ( new MenuItem ( ) { Title = "Child item #1.2" } );
			//root . Items . Add ( childItem1 );
			//root . Items . Add ( new MenuItem ( ) { Title = "Child item #2" } );
			//treeView3 . Items . Add ( root );
			listBox.Items.Clear ( );
			treeView4 . Items . Clear ( );
			LazyLoading Lazytree = new LazyLoading ( treeView4, ShowAllfiles );
			//			Treeviewsclass = this;
			////			TvExplorer = TreeExplorer . Explorer;
			//			TvExplorer = new ExplorerClass();
			////			if ( TvExplorer . FullPath == "")
			//				TvExplorer . FullPath = @"C:\\";
			//			TvExplorer . GetDrives ( );
			//			TvExplorer . GetDirectories( );
			//			TvExplorer . GetFiles( );
			// Use my Folder class to get all data up front
		}
		private void Window_Loaded ( object sender , RoutedEventArgs e )
		{
			CreateStaticData ( );
			//Loads treeView4 with directory entries
			//			LazyLoading. LazyLoad( treeView4 );
			//DefaultDrive = @"C:\";
			//Tvlistbox = listBox;
			PopulateListbox ( );
		}

		private void AddItem_Click ( object sender , RoutedEventArgs e )
		{
			TreeViewItem newChild = new TreeViewItem();
			if ( CurrentTree <= 2 && CurrentLevel == 1 )
			{
				newChild . Header = Textbox . Text;
				if ( CurrentTree == 1 )
					treeView1 . Items . Add ( newChild );
				else if ( CurrentTree == 2 )
					treeView2 . Items . Add ( newChild );
			}
			else if ( CurrentTree == 3 && CurrentLevel == 1 )
			{
				newChild . Header = Textbox . Text;
				family1 = new Family ( ) { Name = Textbox . Text };
				families . Add ( family1 );
				treeView1 . Items . Add ( newChild );
			}
			else if ( CurrentTree == 3 && CurrentLevel == 2 )
			{
				newChild . Header = Textbox . Text;
				family1 = new Family ( ) { Name = Textbox . Text };
				families . Add ( family1 );
				//					Members members =. new Members ( );
				//				Family . Members . Add (Name=Textbox . Text );
				treeView3 . ItemsSource = null;
				treeView3 . ItemsSource = families;
				//				treeView3 . Item . Add ( newChild );
			}
		}
		private void DeleteItem_Click ( object sender , RoutedEventArgs e )
		{
			// Removes ROOT items only, not submenu items
			treeView1 . Items . RemoveAt ( treeView1 . Items . IndexOf ( treeView1 . SelectedItem ) );
		}

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
		private void ContractAll ( object sender , RoutedEventArgs e )
		{
			// Contract all button

			TreeView tv = treeView1 as TreeView;
			ExpandTreeview ( tv , false );
			tv = treeView2 as TreeView;
			ExpandTreeview ( tv , false );
			tv = treeView3 as TreeView;
			ExpandTreeview ( tv , false );
		}
		private void ExpandAll ( object sender , RoutedEventArgs e )
		{
			// Expand all button
			TreeView tv = treeView1 as TreeView;
			ExpandTreeview ( tv , true );
			tv = treeView2 as TreeView;
			ExpandTreeview ( tv , true );
			tv = treeView3 as TreeView;
			ExpandTreeview ( tv , true );
		}
		private void ExpandTreeview ( TreeView tv , bool direction )
		{
			// Service method  to handle Expand/Contract of any treeview recieved as a parameter
			foreach ( var item in tv . Items )
			{
				TreeViewItem treeItem = tv.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
				if ( treeItem != null )
				{
					ExpandAll ( treeItem , direction );
					treeItem . IsExpanded = direction;
				}
			}
		}
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
			var item = 	e.NewValue as TreeViewItem;
			int  count = LazyLoading.TreeViewItem_Count ( item, ShowAllfiles);
			Selection . Text = $"{item . Header.ToString()}, Count = {count}";
		}
		#endregion selection changing

		private void TreeViewItem_Expanded ( object sender , RoutedEventArgs e )
		{

			TreeViewItem item = e.Source as TreeViewItem;
			LazyLoading . TreeViewItem_Expanded ( item);
			CurrentItem = item;
			listBox . Refresh ( );
		}

		private void treeView4_Selected ( object sender , RoutedEventArgs e )
		{
			int x = 0;
//			var v = e.OriginalSource as TreeViewItem;
//			int  count = LazyLoading.TreeViewItem_Count ( v, ShowAllfiles);
//			Selection . Text = $"{v. Header . ToString ( )}, Count = {count}";
		}

		private void PopulateListbox ( )
		{
			// Use my Folder class to get all data up front
			//ExplorerClass explorer = new ExplorerClass();
			return;
			//List<string>  AllDrives;
			//LbStrings . Add ( "Path : " + TvExplorer . FullPath );
			//LbStrings . Add ( "Name : " + TvExplorer . Name );
			//if ( TvExplorer . FullPath == "" )
			//	TvExplorer . FullPath = @"C:\\";
			//AllDrives = TvExplorer. GetDrives( );
			//LbStrings . Add ( "Drives :" );
			//foreach ( var item in AllDrives )
			//{
			//	LbStrings . Add ( item );
			//}
			//LbStrings . Add ( "******************" );
			//List<DirectoryInfo> AllDirectories= TvExplorer. GetDirectories(TvExplorer.FullPath );
			//if (LoggingToListbox)
			//foreach ( var item in AllDirectories )
			//{
			//	LbStrings . Add ( item . Name );
			//}
			//LbStrings . Add ( "******************" );
			//List<FileInfo>  AllFiles = TvExplorer. GetFiles ( );
			//LbStrings . Add ( "Files :" );
			//foreach ( var item in AllFiles )
			//{
			//	LbStrings . Add ( item . Name );
			//}
			//listBox . ItemsSource = LbStrings;
		}
		private void CreateStaticData ( )
		{
			family1 = new Family ( ) { Name = "The Doe's" };
			family1 . Members . Add ( new FamilyMember ( ) { Name = "John Doe" , Age = 42 } );
			family1 . Members . Add ( new FamilyMember ( ) { Name = "Jane Doe" , Age = 39 } );
			family1 . Members . Add ( new FamilyMember ( ) { Name = "Sammy Doe" , Age = 13 } );
			families . Add ( family1 );

			Family family2 = new Family() { Name = "The Moe's" };
			family2 . Members . Add ( new FamilyMember ( ) { Name = "Mark Moe" , Age = 31 , Gender = "Male" , Employed = true } );
			family2 . Members . Add ( new FamilyMember ( ) { Name = "Norma Moe" , Age = 28 , Gender = "Female" , Employed = false } );
			families . Add ( family2 );

			treeView3 . ItemsSource = families;
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
			LazyLoading . TreeViewItem_Expanded ( CurrentItem , ShowAllfiles );
		}
	}
}
