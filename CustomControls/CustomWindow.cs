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

using MyDev . ViewModels;

namespace MyDev . CustomControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MyDev.CustomControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MyDev.CustomControls;assembly=MyDev.CustomControls"
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
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class CustomWindow : Window
    {
        public override void OnApplyTemplate ( )
        {
            Button minimizeButton = GetTemplateChild ( "minimizeButton" ) as Button;
            if ( minimizeButton != null )
                minimizeButton . Click += MinimizeClick;

            Button restoreButton = GetTemplateChild ( "restoreButton" ) as Button;
            if ( restoreButton != null )
                restoreButton . Click += RestoreClick;

            Button closeButton = GetTemplateChild ( "closeButton" ) as Button;
            if ( closeButton != null )
                closeButton . Click += CloseClick;

            base . OnApplyTemplate ( );
        }
        private void moveRectangle_PreviewMouseDown ( object sender , MouseButtonEventArgs e )
        {
            if ( Mouse . LeftButton == MouseButtonState . Pressed )
                DragMove ( );
        }
        #region Click events
        protected void MinimizeClick ( object sender , RoutedEventArgs e )
        {WindowState = WindowState . Minimized;}

        protected void RestoreClick ( object sender , RoutedEventArgs e )
        {WindowState = ( WindowState == WindowState . Normal ) ? WindowState . Maximized : WindowState . Normal;}

        protected void CloseClick ( object sender , RoutedEventArgs e )
        {Close ( );}
        #endregion
    }
}
