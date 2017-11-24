namespace BLEntities
{
    public class Config
    {
        public int ProfitAmount { get; set; }
        public int ProfitPercentage { get; set; }

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

            var config = (Config)obj;
            return ProfitAmount == config.ProfitAmount && ProfitPercentage == config.ProfitPercentage;
        }

        public override int GetHashCode()
        {
            return ProfitAmount.GetHashCode() + ProfitPercentage.GetHashCode();
        }
    }
}
