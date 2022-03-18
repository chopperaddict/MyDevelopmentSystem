﻿using MyDev . Dapper;
using MyDev . Models;
using MyDev . Sql;
using MyDev . SQL;
using MyDev . UserControls;
using MyDev . ViewModels;

using System;
using System . Collections . Generic;
using System . Collections . ObjectModel;
using System . Configuration;
using System . Data . SqlClient;
using System . Diagnostics;
using System . Runtime . CompilerServices;
using System . Runtime . InteropServices . WindowsRuntime;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Data;
using System . Windows . Input;
using System . Windows . Media;

namespace MyDev . Views
{
	/// <summary>
	/// Interaction logic for BankGridView.xaml
	/// This is my FIRST MVVM project so this only contains the most BASIC
	/// Code Behind.
	/// All the actual work is performed in the VIEWMODEL file - MVVMGRIDMODEL.CS
	/// </summary>
	public partial class MvvmDataGrid : Window
	{
		// Variables  for the Classes in use
//		public static MvvmGridModel MvvmGridmodel { get; set; }
		private MvvmViewModel MvvM { get; set; }
		public static MvvmDataGrid BankGridViewWindow { get; set; }

		public  FlowdocLib fdl = new FlowdocLib();
		public FlowDoc fdoc = new FlowDoc();
		private object movingobject { get; set; }
		public object MovingObject
		{
			get { return movingobject; }
			set { movingobject = value; }
		}

		private double CpFirstXPos=0;
		private double CpFirstYPos=0;
	
		public MvvmDataGrid ( )
		{
			// WORKS WELL   			
			InitializeComponent ( );

			// Get  the base class loaded into variable
		}

		private void BankGV_Loaded ( object sender , RoutedEventArgs e )
		{
			MvvM = new MvvmViewModel ( this );
			this . DataContext = MvvM;

			//			MvvmGridmodel = MvvM . mvgm;
			//this . DataContext = MvvmGridmodel;
			MvvM . Flowdoc = Flowdoc;
			canvas . Height = this . Height;
			canvas . Width= this . Width;
			MvvM . canvas = canvas;

			// Sets it to x:Name "BankGV"
			BankGridViewWindow = this;
//			Utils . SetupWindowDrag ( this );
			MvvM . MovingObject = MovingObject;
			// FlowDoc support
			Listviews lv = new Listviews();
			Flowdoc . ExecuteFlowDocBorderMethod += lv.FlowDoc_ExecuteFlowDocBorderMethod;
			Flowdoc . ExecuteFlowDocMaxmizeMethod += new EventHandler ( MaximizeFlowDoc );
			this . SizeChanged += MvvmDataGrid_SizeChanged;
			canvas . Visibility = Visibility . Visible;

		}

		private void MvvmDataGrid_SizeChanged ( object sender , SizeChangedEventArgs e )
		{
			canvas . Width = this . Width;
			canvas . Height = this . Height;
			canvas . SetValue ( HeightProperty , ( object ) MainGrid . Height );
			canvas . SetValue ( WidthProperty , ( object ) MainGrid . Width );
		}
		private void BankGV_KeyDown ( object sender , KeyEventArgs e )
		{
			if ( e . Key == Key . F8 )
				Debugger . Break ( );
		}

		private void acfiltertext_TextChanged ( object sender , TextChangedEventArgs e )
		{

		}

		private void GetColumnNamesBtn_Click ( object sender , RoutedEventArgs e )
		{
			List<string> list = new List<string>();
			ObservableCollection<GenericClass> GenericClass = new ObservableCollection<GenericClass>(); 
			Dictionary<string, string> dict = new Dictionary<string, string>();
			// This returns a Dictionary<sting,string> PLUS a collection  and a List<string> passed by ref....
			dict = GenericDbHandlers . GetDbTableColumns ( ref GenericClass,  ref list, "BankAccount" , "IAN1");
			
			SqlServerCommands . LoadActiveRowsOnlyInGrid ( dataGrid2 , GenericClass , DapperSupport . GetGenericColumnCount ( GenericClass ) );
			if ( Flags . ReplaceFldNames )
			{
				GenericDbHandlers . ReplaceDataGridFldNames ( "BankAccount" , ref dataGrid2 );
			}
		}
		private void dataGrid_PreviewMouseRightButtonDown ( object sender , MouseButtonEventArgs e )
		{
			MvvM . ShowRecordData ( dataGrid);
		}
	
	#region Flowdoc support via library
		protected void MaximizeFlowDoc ( object sender , EventArgs e )
		{
			// Clever "Hook" method that Allows the flowdoc to be resized to fill window
			// or return to its original size and position courtesy of the Event declard in FlowDoc
			fdl . MaximizeFlowDoc ( Flowdoc , canvas , e );
		}
		private void Flowdoc_MouseLeftButtonUp ( object sender , MouseButtonEventArgs e )
		{
			// Window wide  !!
			// Called  when a Flowdoc MOVE has ended
			MovingObject = fdl . Flowdoc_MouseLeftButtonUp ( sender , Flowdoc , MovingObject , e );
			ReleaseMouseCapture ( );
		}
		// CALLED WHEN  LEFT BUTTON PRESSED
		private void Flowdoc_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
		{
			//In this event, we get current mouse position on the control to use it in the MouseMove event.
			MovingObject = fdl . Flowdoc_PreviewMouseLeftButtonDown ( sender , Flowdoc , e );
			Console . WriteLine ($"MvvmDataGrid Btn down {MovingObject}");
		}
		private void Flowdoc_MouseMove ( object sender , MouseEventArgs e )
		{
			// We are Resizing the Flowdoc using the mouse on the border  (Border.Name=FdBorder)
				fdl . Flowdoc_MouseMove ( Flowdoc , canvas , MovingObject , e );
		}
		// Shortened version proxy call		
		private void Flowdoc_LostFocus ( object sender , RoutedEventArgs e )
		{
			Flowdoc . BorderClicked = false;
		}
		public void FlowDoc_ExecuteFlowDocBorderMethod ( object sender , EventArgs e )
		{
			// EVENTHANDLER to Handle resizing
			FlowDoc fd = sender as FlowDoc;
			Point pt = Mouse . GetPosition (canvas );
			double dLeft = pt.X;
			double dTop= pt.Y;
		}
		public void fdmsg ( string line1 , string line2 = "" , string line3 = "" )
		{
			//We have to pass the Flowdoc.Name, and Canvas.Name as well as up   to 3 strings of message
			//  you can  just provie one if required
			// eg fdmsg("message text");
			fdl . FdMsg ( Flowdoc , canvas , line1 , line2 , line3 );
		}

		#endregion Flowdoc support via library

		private void DoDragMove ( object sender)
		{
			//Handle the button NOT being the left mouse button
			// which will crash the DragMove Fn.....
			var inst = sender as Window;
			try
			{
			//	inst. DragMove ( );
			}
			catch
			{
				return;
			}
		}
		private void BankGV_Closed ( object sender , EventArgs e )
		{
			Listviews lv = new Listviews();
			Flowdoc . ExecuteFlowDocBorderMethod -= lv . FlowDoc_ExecuteFlowDocBorderMethod;
			Flowdoc . ExecuteFlowDocMaxmizeMethod -= new EventHandler ( MaximizeFlowDoc );

		}

		private void BankGV_PreviewMouseLeftButtonDown ( object sender , MouseButtonEventArgs e )
		{
			var ee = sender;
			DoDragMove (sender );
		}
	}
}

