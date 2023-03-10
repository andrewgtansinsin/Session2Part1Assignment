using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

[assembly: Parallelize(Workers = 10, Scope = ExecutionScope.MethodLevel)]
namespace Session2Part1Assignment
{
    [TestClass]
    public class HttpClientTest
    {
        private static HttpClient httpClient;

        private static readonly string BaseURL = "https://petstore.swagger.io/v2/";

        private static readonly string UsersEndpoint = "pet";

        private static string GetURL(string enpoint) => $"{BaseURL}{enpoint}";

        private static Uri GetURI(string endpoint) => new Uri(GetURL(endpoint));

        private readonly List<PetModel> cleanUpList = new List<PetModel>();

        private static readonly string V = "https://images.app.goo.gl/KvPjfUZwUKfeJ34M8";

        [TestInitialize]
        public void TestInitialize()
        {
            httpClient = new HttpClient();
        }

        [TestCleanup]
        public async Task TestCleanUp()
        {
           foreach (var data in cleanUpList)
            {
                var httpResponse = await httpClient.DeleteAsync(GetURL($"{UsersEndpoint}/{data.Name}"));
            }
        }

        [TestMethod]
        public async Task PutMethod()
        {
            #region create data

            // Create Json Object here
            PetModel petData = new PetModel()
            {
                Id = 88,
                Category = {
                    Id = 8008,
                    Name = "Persian Cat"
                },
                Name = "Garfield",
                PhotoUrls = new string[] {V},
                Tags = {
                    Id = new long[] {2023},
                    Name = new string[] {"Andrew Tansinsin"}
                },
                Status = "available"
            };

            // Serialize Content
            var request = JsonConvert.SerializeObject(petData);
            var postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Post Request
            await httpClient.PostAsync(GetURL(UsersEndpoint), postRequest);

            #endregion

            #region get Username of the created data

            // Get Request
            var getResponse = await httpClient.GetAsync(GetURI($"{UsersEndpoint}/{petData.Name}"));

            // Deserialize Content
            var listPetData = JsonConvert.DeserializeObject<PetModel>(getResponse.Content.ReadAsStringAsync().Result);

            // filter created data
            var createdPetData = listPetData.Name;

            #endregion

            #region send put request to update data

            // Update value of petData
            petData = new PetModel()
            {
                Id = listPetData.Id,
                Category = {
                    Id = listPetData.Category.Id,
                    Name = listPetData.Category.Name
                },
                Name = "Garfunkel.put.updated",
                PhotoUrls = new string[] { V },
                Tags = {
                    Id = listPetData.Tags.Id,
                    Name = listPetData.Tags.Name
                },
                Status = listPetData.Status
            };
            
            // Serialize Content
            request = JsonConvert.SerializeObject(petData);
            postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Put Request
            var httpResponse = await httpClient.PutAsync(GetURL($"{UsersEndpoint}/{createdPetData}"), postRequest);

            // Get Status Code
            var statusCode = httpResponse.StatusCode;

            #endregion

            #region get updated data

            // Get Request
            getResponse = await httpClient.GetAsync(GetURI($"{UsersEndpoint}/{petData.Name}"));

            // Deserialize Content
            listPetData = JsonConvert.DeserializeObject<PetModel>(getResponse.Content.ReadAsStringAsync().Result);

            // filter created data
            createdPetData = listPetData.Name;

            #endregion

            #region cleanup data

            // Add data to cleanup list
            cleanUpList.Add(listPetData);

            #endregion

            #region assertion

            // Assertion
            Assert.AreEqual(HttpStatusCode.OK, statusCode, "Status code is not equal to 201");
            Assert.AreEqual(petData.Name, createdPetData, "Username not matching");

            #endregion
            
        }
    }
}
