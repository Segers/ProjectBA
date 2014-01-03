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
    class Podium
    {
        public int ID { get; set; }
        public string Name { get; set; }


        private static ObservableCollection<Podium> podiumlist = GetPodia();

        public static ObservableCollection<Podium> GetPodia()
        {
            ObservableCollection<Podium> podiumlist = new ObservableCollection<Podium>();
            String sql = "SELECT * FROM Stage";
            DbDataReader reader = Database.GetData(sql);
            Database.GetData(sql);

            while (reader.Read())
            {
                podiumlist.Add(MaakPodiumAan(reader));
            }
            return podiumlist;
        }

        private static Podium MaakPodiumAan(IDataRecord rij)
        {
            Podium nieuw = new Podium();
            nieuw.ID = Convert.ToInt32(rij["StageID"].ToString());
            nieuw.Name = rij["Name"].ToString();



            return nieuw;
        }


        public static int AddStage(Podium NewPodiumName)
        {
            DbTransaction trans = null;

            try
            {
                trans = Database.BeginTransaction();

                string sql = "INSERT INTO stage(Name) VALUES (@name);";
                DbParameter par1 = Database.AddParameter("@name", NewPodiumName.Name);

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


        public static int EditStage(Podium PodiumName)
        {
            DbTransaction trans = null;

            try
            {
                trans = Database.BeginTransaction();

                string sql = "UPDATE stage SET Name=@name WHERE StageID=@StageID";
                DbParameter par1 = Database.AddParameter("@name", PodiumName.Name);
                DbParameter par2 = Database.AddParameter("@StageID", PodiumName.ID);
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
