
using System.Linq;
using System.Text;

namespace KDZ3MOD3
{
    public class CSVProcessing
    {
        string[] headEn = { "ID", "SculpName", "Photo", "Author", "ManufactYear", "Material", "Description", "LocationPlace", "Longitude_WGS84", "Latitude_WGS84", "global_id", "geodata_center", "geoarea" };
        string[] headRu = { "Код", "Наименование скульптуры", "Фотография", "Автор", "Год изготовления", "Материал изготовления", "Описание", "Месторасположение", "Долгота в WGS-84", "Широта в WGS-84", "global_id", "geodata_center", " geoarea" };

        public string? Message { get; set; }
        public bool Flag { get; set; }
        /// <summary>
        /// the method that reads the csv file.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public List<Monuments> Read(StreamReader stream)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            List<Monuments> objects = new List<Monuments>();
            try
            {
                string? line = stream.ReadLine();
                List<string> data = new List<string>();
                while (line != null)
                {
                    data.Add(line);
                    line = stream.ReadLine();
                }

                if (data.Count < 2)
                {
                    throw new FormatException();
                }
                CheckCap(data[0], data[1]);
                for (int i = 2; i < data.Count; i++)
                {

                    string[] row = data[i].Split(';');
                    string id = row[0].Trim('\"');
                    string? skulpName = row[1].Trim('\"');
                    string? photo = row[2].Trim('\"');
                    string? author = row[3].Trim('\"');
                    string? manYear = row[4].Trim('\"');
                    string? material = row[5].Trim('\"');
                    string? description = row[6].Trim('\"');
                    string? locPlace = row[7].Trim('\"');
                    string? longitude = row[8].Trim('\"');
                    string? latitude = row[9].Trim('\"');
                    string? glid = row[10].Trim('\"');
                    string? geodataCenter = row[11].Trim('\"');
                    string? geoarea = row[12].Trim('\"');
                    Monuments mon = new Monuments(id, skulpName, photo, author, manYear, material, description, locPlace, longitude, latitude, glid,  geodataCenter, geoarea);
                    objects.Add(mon);
                }
                Flag = true;
                stream.Close();
            }
            catch (ArgumentException)
            {
                Message = "Некорректная строка. Попробуйте еще раз.";
                Flag = false;
                stream.Close();
            }
            catch (FormatException)
            {
                Message = "Недостаточное количество строк в файле. Попробуйте еще раз.";
                Flag = false;
                stream.Close();
            }
            catch (Exception)
            {
                Message = "Неизвестная ошибка. Попробуйте еще раз.";
                Flag = false;
                stream.Close();
            }
            return objects;
        }
        /// <summary>
        /// the method that checks the csv file.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <exception cref="ArgumentException"></exception>
        private void CheckCap(string first, string second)
        {

            string[] helpWithEn = new string[headEn.Length];
            string[] helpWithRu = new string[headRu.Length];
            for (int i = 0; i < helpWithEn.Length; i++)
            {
                helpWithEn[i] = '"' + headEn[i] + '"';


                helpWithRu[i] = '"' + headRu[i] + '"';
            }
            string headEnStr = string.Join(';', helpWithEn) + ";";
            string headRuStr = string.Join(';', helpWithRu) + ";";
            if (first != "\"ID\";\"SculpName\";\"Photo\";\"Author\";\"ManufactYear\";\"Material\";\"Description\";\"LocationPlace\";\"Longitude_WGS84\";\"Latitude_WGS84\";\"global_id\";\"geodata_center\";\"geoarea\";")
            {
                throw new ArgumentException();
            }
            if (second != "\"Код\";\"Наименование скульптуры\";\"Фотография\";\"Автор\";\"Год изготовления\";\"Материал изготовления\";\"Описание\";\"Месторасположение\";\"Долгота в WGS-84\";\"Широта в WGS-84\";\"global_id\";\"geodata_center\";\"geoarea\";")
            {
                throw new ArgumentException();
            }
        }
        /// <summary>
        /// the method that writes the csv file.
        /// </summary>
        /// <param name="libraries"></param>
        /// <returns></returns>
        public Stream Write(List<Monuments> libraries)
        {
            var monString = (from lib in libraries select lib.ToScv()).ToList();
            string[] helpWithEn = new string[headEn.Length];
            string[] helpWithRu = new string[headRu.Length];
            for (int i = 0; i < helpWithEn.Length; i++)
            {
                helpWithEn[i] = '"' + headEn[i] + '"';
                helpWithRu[i] = '"' + headRu[i] + '"';
            }
            string headEnStr = string.Join(';', helpWithEn) + ";";
            string headRuStr = string.Join(';', helpWithRu) + ";";
            var linesToFile =
                new List<string>(new[] { headEnStr, headRuStr }).Concat(monString).ToList();
            using (StreamWriter file = new StreamWriter("../../../../KDZ3MOD3/fileToSend.csv"))
            {
                foreach (var line in linesToFile)
                {
                    file.WriteLine(line);
                }
            }
            return File.Open("../../../../KDZ3MOD3/fileToSend.csv", FileMode.Open);
        }
        /// <summary>
        /// A constructor without parameters.
        /// </summary>
        public CSVProcessing() { }

    }
}
