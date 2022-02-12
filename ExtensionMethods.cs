using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows . Threading;
using System . Windows;

namespace MyDev
{
	public static class ExtensionMethods
	{
		private static Action EmptyDelegate = delegate ( ) { };

		public static void Refresh ( this UIElement uiElement )
		{
			try
			{
				uiElement . Dispatcher . Invoke ( DispatcherPriority . Render , EmptyDelegate );
			}
			catch
			{

			}
		}
	}
}