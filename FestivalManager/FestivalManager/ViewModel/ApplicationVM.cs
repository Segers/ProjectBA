using GalaSoft.MvvmLight.Command;
using Oefening1.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _FestivalManager.ViewModel
{
    class ApplicationVM : ObservableObject
    {

        public ApplicationVM()
        {
            _pages = new List<IPage>();

            //hieronder pages toevoegen
            _pages.Add(new HomePageVM());
            _pages.Add(new SubPage1VM());

            //huidige page instellen
            CurrentPage = Pages[0];

        }

        private IPage _currentPage;
        public IPage CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        private List<IPage> _pages;
        public List<IPage> Pages
        {
            get
            {
                return _pages;
            }
        }


        //een command nodig om een andere user control als de huidig zichtbare user control aan te geven.
        public ICommand ChangePageCommand
        {
            get { return new RelayCommand<IPage>(ChangePage); }
        }


        private void ChangePage(IPage page)
        {
            CurrentPage = page;
        }

    }
}
