﻿using Engine.Actions;
using Newtonsoft.Json;

namespace Engine.Models
{
    /// <summary>
    /// 物品
    /// </summary>
    public class GameItem
    {
        /// <summary>
        /// 物品类型
        /// </summary>
        [JsonIgnore]
        public ItemCategory Category { get; }
        /// <summary>
        /// 物品id
        /// </summary>
        public int ItemTypeID { get; }
        /// <summary>
        /// 名称
        /// </summary>
        [JsonIgnore]
        public string Name { get; }
        /// <summary>
        /// 价格
        /// </summary>
        [JsonIgnore]
        public int Price { get; }
        /// <summary>
        /// 是否唯一
        /// </summary>
        [JsonIgnore]
        public bool IsUnique { get; }
        [JsonIgnore]
        public IAction Action { get; set; }
        public GameItem(ItemCategory category, int itemTypeID, string name, int price, bool isUnique = false, IAction action = null)
        {
            ItemTypeID = itemTypeID;
            Name = name;
            Price = price;
            IsUnique = isUnique;
            Category = category;
            Action = action;
        }
        public void PerformAction(LivingEntity actor, LivingEntity target)
        {
            Action?.Execute(actor, target);
        }
        public GameItem Clone()
        {
            return new GameItem(Category, ItemTypeID, Name, Price, IsUnique, Action);
        }
    }
    public enum ItemCategory
    {
        Miscellaneous,
        Weapon,
        Consumable
    }
}
