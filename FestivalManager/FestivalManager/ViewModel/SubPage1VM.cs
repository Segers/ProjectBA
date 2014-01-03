using Oefening1.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _FestivalManager.ViewModel
{
    class SubPage1VM : ObservableObject, IPage
    {

        public string Name
        {
            get { return "SubPage1"; }
        }
    }
}
