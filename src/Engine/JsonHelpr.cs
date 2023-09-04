using System.Text.Json;

namespace Engine
{
    public static class JsonHelpr
    {
        public static T GetEntity<T>(string path)
        {
            string jsonString = File.ReadAllText(path);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Missing data file: {Path.GetFileName(path)}");
            }
            return JsonSerializer.Deserialize<T>(jsonString)!;

        }


    }
}
