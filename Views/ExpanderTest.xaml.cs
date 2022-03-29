using System;
using System. Collections. Generic;
using System. Collections. ObjectModel;
using System. Data. SqlClient;
using System. Windows;
using System. Windows. Controls;
using System. Windows. Controls. Primitives;
using System. Windows. Media. Media3D;

using MyDev. Models;
using MyDev. Sql;
using MyDev. SQL;
using MyDev. ViewModels;

namespace MyDev. Views
{
    /// <summary>
    /// Interaction logic for ExpanderTest.xaml
    /// 
    /// /// uses ListItem Class and ApplicationListItem Class, bth in ViewModels folder
    /// </summary>
    public partial class ExpanderTest : Window
    {
        public ObservableCollection<GenericClass> genclass = new ObservableCollection<GenericClass> ( );

        public static Point MousePos { get; set; }
        public List<ListItem> Items { get; } = new List<ListItem> ( );
        List<string> list = new List<string> ( );
        public ExpanderTest ( )
        {
            InitializeComponent ( );
            this. DataContext = this;
            var listItem1 = new ListItem ( "CMM" );
            var appListItem1 = new ApplicationListItem ( "CMM - Local (English)", "Open", "Update", false, false );
            var appListItem2 = new ApplicationListItem ( "CMM - Local (German)", "Open", "Update", false, false );
            var appListItem3 = new ApplicationListItem ( "CMM - Local (Japanese)", "Open", "Update", true );
            listItem1. AddListItem ( appListItem1 );
            listItem1. AddListItem ( appListItem2 );
            listItem1. AddListItem ( appListItem3 );
            Items. Add ( listItem1 );

            var listItem2 = new ListItem ( "Sales Aid" );
            var appListItem4 = new ApplicationListItem ( "Sales Aid - Local (English)", "Open", "Update", true, false );
            listItem2. AddListItem ( appListItem4 );
            Items. Add ( listItem2 );

            ///Splitter.IsFocused

            // Load Generic Db for BankAccount
            string ResultString = "";
            genclass = SqlSupport. LoadGeneric ( "Select * from bankaccount", out ResultString, 0, false );
            SqlServerCommands. LoadActiveRowsOnlyInGrid ( DGrid1, genclass, SqlServerCommands. GetGenericColumnCount ( genclass ) );
            //if ( Flags. ReplaceFldNames )
            ////{
            ////    GenericDbHandlers. ReplaceDataGridFldNames ( "BANKACCOUNT", ref Dgrid1 );
            ////}
            //Dgrid1. Visibility = Visibility. Visible;
        }
        private void CloseBtn_Click ( object sender, RoutedEventArgs e )
        {
            this. Close ( );
        }

        private void Button_Click ( object sender, RoutedEventArgs e )
        {
            this. Close ( );
        }

        private void Splitter_DragStarted ( object sender, System. Windows. Controls. Primitives. DragStartedEventArgs e )
        {
            object obj = e;
            double d = e. HorizontalOffset;
            //panel. Width = e.;
            //Textleft. 
        }

        private void Splitter_DragCompleted ( object sender, System. Windows. Controls. Primitives. DragCompletedEventArgs e )
        {
            // Textleft. Opacity = 0.7;

        }

        private void Splitter_DragDelta ( object sender, System. Windows. Controls. Primitives. DragDeltaEventArgs e )
        {
            // Tracks verticalSplitter and shifts it's "Options ==>" label to follow the splitter
            //leftpanel. UpdateLayout ( );
            //leftpanel. Refresh ( );
            //if ( MousePos.X > this. ActualWidth - 200 )
            //{
            //    double d = Convert.ToDouble(MousePos. X);
            //   // splitcol. Width =d;
            //    e. Handled = true;
            //    return;
            //}
            //    GridSplitter gs = sender as GridSplitter;
            //Thickness th = new Thickness ( 0, 0, 0, 0 );
            ////th = Textright. Margin;
            //if ( this. HorizontalAlignment == HorizontalAlignment. Stretch &&
            //    this. VerticalAlignment == VerticalAlignment. Stretch &&
            //    this. ActualWidth <= this. ActualHeight )
            //{
            //}

           // Splitter. ResizeBehavior = GridResizeBehavior. PreviousAndCurrent;
           //if ( e. HorizontalChange == 1 )
           //{
           //Textright. Visibility = Visibility. Hidden;
           //if ( th. Left < MaxWidth )
           //{
           //    th. Left += e. HorizontalChange;
           //    if ( th. Left > 10 )
           //        Textright. Margin = th;
           //}
           //}
           //else
           //    Textright. Visibility = Visibility. Visible;
           //if ( Textright. Margin. Left > ( this. ActualWidth - Textright. ActualWidth ) - 100 && e. HorizontalChange > 0 )
           //{
           //    //gs. DragIncrement = 2000;
           //    Splitter. HorizontalAlignment = HorizontalAlignment. Left;
           //    e. Handled = false;
           //}
           //else
           //{
           //    Splitter. HorizontalAlignment = HorizontalAlignment. Stretch;
           //    gs. DragIncrement = 1;
           //    //Utils. TrackSplitterPosition ( Textright, this. ActualWidth - Textright.ActualWidth, e );
           //    Thickness th = new Thickness ( 0, 0, 0, 0 );
           //    th = Textright. Margin;
           //    if ( th. Left < MaxWidth )
           //    {
           //        th. Left += e. HorizontalChange;
           //        if ( th. Left > 10 )
           //            Textright. Margin = th;
           //    }


        }

    private void Splitter_MouseLeftButtonDown ( object sender, System. Windows. Input. MouseButtonEventArgs e )
    {
  //          MousePos = e. GetPosition ( Dgrid1 );
        }
        private void Splitter_MouseLeftButtonUp ( object sender, System. Windows. Input. MouseButtonEventArgs e )
    {
        //Splitter. LostFocus ( );
    }

        private void Splitter_MouseMove ( object sender, System. Windows. Input. MouseEventArgs e )
        {
    //       MousePos = e. GetPosition ( Dgrid1 );
            //Console. WriteLine (".......");
        }
    }
}
