using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_MVVM.Model
{
    class Festival
    {
        public String Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String Location { get; set; }


        public static ObservableCollection<Festival> GetFestivalInfo()
        {
            ObservableCollection<Festival> festivalList = new ObservableCollection<Festival>();
            String sql = "SELECT * FROM festival";
            DbDataReader reader = Database.GetData(sql);
            Database.GetData(sql);

            while (reader.Read())
            {
                festivalList.Add(MaakFestivalAan(reader));
            }
            return festivalList;
        }

        private static Festival MaakFestivalAan(IDataRecord rij)
        {
            Festival nieuw = new Festival();
            nieuw.Name = rij["Name"].ToString();
            String startdatum = rij["StartDate"].ToString();
            nieuw.StartDate = Convert.ToDateTime(startdatum);
            String Einddatum = rij["EndDate"].ToString();
            nieuw.EndDate = Convert.ToDateTime(Einddatum);
            nieuw.Location = rij["Location"].ToString();

            return nieuw;
        }
    }
}
