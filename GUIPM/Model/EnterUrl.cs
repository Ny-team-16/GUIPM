using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIPM
{
    public class EnterUrl
    {
        public string UrlToScrape()
        {
            while (true)
            {
                Console.Write("Please provide a URL to continue: ");
                string Url = Console.ReadLine();

                if (Uri.TryCreate(Url, UriKind.Absolute, out Uri validatedUri) &&
                    (validatedUri.Scheme == Uri.UriSchemeHttp || validatedUri.Scheme == Uri.UriSchemeHttps))
                {
                    return validatedUri.ToString();
                }
                else
                {
                    Console.WriteLine("Invalid URL. Please enter a valid URL that starts with http:// or https://");
                }
            }

            
        }
       
    }
}
