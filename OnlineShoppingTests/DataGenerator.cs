using OnlineShopping;

namespace OnlineShoppingTests
{
    public class DataGenerator
    {
        public static void GenerateCustomers(int numberOfCustomers)
        {
            var nameGeneratorString = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            for (var i = 0; i < numberOfCustomers; i++)
            {
                var nameGenerator = new Random();
                var name = new string(
                    Enumerable
                        .Repeat(nameGeneratorString, 5)
                        .Select(s => s[nameGenerator.Next(s.Length)])
                        .ToArray()
                );

                var email = name + "@ex.com";
                var password = name + "123!";
                var address = new Random().Next(1, 100) + " Main Street";
                var phoneNo = "1234567890";

                new Guest().Register(name, email, password, address, phoneNo);
            }
        }

        public static void GenerateProducts(int numberOfProducts)
        {
            var nameGeneratorString = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            for (var i = 0; i < numberOfProducts; i++)
            {
                var nameGenerator = new Random();
                var name = new string(
                    Enumerable
                        .Repeat(nameGeneratorString, 5)
                        .Select(s => s[nameGenerator.Next(s.Length)])
                        .ToArray()
                );

                var group = new string(
                    Enumerable
                        .Repeat(nameGeneratorString, 5)
                        .Select(s => s[nameGenerator.Next(s.Length)])
                        .ToArray()
                );

                var subGroup = new string(
                    Enumerable
                        .Repeat(nameGeneratorString, 5)
                        .Select(s => s[nameGenerator.Next(s.Length)])
                        .ToArray()
                );

                var price = new Random().Next(1, 50);

                new Admin().AddProduct(name, group, subGroup, price);
            }
        }
    }
}
