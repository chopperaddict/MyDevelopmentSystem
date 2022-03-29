using System. Collections. Generic;


/// <summary>
/// Used by Expander.XAML
/// </summary>
namespace MyDev. Views
{
    public class ListItem
    {
        #region declarations
        public string Title { get; set; }
        public List<ApplicationListItem> ListItems { get; set; } = new List<ApplicationListItem> ( );
        public ListItem ( string title )
        {
            Title = title;
        }
        #endregion declarations

        public void AddListItem ( ApplicationListItem listItem )
        {
            ListItems. Add ( listItem );
        }
    }
}
