using Engine.Actions;

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
        public ItemCategory Category { get; }
        /// <summary>
        /// 物品id
        /// </summary>
        public int ItemTypeID { get; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 价格
        /// </summary>
        public int Price { get; }
        /// <summary>
        /// 是否唯一
        /// </summary>
        public bool IsUnique { get; }
        public AttackWithWeapon Action { get; set; }
        public GameItem(int itemTypeID, string name, int price, bool isUnique = false, AttackWithWeapon action = null, ItemCategory category = ItemCategory.Miscellaneous)
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
            return new GameItem(ItemTypeID, Name, Price, IsUnique, Action, Category);
        }
    }
    public enum ItemCategory
    {
        Miscellaneous,
        Weapon
    }
}
