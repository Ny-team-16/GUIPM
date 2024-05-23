using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIPM
{
    public class Categorisation : Reviews //Categorisation class inherits from the Reviews class
    {
        private bool IsNegative(Review review) //Defining the negative reviews
        {
            //If the character 11 of the string StarRating is == '1', '2' or '3', it is considered as a negative review
            return review.StarRating[11] == '1' || review.StarRating[11] == '2' || review.StarRating[11] == '3';
        }
        private bool IsPositive (Review review) //Defining the positive reviews
        {
            //If the character 11 of the string StarRating is == '4' or '5', it is considered as a positive review
            return review.StarRating[11] == '4' || review.StarRating[11] == '5';
        }

        // Method to get all negative reviews (1-3 stars)
        public List<Review> GetNegativeReviews(List<Review> reviews)
        {
            return ReviewsList.Where(review => IsNegative(review)).ToList();
        }

        //Method to get all positive reviews (4-5 stars)
        public List<Review> GetPositiveReviews(List<Review> reviews)
        {
            return ReviewsList.Where(review => IsPositive(review)).ToList();
        }


    }
}
