using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp1.Services
{
    public class SerializationManager
    {
        public static readonly string XmlFilePath = "Instances.xml";
        public static readonly string JsonFilePath = "Instances.json";

        public static async Task SerializeToXmlAsync<T>(List<T> items)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            await using var writer = new FileStream(XmlFilePath, FileMode.Create);
            serializer.Serialize(writer, items);
        }

        public static async Task<List<T>> DeserializeFromXmlAsync<T>()
        {
            try
            {
                if (!File.Exists(XmlFilePath))
                    return new List<T>();

                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                await using var reader = new FileStream(XmlFilePath, FileMode.Open);
                return (List<T>)serializer.Deserialize(reader) ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during XML deserialization: {ex.Message}");
                return new List<T>();
            }
        }

        public static async Task SerializeToJsonAsync<T>(List<T> items)
        {
            await using var stream = File.Create(JsonFilePath);
            await JsonSerializer.SerializeAsync(stream, items, new JsonSerializerOptions { WriteIndented = true });
        }

        public static async Task<List<T>> DeserializeFromJsonAsync<T>()
        {
            if (!File.Exists(JsonFilePath))
                return new List<T>();

            await using var stream = File.OpenRead(JsonFilePath);
            return await JsonSerializer.DeserializeAsync<List<T>>(stream) ?? new List<T>();
        }
    }
}