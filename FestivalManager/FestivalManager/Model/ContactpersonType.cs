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
    class ContactpersonType
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public static ObservableCollection<ContactpersonType> GetContactTypes()
        {
            ObservableCollection<ContactpersonType> Contactlist = new ObservableCollection<ContactpersonType>();
            String sql = "SELECT * FROM contactpersontype";
            DbDataReader reader = Database.GetData(sql);
            Database.GetData(sql);

            while (reader.Read())
            {
                Contactlist.Add(MaakContactTypeAan(reader));
            }
            return Contactlist;
        }

        private static ContactpersonType MaakContactTypeAan(IDataRecord rij)
        {
            ContactpersonType nieuw = new ContactpersonType();
            nieuw.ID = Convert.ToInt32(rij["ContactpersonTypeID"].ToString());
            nieuw.Name = rij["Name"].ToString();

            return nieuw;
        }

        public static int AddContactType(ContactpersonType NewContactType)
        {
            DbTransaction trans = null;

            try
            {
                trans = Database.BeginTransaction();

                string sql = "INSERT INTO contactpersontype(Name) VALUES (@name);";
                DbParameter par1 = Database.AddParameter("@name", NewContactType.Name);

                int rowsaffected = 0;
                rowsaffected += Database.ModifyData(trans, sql, par1);
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

        public static int EditContactType(ContactpersonType ContactType)
        {
            DbTransaction trans = null;

            try
            {
                trans = Database.BeginTransaction();

                string sql = "UPDATE contactpersontype SET Name=@name WHERE ContactpersontypeID=@contactID";
                DbParameter par1 = Database.AddParameter("@name", ContactType.Name);
                DbParameter par2 = Database.AddParameter("@contactID", ContactType.ID);
                int rowsaffected = 0;
                rowsaffected += Database.ModifyData(trans, sql, par1, par2);
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

        public static ContactpersonType GetContactpersonTypeByID(int id)
        {
            ContactpersonType gevondenContactpersonType = new ContactpersonType();
            DbParameter par = Database.AddParameter("@ID", id);
            DbDataReader reader = Database.GetData("SELECT * FROM ContactpersonType WHERE ContactpersonTypeID=@ID", par);
            while (reader.Read())
            {
                gevondenContactpersonType = MaakContactTypeAan(reader);
                return gevondenContactpersonType;
            }

            return gevondenContactpersonType;
        }

    }
}
