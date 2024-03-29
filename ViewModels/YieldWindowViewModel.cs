﻿using System;
using System . Collections;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . ComponentModel;
using System . Data;
using System . Diagnostics;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Data;
using System . Windows . Input;

using MyDev . SQL;
using MyDev . Views;


using TextBox = System . Windows . Controls . TextBox;

namespace MyDev . ViewModels
{
    public  class YieldWindowViewModel :INotifyPropertyChanged
    {
        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged ( string propertyName )
        {
            if ( PropertyChanged != null )
            {
                PropertyChanged ( this , new PropertyChangedEventArgs ( propertyName ) );
            }
        }
        #endregion OnPropertyChanged

        public ICommand LoadStack1 { get; set; }
        public ICommand LoadStack2 { get; set; }
        public ICommand LoadYield1 { get; set; }
        public ICommand LoadYield2 { get; set; }
        public ICommand LoadNormal { get; set; }
        public ICommand Iterate { get; set; }
        public ICommand Iterate2 { get; set; }
        public ICommand CloseAllBtn { get; set; }
        public string Counter1 { get; set; }
        public string Counter2 { get; set; }
        public string Duration1 { get; set; }
        public string Duration2 { get; set; }
        public int limit1 { get; set; }
        public int limit2 { get; set; }

        private int stackorder = 0;

        #region Std Variables / Properties

        private YieldWindow win { get; set; }
        public GenericStack<BankAccountViewModel> gstack { get; set; }

        public string SqlCommand = "Select  * from BankAccount";
        public ObservableCollection<BankAccountViewModel> bvm2 = new ObservableCollection<BankAccountViewModel> ( );
 //       public CollectionView BankCollectionView { get; set; }
        public  DataTable dt = new DataTable ( );
        public Stopwatch sw = new Stopwatch ( );
        public  bool Grid1RowEdited { set; get; }
        public int EditedRow1 { get; set; }
        public int EditedRow2 { get; set; }
        public bool Grid2RowEdited { set; get; }
        private string duration { get; set; }

        // Nullable type declarations for testing
        public Nullable<int> nullint { get; set; }
        public int? nulltype { get; set; }

        #endregion Variables / Properties

        public string Button1Text { get; } = "Load Stack";

        #region Full Properties
        public string customernum1;
        public  string customernum2;
        private BankAccountViewModel selectedaccount1;
        private BankAccountViewModel selectedaccount2;
        private string Infopanel;

        public string InfoPanel
        {
            get { return Infopanel; }
            set { Infopanel = value; NotifyPropertyChanged ( "InfoPanel" ); }
        }
        
        private ObservableCollection<BankAccountViewModel> bview;
        public ObservableCollection<BankAccountViewModel> bvm
        {
            get { return bview; }
            set { bview = value;  
                if ( value == null ) Console . WriteLine ($"bview set to null");
                else Console . WriteLine ( $"bview set to {value}" );
                NotifyPropertyChanged ( "bvm" );
            }
        }
        public string CustomerNum1
        {
            get { return customernum1; }
            set { customernum1 = value; NotifyPropertyChanged ( "CustomerNum1" ); }
        }
        public string CustomerNum2
        {
            get { return customernum2; }
            set { customernum2 = value; NotifyPropertyChanged ( "CustomerNum2" ); }
        }
        public BankAccountViewModel SelectedAccount1
        {
            get { return selectedaccount1; }
            set
            {
                selectedaccount1 = value;
                NotifyPropertyChanged ( "SelectedAccount1" );
                //                Console . WriteLine (   $"{SelectedAccount1.ToString()}");
                CustomerNum1 = SelectedAccount1? . CustNo;
            }
        }
        public BankAccountViewModel SelectedAccount2
        {
            get { return selectedaccount2; }
            set
            {
                if ( value != null )
                {
                    selectedaccount2 = value;
                    NotifyPropertyChanged ( "SelectedAccount2" );
                    CustomerNum2 = SelectedAccount2? . CustNo;
                }
            }
        }



        //public SelectionChangedEventArgs SelectionChangedEventArgs { get; set; }
        #endregion Full Properties

        //public SelectionChangedEventHandler Dgrid2_SelectionChanged { get; internal set; }
        public YieldWindowViewModel ( ) {
            LoadStack1 = new RelayCommand ( ExecuteLoadstack1 , CanExecuteLoadstack1 );
            LoadStack2 = new RelayCommand ( ExecuteLoadstack2 , CanExecuteLoadstack2 );
            LoadYield1 = new RelayCommand ( ExecuteLoadYield1 , CanExecuteLoadYield1 );
            LoadYield2 = new RelayCommand ( ExecuteLoadYield2 , CanExecuteLoadYield2 );
            LoadNormal = new RelayCommand ( ExecuteLoadNormal , CanExecuteLoadNormal );
            Iterate = new RelayCommand ( ExecuteLoadIterate , CanExecuteIterate );
            Iterate2 = new RelayCommand ( ExecuteLoadIterate2 , CanExecuteIterate2 );
            CloseAllBtn = new RelayCommand ( ExecuteCloseAllBtn , CanExecuteCloseAllBtn );
        }


        private void ExecuteCloseAllBtn ( object obj )
        {
            Application . Current . Shutdown ( );
        }

        #region CanExecute
        private bool CanExecuteLoadstack1 ( object arg )
        { return true; }
        private bool CanExecuteLoadstack2 ( object arg )
        { return true; }
          private bool CanExecuteLoadYield1 ( object arg )
        {return true;}
        private bool CanExecuteLoadYield2 ( object arg )
        { return true; }
        private bool CanExecuteLoadNormal( object arg )
        { return true; }
        private bool CanExecuteIterate ( object arg )
        {return true; }
        private bool CanExecuteIterate2 ( object arg )
        {return true;}
        private bool CanExecuteCloseAllBtn ( object arg )
        {return true; }

        #endregion CanExecute

        private void ExecuteLoadstack1 ( object obj )
        {Loadstack1 ( );}
        public void ExecuteLoadstack2 (object arg )
        {
            if ( Convert . ToInt16 ( arg ) == 0 )
                stackorder = 1;
            else stackorder = 2;
            iterate_Click ( );
            //            Stack2_PreviewButton(1 );
        }
        private void ExecuteLoadYield1 ( object obj )
        {
            loadyield1 ( );
        }
        private void ExecuteLoadYield2 ( object obj )
        {Loadyield2 ( );}
         private void ExecuteLoadNormal ( object obj )
        {Loadnormal ( );}

        private void ExecuteLoadIterate ( object obj )        
        {iterate_Click ( );}
        private void ExecuteLoadIterate2 ( object obj )
        {
            //Iterate_Click2 ( );
        }

        public void PassViewPointer ( YieldWindow yieldwin )
        {
            sw . Start ( );
            sw . Stop ( );
            win = yieldwin;
            bvm = new ObservableCollection<BankAccountViewModel> ( );
            //            UpdateBankRecord += YieldWindow_UpdateBankRecord;
            Loadnormal (  );
            win . dgrid1 . ItemsSource = bvm;  
        }  
  
        public void Dgrid1_SelectionChanged ( object sender , System . Windows . Controls . SelectionChangedEventArgs e )
        {
            int counter = 0;
            if ( Grid1RowEdited )
            {
                BankAccountViewModel bv = SelectedAccount1;
                // Update Sql here !!!!
                // The flag tells us the Previously selected record was changed.
                BankCollection . UpdateBankDb ( bv , "" );
                Grid1RowEdited = false;
                SelectedAccount1 = win.dgrid1 . SelectedItem as BankAccountViewModel;
            }
            else
            {
                DataGrid dg = sender as DataGrid;
                SelectedAccount1 = win .dgrid1 . SelectedItem as BankAccountViewModel;
            }
            try
            {
                if ( SelectedAccount2 != null )
                {
                    // select same record in Grid2
                    foreach ( BankAccountViewModel item in win. dgrid2 . Items )
                    {
                        if ( item ?. CustNo == SelectedAccount1? . CustNo && item? . BankNo == SelectedAccount1 ?. BankNo )
                        {
                            win . dgrid2 . SelectedIndex = counter;
                            win . dgrid2 . SelectedItem = counter;
                            win . dgrid2 . BringIntoView ( );
                            Utils . ScrollRecordIntoView ( win . dgrid2 , counter );
                            break;
                        }
                        counter++;
                    }
                }
            }
            catch ( Exception )
            { }
        }

        public void dgrid2_SelectionChanged ( object sender , System . Windows . Controls . SelectionChangedEventArgs e )
        {
            if ( Grid2RowEdited )
            {
                BankAccountViewModel bv = SelectedAccount2;
                // Update Sql here !!!!
                // The flag tells us the Previously selected record was changed.
                BankCollection . UpdateBankDb ( bv , "" );
                Grid2RowEdited = false;
                SelectedAccount2 = win . dgrid2 . SelectedItem as BankAccountViewModel;
            }
            else
            {
                DataGrid dg = sender as DataGrid;
                SelectedAccount2 = win . dgrid2 . SelectedItem as BankAccountViewModel;
            }
            if ( SelectedAccount2 == null )
                return;
            int counter = 0;
            {
                foreach ( BankAccountViewModel item in win . dgrid1 . Items )
                {
                    if ( item . CustNo == SelectedAccount2 . CustNo && item . BankNo == SelectedAccount2 . BankNo )
                    {
                        win . dgrid1 . SelectedIndex = counter;
                        win . dgrid1 . SelectedItem = counter;
                        win . dgrid1 . BringIntoView ( );
                        Utils . ScrollRecordIntoView ( win . dgrid1 , counter );
                        break;
                    }
                    counter++;
                }
            }
         }  

        private void YieldWindow_UpdateBankRecord ( object sender , DbArgs args )
        {
        //    //if ( args . UseMatch == false )
        //    //{
        //    dgrid1 . SelectedIndex = args . index;
        //    dgrid1 . SelectedItem = args . index;
        //    dgrid1 . BringIntoView ( );
        //    dgrid1 . Refresh ( );
        //    dgrid1 . BringIntoView ( );
        //    Utils . ScrollRecordIntoView ( dgrid1 , args . index );
        //    //}
        //    //else
        //    //{
        //    //    dgrid1 . SelectedIndex = BankAccountViewModel . FindMatchingIndex ( args . bvm , dgrid1 );
        //    //    dgrid1 . SelectedItem= dgrid1 . SelectedIndex;
        //    //    dgrid1 . BringIntoView ( );
        //    //    Utils . ScrollRecordIntoView ( dgrid1 , dgrid1 . SelectedIndex );
        //    //}
        }

        public  void Loadnormal ( )
        {
            //Standard Db load
            bvm . Clear ( );
            dt . Clear ( );
            sw . Restart ( );
            SqlCommand = "Select * from BankAccount order by CustNo,BankNo";
            dt = DataLoadControl . GetDataTable ( SqlCommand );
            bvm = SqlSupport . LoadBankCollection ( dt , true );
            sw . Stop ( );
            win. duration1 . Content = $"{sw . ElapsedMilliseconds} ms";
            win . counter2 . Content = bvm . Count . ToString ( );
            win . dgrid1 . ItemsSource = null;
            win . dgrid1 . Items . Clear ( );
            win . dgrid1 . ItemsSource = bvm;
            win . dgrid1 . SelectedIndex = 0;
            win . dgrid1 . SelectedItem = 0;
            Utils . ScrollRecordIntoView (win .dgrid1, 0 );
            InfoPanel = $"Loaded grid1 from Db data in {sw . ElapsedMilliseconds} ms";
        }
        public  void Loadstack1(  )
        {
            // load stack
            if ( bvm . Count > 0 )
            {
                sw . Restart ( );
                // Create stack just largeenought for our data to save memory
                gstack = new GenericStack<BankAccountViewModel> ( bvm . Count + 1 );
                foreach ( BankAccountViewModel item in bvm )
                {
                    gstack . Push ( item );
                }
                sw . Stop ( );
                  InfoPanel = $"Loaded Stack with {gstack . Count ( )} records from Db in {sw . ElapsedMilliseconds} ms";
            }
            else
             InfoPanel = $"Table(bvm) needs to be reloaded before stack can be loaded";
        }

        private bool usenew = false;
        private void loadyield1 ( )
        {
            usenew = true;
            Loadyield2 ( );
           usenew = false;
        }

        public  void Loadyield2 ()
        {
            // Db load using yield return
            win.dgrid2 . ItemsSource = null;
            win . dgrid2 . Items . Clear ( );
            bvm2 . Clear ( );
            dt . Clear ( );
            SqlCommand = "Select * from BankAccount order by CustNo,BankNo";
            //This is controlled by Attribute  [conditional USECW] set in Extensions.cs
            "Select * from BankAccount order by CustNo,BankNo" . log ( );
            "Select * from BankAccount order by CustNo,BankNo" . CW ( );
            dt = DataLoadControl . GetDataTable ( SqlCommand );
            win . dgrid2 . Items . Clear ( );
            win . dgrid2 . ItemsSource = bvm2;
            int lim1 = limit1 == 0 ? 1 : limit1;
            int lim2 = limit2 == 0 ? 4 : limit2;
            BankAccountViewModel bvrecord = new BankAccountViewModel ( );
            //This only returns matching items to the ACTYPE(s) passed in
            // which SAVES us using loads of memory to get ALL records and
            //ONLY THEN choose what we want to display in our grid
            sw . Restart ( );
            //            IEnumerator ie = bvm . GetEnumerator ( );
            if ( usenew == false )
            {
                foreach ( DataRow dtBank in LoadSelectedBankYield ( lim1 , lim2 ) )
                {
                    //    slow way !!!!
                    bvrecord = new BankAccountViewModel ( ); ;
                    // only process firther if it matches our ocndition
                    bvrecord . Id = Convert . ToInt32 ( dtBank [ 0 ] );
                    bvrecord . BankNo = dtBank [ 1 ] . ToString ( );
                    bvrecord . CustNo = dtBank [ 2 ] . ToString ( );
                    bvrecord . AcType = Convert . ToInt32 ( dtBank [ 3 ] );
                    bvrecord . Balance = Convert . ToDecimal ( dtBank [ 4 ] );
                    bvrecord . IntRate = Convert . ToDecimal ( dtBank [ 5 ] );
                    bvrecord . ODate = Convert . ToDateTime ( dtBank [ 6 ] );
                    bvrecord . CDate = Convert . ToDateTime ( dtBank [ 7 ] );
                    bvm2 . Add ( bvrecord );
                    win.dgrid2 . Refresh ( );
                }
            }
            else
            {
                foreach ( BankAccountViewModel dtBank in Utils . ReadGenericCollection ( bvm ) )
                {
                    //    faster way !!!!
                    if ( dtBank . AcType >= lim1 && dtBank . AcType <= lim2 )
                    {
                        bvm2 . Add ( dtBank );
                        win.dgrid2 . Refresh ( );
                    }
                }
            }
            sw . Stop ( );
            Counter1 = bvm2 . Count . ToString ( );
            Duration2= $"{sw . ElapsedMilliseconds} ms";
            win.dgrid2 . SelectedIndex = 0;
            win.dgrid2 . SelectedItem = 0;
            InfoPanel = $"Loaded grid2 from Db data in {sw . ElapsedMilliseconds} ms";
        }
        public IEnumerable<DataRow> LoadSelectedBankYield ( int lim1 , int lim2 )
        {
            // Use Yield return for max efficiency
            //YIELD RETURN Method to return Bank records matching ACTYPE lowHigh values pased to it
            // Only  returns records that have a matching ACTYPE(s), saving memory
            // that would otherwise be used processing all the (nearly 5000 records in this Db
            // NB There s a time overhead for this.
            BankAccountViewModel bvrecord = new BankAccountViewModel ( );
            foreach ( DataRow item in dt . Rows )
            {
                bvrecord . AcType = Convert . ToInt32 ( item [ 3 ] );
                if ( bvrecord . AcType >= lim1 && bvrecord . AcType <= lim2 )
                    yield return item;
            }
            //return null;
        }

        //private void Closebtn ( object sender , RoutedEventArgs e )
        //{
        //    this . Close ( );
        //}
        ////        private bool MatchonContent = false;

        public void dgrid1_CellEditEnding ( )
        {
            EditedRow1 = win.dgrid1 . SelectedIndex;
            SelectedAccount1 = win.dgrid1 . SelectedItem as BankAccountViewModel;
            Grid1RowEdited = true;
        }

        public void dgrid2_CellEditEnding (  )
        {

            EditedRow2 = win.dgrid2 . SelectedIndex;
            SelectedAccount2 = win.dgrid2 . SelectedItem as BankAccountViewModel;
            Grid2RowEdited = true;
        }

        #region Iteration example
        public void iterate_Click (  )
        {
            // This uses a Generic Iteration Method in UTILS.CS to read the data that handles any type of Collection
            // it generates & uses the IEnumerable Iterator method to access the collection and  returns them indiviually
            // because it uses the yield return  system, so it can be used to obtain any number of records
            // as individual items without having to load them all into memory first
            bvm2 . Clear ( );
            win.dgrid2 . ItemsSource = null;
            win . dgrid2 . Items . Clear ( );
            win . dgrid2 . ItemsSource = bvm2;
            sw . Restart ( );
            if ( bvm . Count > 0 )
            {
                foreach ( BankAccountViewModel item in Utils . ReadGenericCollection ( bvm ) )
                {
                    bvm2 . Add ( item );
                    win . dgrid2 . Refresh ( );
                }
            }
            sw . Stop ( );
            Duration2= $"{sw . ElapsedMilliseconds} ms";
            InfoPanel = $"grid2 reloaded with Generic iteration values...";
        }

        private void IterateCollection ( )
        {
            // How to use the IEnumerable collecton's Iterator
            BankAccountViewModel bvrecord = new BankAccountViewModel ( );
            IEnumerator ie = bvm . GetEnumerator ( );
            while ( true )
            {
                if ( ie . MoveNext ( ) )
                {
                    bvrecord = ie . Current as BankAccountViewModel;
                    Console . WriteLine ( bvrecord . CustNo );
                }
                else break;
            }
            InfoPanel = $"grid2 reloaded using Enumeration...";
        }
        #endregion Iteration example

        //private void CloseAllBtn_Click ( object sender , RoutedEventArgs e )
        //{
        //    Application . Current . Shutdown ( );
        //}

        //private void Window_Closed ( object sender , EventArgs e )
        //{
        //    //            UpdateBankRecord -= YieldWindow_UpdateBankRecord;
        //}

        public  void Stack2_PreviewButton (int i )
        {
            stackorder = i;
            stack2_Click ( );
        }

        //private int stackorder = 0;
        private void stack2_Click ( )
        {
            // Show stack content Ascending
            if ( gstack . Count ( ) > 0 )
            {
                win.dgrid2 . ItemsSource = null;
                win . dgrid2 . Items . Clear ( );
                bvm2 . Clear ( );
                int lim1 = limit1==0 ? 1 : limit1 ;
                int lim2 = limit2 ==0? 4 : limit2;
                sw . Restart ( );
                if ( stackorder == 1 )
                {
                    foreach ( BankAccountViewModel item in gstack . BottomToTop )
                    {
                        if ( item . AcType >= lim1 && item . AcType <= lim2 )
                            bvm2 . Add ( item );
                    }
                }
                else
                {
                    foreach ( BankAccountViewModel item in gstack . TopToBottom )
                    {
                        if ( item . AcType >= lim1 && item . AcType <= lim2 )
                            bvm2 . Add ( item );
                    }
                }
                //Reset sort order for stack
                string sort = stackorder == 0 ? "Ascending" : "Descending";
                stackorder = 0;
                win.dgrid2 . ItemsSource = bvm2;
                win . dgrid2 . SelectedIndex = 0;
                win . dgrid2 . SelectedItem = 0;
                win . dgrid2 . Refresh ( );
                sw . Stop ( );
                Counter1 = bvm2 . Count . ToString ( );
                Duration2= $"{sw . ElapsedMilliseconds} ms";
                InfoPanel = $"Loaded grid {sort} from Stack in {sw . ElapsedMilliseconds} ms";
                BankAccountViewModel ba = win . dgrid2 . SelectedItem as BankAccountViewModel;
                // Not available in c# 7.3 !!!!!  use 9.0 - which I cannot do
                //var usertype = new ( ba . CustNo , ba . BankNo , ba . Balance );
            }
            else
            {
                InfoPanel= $"Stack needs to be reLoaded ....";
            }
        }
        private void ExecuteStack2_Preview ( object obj )
        {
            Stack2_PreviewButton ( 2 );
        }

        //#region Delegate Testing
        //public bool DoCompare ( BankAccountViewModel a , BankAccountViewModel b )
        //{
        //    if ( a == null || b == null )
        //        return false;
        //    if ( a . Equals ( b ) )
        //        return true;
        //    return false;
        //}
        //public int MyFunc2 ( int a , int b )
        //{
        //    if ( a > b )
        //        return 0;
        //    else
        //        return 1;
        //}
        //public bool test ( Delegates . MyFunc mf , int a , int b , int c )
        //{
        //    int x = mf ( 100 , b );
        //    return x > 0 ? false : true;
        //}
        //#endregion Delegate Testing

    }
}

