using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project_MVVM.Model
{
    class Ticket
    {
        public int ID { get; set; }
        public string Ticketholder { get; set; }
        public string TicketholderEmail { get; set; }
        public TicketType TicketType { get; set; }
        public int Amount { get; set; }

        public static ObservableCollection<Ticket> GetTickets()
        {
            ObservableCollection<Ticket> ticketList = new ObservableCollection<Ticket>();
            String sql = "SELECT * FROM Ticket INNER JOIN TicketType ON Ticket.TicketTypeID = TicketType.TicketTypeID";
            DbDataReader reader = Database.GetData(sql);
            Database.GetData(sql);

            while (reader.Read())
            {
                ticketList.Add(MaaktTicketAan(reader));
            }
            return ticketList;
        }

        private static Ticket MaaktTicketAan(IDataRecord rij)
        {
            Ticket nieuw = new Ticket();
            
            nieuw.ID = Convert.ToInt32(rij["TicketTypeID"].ToString());
            nieuw.Ticketholder = rij["TicketHolder"].ToString();
            nieuw.TicketholderEmail = rij["TicketholderEmail"].ToString();
            TicketType ticketType = new TicketType();
            ticketType = TicketType.GetTicketTypeByID(Convert.ToInt32(rij["TicketTypeID"]));
            nieuw.TicketType = ticketType;
            nieuw.Amount = Convert.ToInt32(rij["Amount"].ToString());



            return nieuw;
        }






        public static int AddTicket(Ticket NewTicket)
        {
            DbTransaction trans = null;

            try
            {
                trans = Database.BeginTransaction();

                string sql = "INSERT INTO Ticket(Ticketholder,TicketholderEmail,TicketTypeID,Amount) VALUES (@Ticketholder,@TicketholderEmail,@TicketTypeID,@Amount);";
                DbParameter par1 = Database.AddParameter("@Ticketholder", NewTicket.Ticketholder);
                DbParameter par2 = Database.AddParameter("@TicketholderEmail", NewTicket.TicketholderEmail);
                DbParameter par3 = Database.AddParameter("@TicketTypeID", NewTicket.TicketType.ID);
                DbParameter par4 = Database.AddParameter("@Amount", NewTicket.Amount);
                int rowsaffected = 0;
                rowsaffected += Database.ModifyData(trans, sql, par1, par2, par3, par4);
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


        public static int EditTicket(Ticket Ticket)
        {
            DbTransaction trans = null;

            try
            {
                trans = Database.BeginTransaction();

                string sql = "UPDATE ticket SET TicketHolder=@name, TicketHolderEmail=@email, TicketTypeID=@ttID, Amount=@Amount WHERE ID=@TicketID";
                DbParameter par1 = Database.AddParameter("@name", Ticket.Ticketholder);
                DbParameter par2 = Database.AddParameter("@email", Ticket.TicketholderEmail);
                DbParameter par3 = Database.AddParameter("@ttID", Ticket.TicketType.ID);
                DbParameter par4 = Database.AddParameter("@Amount", Ticket.Amount);
                DbParameter par5 = Database.AddParameter("@TicketID", Ticket.ID);
                int rowsaffected = 0;
                rowsaffected += Database.ModifyData(trans, sql, par1, par2, par3, par4, par5);
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


        #region Methodes
        public static void PrintWord(Ticket ticket, string sPad)
        {
            string sFileNaam = ticket.ID + "_" + ticket.Ticketholder + ".docx";
            //string sFullPad = sPad + "\\" + sFileNaam;
            string sFullPad = "C:\\Users\\Tim\\Documents\\GitHub\\ProjectBA\\FestivalManager\\" + sFileNaam;
            try
            {
                //File.Copy("template.docx", sFullPad, true);
                File.Copy("template.docx", sFullPad, true);
                
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            WordprocessingDocument newDoc = WordprocessingDocument.Open(sFullPad, true);
            IDictionary<string, BookmarkStart> bookmarks = new Dictionary<string, BookmarkStart>();
            foreach (BookmarkStart bms in newDoc.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
            {
                bookmarks[bms.Name] = bms;
            }


            RunProperties propTitle = new RunProperties();
            RunFonts fontTitle = new RunFonts() { Ascii = "Segoe UI", HighAnsi = "Segoe UI" };
            FontSize sizeTitle = new FontSize() { Val = "36" };

            propTitle.Append(fontTitle);
            propTitle.Append(sizeTitle);



            bookmarks["Name"].Parent.InsertAfter<Run>(new Run(new Text(ticket.Ticketholder)), bookmarks["Name"]);
            bookmarks["Email"].Parent.InsertAfter<Run>(new Run(new Text(ticket.TicketholderEmail)), bookmarks["Email"]);
            bookmarks["Type"].Parent.InsertAfter<Run>(new Run(new Text(ticket.TicketType.Name)), bookmarks["Type"]);
            bookmarks["Amount"].Parent.InsertAfter<Run>(new Run(new Text(ticket.Amount.ToString())), bookmarks["Amount"]);
            bookmarks["Price"].Parent.InsertAfter<Run>(new Run(new Text(ticket.TicketType.Price.ToString())), bookmarks["Price"]);
            double iTotalPrice = ticket.Amount * ticket.TicketType.Price;
            bookmarks["Totalprice"].Parent.InsertAfter<Run>(new Run(new Text(iTotalPrice.ToString())), bookmarks["Totalprice"]);

            //BARCODE TOEVOEGEN            
            //string code = Guid.NewGuid().ToString();
            string code = GenerateUnique(ticket.TicketholderEmail);
            Run run = new Run(new Text(code));

            RunProperties prop = new RunProperties();
            RunFonts font = new RunFonts() { Ascii = "Free 3 of 9 Extended", HighAnsi = "Free 3 of 9 Extended" };
            FontSize size = new FontSize() { Val = "96" };

            prop.Append(font);
            prop.Append(size);
            run.PrependChild<RunProperties>(prop);

            bookmarks["Barcode"].Parent.InsertAfter<Run>(run, bookmarks["Barcode"]);

            newDoc.Close();
            MessageBox.Show(sFullPad + " is opgeslaan");
        }

        public static string GenerateUnique(string sEmail)
        {
            string ticks = DateTime.UtcNow.Ticks.ToString();
            string s1 = ticks.Substring(ticks.Length / 2, ticks.Length - (ticks.Length / 2));

            return s1;
        }
        #endregion
    }


}
