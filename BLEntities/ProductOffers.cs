using System.Linq;

namespace BLEntities
{
    using System.Collections.Generic;

    public class ProductOffers
    {
        public Product Product { get; set; }
        public IList<Offer> Offers { get; set; }

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

            var productOffers = (ProductOffers)obj;
            if (!Product.Equals(productOffers.Product))
            {
                return false;
            }

            if (Offers.Count != productOffers.Offers.Count)
            {
                return false;
            }

            for (int i = 0; i < Offers.Count; i++)
            {
                if (!Offers[i].Equals(productOffers.Offers[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = Product.GetHashCode() * 17;
            foreach (var offer in Offers)
            {
                hashCode += offer.GetHashCode() * 17;
            }

            return hashCode;
        }
    }
}
