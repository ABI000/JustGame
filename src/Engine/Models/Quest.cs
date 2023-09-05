using Newtonsoft.Json;

namespace Engine.Models
{
    public class Quest
    {
        /// <summary>
        /// 任务id
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// 任务名称
        /// </summary>
        [JsonIgnore]
        public string Name { get; }

        /// <summary>
        /// 任务描述
        /// </summary>
        [JsonIgnore]
        public string Description { get; }
        /// <summary>
        /// 任务物品
        /// </summary>    
        [JsonIgnore]
        public List<ItemQuantity> ItemsToComplete { get; }
        /// <summary>
        /// 经验奖励
        /// </summary>
        [JsonIgnore]
        public int RewardExperiencePoints { get; }
        /// <summary>
        /// 金钱奖励
        /// </summary>
        [JsonIgnore]
        public int RewardGold { get; }
        /// <summary>
        /// 物品奖励
        /// </summary>
        [JsonIgnore]
        public List<ItemQuantity> RewardItems { get; }
        [JsonIgnore]
        public string ToolTipContents =>
            Description + Environment.NewLine + Environment.NewLine +
            "Items to complete the quest" + Environment.NewLine +
            "===========================" + Environment.NewLine +
            string.Join(Environment.NewLine, ItemsToComplete.Select(i => i.QuantityItemDescription)) +
            Environment.NewLine + Environment.NewLine +
            "Rewards\r\n" +
            "===========================" + Environment.NewLine +
            $"{RewardExperiencePoints} experience points" + Environment.NewLine +
            $"{RewardGold} gold pieces" + Environment.NewLine +
            string.Join(Environment.NewLine, RewardItems.Select(i => i.QuantityItemDescription));

        /// <summary>
        /// 初始化任务
        /// </summary>
        /// <param name="id">任务id</param>
        /// <param name="name">任务名称</param>
        /// <param name="description">任务描述</param>
        /// <param name="itemsToComplete">任务物品</param>
        /// <param name="rewardExperiencePoints">经验奖励</param>
        /// <param name="rewardGold">金钱奖励</param>
        /// <param name="rewardItems">物品奖励</param>
        public Quest(int id, string name, string description,
                     int rewardExperiencePoints, int rewardGold, List<ItemQuantity>? itemsToComplete = null, List<ItemQuantity>? rewardItems = null)
        {
            Id = id;
            Name = name;
            Description = description;
            ItemsToComplete = itemsToComplete ?? new List<ItemQuantity>();
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
            RewardItems = rewardItems ?? new List<ItemQuantity>();
        }

        public void AddItemToComplete(int itemID, int quantity)
        {
            ItemsToComplete.Add(new ItemQuantity(itemID, quantity));
        }
        public void AddRewardItem(int itemID, int quantity)
        {
            RewardItems.Add(new ItemQuantity(itemID, quantity));
        }
    }
}
