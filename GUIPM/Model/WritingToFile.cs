using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text; //To support Danish letter ø, å, æ format to CSV file using UTF8-Encoding
using OfficeOpenXml; //To open the reviews in Excel

namespace GUIPM
{
    public class WritingToFile : SortReviews
    {
        public void PrintReviewsToTxt() //Method to print all sorted reviews to Txt file
        {
            // Direct access to the Reviews
            var Reviews = ScrapeReviews();
            BubbleSortReviews(Reviews); //To retrieve the sorted ReviewsList

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // File path where the reviews will be written
            string filePath = Path.Combine(docPath, "Reviews.txt");

            Console.WriteLine($"File path: {filePath}");

            try
            {
                using (StreamWriter outputFile = new StreamWriter(filePath)) //using streamwriter to write the reviews to file
                {
                    foreach (var review in Reviews)
                    {
                        outputFile.WriteLine(review);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Reviews successfully written to file.");
            }
        }

        public void PrintReviewsToCsv()  // Method to print all sorted reviews in CSV file
        {
            var Reviews = ScrapeReviews();
            BubbleSortReviews(Reviews); //To retrieve the sorted ReviewsList

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // File path where the reviews will be written, changed extension to .csv
            string filePath = Path.Combine(docPath, "Reviews.csv");

            Console.WriteLine($"File path: {filePath}");

            try
            {
                // Ensure the StreamWriter writes with UTF-8 encoding
                using (StreamWriter outputFile = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    // Write the header line for the CSV file
                    outputFile.WriteLine("Author,Title,StarRating,Date,Content");

                    // Write each review in CSV format
                    foreach (var review in ReviewsList)
                    {
                        outputFile.WriteLine(FormatForCsv(review));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Reviews successfully written to file.");
            }
        }


        public void PrintReviewsToExcel()  // Method to print all sorted reviews in Excel
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; //To be able to write in Excel without paying

            var Reviews = ScrapeReviews();
            BubbleSortReviews(Reviews); //To retrieve the sorted ReviewsList

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // File path where all the reviews will be written, change extension to .xlsx
            string filePath = Path.Combine(docPath, "Reviews.xlsx");

            Console.WriteLine($"File path: {filePath}");

            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();  // Ensures we are creating a new workbook
                }

                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Reviews");

                    // Adding header row
                    worksheet.Cells["A1"].Value = "Author";
                    worksheet.Cells["B1"].Value = "Title";
                    worksheet.Cells["C1"].Value = "StarRating";
                    worksheet.Cells["D1"].Value = "Date";
                    worksheet.Cells["E1"].Value = "Content";

                    int row = 2;  // Start on the second row to skip headers
                    foreach (var review in ReviewsList)
                    {
                        worksheet.Cells[row, 1].Value = review.Author;
                        worksheet.Cells[row, 2].Value = review.Title;
                        worksheet.Cells[row, 3].Value = review.StarRating;
                        worksheet.Cells[row, 4].Value = review.Date.ToString("yyyy-MM-dd");
                        worksheet.Cells[row, 5].Value = review.Content;
                        row++;
                    }

                    package.Save();  // Save the changes to the file
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Reviews successfully written to Excel file.");
            }
        }
        public void PrintNegativeReviewsToTxt() //Method to print all sorted negative reviews to txt file 
        {
            var Reviews = ScrapeReviews();
            BubbleSortNegativeReviews(Reviews); //To retrieve the sorted negative review list

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // File path where the negative reviews will be written
            string filePath = Path.Combine(docPath, "NegativeReviews.txt");

            Console.WriteLine($"File path: {filePath}");

            try
            {
                using (StreamWriter outputFile = new StreamWriter(filePath))
                {
                    foreach (var review in Reviews)
                    {
                        outputFile.WriteLine(review);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Reviews successfully written to file.");
            }
        }

        public void PrintNegativeReviewsToCsv()  // Method to print all sorted negative reviews in CSV file
        {
            var Reviews = ScrapeReviews();
            BubbleSortNegativeReviews(Reviews); //To retrieve the sorted negative review list

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // File path where the negative reviews will be written, changed extension to .csv
            string filePath = Path.Combine(docPath, "NegativeReviews.csv");

            Console.WriteLine($"File path: {filePath}");

            try
            {
                // Ensure the StreamWriter writes with UTF-8 encoding
                using (StreamWriter outputFile = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    // Write the header line for the CSV file
                    outputFile.WriteLine("Author,Title,StarRating,Date,Content");

                    // Write each review in CSV format
                    foreach (var review in Reviews)
                    {
                        outputFile.WriteLine(FormatForCsv(review));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Reviews successfully written to file.");
            }
        }

        // Helper method to format a review for CSV output
        private string FormatForCsv(Review review)
        {
            return $"\"{EscapeCsv(review.Author)}\"," +
            $"\"{EscapeCsv(review.Title)}\"," +
            $"\"{review.StarRating}\"," +
            $"\"{review.Date.ToString("yyyy-MM-dd")}\"," +
            $"\"{EscapeCsv(review.Content)}\"";

        }

        // Helper method to escape CSV data by wrapping in quotes and escaping internal quotes
        private string EscapeCsv(string data)
        {
            return data.Replace("\"", "\"\"");  // Replace internal quotes with two quotes for CSV formatting
        }

        public void PrintNegativeReviewsToExcel()  // Method to print all sorted negative reviews in Excel
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; //To be able to write in Excel without paying

            var Reviews = ScrapeReviews();
            BubbleSortNegativeReviews(Reviews); //To retrieve the negative review list

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // File path where the negative reviews will be written, change extension to .xlsx
            string filePath = Path.Combine(docPath, "NegativeReviews.xlsx");

            Console.WriteLine($"File path: {filePath}");

            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();  // Ensures we are creating a new workbook
                }

                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Reviews");

                    // Adding header row
                    worksheet.Cells["A1"].Value = "Author";
                    worksheet.Cells["B1"].Value = "Title";
                    worksheet.Cells["C1"].Value = "StarRating";
                    worksheet.Cells["D1"].Value = "Date";
                    worksheet.Cells["E1"].Value = "Content";

                    int row = 2;  // Start on the second row to skip headers
                    foreach (var review in Reviews)
                    {
                        worksheet.Cells[row, 1].Value = review.Author;
                        worksheet.Cells[row, 2].Value = review.Title;
                        worksheet.Cells[row, 3].Value = review.StarRating;
                        worksheet.Cells[row, 4].Value = review.Date.ToString("yyyy-MM-dd");
                        worksheet.Cells[row, 5].Value = review.Content;
                        row++;
                    }

                    package.Save();  // Save the changes to the file
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Reviews successfully written to Excel file.");
            }
        }

        public void PrintPositiveReviewsToTxt() //Method to print all sorted positive reviews to Txt file
        {
            var Reviews = ScrapeReviews();
            BubbleSortPositiveReviews(Reviews); //To retrieve the sorted positive review list

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // File path where the positive reviews will be written
            string filePath = Path.Combine(docPath, "PositiveReviews.txt");

            Console.WriteLine($"File path: {filePath}");

            try
            {
                using (StreamWriter outputFile = new StreamWriter(filePath))
                {
                    foreach (var review in Reviews)
                    {
                        outputFile.WriteLine(review);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Reviews successfully written to file.");
            }
        }

        public void PrintPositiveReviewsToCsv()  // Method to print all sorted positive reviews in CSV file
        {
            var Reviews = ScrapeReviews();
            BubbleSortPositiveReviews(Reviews); //To retrieve the sorted positive review list

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // File path where the positive reviews will be written, changed extension to .csv
            string filePath = Path.Combine(docPath, "PositiveReviews.csv");

            Console.WriteLine($"File path: {filePath}");

            try
            {
                // Ensure the StreamWriter writes with UTF-8 encoding
                using (StreamWriter outputFile = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    // Write the header line for the CSV file
                    outputFile.WriteLine("Author,Title,StarRating,Date,Content");

                    // Write each review in CSV format
                    foreach (var review in Reviews)
                    {
                        outputFile.WriteLine(FormatForCsv(review));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Reviews successfully written to file.");
            }
        }

        public void PrintPositiveReviewsToExcel()  // Method to print all sorted positive reviews in Excel
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; //To be able to write in Excel without paying

            var Reviews = ScrapeReviews();
            BubbleSortPositiveReviews(Reviews); //To retrieve the sorted positive review list

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // File path where the positive reviews will be written, change extension to .xlsx
            string filePath = Path.Combine(docPath, "PositiveReviews.xlsx");

            Console.WriteLine($"File path: {filePath}");

            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();  // Ensures we are creating a new workbook
                }

                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Reviews");

                    // Adding header row
                    worksheet.Cells["A1"].Value = "Author";
                    worksheet.Cells["B1"].Value = "Title";
                    worksheet.Cells["C1"].Value = "StarRating";
                    worksheet.Cells["D1"].Value = "Date";
                    worksheet.Cells["E1"].Value = "Content";

                    int row = 2;  // Start on the second row to skip headers
                    foreach (var review in Reviews)
                    {
                        worksheet.Cells[row, 1].Value = review.Author;
                        worksheet.Cells[row, 2].Value = review.Title;
                        worksheet.Cells[row, 3].Value = review.StarRating;
                        worksheet.Cells[row, 4].Value = review.Date.ToString("yyyy-MM-dd");
                        worksheet.Cells[row, 5].Value = review.Content;
                        row++;
                    }

                    package.Save();  // Save the changes to the file
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Reviews successfully written to Excel file.");
            }
        }

    }
}



