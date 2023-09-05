using Engine.Factories;
using Newtonsoft.Json;

namespace Engine
{
    public static class JsonHelpr
    {
        public static T GetEntity<T>(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Missing data file: {Path.GetFileName(path)}");
            }
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path))!;

        }


    }
}
