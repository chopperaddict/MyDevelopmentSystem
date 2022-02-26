using System;
using System . Collections . Generic;
using System . IO;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows . Controls;
using System . Windows;

namespace MyDev . Views
{
	public class LazyLoading
	{
//		private static bool ShowAllFiles = false;
		public LazyLoading ( TreeView treeview, bool showall = false)
		{
 			DriveInfo[] drives = DriveInfo.GetDrives();
			foreach ( DriveInfo driveInfo in drives )
				treeview . Items . Add ( CreateTreeItem ( driveInfo ) );
		}

		public static void TreeViewItem_Expanded ( TreeViewItem e, bool showall=false )
		{
			// Create and populate the Treeview with entries for current node
			TreeViews . Tvlistbox . Items . Clear ( );
			TreeViewItem item = e;//.Source as TreeViewItem;
			if ( ( item . Items . Count == 1 ) && ( item . Items [ 0 ] is string ) )
			{
				item . Items . Clear ( );

				DirectoryInfo expandedDir = null;
				if ( item . Tag is DriveInfo )
					expandedDir = ( item . Tag as DriveInfo ) . RootDirectory;
				else if ( item . Tag is DirectoryInfo )
					expandedDir = ( item . Tag as DirectoryInfo );
				try
				{
					foreach ( DirectoryInfo subDir in expandedDir . GetDirectories ( ) )
					{
						// Exclude System files/Folders
						FileAttributes fa = subDir.Attributes;
						string entry = subDir.ToString().ToUpper();
						string s = fa.ToString();
						if(  
							( s . ToUpper ( ) . Contains ( "BOOTMGR" ) == false
							&& s . ToUpper ( ) . Contains ( "BOOTNXT" ) == false
							&& s . ToUpper ( ) . Contains ( "BOOTSTAT" ) == false
							&& s . ToUpper ( ) . Contains ( "BOOTSECT" ) == false )
							&& ( entry . Contains ( "BOOTMGR" ) == false
							&& entry . Contains ( "BOOTNXT" ) == false
							&& entry . Contains ( "BOOTSTAT" ) == false
							&& entry . Contains ( "BOOTNXT" ) == false
							&& entry . Contains ( "BACKUP_PARTITION" ) == false
							&& entry . Contains ( "BOOTSECT" ) == false ) )
						{
							item . Items . Add ( CreateTreeItem ( subDir ) );
							TreeViews . Tvlistbox . Items . Add ( "Type :  " + subDir . ToString ( ) );
						}
						else if ( showall == true )
						{
							item . Items . Add ( CreateTreeItem ( subDir ) );
							TreeViews . Tvlistbox . Items . Add ( subDir . ToString ( ) );
							Console . WriteLine ( $"Hidden Directory: subDir" );
						}
					}
				} catch { }
				try
				{
					foreach ( FileInfo subDir in expandedDir . GetFiles ( ) )
					{
						// Exclude System files/Folders
						FileAttributes fa = subDir.Attributes;
						string entry = subDir.ToString().ToUpper();
						string s = fa.ToString();
						if (
							( s . ToUpper ( ) . Contains ( "BOOTMGR" ) == false
							&& s . ToUpper ( ) . Contains ( "BOOTNXT" ) == false
							&& s . ToUpper ( ) . Contains ( "BOOTSTAT" ) == false
							&& s . ToUpper ( ) . Contains ( "BOOTSECT" ) == false )
							&& ( entry . Contains ( "BOOTMGR" ) == false
							&& entry . Contains ( "BOOTNXT" ) == false
							&& entry . Contains ( "BOOTSTAT" ) == false
							&& entry . Contains ( "BOOTNXT" ) == false
							&& entry . Contains ( "BACKUP_PARTITION" ) == false
							&& entry . Contains ( "BOOTSECT" ) == false ) )
						{
							item . Items . Add ( CreateTreeFile ( subDir ) );
							TreeViews . Tvlistbox . Items . Add ( subDir . ToString ( ) );
						}
						else if ( showall == true )
						{
							item . Items . Add ( CreateTreeFile ( subDir ) );
							TreeViews . Tvlistbox . Items . Add ( "Type : " + subDir . ToString ( ) );
							Console . WriteLine ( $"Hidden file : subDir" );
						}
					}
				} catch { }
			}
			TreeViews . Tvlistbox . Refresh ( );
		}
		public static int TreeViewItem_Count( TreeViewItem e , bool showall = false )
		{
			// Just count all entries DIRECTLY below the selected tree node
			int EntryCount = 0;
			TreeViewItem item = e;//.Source as TreeViewItem;
			if ( ( item . Items . Count == 1 ) && ( item . Items [ 0 ] is string ) )
			{
				DirectoryInfo expandedDir = null;
				if ( item . Tag is DriveInfo )
					expandedDir = ( item . Tag as DriveInfo ) . RootDirectory;
				else if ( item . Tag is DirectoryInfo )
					expandedDir = ( item . Tag as DirectoryInfo );
				try
				{
					foreach ( DirectoryInfo subDir in expandedDir . GetDirectories ( ) )
					{
						EntryCount++;
					}
				} catch { }
				try
				{
					foreach ( FileInfo subDir in expandedDir . GetFiles ( ) )
					{
						EntryCount++;
					}
					} catch { }
			}
			else 
				EntryCount = item . Items . Count;
			return EntryCount;
		}

	#region Low level update  methods
		private static TreeViewItem CreateTreeItem ( object o )
		{
			TreeViewItem item = new TreeViewItem();
			item . Header = o . ToString ( );
			item . Tag = o;
			item . Items . Add ( "Loading..." );
			return item;
		}
		private static TreeViewItem CreateTreeFile ( object o )
		{
			string str = o.ToString();
			TreeViewItem item = new TreeViewItem();
			item . Header = o . ToString ( );
			item . Tag = o;
			//			item . Items . Add ( o);
			return item;
		}
		#endregion Low level update  methods
	}

}