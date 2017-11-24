namespace BLEntities
{
    using System.Collections.Generic;

    public class Exchange
    {
        public IList<ProductOffers> ProductOffers { get; set; }

        public Exchange()
        {
            ProductOffers = new List<ProductOffers>();
        }

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

            var exchange = (Exchange)obj;
            if (ProductOffers.Count != exchange.ProductOffers.Count)
            {
                return false;
            }

            for (int i = 0; i < ProductOffers.Count; i++)
            {
                if (!ProductOffers[i].Equals(exchange.ProductOffers[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            foreach (var productOffer in ProductOffers)
            {
                hashCode += productOffer.GetHashCode() * 17;
            }

            return hashCode;
        }
    }
}
