using Newtonsoft.Json;

namespace Core.Utils
{
    public class JsonFileUtils
    {
        public class JsonFileUltils
    {
        public static string ReadJsonFile(string path)
        {
            if (!Directory.Exists(path))
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), path);

                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("Can't find file " + path);
                }
            }

            return File.ReadAllText(path);
        }

        public static T ReadJsonAndParse<T>(string path) where T : class
        {
            var jsonContent = ReadJsonFile(path);
            return JsonConvert.DeserializeObject<T>(jsonContent);
        }
    }
    }
}