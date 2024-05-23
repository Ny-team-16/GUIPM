using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIPM
{
    public class SortReviews : Categorisation
    {
        public void BubbleSortReviews(List<Review> reviews)
        {
            bool swapped;
            do
            {
                swapped = false;
                for (int i = 0; i < reviews.Count - 1; i++)
                {
                    if (reviews[i].StarRating[11] > reviews[i + 1].StarRating[11])
                    {
                        // Swap the reviews
                        Review temp = reviews[i];
                        reviews[i] = reviews[i + 1];
                        reviews[i + 1] = temp;
                        swapped = true;                       
                    }
                }
            } while (swapped);

            //Print the sorted list of reviews after the sorting process is completed
            foreach (Review review in reviews)
            {
                Console.WriteLine(review);
            }
        }

        public void BubbleSortPositiveReviews (List<Review> reviews)
        {

            bool swapped;
            do
            {
                swapped = false;
                for (int i = 0; i < reviews.Count - 1; i++)
                {
                    if (reviews[i].StarRating[11] > reviews[i + 1].StarRating[11])
                    {
                        // Swap the positive reviews
                        Review temp = reviews[i];
                        reviews[i] = reviews[i + 1];
                        reviews[i + 1] = temp;
                        swapped = true;
                    }
                }
            } while (swapped);

            //Print the sorted list of positive reviews after the sorting process is completed
            foreach (Review review in reviews)
            {
                Console.WriteLine(review);
            }
        }

        public void BubbleSortNegativeReviews(List<Review> reviews) //Method to sort the negative reviews
        {
            bool swapped;
            do
            {
                swapped = false;
                for (int i = 0; i < reviews.Count - 1; i++)
                {
                    if (reviews[i].StarRating[11] > reviews[i + 1].StarRating[11])
                    {
                        // Swap the negative reviews
                        Review temp = reviews[i];
                        reviews[i] = reviews[i + 1];
                        reviews[i + 1] = temp;
                        swapped = true;
                    }
                }
            } while (swapped);

            //Print the sorted list of negative reviews after the sorting process is completed
            foreach (Review review in reviews)
            {
                Console.WriteLine(review);
            }
        }

    }
}
