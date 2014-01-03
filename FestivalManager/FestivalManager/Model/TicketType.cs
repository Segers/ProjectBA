using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project_MVVM.Model
{

    [AttributeUsage(AttributeTargets.Property,
                Inherited = false,
                AllowMultiple = false)]
    internal sealed class OptionalAttribute : Attribute
    {
    }

    class TicketType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int AvailableTickets { get; set; }
        [Optional]
        public int AantalBesteld { get; set; }

        public static ObservableCollection<TicketType> GetTicketType(TicketType optionalTT = null)
        {
            ObservableCollection<TicketType> ticketlist = new ObservableCollection<TicketType>();
            String sql = "SELECT * FROM TicketType";
            DbDataReader reader = Database.GetData(sql);
            Database.GetData(sql);

            while (reader.Read())
            {
                ticketlist.Add(MaaktTicketTypeAan(reader));
            }
            return ticketlist;
        }

        public static TicketType MaaktTicketTypeAan(IDataRecord rij, TicketType optionalTT = null)
        {
            TicketType nieuw = new TicketType();
            nieuw.ID = Convert.ToInt32(rij["TicketTypeID"].ToString());
            nieuw.Name = rij["Name"].ToString();
            nieuw.Price = Convert.ToDouble(rij["Price"].ToString());
            nieuw.AvailableTickets = Convert.ToInt32(rij["AvailableTickets"].ToString());
            nieuw.AantalBesteld = GetAantalBesteld(optionalTT);


            return nieuw;
        }

        public static int MaakAantalAan(IDataRecord rij) 
        {

            int aantal = 0;

            aantal =Convert.ToInt32(rij["aantal"]);

            return aantal;
        }

        public static int EditType(TicketType ticketType)
        {
            DbTransaction trans = null;

            try
            {
                trans = Database.BeginTransaction();

                string sql = "UPDATE TicketType SET Name=@Name,Price=@Price,AvailableTickets=@AvailableTickets WHERE TicketTypeID=@ID";
                DbParameter par1 = Database.AddParameter("@Name", ticketType.Name);
                DbParameter par2 = Database.AddParameter("@ID", ticketType.ID);
                DbParameter par3 = Database.AddParameter("@Price", ticketType.Price);
                DbParameter par4 = Database.AddParameter("@AvailableTickets", ticketType.AvailableTickets);

                int rowsaffected = 0;
                rowsaffected += Database.ModifyData(trans, sql, par1, par2, par3, par4);
                if (rowsaffected == 1)
                {
                    MessageBox.Show("Wijzigen is gelukt", "Gelukt", System.Windows.MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show("Wijzigen mislukt", "Mislukt", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                trans.Commit();
                return rowsaffected;
            }
            catch (Exception)
            {
                trans.Rollback();
                return 0;
            }
        }

        public static int AddType(TicketType ticketType)
        {
            DbTransaction trans = null;

            try
            {
                trans = Database.BeginTransaction();

                string sql = "INSERT INTO TicketType(Name,Price,AvailableTickets) VALUES(@Name,@price,@AvailableTickets)";
                DbParameter par1 = Database.AddParameter("@Name", ticketType.Name);
                DbParameter par2 = Database.AddParameter("@Price", ticketType.Price);
                DbParameter par3 = Database.AddParameter("@AvailableTickets", ticketType.AvailableTickets);

                int rowsaffected = 0;
                rowsaffected += Database.ModifyData(trans, sql, par1, par2, par3);
                if (rowsaffected == 1)
                {
                    MessageBox.Show("Opslaan is gelukt", "Gelukt", System.Windows.MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show("Opslaan mislukt", "Mislukt", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                trans.Commit();
                return rowsaffected;
            }
            catch (Exception)
            {
                trans.Rollback();
                 return 0;
            }
        }

        public static TicketType GetTicketTypeByID(int id)
        {
            TicketType gevondenTicket = new TicketType();
            DbParameter par = Database.AddParameter("@ID", id);
            DbDataReader reader = Database.GetData("SELECT * FROM tickettype WHERE TicketTypeID=@ID", par);
            while (reader.Read())
            {
                gevondenTicket = MaaktTicketTypeAan(reader);
                return gevondenTicket;
            }

            return gevondenTicket;
        }

        public static int GetAantalBesteld(TicketType TT)
        {
            int aantal = 0;
            if (TT != null)
            {
                DbParameter par = Database.AddParameter("@ID", TT.ID);
                DbDataReader reader = Database.GetData("Select Sum(Amount) as aantal From ticket where TicketTypeID=@id", par);
                while (reader.Read())
                {
                    aantal += MaakAantalAan(reader);
                    return aantal;
                }
            }
            else
            {
                aantal = 0;
            }
                return aantal;
            
        }


        //public static int DeleteType(TicketType ticketType)
        //{
        //    return Database.DeleteItem("TicketType", ticketType.ID);
        //}
    }

}
