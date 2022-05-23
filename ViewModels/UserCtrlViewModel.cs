using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Input;
using System . Windows . Media . Imaging;

using MyDev . UserControls;
using MyDev . Views;


namespace MyDev . ViewModels
{
    public class UserCtrlViewModel
    {
        public static UserCtrlViewModel ucvm { get; set; }
        public static UserControlsViewer UcCtrlsViewer { get; set; }
        private ContentControl UCContentcontrol { get; set; }
        public UCListBox uclistbox { get; set; }
        public UcHostWindow uhw { get; set; }

        public UserCtrlViewModel ( )
        {
            ucvm = this;

            // This is the actual viewer window that hosts the listbox User control initially
            UcHostWindow uhw = new UcHostWindow ( );
            uhw . Show ( );

            //This is the  full "window" we are going t host in our viewer window above
            //UcCtrlsViewer = new UserControlsViewer ( );
            //// load  control into viewer window
            //uhw . UCHostContent . Content = UcCtrlsViewer;
            //// Now load its own contentcontrol with listbox
            //uclistbox = new UCListBox ( );
            //UcCtrlsViewer . Contentctrl . Content = uclistbox;
        //    UcCtrlsViewer . Command2 . Content = "Hide List of Controls";
        }


        public static UserCtrlViewModel GetUserCtrlViewModel ( )
        { return ucvm; }
    }
}
