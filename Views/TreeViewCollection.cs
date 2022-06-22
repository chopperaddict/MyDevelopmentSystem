

using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . ComponentModel;
using System . IO;
using System . Linq;
using System . Windows . Controls;

public class TreeViewCollection : ObservableCollection<TreeViewItem>
{
    #region OnPropertyChanged
    new public event PropertyChangedEventHandler PropertyChanged;
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

    public static ObservableCollection<TreeViewItem> tvitems = new ObservableCollection<TreeViewItem> ( );
    public static TreeViewItem TvItem = new TreeViewItem ( );
    public static Dictionary<string , string> VolumeLabelsDict = new Dictionary<string , string> ( );
    public static List<string> ValidFiles = new List<string> ( );

    public TreeViewItem deftvitem = new TreeViewItem ( );
    public TreeViewItem temptvitem = new TreeViewItem ( );



    public TreeViewCollection ()
    {
        tvitems = LoadDrives ( );
    }

    #region Full Props

    private bool showAllFiles;
    private bool showVolumeLabels;
    private TreeViewItem selectedItem;
    private TreeViewItem selectionChanged;
    private TreeViewItem currentItem;
    
    public bool ShowAllFiles
    {
        get { return showAllFiles; }
        set { showAllFiles = value; OnPropertyChanged ( "ShowAllFiles" ); }
    }
    public bool ShowVolumeLabels
    {
        get { return showVolumeLabels; }
        set { showVolumeLabels = value; OnPropertyChanged ( "ShowVolumeLabels" ); }
    }
    public TreeViewItem SelectedItem
    {
        get { return selectedItem; }
        set { selectedItem = value; currentItem = value; OnPropertyChanged ( "SelectedItem" ); }
    }
    public TreeViewItem SelectionChanged
    {
        get { return selectionChanged; }
        set { selectionChanged = value; currentItem = value; OnPropertyChanged ( "SelectionChanged" ); }
    }
    public TreeViewItem CurrentItem
    {
        get { return currentItem; }
        set { currentItem = value; OnPropertyChanged ( "CurrentItem" ); }
    }
    
    #endregion Full Props


    #region Utility Support Methods
    public  TreeViewItem FindEntry ( TreeViewItem tvitem )
    {
        TreeViewItem currentitem = null;
        foreach ( TreeViewItem item in tvitems )
        {
            if ( item == tvitem )
            {
                currentitem = item;
                break;
            }
        }
        return currentitem;
    }
    public TreeViewItem GetExpanded ( TreeViewItem tvitem  )
    {
        temptvitem = FindEntry ( tvitem );
        if ( temptvitem . IsExpanded )
            return temptvitem;
        else 
            return null;
    }
    public TreeViewItem GetCurrent( )
    {
        return CurrentItem;
    }
    public TreeViewItem GetSelected ( )
    {
        return SelectedItem;
    }
    public TreeViewItem SetCurrent( TreeViewItem tvitem  )
    {
        temptvitem = FindEntry ( tvitem );
        if ( temptvitem != null )
        {
            CurrentItem = tvitem;
            return temptvitem;
        }
        return null;
    }
    public TreeViewItem SetSelected ( TreeViewItem tvitem , bool selectitem = true )
    {
        temptvitem = FindEntry ( tvitem );
        if ( temptvitem != null )
        {
            if ( selectitem )
                SelectedItem = tvitem;
            else
                SelectedItem = null;
            return SelectedItem;
        }
        return null;
    }
    public TreeViewItem SetExpanded ( TreeViewItem tvitem , bool selectitem = true )
    {
        temptvitem = FindEntry ( tvitem );
        if ( temptvitem != null )
        {
            temptvitem . IsExpanded = selectitem;
            return temptvitem;
        }
        return null;
    }
     public ObservableCollection<TreeViewItem> LoadDrives ( string drivetoload = "" )
    {
        bool ValidDrive = false;
        //            bool HasHiddenItems = false;
        string volabel = "";
        string DriveHeader = "";
        string Padding = "                 ";
        bool isvalid = false;
        tvitems . Clear ( );
        //            listBox . Items . Clear ( );
        VolumeLabelsDict . Clear ( );
        LoadValidFiles ( );
        if ( drivetoload == "ALL" )
            drivetoload = "";
        foreach ( var drive in Directory . GetLogicalDrives ( ) )
        {
            ValidDrive = false;
            DriveHeader = "";
            if ( drivetoload . ToUpper ( ) != "" )
            {
                if ( drive . ToUpper ( ) != drivetoload . ToUpper ( ) )
                    continue;
            }
            //Add Drive to Treeview
            DriveInfo [ ] di = DriveInfo . GetDrives ( );
            foreach ( var item in di )
            {
                if ( item . Name == drive )
                {
                    if ( item . DriveType == DriveType . CDRom )
                    {
                        ValidDrive = false;
                        DriveHeader = Padding . Substring ( 0 , 10 );
                        DriveHeader += "CdRom Drive";
                        string newlabel = " " + DriveHeader;
                        VolumeLabelsDict . Add ( drive , newlabel );
                    }
                    else
                    {
                        List<string> directories = new List<string> ( );
                        GetDirectories ( item . ToString ( ) , out directories );
                        foreach ( var dir in directories )
                        {
                            bool HasHidden = false;
                            if ( CheckIsVisible ( dir . ToUpper ( ) , ShowAllFiles , out HasHidden ) == true )
                            {
                                isvalid = true;
                                string newlabel = " " + item . VolumeLabel;
                                VolumeLabelsDict . Add ( drive , newlabel );
                                if ( ShowVolumeLabels == true )
                                {
                                    DriveHeader = $"    [{newlabel}]";
                                }
                                break;
                            }
                        }
                        if ( isvalid )
                        {
                            if ( ShowVolumeLabels == true )
                                DriveHeader = $"   [{item . VolumeLabel}]";
                            ValidDrive = true;
                        }
                        else
                            volabel = $"    [{item . VolumeLabel}]";
                    }
                    break;
                }
            }
            if ( ValidDrive == true )
            {
                var item = new TreeViewItem ( );
                item . Header = drive + DriveHeader;
                item . Tag = drive;
                // Add Dummy entry so we get an "Can be Opened" triangle icon
                item . Items . Add ( "Loading" );
                //                DrivesCombo . Items . Add ( drive . ToString ( ) );
                tvitems . Add ( item );
            }
            else
            {
                var item = new TreeViewItem ( );
                if ( ShowVolumeLabels == true )
                    item . Header = drive + volabel;
                else
                    item . Header = drive + DriveHeader;
                item . Tag = drive;
                //DrivesCombo . Items . Add ( drive . ToString ( ) );
                tvitems . Add ( item );
            }
        }
        //DrivesCombo . Items . Add ( "ALL" );
        //DrivesCombo . SelectedIndex = 0;
        //DrivesCombo . SelectedItem = 0;
        return tvitems;
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
                bool HasHidden= false;
                foreach ( var item in directs )
                {
                    try
                    {
                        if ( filterSysfiles )
                        {
                            if ( IsSystemFile ( item . ToUpper ( ) ) == true )
                            {
                                continue;
                            }
                        }
                        if ( CheckIsVisible ( item . ToUpper ( ) , ShowAllFiles , out HasHidden ) == true )
                        {
                            directories . Add ( item );
                        }
                        count++;
                    }
                    catch ( Exception ex ) { Console . WriteLine ( $"GetDirectories : 980 : {ex . Message}" ); }
                }
            }
        }
        catch ( Exception ex )
        {
            { Console . WriteLine ( $"GetDirectories : 981 : {ex . Message}" ); }
        }
        dirs = directories;
        return count;
    }
    public int GetDirectoryCount ( string path )
    {
        int count = 0;
        List<string> directories = new List<string> ( );
        try
        {
            string [ ] directs = Directory . GetDirectories ( path );
            foreach ( var item in directs )
            {
                bool HasHidden = false;
                if ( CheckIsVisible ( item . ToUpper ( ) , ShowAllFiles , out HasHidden ) == true )
                {
                    count++;
                }
                //count = directs . Length;
            }
        }
        catch ( Exception ex )
        {
            { Console . WriteLine ( $"GetDirectoryCount : 9968 : {ex . Message}" ); }
        }
        return count;
    }
    public int GetFiles ( string path , out List<string> allfiles )
    {
        int count = 0;
        var files = new List<string> ( );
        allfiles = new List<string> ( );
        // Get a list of all items in the current folder
        try
        {
            if ( GetFilesCount ( path ) <= 0 )
                return 0;
            //var file = Directory . EnumerateFiles ( path , "*.*" );
            var filecount = Directory . GetFiles ( path , "*.*" , SearchOption . TopDirectoryOnly );
            if ( filecount . Count ( ) > 0 )
            {
                foreach ( var item in filecount )
                {
                    bool HasHidden = false;

                    if ( CheckIsVisible ( item . ToUpper ( ) , ShowAllFiles , out HasHidden ) == true )
                    {
                        files . Add ( item );
                        count++;
                        allfiles . Add ( item );
                        // working correctly
                        //                        UpdateListBox ( item );
                    }

                }
            }
        }
        catch ( Exception ex )
        {
            Console . WriteLine ( $"GetFiles : 1052 : {ex . Message}" );
        }
        return count;
    }
    public int GetFilesCount ( string path )
    {
        int count = 0;
        bool result = true;
        var files = new List<string> ( );
        // Get a list of all items in the current folder
        try
        {
            var dirfile = Directory . GetFiles ( path,"*.*",SearchOption.TopDirectoryOnly) ;
            count = ( int ) dirfile . Length;
            if(count > 0 )
            {

            }
            //          ShowProgress ( );
        }
        catch ( Exception ex )
        {
            Console . WriteLine ( $"GetFilesCount : 1081 : {ex . Message}" );
            result = false;
        }
        if ( result == false )
            return -1;

        return count;
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
    public static bool CheckIsVisible ( string entry , bool showall , out bool HasHidden )
    {
        HasHidden = false;
        entry = entry . ToUpper ( );
        if ( showall == false )
        {
            foreach ( var item in ValidFiles )
            {
                if ( entry . Contains ( item . ToUpper ( ) ) )
                {
                    HasHidden = true;
                    return false;
                }
            }
            return true;
        }
        return true;
    }

    #endregion Utility Support Methods

}
