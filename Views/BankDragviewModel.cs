using MyDev . ViewModels;

namespace MyDev . Views
{
	public partial class DragDropClient
	{
		//	public delegate string QualifyingFileLocations ( string [ ] possiblefolders, string searchfilename );

		#region Various DragView Type Declarations

		/// <summary>
		/// Interaction logic for DragDropClient.xaml
		/// </summary>
		/// 
		public class BankDragviewModel : BankAccountViewModel
		{
			public string RecordType
			{
				get; set;
			}
			public override string ToString ( )
			{
				return base . ToString ( );
			}

		}
		#endregion
	}
}


