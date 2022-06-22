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
using System . Windows . Navigation;
using System . Windows . Shapes;

namespace MyDev . UserControls
{
    /// <summary>
    /// Interaction logic for SpinnerUserControl.xaml
    /// </summary>
    public partial class SpinnerUserControl : UserControl
    {
        public SpinnerUserControl ( )
        {
            InitializeComponent ( );
            DataContext = this;
            IsShown = true;
            Diameter = 50;
            GapPercent = 35;

        }



        public Brush Fill
        {
            get { return ( Brush ) GetValue ( FillProperty ); }
            set { SetValue ( FillProperty , value ); }
        }
       public static readonly DependencyProperty FillProperty =
            DependencyProperty . Register ( "Fill" , typeof ( Brush ) , typeof ( SpinnerUserControl ) , new PropertyMetadata ( Brushes.Black) );
        public bool IsShown
        {
            get { return ( bool ) GetValue ( IsShownProperty ); }
            set { SetValue ( IsShownProperty , value ); }
        }
        public static readonly DependencyProperty IsShownProperty =
             DependencyProperty . Register ( "IsShown" , typeof ( bool ) , typeof ( SpinnerUserControl ) , new PropertyMetadata ( ( bool ) true ) );
        public double Diameter
        {
            get { return ( double ) GetValue ( DiameterProperty ); }
            set { SetValue ( DiameterProperty , value ); }
        }
        public static readonly DependencyProperty DiameterProperty =
            DependencyProperty . Register ( "Diameter" , typeof ( double ) , typeof ( SpinnerUserControl ) , new PropertyMetadata ( ( double ) 85.0 ) );
        public double StrokeThickness
        {
            get { return ( double ) GetValue ( StrokeThicknessProperty ); }
            set { SetValue ( StrokeThicknessProperty , value ); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty . Register ( "StrokeThickness" , typeof ( double ) , typeof ( SpinnerUserControl ) , new PropertyMetadata ( ( double ) 5.0 ) );
        public double GapPercent
        {
            get { return ( double ) GetValue ( GapPercentProperty ); }
            set { SetValue ( GapPercentProperty , value ); }
        }
        public static readonly DependencyProperty GapPercentProperty =
            DependencyProperty . Register ( "GapPercent" , typeof ( double ) , typeof ( SpinnerUserControl ) , new PropertyMetadata ( ( double ) 15.0 ) );
        public PenLineCap Cap
        {
            get { return ( PenLineCap ) GetValue ( CapProperty ); }
            set { SetValue ( CapProperty , value ); }
        }
        public static readonly DependencyProperty CapProperty =
            DependencyProperty . Register ( "Cap" , typeof ( PenLineCap ) , typeof ( SpinnerUserControl ) , new PropertyMetadata ( PenLineCap . Triangle ) );

        private void SpinnerParent_Loaded ( object sender , RoutedEventArgs e )
        {

        }

        private void SpinnerParent_MouseDown ( object sender , MouseButtonEventArgs e )
        {

        }
    }
}
