using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using OfficeOpenXml;
using System.IO;
using System.Text;

namespace GUIPM
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string currentTask;
        private string selectedOption;
        private string urlToScrape;
        private string searchKeyword;
        private string keywordToSearch;
        private bool isUrlInputVisible;
        private bool isSearchInputVisible;
        private bool isCategorizeInputVisible;
        private ObservableCollection<Review> reviews;
        private ObservableCollection<Review> searchResults;
        private ObservableCollection<Review> positiveReviews;
        private ObservableCollection<Review> negativeReviews;

        public string CurrentTask
        {
            get { return currentTask; }
            set
            {
                currentTask = value;
                OnPropertyChanged(nameof(CurrentTask));
            }
        }

        public string SelectedOption
        {
            get { return selectedOption; }
            set
            {
                selectedOption = value;
                OnPropertyChanged(nameof(SelectedOption));
                OnSelectedOptionChanged();
            }
        }

        public string UrlToScrape
        {
            get { return urlToScrape; }
            set
            {
                urlToScrape = value;
                OnPropertyChanged(nameof(UrlToScrape));
            }
        }

        public string SearchKeyword
        {
            get { return searchKeyword; }
            set
            {
                searchKeyword = value;
                OnPropertyChanged(nameof(SearchKeyword));
            }
        }

        public string KeywordToSearch
        {
            get { return keywordToSearch; }
            set
            {
                keywordToSearch = value;
                OnPropertyChanged(nameof(KeywordToSearch));
            }
        }

        public bool IsUrlInputVisible
        {
            get { return isUrlInputVisible; }
            set
            {
                isUrlInputVisible = value;
                OnPropertyChanged(nameof(IsUrlInputVisible));
            }
        }

        public bool IsSearchInputVisible
        {
            get { return isSearchInputVisible; }
            set
            {
                isSearchInputVisible = value;
                OnPropertyChanged(nameof(IsSearchInputVisible));
            }
        }

        public bool IsCategorizeInputVisible
        {
            get { return isCategorizeInputVisible; }
            set
            {
                isCategorizeInputVisible = value;
                OnPropertyChanged(nameof(IsCategorizeInputVisible));
            }
        }

        public ObservableCollection<Review> Reviews
        {
            get { return reviews; }
            set
            {
                reviews = value;
                OnPropertyChanged(nameof(Reviews));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ObservableCollection<Review> SearchResults
        {
            get { return searchResults; }
            set
            {
                searchResults = value;
                OnPropertyChanged(nameof(SearchResults));
            }
        }

        public ObservableCollection<Review> PositiveReviews
        {
            get { return positiveReviews; }
            set
            {
                positiveReviews = value;
                OnPropertyChanged(nameof(PositiveReviews));
            }
        }

        public ObservableCollection<Review> NegativeReviews
        {
            get { return negativeReviews; }
            set
            {
                negativeReviews = value;
                OnPropertyChanged(nameof(NegativeReviews));
            }
        }

        public ObservableCollection<string> Options { get; }

        public ICommand SelectOptionCommand { get; }
        public ICommand ScrapeCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand CategorizeCommand { get; }
        public ICommand SortAllReviewsCommand { get; }
        public ICommand SortPositiveReviewsCommand { get; }
        public ICommand SortNegativeReviewsCommand { get; }
        public ICommand PrintAllReviewsToTxtCommand { get; }
        public ICommand PrintAllReviewsToCsvCommand { get; }
        public ICommand PrintAllReviewsToExcelCommand { get; }
        public ICommand PrintPositiveReviewsToTxtCommand { get; }
        public ICommand PrintPositiveReviewsToCsvCommand { get; }
        public ICommand PrintPositiveReviewsToExcelCommand { get; }
        public ICommand PrintNegativeReviewsToTxtCommand { get; }
        public ICommand PrintNegativeReviewsToCsvCommand { get; }
        public ICommand PrintNegativeReviewsToExcelCommand { get; }
        public ICommand SearchKeywordCommand { get; }
        public ICommand ExitCommand { get; }

        public MainViewModel()
        {
            Console.WriteLine("MainViewModel initialized.");

            Options = new ObservableCollection<string>
            {
            "1. Scraping the website",
            "2. Searching the reviews",
            "3. Categorizing the reviews",
            "4. Sorting the reviews",
            "5. Printing the reviews",
            "6. Search for keywords",
            "7. Exit"
            };

            SelectOptionCommand = new RelayCommand(SelectOption);
            ScrapeCommand = new RelayCommand(ScrapeWebsite, CanExecuteScrape);
            SearchCommand = new RelayCommand(SearchReviews, CanExecuteSearch);
            CategorizeCommand = new RelayCommand(async () => await CategorizeReviews(), CanExecuteCategorize);
            SortAllReviewsCommand = new RelayCommand(SortAllReviews, CanExecuteSortAll);
            SortPositiveReviewsCommand = new RelayCommand(SortPositiveReviews, CanExecuteSort);
            SortNegativeReviewsCommand = new RelayCommand(SortNegativeReviews, CanExecuteSort);
            PrintAllReviewsToTxtCommand = new RelayCommand(PrintAllReviewsToTxt, CanExecutePrint);
            PrintAllReviewsToCsvCommand = new RelayCommand(PrintAllReviewsToCsv, CanExecutePrint);
            PrintAllReviewsToExcelCommand = new RelayCommand(PrintAllReviewsToExcel, CanExecutePrint);
            PrintPositiveReviewsToTxtCommand = new RelayCommand(PrintPositiveReviewsToTxt, CanExecutePrint);
            PrintPositiveReviewsToCsvCommand = new RelayCommand(PrintPositiveReviewsToCsv, CanExecutePrint);
            PrintPositiveReviewsToExcelCommand = new RelayCommand(PrintPositiveReviewsToExcel, CanExecutePrint);
            PrintNegativeReviewsToTxtCommand = new RelayCommand(PrintNegativeReviewsToTxt, CanExecutePrint);
            PrintNegativeReviewsToCsvCommand = new RelayCommand(PrintNegativeReviewsToCsv, CanExecutePrint);
            PrintNegativeReviewsToExcelCommand = new RelayCommand(PrintNegativeReviewsToExcel, CanExecutePrint);
            SearchKeywordCommand = new RelayCommand(ExecuteSearchKeyword, CanExecuteSearchKeyword); // Updated here
            ExitCommand = new RelayCommand(ExitApplication);

            Reviews = new ObservableCollection<Review>();
            SearchResults = new ObservableCollection<Review>();
            PositiveReviews = new ObservableCollection<Review>();
            NegativeReviews = new ObservableCollection<Review>();

            Console.WriteLine("Commands initialized.");
    }


        private void OnSelectedOptionChanged()
        {
            IsUrlInputVisible = SelectedOption == "1. Scraping the website";
            IsSearchInputVisible = SelectedOption == "2. Searching the reviews";
            IsCategorizeInputVisible = SelectedOption == "3. Categorizing the reviews";
        }

        private void SelectOption()
        {
            if (SelectedOption == null)
            {
                CurrentTask = "No option selected. Please choose an option.";
                return;
            }

            CurrentTask = $"You selected: {SelectedOption}";
        }

        private bool CanExecuteScrape()
        {
            return !string.IsNullOrWhiteSpace(UrlToScrape);
        }

        private async void ScrapeWebsite()
        {
            if (string.IsNullOrWhiteSpace(UrlToScrape))
            {
                MessageBox.Show("Please enter a valid URL to scrape.");
                return;
            }

            Reviews reviewsInstance = new Reviews(UrlToScrape);
            var allReviews = await Task.Run(() => reviewsInstance.ScrapeReviews());
            Reviews.Clear();
            foreach (var review in allReviews)
            {
                Reviews.Add(review);
            }
            CurrentTask = $"Scraped {allReviews.Count} reviews from {UrlToScrape}";
        }

        private bool CanExecuteSearch()
        {
            return Reviews.Any() && !string.IsNullOrWhiteSpace(SearchKeyword);
        }

        private void SearchReviews()
        {
            if (Reviews == null || !Reviews.Any())
            {
                MessageBox.Show("Please scrape a website first before searching.");
                return;
            }

            if (string.IsNullOrWhiteSpace(SearchKeyword))
            {
                MessageBox.Show("Please enter a keyword to search for.");
                return;
            }

            var searchResults = Reviews.Where(review => review.Content.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase)).ToList();
            SearchResults.Clear();
            foreach (var review in searchResults)
            {
                SearchResults.Add(review);
            }
            CurrentTask = $"Search results for '{SearchKeyword}': {searchResults.Count} reviews found.";
        }

        private bool CanExecuteCategorize()
        {
            Console.WriteLine($"CanExecuteCategorize called. Reviews count: {Reviews?.Count}");
            return Reviews != null && Reviews.Any();
        }

        private async Task CategorizeReviews()
        {
            Console.WriteLine("CategorizeReviews called.");
            if (Reviews == null || !Reviews.Any())
            {
                MessageBox.Show("Please scrape a website first before categorizing.");
                return;
            }

            await Task.Run(() =>
            {
                var negativeReviews = Reviews.Where(review => IsNegative(review)).ToList();
                var positiveReviews = Reviews.Where(review => IsPositive(review)).ToList();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    PositiveReviews.Clear();
                    foreach (var review in positiveReviews)
                    {
                        PositiveReviews.Add(review);
                    }

                    NegativeReviews.Clear();
                    foreach (var review in negativeReviews)
                    {
                        NegativeReviews.Add(review);
                    }

                    CurrentTask = $"Categorized {PositiveReviews.Count} positive and {NegativeReviews.Count} negative reviews.";
                });
            });
        }

        private bool CanExecuteSort()
        {
            return PositiveReviews.Any() || NegativeReviews.Any();
        }

        private bool CanExecuteSortAll()
        {
            return Reviews.Any();
        }

        private void SortAllReviews()
        {
            var sortedAllReviews = Reviews.ToList();
            BubbleSortReviews(sortedAllReviews);
            Reviews.Clear();
            foreach (var review in sortedAllReviews)
            {
                Reviews.Add(review);
            }
            CurrentTask = "Sorted all reviews.";
        }

        private void SortPositiveReviews()
        {
            var sortedPositiveReviews = PositiveReviews.ToList();
            BubbleSortReviews(sortedPositiveReviews);
            PositiveReviews.Clear();
            foreach (var review in sortedPositiveReviews)
            {
                PositiveReviews.Add(review);
            }
            CurrentTask = "Sorted positive reviews.";
        }

        private void SortNegativeReviews()
        {
            var sortedNegativeReviews = NegativeReviews.ToList();
            BubbleSortReviews(sortedNegativeReviews);
            NegativeReviews.Clear();
            foreach (var review in sortedNegativeReviews)
            {
                NegativeReviews.Add(review);
            }
            CurrentTask = "Sorted negative reviews.";
        }

        private void BubbleSortReviews(List<Review> reviews)
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

            // Print the sorted list of reviews after the sorting process is completed
            foreach (Review review in reviews)
            {
                Console.WriteLine(review);
            }
        }

        private bool IsNegative(Review review)
        {
            return review.StarRating.Length > 11 && (review.StarRating[11] == '1' || review.StarRating[11] == '2' || review.StarRating[11] == '3');
        }

        private bool IsPositive(Review review)
        {
            return review.StarRating.Length > 11 && (review.StarRating[11] == '4' || review.StarRating[11] == '5');
        }

        private bool CanExecutePrint()
        {
            return Reviews.Any();
        }

        private void PrintAllReviewsToTxt()
        {
            WriteReviewsToTxt(Reviews.ToList(), "Reviews.txt");
        }

        private void PrintAllReviewsToCsv()
        {
            WriteReviewsToCsv(Reviews.ToList(), "Reviews.csv");
        }

        private void PrintAllReviewsToExcel()
        {
            WriteReviewsToExcel(Reviews.ToList(), "Reviews.xlsx");
        }

        private void PrintPositiveReviewsToTxt()
        {
            WriteReviewsToTxt(PositiveReviews.ToList(), "PositiveReviews.txt");
        }

        private void PrintPositiveReviewsToCsv()
        {
            WriteReviewsToCsv(PositiveReviews.ToList(), "PositiveReviews.csv");
        }

        private void PrintPositiveReviewsToExcel()
        {
            WriteReviewsToExcel(PositiveReviews.ToList(), "PositiveReviews.xlsx");
        }

        private void PrintNegativeReviewsToTxt()
        {
            WriteReviewsToTxt(NegativeReviews.ToList(), "NegativeReviews.txt");
        }

        private void PrintNegativeReviewsToCsv()
        {
            WriteReviewsToCsv(NegativeReviews.ToList(), "NegativeReviews.csv");
        }

        private void PrintNegativeReviewsToExcel()
        {
            WriteReviewsToExcel(NegativeReviews.ToList(), "NegativeReviews.xlsx");
        }

        private void WriteReviewsToTxt(List<Review> reviews, string fileName)
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = Path.Combine(docPath, fileName);

            try
            {
                using (StreamWriter outputFile = new StreamWriter(filePath))
                {
                    foreach (var review in reviews)
                    {
                        outputFile.WriteLine(review);
                    }
                }
                CurrentTask = $"Reviews successfully written to {fileName}.";
            }
            catch (Exception e)
            {
                CurrentTask = "Exception: " + e.Message;
            }
        }

        private void WriteReviewsToCsv(List<Review> reviews, string fileName)
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = Path.Combine(docPath, fileName);

            try
            {
                using (StreamWriter outputFile = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    outputFile.WriteLine("Author,Title,StarRating,Date,Content");
                    foreach (var review in reviews)
                    {
                        outputFile.WriteLine(FormatForCsv(review));
                    }
                }
                CurrentTask = $"Reviews successfully written to {fileName}.";
            }
            catch (Exception e)
            {
                CurrentTask = "Exception: " + e.Message;
            }
        }

        private void WriteReviewsToExcel(List<Review> reviews, string fileName)
        {
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;  // Fully qualify LicenseContext
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = Path.Combine(docPath, fileName);

            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }

                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Reviews");

                    worksheet.Cells["A1"].Value = "Author";
                    worksheet.Cells["B1"].Value = "Title";
                    worksheet.Cells["C1"].Value = "StarRating";
                    worksheet.Cells["D1"].Value = "Date";
                    worksheet.Cells["E1"].Value = "Content";

                    int row = 2;
                    foreach (var review in reviews)
                    {
                        worksheet.Cells[row, 1].Value = review.Author;
                        worksheet.Cells[row, 2].Value = review.Title;
                        worksheet.Cells[row, 3].Value = review.StarRating;
                        worksheet.Cells[row, 4].Value = review.Date.ToString("yyyy-MM-dd");
                        worksheet.Cells[row, 5].Value = review.Content;
                        row++;
                    }

                    package.Save();
                }
                CurrentTask = $"Reviews successfully written to {fileName}.";
            }
            catch (Exception e)
            {
                CurrentTask = "Exception: " + e.Message;
            }
        }

        private string FormatForCsv(Review review)
        {
            return $"\"{EscapeCsv(review.Author)}\",\"{EscapeCsv(review.Title)}\",\"{review.StarRating}\",\"{review.Date.ToString("yyyy-MM-dd")}\",\"{EscapeCsv(review.Content)}\"";
        }

        private string EscapeCsv(string data)
        {
            return data.Replace("\"", "\"\"");
        }

        private bool CanExecuteSearchKeyword()
        {
            return !string.IsNullOrWhiteSpace(KeywordToSearch);
        }

        private void ExecuteSearchKeyword()
        {
            Console.WriteLine("ExecuteSearchKeyword method called.");

            if (string.IsNullOrWhiteSpace(KeywordToSearch))
            {
                MessageBox.Show("Please enter a keyword to search for.");
                return;
            }

            var reviewsInstance = new Reviews(UrlToScrape);
            var allReviews = reviewsInstance.ScrapeReviews();
            int count = 0;

            foreach (var review in allReviews)
            {
                if (review.Content.ToLower().Contains(KeywordToSearch.ToLower()))
                {
                    count++;
                }
            }

            CurrentTask = $"The keyword '{KeywordToSearch}' appears {count} times in the reviews.";
            Console.WriteLine(CurrentTask);
        }

        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            CommandManager.InvalidateRequerySuggested();
        }
    }
}

public class RelayCommand : ICommand
{
    private readonly Action execute;
    private readonly Func<bool> canExecute;

    public RelayCommand(Action execute, Func<bool> canExecute = null)
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute;
    }

    public bool CanExecute(object parameter) => canExecute == null || canExecute();

    public void Execute(object parameter) => execute();

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}
