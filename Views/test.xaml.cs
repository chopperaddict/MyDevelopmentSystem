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

namespace MyDev . Views
{
    /// <summary>
    /// Interaction logic for test.xaml
    /// </summary>
    public partial class test : Window
    {
        public test ( )
        {
            InitializeComponent ( );
            Button_Click ( null , null );
        }

        private void Button_Click ( object sender , RoutedEventArgs e )
        {
            //SupportMethods sm = new SupportMethods ();
            SupportMethods . ProcessExecuteRequest ( this , null , textbox1 , $"NotePad.exe " );
            SupportMethods . ProcessExecuteRequest ( this , null , textbox1 , $"Write.exe " );
        }
    }
}
