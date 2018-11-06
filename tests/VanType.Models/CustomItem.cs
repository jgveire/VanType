namespace VanType.Models
{
    public class CustomItem
    {
        public CustomItem(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}