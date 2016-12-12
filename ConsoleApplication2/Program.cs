using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;

namespace ConsoleApplication2
{
    class Program
    {
        private const string EndpointUrl = "https://localhost:8081";
        private const string key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        static void Main(string[] args)
        {
            try
            {
                CreateDocumentClient().Wait();
                                
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            Console.ReadKey();
        }

        private static async Task CreateDocumentClient()
        {
            // Create a new instance of the DocumentClient
            using (var client = new DocumentClient(new Uri(EndpointUrl), key))
            {
                Console.WriteLine("Authorization Successful");
                await CreateDatabase(client);
            }
        }

        private async static Task CreateDatabase(DocumentClient client)
        {
            Console.WriteLine();
            Console.WriteLine("Creating Database...");

            var databaseDefinition = new Database { Id = "mynewdb" };
            var result = await client.CreateDatabaseAsync(databaseDefinition);
            var database = result.Resource;

            Console.WriteLine(" Database Id: {0}; Rid: {1}", database.Id, database.ResourceId);
            Console.WriteLine("Database Created");
        }
    }
    
}
