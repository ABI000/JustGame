using Engine.Models;

namespace Engine.Factories
{
    public static class MonsterFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Monster.json";
        private static readonly List<Monster> _baseMonsters = new();
        static MonsterFactory()
        {
            List<MonsterGameData> monsterGameDatas = JsonHelpr.GetEntity<List<MonsterGameData>>(GAME_DATA_FILENAME);

            foreach (var item in monsterGameDatas)
            {
                _baseMonsters.Add(new Monster(item.Id, item.Name, $".\\Images\\Monsters\\{item.ImageName}", item.MaximumHitPoints, ItemFactory.CreateGameItem(item.WeaponID), item.RewardXP, item.Gold));
            }
        }
        public static Monster GetMonster(int id)
        {
            return _baseMonsters.FirstOrDefault(m => m.Id == id)?.GetNewInstance();
        }
    }


    public class MonsterGameData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaximumHitPoints { get; set; }
        public int WeaponID { get; set; }
        public int RewardXP { get; set; }
        public int Gold { get; set; }
        public string ImageName { get; set; }
        public List<GameItem> LootItems { get; set; }
    }
}
