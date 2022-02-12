using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading;
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
	/// Interaction logic for Progressbar.xaml
	/// </summary>
	public partial class Progressbar : Window
	{
		public bool AutoRun { get; set; }
		public bool AutoStop { get; set; }
		public Progressbar ( )
		{
			InitializeComponent ( );
			//			ValueChanged +=
			Progbar . Value = 0;
			Progvalue = Progbar . Value;
			this . Show ( );
			Percent . BringIntoView ( );
			AutoRun = false;
		}

		//private void Progbar_ValueChanged1 ( object sender , RoutedPropertyChangedEventArgs<double> e )
		//{
		//	throw new NotImplementedException ( );
		//}

		private void Timercall ( )
		{
			timerProgress ( );
		}
		private void timerProgress ( )
		{
			if ( Progvalue < 100 )
			{
				Thread . Sleep ( 250 );
				Progvalue++;//= Progbar . Value;
				Progbar . Value = Progvalue;
				Progbar_Update ( );
			}
			if ( AutoRun )
			{
				Timercall ( );
			}
		}

		private void Progbar_Update ( )
		{
			//ProgressBar pb = sender as ProgressBar;
			if ( Progvalue == 0 )
			{
				Progbar . Refresh ( );
				return;
			}
			if ( Progvalue == 100 )
			{
				startstop . IsEnabled = true;
				Progbar . Value = 0;
				Progvalue = 0;
			}
			Progbar . Value = Progvalue;
			Progbar . Refresh ( );
			this . Refresh ( );
		}

		private void Button_Click ( object sender , RoutedEventArgs e )
		{
			// Auto 
			Button btn = sender as Button;
			Progbar . Refresh ( );
			Progvalue = 1;
			AutoRun = true;
			btn . IsEnabled = false;
			TriggerAuto ( );
		}
		private void TriggerAuto ( )
		{
			do
			{
				if ( AutoStop )
				{
					AutoStop = false;
					break;
				}
				if ( Progvalue >= 100 || Progvalue == 0 )
					break;
				Thread . Sleep ( 250 );
				Progvalue++;
				Progbar_Update ( );
			} while ( true );
		}
		private void Button1_Click ( object sender , RoutedEventArgs e )
		{
			// single increment
			AutoRun = false;
			Progbar . Refresh ( );
			Progvalue = 1;
			Progbar_Update ( );
		}

		private void Stop_Click ( object sender , RoutedEventArgs e )
		{
			Button btn = sender as Button;
			btn . IsEnabled = true;
			AutoStop = true;
		}


		public double Progvalue
		{
			get { return ( double ) GetValue ( ProgvalueProperty ); }
			set { SetValue ( ProgvalueProperty , value ); }
		}
		public static readonly DependencyProperty ProgvalueProperty =
		    DependencyProperty.Register("Progvalue", typeof(double), typeof(Progressbar), new PropertyMetadata((double) 0));

		private void progbar_KeyDown ( object sender , KeyEventArgs e )
		{
			if ( e . Key == Key . Escape )
				AutoStop = true;
		}
	}
}
