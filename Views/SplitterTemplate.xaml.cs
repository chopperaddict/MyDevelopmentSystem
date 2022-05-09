using System;
using System . Collections . Generic;
using System . ComponentModel;
using System . Data . SqlClient;
using System . Data;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Controls . Primitives;
using System . Windows . Data;
using System . Windows . Documents;
using System . Windows . Input;
using System . Windows . Media;
using System . Windows . Media . Imaging;
using System . Windows . Shapes;

using MyDev . Models;
using MyDev . UserControls;
using MyDev . ViewModels;
using MyDev . Dapper;
using MyDev . SQL;
using System . Collections . ObjectModel;
using System . Xml . Linq;
using static System . Windows . Forms . VisualStyles . VisualStyleElement . ProgressBar;
using MyDev . Sql;
using static System . Windows . Forms . LinkLabel;
using System . Windows . Media . Animation;


namespace MyDev . Views
{
    /// <summary>
    /// Interaction logic for SplitterTemplate.xaml
    /// </summary>
    public partial class SplitterTemplate : Window
    {
        public BankAccountViewModel bvm = new BankAccountViewModel ( );
        public CustomerViewModel cvm = new CustomerViewModel ( );
        public DetailsViewModel dvm = new DetailsViewModel ( );
        public GenericClass gvm = new GenericClass ( );

        // Collections
        public ObservableCollection<BankAccountViewModel> bankaccts = new ObservableCollection<BankAccountViewModel> ( );
        public ObservableCollection<CustomerViewModel> custaccts = new ObservableCollection<CustomerViewModel> ( );
        public ObservableCollection<DetailsViewModel> detaccts = new ObservableCollection<DetailsViewModel> ( );
        public ObservableCollection<GenericClass> genaccts = new ObservableCollection<GenericClass> ( );

        #region All Template setup stuff

        #region Variables used for splitters
        private double MaxColWidth1 { get; set; }
        private double MinRowHeight1 { get; set; }
        private double MaxRowHeight { get; set; }
        #endregion Variables used for splitters

        private string SqlCommand = "";
        private string DefaultSqlCommand = "Select * from BankAccount";
//        private bool UseBGThread = false;
        private bool LoadDirect = true;

        #region Binding full props
        // Full properties used in Binding to I/f objects

        private bool ismouseDown;
        public bool isMouseDown
        {
            get { return ismouseDown; }
            set { ismouseDown = value; }
        }
        private object movingobject;
        public object MovingObject
        {
            get { return movingobject; }
            set { movingobject = value; }
        }

        private double FirstXPos = 0;
        private double FirstYPos = 0;

        #endregion Binding full props

        #region Flowdoc file wide variables
        public FlowdocLib fdl = new FlowdocLib ( );
        private double XLeft = 0;
        private double YTop = 0;
        #endregion Flowdoc file wide variables

        #endregion All Template setup stuff

        #region Startup
        public SplitterTemplate ( )
        {
            InitializeComponent ( );
            // MOST important line !!!!
            this . DataContext = this;
            MaxColWidth1 = 340;
            MinRowHeight1 = 255;
            MaxRowHeight = 275;
        }
        private void Window_Loaded ( object sender , RoutedEventArgs e )
        {
            // Startup settings
            #region Template settings
            #region  Handling for splitters
            imgup = new BitmapImage ( new Uri ( @"\icons\down arroiw red.png" , UriKind . Relative ) );
            vimgmove = new BitmapImage ( new Uri ( @"\icons\Lrg ltrt arrow red copy.png" , UriKind . Relative ) );
            LhHsplitter = new BitmapImage ( new Uri ( @"\icons\down arroiw red.png" , UriKind . Relative ) );
            //            lsplitrow1 . Height = (GridLength)1;
            // This sets the relative height of t Grid'Left hand side row heights - works  too
            //LeftPanelgrid . RowDefinitions [ 0 ] . Height = new GridLength ( 20 , GridUnitType . Star );
            //LeftPanelgrid . RowDefinitions [ 1 ] . Height = new GridLength ( 20 , GridUnitType . Pixel );
            //LeftPanelgrid . RowDefinitions [ 2 ] . Height = new GridLength ( 20 , GridUnitType . Star );

            //Maingrid . RowDefinitions [ 0 ] . Height = new GridLength ( 0.00 , GridUnitType . Pixel );
            //Maingrid . RowDefinitions [ 1 ] . Height = new GridLength ( 0 , GridUnitType . Star );
            //Maingrid . RowDefinitions [ 2 ] . Height = new GridLength ( 20 , GridUnitType . Pixel );
            //Maingrid . RowDefinitions [ 3 ] . Height = new GridLength ( 300 , GridUnitType . Star );
            //Maingrid . RowDefinitions [ 4 ] . Height = new GridLength ( 75 , GridUnitType . Pixel );

            //RightPanelGrid . RowDefinitions [ 0 ] . Height = new GridLength ( 20 , GridUnitType . Star );
            //RightPanelGrid . RowDefinitions [ 1 ] . Height = new GridLength ( 20 , GridUnitType . Pixel );
            //RightPanelGrid . RowDefinitions [ 2 ] . Height = new GridLength ( 20 , GridUnitType . Star );
            #endregion  Handling for splitters
            #endregion Template settings
            #region Template settings
            Flowdoc . ExecuteFlowDocMaxmizeMethod += new EventHandler ( MaximizeFlowDoc );
            ShowdragText = "Drag Down here to ";
            ShowText = "Show more records";
            this . SetValue ( MaxSplitterWidthProperty , this . ActualWidth );
            this . SetValue ( MaxSplitterHeightProperty , this . ActualHeight );
            #endregion Template   settings

             }
        #endregion Startup

  
        #region DP.s
        public double LeftTopSplitterOffset
        {
            get { return ( double ) GetValue ( LeftTopSplitterOffsetProperty ); }
            set { SetValue ( LeftTopSplitterOffsetProperty , value ); }
        }
        public static readonly DependencyProperty LeftTopSplitterOffsetProperty =
            DependencyProperty . Register ( "LeftTopSplitterOffset" , typeof ( double ) , typeof ( SplitViewer ) , new PropertyMetadata ( ( double ) 0 ) );
        public string LeftSplitterText
        {
            get { return ( string ) GetValue ( LeftSplitterTextProperty ); }
            set { SetValue ( LeftSplitterTextProperty , value ); }
        }
        public static readonly DependencyProperty LeftSplitterTextProperty =
           DependencyProperty . Register ( "LeftSplitterText" , typeof ( string ) , typeof ( SplitViewer ) , new PropertyMetadata ( "Drag Down  " ) );
        public string ShowText
        {
            get { return ( string ) GetValue ( ShowTextProperty ); }
            set { SetValue ( ShowTextProperty , value ); }
        }
        public static readonly DependencyProperty ShowTextProperty =
            DependencyProperty . Register ( "ShowText" , typeof ( string ) , typeof ( SplitViewer ) , new PropertyMetadata ( "Show More Records" ) );
        public string ShowdragText
        {
            get { return ( string ) GetValue ( ShowdragTextProperty ); }
            set { SetValue ( ShowdragTextProperty , value ); }
        }
        public static readonly DependencyProperty ShowdragTextProperty =
            DependencyProperty . Register ( "ShowdragText" , typeof ( string ) , typeof ( SplitViewer ) , new PropertyMetadata ( "Drag Up/Down to  " ) );
        public BitmapImage imgup
        {
            get { return ( BitmapImage ) GetValue ( imgupProperty ); }
            set { SetValue ( imgupProperty , value ); }
        }
        public static readonly DependencyProperty imgupProperty =
            DependencyProperty . Register ( "imgup" , typeof ( BitmapImage ) ,
    typeof ( SplitViewer ) ,
                 new PropertyMetadata ( null ) );
        public BitmapImage imgdn
        {
            get { return ( BitmapImage ) GetValue ( imgdnProperty ); }
            set { SetValue ( imgdnProperty , value ); }
        }
        public static readonly DependencyProperty imgdnProperty =
            DependencyProperty . Register ( "imgdn" ,
                 typeof ( BitmapImage ) ,
                   typeof ( SplitViewer ) ,
                new PropertyMetadata ( null ) );
        public BitmapImage imgmv
        {
            get { return ( BitmapImage ) GetValue ( imgmvProperty ); }
            set { SetValue ( imgmvProperty , value ); }
        }
        public static readonly DependencyProperty imgmvProperty =
             DependencyProperty . Register ( "imgmv" ,
                  typeof ( BitmapImage ) ,
                    typeof ( SplitViewer ) ,
                 new PropertyMetadata ( null ) );
        public BitmapImage vimgleft
        {
            get { return ( BitmapImage ) GetValue ( vimgleftProperty ); }
            set { SetValue ( vimgleftProperty , value ); }
        }
        public static readonly DependencyProperty vimgleftProperty =
            DependencyProperty . Register ( "vimgleft" , typeof ( BitmapImage ) , typeof ( SplitViewer ) , new PropertyMetadata ( null ) );
        public BitmapImage vimgright
        {
            get { return ( BitmapImage ) GetValue ( vimgrightProperty ); }
            set { SetValue ( vimgrightProperty , value ); }
        }
        public static readonly DependencyProperty vimgrightProperty =
           DependencyProperty . Register ( "vimgright" , typeof ( BitmapImage ) , typeof ( SplitViewer ) , new PropertyMetadata ( null ) );
        public BitmapImage vimgmove
        {
            get { return ( BitmapImage ) GetValue ( vimgmoveProperty ); }
            set { SetValue ( vimgmoveProperty , value ); }
        }
        public static readonly DependencyProperty vimgmoveProperty =
            DependencyProperty . Register ( "vimgmove" , typeof ( BitmapImage ) , typeof ( SplitViewer ) , new PropertyMetadata ( null ) );
        public int DbCount
        {
            get { return ( int ) GetValue ( DbCountProperty ); }
            set { SetValue ( DbCountProperty , value ); }
        }
        public BitmapImage LhHsplitter
        {
            get { return ( BitmapImage ) GetValue ( LhHsplitterProperty ); }
            set { SetValue ( LhHsplitterProperty , value ); }
        }
        public static readonly DependencyProperty LhHsplitterProperty =
            DependencyProperty . Register ( "LhHsplitter" , typeof ( BitmapImage ) , typeof ( SplitViewer ) , new PropertyMetadata ( null ) );

        // Using a DependencyProperty as the backing store for DbCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DbCountProperty =
            DependencyProperty . Register ( "DbCount" , typeof ( int ) , typeof ( SplitViewer ) , new PropertyMetadata ( 0 ) );
        public double MaxSplitterHeight
        {
            get { return ( double ) GetValue ( MaxSplitterHeightProperty ); }
            set { SetValue ( MaxSplitterHeightProperty , value ); }
        }
        public static readonly DependencyProperty MaxSplitterHeightProperty =
            DependencyProperty . Register ( "MaxSplitterHeight" , typeof ( double ) , typeof ( SplitViewer ) , new PropertyMetadata ( ( double ) 500 ) );
        public double MaxSplitterWidth
        {
            get { return ( double ) GetValue ( MaxSplitterWidthProperty ); }
            set { SetValue ( MaxSplitterWidthProperty , value ); }
        }
        public static readonly DependencyProperty MaxSplitterWidthProperty =
            DependencyProperty . Register ( "MaxSplitterWidth" , typeof ( double ) , typeof ( SplitViewer ) , new PropertyMetadata ( ( double ) 500 ) );
        #endregion DP.s

        #region FlowDoc support
        /// <summary>
        ///  These are the only methods any window needs ot provide support for my FlowDoc system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void MaximizeFlowDoc ( object sender , EventArgs e )
        {
            // Clever "Hook" method that Allows the flowdoc to be resized to fill window
            // or return to its original size and position courtesy of the Event declard in FlowDoc
            fdl . MaximizeFlowDoc ( Flowdoc , canvas , e );
        }
        private void Flowdoc_MouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
        {
            // Window wide  !!
            // Called  when a Flowdoc MOVE has ended
            MovingObject = fdl . Flowdoc_MouseLeftButtonUp ( sender , Flowdoc , MovingObject , e );
            VSplitDown = false;
            ReleaseMouseCapture ( );
        }
        private void Flowdoc_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
        {
            //In this event, we get current mouse position on the control to use it in the MouseMove event.
            MovingObject = fdl . Flowdoc_PreviewMouseLeftButtonDown ( sender , Flowdoc , e );
        }
        private void Flowdoc_MouseMove ( object sender , MouseEventArgs e )
        {
            // We are Resizing the Flowdoc using the mouse on the border  (Border.Name=FdBorder)
            fdl . Flowdoc_MouseMove ( Flowdoc , canvas , MovingObject , e );
        }
        // Shortened version proxy call		
        private void Flowdoc_LostFocus ( object sender , RoutedEventArgs e )
        {
            Flowdoc . BorderClicked = false;
        }
        public void FlowDoc_ExecuteFlowDocBorderMethod ( object sender , EventArgs e )
        {
            // EVENTHANDLER to Handle resizing
            FlowDoc fd = sender as FlowDoc;
            Point pt = Mouse . GetPosition ( canvas );
            double dLeft = pt . X;
            double dTop = pt . Y;
        }
        public void fdmsg ( string line1 , string line2 = "" , string line3 = "" )
        {
            //We have to pass the Flowdoc.Name, and Canvas.Name as well as up   to 3 strings of message
            //  you can  just provie one if required
            // eg fdmsg("message text");
            fdl . FdMsg ( Flowdoc , canvas , line1 , line2 , line3 );
        }
        #endregion FlowDoc support  

        #region Splitter support methods

        #region Left Horizontal Splitter

        private void LeftSplitter_DragStarted ( object sender , DragStartedEventArgs e )
        {
            if ( slideupPanel . ActualHeight <= 10 )
            {
                LeftSplitterText = "Drag Up   ";
                LhHsplitter = new BitmapImage ( new Uri ( @"\icons\up arroiw red.png" , UriKind . Relative ) );
                ShowText = " To view lower Datagrid";
            }
            else if ( LeftHorzintalSplitter . ActualHeight <= 10 )
            {
                LeftSplitterText = "Drag Down ";
                LhHsplitter = new BitmapImage ( new Uri ( @"\icons\down arroiw red.png" , UriKind . Relative ) );
                ShowText = " To change view proportions";
            }
            else
            {
                LeftSplitterText = "Drag Up or Down  ";
                LhHsplitter = new BitmapImage ( new Uri ( @"\icons\Lrg updown arrow red copy.png" , UriKind . Relative ) );
                ShowText = " To change Datagrids view position split";
            }
        }
        private void LeftSplitter_DragCompleted ( object sender , DragCompletedEventArgs e )
        {
            if ( slidedownPanel . ActualHeight <= 10 )
            {
                LeftSplitterText = "Drag Down   ";
                LhHsplitter = new BitmapImage ( new Uri ( @"\icons\down arroiw red.png" , UriKind . Relative ) );
                ShowText = " To show more of lower Datagrid";
                LeftTopSplitterOffset = slidedownPanel . ActualHeight;
                Thickness th = new Thickness ( 0 , 0 , 0 , 0 );
                th . Top = LeftTopSplitterOffset;
                innerLeftBorder . Margin = th;
            }
            else if ( slideupPanel . ActualHeight <= 10 )
            {
                LeftSplitterText = "Drag Up  ";
                LhHsplitter = new BitmapImage ( new Uri ( @"\icons\up arroiw red.png" , UriKind . Relative ) );
                ShowText = " To show more of upper DataGrid.";
                LeftTopSplitterOffset = slidedownPanel . ActualHeight;
                Thickness th = new Thickness ( 0 , 0 , 0 , 0 );
                LeftPaneGrid . Margin = th;
            }
            else
            {
                LeftSplitterText = "Drag Up or Down  ";
                LhHsplitter = new BitmapImage ( new Uri ( @"\icons\Lrg updown arrow red copy.png" , UriKind . Relative ) );
                ShowText = " To change Datagrids view position split";
                // Green row grid
                LeftTopSplitterOffset = slidedownPanel . ActualHeight;
                Thickness th = new Thickness ( 0 , 0 , 0 , 0 );
                LeftPaneGrid . Margin = th;
                // Yellow  row grid
                Thickness th2 = new Thickness ( 0 , 0 , 0 , 0 );
                th2 . Top = LeftTopSplitterOffset;
                innerLeftBorder . Margin = th2;
            }
        }
        #endregion Left Horizontal Splitter

        #region Right Horizontal splitter resize handlers
        //All working well 31/3/2022 - MY BIRTHDAY !!
        private void RtSplitter_DragStarted ( object sender , System . Windows . Controls . Primitives . DragStartedEventArgs e )
        {
            if ( Middlerow1 . ActualHeight <= 10 )
            {
                imgup = new BitmapImage ( new Uri ( @"\icons\down arroiw red.png" , UriKind . Relative ) );
                ShowdragText = "Drag Down to ";
                ShowText = "show Upper Options Pane";
            }
            else if ( Middlerow2 . ActualHeight <= 10 )
            {
                // At bottom of screen
                imgup = new BitmapImage ( new Uri ( @"\icons\up arroiw red.png" , UriKind . Relative ) );
                ShowdragText = "Drag Up to ";
                ShowText = "show lower Options panel";
            }
            else
            {
                imgup = new BitmapImage ( new Uri ( @"\icons\Lrg updown arrow red copy.png" , UriKind . Relative ) );
                ShowdragText = "Drag Up/Down to ";
                ShowText = "Adjust Splitter Panels size";
            }
        }
        private void RtSplitter_DragCompleted ( object sender , System . Windows . Controls . Primitives . DragCompletedEventArgs e )
        {
            if ( Middlerow2 . ActualHeight <= 10 )
            {
                imgup = new BitmapImage ( new Uri ( @"\icons\up arroiw red.png" , UriKind . Relative ) );
                ShowdragText = "Drag Up to ";
                ShowText = "show lower Options Pane";
            }
            else if ( Middlerow1 . ActualHeight <= 10 )
            {
                imgup = new BitmapImage ( new Uri ( @"\icons\down arroiw red.png" , UriKind . Relative ) );
                ShowdragText = "Drag Down to ";
                ShowText = "show upper Options Pane";
            }
            else
            {
                imgup = new BitmapImage ( new Uri ( @"\icons\Lrg updown arrow red copy.png" , UriKind . Relative ) );
                //                imgmv = new BitmapImage ( new Uri ( @"\icons\sync.ico" , UriKind . Relative ) );
                ShowdragText = "Drag Up/Down to ";
                ShowText = "Adjust Splitter Panels size";
            }
        }
        #endregion Horizontal splitter resize handlers

        #region Vertical splitter resize handlers
        //All working well 31/3/2022 - MY BIRTHDAY !!
        private void VSplitter_DragStarted ( object sender , DragStartedEventArgs e )
        {

            //if ( Col0 . ActualWidth >= MaxSplitterWidth )
            //{
            //    vimgmove = new BitmapImage ( new Uri ( @"\icons\left arroiw red.png" , UriKind . Relative ) );
            //}
            //else if ( Col0 . ActualWidth <= 11 )
            //{
            //    vimgmove = new BitmapImage ( new Uri ( @"\icons\right arroiw red.png" , UriKind . Relative ) );
            //}
            //else
            //{
            //    vimgmove = new BitmapImage ( new Uri ( @"\icons\Lrg ltrt arrow red copy.png" , UriKind . Relative ) );
            //}
        }

        private void VSplitter_DragCompleted ( object sender , DragCompletedEventArgs e )
        {
            //double width = Col0 . ActualWidth;
            //double rwidth = Col2 . ActualWidth;
            //Console . WriteLine ( $"{width}, {rwidth}, {MaxSplitterWidth}" );
            //if ( Col0 . ActualWidth <= 10 )
            //{
            //    vimgmove = new BitmapImage ( new Uri ( @"\icons\right arroiw red.png" , UriKind . Relative ) );
            //}
            //else if ( rwidth <= 10 )
            //{
            //    vimgmove = new BitmapImage ( new Uri ( @"\icons\left arroiw red.png" , UriKind . Relative ) );
            //}
            //else
            //{
            //    vimgmove = new BitmapImage ( new Uri ( @"\icons\Lrg ltrt arrow red copy.png" , UriKind . Relative ) );
            //}
        }
        #endregion Vertical splitter resize handlers

        #endregion Splitter support methods

        #region Closedown
        private void Window_Closing ( object sender , System . ComponentModel . CancelEventArgs e )
        {

        }
        private void App_Close ( object sender , RoutedEventArgs e )
        {
            // Close down Application
            this . Close ( );
            Application . Current . Shutdown ( );
        }
        private void Datagrids_Close ( object sender , RoutedEventArgs e )
        {
            this . Close ( );
        }

        #endregion Closedown

        private void Window_PreviewKeyDown ( object sender , KeyEventArgs e )
        {

        }

        private void Window_PreviewMouseMove ( object sender , MouseEventArgs e )
        {

        }

        private void splttermplatet_SizeChanged ( object sender , SizeChangedEventArgs e )
        {
            Size size = e . NewSize;

            LeftTopSplitterOffset = slidedownPanel . ActualHeight;
            //Thickness th1 = new Thickness ( 0 , 0 , 0 , 0 );
            //th1 . Top = LeftTopSplitterOffset;
            //LeftPaneGrid . Margin = th1;

            Thickness th2 = new Thickness ( 0 , 0 , 0 , 0 );
            th2 . Top = LeftTopSplitterOffset;
            innerLeftBorder . Margin = th2;

            //Thickness th = new Thickness ( 0 , 0 , 0 , 0 );
            //th = vsplitterbar . Margin;
            //th . Left = innerLeftBorder . ActualWidth - 120;
            //vsplitterbar . Margin = th;
        }

        private bool VSplitDown = false;
        private double LeftWidth = 0;
        private double RightWidth = 0;
        private void vsplitterbar_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
        {
            VSplitDown = true;
        }

        private void vsplitterbar_PreviewMouseMove ( object sender , MouseEventArgs e )
        {
            //if( VSplitDown )
            //{
            //    double dbl = 0, diff=0;
            //    var conv = new GridLengthConverter ( );
            //    LeftWidth = outer0 . ActualWidth;
            //    RightWidth = outer2 . ActualWidth;
            //    Border bd = sender as Border;
            //    Point pt = e . GetPosition ( outer0 );
            //    diff = pt . X - LeftWidth - 10;
            //    LeftWidth += diff;
            //    RightWidth -= diff;

            //    Thickness th = new Thickness ( 0 , 0 , 0 , 0 );
            //    //if ( RightWidth > 350 )
            //{
            //    outer0 . Width = ( GridLength ) conv . ConvertFrom ( LeftWidth );
            //    outer2 . Width = ( GridLength ) conv . ConvertFrom ( RightWidth );
            //}
            //}
            //else
            //    Console . WriteLine ( $"missed {LeftWidth}, {RightWidth}" );

        }

        private void vsplitterbar_PreviewMouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
        {
            //VSplitDown = false;
            //ReleaseMouseCapture ( );
        }
    }
}
