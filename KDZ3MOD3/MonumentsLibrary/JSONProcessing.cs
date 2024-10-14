using System.Text.Encodings.Web;
using System.Text.Json;
namespace KDZ3MOD3
{
    public class JSONProcessing
    {
        /// <summary>
        /// The method that reads the json format file.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public List<Monuments> Read(StreamReader stream)
        {
            string? line = stream.ReadLine();
            List<string> linesInFile = new List<string>();
            while (line != null)
            {
                linesInFile.Add(line);
                line = stream.ReadLine();
            }
            List<Monuments> result = JsonSerializer.Deserialize<List<Monuments>>(String.Join('\n', linesInFile))!;
            return result;
        }

        /// <summary>
        /// The method that writes a file in the json format.
        /// </summary>
        /// <param name="monuments"></param>
        /// <returns></returns>
        public Stream Write(List<Monuments> monuments)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            var lines = JsonSerializer.Serialize(monuments, options);
            using (var file = new StreamWriter("../../../../KDZ3MOD3/fileToSend.csv"))
            {
                file.WriteLine(lines);
            }
            return File.Open("../../../../KDZ3MOD3/fileToSend.csv", FileMode.Open);
        }
        /// <summary>
        /// A constructor without parameters.
        /// </summary>
        public JSONProcessing() { }
    }
}
