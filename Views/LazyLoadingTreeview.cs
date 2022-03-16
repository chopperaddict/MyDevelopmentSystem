using System;
using System . Collections . Generic;
using System . IO;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows . Controls;
using System . Windows;
using MyDev . ViewModels;
using System . Windows . Shapes;
using System . Runtime . Remoting . Messaging;

namespace MyDev . Views
{
	public class XLazyLoadingTreeview
	{
		//public static TreeView Treeview { get; set; }
		//public static TreeViews TVs { get; set; }

		//public static List<string> LbStrings = new List<string>();
		//public static ExplorerClass Texplorer;

		//public XLazyLoadingTreeview ( )
		//{
		//	Texplorer = new ExplorerClass();
		//	Texplorer . FullPath = @"C:\\";
		//}
		//public static void LazyLoadTreeview ( TreeView tv , TreeViews tvs )
		//{
		//	DriveInfo[] drives = DriveInfo.GetDrives();
		//	Treeview = tv;
		//	TVs = tvs;
		//	foreach ( DriveInfo driveInfo in drives )
		//		tv . Items . Add ( CreateTreeItem ( driveInfo ) );
		//}
		//public static void TreeViewItem4_Expanded  ( object sender , RoutedEventArgs e )
		//{
		//	string fullpath = "";
		//	// This is what we are updating in our TreeView
		//	TreeViewItem item = e.Source as TreeViewItem;

		//	TreeViewItem tv = e.Source as TreeViewItem;
		//	Texplorer = new ExplorerClass ( );

		//	string newpath = tv . Header . ToString ( );
		//	DirectoryInfo di = new DirectoryInfo(newpath);
		//	DriveInfo dinfo = new DriveInfo(newpath);
		//	List<string > _drives = new List<string>();
		//	var drives = Texplorer . GetDrives ( newpath);

		//	string currentdrive = Texplorer.Name;

		//	Console . WriteLine ( $"Count={item . Items . Count}, Content='{item . Items [ 0 ]}'" );
		//	LbStrings . Clear ( );
		//	if ( ( item . Items . Count == 1 ) && ( item . Items [ 0 ] is string ) )
		//	{
		//		item . Items . Clear ( );
		//		try
		//		{
		//			var dirs =new List<DirectoryInfo>();
		//			dirs = Texplorer . GetDirectories (  );
		//			foreach ( var dir in dirs )
		//			{
		//				FileAttributes fa = dir.Attributes;
		//				string s = fa.ToString();
		//				if (
		//					( s . ToUpper ( ) . Contains ( "BOOTMGR" )
		//					|| s . ToUpper ( ) . Contains ( "BOOTNXT" )
		//					|| s . ToUpper ( ) . Contains ( "BOOTSECT" ) ) == false )
		//				{
		//					item . Items . Add ( CreateTreeItem ( dir . Name ) );
		//					LbStrings . Add ( dir . Name );
		//				}
		//			}
		//		} catch { }
		//	}

		//	// Now use my Files Class to get the list of any lowest level files  & add to treeview 
		//	// by adding each one to the TreeViewItem collection "item"
		//	// if tey are not Special files or they are marked as Hidden
		//	List<FileInfo>  AllFiles = Texplorer. GetFiles ( );
		//	foreach ( var str in AllFiles )
		//	{
		//		FileAttributes fa = str.Attributes;
		//		string s = fa.ToString();
		//		if ( s . Contains ( "Hidden" ) == false )
		//		{
		//			if (
		//				( s . ToUpper ( ) . Contains ( "BOOTMGR" )
		//				|| s . ToUpper ( ) . Contains ( "BOOTNXT" )
		//				|| s . ToUpper ( ) . Contains ( "BOOTSECT" ) ) == false )
		//			{
		//				Console . WriteLine ($"{str}");
		//				item . Items . Add ( CreateTreeFile ( str ) );
		//				LbStrings . Add ( str . ToString ( ) );
		//			}
		//		}
		//	}
		//}

		//private static TreeViewItem CreateTreeItem ( object o )
		//{
		//	string str = o.ToString();
		//	TreeViewItem item = new TreeViewItem();
		//	item . Header = o . ToString ( );
		//	item . Tag = o;
		//	item . Items . Add ( "Loading..." );
		//	return item;
		//}
		//private static TreeViewItem CreateTreeFile ( object o )
		//{
		//	string str = o.ToString();
		//	TreeViewItem item = new TreeViewItem();
		//	item . Header = o . ToString ( );
		//	item . Tag = o;
		//	//			item . Items . Add ( o);
		//	return item;
		//}
	}
}

