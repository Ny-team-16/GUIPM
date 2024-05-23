using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIPM
{
    public class Review
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string StarRating { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }

        public override string ToString()
        {
            return $"Author: {Author}\n Title: {Title}\n Rating: {StarRating}\n Date: {Date.ToString("yyyy-MM-dd HH:mm:ss")}\n Content: {Content}";
        }
    }
}
