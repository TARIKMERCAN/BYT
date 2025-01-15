using System;
using System.Linq;
using System.Reflection;

namespace RestaurantManagementSystem.Services
{
    public static class ExtentManager
    {
        public static void LoadAllExtents()
        {
            try
            {
                // Use reflection to find all subclasses of SerializableObject<T>
                var serializableTypes = Assembly.GetExecutingAssembly()
                                                .GetTypes()
                                                .Where(t => t.BaseType != null &&
                                                            t.BaseType.IsGenericType &&
                                                            t.BaseType.GetGenericTypeDefinition() == typeof(SerializableObject<>));

                foreach (var type in serializableTypes)
                {
                    var loadMethod = type.GetMethod("LoadExtent", BindingFlags.Public | BindingFlags.Static);
                    loadMethod?.Invoke(null, null);
                }

                Console.WriteLine("All Extents Loaded Successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Loading Extents: {ex.Message}");
            }
        }

        public static void SaveAllExtents()
        {
            try
            {
                // Use reflection to find all subclasses of SerializableObject<T>
                var serializableTypes = Assembly.GetExecutingAssembly()
                                                .GetTypes()
                                                .Where(t => t.BaseType != null &&
                                                            t.BaseType.IsGenericType &&
                                                            t.BaseType.GetGenericTypeDefinition() == typeof(SerializableObject<>));

                foreach (var type in serializableTypes)
                {
                    var saveMethod = type.GetMethod("SaveExtent", BindingFlags.Public | BindingFlags.Static);
                    saveMethod?.Invoke(null, null);
                }

                Console.WriteLine("All Extents Saved Successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Saving Extents: {ex.Message}");
            }
        }
    }
}
