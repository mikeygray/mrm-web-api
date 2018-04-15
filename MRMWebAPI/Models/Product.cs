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

        public override bool Equals(object obj)
        {
            if (obj is Product product)
            {
                if (product.Id == _id && product.Name == _name && product.Description == _description && product.Category == _category)
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}