using MyDev . ViewModels;

namespace MyDev . Views
{
	public partial class DragDropClient
	{
		public class DetailsDragviewModel : DetailsViewModel
		{
			public string RecordType
			{
				get; set;
			}
			public override string ToString ( )
			{
				return base . ToString ( );
			}
			public DetailsDragviewModel ( )
			{
			}
		}
	}
}


