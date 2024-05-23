using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace GUIPM
{
    public class Reviews : EnterUrl
    {
        private string Url;
        public Reviews(string url)
        {
            Url = url;
        }
        protected List<Review> ReviewsList { get; set; } = new List<Review>();

        public Reviews()
        {
            Url = UrlToScrape();  // Call the method within the constructor
                                  // Url is initialized with a valid value when an instance is created
        }

        public List<Review> ScrapeReviews()
        {
            if (string.IsNullOrWhiteSpace(Url))
            {
                throw new ArgumentException("URL cannot be null or empty", nameof(Url));
            }

            // Ensure URL starts with http:// or https://
            if (!Url.StartsWith("http://") && !Url.StartsWith("https://"))
            {
                Url = "https://" + Url;
            }

            List<Review> reviews = new List<Review>();

            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            IWebDriver driver = new ChromeDriver();

            try
            {
                driver.Navigate().GoToUrl(Url);
                Thread.Sleep(5000); // Let JavaScript load
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while trying to navigate to the URL: {ex.Message}");
                return null; // Or handle as needed
            }

            string pageSource = driver.PageSource;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(pageSource);

            var reviewNodes = doc.DocumentNode.SelectNodes("//article[contains(@class, 'styles_reviewCard__hcAvl')]");
            if (reviewNodes != null)
            {
                foreach (var node in reviewNodes)
                {
                    var review = new Review
                    {
                        Author = node.SelectSingleNode(".//a[@name='consumer-profile']/span")?.InnerText.Trim() ?? "Anonymous",
                        Title = node.SelectSingleNode(".//h2[contains(@class, 'typography_heading-s__f7029')]")?.InnerText.Trim() ?? "No title found",
                        Content = node.SelectSingleNode(".//div[@data-review-content='true']/p")?.InnerText.Trim() ?? "No content found",
                        StarRating = node.SelectSingleNode(".//div[contains(@class, 'star-rating_starRating__4rrcf')]/img")?.GetAttributeValue("alt", "No rating found")
                    };

                    string dateString = node.SelectSingleNode(".//time[@data-service-review-date-time-ago='true']")?.GetAttributeValue("title", null);
                    string format = "dddd 'den' d. MMMM yyyy 'kl.' HH.mm.ss";
                    CultureInfo provider = CultureInfo.GetCultureInfo("da-DK");

                    if (DateTime.TryParseExact(dateString, format, provider, DateTimeStyles.None, out DateTime parsedDate))
                    {
                        review.Date = parsedDate;
                    }
                    else
                    {
                        Console.WriteLine("Failed to parse date: " + dateString);
                        review.Date = DateTime.MinValue; // Use MinValue to indicate a failed parse
                    }

                    ReviewsList.Add(review);

                    // Print the review to the console
                    Console.WriteLine(review.ToString());//Calling the ToString method to print every review
                }
            }
            else
            {
                Console.WriteLine("No reviews found.");
            }

            driver.Quit();

            return ReviewsList;
        }
    }
}
