using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HouseManager
{
    class Boundary
    {

        internal void load()
        {
            var rootPath = System.IO.Directory.GetCurrentDirectory();
            //Consol.WriteLine($"path:{rootPath}");
            var regionPath = $"{rootPath}\\config\\region.json";
            var data = File.ReadAllText(regionPath);
            //Consol.WriteLine(data);
            var regions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(data);
            for (var i = 0; i < regions.Count; i++) 
            {
                //Consol.WriteLine(regions[i]);
                var filePath= $"{rootPath}\\config\\boundary\\{regions[i]}.txt";

            }
            Console.ReadLine();
            
            //   var 
            // throw new NotImplementedException();
        }
    }
}
