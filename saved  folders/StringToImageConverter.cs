﻿using MyDev . Views;

using System;
using System . Collections . Generic;
using System . Globalization;
using System . IO;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Data;
using System . Windows . Media . Imaging;

namespace MyDev . Converts
{
	[ValueConversion ( typeof ( string ) , typeof ( BitmapImage ) )]
	public class StringToImageConverter : IValueConverter
	{
		public static StringToImageConverter Instance = new StringToImageConverter();
		public object Convert ( object value , Type targetType , object parameter , CultureInfo culture )
		{
			if ( value == null )
				return value;
			var path = (string)value;
			if ( path == null )
				return null;
			var name = TreeViews.GetFileFolderName(path);
			var image = "/icons/new.ico";

			//if((bool)parameter == true)
			//	image = "/icons/folder.gif";
			FileInfo fi = new FileInfo(path);
			FileAttributes fa =  fi . Attributes;
			string attr = fa . ToString ( );
			if (path.Length == 3  && path.Contains("\\" ))
			{
				image = "/icons/folder.gif";
			}
			else if ( attr . Contains ( "Directory" ) )
			{
				image = "/icons/folder-open.png";
			}
			else
			{
				image = "/icons/templateicon.ico";    // its a fille
			}
			//if ( string . IsNullOrEmpty ( name ) ) // must be a drive
			//	image = "/icons/new.ico";
			//else if ( name . Contains ( "." ) )
			//	image = "/icons/templateicon.ico";    // its a fille
			//else
			//	image = "/icons/folder-open.png";
			//File alone
			Uri uri = new Uri ( $"pack://application:,,,{image}" );
			BitmapImage source = new BitmapImage ( uri );
			return source;
		}

		public object ConvertBack ( object value , Type targetType ,
		    object parameter , CultureInfo culture )
		{
			throw new NotSupportedException ( "Cannot convert back" );
		}
	}
}
