using Engine.Models;

namespace Engine.Factories
{
    internal static class QuestFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Quest.json";
        private static readonly List<Quest> _quests = new List<Quest>();
        static QuestFactory()
        {
            List<Quests> quests = JsonHelpr.GetEntity<List<Quests>>(GAME_DATA_FILENAME);

            foreach (var item in quests)
            {
                var quest = new Quest(item.Id, item.Name, item.Description, item.RewardExperiencePoints, item.RewardGold);
                foreach (var ingredient in item.ItemsToComplete)
                {
                    quest.AddItemToComplete(ingredient.Id, ingredient.Quantity);
                }
                foreach (var outputItem in item.RewardItems)
                {
                    quest.AddRewardItem(outputItem.Id, outputItem.Quantity);
                }
                _quests.Add(quest);
            }
            // Declare the items need to complete the quest, and its reward items
            //List<ItemQuantity> itemsToComplete = new();
            //List<ItemQuantity> rewardItems = new();
            //itemsToComplete.Add(new ItemQuantity(9001, 5));
            //rewardItems.Add(new ItemQuantity(1002, 1));
            //// Create the quest
            //_quests.Add(new Quest(1,
            //                      "Clear the herb garden",
            //                      "Defeat the snakes in the Herbalist's garden",
            //                      itemsToComplete,
            //                      25, 10,
            //                      rewardItems));
        }
        internal static Quest GetQuestByID(int id)
        {
            return _quests.FirstOrDefault(quest => quest.Id == id);
        }

        public class Quests
        {
            public int Id { get; set; }
            public int RewardExperiencePoints { get; set; }
            public int RewardGold { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public List<ItemQuantityGameData> ItemsToComplete { get; set; }
            public List<ItemQuantityGameData> RewardItems { get; set; }
        }
    }
}
