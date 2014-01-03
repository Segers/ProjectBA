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
    class Band
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public ObservableCollection<Genre> Genres { get; set; }

        public static ObservableCollection<Band> GetBands()
        {
            ObservableCollection<Band> bandlist = new ObservableCollection<Band>();
            //bandlist.Add(new Band() { ID = "1", Name = "test", Picture = "/url", Description = "/", Twitter = "/", Facebook = "/", Genres = null });
            //bandlist.Add(new Band() { ID = "2", Name = "qfqfqd", Picture = "/url", Description = "qdfsd", Twitter = "/", Facebook = "/", Genres = null });

            String sql = "SELECT band.name, band.BandID, band.picture, band.Description, band.twitter, band.facebook, genre.name as genrename, genre.genreID  FROM genre INNER join band ON genre.GenreID = band.GenreID";
            //SELECT * FROM Band INNER JOIN genre ON Band.GenreID = Genre.GenreID
            DbDataReader reader = Database.GetData(sql);
            Database.GetData(sql);

            while (reader.Read())
            {
                bandlist.Add(MaakBand(reader));
            }
            return bandlist;
        }

        private static Band MaakBand(IDataRecord rij)
        {
            Band nieuw = new Band();
            Genre nieuwgenre = new Genre();
            nieuw.ID = Convert.ToInt32(rij["BandID"].ToString());
            nieuw.Name = rij["Name"].ToString();
            nieuw.Picture = "/";
            nieuw.Description = rij["Description"].ToString();
            nieuw.Twitter = rij["Twitter"].ToString();
            nieuw.Facebook = rij["Facebook"].ToString();
            nieuwgenre.ID = Convert.ToInt32(rij["genreID"].ToString());
            nieuwgenre.Name = rij["genrename"].ToString()  ;
            nieuw.Genres = null;
            

            return nieuw;
        }

        public static int AddBand(Band NewBand)
        {
            DbTransaction trans = null;

            try
            {
                trans = Database.BeginTransaction();

                string sql = "INSERT INTO band(Name,Description,Twitter,Facebook,GenreID) VALUES (@name,@description,@twitter,@facebook,@genreid)";
                DbParameter par1 = Database.AddParameter("@name", NewBand.Name);
                DbParameter par2 = Database.AddParameter("@description", NewBand.Description);
                DbParameter par3 = Database.AddParameter("@twitter", NewBand.Twitter);
                DbParameter par4 = Database.AddParameter("@facebook", NewBand.Facebook);
                DbParameter par5 = Database.AddParameter("@genreid", null);
                int rowsaffected = 0;
                rowsaffected += Database.ModifyData(trans, sql, par1, par2, par3, par4, par5);
                if (rowsaffected == 1)
                {
                    MessageBox.Show("Toevoegen is gelukt", "Gelukt", System.Windows.MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show("Toevoegen mislukt", "Mislukt", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
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
