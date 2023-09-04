using Engine.Actions;
using Engine.Models;
using System.Text.Json;
namespace Engine.Factories
{
    public static class ItemFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\GameItems.json";
        private static readonly List<GameItem> _standardGameItems = new List<GameItem>();
        static ItemFactory()
        {
            //读取配置文件

            GameItems gameItems = JsonHelpr.GetEntity<GameItems>(GAME_DATA_FILENAME);
            foreach (Weapons gameItem in gameItems.Weapons)
            {
                BuildWeapon(gameItem.ItemTypeID, gameItem.Name, gameItem.Price, gameItem.MinimumDamage, gameItem.MaximumDamage);
            }
            foreach (GameItem gameItem in gameItems.MiscellaneousItems)
            {
                BuildMiscellaneousItem(gameItem.ItemTypeID, gameItem.Name, gameItem.Price);
            }
            foreach (HealingItems gameItem in gameItems.HealingItems)
            {
                BuildHealingItem(gameItem.ItemTypeID, gameItem.Name, gameItem.Price, gameItem.HitPointsToHeal);
            }
        }
        public static GameItem? CreateGameItem(int itemTypeID)
        {
            return _standardGameItems.FirstOrDefault(item => item.ItemTypeID == itemTypeID)!.Clone();
        }
        /// <summary>
        /// 普通物品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        private static void BuildMiscellaneousItem(int id, string name, int price)
        {
            _standardGameItems.Add(new GameItem(ItemCategory.Miscellaneous, id, name, price));
        }
        /// <summary>
        /// 武器
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <param name="minimumDamage"></param>
        /// <param name="maximumDamage"></param>
        private static void BuildWeapon(int id, string name, int price, int minimumDamage, int maximumDamage)
        {
            GameItem weapon = new(ItemCategory.Weapon, id, name, price, true);
            weapon.Action = new AttackWithWeapon(weapon, minimumDamage, maximumDamage);
            _standardGameItems.Add(weapon);
        }
        /// <summary>
        /// 消耗品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <param name="hitPointsToHeal"></param>
        private static void BuildHealingItem(int id, string name, int price, int hitPointsToHeal)
        {
            GameItem item = new GameItem(ItemCategory.Consumable, id, name, price);
            item.Action = new Heal(item, hitPointsToHeal);
            _standardGameItems.Add(item);
        }
        public static string ItemName(int itemTypeID)
        {
            return _standardGameItems.FirstOrDefault(i => i.ItemTypeID == itemTypeID)?.Name ?? "";
        }
    }

    public class GameItems
    {
        public List<GameItem> MiscellaneousItems { get; set; }
        public List<Weapons> Weapons { get; set; }
        public List<HealingItems> HealingItems { get; set; }
    }

    public class Weapons : GameItem
    {
        public int MaximumDamage { get; set; }
        public int MinimumDamage { get; set; }
        public Weapons(ItemCategory category, int itemTypeID, string name, int price, bool isUnique = false, IAction action = null) : base(category, itemTypeID, name, price, isUnique, action)
        {
        }
    }
    public class HealingItems : GameItem
    {
        public int HitPointsToHeal { get; set; }
        public HealingItems(ItemCategory category, int itemTypeID, string name, int price, bool isUnique = false, IAction action = null) : base(category, itemTypeID, name, price, isUnique, action)
        {
        }
    }

}
