using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GPXBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            var doc = new gpx();
            doc.loadFile(@"D:\Box\Personal\TrackerData\trackinfo.csv");
            doc.outputFile();
        }
    }

    public class gpx
    {
        List<XMLPoint> pointList = new List<XMLPoint>();
        public void loadFile(string file)
        {

        
            List<String> lines = new List<string>(File.ReadAllLines(file));
            int count = 0;
            foreach (var line in lines.Skip(1))
            {
                count++;
                //if (count > 10)
                  //  break;
                string[] parts = line.Split(';');
                var lat = double.Parse(parts[parts.Length - 2]);
                var lon = double.Parse(parts[parts.Length - 1]);
                var dt = parts[1];
                var tm = parts[2];

                DateTime x = DateTime.Parse(dt + " " + tm);
                DateTime y = x.ToUniversalTime();
                Console.WriteLine(string.Format("{0}\t{1}", lat, lon));
                Console.WriteLine(string.Format("{0}\t{1}", dt, tm));

                var point = new XMLPoint();
                point.lon = lon;
                point.lat = lat;
                point.timestamp = String.Format("{0:s}", y)+"Z";

                pointList.Add(point);
            }
        }

        public void outputFile()
        {
            StringBuilder pointsXML = new StringBuilder();


            foreach (var item in pointList.OrderBy(o => o.timestamp))
            {
                pointsXML.Append(item.TransformText());
            }

            var xmlDoc = new XMLBase();
            xmlDoc.coords = pointsXML.ToString();

            File.WriteAllText(@"D:\Box\Personal\TrackerData\trackinfo1.gpx", xmlDoc.TransformText());
        }
    }


    public partial class XMLBase
    {
        public string coords;
    }

    public partial class XMLPoint
    {
        public double lat;
        public double lon;
        public string timestamp;
    }
}
