using System.Xml.Linq;
using System.Xml.Serialization;

namespace RestaurantManagementSystem.Services
{
    public abstract class SerializableObject<T>
    {
        // List to hold all instances of the class
        public static List<T> Instances { get; private set; } = new List<T>();

        // Method to add a new instance
        public static void AddInstance(T instance)
        {
            if (Instances.Contains(instance))
            {
                Console.WriteLine($"Duplicate instance detected. Skipping addition: {instance}");
                return;
            }
            Instances.Add(instance);
            Console.WriteLine($"Added new instance: {instance}");
        }

        // Method to save the extent to an XML file
        public static void SaveExtent()
        {
            var filePath = $"{typeof(T).Name}_Extent.xml";
            try
            {
                using var writer = new StringWriter();
                var serializer = new XmlSerializer(typeof(List<T>));

                // Serialize the instances list
                serializer.Serialize(writer, Instances);

                // Add metadata to the XML document
                var document = XDocument.Parse(writer.ToString());
                var root = document.Root;
                if (root != null)
                {
                    var metadata = new XElement("Metadata",
                        new XElement("GeneratedBy", "MyApp"),
                        new XElement("GeneratedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    );
                    root.AddFirst(metadata);
                }

                // Save the XML document
                document.Save(filePath);
                Console.WriteLine($"Extent for {typeof(T).Name} saved to {filePath} with custom metadata.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving extent for {typeof(T).Name}: {ex.Message}");
            }
        }

        // Method to load the extent from an XML file
        public static void LoadExtent()
        {
            var filePath = $"{typeof(T).Name}_Extent.xml";

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File {filePath} not found. Creating an empty extent for {typeof(T).Name}.");
                Instances = new List<T>();
                return;
            }

            try
            {
                using var reader = new StreamReader(filePath);
                var serializer = new XmlSerializer(typeof(List<T>));

                try
                {
                    var deserialized = serializer.Deserialize(reader);

                    if (deserialized is List<T> list)
                    {
                        Instances = list;
                    }
                    else
                    {
                        Console.WriteLine($"Deserialization returned null or unexpected type for {typeof(T).Name}. Initializing empty list.");
                        Instances = new List<T>();
                    }

                    Console.WriteLine($"Extent for {typeof(T).Name} loaded from {filePath}.");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Error during deserialization of {typeof(T).Name}: {ex.Message}");
                    Instances = new List<T>(); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading extent for {typeof(T).Name}: {ex.Message}");
                Instances = new List<T>();
            }
        }
    }
}
