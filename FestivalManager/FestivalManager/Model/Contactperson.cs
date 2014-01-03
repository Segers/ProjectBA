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
    class Contactperson
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public ContactpersonType JobRole { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Cellphone { get; set; }

        public static ObservableCollection<Contactperson> GetContacts()
        {
            ObservableCollection<Contactperson> Contactlist = new ObservableCollection<Contactperson>();
            String sql = "SELECT * FROM contactperson";
            DbDataReader reader = Database.GetData(sql);
            Database.GetData(sql);

            while (reader.Read())
            {
                Contactlist.Add(MaakContactTypeAan(reader));
            }
            return Contactlist;
        }

        private static Contactperson MaakContactTypeAan(IDataRecord rij)
        {
            Contactperson nieuw = new Contactperson();
            nieuw.ID = Convert.ToInt32(rij["ContactpersonID"].ToString());
            nieuw.Name = rij["Name"].ToString();
            nieuw.Company = rij["Company"].ToString();
            ContactpersonType contactType = new ContactpersonType();
            contactType = ContactpersonType.GetContactpersonTypeByID(Convert.ToInt32(rij["ContactpersonTypeID"]));
            nieuw.JobRole = contactType;
            nieuw.City = rij["City"].ToString();
            nieuw.Email = rij["Email"].ToString();
            nieuw.Phone = rij["Phone"].ToString();
            nieuw.Cellphone = rij["Cellphone"].ToString();
            return nieuw;
        }


        public static int AddContact(Contactperson NewContact)
        {
            DbTransaction trans = null;

            try
            {
                trans = Database.BeginTransaction();

                string sql = "INSERT INTO contactperson(Name,Company,ContactPersonTypeID,City,Email,Phone) VALUES (@Name,@Company,@ContactTypePersonID,@City,@Email,@Phone)";
                DbParameter par1 = Database.AddParameter("Name", NewContact.Name);
                DbParameter par2 = Database.AddParameter("Company", NewContact.Company);
                DbParameter par3 = Database.AddParameter("ContactTypePersonID", NewContact.JobRole.ID);
                DbParameter par4 = Database.AddParameter("City", NewContact.City);
                DbParameter par5 = Database.AddParameter("Email", NewContact.Email);
                DbParameter par6 = Database.AddParameter("Phone", NewContact.Phone);
                int rowsaffected = 0;
                rowsaffected += Database.ModifyData(trans, sql, par1, par2, par3, par4, par5, par6);
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


        public static int EditContact(Contactperson contact)
        {
            DbTransaction trans = null;

            try
            {
                trans = Database.BeginTransaction();

                string sql = "UPDATE contactperson SET Name=@name, Company=@company, ContactpersonTypeID=@cptID, City=@City, Email=@Email, Phone=@Phone WHERE ContactpersonID=@contactID";
                DbParameter par1 = Database.AddParameter("@name", contact.Name);
                DbParameter par2 = Database.AddParameter("@company", contact.Company);
                DbParameter par3 = Database.AddParameter("@cptID", contact.JobRole.ID);
                DbParameter par4 = Database.AddParameter("@City", contact.City);
                DbParameter par5 = Database.AddParameter("@Email", contact.Email);
                DbParameter par6 = Database.AddParameter("@Phone", contact.Phone);
                DbParameter par7 = Database.AddParameter("@contactID", contact.ID);
                int rowsaffected = 0;
                rowsaffected += Database.ModifyData(trans, sql, par1, par2, par3, par4, par5, par6, par7);
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

        public static int DeleteContact(Contactperson contact)
        {
            DbTransaction trans = null;
            try
            {
                trans = Database.BeginTransaction();
                string sql = "DELETE FROM contactperson WHERE contactpersonID = @ID;";

                DbParameter parID = Database.AddParameter("@ID", contact.ID);
                int rowsaffected = 0;
                rowsaffected += Database.ModifyData(sql, parID);
                if (rowsaffected == 0)
                {
                    MessageBox.Show("Verwijderen mislukt", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error, System.Windows.MessageBoxResult.OK);
                }
                else
                {
                    MessageBox.Show("Succesvol verwijderd", "Gelukt", System.Windows.MessageBoxButton.OK);
                    Console.WriteLine(rowsaffected + " row(s) are deleted");
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
    }
}
