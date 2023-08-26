using Microsoft.AspNetCore.Mvc;

using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Time.com.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetHighlights()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string url = "https://time.com/";

                string html = await httpClient.GetStringAsync(url);

                var headlines = new List<Dictionary<string, string>>();

                int lstartIndex = 0;
                int lendIndex = 0;
                int hstartIndex = 0;
                int hendIndex = 0;

                // Find and extract headlines based on specific HTML patterns
                while ((lstartIndex = html.IndexOf("<a class=\"most-popular-feed__item-section\"", lendIndex)) >= 0)
                {
                    //Console.WriteLine(html.IndexOf("<a class=\"most-popular-feed__item-section\"", 0));
                    int linkStartIndex = html.IndexOf("<a href=\"", lstartIndex) + "<a href=\"".Length;
                    // Console.WriteLine(linkStartIndex);
                    int linkEndIndex = html.IndexOf("\"", linkStartIndex);
                    lstartIndex = linkStartIndex;
                    lendIndex = linkEndIndex;

                    string link = html.Substring(linkStartIndex, linkEndIndex - linkStartIndex).Trim();


                    if (hendIndex >= 0)
                    {
                        hstartIndex = html.IndexOf("<h3 class=\"most-popular-feed__item-headline\">", linkEndIndex) + "<h3 class=\"most-popular-feed__item-headline\">".Length;
                        hendIndex = html.IndexOf("</h3>", hstartIndex); ;
                        string headline = html.Substring(hstartIndex, hendIndex - hstartIndex).Trim();

                        // Search for the link within the context of the specific headline


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
                    return Ok(headlines);
                }
                else
                {
                    return new JsonResult(headlines);
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500,new { message = "Error" });    

            }

        }









    }

       

      
    
}