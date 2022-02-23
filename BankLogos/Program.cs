// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json.Linq;
using System.Net;

Console.WriteLine("bank images downloader from UNNAX partner");
string dir = @"C:\banklogos\";
Console.WriteLine("path to save: " + dir );

var client = new WebClient();

// If directory does not exist, create it
if (!Directory.Exists(dir))
{
    Directory.CreateDirectory(dir);
}

var content = client.DownloadString("https://integration.unnax.com/api/v3/banks/?details=true&limit=500&offset=0");
var count = int.Parse(JObject.Parse(content)["count"].ToString());
Console.WriteLine("count : " + count);


for (int i = 0; i <= count; i++)
{
    try
    {
        var entityLogoPath = JObject.Parse(content)["results"]?[i]?["details"]?["logo"];
        var bankId = JObject.Parse(content)?["results"]?[i]?["id"];

        if (entityLogoPath != null)
        {
            byte[] fileBytes = client.DownloadData(entityLogoPath.ToString());
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("trying...: " + entityLogoPath);
            File.WriteAllBytes(dir + bankId + ".svg", fileBytes);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Downloaded");
        }

    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("trying...others:" + ex.Message);
    }
}

Console.WriteLine("End!");