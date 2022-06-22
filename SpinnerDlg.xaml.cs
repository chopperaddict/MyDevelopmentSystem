using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Data;
using System . Windows . Documents;
using System . Windows . Input;
using System . Windows . Media;
using System . Windows . Media . Imaging;
using System . Windows . Shapes;

using MyDev . UserControls;
using MyDev . Views;

namespace MyDev
{
    /// <summary>
    /// Interaction logic for SpinnerDlg.xaml
    /// </summary>
    public partial class SpinnerDlg : Window
    {
        private Window _parent { get; set; }
        private static Window ThisWin { get; set; }
            MessageListener msglistner { get; set; }
        public SpinnerDlg (Window parent = null)
        {
            InitializeComponent ( );
            ThisWin = this;
            //this.Show ( );
            _parent = parent;
        }

        public  void SetSize (double height, double width, double diameter, double gap )
        {
            ThisWin . Height = height;
            ThisWin . Width= width;
            this . Diameter = diameter;
            this . GapPercent = gap;
        }
        public bool IsShown
        {
            get { return ( bool ) GetValue ( IsShownProperty ); }
            set { SetValue ( IsShownProperty , value ); }
        }
        public static readonly DependencyProperty IsShownProperty =
             DependencyProperty . Register ( "IsShown" , typeof ( bool ) , typeof ( SpinnerDlg ) , new PropertyMetadata ( ( bool ) true ) );
        public double Diameter
        {
            get { return ( double ) GetValue ( DiameterProperty ); }
            set { SetValue ( DiameterProperty , value ); }
        }
        public static readonly DependencyProperty DiameterProperty =
            DependencyProperty . Register ( "Diameter" , typeof ( double ) , typeof ( SpinnerDlg ) , new PropertyMetadata ( ( double ) 85.0 ) );
        public double StrokeThickness
        {
            get { return ( double ) GetValue ( StrokeThicknessProperty ); }
            set { SetValue ( StrokeThicknessProperty , value ); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty . Register ( "StrokeThickness" , typeof ( double ) , typeof ( SpinnerDlg ) , new PropertyMetadata ( ( double ) 5.0 ) );
        public double GapPercent
        {
            get { return ( double ) GetValue ( GapPercentProperty ); }
            set { SetValue ( GapPercentProperty , value ); }
        }
        public static readonly DependencyProperty GapPercentProperty =
            DependencyProperty . Register ( "GapPercent" , typeof ( double ) , typeof ( SpinnerDlg ) , new PropertyMetadata ( ( double ) 15.0 ) );
        public PenLineCap Cap
        {
            get { return ( PenLineCap ) GetValue ( CapProperty ); }
            set { SetValue ( CapProperty , value ); }
        }
        public static readonly DependencyProperty CapProperty =
            DependencyProperty . Register ( "Cap" , typeof ( PenLineCap ) , typeof ( SpinnerDlg ) , new PropertyMetadata ( PenLineCap . Triangle ) );

        private void SpinnerParent_Loaded ( object sender , RoutedEventArgs e )
        {
          
        }

        private void SpinnerParent_MouseDown ( object sender , MouseButtonEventArgs e )
        {
            //xspinner . IsShown = false;
        }

        private void CheckBox_Checked ( object sender , RoutedEventArgs e )
        {
            //if ( spinner . IsShown )
            //    spinner . IsShown = false;
            //else
            //    spinner . IsShown = true;
        }

        private void Button_Click ( object sender , RoutedEventArgs e )
        {

        }

        private void Button_Close ( object sender , RoutedEventArgs e )
        {
            this . Close ( );
        }
    }
}
