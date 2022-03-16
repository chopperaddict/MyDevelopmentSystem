using Microsoft . Win32;

using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
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
using System . Xml . Linq;

using static System . Net . Mime . MediaTypeNames;

namespace MyDev . UserControls
{
	/// <summary>
	/// Interaction logic for Ucontrol1.xaml
	/// </summary>
	public partial class Ucontrol1 :UserControl
	{
		ObservableCollection<string> Strings = new ObservableCollection<string>();

		string[] strings={
		"asdsafdafda",
		"hjhkghkg",
		"hjkjkhgkgk"};

		public Ucontrol1 ( )
		{
			InitializeComponent ( );
			this . SizeChanged += Ucontrol1_SizeChanged;
			this . DataContext = this;
			//listbox1 . Items . Clear ( );
			//listbox1 . Items . Add ( strings [ 0 ]);
			//listbox1 . Items . Add ( strings [ 1 ]);
			//listbox1 . Items . Add ( strings [ 2 ]);
			//listbox1 . ItemsSource = Strings;
		}

		private void Ucontrol1_SizeChanged ( object sender , SizeChangedEventArgs e )
		{
			//if( MainGrid . ActualHeight  > 0)
			//	listbox1.Height = MainGrid.ActualHeight;
		}

		private void U1Ctrl_Loaded ( object sender , RoutedEventArgs e )
		{
			string str;
			string[] buffer;
			//StringBuilder sb = new StringBuilder();
			int indx=0, offset=0;
			str = File . ReadAllText ( @"C:\\Users\ianch\Documents\library1 functions.txt");
			
			buffer = str . Split ( '\n' );
			foreach ( var item in buffer)
			{
				if(item.Length > 0)
					listbox1 . Items . Add ( item.Substring(0, item.Length-1) );
			}
		}
	}
}
