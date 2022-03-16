using MyDev . ViewModels;

using System . Collections . ObjectModel;
using System . ComponentModel;

namespace MyDev . ViewModels
{
	/// <summary>
	/// Interaction logic for TreeViews.xaml
	/// </summary>
	public class MenuItem : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public ObservableCollection<MenuItem> Items { get; set; }
		//public MenuItem ( )
		//{
		//	this . Items = new ObservableCollection<MenuItem> ( );
		//}
		private string title;
		public string Title
		{
			get { return title; }
			set { title = value; }
		}
		private bool isSelected;
		public bool IsSelected
		{
			get { return isSelected; }
			set { isSelected = value; }// OnPropertyChanged ( IsSelected . ToString ( ) ); }
		}
		private bool isExpanded;
		public bool IsExpanded
		{
			get { return isExpanded; }
			set { isExpanded = value; }//. OnPropertyChanged ( IsExpanded . ToString ( ) ); }
		}
		public MenuItem ( string Title)
		{
			this . Items = new ObservableCollection<MenuItem> ( );
			this.title = Title;
		}

		protected void OnPropertyChanged ( string PropertyName )
		{
			if ( null != PropertyChanged )
			{
				PropertyChanged ( this ,
					  new PropertyChangedEventArgs ( PropertyName ) );
			}
		}

		//		public ExplorerClass myfolder = new ExplorerClass();
	}
}
