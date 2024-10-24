using System.Text.Json;

namespace ConsoleApp1
{
    public static class Serializer<T>
    {
        private static string FileName => $"{typeof(T).Name}.json";

        public static async Task SerializeObject(T instance)
        {
            List<T> objects = new List<T>();
            if (File.Exists(FileName))
            {
                objects = await DeserializeObjects();
            }
            objects.Add(instance);
            await using var stream = File.Create(FileName);
            await JsonSerializer.SerializeAsync(stream, objects);
        }

        public static async Task<List<T>> DeserializeObjects()
        {
            await using var stream = File.OpenRead(FileName);
            return await JsonSerializer.DeserializeAsync<List<T>>(stream) ?? new List<T>();
        }
    }
}