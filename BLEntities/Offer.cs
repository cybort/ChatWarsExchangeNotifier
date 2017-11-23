namespace BLEntities
{
    public class Offer
    {
        public int Count { get; set; }
        public int Price { get; set; }

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

            var offer = (Offer)obj;
            return Count == offer.Count && Price == offer.Price;
        }

        public override int GetHashCode()
        {
            return Price.GetHashCode() + Price.GetHashCode();
        }
    }
}
