namespace DALConfigWorker
{
    using BLEntities;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;

    public class ConfigWorker
    {
        private readonly string productsFilePath;
        private readonly string configFilePath;

        public ConfigWorker(string rootPath)
        {
            productsFilePath = $@"{rootPath}/Configs/Products.json";
            configFilePath = $@"{rootPath}/Configs/Config.json";
        }

        public IList<Product> GetAllProducts()
        {
            var json = File.ReadAllText(productsFilePath);
            var products = JsonConvert.DeserializeObject<List<Product>>(json);
            return products;
        }

        public Config GetConfig()
        {
            var json = File.ReadAllText(configFilePath);
            var config = JsonConvert.DeserializeObject<Config>(json);
            return config;
        }
    }
}
