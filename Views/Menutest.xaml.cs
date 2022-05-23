using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . ComponentModel . Design;
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

using MyDev . ViewModels;

using MyDev . Views;

namespace MyDev . Views
{
    public partial class Menutest : Window
    {
        public ObservableCollection<MenuTestViewModel> MenuItems { get; set; }
        public Menutest ( )
        {
            InitializeComponent ( );

            MenuItems = new ObservableCollection<MenuTestViewModel>
            {
                new MenuTestViewModel 
                { 
                    Header = "Alpha" ,
                    MenuItems = new ObservableCollection<MenuTestViewModel>
                        {
                            new MenuTestViewModel { Header = "Alpha1" },
                            new MenuTestViewModel { Header = "Alpha2" }
                            }
                 },
                new MenuTestViewModel
                {
                    Header = "Beta" ,
                    MenuItems = new ObservableCollection<MenuTestViewModel>
                        {
                            new MenuTestViewModel { Header = "Beta1" },
                            new MenuTestViewModel { Header = "Beta2",
                                MenuItems = new ObservableCollection<MenuTestViewModel>
                                {
                                    new MenuTestViewModel { Header = "Beta1a" },
                                    new MenuTestViewModel { Header = "Beta1b" },
                                    new MenuTestViewModel { Header = "Beta1c" }
                                }
                            },
                            new MenuTestViewModel { Header = "Beta3" }
                        }
                },
                new MenuTestViewModel { Header = "Gamma" }
            };
            DataContext = this;
            //}
        }
    }

}