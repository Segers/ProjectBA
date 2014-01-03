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
    class Genre
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public static ObservableCollection<Genre> GetGenres()
        {
            ObservableCollection<Genre> GenreList = new ObservableCollection<Genre>();
            String sql = "SELECT * FROM genre";
            DbDataReader reader = Database.GetData(sql);
            Database.GetData(sql);

            while (reader.Read())
            {
                GenreList.Add(MaaktGenreAan(reader));
            }
            return GenreList;
        }

        private static Genre MaaktGenreAan(IDataRecord rij)
        {
            Genre nieuw = new Genre();
            nieuw.ID = Convert.ToInt32(rij["GenreID"].ToString());
            nieuw.Name = rij["Name"].ToString();




            return nieuw;
        }


        public static int AddGenre(Genre NewGenre)
        {
            DbTransaction trans = null;

            try
            {
                trans = Database.BeginTransaction();

                string sql = "INSERT INTO Genre(Name) VALUES (@name);";
                DbParameter par1 = Database.AddParameter("@name", NewGenre.Name);

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


        public static int EditGenre(Genre Genre)
        {
            DbTransaction trans = null;

            try
            {
                trans = Database.BeginTransaction();

                string sql = "UPDATE Genre SET Name=@name WHERE GenreID=@GenreID";
                DbParameter par1 = Database.AddParameter("@name", Genre.Name);
                DbParameter par2 = Database.AddParameter("@GenreID", Genre.ID);
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

    }
}
