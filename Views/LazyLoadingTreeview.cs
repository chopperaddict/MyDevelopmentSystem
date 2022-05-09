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
    public class LazyLoadingTreeview
    {
        public static TreeView Treeview { get; set; }
        public static TreeViews TVs { get; set; }

        public static List<string> LbStrings = new List<string> ( );
        public static ExplorerClass Texplorer;

        public LazyLoadingTreeview ( )
        {
            Texplorer = new ExplorerClass ( );
            Texplorer . FullPath = @"C:\\";
        }
        public static void LazyLoadTreeview ( TreeView tv , TreeViews tvs , ref List<string> errors )
        {
            DriveInfo [ ] drives = DriveInfo . GetDrives ( );
            Treeview = tv;
             foreach ( DriveInfo driveInfo in drives )
            {
                if ( driveInfo . IsReady == true )
                {
                    tv . Tag = driveInfo;
                    tv . Items . Add ( CreateTreeItem ( driveInfo ) );
                    errors?.Add ( $"Drive [{driveInfo . Name} loaded, Drive type = {driveInfo . DriveType}, Volume={driveInfo . VolumeLabel}" );
                }
                else
                {
                    Console . WriteLine ( $"Drive [{driveInfo . Name} not loaded !, Drive type = {driveInfo . DriveType} - Ignoring !" );
                    errors?.Add ( $"Drive [{driveInfo . Name} not loaded !, Drive type = {driveInfo . DriveType}" );
                }
                var index = tv . Items . Count;
            }
            Texplorer = new ExplorerClass ( );
        }
        public static void TreeViewItem4_Expanded ( object sender , RoutedEventArgs e , TreeViewItem CurrentTreeItem )
        {
            bool Reentrant = false;
            TreeViewItem tv = null;
            TreeViewItem TopItem = null;
            // This is what we are updating in our TreeView
            TreeViewItem item = e?.Source as TreeViewItem;
            if ( item == null && CurrentTreeItem == null )
                return;
            else
            {
                if ( item != null )
                {
                    tv = e . Source as TreeViewItem;
                    CurrentTreeItem = tv;
                    item = CurrentTreeItem;
                }
                else
                {
                    tv = CurrentTreeItem;
                }
            }
            if ( CurrentTreeItem != null )
                item = CurrentTreeItem;

            if ( item != null )
                TopItem = item;

            Texplorer = new ExplorerClass ( );

            string newpath = tv . Header . ToString ( );

            //All seems to be working using the di.FullName path ???
            if ( newpath . Contains ( "\\" ) == false )
            {
                var v = tv . Parent as TreeViewItem;
                if ( v . Header . ToString ( ) . Contains ( "\\" ) == false )
                    newpath = v . Header . ToString ( ) + "\\" + tv . Header . ToString ( );
                else
                    newpath = v . Header . ToString ( ) + tv . Header . ToString ( );
            }
            DirectoryInfo di = new DirectoryInfo ( newpath );
            DriveInfo dinfo = new DriveInfo ( di . FullName );
            List<string> _drives = new List<string> ( );
            var drives = Texplorer . GetDrives ( di . FullName );
            LbStrings . Clear ( );
            if ( ( item . Items . Count == 1 ) && ( item . Items [ 0 ] is string ) )
            {
                Console . WriteLine ($"Loading files for newpath = {newpath}");
                DirectoryInfo NextDirectory = null;
                int folderindex = 0, foldercount = 0; ;
                item . Items . Clear ( );
                try
                {
                    var dirs = new List<DirectoryInfo> ( );
                    dirs = Texplorer . GetDirectories ( );
                    if ( Reentrant == false )
                    {
                        foreach ( var dir in dirs )
                        {
                            if ( dir == null )
                                break;
                            Console . WriteLine ($"Adding dir= {dir}");
                            FileAttributes fa = dir . Attributes;
                            string s = fa . ToString ( );
                            if (
                                ( s . ToUpper ( ) . Contains ( "BOOTMGR" )
                                || s . ToUpper ( ) . Contains ( "BOOTNXT" )
                                || s . ToUpper ( ) . Contains ( "BOOTSECT" ) ) == false )
                            {
                                item . Items . Add ( CreateTreeItem ( dir . Name ) );
                               // Console . WriteLine ($"Adding {dir.Name}");
                                LbStrings . Add ( dir . Name );
                            }
                        }
                    }
                    else
                    {
                        foldercount++;
                        if ( foldercount < dirs . Count )
                        {
                            NextDirectory = dirs [ foldercount ];
                        }
                    }
                }
                catch { }
            }

            // Now use my Files Class to get the list of any lowest level files  & add to treeview 
            // by adding each one to the TreeViewItem collection "item"
            // if they are not Special files or they are marked as Hidden
            List<FileInfo> AllFiles = Texplorer . GetFiles ( );
            foreach ( var str in AllFiles )
            {
                FileAttributes fa = str . Attributes;
                string s = fa . ToString ( );
                if ( s . Contains ( "Hidden" ) == false )
                {
                    if (
                        ( s . ToUpper ( ) . Contains ( "BOOTMGR" )
                        || s . ToUpper ( ) . Contains ( "BOOTNXT" )
                        || s . ToUpper ( ) . Contains ( "BOOTSECT" ) ) == false )
                    {
                        Console . WriteLine ( $"Adding folder {str}" );
                        item . Items . Add ( CreateTreeFile ( str ) );
                        LbStrings . Add ( str . ToString ( ) );
                    }
                }
            }
        }

        private static TreeViewItem CreateTreeItem ( object o )
        {
            string str = o . ToString ( );
            TreeViewItem item = new TreeViewItem ( );
            item . Header = o . ToString ( );
            item . Tag = o;
            // Add  dummy entry so we can expand it
            item . Items . Add ( "Loading..." );
            return item;
        }
        private static TreeViewItem CreateTreeFile ( object o )
        {
            string str = o . ToString ( );
            TreeViewItem item = new TreeViewItem ( );
            item . Header = o . ToString ( );
            item . Tag = o;
            //			item . Items . Add ( o);
            return item;
        }
    }
}

