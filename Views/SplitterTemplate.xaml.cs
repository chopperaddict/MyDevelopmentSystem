using System;
using System . Collections . Generic;
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

namespace MyDev . Views
{
    /// <summary>
    /// Interaction logic for SplitterTemplate.xaml
    /// </summary>
    public partial class SplitterTemplate : Window
    {
        #region All Template setup stuff
        #region Variables used for splitters
        private double MaxColWidth1 { get; set; }
        private double MinRowHeight1 { get; set; }
        private double MaxRowHeight { get; set; }
        #endregion Variables used for splitters

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
            vimgmove = new BitmapImage ( new Uri ( @"\icons\right arroiw red.png" , UriKind . Relative ) );
            LhHsplitter = new BitmapImage ( new Uri ( @"\icons\down arroiw red.png" , UriKind . Relative ) );
            //            lsplitrow1 . Height = (GridLength)1;
            // This sets the relative height of a Grid's row heights - works  too
            LeftPanelgrid . RowDefinitions [ 0 ] . Height = new GridLength ( 0.01 , GridUnitType . Star );
            LeftPanelgrid . RowDefinitions [ 1 ] . Height = new GridLength ( 20 , GridUnitType . Pixel );
            LeftPanelgrid . RowDefinitions [ 2 ] . Height = new GridLength ( 20 , GridUnitType . Star );

            Maingrid . RowDefinitions [ 0 ] . Height = new GridLength ( 0.00 , GridUnitType . Pixel );
            Maingrid . RowDefinitions [ 1 ] . Height = new GridLength ( 0 , GridUnitType . Star );
            Maingrid . RowDefinitions [ 2 ] . Height = new GridLength ( 20 , GridUnitType . Pixel );
            Maingrid . RowDefinitions [ 3 ] . Height = new GridLength ( 1 , GridUnitType . Star );
            Maingrid . RowDefinitions [ 4 ] . Height = new GridLength ( 75 , GridUnitType . Pixel );

            Maingrid . ColumnDefinitions [ 0 ] . Width = new GridLength ( 0 , GridUnitType . Pixel );
            Maingrid . ColumnDefinitions [ 1 ] . Width = new GridLength ( 20 , GridUnitType . Pixel );
            Maingrid . ColumnDefinitions [ 2 ] . Width = new GridLength ( 1 , GridUnitType . Star );
            Maingrid . ColumnDefinitions [ 3 ] . Width = new GridLength ( 10 , GridUnitType . Pixel );
            #endregion  Handling for splitters
            #endregion Template settings
            #region Template settings
            Flowdoc . ExecuteFlowDocMaxmizeMethod += new EventHandler ( MaximizeFlowDoc );
            ShowdragText = "Drag Down here to ";
            ShowText = "Show more records";
            #endregion Template   settings

        }

        #region Splitter support methods
        #region DP.s
        public string LeftSplitterText
        {
            get { return ( string ) GetValue ( LeftSplitterTextProperty ); }
            set { SetValue ( LeftSplitterTextProperty , value ); }
        }
        public static readonly DependencyProperty LeftSplitterTextProperty =
           DependencyProperty . Register ( "LeftSplitterText" , typeof ( string ) , typeof ( SplitterTemplate ) , new PropertyMetadata ( "Drag Down  " ) );
        public string ShowText
        {
            get { return ( string ) GetValue ( ShowTextProperty ); }
            set { SetValue ( ShowTextProperty , value ); }
        }
        public static readonly DependencyProperty ShowTextProperty =
            DependencyProperty . Register ( "ShowText" , typeof ( string ) , typeof ( SplitterTemplate ) , new PropertyMetadata ( "Show More Records" ) );
        public string ShowdragText
        {
            get { return ( string ) GetValue ( ShowdragTextProperty ); }
            set { SetValue ( ShowdragTextProperty , value ); }
        }
        public static readonly DependencyProperty ShowdragTextProperty =
            DependencyProperty . Register ( "ShowdragText" , typeof ( string ) , typeof ( SplitterTemplate ) , new PropertyMetadata ( "Drag Up/Down to  " ) );
        public BitmapImage imgup
        {
            get { return ( BitmapImage ) GetValue ( imgupProperty ); }
            set { SetValue ( imgupProperty , value ); }
        }
        public static readonly DependencyProperty imgupProperty =
            DependencyProperty . Register ( "imgup" , typeof ( BitmapImage ) ,
    typeof ( SplitterTemplate ) ,
                 new PropertyMetadata ( null ) );
        public BitmapImage imgdn
        {
            get { return ( BitmapImage ) GetValue ( imgdnProperty ); }
            set { SetValue ( imgdnProperty , value ); }
        }
        public static readonly DependencyProperty imgdnProperty =
            DependencyProperty . Register ( "imgdn" ,
                 typeof ( BitmapImage ) ,
                   typeof ( SplitterTemplate ) ,
                new PropertyMetadata ( null ) );
        public BitmapImage imgmv
        {
            get { return ( BitmapImage ) GetValue ( imgmvProperty ); }
            set { SetValue ( imgmvProperty , value ); }
        }
        public static readonly DependencyProperty imgmvProperty =
             DependencyProperty . Register ( "imgmv" ,
                  typeof ( BitmapImage ) ,
                    typeof ( SplitterTemplate ) ,
                 new PropertyMetadata ( null ) );
        public BitmapImage vimgleft
        {
            get { return ( BitmapImage ) GetValue ( vimgleftProperty ); }
            set { SetValue ( vimgleftProperty , value ); }
        }
        public static readonly DependencyProperty vimgleftProperty =
            DependencyProperty . Register ( "vimgleft" , typeof ( BitmapImage ) , typeof ( SplitterTemplate ) , new PropertyMetadata ( null ) );
        public BitmapImage vimgright
        {
            get { return ( BitmapImage ) GetValue ( vimgrightProperty ); }
            set { SetValue ( vimgrightProperty , value ); }
        }
        public static readonly DependencyProperty vimgrightProperty =
           DependencyProperty . Register ( "vimgright" , typeof ( BitmapImage ) , typeof ( SplitterTemplate ) , new PropertyMetadata ( null ) );
        public BitmapImage vimgmove
        {
            get { return ( BitmapImage ) GetValue ( vimgmoveProperty ); }
            set { SetValue ( vimgmoveProperty , value ); }
        }
        public static readonly DependencyProperty vimgmoveProperty =
            DependencyProperty . Register ( "vimgmove" , typeof ( BitmapImage ) , typeof ( SplitterTemplate ) , new PropertyMetadata ( null ) );
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
            DependencyProperty . Register ( "LhHsplitter" , typeof ( BitmapImage ) , typeof ( SplitterTemplate ) , new PropertyMetadata ( null ) );

        // Using a DependencyProperty as the backing store for DbCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DbCountProperty =
            DependencyProperty . Register ( "DbCount" , typeof ( int ) , typeof ( SplitterTemplate ) , new PropertyMetadata ( 0 ) );


        public double MaxSplitterHeight
        {
            get { return ( double ) GetValue ( MaxSplitterHeightProperty ); }
            set { SetValue ( MaxSplitterHeightProperty , value ); }
        }

        // Using a DependencyProperty as the backing store for MaxSplitterHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxSplitterHeightProperty =
            DependencyProperty . Register ( "MaxSplitterHeight" , typeof ( double ) , typeof ( SplitterTemplate ) , new PropertyMetadata ( (double) 500 ) );


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

        #region Horizontal splitter resize handlers
        /// <summary>
        /// Interaction logic for SplitterTemplate.xaml
        /// </summary>
        private void Splitter_DragStarted ( object sender , System . Windows . Controls . Primitives . DragStartedEventArgs e )
        {
            if ( Row1 . ActualHeight >= MaxSplitterHeight - 20)
            {
                // At bottom of screen
                imgup = new BitmapImage ( new Uri ( @"\icons\up arroiw red.png" , UriKind . Relative ) );
                //                imgup = new BitmapImage ( new Uri ( @"\icons\sync.ico" , UriKind . Relative ) );
                ShowdragText = "Drag Up to ";
                ShowText = "Hide Split panel";
                //                RotateTransform rotateTransform1 = new RotateTransform ( 90 , -15 , 15 );
            }
            else if ( Row1 . ActualHeight <= 11 )
            {
                imgup = new BitmapImage ( new Uri ( @"\icons\down arroiw red.png" , UriKind . Relative ) );
                ShowdragText = "Drag Down to ";
                ShowText = "Split Panel & Show Upper Pane";
            }
            else
            {
                imgup = new BitmapImage ( new Uri ( @"\icons\Lrg updown arrow red copy.png" , UriKind . Relative ) );
                ShowdragText = "Drag Up/Down to ";
                ShowText = "Adjust Splitter Panels size";
            }
        }
        private void Splitter_DragCompleted ( object sender , System . Windows . Controls . Primitives . DragCompletedEventArgs e )
        {
            if ( Row1 . ActualHeight >= MaxSplitterHeight  - 200)
            {
                imgup = new BitmapImage ( new Uri ( @"\icons\up arroiw red.png" , UriKind . Relative ) );
                ShowdragText = "Drag Up to ";
                ShowText = "Split panel & show lower Pane";
            }
            else if ( Row1 . ActualHeight <= 10 )
            {
                imgup = new BitmapImage ( new Uri ( @"\icons\down arroiw red.png" , UriKind . Relative ) );
                ShowdragText = "Drag Down to ";
                ShowText = "Split panel & show  upper pane";
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
        private void VSplitter_DragStarted ( object sender , DragStartedEventArgs e )
        {
            //vimgmove = null;
            if ( Col0 . ActualWidth >= MaxColWidth1 )
            {
                vimgmove = new BitmapImage ( new Uri ( @"\icons\left arroiw red.png" , UriKind . Relative ) );
            }
            else if ( Col0 . ActualWidth <= 11 )
            {
                vimgmove = new BitmapImage ( new Uri ( @"\icons\right arroiw red.png" , UriKind . Relative ) );
            }
            else
            {
                vimgmove = new BitmapImage ( new Uri ( @"\icons\Lrg ltrt arrow red copy.png" , UriKind . Relative ) );
            }
        }
        private void VSplitter_DragCompleted ( object sender , DragCompletedEventArgs e )
        {
            //            vimgmove = new BitmapImage ( new Uri ( @"\icons\left arroiw red.png" , UriKind . Relative ) );

            if ( Col0 . ActualWidth >= MaxColWidth1 )
            {
                vimgmove = new BitmapImage ( new Uri ( @"\icons\left arroiw red.png" , UriKind . Relative ) );
            }
            else if ( Col0 . ActualWidth <= 11 )
            {
                vimgmove = new BitmapImage ( new Uri ( @"\icons\right arroiw red.png" , UriKind . Relative ) );
            }
            else
            {
                vimgmove = new BitmapImage ( new Uri ( @"\icons\Lrg ltrt arrow red copy.png" , UriKind . Relative ) );
                //vimgmove . Rotation = Rotation . Rotate180;
            }
        }
        #endregion Vertical splitter resize handlers

        #region Left Horizontal Splitter
        private void LeftSplitter_DragCompleted ( object sender , DragCompletedEventArgs e )
        {
            if ( lsplitrow1 . ActualHeight >= Maingrid . ActualHeight - 100 )
            {
                LeftSplitterText = "Drag Up  ";
                LhHsplitter = new BitmapImage ( new Uri ( @"\icons\up arroiw red.png" , UriKind . Relative ) );
            }
            else if ( lsplitrow1 . ActualHeight <= 11 )
            {
                LeftSplitterText = "Drag Down  ";
                LhHsplitter = new BitmapImage ( new Uri ( @"\icons\down arroiw red.png" , UriKind . Relative ) );
            }
            else
            {
                LeftSplitterText = "Drag Up or Down  ";
                LhHsplitter = new BitmapImage ( new Uri ( @"\icons\Lrg updown arrow red copy.png" , UriKind . Relative ) );
            }
            //         LeftSplitterText = "Drag Up/Down to access secondary viewers ";
        }

        private void LeftSplitter_DragStarted ( object sender , DragStartedEventArgs e )
        {
            if ( lsplitrow1 . ActualHeight <= MinRowHeight1 )
            {
                LeftSplitterText = "Drag Up or Down  ";
                LhHsplitter = new BitmapImage ( new Uri ( @"\icons\Lrg updown arrow red copy.png" , UriKind . Relative ) );
                //                LhHsplitter = new BitmapImage ( new Uri ( @"\icons\down arroiw red copy.png" , UriKind . Relative ) );
                //              LhHsplitter = new BitmapImage ( new Uri ( @"\icons\up arroiw red.png" , UriKind . Relative ) );
            }
            else if ( lsplitrow1 . ActualHeight <= 10 )
            {
                LeftSplitterText = "Drag Down ";
                LhHsplitter = new BitmapImage ( new Uri ( @"\icons\down arroiw red.png" , UriKind . Relative ) );
            }
            else
            {
                LeftSplitterText = "Drag Up or Down  ";
                LhHsplitter = new BitmapImage ( new Uri ( @"\icons\Lrg updown arrow red copy.png" , UriKind . Relative ) );
            }

        }
        #endregion Left Horizontal Splitter

        #endregion Splitter support methods

        #endregion All Template setup stuff


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
            this.SetValue(MaxSplitterHeightProperty, e . NewSize.Height);
        }
    }
}
