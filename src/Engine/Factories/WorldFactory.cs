using Engine.Models;

namespace Engine.Factories
{
    internal static class WorldFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\World.json";
        internal static World CreateWorld()
        {
            World newWorld = new();
            Worlds worlds = JsonHelpr.GetEntity<Worlds>(GAME_DATA_FILENAME);

            foreach (var item in worlds.Locations)
            {
                newWorld.AddLocation(item.XCoordinate, item.YCoordinate, item.Name, item.Description, item.ImageName);
            }

            foreach (var item in worlds.MonsterSpawnAreas)
            {
                newWorld.LocationAt(item.XCoordinate, item.YCoordinate).AddMonster(item.Id, item.ChanceOfEncountering);
            }
            foreach (var item in worlds.QuestHubs)
            {
                newWorld.LocationAt(item.XCoordinate, item.YCoordinate).AddQuest(QuestFactory.GetQuestByID(item.Id));
            }
            foreach (var item in worlds.TraderSpawnPoints)
            {
                newWorld.LocationAt(item.XCoordinate, item.YCoordinate).AddTrader(TraderFactory.GetTraderByName(item.Name));
            }
            return newWorld;
        }
    }

    public class Worlds
    {
        public List<Locations> Locations { get; set; }
        public List<BaseWorldData>? QuestHubs { get; set; }
        public List<MonsterSpawnArea>? MonsterSpawnAreas { get; set; }
        public List<BaseWorldData>? TraderSpawnPoints { get; set; }
    }
    public class MonsterSpawnArea : BaseWorldData
    {
        public int ChanceOfEncountering { get; set; }
    }
    public class BaseWorldData
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        /// <summary>
        /// 横坐标
        /// </summary>
        public int XCoordinate { get; set; }
        /// <summary>
        /// 纵坐标
        /// </summary>
        public int YCoordinate { get; set; }
    }
    public class Locations: BaseWorldData
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string ImageName { get; set; }

    }

}
