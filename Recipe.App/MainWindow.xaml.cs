using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Recipe.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Recipe> Recipes { get; set; }
        public ObservableCollection<Recipe> FilteredRecipes { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            InitializeRecipes();
            FilteredRecipes = new ObservableCollection<Recipe>(Recipes);
            RecipeListView.ItemsSource = FilteredRecipes;
        }
        private void InitializeRecipes()
        {
            Recipes = new ObservableCollection<Recipe>
            {
                new Recipe { Name = "Pancakes", Ingredients = "Flour, Milk, Eggs, Sugar", FoodGroup = "Grains", Calories = 350, Steps = new string[] { "Mix ingredients", "Cook on griddle", "Serve with syrup" } },
                new Recipe { Name = "Spaghetti", Ingredients = "Pasta, Tomato Sauce, Ground Beef", FoodGroup = "Grains", Calories = 500, Steps = new string[] { "Boil pasta", "Cook sauce with beef", "Mix and serve" }  },
                new Recipe { Name = "Bread", Ingredients = "Flour, Water, Yeast, Salt", FoodGroup = "Grains", Calories = 150, Steps = new string[] { "Mix ingredients", "Knead dough", "Bake in oven" }  }
            };
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeGrid.Visibility = Visibility.Visible;
            MenuGrid.Visibility = Visibility.Collapsed;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            HomeGrid.Visibility = Visibility.Collapsed;
            MenuGrid.Visibility = Visibility.Visible;
        }

        private void FilterRecipesButton_Click(object sender, RoutedEventArgs e)
        {
            string ingredient = IngredientTextBox.Text.ToLower();
            string foodGroup = FoodGroupTextBox.Text.ToLower();
            bool isCaloriesParsed = int.TryParse(CaloriesTextBox.Text, out int maxCalories);

            var filtered = Recipes.Where(r =>
                (string.IsNullOrEmpty(ingredient) || r.Ingredients.ToLower().Contains(ingredient)) &&
                (string.IsNullOrEmpty(foodGroup) || r.FoodGroup.ToLower().Contains(foodGroup)) &&
                (!isCaloriesParsed || r.Calories <= maxCalories)).ToList();

            FilteredRecipes.Clear();
            filtered.ForEach(r => FilteredRecipes.Add(r));
        }
        private void ShowPieChartButton_Click(object sender, RoutedEventArgs e)
        {
            var plotModel = new PlotModel { Title = "Food Group Distribution" };
            var pieSeries = new PieSeries();

            var foodGroupCounts = FilteredRecipes.GroupBy(r => r.FoodGroup)
                .Select(g => new { FoodGroup = g.Key, Count = g.Count() }).ToList();

            foreach (var group in foodGroupCounts)
            {
                pieSeries.Slices.Add(new PieSlice(group.FoodGroup, group.Count) { IsExploded = true });
            }

            plotModel.Series.Add(pieSeries);

            var plotView = new PlotView
            {
                Model = plotModel,
                Margin = new Thickness(10)
            };

            var chartWindow = new Window
            {
                Title = "Pie Chart",
                Content = plotView,
                Width = 400,
                Height = 400
            };

            chartWindow.ShowDialog();
        
    }
}
}