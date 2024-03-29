﻿using System;
using System . Collections . Generic;
using System . IO;
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

namespace MyDev . UserControls
{
    /// <summary>
    /// Interaction logic for MvvmImageUC.xaml
    /// </summary>
    public partial class MvvmImageUC 
    {

        public Image CurrentImage { get; set; }
        public bool LeftMouseDown { get; set; }
        public static MvvmContainerViewModel mcvm { set; get; }
        public static MvvmImageUC mvvmImageuc { get; set; }
        public MvvmImageUC ( )
        {
            InitializeComponent ( );
            mcvm = MvvmContainerViewModel . GetMvvmContainerViewModel ( );
            mvvmImageuc = this;
        }

        private void Image_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
        {

        }
        public void  GetListOfImages()
        {
            string path = @"C:\\Users\ianch\pictures\";
            var imagefiles = Directory.GetFiles ( path);
            // Get pointer to listbox Viewmodel
            MvvmListboxUC  listboxuc = MvvmListboxUC . GetListBoxUc ( );
        }
        private void Grid_PreviewMouseMove ( object sender , MouseEventArgs e )
        {
            //pt = GetMousePosition ( Utils . FindVisualParent<Window> ( this ) , "WINDOW" );

            //var ctrl = Utils . FindVisualParent<Window> ( sender as UIElement );

            ////pt = GetMousePosition ( sender as UIElement , "CONTROL" );


         Image img =  Utils . FindVisualParent<Image> ( sender as UIElement );
            Thickness th = new Thickness ( );
            th = img . Margin;
            if ( LeftMouseDown )
            {
                Point pt = GetMousePosition ( Utils . FindVisualParent<Window> ( sender as Image) , "IMAGE" );
                th . Left = pt . X;
                th . Top = pt . Y;
                Console . WriteLine ( $"Mouse mmmmmmmove {pt.X} , {pt.Y}. th={th.Left},{th.Top}, {th.Right}, {th.Bottom}.........." );
                 img . Margin = th;
            }
        }
        private Point GetMousePosition ( object window , string mode = "SCREEN" )
        {
            var position = new Point ( );
            Window win = new Window ( );
            // Position of the mouse relative to the Screen
            // and allows for the window being moved around as well
            if ( mode . ToUpper ( ) == "SCREEN" )
            {
                win = window as Window;
                position = new Point ( Mouse . GetPosition ( win ) . X + win . Left , Mouse . GetPosition ( win ) . Y + win . Top );
                Console . WriteLine ( $"Mouse to Screen X = {position . X}, Y = {position . Y}" );
            }
            // Position of the mouse relative to the window
            else if ( mode . ToUpper ( ) == "WINDOW" )
            {
                win = window as Window;
                position = Mouse . GetPosition ( win );
                Console . WriteLine ( $"Mouse to Window  {win . ToString ( )} X = {position . X}, Y = {position . Y}" );
            }
            else if ( mode . ToUpper ( ) == "IMAGE" )
            {
                Image ctrl = window as Image;
                position = Mouse . GetPosition ( ctrl );
                //Console . WriteLine ( $"Mouse to Image {ctrl . ToString ( )} X = {position . X}, Y = {position . Y}" );
            }
            else
            {
                win = window as Window;
                // converts to screen position of specified window
                position = win . PointToScreen ( Mouse . GetPosition ( win ) );
                Console . WriteLine ( $"Screen from Window {win . ToString ( )} X = {position . X}, Y = {position . Y}" );
            }
            // Add the window position
            return position;
            //            return new Point ( position . X + win . Left , position . Y + win . Top );
        }

        private void Image_IsMouseDirectlyOverChanged ( object sender , DependencyPropertyChangedEventArgs e )
        {
            var ctrl = sender as UIElement;
            string objstring = ctrl . ToString ( );
            if ( objstring . Contains ( ".Image" ) )
            {
                Image img = ctrl as Image;
                string name = img . Name;
                if ( img . GetType ( ) . Equals ( typeof ( Image ) ) )
                {
                    GetMousePosition ( img , "IMAGE" );
                }
//                UIElement CurrentImage = Utils . FindVisualParent<UIElement> ( ctrl ) as UIElement;
//                GetMousePosition ( img , "CONTROL" );

            }
            else if ( objstring . Contains ( "pack://" ) )
            {
                Image CurrentImage = Utils . FindVisualParent<UIElement> ( ctrl ) as Image;
                GetMousePosition ( ctrl , "CONTROL" );
            }
            else
                GetMousePosition ( ctrl , "CONTROL" );
        }

        private void Image1_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
        {
            LeftMouseDown = true;
        }

        private void Image1_PreviewMouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
        {
            LeftMouseDown = false;
        }
    }
}
