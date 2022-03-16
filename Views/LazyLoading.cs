using System . Windows . Controls;

using MyDev . ViewModels;

namespace MyDev . Views
{
	public class XLazyLoading
	{
		//		private static bool ShowAllFiles = false;
		//		private static TreeViews treeviews=null;
		//		private static ListBox TvListbox = null;

		//		public XLazyLoading ( TreeView treeview , ref Directories directories , bool showall = false )
		//		{
		//		}

		////		public static void xTreeViewItem4_Expanded  ( object sender, RoutedEventArgs e)
		//		{
		//			// Create and populate the Treeview with entries for current node
		//			// get parent to open
		////			string path=e.Tag.ToString();
		//			var directories = new List<string>();
		//			//tvtree . Items . Clear ( );
		//			TvListbox . Items . Clear ( );
		//			TvListbox . UpdateLayout ( );
		//			var item = sender as TreeViewItem;

		//			string Fullpath = (string)item.Tag;
		//			if ( item . Items . Count != 1 || item . Items [ 0 ] != null )
		//				return;
		//			try
		//			{
		//				var dirs = Directory . GetDirectories( Fullpath);
		//				if ( dirs . Length > 0 )
		//					directories . AddRange ( dirs );
		//			  } catch { }
		//			directories . ForEach ( directoryPath =>
		//			{
		//				var subitem = new TreeViewItem()
		//				{
		//					Header = Path.GetDirectoryName(directoryPath),
		//					Tag = directoryPath
		//				};
		//				// add the dummy entry
		//				subitem. Items . Add ( null );
		//				// force it  to iterate 
		//				subitem . Expanded += TreeViewItem4_Expanded ;
		//				item . Items . Add ( subitem );
		//				TvListbox . Items . Add ( subitem );
		//			} );
		//			//foreach ( var dir in directories )
		//			//{
		//			//	Header = dir;
		//			//	Tag = dir;
		//			//	tvtree . Items . Add(dir );
		//			//	TvListbox . Items . Add(item );
		//			//}


		//			{
		//				//DirectoryInfo expandedDir = null;
		//				//if ( item . Tag is DriveInfo )
		//				//	expandedDir = ( item . Tag as DriveInfo ) . RootDirectory;
		//				//else if ( item . Tag is DirectoryInfo )
		//				//	expandedDir = ( item . Tag as DirectoryInfo );

		//				//// Read all sub folder first 
		//				//try
		//				//{
		//				//	int count = -1;
		//				//	foreach ( DirectoryInfo subDir in expandedDir . GetDirectories ( ) )
		//				//	{
		//				//		// Exclude System files/Folders
		//				//		FileAttributes fa = subDir.Attributes;
		//				//		string entry = subDir.ToString().ToUpper();
		//				//		string s = fa.ToString();
		//				//		if ( CheckIsVisible (entry , s , showall ) == false )
		//				//		{
		//				//			if ( count == -1 )
		//				//			{
		//				//				count = TreeViewItem_CountDirectories ( subDir ,ref directories, showall );
		//				//				count += TreeViewItem_CountFiles ( subDir , ref directories , showall );
		//				//			}
		//				//			if ( count >= 1 )
		//				//			{
		//				//				item . Items . Add ( CreateTreeItem ( subDir , ref directories ) );
		//				//			}
		//				//			else
		//				//			{
		//				//				item . Items . Add ( CreateTreeFile ( subDir ) );
		//				//			}
		//				//			treeviews . listBox . Items . Add ( subDir . ToString ( ) );
		//				//		}
		//				//		else if ( showall == true )
		//				//		{
		//				//			item . Items . Add ( CreateTreeItem ( subDir , ref directories ) );
		//				//			treeviews . listBox . Items . Add ( subDir . ToString ( ) );
		//				//			Console . WriteLine ( $"Hidden Directory: subDir" );
		//				//		}
		//				//		//directories . DirectoryInfo = subDir;
		//				//		//directories . CurrentDirectory = subDir . Name;
		//				//		//directories . FullPath= subDir . FullName;
		//				//	}
		//				//} catch { }
		//			}
		//	// Now Read all files in this sub folder 
		//	//try
		//	//{
		//	//	foreach ( FileInfo subDir in expandedDir . GetFiles ( ) )
		//	//	{
		//	//		// Exclude System files/Folders
		//	//		FileAttributes fa = subDir.Attributes;
		//	//		string entry = subDir.ToString().ToUpper();
		//	//		string s = fa.ToString();
		//	//		if ( CheckIsVisible ( entry , s , showall ) == false )
		//	//		{
		//	//			int count = TreeViewItem_CountFiles( expandedDir , ref directories, showall );
		//	//			if(count >= 1)
		//	//				item . Items . Add ( CreateTreeFile ( subDir ) );
		//	//			else
		//	//				item . Items . Add ( CreateTreeFile ( subDir ) );
		//	//			treeviews . listBox . Items . Add ( subDir . ToString ( ) );
		//	//			directories . FileInfo = subDir;
		//	//			directories . CurrentSelection = subDir . Name;
		//	//		}
		//	//		else if ( showall == true )
		//	//		{
		//	//			int count = TreeViewItem_CountFiles( expandedDir , ref directories, showall );
		//	//			item . Items . Add ( CreateTreeFile ( subDir ) );
		//	//			treeviews . listBox . Items . Add ( subDir . ToString ( ) );
		//	//			directories . FileInfo = subDir;
		//	//			Console . WriteLine ( $"Hidden file : subDir" );
		//	//		}
		//	//	}
		//	//} catch { }
		//	//}
		//	//			treeviews . listBox . Refresh ( );
		//}
		//public static int xTreeViewItem_CountDirectories ( DirectoryInfo e , ref Directories directories , bool showall = false )
		//{
		//	// Just count all entries DIRECTLY below the selected tree node
		//	int EntryCount = 0;
		//	try
		//	{
		//		Console . WriteLine ( "COUNT RESTART" );
		//		foreach ( DirectoryInfo subDir in e . GetDirectories ( ) )
		//		{
		//			FileAttributes fa = subDir.Attributes;
		//			string entry = subDir.ToString().ToUpper();
		//			string s = fa.ToString();
		//			if ( CheckIsVisible ( entry , s , showall ) == false )
		//			{
		//				EntryCount++;
		//				Console . WriteLine ( $"Dir  : {subDir} / {EntryCount}" );
		//			}
		//			else if ( showall )
		//			{
		//				EntryCount++;
		//				Console . WriteLine ( $"Dir  (Hidden) : {subDir} / {EntryCount}" );
		//			}
		//		}
		//	} catch { } finally { }
		//	try
		//	{
		//		foreach ( FileInfo subDir in e . GetFiles ( ) )
		//		{
		//			FileAttributes fa = subDir.Attributes;
		//			string entry = subDir.ToString().ToUpper();
		//			string s = fa.ToString();
		//			if ( CheckIsVisible ( entry , s , showall ) == false )
		//			{
		//				EntryCount++;
		//				Console . WriteLine ( $"File : {subDir} / {EntryCount}" );
		//			}
		//			else if ( showall )
		//			{
		//				EntryCount++;
		//				Console . WriteLine ( $"File : (Hidden) : {subDir} / {EntryCount}" );
		//			}
		//		}
		//	} catch { }
		//	Console . WriteLine ( "COUNT END" );
		//	return EntryCount;
		//}
		//private static bool xCheckIsVisible ( string entry , string s , bool showall )
		//{
		//	if ( showall == false &&
		//		( s . ToUpper ( ) . Contains ( "BOOTMGR" ) == false
		//		&& s . ToUpper ( ) . Contains ( "BOOTNXT" ) == false
		//		&& s . ToUpper ( ) . Contains ( "BOOTSTAT" ) == false
		//		&& s . ToUpper ( ) . Contains ( "BOOTSECT" ) == false )
		//		&& ( entry . Contains ( "BOOTMGR" ) == false
		//		&& entry . Contains ( "BOOTNXT" ) == false
		//		&& entry . Contains ( "BOOTSTAT" ) == false
		//		&& entry . Contains ( "RECOVERY" ) == false
		//		&& entry . Contains ( "BOOTNXT" ) == false
		//		&& entry . Contains ( "BACKUP_PARTITION" ) == false
		//		&& entry . Contains ( "BOOTSECT" ) == false ) )
		//		return false;
		//	else
		//		return true;
		//}
		//public static int xTreeViewItem_CountFiles ( DirectoryInfo e , ref Directories directories , bool showall = false )
		//{
		//	// Just count all entries DIRECTLY below the selected tree node
		//	int EntryCount = 0;
		//	try
		//	{
		//		foreach ( FileInfo subDir in e . GetFiles ( ) )
		//		{
		//			EntryCount++;
		//		}
		//	} catch { }
		//	return EntryCount;
		//}

		//#region Low level update  methods
		//private static TreeViewItem CreateTreeItem ( object o , ref Directories directories )
		//{
		//	string obj = o.ToString();
		//	TreeViewItem item = new TreeViewItem();
		//	item . Header = obj;
		//	item . Tag = obj;
		//	//			 add dummy entry
		//	item . Items . Add ( "Loading..." );
		//	if ( obj . Contains ( ":\\" ) )
		//		directories . CurrentDrive = obj;
		//	else
		//		directories . CurrentDirectory = obj;
		//	return item;
		//}
		//private static TreeViewItem CreateTreeFile ( object o )
		//{
		//	string str = o.ToString();
		//	TreeViewItem item = new TreeViewItem();
		//	item . Header = o . ToString ( );
		//	item . Tag = o . ToString ( );
		//	return item;
		//}
		//		#endregion Low level update  methods
	}

}