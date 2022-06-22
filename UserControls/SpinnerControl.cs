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
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MyDev.UserControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MyDev.UserControls;assembly=MyDev.UserControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:SpinnerControl/>
    ///
    /// </summary>
    public class SpinnerControl : Control
    {
        static SpinnerControl ( )
        {DefaultStyleKeyProperty . OverrideMetadata ( typeof ( SpinnerControl ) , new FrameworkPropertyMetadata ( typeof ( SpinnerControl ) ) );}
        public Brush Fill
        {
            get { return ( Brush ) GetValue ( FillProperty ); }
            set { SetValue ( FillProperty , value ); }
        }
        public static readonly DependencyProperty FillProperty =
            DependencyProperty . Register ( "Fill" , typeof ( Brush ) , typeof ( SpinnerControl ) , new PropertyMetadata ( Brushes.Red) );
        public bool  IsShown
        {
            get { return ( bool  ) GetValue ( IsShownProperty ); }
            set { SetValue ( IsShownProperty , value ); }
        }
       public static readonly DependencyProperty IsShownProperty =
            DependencyProperty . Register ( "IsShown" , typeof ( bool  ) , typeof ( SpinnerControl ) , new PropertyMetadata ( (bool)false ) );
        public double Diameter
        {
            get { return ( double ) GetValue ( DiameterProperty ); }
            set { SetValue ( DiameterProperty , value ); }
        }
        public static readonly DependencyProperty DiameterProperty =
            DependencyProperty . Register ( "Diameter" , typeof ( double ) , typeof ( SpinnerControl ) , new PropertyMetadata ( (double)85.0 ) );
       public double StrokeThickness
        {
            get { return ( double ) GetValue ( StrokeThicknessProperty ); }
            set { SetValue ( StrokeThicknessProperty , value ); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty . Register ( "StrokeThickness" , typeof ( double ) , typeof ( SpinnerControl ) , new PropertyMetadata ( (double)5.0 ) );
        public double GapPercent
        {
            get { return ( double ) GetValue ( GapPercentProperty ); }
            set { SetValue ( GapPercentProperty , value ); }
        }
        public static readonly DependencyProperty GapPercentProperty =
            DependencyProperty . Register ( "GapPercent" , typeof ( double ) , typeof ( SpinnerControl ) , new PropertyMetadata ( (double)15.0) );


        public PenLineCap Cap
        {
            get { return ( PenLineCap ) GetValue ( CapProperty ); }
            set { SetValue ( CapProperty , value ); }
        }

        // Using a DependencyProperty as the backing store for Cap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CapProperty =
            DependencyProperty . Register ( "Cap" , typeof ( PenLineCap ) , typeof ( SpinnerControl) , new PropertyMetadata ( PenLineCap.Triangle ) );





    }
}

