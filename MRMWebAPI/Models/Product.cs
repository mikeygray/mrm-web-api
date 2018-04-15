namespace MRMWebAPI.Models
{
    public class Product
    {
        private int _id;
        private string _name;
        private string _description;
        private string _category;
        
        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Description { get => _description; set => _description = value; }
        public string Category { get => _category; set => _category = value; }

    }
}