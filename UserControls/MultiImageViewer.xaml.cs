using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . ComponentModel;
using System . IO;
using System . Linq;
using System . Text;
using System . Threading;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Data;
using System . Windows . Documents;
using System . Windows . Input;
using System . Windows . Media;
using System . Windows . Media . Imaging;
using System . Windows . Navigation;
using System . Windows . Shapes;
using System . Windows . Threading;

namespace MyDev . UserControls
{
    /// <summary>
    /// Interaction logic for MultiImageViewer.xaml
    /// </summary>
    public partial class MultiImageViewer : UserControl, IDataErrorInfo
    {
        public ObservableCollection<Image> MVImages = new ObservableCollection<Image> ( );
        public bool WrapPanelLoaded { get; set; } = false;
        public int WPLoadCount { get; set; } = 0;
        private UserControlsViewer ucv { get; set; }
        public string Error { get; }
        string IDataErrorInfo.Error
        {
            get
            {
                if ( img == null )
                {
                    return "Image cannot be null.";
                }
                return null;
            }
        }
        public string this [ string PropertyName ] 
        { 
            get 
            { 
                if(PropertyName == "img")
                {
                    if ( img . Source == null )
                        return null;
                }
                return null;
            } 
        }

        //BackgroundWorker worker = new BackgroundWorker ( );
        public MultiImageViewer ( )
        {
            InitializeComponent ( );
            ucv = UserControlsViewer . GetUCviewer ( );
            if ( ucv.WrapPanelLoaded == false || MVImages . Count == 0 )
            {
                LoadImages ( );
                this . Refresh ( );

                {
                    //Dispatcher . Invoke ( ( ) =>
                    //{
                    //    Task . Run ( ( ) => LoadMultiViewImages ( Images , sp1 ) );

                    //} , System . Windows . Threading . DispatcherPriority . Background );
                    //Task . Run ( ( ) => LoadMultiViewImages ( Images , sp1 ) );

                    //ThreadStart threadDelegate = new ThreadStart ( LoadMultiViewImages );
                    //Thread newThread = new Thread ( threadDelegate );
                    //newThread . Start ( );


                    //                Thread thread = new Thread ( LoadMultiViewImages) ;

                }
                //ucv . uclistbox . UClistbox . ItemsSource = null;
                //ucv . uclistbox . UClistbox . Items . Clear ( );
                sp1 . Children . Clear ( );

                Thread thread = new Thread ( WrapPanelSetup );
                thread . Start ( );

                // reset images listbox contents we removed earlier
                //ucv . uclistbox . UClistbox . ItemsSource = MVImages;
                //ucv . uclistbox . UClistbox . SelectedIndex = ucv . uclistbox . CurrentIndex == -1 ? 0 : ucv . uclistbox . CurrentIndex;
                //ucv . uclistbox . UClistbox . SelectedItem = ucv . uclistbox . CurrentIndex == -1 ? 0 : ucv . uclistbox . CurrentIndex;
                //ucv . WrapPanelImages . Content = this;
                //ucv . WrapPanelLoaded = true;
                //WrapPanelLoaded = true;
            }
            else
            {
                ucv . InfoPanel . Text = $"Multi Image Viewer";
                //WrapPanelLoaded = true;
                ucv . WrapPanelLoaded = true;
            }

            //WrapPanelSetup ( );
        }
        async private void WrapPanelSetup ( )
        {
            {
                //            await Dispatcher . BeginInvoke ( DispatcherPriority . Normal , ( Action ) ( async ( ) =>
                //                 LoadMultiViewImages ()
                ////                LoadMultiViewImages ( Images , sp1 )
                //            ) );
            }
            if ( ucv . WrapPanelLoaded == false )
            {
                //ucv . uclistbox . UClistbox . ItemsSource = null;
                //ucv . uclistbox . UClistbox . Items . Clear ( );
//                sp1 . Children . Clear ( );
                await Dispatcher . BeginInvoke ( new Action ( ( ) =>
                 LoadMultiViewImages ( )
                ) , DispatcherPriority . ApplicationIdle );

                //await Dispatcher . BeginInvoke ( new Action ( ( ) =>
                //    // reset images listbox contents we removed earlier
                //    ucv . uclistbox . UClistbox . ItemsSource = MVImages
                //) , DispatcherPriority . ApplicationIdle );

                //await Dispatcher . BeginInvoke ( new Action ( ( ) =>
                //    ucv . uclistbox . UClistbox . SelectedIndex = ucv . uclistbox . CurrentIndex == -1 ? 0 : ucv . uclistbox . CurrentIndex
                //) , DispatcherPriority . ApplicationIdle );
                //await Dispatcher . BeginInvoke ( new Action ( ( ) =>
                //    ucv . uclistbox . UClistbox . SelectedItem = ucv . uclistbox . CurrentIndex == -1 ? 0 : ucv . uclistbox . CurrentIndex
                //) , DispatcherPriority . ApplicationIdle );
                //await Dispatcher . BeginInvoke ( new Action ( ( ) =>
                //    ucv . WrapPanelImages . Content = this
                //) , DispatcherPriority . ApplicationIdle );
            
//                Mouse . OverrideCursor = Cursors . Arrow;
            }
            else
            {
                //ucv . InfoPanel . Text = $"Multi Image Viewer";
                //WrapPanelLoaded = true;
                //ucv . WrapPanelLoaded = true;
            }

            {
                //await Task . Run ( ( ) => LoadMultiViewImages ( Images , sp1 ) );
                //return;

                //worker = new BackgroundWorker ( );
                //worker . RunWorkerCompleted += new RunWorkerCompletedEventHandler ( worker_RunWorkerCompleted );
                //worker . DoWork += new DoWorkEventHandler ( worker_DoWork );
                //worker . WorkerReportsProgress = true;
                //worker . ProgressChanged += worker_ProgressChanged;
                //worker . RunWorkerAsync ( 10000 );
                //          Mouse . OverrideCursor = Cursors . Arrow;
            }
        }

        private async void LoadMultiViewImages ( )
        {
            WPLoadCount = 0;
//            ucv . uclistbox . UClistbox . ItemsSource = null;
//            ucv . uclistbox . UClistbox . Items . Clear ( );
            for ( int x = WPLoadCount ; x < MVImages . Count ; x++ )
            {
                await this . Dispatcher . BeginInvoke ( new Action ( ( ) =>
                {
                    sp1 . Children . Add ( MVImages [ x ] );
                    WPLoadCount++;
                    string msg = $"{WPLoadCount}/{MVImages . Count} images loaded successfully.....";
                    ucv . Loadcounter . Text = msg;
                    Thread . Sleep ( 5 );
                } ) );
                if ( WPLoadCount >= 50 )
                {
                    break;
                }
            }
            // Only now we set Images Loaded into WrapPanel flags
  //         WrapPanelLoaded = true;
            ucv . WrapPanelLoaded = true;
        }
        private void LoadImages ( )
        {
            int count = 0;
            if ( MVImages . Count == 0 )
            {
                Mouse . OverrideCursor = Cursors . Wait;
                string path = @"C:\\Users\ianch\pictures\";
                var imagefiles = Directory . GetFiles ( path );
                foreach ( var imagefile in imagefiles )
                {
                    if ( imagefile . ToUpper ( ) . Contains ( ".PSD" ) == false )
                    {
                        try
                        {
                            Uri url = new Uri ( imagefile );
                            BitmapImage img = new BitmapImage ( url );
                            Image image = new Image ( );
                            image . Source = img;
                            image . Height = 100;
                            image . Width = 100;
                            image . Tag = url;
                            MVImages . Add ( image );
                            count++;
                        }
                        catch ( Exception ex ) { Console . WriteLine ( $"{ex . Message}" ); }
                    }
                }
            }
            Console . WriteLine ( $"{count} images loaded into ObservableCollection MvImages" );
            Mouse . OverrideCursor = Cursors . Arrow;
        }

        private void img_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
        {
            // Show image in standard viewer & set Images listbox pointer to this image as well !!
            int index = 0;
            WrapPanel img = sender as WrapPanel;
            Image imagename = e . OriginalSource as Image;
            string s = imagename . Tag . ToString ( );
            s = s . Substring ( 8 );
            Uri url = new Uri ( s );
            BitmapImage img2 = new BitmapImage ( url );
            Image image = new Image ( );
            image . Source = img2;
            ucv . Contentctrl . Content = image;
            ucv . Contentctrl . Refresh ( );
            ucv . WrapPanelImages . Visibility = Visibility . Collapsed;
            ucv . Contentctrl . Visibility = Visibility . Visible;

            UCListBox ucl = UCListBox . GetUcLisbox ( );
            foreach ( var item in MVImages )
            {
                string str = item . Tag . ToString ( ) . Substring ( 8 );
                if ( str == s )
                {
                    ucl . CurrentIndex = index;
                    ucl . UClistbox . SelectedIndex = index;
                    ucl . UClistbox . SelectedItem = index;
                    break;
                }
                index++;
            }

        }
        public void ChangeSize ( )
        {
            UserControl_SizeChanged ( null , null );
        }
        private void UserControl_SizeChanged ( object sender , SizeChangedEventArgs e )
        {
            sv1 . Width = this . Width;
            sv1 . Height = this . Height;
        }

        private void sp1_ScrollChanged ( object sender , ScrollChangedEventArgs e )
        {
            var v = e . VerticalChange;
            Console . WriteLine ( $"{v}" );
        }

        private void sp1_PreviewMouseWheel ( object sender , MouseWheelEventArgs e )
        {
            var v = e . Delta;
            Console . WriteLine ( $"Wheel Delta = {v}" );

        }

        private void sp1_MouseWheel ( object sender , MouseWheelEventArgs e )
        {
            var v = e . GetPosition ( sp1 );
            Console . WriteLine ( $"oisition = {v}" );

        }

        private void img_PreviewMouseRightButtonDown ( object sender , MouseButtonEventArgs e )
        {
            int index = 0;
            WrapPanel img = sender as WrapPanel;
            Image imagename = e . OriginalSource as Image;
            string s = imagename . Tag . ToString ( );
            s = s . Substring ( 8 );
            for ( int x = 0 ; x < MVImages . Count; x++ )
            {
                if ( MVImages [ x ] . Name == s )
                {
                    break;
                }
            }
        }
        //    private async void worker_DoWork ( object sender , DoWorkEventArgs e )
        //    {
        //        // do the work required here.... it  shoulld return  the data set, whatever that is ?
        //        //Dispatcher . BeginInvoke ( DispatcherPriority . Normal , ( Action ) ( async ( ) =>
        //        //    Task.Run(worker . ReportProgress ( 0 , null ));
        //        //) );
        //        //await  Task . Run ( () => worker . ReportProgress ( 0 , null ) );

        //        //worker . ReportProgress ( 0 , null );
        //        // Dispatcher . Invoke ( ( ) =>
        //        //{
        //        //    Task . Run ( ( ) => LoadMultiViewImages ( Images , sp1 ) )

        //        //} , System . Windows . Threading . DispatcherPriority . Background; 
        //    }
        //    void worker_RunWorkerCompleted ( object sender , RunWorkerCompletedEventArgs e )
        //    {
        //        Console . WriteLine ( "Background worker completed...." );
        //        // handle the data once it has been loaded here ....
        //        // Typically something like 
        //        // datagrid . ItemsSource = "Data received object" from above
        //    }

        //    async public void worker_ProgressChanged ( object sender , ProgressChangedEventArgs e )
        //    {

        //        Console . WriteLine ( "in progress changed...." );
        //        return;
        //        sp1 . Children . Clear ( );
        //        if ( WrapPanelLoaded == false )
        //        {
        //            ucv . uclistbox . UClistbox . ItemsSource = null;
        //            //ucv . uclistbox . UClistbox . Items . Clear ( );

        //            foreach ( var item in MVImages )
        //            {
        //                sp1 . Children . Add ( item );
        //                //                    miv . sp1 . UpdateLayout ( );
        //            }
        //            Mouse . OverrideCursor = Cursors . Wait;
        //            ucv . Contentctrl . Content = this;
        //            ucv . Contentctrl . Refresh ( );
        //            //WrapPanelImages . Visibility = Visibility . Visible;
        //            //WrapPanelImages . Opacity = 1;
        //            ucv . WrapPanelImages . Content = this;
        //            Mouse . OverrideCursor = Cursors . Arrow;
        //        }
        //        else
        //        {
        //            Mouse . OverrideCursor = Cursors . Wait;
        //            ucv . Contentctrl . Visibility = Visibility . Collapsed;
        //            ucv . WrapPanelImages . Visibility = Visibility . Visible;
        //            ucv . WrapPanelImages . Opacity = 1;
        //            Mouse . OverrideCursor = Cursors . Arrow;
        //        }
        //        ucv . InfoPanel . Text = $"Multi Image Viewer";
        //        WrapPanelLoaded = true;
        //        ucv . WrapPanelLoaded = true;
        //        return;
        //    }
    }

}
