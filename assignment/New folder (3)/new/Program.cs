using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

class Program
{
    static async Task Main(string[] args)
    {
        string jsonResult = await GetMostReadHeadlinesAsJsonAsync();
        Console.WriteLine(jsonResult);
    }

    static async Task<string> GetMostReadHeadlinesAsJsonAsync()
    {
        HttpClient httpClient = new HttpClient();
        string url = "https://time.com/";

        string html = await httpClient.GetStringAsync(url);

        var headlines = new List<Dictionary <string,string>>();

        int lstartIndex = 0;
        int lendIndex = 0;
        int hstartIndex = 0;
        int hendIndex = 0;

        while ((lstartIndex = html.IndexOf("<a class=\"most-popular-feed__item-section\"", lendIndex)) >= 0)
        {
            int linkStartIndex = html.IndexOf("<a href=\"", lstartIndex) + "<a href=\"".Length;
            int linkEndIndex = html.IndexOf("\"", linkStartIndex);
           
            lendIndex = linkEndIndex;

            string link = html.Substring(linkStartIndex, linkEndIndex - linkStartIndex).Trim();
            

            if (hendIndex >= 0)
            {
                hstartIndex = html.IndexOf("<h3 class=\"most-popular-feed__item-headline\">",linkEndIndex) + "<h3 class=\"most-popular-feed__item-headline\">".Length;
                hendIndex = html.IndexOf("</h3>", hstartIndex); ;
                string headline = html.Substring(hstartIndex, hendIndex - hstartIndex).Trim();

                

                var headlineInfo = new Dictionary<string, string>
        {
            { "title", headline },
            { "link", link }
        };

                headlines.Add(headlineInfo);
            }
        }

        if (headlines.Count > 0)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(headlines, Newtonsoft.Json.Formatting.Indented);
        }
        else
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { Message = "No most read headlines found." }, Newtonsoft.Json.Formatting.Indented);
        }
    }
}