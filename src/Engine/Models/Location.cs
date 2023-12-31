﻿using Engine.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Location
    {
        /// <summary>
        /// 横坐标
        /// </summary>
        public int XCoordinate { get; }
        /// <summary>
        /// 纵坐标
        /// </summary>
        public int YCoordinate { get; }
        /// <summary>
        /// 地名
        /// </summary>
        [JsonIgnore]
        public string Name { get; }
        /// <summary>
        /// 描述
        /// </summary>
        [JsonIgnore]
        public string Description { get; }
        /// <summary>
        /// 图片
        /// </summary>
        [JsonIgnore]
        public string ImageName { get; }

        [JsonIgnore]
        /// <summary>
        /// 在图任务
        /// </summary>
        public List<Quest> QuestsAvailableHere { get; } = new List<Quest>();
        /// <summary>
        /// 交易人员
        /// </summary>
        [JsonIgnore]
        public Trader TraderHere { get; set; }
        /// <summary>
        /// 怪物
        /// </summary>
        [JsonIgnore]
        public List<MonsterEncounter> MonstersHere { get; set; } =
            new List<MonsterEncounter>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xCoordinate"></param>
        /// <param name="yCoordinate"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="imageName"></param>
        public Location(int xCoordinate, int yCoordinate, string name, string description, string imageName)
        {
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
            Name = name;
            Description = description;
            ImageName = $".\\Images\\Locations\\{imageName}";
        }
        public void AddMonster(int monsterID, int chanceOfEncountering)
        {
            if (MonstersHere.Exists(m => m.MonsterID == monsterID))
            {
                // This monster has already been added to this location.
                // So, overwrite the ChanceOfEncountering with the new number.
                MonstersHere.First(m => m.MonsterID == monsterID).ChanceOfEncountering = chanceOfEncountering;
            }
            else
            {
                // This monster is not already at this location, so add it.
                MonstersHere.Add(new MonsterEncounter(monsterID, chanceOfEncountering));
            }
        }
        public void AddTrader(Trader trader)
        {
            TraderHere = trader;
        }
        public void AddQuest(Quest quest)
        {
            if (!QuestsAvailableHere.Any(x => x.Id == quest.Id))
            {
                QuestsAvailableHere.Add(quest);
            }
        }

        public Monster GetMonster()
        {
            if (!MonstersHere.Any())
            {
                return null;
            }
            // Total the percentages of all monsters at this location.
            int totalChances = MonstersHere.Sum(m => m.ChanceOfEncountering);
            // Select a random number between 1 and the total (in case the total chances is not 100).
            int randomNumber = RandomNumberGenerator.NumberBetween(1, totalChances);
            // Loop through the monster list, 
            // adding the monster's percentage chance of appearing to the runningTotal variable.
            // When the random number is lower than the runningTotal,
            // that is the monster to return.
            int runningTotal = 0;
            foreach (MonsterEncounter monsterEncounter in MonstersHere)
            {
                runningTotal += monsterEncounter.ChanceOfEncountering;
                if (randomNumber <= runningTotal)
                {
                    return MonsterFactory.GetMonster(monsterEncounter.MonsterID);
                }
            }
            // If there was a problem, return the last monster in the list.
            return MonsterFactory.GetMonster(MonstersHere.Last().MonsterID);
        }
    }

}
