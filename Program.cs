using System.Xml;

namespace CurrencyExchangeApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string? url = "https://www.nbkr.kg/XML/daily.xml";

            using HttpClient client = new();
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string? xmlString = await response.Content.ReadAsStringAsync();
                    XmlDocument xmlDoc = new();
                    xmlDoc.LoadXml(xmlString);

                    XmlNodeList? currencyNodes = xmlDoc.SelectNodes("//Currency");
                    if (currencyNodes != null)
                    {
                        Console.WriteLine("Курсы валют Национального банка Кыргызской Республики:");
                        foreach (XmlNode currencyNode in currencyNodes)
                        {
                            string? isoCode = currencyNode.Attributes?["ISOCode"]?.Value;
                            string? value = currencyNode.SelectSingleNode("Value")?.InnerText;
                            if (isoCode != null && value != null)
                            {
                                Console.WriteLine($"{isoCode}: {value}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Не удалось найти курсы валют в XML.");
                    }
                }
                else
                {
                    Console.WriteLine($"Ошибка при выполнении HTTP-запроса: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }
    }
}
