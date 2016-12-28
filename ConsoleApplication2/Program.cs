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
        private static Database newdb;
        static DocumentClient client;

        static void Main(string[] args)
        {
            try
            {
                CheckDatabaseAndCreate().Wait();
                CreateCollection(client, "MyCollection1").Wait();
                CreateCollection(client, "MyCollection2", "S2").Wait();


            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            Console.ReadKey();
        }

        private async static Task CreateCollection(DocumentClient client, string collectionId, string offerType = "S1")
        {

            Console.WriteLine();
            Console.WriteLine("**** Create Collection {0} in {1} ****", collectionId, newdb.Id);

            var collectionDefinition = new DocumentCollection { Id = collectionId };
            var options = new RequestOptions { OfferType = offerType };
            var result = await client.CreateDocumentCollectionAsync(newdb.SelfLink,
               collectionDefinition, options);
            var collection = result.Resource;

            Console.WriteLine("Created new collection");
            ViewCollection(collection);
        }

        static async Task CheckDatabaseAndCreate()
        {
            try
            {

                using (client = new DocumentClient(new Uri(EndpointUrl), key))
                {
                    Console.WriteLine("Fetching Database from local emulator");
                    IEnumerable<Database> database = from db in client.CreateDatabaseQuery() select db;

                    string dbid = "mynewdb";

                    newdb = (from db in client.CreateDatabaseQuery()
                                      where db.Id == dbid
                                      select db).AsEnumerable().FirstOrDefault();

                    if (newdb == null)
                    {
                        await client.CreateDatabaseAsync(new Database() { Id = "mynewdb" });
                        Console.WriteLine("Created database");
                        Console.WriteLine("Database fetched" + newdb.Id);
                    }
                    else
                    {
                        Console.WriteLine("Database fetched" + newdb.Id);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void ViewCollection(DocumentCollection collection)
        {
            Console.WriteLine("Collection ID: {0} ", collection.Id);
            Console.WriteLine("Resource ID: {0} ", collection.ResourceId);
            Console.WriteLine("Self Link: {0} ", collection.SelfLink);
            Console.WriteLine("Documents Link: {0} ", collection.DocumentsLink);
            Console.WriteLine("UDFs Link: {0} ", collection.UserDefinedFunctionsLink);
            Console.WriteLine(" StoredProcs Link: {0} ", collection.StoredProceduresLink);
            Console.WriteLine("Triggers Link: {0} ", collection.TriggersLink);
            Console.WriteLine("Timestamp: {0} ", collection.Timestamp);
        }

    } 
}
