using Newtonsoft.Json;//The type JObject comes from the Newtonsoft.Json.Linq library and is a .NET object we can treat as JSON.
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiTest
{
  class Program
  {
    static void Main()
    {
      var apiCallTask = ApiHelper.ApiCall("goJIpDLVGSytpsALpO3B0YsAwwAj0CLo");// create a variable to store the returned Task from apiHelper's static async method apiCall(see below in the ApiHelper class)
      var result = apiCallTask.Result;//stored result from action above. In this case: a string representation of API call's response content
      JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(result);//converts the JSON-formatted string result into a JObject.

      List<Article> articleList = JsonConvert.DeserializeObject<List<Article>>(jsonResponse["results"].ToString());//DeserializedObject() will auto grab any JSON keys in our response that match the names of the properties in our class. Prop names in class MUST match JSON keys.

      foreach (Article article in articleList)
      {
        Console.WriteLine($"Section: {article.Section}");
        Console.WriteLine($"Title: {article.Title}");
        Console.WriteLine($"Abstract: {article.Abstract}");
        Console.WriteLine($"Url: {article.Url}");
        Console.WriteLine($"Byline: {article.Byline}");
      }
    }
  }

  class ApiHelper//We create a class called ApiHelper that contains a static method ApiCall which takes an apiKey parameter.
  {
    public static async Task<string> ApiCall(string apiKey)//a generic Task can also be returned
    {
      RestClient client = new RestClient("https://api.nytimes.com/svc/topstories/v2");//We instantiate a RestSharp RestClient object and store the connection in a variable called client.

      RestRequest request = new RestRequest($"home.json?api-key={apiKey}", Method.GET);//Next, we create a RestRequest object. This is our actual request. We include the path to the endpoint we are looking for (home.json) along with our API key. We also specify that we will be using a GET Http method.
      var response = await client.ExecuteTaskAsync(request);//Then we use the await keyword to specify that we need to receive a result before we attempt to define response. We call the RestClient's ExecuteTaskAsync method and pass in our request object.
      return response.Content;//Finally, we return the Content property of the response variable, which is a string representation of the response content.
    }
  }
}