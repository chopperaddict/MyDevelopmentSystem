namespace MyDev. Views
{
    /// <summary>
    /// Used by Expander.XAML
    /// This is basically the template for creating individual items that are added to the public 
    /// List<ApplicationListItem> containing each of the  items in each Expander group
    /// 
    /// Seems a bit convolute to me, as it could be done directly in ListItems ???
    /// </summary>
    public class ApplicationListItem
    {
        public ApplicationListItem ( string name, string openButtonText, string updateButtonText,
           bool updateButtonVisibility = false, bool progressVisibility = true )
        {
            Name = name;
            OpenButtonText = openButtonText;
            UpdateButtonText = updateButtonText;
            UpdateButtonVisibility = updateButtonVisibility;
            ProgressVisibility = progressVisibility;
        }

        public string Name { get; set; }
        public string OpenButtonText { get; set; }
        public string UpdateButtonText { get; set; }
        public bool UpdateButtonVisibility { get; set; }
        public bool ProgressVisibility { get; set; }
    }
}
