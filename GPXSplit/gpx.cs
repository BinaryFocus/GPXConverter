using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPXSplit
{
    public class gpx
    {
        List<XMLPoint> pointList = new List<XMLPoint>();
        public void loadFile(string file, DateTime startDate, DateTime endDate)
        {

            List<String> lines = new List<string>(File.ReadAllLines(file));
            foreach (var line in lines.Skip(1))
            {

                string[] parts = line.Split(',');
                var lat = double.Parse(parts[parts.Length - 3]);
                var lon = double.Parse(parts[parts.Length - 2]);

                DateTime x = DateTime.Parse(parts[1]);
                DateTime y = x.ToUniversalTime();

                var point = new XMLPoint();
                point.lon = lon;
                point.lat = lat;
                point.timestamp = String.Format("{0:s}", y) + "Z";

                if(y > startDate & y < endDate)
                {
                    pointList.Add(point);
                }

            }
        }

        public void outputFile(string outputFolder)
        {
            StringBuilder pointsXML = new StringBuilder();
            foreach (var item in pointList.OrderBy(o => o.timestamp))
            {
                pointsXML.Append(item.TransformText());
            }

            var xmlDoc = new XMLBase();
            xmlDoc.coords = pointsXML.ToString();

            var outputFile = Path.Combine(outputFolder, "output.gpx");
            File.WriteAllText(outputFile, xmlDoc.TransformText());
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
