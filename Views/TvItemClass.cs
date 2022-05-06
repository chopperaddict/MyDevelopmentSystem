
#define DEBUGEXPAND
#undef DEBUGEXPAND

using System;
using System . Collections;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . ComponentModel;
using System . IO;
using System . Linq;
using System . Windows;
using System . Windows . Controls;

public class TvItemClass : TreeViewItem
{
    public static TvItemClass tvitemclass { get; set; }
    public  TreeViewCollection TvCollection = new TreeViewCollection ( );
    public TreeViewItem deftvitem = new TreeViewItem ( );
    public TreeViewItem temptvitem = new TreeViewItem ( );
    public static ObservableCollection<TreeViewItem> tvcollectionitems { get; set; }
    //public static Dictionary<string , string> VolumeLabelsDict = new Dictionary<string , string> ( );
    //public static List<string> ValidFiles = new List<string> ( );

    public TvItemClass ( )
    {
        tvcollectionitems = TreeViewCollection . tvitems;
        //tvcollectionitems = LoadDrives ( );
        deftvitem = new TreeViewItem ( );
        TreeViewItem tempitem = new TreeViewItem ( );
        deftvitem . Header = @"C:\\";
        deftvitem . Tag = @"C:\\";
        tempitem = TvCollection.FindEntry ( deftvitem );
        TvCollection . CurrentItem = tempitem;
        TvCollection .SelectedItem = tempitem;
        tvitemclass = this;
        //TvCollection.
    }
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

    #region Full Properties
    private bool isExpanded;
    private int expandLevel;
    private bool showVolumeLabels;
    private bool showAllFiles;
 
    public new bool IsExpanded
    {
        get { return isExpanded; }
        set { isExpanded = value; OnPropertyChanged ( "IsExpanded" ); }
    }
    public bool ShowVolumeLabels
    {
        get { return showVolumeLabels; }
        set { showVolumeLabels = value; OnPropertyChanged ( "ShowVolumeLabels" ); }
    }
    public bool ShowAllFiles
    {
        get { return showAllFiles; }
        set { showAllFiles = value; OnPropertyChanged ( "ShowAllFiles" ); }
    }
    public int ExpandLevel
    {
        get { return expandLevel; }
        set { expandLevel = value; OnPropertyChanged ( "ExpandLevel" ); }
    }
    #endregion Full Properties
    //public ObservableCollection<TreeViewItem> LoadDrives ( string drivetoload = "" )
    //{
    //    bool ValidDrive = false;
    //    //            bool HasHiddenItems = false;
    //    string volabel = "";
    //    string DriveHeader = "";
    //    string Padding = "                 ";
    //    bool isvalid = false;
    //    tvcollectionitems . Clear ( );
    //    //            listBox . Items . Clear ( );
    //    VolumeLabelsDict . Clear ( );
    //    LoadValidFiles ( );
    //    if ( drivetoload == "ALL" )
    //        drivetoload = "";
    //    foreach ( var drive in Directory . GetLogicalDrives ( ) )
    //    {
    //        ValidDrive = false;
    //        DriveHeader = "";
    //        if ( drivetoload . ToUpper ( ) != "" )
    //        {
    //            if ( drive . ToUpper ( ) != drivetoload . ToUpper ( ) )
    //                continue;
    //        }
    //        //Add Drive to Treeview
    //        DriveInfo [ ] di = DriveInfo . GetDrives ( );
    //        foreach ( var item in di )
    //        {
    //            if ( item . Name == drive )
    //            {
    //                if ( item . DriveType == DriveType . CDRom )
    //                {
    //                    ValidDrive = false;
    //                    DriveHeader = Padding . Substring ( 0 , 10 );
    //                    DriveHeader += "CdRom Drive";
    //                    string newlabel = " " + DriveHeader;
    //                    VolumeLabelsDict . Add ( drive , newlabel );
    //                }
    //                else
    //                {
    //                    List<string> directories = new List<string> ( );
    //                    GetDirectories ( item . ToString ( ) , out directories );
    //                    foreach ( var dir in directories )
    //                    {
    //                        bool HasHidden = false;
    //                        if ( CheckIsVisible ( dir . ToUpper ( ) , ShowAllFiles , out HasHidden ) == true )
    //                        {
    //                            isvalid = true;
    //                            string newlabel = " " + item . VolumeLabel;
    //                            VolumeLabelsDict . Add ( drive , newlabel );
    //                            if ( ShowVolumeLabels == true )
    //                            {
    //                                DriveHeader = $"    [{newlabel}]";
    //                            }
    //                            break;
    //                        }
    //                    }
    //                    if ( isvalid )
    //                    {
    //                        if ( ShowVolumeLabels == true )
    //                            DriveHeader = $"   [{item . VolumeLabel}]";
    //                        ValidDrive = true;
    //                    }
    //                    else
    //                        volabel = $"    [{item . VolumeLabel}]";
    //                }
    //                break;
    //            }
    //        }
    //        if ( ValidDrive == true )
    //        {
    //            var item = new TreeViewItem ( );
    //            item . Header = drive + DriveHeader;
    //            item . Tag = drive;
    //            // Add Dummy entry so we get an "Can be Opened" triangle icon
    //            item . Items . Add ( "Loading" );
    //            //                DrivesCombo . Items . Add ( drive . ToString ( ) );
    //            tvcollectionitems . Add ( item );
    //        }
    //        else
    //        {
    //            var item = new TreeViewItem ( );
    //            if ( ShowVolumeLabels == true )
    //                item . Header = drive + volabel;
    //            else
    //                item . Header = drive + DriveHeader;
    //            item . Tag = drive;
    //            //DrivesCombo . Items . Add ( drive . ToString ( ) );
    //            tvcollectionitems . Add ( item );
    //        }
    //    }
    //    //DrivesCombo . Items . Add ( "ALL" );
    //    //DrivesCombo . SelectedIndex = 0;
    //    //DrivesCombo . SelectedItem = 0;
    //    return tvcollectionitems;
    //}
    public void ExpandTreeViewItem ( TreeViewItem tvitem , TreeViewItem Mouseovertvitem )
    {
        if ( tvitem == null )
        {
            //if ( Mouseovertvitem != null )
            //    tvitem = Mouseovertvitem;
            //else
                return;
        }
        // Set ObsCollection item as current item
        tvitem.IsSelected = true ;
        if(tvitem == null )
            return;
         //Selection . Text = $"{item . Tag . ToString ( )}";
        //ScrollCurrentTvItemIntoView ( item );
        //ActiveTree . Refresh ( );

        var directories = new List<string> ( );
        var Allfiles = new List<string> ( );
        string Fullpath = tvitem . Tag . ToString ( ) . ToUpper ( );
        int DirectoryCount = 0, filescount = 0;
        int itemscount = tvitem . Items . Count;
         var tvi = tvitem as TreeViewItem;
        TreeViewItem Caller = new TreeViewItem ( );
        //        Caller . Header = currentHeader;
        //var itemheader = tvitem . Items [ 0 ] . ToString ( );
        //  UpdateListBox ( $"{item . Tag . ToString ( )}" );
        // Get a list of all items in the current folder
        int dircount = TvCollection.GetDirectoryCount ( Fullpath );
        if ( dircount > 0 )
        {
            int count = TvCollection . GetDirectories ( Fullpath , out directories );
            if ( count > 250 )
            {
                MessageBoxResult result = System . Windows . MessageBox . Show ( $"Directory {Fullpath} contains {count} Files\nExpanding these will take a considerable time...\n\nAre you sure you want to continue ?" ,
                 "Potential long delay" , MessageBoxButton . YesNoCancel , MessageBoxImage . Warning , MessageBoxResult . Cancel );
                if ( result == MessageBoxResult . Yes )
                {
                    // Remove DUMMY entry
                    //if ( itemheader != null && itemheader == "Loading" )
                    //    tvitem . Items . Clear ( );
                    DirectoryCount = count;
                    //ShowProgress ( );
                    DirectoryCount = AddDirectoriesToTestTreeview ( directories , tvitem , null );
                }
                else if ( result == MessageBoxResult . Cancel )
                {
                    AbortExpand = true;
                    {
                        //Caller . Header = currentHeader;
                        //iterations = 0;
                        //BusyLabel . Text = "";
                        return;
                    }
                }
                else
                {
                    ExpandLimited = true;
                    {
                        //Caller . Header = currentHeader;
                        //iterations = 0;
                        //BusyLabel . Text = "";
                        return;
                    }
                }
            }
            else
            {
                DirectoryCount = count;
                //ShowProgress ( );
                if ( directories . Count > 0 )
                {
                    if ( tvitem . Items . Count > 0 && tvitem . Items [ 0 ] . ToString ( ) == "Loading" )
                    {
                        tvitem . Items . Clear ( );
                    }
                     if ( itemscount >= 1 )
                    {
                        tvitem . IsExpanded = true;
                    }
                    else
                    {
                        //iterations++;
                        tvitem . IsExpanded = true;
                        tvitem . IsSelected = true;
                        //TvCollection . SetSelected ( tvitem );
                        //TvCollection . SetExpanded ( tvitem );
                        //Selected . IsExpanded = true;
                        //ScrollCurrentTvItemIntoView ( tvitem );
                        //ActiveTree . Refresh ( );
                        DirectoryCount = AddDirectoriesToTestTree ( directories , tvitem , null );
                    }
                }
            }
        }
        else
        {
            DirectoryCount = 0;
            //ShowProgress ( );
        }
        // Now Get FILES

        if ( itemscount < DirectoryCount &&  TvCollection . GetFilesCount ( Fullpath ) > 0 )
        {
            TvCollection.GetFiles ( Fullpath , out Allfiles );
            filescount = Allfiles . Count;
            if ( filescount > 500 )
            {
                MessageBoxResult result = MessageBox . Show ( $"Directory {Fullpath} contains {filescount} Files\nExpanding these will take a considerable time...\n\nAre you sure you want to expand  thiis  subdirectory?\n\n(Cancel to stop the entire Expansion immediately)" ,
                 "Potential long delay" , MessageBoxButton . YesNoCancel , MessageBoxImage . Warning , MessageBoxResult . Cancel );
                if ( result == MessageBoxResult . Yes )
                {
                    if ( tvitem . Items . Count > 0 && tvitem . Items [ 0 ] . ToString ( ) == "Loading" )
                    {
                        tvitem . Items . Clear ( );
                    }
                    AddFilesToTreeview ( Allfiles , tvitem );
                }
                else if ( result == MessageBoxResult . Cancel )
                {
                    AbortExpand = true;
                    {
                        //Caller . Header = currentHeader;
                        //iterations = 0;
                        //BusyLabel . Text = "";
                        return;
                    }
                }
                else
                {
                    ExpandLimited = true;
                    {
                        //Caller . Header = currentHeader;
                        //iterations = 0;
                        //BusyLabel . Text = "";
                        return;
                    }
                }
            }
            else
            {
                if ( tvitem . Items .Count > 0 && tvitem . Items [ 0 ] . ToString ( ) == "Loading" )
                {
                    tvitem . Items . Clear ( );
                }
                AddFilesToTreeview ( Allfiles , tvitem );
                //ScrollCurrentTvItemIntoView ( tvitem );
                //ActiveTree . Refresh ( );
            }
        }

        //Handle No files, no dirs scentio
        if ( DirectoryCount == 0 && Allfiles . Count == 0 )
        {
            if ( tvitem . Items .Count > 0 && tvitem . Items [ 0 ] . ToString ( ) == "Loading" )
            {
                tvitem . Items . Clear ( );
                tvitem . IsExpanded = false;
                //                TvCollection . SetExpanded ( tvitem , false );
                tvitem . IsSelected= true;
//                TvCollection.SetSelected ( tvitem , true );
                //tvitem . IsSelected = true;
                //CurrentItem= tvitem;
                //                ActiveTree . Refresh ( );
                //                ShowProgress ( );
            }
            //if ( ExceptionMessage != "" )
            //{
            //    Selection . Text = ExceptionMessage;
            //    ExceptionMessage = "";
            //}
            //else
            //{
            //    Selection . Text = "This Subdirectory does not contain any Non System or Hidden files, or perhaps Access is denied by Windows ...";
            //}
        }
        else
        {
            tvitem . IsSelected = true;
//            tvitem . IsExpanded = true;
            //ActiveTree . UpdateLayout ( );
            //ActiveTree . Refresh ( );
            //Selection . Text = $"{tvitem .Header . ToString ( )} SubDirectories = {DirectoryCount} , Files = {Allfiles . Count}";
        }
    }
    public int AddDirectoriesToTestTree ( List<string> directories , TreeViewItem item , ListBox lBox = null , bool UseExpand = true )
    {
        int added = 0;
        int TotalDirs = 0;
        int TotalFiles = 0;
        item . Items . Clear ( );

//        TreeViewItem ListItem = TvCollection . FindEntry ( item );
        TreeViewItem ListItem = item;
        if ( ListItem == null )
            return -1;
        else
        {
//            TvCollection . SetExpanded ( ListItem , true );
            item.IsExpanded =true ;
        }
        foreach ( var directoryPath in directories )
        {
            //directories . ForEach ( directoryPath =>
            //{
            var subitem = new TreeViewItem ( );
            subitem . Header = GetFileFolderName ( directoryPath );
            subitem . Tag = directoryPath;
            //           UpdateListBox ( directoryPath . ToUpper ( ) );
            bool HasHidden = false;
            if ( TreeViewCollection . CheckIsVisible ( directoryPath . ToUpper ( ) , ShowAllFiles , out HasHidden ) == true )
            {     // add the dummy entry to each of the subdirectories we are adding to the tree so we get the Expand icons
                TotalDirs = TvCollection . GetDirectoryCount ( directoryPath );
                TotalFiles = TvCollection . GetFilesCount ( directoryPath );
                if ( TotalFiles == -1 )
                    TotalFiles = 0;
                // Add  item to collection
                ListItem . Items. Add ( subitem );
                // Add DUMMY entry as we have content in this folder
                var dummy = new TreeViewItem ( );
                dummy . Header = "Loading";
                subitem . Items . Add ( dummy );
//                subitem . IsExpanded = false;
                subitem . IsSelected = true;
//                TvCollection . SetExpanded (subitem,false);
//                TvCollection . SetSelected ( ListItem , true );
                ListItem . IsSelected = true;
  //              TvCollection . SetCurrent( subitem);
                Console . WriteLine ( $"ADDDIRECTORIESTOTESTTREEVIEW : {subitem . Header}  / {subitem . Tag}" );
                // This works well in stopping empty Folders form having an Open Icon
                // But may not be the best idea ?
                //if ( TotalDirs > 0 || TotalFiles > 0 )
                //{
                //    // Add DUMMY entry as we have content in this folder
                //    dummy . Header = "Loading";
                //    subitem . Items . Add ( dummy );
                //    //Selected . Items . Add ( dummy );
                //    //                  ScrollCurrentTvItemIntoView ( subitem );
                //    //if(subitem.Header == "BOOT")
                //    //    Console . WriteLine ();
                //    //                    if ( FullExpandinProgress == false )
                //    //                        ActiveTree . Refresh ( );
                //}
                //                    Console . WriteLine ( $"3 - ADTT : Added Subdir {subitem . Tag . ToString ( )} to expanded {item . Tag . ToString ( )}" );
                added++;
            }
            //           ShowProgress ( );
            //});
        }
        return added;
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
    public int AddDirectoriesToTestTreeview ( List<string> directories , TreeViewItem item , ListBox lBox = null , bool UseExpand = true )
    {
        int added = 0;
        if ( directories . Count == 0 )
            return 0;
        item . Items . Clear ( );
        foreach ( var dir in directories )
        {
            var subitem = new TreeViewItem ( );

            //ShowProgress ( );
            bool HasHidden = false;
            if ( TreeViewCollection . CheckIsVisible ( dir . ToUpper ( ) , ShowAllFiles , out HasHidden ) == true )
            {
                try
                {
                    subitem . Header = GetFileFolderName ( dir );
                    subitem . Tag = dir;
                    item . Items . Add ( subitem );
                    item . IsExpanded = true;
                    item . IsSelected = true;
                    TvCollection . SetExpanded ( item);
                    TvCollection . SetSelected ( item ); ;
                    TvCollection . SetCurrent ( item);

                    subitem . BringIntoView ( );
                    //ActiveTree . HorizontalAlignment = HorizontalAlignment . Left;
                    //ScrollTvItemIntoView ( subitem );
                    //ScrollCurrentTvItemIntoView ( subitem );
                    //ActiveTree . Refresh ( );
                    //UpdateListBox ( subitem . Tag . ToString ( ) );

                    int count = TvCollection. GetDirectories ( dir , out directories );
                    if ( count > 0 )
                    {
                        var tv = new TreeViewItem ( );
                        tv . Header = "Loading";
                        //                      tv . Tag = "Loading";
                        subitem . Items . Add ( tv );
                        subitem . IsSelected = true;
                        TvCollection.SetCurrent ( subitem ); 
                        //ScrollCurrentTvItemIntoView ( subitem );
                        //ScrollTvItemIntoView ( subitem );

                        AddDirectoriesToTestTreeview ( directories , subitem , null );
                    }
                    else
                    {
                        var tv = new TreeViewItem ( );
                        tv = item;
                        if ( tv . Items[0] . ToString ( ) == "Loading" )
                            item . Items . Clear ( );
                    }
                }
                //}
                catch ( Exception ex )
                {
                    Console . WriteLine ( $"AddDirectoriesoTestTreeView : 903 ; Invalid  directory accessed {ex . Message}" );
                }
            }
            //ShowProgress ( );
            added++;
        }
        return added;
    }
    public int AddFilesToTreeview ( List<string> Allfiles , TreeViewItem item )
    {
        int count = 0;
        if ( item . Items . Count == 1 )
        {
            try
            {
                var tmp = item . Items[0] as TreeViewItem;
                if ( tmp.Header.ToString().Contains("Loading") )
                    item. Items . Clear ( );
            }
            catch ( Exception ex )
            {
                Console . WriteLine ( $"{ex . Message}" );
            }
        }
        //     TreeViewItem ListEntry = FindEntry ( item );
        item . IsSelected = true;
//        CurrentItem = item;

        //            item . IsExpanded = true;
        foreach ( var itm in Allfiles )
        {
            bool HasHidden = false;
            //ShowProgress ( );
            if ( TreeViewCollection . CheckIsVisible ( itm . ToUpper ( ) , ShowAllFiles , out HasHidden ) == true )
            {
                var subitem = new TreeViewItem ( );
                subitem . Header = GetFileFolderName ( itm );
                subitem . Tag = itm;
//                subitem . IsExpanded = false;
                subitem . IsSelected = true;
//                CurrentItem = subitem;
                subitem . Items . Clear ( );
                item. Items . Add ( subitem );
                item . IsSelected = true;
//                item . IsExpanded = false;
                //try
                //{
                //    string tmp = Selected . Items [ 0 ] . ToString ( );
                //    if ( tmp == "Loading" )
                //        Selected . Items . Clear ( );
                //}
                //catch ( Exception ex ) { Console . WriteLine ($"{ex.Message}"); }
                // item . Items . Add ( subitem );
                //tvitems.Add ( subitem );
                //ScrollTvItemIntoView ( subitem );
                //ActiveTree . Refresh ( );
                count++;
            }
            //ShowProgress ( );
            //if ( FullExpandinProgress == false )
            //    ActiveTree . Refresh ( );
        }
        return count;
    }
}