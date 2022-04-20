using System . ComponentModel;

namespace MyDev . ViewModels
{
    public class Notifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged ( string propertyName )
        {
            var handler = PropertyChanged;
            if ( handler != null )
                handler ( this , new PropertyChangedEventArgs ( propertyName ) );
        }
    }

}
