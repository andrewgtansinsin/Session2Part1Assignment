using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Parallelize(Workers = 10, Scope = ExecutionScope.MethodLevel)]
namespace Session2Part1Assignment
{
    [TestClass]
    public class HttpClientTest
    {
        private static HttpClient httpClient;

        private static readonly string BaseURL = "https://petstore.swagger.io/v2/";

        private static readonly string UsersEndpoint = "user";

        private static string GetURL(string enpoint) => $"{BaseURL}{enpoint}";

        private static Uri GetURI(string endpoint) => new Uri(GetURL(endpoint));

        private readonly List<UserModel> cleanUpList = new List<UserModel>();

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
                var httpResponse = await httpClient.DeleteAsync(GetURL($"{UsersEndpoint}/{data.Username}"));
            }
        }

        [TestMethod]
        public async Task GetMethod()
        {
            #region create data

            // Create Json Object
            UserModel userData = new UserModel()
            {
                Username = "test123.get",
                FirstName = "testFName",
                LastName = "testLName",
                Email = "test@email.com",
                Password = "123",
                Phone = "123456789",
                UserStatus = 0
            };

            // Serialize Content
            var request = JsonConvert.SerializeObject(userData);
            var postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Post Request
            await httpClient.PostAsync(GetURL(UsersEndpoint), postRequest);

            #endregion

            #region get data

            // Send Request
            var httpResponse = await httpClient.GetAsync(GetURI($"{UsersEndpoint}/{userData.Username}"));

            // Get Content
            var httpResponseMessage = httpResponse.Content;
            
            // Get Status Code
            var statusCode = httpResponse.StatusCode;

            // Deserialize Content
            var listUserData = JsonConvert.DeserializeObject<UserModel>(httpResponseMessage.ReadAsStringAsync().Result);

            #endregion

            #region cleanupdata

            // Add data to cleanup list
            cleanUpList.Add(listUserData);

            #endregion

            #region assertion

            // Assertion
            Assert.AreEqual(HttpStatusCode.OK, statusCode, "Status code is not equal to 200");
            Assert.IsTrue(listUserData.Username == userData.Username);

            #endregion
        }

        [TestMethod]
        public async Task PostMethod()
        {
            #region create data and send post request

            // Create Json Object
            UserModel userData = new UserModel()
            {
                Username = "test123.post",
                FirstName = "testFName",
                LastName = "testLName",
                Email = "test@email.com",
                Password = "123",
                Phone = "123456789",
                UserStatus = 0
            };

            // Serialize Content
            var request = JsonConvert.SerializeObject(userData);
            var postRequest = new StringContent(request,Encoding.UTF8, "application/json");

            // Send Request
            var httpResponse = await httpClient.PostAsync(GetURL(UsersEndpoint), postRequest);

            // Get Status Code
            var statusCode = httpResponse.StatusCode;

            #endregion

            #region get created data

            // Get Request
            var getResponse = await httpClient.GetAsync(GetURI($"{UsersEndpoint}/{userData.Username}"));

            // Deserialize Content
            var listUserData = JsonConvert.DeserializeObject<UserModel>(getResponse.Content.ReadAsStringAsync().Result);

            var createdUserData = listUserData.Username;

            #endregion

            #region cleanupdata

            // Add data to cleanup list
            cleanUpList.Add(listUserData);
            #endregion

            #region assertion

            // Assertion
            Assert.AreEqual(HttpStatusCode.OK, statusCode, "Status code is not equal to 201");
            Assert.AreEqual(userData.Username, createdUserData, "Username not matching");
            
            #endregion

        }


        [TestMethod]
        public async Task PutMethod()
        {
            #region create data

            // Create Json Object
            UserModel userData = new UserModel()
            {
                Username = "test123.put",
                FirstName = "testFName",
                LastName = "testLName",
                Email = "test@email.com",
                Password = "123",
                Phone = "123456789",
                UserStatus = 0
            };

            // Serialize Content
            var request = JsonConvert.SerializeObject(userData);
            var postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Post Request
            await httpClient.PostAsync(GetURL(UsersEndpoint), postRequest);

            #endregion

            #region get Username of the created data

            // Get Request
            var getResponse = await httpClient.GetAsync(GetURI($"{UsersEndpoint}/{userData.Username}"));

            // Deserialize Content
            var listUserData = JsonConvert.DeserializeObject<UserModel>(getResponse.Content.ReadAsStringAsync().Result);

            // filter created data
            var createdUserData = listUserData.Username;

            #endregion

            #region send put request to update data

            // Update value of userData
            userData = new UserModel()
            {
                Id = listUserData.Id,
                Username = "test123.put.updated",
                FirstName = listUserData.FirstName,
                LastName = listUserData.LastName,
                Email = listUserData.Email,
                Password = listUserData.Password,
                Phone = listUserData.Phone,
                UserStatus = listUserData.UserStatus,
            };
            
            // Serialize Content
            request = JsonConvert.SerializeObject(userData);
            postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Put Request
            var httpResponse = await httpClient.PutAsync(GetURL($"{UsersEndpoint}/{createdUserData}"), postRequest);

            // Get Status Code
            var statusCode = httpResponse.StatusCode;

            #endregion

            #region get updated data

            // Get Request
            getResponse = await httpClient.GetAsync(GetURI($"{UsersEndpoint}/{userData.Username}"));

            // Deserialize Content
            listUserData = JsonConvert.DeserializeObject<UserModel>(getResponse.Content.ReadAsStringAsync().Result);

            // filter created data
            createdUserData = listUserData.Username;

            #endregion

            #region cleanup data

            // Add data to cleanup list
            cleanUpList.Add(listUserData);

            #endregion

            #region assertion

            // Assertion
            Assert.AreEqual(HttpStatusCode.OK, statusCode, "Status code is not equal to 201");
            Assert.AreEqual(userData.Username, createdUserData, "Username not matching");

            #endregion
            
        }

        [TestMethod]
        public async Task DeleteMethod()
        {
            #region create data

            // Create Json Object
            UserModel userData = new UserModel()
            {
                Username = "test123.delete",
                FirstName = "testFName",
                LastName = "testLName",
                Email = "test@email.com",
                Password = "123",
                Phone = "123456789",
                UserStatus = 0
            };

            // Serialize Content
            var request = JsonConvert.SerializeObject(userData);
            var postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Post Request
            await httpClient.PostAsync(GetURL(UsersEndpoint), postRequest);

            #endregion

            #region get Username of the created data

            // Get Request
            var getResponse = await httpClient.GetAsync(GetURI($"{UsersEndpoint}/{userData.Username}"));

            // Deserialize Content
            var listUserData = JsonConvert.DeserializeObject<UserModel>(getResponse.Content.ReadAsStringAsync().Result);

            // filter created data
            var createdUserData = listUserData.Username;

            #endregion

            #region send delete request

            // Send Delete Request
            var httpResponse = await httpClient.DeleteAsync(GetURL($"{UsersEndpoint}/{createdUserData}"));

            // Get Status Code
            var statusCode = httpResponse.StatusCode;

            #endregion

            #region assertion

            // Assertion
            Assert.AreEqual(HttpStatusCode.OK, statusCode, "Status code is not equal to 201");

            #endregion
        }

        [TestMethod]
        public async Task SendPostGetMethod()
        {
            #region create data

            // Create Json Object
            UserModel userData = new UserModel()
            {
                Username = "test123.sendpostget",
                FirstName = "testFName",
                LastName = "testLName",
                Email = "test@email.com",
                Password = "123",
                Phone = "123456789",
                UserStatus = 0
            };

            #endregion

            #region send post request

            var httpResponse = await SendAsyncFunction(HttpMethod.Post, UsersEndpoint, userData);

            var statusCode = httpResponse.StatusCode;

            #endregion

            #region get created data

            // Get Request
            var getResponse = await SendAsyncFunction(HttpMethod.Get, $"{UsersEndpoint}/{userData.Username}");

            // Deserialize Content
            var listUserData = JsonConvert.DeserializeObject<UserModel>(getResponse.Content.ReadAsStringAsync().Result);

            var createdUserData = listUserData.Username;

            #endregion

            #region cleanupdata

            // Add data to cleanup list
            cleanUpList.Add(listUserData);
            #endregion

            #region assertion

            // Assertion
            Assert.AreEqual(HttpStatusCode.OK, statusCode, "Status code is not equal to 201");
            Assert.AreEqual(userData.Username, createdUserData, "Username not matching");

            #endregion

        }

        /// <summary>
        /// Reusable method
        /// </summary>
        private async Task<HttpResponseMessage> SendAsyncFunction(HttpMethod method, string url, UserModel userData = null)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();

            httpRequestMessage.Method = method;
            httpRequestMessage.RequestUri = GetURI(url);
            httpRequestMessage.Headers.Add("Accept", "application/json");

            if (userData != null)
            {
                // Serialize Content
                var request = JsonConvert.SerializeObject(userData);
                httpRequestMessage.Content = new StringContent(request, Encoding.UTF8, "application/json");
            } 

            var httpResponse = await httpClient.SendAsync(httpRequestMessage);

            return httpResponse;
        }


        [TestMethod]
        public async Task PatchMethod()
        {
            #region create data

            // Create Json Object
            UserModel userData = new UserModel()
            {
                Username = "test123.patch",
                FirstName = "testFName",
                LastName = "testLName",
                Email = "test@email.com",
                Password = "123",
                Phone = "123456789",
                UserStatus = 0
            };

            // Serialize Content
            var request = JsonConvert.SerializeObject(userData);
            var postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Post Request
            await httpClient.PostAsync(GetURL(UsersEndpoint), postRequest);

            #endregion

            #region get Username of the created data

            // Get Request
            var getResponse = await httpClient.GetAsync(GetURI($"{UsersEndpoint}/{userData.Username}"));

            // Deserialize Content
            var listUserData = JsonConvert.DeserializeObject<UserModel>(getResponse.Content.ReadAsStringAsync().Result);

            // filter created data
            var createdUserData = listUserData.Username;

            #endregion

            #region send patch request to update data

            // Update value of userData
            userData = new UserModel()
            {
                Username = "test123.patch.updated"
            };

            // Serialize Content
            request = JsonConvert.SerializeObject(userData);
            postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Put Request
            var httpResponse = await httpClient.PatchAsync(GetURL($"{UsersEndpoint}/{createdUserData}"), postRequest);

            // Get Status Code
            var statusCode = httpResponse.StatusCode;

            #endregion

            #region cleanup data

            // Add data to cleanup list
            cleanUpList.Add(listUserData);

            #endregion

            #region assertion

            // Assertion
            Assert.AreEqual(HttpStatusCode.MethodNotAllowed, statusCode, "Status code is not equal to 405");

            #endregion

        }

    }
}
