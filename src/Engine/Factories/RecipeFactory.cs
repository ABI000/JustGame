using Engine.Models;

namespace Engine.Factories
{
    public static class RecipeFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Recipe.json";
        private static readonly List<Recipe> _recipes = new();
        static RecipeFactory()
        {
            List<RecipeGameData> monsterGameDatas = JsonHelpr.GetEntity<List<RecipeGameData>>(GAME_DATA_FILENAME);

            foreach (var item in monsterGameDatas)
            {
                var recipe = new Recipe(item.Id, item.Name);
                foreach (var ingredient in item.Ingredients)
                {
                    recipe.AddIngredient(ingredient.Id, ingredient.Quantity);
                }
                foreach (var outputItem in item.OutputItems)
                {
                    recipe.AddOutputItem(outputItem.Id, outputItem.Quantity);
                }
                _recipes.Add(recipe);
            }
        }
        public static Recipe RecipeByID(int id)
        {
            return _recipes.FirstOrDefault(x => x.ID == id);
        }
    }


    public class RecipeGameData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ItemQuantityGameData> Ingredients { get; set; }
        public List<ItemQuantityGameData> OutputItems { get; set; }

    }
    public class ItemQuantityGameData
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}
