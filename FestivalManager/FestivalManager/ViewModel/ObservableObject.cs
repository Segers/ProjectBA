using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _FestivalManager.ViewModel
{
    class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //eigen methode (gebaseerd op cursus)
        //deze methode gaan we aanroepen van zodra een property wijzigt
        protected void OnPropertyChanged(string propertyName)
        {
            //controle of event (vuurpijl) beschikbaar is
            if (PropertyChanged != null)
            {
                //vuurpijl afschieten --> merk op: we geven de naam van de
                //property door dat gewijzigd is
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}
