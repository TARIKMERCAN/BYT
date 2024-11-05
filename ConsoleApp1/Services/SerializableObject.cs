namespace ConsoleApp1.Services
{
    public abstract class SerializableObject<T> where T : SerializableObject<T>, new()
    {
        private static List<T> _instances = new List<T>();
        public static List<T> GetAllInstances() => new List<T>(_instances);

        public static List<T> Instances => _instances;
        
        public static void AddInstance(T instance)
        {
            _instances.Add(instance);
        }

        public static void ClearInstances()
        {
            _instances.Clear();
        }
    }
}