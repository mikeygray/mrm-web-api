namespace MRMWebAPI.Migrations
{
    using MRMWebAPI.Models;
    using Newtonsoft.Json.Linq;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    internal sealed class Configuration : DbMigrationsConfiguration<ProductServiceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ProductServiceContext context)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string jsonRaw = string.Empty;

            try
            {
                /*
                 * JSON data file included as an embedded resource and then read. Resource location/name saved in Resource.resx
                 * Not suitable for large seeds? 
                 */
                using (Stream stream = assembly.GetManifestResourceStream(Properties.Resources.ProductsEmbeddedResourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    jsonRaw = reader.ReadToEnd();
                }

                var jsonToken = JToken.Parse(jsonRaw);
                var productsArray = jsonToken.ToObject<Product[]>();
                var count = 0;

                foreach (Product product in productsArray)
                {
                    product.Id = count++;
                    //Do I even needs this? The DB assigns its own Id anyway?

                    context.Products.AddOrUpdate(x => x.Id, product);
                    //Debug.WriteLine("{0} | {1} | {2} | {3}", product.Id, product.Name, product.Category, product.Description);
                }
            }
            catch (System.Exception ex)
            {
                /*
                 * Could be overloaded/replaced to write to log, etc.
                 */
                Debug.WriteLine("Error Parsing JSON and Seeding DB: " + ex.Message);
            }
        }
    }
}
