using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Quest
    {
        /// <summary>
        /// 任务id
        /// </summary>
        public int ID { get; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// 任务描述
        /// </summary>
        public string Description { get; }
        /// <summary>
        /// 任务物品
        /// </summary>       
        public List<ItemQuantity> ItemsToComplete { get; }
        /// <summary>
        /// 经验奖励
        /// </summary>
        public int RewardExperiencePoints { get; }
        /// <summary>
        /// 金钱奖励
        /// </summary>
        public int RewardGold { get; }
        /// <summary>
        /// 物品奖励
        /// </summary>
        public List<ItemQuantity> RewardItems { get; }
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
        public Quest(int id, string name, string description, List<ItemQuantity> itemsToComplete,
                     int rewardExperiencePoints, int rewardGold, List<ItemQuantity> rewardItems)
        {
            ID = id;
            Name = name;
            Description = description;
            ItemsToComplete = itemsToComplete;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
            RewardItems = rewardItems;
        }
    }
}
