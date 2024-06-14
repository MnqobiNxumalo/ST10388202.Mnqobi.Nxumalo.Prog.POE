using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe.App
{
    public class Recipe
    {
        public string Name { get; set; }
        public string  Ingredients { get; set; }
        public string FoodGroup { get; set; }
        public int Calories { get; set;}
        public string[] Steps { get; set; }

    }
}
