using GalaSoft.MvvmLight.Command;
using Oefening1.ViewModel;
using Project_MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace _FestivalManager.ViewModel
{
    class HomePageVM : ObservableObject, IPage
    {



        #region constructor
        public HomePageVM()
        {
            BandList = Band.GetBands();
            ContactList = Contactperson.GetContacts();
            ContactTypeList = ContactpersonType.GetContactTypes();
            
            GenreList = Genre.GetGenres();
            SelectedTicketType = new TicketType();
            TicketTypeList = TicketType.GetTicketType(SelectedTicketType);
            NewPodium = new Podium();
            NewBand = new Band();
            NewGenre = new Genre();
            NewTicket = new Ticket();
            NewContactType = new ContactpersonType();
            NewContact = new Contactperson();
            TicketList = Ticket.GetTickets();
            PodiumList = Podium.GetPodia();
            FestivalList = Festival.GetFestivalInfo();
            
        }

        public string Name
        {
            get { return "Home"; }
        }

        #endregion

        #region Properties
        private ObservableCollection<Band> _bandList;

        public ObservableCollection<Band> BandList
        {
            get
            {
                return _bandList;
            }
            set
            {
                _bandList = value; OnPropertyChanged("BandList");
            }
        }

        private ObservableCollection<Contactperson> _contactList;

        public ObservableCollection<Contactperson> ContactList
        {
            get
            {
                return _contactList;
            }
            set
            {
                _contactList = value; OnPropertyChanged("ContactList");
            }
        }

        private ObservableCollection<ContactpersonType> _contactTypeList;

        public ObservableCollection<ContactpersonType> ContactTypeList
        {
            get
            {
                return _contactTypeList;
            }
            set
            {
                _contactTypeList = value; OnPropertyChanged("ContactTypeList");
            }
        }


        private ObservableCollection<TicketType> _ticketTypeList;

        public ObservableCollection<TicketType> TicketTypeList
        {
            get
            {
                return _ticketTypeList;
            }
            set
            {
                _ticketTypeList = value; OnPropertyChanged("TicketTypeList"); OnPropertyChanged("TicketList");
            }
        }

        private ObservableCollection<Ticket> _ticketList;

        public ObservableCollection<Ticket> TicketList
        {
            get
            {
                return _ticketList;
            }
            set
            {
                _ticketList = value; OnPropertyChanged("TicketList");
            }
        }

        private ObservableCollection<Podium> _podiumList;

        public ObservableCollection<Podium> PodiumList
        {
            get
            {
                return _podiumList;
            }
            set
            {
                _podiumList = value; OnPropertyChanged("PodiumList");
            }
        }

        private ObservableCollection<Genre> _genreList;

        public ObservableCollection<Genre> GenreList
        {
            get
            {
                return _genreList;
            }
            set
            {
                _genreList = value; OnPropertyChanged("GenreList");
            }
        }

        private ObservableCollection<Festival> _festivalList;

        public ObservableCollection<Festival> FestivalList
        {
            get
            {
                return _festivalList;
            }
            set
            {
                _festivalList = value; OnPropertyChanged("FestivalList");
            }
        }

        private TicketType _selectedtickettype;

        public TicketType SelectedTicketType
        {
            get { return _selectedtickettype; }
            set { _selectedtickettype = value; OnPropertyChanged("SelectedTicketType"); }
        }

        private Podium _newPodium;

        public Podium NewPodium
        {
            get { return _newPodium; }
            set { _newPodium = value; OnPropertyChanged("NewPodium"); }
        }


        private Genre _newGenre;
        public Genre NewGenre
        {
            get { return _newGenre; }
            set { _newGenre = value; OnPropertyChanged("NewGenre"); }
        }

        private Band _newBand;
        public Band NewBand
        {
            get { return _newBand; }
            set { _newBand = value; OnPropertyChanged("NewBand"); }
        }

        private Ticket _newTicket;
        public Ticket NewTicket
        {
            get { return _newTicket; }
            set { _newTicket = value; OnPropertyChanged("NewTicket"); }
        }

        private ContactpersonType _newContactType;
        public ContactpersonType NewContactType
        {
            get { return _newContactType; }
            set { _newContactType = value; OnPropertyChanged("NewContactType"); }
        }

        private Contactperson _newContact;
        public Contactperson NewContact
        {
            get { return _newContact; }
            set { _newContact= value; OnPropertyChanged("NewContact"); }
        }


        
        #endregion


        #region Commands Band
        public ICommand addBandCommand
        {
            get { return new RelayCommand(AddBand); }
        }

        private void AddBand()
        {
            if (NewBand.Name != null && NewBand.Description !=  null)
            {

                int i = Band.AddBand(NewBand);
                if (i != 0)
                {
                    BandList = Band.GetBands();
                }
            }
        }
        #endregion

        #region Commands TicketType

        public ICommand addTicketTypeCommand
        {
            get { return new RelayCommand(AddTicketType); }
        }

        private void AddTicketType()
        {
            if (SelectedTicketType.Name != null && SelectedTicketType.Price != 0.0 && SelectedTicketType.AvailableTickets != 0)
            {

                int i = TicketType.AddType(SelectedTicketType);
                if (i != 0)
                {
                    TicketTypeList = TicketType.GetTicketType(SelectedTicketType);
                }
            }
        }


        public ICommand EditicketTypeCommand
        {
            get { return new RelayCommand(EditicketType); }
        }
        public void EditicketType()
        {
            if (SelectedTicketType.Name != null)
            {

                int i = TicketType.EditType(SelectedTicketType);
                if (i != 0)
                {
                    TicketTypeList = TicketType.GetTicketType();
                }
            }
        }


        private void RemoveTicketType()
        {
            //if (SelectedTicketType.ID != null)
            //{
            //    int i = TicketType.DeleteType(SelectedTicketType);
            //    if (i != 0)
            //    {
            //        TicketTypeList = TicketType.GetTicketType();
            //    }
            //}
        }
        #endregion

        #region Commands Podium

        public ICommand SaveStageCommand
        {
            get { return new RelayCommand(SaveStage); }
        }
        public void SaveStage()
        {
            if (NewPodium.Name != null)
            {

                int i = Podium.AddStage(NewPodium);
                if (i != 0)
                {
                    PodiumList = Podium.GetPodia();
                }
            }
        }

        public ICommand EditStageCommand
        {
            get { return new RelayCommand(EditStage); }
        }
        public void EditStage()
        {
            if (NewPodium.Name != null)
            {

                int i = Podium.EditStage(NewPodium);
                if (i != 0)
                {
                    PodiumList = Podium.GetPodia();
                }
            }
        }


        #endregion

        #region Commands Genre
        public ICommand AddGenreCommand
        {
            get { return new RelayCommand(AddGenre); }
        }
        public void AddGenre()
        {
            if (NewGenre.Name != null)
            {

                int i = Genre.AddGenre(NewGenre);
                if (i != 0)
                {
                    //
                }
            }
        }

        public ICommand EditGenreCommand
        {
            get { return new RelayCommand(EditGenre); }
        }
        public void EditGenre()
        {
            if (NewGenre.Name != null)
            {

                int i = Genre.EditGenre(NewGenre);
                if (i != 0)
                {
                    GenreList = Genre.GetGenres();
                }
            }
        }



        #endregion

        #region Commands Ticket

        public ICommand SaveTicketCommand
        {
            get { return new RelayCommand(SaveTicket); }
        }
        public void SaveTicket()
        {
            if (NewTicket.Ticketholder != null && NewTicket.TicketholderEmail != null && NewTicket.TicketType.ID != 0 && NewTicket.Amount != 0 )
            {

                int i = Ticket.AddTicket(NewTicket);
                if (i != 0)
                {
                    TicketList = Ticket.GetTickets();
                    TicketTypeList = TicketType.GetTicketType(SelectedTicketType);
                }
            }
        }

        public ICommand EditTicketCommand
        {
            get { return new RelayCommand(EditTicket); }
        }
        public void EditTicket()
        {
            if (NewTicket.Ticketholder != null)
            {

                int i = Ticket.EditTicket(NewTicket);
                if (i != 0)
                {
                    TicketList = Ticket.GetTickets();
                    TicketTypeList = TicketType.GetTicketType(SelectedTicketType);
                }
            }
        }

        public ICommand PrintTicketCommand
        {
            get { return new RelayCommand<int>(PrintTicket); }
        }

        private void PrintTicket(int id)
        {
            Ticket ticket = NewTicket;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                string sPad = fbd.SelectedPath;
                Ticket.PrintWord(ticket, sPad);
            }


        }

        #endregion


        #region Commands ContactType

        public ICommand AddContactTypeCommand
        {
            get { return new RelayCommand(AddContactType); }
        }
        public void AddContactType()
        {
            if (NewContactType.Name != null)
            {

                int i = ContactpersonType.AddContactType(NewContactType);
                if (i != 0)
                {
                    ContactTypeList = ContactpersonType.GetContactTypes();
                }
            }
        }

        public ICommand EditConactTypeCommand
        {
            get { return new RelayCommand(EditContactType); }
        }
        public void EditContactType()
        {
            if (NewContactType.Name != null)
            {

                int i = ContactpersonType.EditContactType(NewContactType);
                if (i != 0)
                {
                    ContactTypeList = ContactpersonType.GetContactTypes();
                }
            }
        }

        #endregion

        #region Commands Contact

        public ICommand AddContactCommand
        {
            get { return new RelayCommand(AddContact); }
        }
        public void AddContact()
        {
            if (NewContact.Name != null)
            {

                int i = Contactperson.AddContact(NewContact);
                if (i != 0)
                {
                    ContactList = Contactperson.GetContacts();
                }
            }
        }

        public ICommand EditConactCommand
        {
            get { return new RelayCommand(EditContact); }
        }
        public void EditContact()
        {
            if (NewContact.Name != null)
            {

                int i = Contactperson.EditContact(NewContact);
                if (i != 0)
                {
                    ContactList = Contactperson.GetContacts();
                }
            }
        }


        public ICommand DeleteConactCommand
        {
            get { return new RelayCommand(DeleteConact); }
        }
        public void DeleteConact()
        {
            if (NewContact.Name != null)
            {

                int i = Contactperson.DeleteContact(NewContact);
                if (i != 0)
                {
                    ContactList = Contactperson.GetContacts();
                }
            }
        }


        #endregion

        //#region Interface
        //public string Name
        //{
        //    get { throw new NotImplementedException(); }
        //}
        //#endregion


    }
}

    


