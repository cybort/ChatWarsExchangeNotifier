namespace BLEntities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            var product = (Product)obj;
            return Id == product.Id && Name.Equals(product.Name);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() + Name.GetHashCode() * 17;
        }
    }
}
