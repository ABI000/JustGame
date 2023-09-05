using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Engine.Factories
{
    /// <summary>
    /// 交易者工厂
    /// </summary>
    public static class TraderFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Trader.json";
        private static readonly List<Trader> _traders = new List<Trader>();
        static TraderFactory()
        {
            List<TraderGameData> traderGameDatas = JsonHelpr.GetEntity<List<TraderGameData>>(GAME_DATA_FILENAME);
            foreach (var data in traderGameDatas)
            {
                Trader trader = new Trader(data.Id, data.Name);
                foreach (var traderGameData in data.InventoryItems)
                {
                    trader.AddItemToInventory(ItemFactory.CreateGameItem(traderGameData.Id));
                }
                AddTraderToList(trader);
            }
        }
        public static Trader GetTraderByName(string name)
        {
            return _traders.FirstOrDefault(t => t.Name == name);
        }
        private static void AddTraderToList(Trader trader)
        {
            if (_traders.Any(t => t.Name == trader.Name))
            {
                throw new ArgumentException($"There is already a trader named '{trader.Name}'");
            }
            _traders.Add(trader);
        }
    }
    public class TraderGameData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ItemQuantityGameData> InventoryItems { get; set; }

    }
}
