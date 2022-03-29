using System;
using System. Collections;
using System. Collections. Generic;
using System. Collections. ObjectModel;
using System. ComponentModel;
using System. Diagnostics;
using System. Linq;
using System. Text;
using System. Threading. Tasks;
using System. Windows;
using System. Windows. Controls;
using System. Windows. Data;
using System. Windows. Documents;
using System. Windows. Input;
using System. Windows. Media;
using System. Windows. Media. Imaging;
using System. Windows. Shapes;

using MyDev. ViewModels;

namespace MyDev. Views
{
    /// <summary>
    /// Interaction logic for ModernViews.xaml
    /// </summary>
    public partial class ModernViews : Window//, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public LinkedList<BankAccountViewModel>BankLnkList = new LinkedList<BankAccountViewModel> ( );
        private static bool UseNotify = false;
        protected void OnPropertyChanged ( string propertyName )
        {
            var handler = PropertyChanged;
            if ( handler != null )
                handler ( this, new PropertyChangedEventArgs ( propertyName ) );
        }

        public static ObservableCollection<BankAccountViewModel> dgbvm = new ObservableCollection<BankAccountViewModel> ( );
        public static ObservableCollection<BankAccountViewModel> lvbvm = new ObservableCollection<BankAccountViewModel> ( );
        public static ObservableCollection<BankAccountViewModel> lbbvm = new ObservableCollection<BankAccountViewModel> ( );
        public CollectionView lvview { get; set; }
        public CollectionView dgview { get; set; }
        public CollectionView lbview { get; set; }
        public PropertyGroupDescription groupDescription { get; set; }
        private int CurrentActiveView { get; set; }

        private string filterText;
        public string FilterText
        {
            get { return filterText; }
            set { filterText = value; OnPropertyChanged ( FilterText ); }
        }

        //private int recCount;
        //public int RecCount
        //{
        //    get { return recCount; }
        //    set { recCount = value; OnPropertyChanged ( RecCount. ToString ( ) ); }
        //}

        #region Record Count Dependencies


        public int RecCount
        {
            get { return ( int ) GetValue ( RecCountProperty ); }
            set { SetValue ( RecCountProperty, value ); }
        }
        // Using a DependencyProperty as the backing store for RecCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RecCountProperty =
            DependencyProperty. Register ( "RecCount", typeof ( int ), typeof ( ModernViews ), new PropertyMetadata ( 0 ) );

        public int RecCountlv
        {
            get { return ( int ) GetValue ( RecCountlvProperty ); }
            set { SetValue ( RecCountlvProperty, value ); }
        }

        // Using a DependencyProperty as the backing store for recCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RecCountlvProperty =
            DependencyProperty. Register ( "RecCounlv", typeof ( int ), typeof ( ModernViews ), new PropertyMetadata ( 0 ), ValidateReccount );
        private static bool ValidateReccount ( object value )
        {
            Console. WriteLine ( $"RecCount = {value}" );
            return true;
        }
        public int RecCountlb
        {
            get { return ( int ) GetValue ( RecCountlbProperty ); }
            set { SetValue ( RecCountlbProperty, value ); }
        }
        // Using a DependencyProperty as the backing store for RecCountlb.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RecCountlbProperty =
            DependencyProperty. Register ( "RecCountlb", typeof ( int ), typeof ( ModernViews ), new PropertyMetadata ( 0 ) );

        public int RecCountdg
        {
            get { return ( int ) GetValue ( RecCountdgProperty ); }
            set { SetValue ( RecCountdgProperty, value ); }
        }
        // Using a DependencyProperty as the backing store for RecCountdg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RecCountdgProperty =
            DependencyProperty. Register ( "RecCountdg", typeof ( int ), typeof ( ModernViews ), new PropertyMetadata ( 0 ) );
        #endregion Recor Count Dependencies

        public ModernViews ( )
        {
            InitializeComponent ( );
            this. Show ( );
            //            DataContext = this;
            //Mouse. OverrideCursor = Cursors. Wait;
            lbview = new CollectionView ( lbbvm );
            lvview = new CollectionView ( lvbvm );
            dgview = new CollectionView ( dgbvm );
            EventControl. BankDataLoaded += EventControl_BankDataLoaded;
            Textbox1. TextChanged += Textbox1_TextChanged;
            lview3. LayoutUpdated += View_CurrentChanged;
            KeyDown += Window_PreviewKeyDown;
            this. UpdateLayout ( );
            MaxListview. Text = "200";
            MaxListbox. Text = "500";
            MaxDatagrid. Text = "2500";
            ItemsList. Visibility = Visibility. Hidden;

        }
        private void GroupViewer_Loaded ( object sender, RoutedEventArgs e )
        {
            this. UpdateLayout ( );
            Infopanel. Content = "Click 'Load Data' to Load Bank Account Data...";
            Infopanel. Refresh ( );

            MouseMove += Grab_MouseMove;
            ClearViewer ( null, null );
            //SqlBankcollection = new BankCollection ( );
            //SqlBankcollectionlv = new BankCollection ( );
            BankAccountViewModel bvm = new BankAccountViewModel ( );
            bvm. CustNo = "Dummy record for empty DataGrid";
            bvm. AcType = 3;
            BankLnkList. AddFirst ( bvm );
            Dgrid1. ItemsSource = BankLnkList;
            Mouse. OverrideCursor = Cursors. Arrow;
            this. DataContext = this;
        }

        private void View_CurrentChanged ( object sender, EventArgs e )
        {
            //if ( lview3. Visibility == Visibility. Visible )
            //{
            //    RecCountlv = lview3. Items. Count;
            //    RecCount = RecCountlv;
            //}
            //else if ( Dgrid1. Visibility == Visibility. Visible )
            //{
            //    RecCountdg = Dgrid1. Items. Count;
            //    RecCount = RecCountdg;
            //}
            //else if ( lbox1. Visibility == Visibility. Visible )
            //{
            //    RecCountlb = Dgrid1. Items. Count;
            //    RecCount = RecCountlb;
            //}
            reccount. UpdateLayout ( );
        }

        private void Grab_MouseMove ( object sender, MouseEventArgs e )
        {
            if ( e. LeftButton == MouseButtonState. Pressed )
                Utils. Grab_MouseMove ( sender, e );
            e. Handled = true;
        }

        private void Window_PreviewKeyDown ( object sender, KeyEventArgs e )
        {
            if ( e. Key == Key. F11 )
            {
                var pos = Mouse. GetPosition ( this );
                Utils. Grab_Object ( sender, pos );
                if ( Utils. ControlsHitList. Count == 0 )
                    return;
                Utils. Grabscreen ( this, Utils. ControlsHitList[0]. VisualHit, null, sender as Control );
            }
        }

        private void EventControl_BankDataLoaded ( object sender, LoadedEventArgs e )
        {
            if ( e. CallerType != "BANKLISTVIEW" )
            {
                Mouse. OverrideCursor = Cursors. Wait;
                Infopanel. Content = "Please wait - Creating View of the data ...";
                Infopanel. Refresh ( );
                // Load data into both collections
                if ( CurrentActiveView == 0 )
                {// ListView data received

                    lview3. Visibility = Visibility. Visible;
                    lview3. ItemsSource = null;
                    lview3. Items. Clear ( );

                    Collection<BankAccountViewModel> lvcollection = new Collection<BankAccountViewModel> ( );
                    lvcollection = e. DataSource as Collection<BankAccountViewModel>;
                    // Create a Collection for the listview  as this is what the grouping system Demands

                    lview3. ItemsSource = lvcollection;
                    lvview = ( CollectionView ) CollectionViewSource. GetDefaultView ( lview3. ItemsSource );
                    if ( lvview. SortDescriptions. Count == 0 )
                    {
                        Infopanel. Content = "Please wait - Creating Sort & Filtering Ssystem for the data ...";
                        Infopanel. UpdateLayout ( );
                        lvview. SortDescriptions. Add ( new SortDescription ( "CustNo", ListSortDirection. Ascending ) );
                    }
                    if ( lvview. Filter == null )
                        lvview. Filter = Filter_CustNo;
                    lview3. ItemsSource = lvview;
                    RecCountlv = lview3. Items. Count;
                    RecCount = RecCountlv;
                    Button1. Opacity = 1;
                    Button1. Content = "Clear DataGrid";
                    Button5. Opacity = 0.3;
                    Button5. Foreground = FindResource ( "Gray1" ) as SolidColorBrush;
                    Mouse. OverrideCursor = Cursors. Arrow;
                    Infopanel. Content = "List View data loaded Successfully...";
                    Infopanel. UpdateLayout ( );
                    reccount. UpdateLayout ( );
                }
                else if ( CurrentActiveView == 1 )
                {
                    //Grid only
                    Dgrid1. Visibility = Visibility. Visible;
                    //dgbvm = e. DataSource as Collection<BankAccountViewModel>;
    
                    Dgrid1. ItemsSource = null;
                    Dgrid1. Items. Clear ( );
                    Collection<BankAccountViewModel> dgcollection = new Collection<BankAccountViewModel> ( );
                    dgcollection = e. DataSource as Collection<BankAccountViewModel>;
                    Dgrid1. ItemsSource = dgcollection;
                    dgview = ( CollectionView ) CollectionViewSource. GetDefaultView ( Dgrid1. ItemsSource );
                    if ( dgview. SortDescriptions. Count == 0 )
                    {
                        Infopanel. Content = "Please wait - Creating Sort & Filtering Ssystem for the data ...";
                        Infopanel. UpdateLayout ( );
                        dgview. SortDescriptions. Add ( new SortDescription ( "CustNo", ListSortDirection. Ascending ) );
                    }
                    if ( dgview. Filter == null )
                        dgview. Filter = Filter_CustNo;
                    Dgrid1. ItemsSource = dgview;
                    Dgrid1. Refresh ( );
                    title. Content = $"Data Grid view of BankAccount.";
                    RecCountdg = Dgrid1. Items. Count;
                    RecCount = RecCountdg;
                    Mouse. OverrideCursor = Cursors. Arrow;
                    Button2. Opacity = 0.3;
                    Button2. Foreground = FindResource ( "Gray3" ) as SolidColorBrush;
                    Button1. Opacity = 1;
                    Button1. Content = "Clear DataGrid";
                    Infopanel. Content = "Data Grid data loaded successfully....";
                    Infopanel. UpdateLayout ( );
                    reccount. UpdateLayout ( );
                }
                else if ( CurrentActiveView == 2 )
                {
                    //Listboxonly
                    lbox1. Visibility = Visibility. Visible;
                    lbox1. ItemsSource = null;
                    lbox1. Items. Clear ( );
                    Collection<BankAccountViewModel> lbcollection = new Collection<BankAccountViewModel> ( );
                    lbcollection = e. DataSource as Collection<BankAccountViewModel>;
                    lbox1. ItemsSource = lbcollection;
                    lbview = ( CollectionView ) CollectionViewSource. GetDefaultView ( lbox1. ItemsSource );
                    if ( lbview. SortDescriptions. Count == 0 )
                    {
                        Infopanel. Content = "Please wait - Creating Sort & Filtering Ssystem for the data ...";
                        Infopanel. UpdateLayout ( );
                        lbview. SortDescriptions. Add ( new SortDescription ( "CustNo", ListSortDirection. Ascending ) );
                    }
                    if ( lbview. Filter == null )
                        lbview. Filter = Filter_CustNo;
                    lbox1. ItemsSource = lbview;
                    RecCountlb = lbox1. Items. Count;
                    RecCount = RecCountlb;
                    Mouse. OverrideCursor = Cursors. Arrow;
                    Button6. Opacity = 0.3;
                    Button6. Foreground = FindResource ( "Gray3" ) as SolidColorBrush;
                    Button1. Opacity = 1;
                    Button1. Content = "Clear ListBox";
                    reccount. UpdateLayout ( );
                    Infopanel. Content = "List Box data loaded Successfully...";
                    Infopanel. UpdateLayout ( );
                }
            }
            Mouse. OverrideCursor = Cursors. Arrow;
            Infopanel. Content = "System Ready ......";
            Infopanel. UpdateLayout ( );
        }
        private void Textbox1_TextChanged ( object sender, TextChangedEventArgs e )
        {
            Infopanel. Content = "Creating filter......";
            Infopanel. Refresh ( );
            Mouse. OverrideCursor = Cursors. Wait;
            TextBox tb = sender as TextBox;
            FilterText = tb. Text;
            if ( Dgrid1. SelectedIndex == -1 )
                Dgrid1. SelectedIndex = 0;
            if ( lview3. SelectedIndex == -1 )
                lview3. SelectedIndex = 0;
            if ( lbox1. SelectedIndex == -1 )
                lbox1. SelectedIndex = 0;
            if ( lview3. Visibility == Visibility. Visible )
                CollectionViewSource. GetDefaultView ( lview3. ItemsSource ). Refresh ( );
            else if ( lbox1. Visibility == Visibility. Visible )
                CollectionViewSource. GetDefaultView ( lbox1. ItemsSource ). Refresh ( );
            else
                CollectionViewSource. GetDefaultView ( Dgrid1. ItemsSource ). Refresh ( );
            if ( CurrentActiveView == 0 )
            {
                RecCountlv = lview3. Items. Count;
                RecCount = RecCountlv;
            }
            else if ( CurrentActiveView == 1 )
            {
                RecCountdg = Dgrid1. Items. Count;
                RecCount = RecCountdg;
            }
            else if ( CurrentActiveView == 2 )
            {
                RecCountlb = lbox1. Items. Count;
                RecCount = RecCountlb;
            }
            Infopanel. Content = "Data filtered......";
            Infopanel. Refresh ( );
            Mouse. OverrideCursor = Cursors. Arrow;
            reccount. UpdateLayout ( );
        }
        public bool Filter_CustNo ( object item )
        {
            if ( String. IsNullOrEmpty ( FilterText ) )
                return true;
            BankAccountViewModel dataitem = item as BankAccountViewModel;
            if ( dataitem. CustNo. Contains ( FilterText ) )
                return true;
            else
                return false;
        }
        private void Expander_Drop ( object sender, DragEventArgs e )
        {

        }

        // Clear either viewer
        private void ClearViewer ( object sender, RoutedEventArgs e )
        {
            Mouse. OverrideCursor = Cursors. Arrow;
            if ( lview3. Visibility == Visibility. Visible )
            {
                lview3. ItemsSource = null;
                lview3. Items. Clear ( );
                lview3. UpdateLayout ( );
                if ( CurrentActiveView == 0 )
                {
                    RecCountlv = lview3. Items. Count;
                    RecCount = RecCountlv;
                }
                else if ( CurrentActiveView == 1 )
                {
                    RecCountdg = Dgrid1. Items. Count;
                    RecCount = RecCountdg;
                }
                else if ( CurrentActiveView == 2 )
                {
                    RecCountlb = lbox1. Items. Count;
                    RecCount = RecCountlb;
                }
                Infopanel. Content = "Click 'Load ListView' to load data...";
                Infopanel. Refresh ( );
                Button5. Opacity = 1;
                Button5. Foreground = FindResource ( "Black0" ) as SolidColorBrush;
            }
            else if ( Dgrid1. Visibility == Visibility. Visible )

            {
                Dgrid1. ItemsSource = null;
                Dgrid1. Items. Clear ( );
                Dgrid1. UpdateLayout ( );
                Infopanel. Content = "Click 'Load DataGrid' to load data...";
                Infopanel. Refresh ( );
                if ( CurrentActiveView == 0 )
                {
                    RecCountlv = lview3. Items. Count;
                    RecCount = RecCountlv;
                }
                else if ( CurrentActiveView == 1 )
                {
                    RecCountdg = Dgrid1. Items. Count;
                    RecCount = RecCountdg;
                }
                else if ( CurrentActiveView == 2 )
                {
                    RecCountlb = lbox1. Items. Count;
                    RecCount = RecCountlb;
                }
                Button2. Opacity = 1;
                Button2. Foreground = FindResource ( "Black0" ) as SolidColorBrush;
            }
            else if ( lbox1. Visibility == Visibility. Visible )

            {
                lbox1. ItemsSource = null;
                lbox1. Items. Clear ( );
                lbox1. UpdateLayout ( );
                Infopanel. Content = "Click 'Load ListBox' to load data...";
                Infopanel. Refresh ( );
                if ( CurrentActiveView == 0 )
                {
                    RecCountlv = lview3. Items. Count;
                    RecCount = RecCountlv;
                }
                else if ( CurrentActiveView == 1 )
                {
                    RecCountdg = Dgrid1. Items. Count;
                    RecCount = RecCountdg;
                }
                else if ( CurrentActiveView == 2 )
                {
                    RecCountlb = lbox1. Items. Count;
                    RecCount = RecCountlb;
                }
                Button6. Opacity = 1;
                Button6. Foreground = FindResource ( "Black0" ) as SolidColorBrush;
            }
            reccount. UpdateLayout ( );
        }
        // load listview
        private void LoadListview ( object sender, RoutedEventArgs e )
        {
            // load listview
            Mouse. OverrideCursor = Cursors. Wait;
            CurrentActiveView = 0;
            Dgrid1. Visibility = Visibility. Hidden;
            lbox1. Visibility = Visibility. Hidden;
            lview3. Visibility = Visibility. Visible;
            ClearViewer ( null, null );
            Infopanel. Content = "Please wait - Loading ListView data...";
            Infopanel. Refresh ( );
            lview3. ItemsSource = null;
            lview3. Items. Clear ( );
            Collection<BankAccountViewModel> lvcollection = new Collection<BankAccountViewModel> ( );
            lvcollection = ListViewDataSupplier. GetBankData ( UseNotify, Convert. ToInt16 ( MaxListview. Text ) );
            if ( UseNotify )
                return;
            lview3. ItemsSource = lvcollection;
            lvview = ( CollectionView ) CollectionViewSource. GetDefaultView ( lview3. ItemsSource );
            if ( lvview. SortDescriptions. Count == 0 )
            {
                Infopanel. Content = "Please wait - Creating Sort & Filtering Ssystem for the data ...";
                Infopanel. UpdateLayout ( );
                lvview. SortDescriptions. Add ( new SortDescription ( "CustNo", ListSortDirection. Ascending ) );
            }
            if ( lvview. Filter == null )
                lvview. Filter = Filter_CustNo;
            lview3. ItemsSource = lvview;
            RecCountlv = lview3. Items. Count;
            RecCount = RecCountlv;
            Button1. Opacity = 1;
            Button1. Content = "Clear DataGrid";
            Button5. Opacity = 0.3;
            Button5. Foreground = FindResource ( "Gray1" ) as SolidColorBrush;
            Mouse. OverrideCursor = Cursors. Arrow;
            Infopanel. Content = "List View data loaded Successfully...";
            Infopanel. UpdateLayout ( );
            reccount. UpdateLayout ( );
        }
        // load data toGrid
        private void LoadDatagrid ( object sender, RoutedEventArgs e )
        {
            // Load Datagrid
            CurrentActiveView = 1;
            lbox1. Visibility = Visibility. Hidden;
            lview3. Visibility = Visibility. Hidden;
            Dgrid1. Visibility = Visibility. Visible;

            ClearViewer ( null, null );
            Infopanel. Content = "Please wait - Loading DataGrid data...";
            Infopanel. Refresh ( );
            Mouse. OverrideCursor = Cursors. Wait;
            Collection<BankAccountViewModel> dgcollection = new Collection<BankAccountViewModel> ( );
            dgcollection = DatagridDataSupplier. GetBankData ( UseNotify, Convert. ToInt16 ( MaxDatagrid. Text ) );
            if ( UseNotify )
                return;
            Dgrid1. ItemsSource = null;
            Dgrid1. Items. Clear ( );
            Dgrid1. ItemsSource = dgcollection;
            dgview = ( CollectionView ) CollectionViewSource. GetDefaultView ( Dgrid1. ItemsSource );
            if ( dgview. SortDescriptions. Count == 0 )
            {
                Infopanel. Content = "Please wait - Creating Sort & Filtering Ssystem for the data ...";
                Infopanel. UpdateLayout ( );
                dgview. SortDescriptions. Add ( new SortDescription ( "CustNo", ListSortDirection. Ascending ) );
            }
            if ( dgview. Filter == null )
                dgview. Filter = Filter_CustNo;
            Dgrid1. ItemsSource = dgview;
            title. Content = $"Data Grid view of BankAccount.";
            RecCountdg = Dgrid1. Items. Count;
            RecCount = RecCountdg;
            Mouse. OverrideCursor = Cursors. Arrow;
            Button2. Opacity = 0.3;
            Button2. Foreground = FindResource ( "Gray3" ) as SolidColorBrush;
            Button1. Opacity = 1;
            Button1. Content = "Clear DataGrid";
            Infopanel. Content = "Data Grid data loaded successfully....";
            Infopanel. UpdateLayout ( );
            reccount. UpdateLayout ( );
        }
        // Load Listbox
        private void LoadListbox ( object sender, RoutedEventArgs e )
        {
            // load data Listbox
            CurrentActiveView = 2;
            lbox1. Visibility = Visibility. Visible;
            Dgrid1. Visibility = Visibility. Hidden;
            lview3. Visibility = Visibility. Hidden;

            ClearViewer ( null, null );
            Infopanel. Content = "Please wait - Loading DataGrid data...";
            Infopanel. Refresh ( );
            Mouse. OverrideCursor = Cursors. Wait;
            lbox1. ItemsSource = null;
            lbox1. Items. Clear ( );
            Collection<BankAccountViewModel> lbcollection = new Collection<BankAccountViewModel> ( );
            lbcollection = ListBoxDataSupplier. GetBankData ( UseNotify, Convert. ToInt16 ( MaxListbox. Text ) );
            if ( UseNotify )
                return;
            lbox1. ItemsSource = lbcollection;
            lbview = ( CollectionView ) CollectionViewSource. GetDefaultView ( lbox1. ItemsSource );
            if ( lbview. SortDescriptions. Count == 0 )
            {
                Infopanel. Content = "Please wait - Creating Sort & Filtering Ssystem for the data ...";
                Infopanel. UpdateLayout ( );
                lbview. SortDescriptions. Add ( new SortDescription ( "CustNo", ListSortDirection. Ascending ) );
            }
            if ( lbview. Filter == null )
                lbview. Filter = Filter_CustNo;
            lbox1. ItemsSource = lbview;
            RecCountlb = lbox1. Items. Count;
            RecCount = RecCountlb;
            Mouse. OverrideCursor = Cursors. Arrow;
            Button6. Opacity = 0.3;
            Button6. Foreground = FindResource ( "Gray3" ) as SolidColorBrush;
            Button1. Opacity = 1;
            Button1. Content = "Clear ListBox";
            reccount. UpdateLayout ( );
            Infopanel. Content = "List Box data loaded Successfully...";
            Infopanel. UpdateLayout ( );
        }
        private void Button3_Click ( object sender, RoutedEventArgs e )
        {
            this. Close ( );
        }
        // Toggle panel visibility
        private void ToggleView ( object sender, RoutedEventArgs e )
        {
            Mouse. OverrideCursor = Cursors. Wait;
            //ItemsList. Visibility = Visibility. Hidden;
            if ( lbox1. Visibility == Visibility. Visible )
            {
                // Show ListView
                CurrentActiveView = 0;
                Dgrid1. Visibility = Visibility. Hidden;
                lbox1. Visibility = Visibility. Hidden;
                lview3. Visibility = Visibility. Visible;
                lvview. Refresh ( );
                if ( lview3. Items. Count == 0 )
                {
                    Infopanel. Content = "Click 'Load ListView' to load data...";
                    Infopanel. Refresh ( );
                }
                else
                {
                    Infopanel. Content = $"Data loaded ({MaxListview. Text}) records....";
                    Infopanel. Refresh ( );
                }
                title. Content = $"ListView view of BankAccounts";
                Button1. Content = "Clear ListView";
                RecCountlv = lview3. Items. Count;
                RecCount = RecCountlv;
                if ( lview3. Items. Count == 0 )
                    Button1. Opacity = 0.3;
                else
                    Button1. Opacity = 1;
            }
            else if ( lview3. Visibility == Visibility. Visible )
            {
                // Showing DataGrid
                CurrentActiveView = 1;
                lview3. Visibility = Visibility. Hidden;
                lbox1. Visibility = Visibility. Hidden;
                Dgrid1. Visibility = Visibility. Visible;
                dgview. Refresh ( );
                if ( Dgrid1. Items. Count == 0 )
                {
                    Infopanel. Content = "Click 'Load DataGrid' to load data...";
                    Infopanel. Refresh ( );
                }
                else
                {
                    Infopanel. Content = $"Data loaded ({MaxListview. Text}) records....";
                    Infopanel. Refresh ( );
                }
                title. Content = $"DataGrid view of BankAccounts";
                Button1. Content = "Clear DataGrid";
                RecCountdg = Dgrid1. Items. Count;
                RecCount = RecCountdg;
                if ( Dgrid1. Items. Count == 0 )
                    Button1. Opacity = 0.3;
                else
                    Button1. Opacity = 1;
            }
            else if ( Dgrid1. Visibility == Visibility. Visible )
            {
                //Showing ListBox
                CurrentActiveView = 2;
                lview3. Visibility = Visibility. Hidden;
                Dgrid1. Visibility = Visibility. Hidden;
                lbox1. Visibility = Visibility. Visible;
                lbview. Refresh ( );
                if ( lbox1. Items. Count == 0 )
                {
                    Infopanel. Content = "Click 'Load ListBox' to load data...";
                    Infopanel. Refresh ( );
                }
                else
                {
                    Infopanel. Content = $"Data loaded ({MaxListview. Text}) records....";
                    Infopanel. Refresh ( );
                }
                title. Content = $"ListBox view of BankAccounts";
                Button1. Content = "Clear ListBox";
                RecCountlb = lbox1. Items. Count;
                RecCount = RecCountlb;
                reccount. UpdateLayout ( );
                if ( lbox1. Items. Count == 0 )
                    Button1. Opacity = 0.3;
                else
                    Button1. Opacity = 1;
            }
            Mouse. OverrideCursor = Cursors. Arrow;
            reccount. UpdateLayout ( );
        }

        private void GroupViewer_KeyDown ( object sender, KeyEventArgs e )
        {
            if ( e. Key == Key. F8 )
                Debugger. Break ( );
        }

        private void MouseOverGreen ( object sender, MouseEventArgs e )
        {
            Label lbl = sender as Label;
            lbl. Background = FindResource ( "Green3") as SolidColorBrush;
            lbl. Foreground = Brushes. White;
        }

        private void MouseLeaveGreen ( object sender, MouseEventArgs e )
        {
            Label lbl = sender as Label;
            lbl. Background = Brushes. LightGreen ;
            lbl. Foreground = Brushes. Black;
        }

        private void MouseOverOrange ( object sender, MouseEventArgs e )
        {
            Label lbl = sender as Label;
            lbl. Background = FindResource ( "Orange1" ) as SolidColorBrush;
            lbl. Foreground = Brushes. White;
        }

        private void MouseLeaveOrange ( object sender, MouseEventArgs e )
        {
            Label lbl = sender as Label;
            lbl. Background = FindResource( "Orange5" ) as SolidColorBrush;
            lbl. Foreground = Brushes. Black;
        }

        private void MouseLeaveRed ( object sender, MouseEventArgs e )
        {
            Label lbl = sender as Label;
            lbl. Background = Brushes. Red;
            lbl. Foreground = Brushes. White;
        }

        private void MouseOverRed ( object sender, MouseEventArgs e )
        {
            Label lbl = sender as Label;
            lbl. Background = Brushes. LightSalmon;
            lbl. Foreground = Brushes. Black;
        }
        private void CloseBtn_Click ( object sender, RoutedEventArgs e )
        {
            this .Close ( );
        }

    }
}
