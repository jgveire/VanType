namespace VanType.Models
{
    public class Lookup<T>
    {
        public T Id { get; set; } = default;

        public string Name { get; set; } = string.Empty;
    }
}
