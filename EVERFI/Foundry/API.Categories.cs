using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using EVERFI.Foundry.Classes;
using System.Linq;

namespace EVERFI.Foundry {
  public partial class API {

    public Category AddCategory(Category MyCategory) {

      RestRequest request = new RestRequest($"/{_ver}/admin/categories", Method.POST); //TODO

      request.AddParameter("application/json", API.CategoryJson(MyCategory), ParameterType.RequestBody);
      request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      IRestResponse response = _client.Execute(request);
      HttpStatusCode statusCode = response.StatusCode;
      int numericCode = (int)statusCode;

      if (numericCode != 201) {
        throw new FoundryException(response.ErrorMessage, numericCode, response.Content);
      }

      CategoryData categoryData = JsonConvert.DeserializeObject<CategoryData>(response.Content);

      return categoryData.Data;
    }

    public Category GetCategoryById(string CategoryId, bool WithLabels) { // Should we always return with List<Label>?

      RestRequest request = new RestRequest($"/{_ver}/admin/categories/{CategoryId}", Method.GET); //TODO
      
      if (WithLabels) {
        request.AddParameter("include", "category_labels", ParameterType.QueryString);
      }
      request.AddHeader("Content-Type", "application/json");
      request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      IRestResponse response = _client.Execute(request);
      HttpStatusCode statusCode = response.StatusCode;
      int numericCode = (int)statusCode;

      if (numericCode != 200) {
        throw new FoundryException(response.ErrorMessage, numericCode, response.Content);
      }

      CategoryData categoryData = JsonConvert.DeserializeObject<CategoryData>(response.Content);

      Category category = categoryData.Data;

      category.ConfigureCategory();
      if (WithLabels) {
        for (int i = 0; i < category.Labels.Count; i++) {
          category.Labels[i] = GetLabelById(category.Labels[i].Id);
        }
      }
      /*else
      {
          category.Labels.Clear();
      }*/

      return category;
    }

    public List<Category> GetCategories(bool WithLabels) {
      
      var Request = new RestRequest($"/{_ver}/admin/categories", Method.GET);
      
      if (WithLabels) {
        Request.AddParameter("include", "category_labels", ParameterType.QueryString);
      }
      Request.AddHeader("Content-Type", "application/json");
      Request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      var Response = _client.Execute(Request);
      var NumericCode = (int)Response.StatusCode;

      if (NumericCode != 200) {
        throw new FoundryException(Response.ErrorMessage, NumericCode, Response.Content);
      }

      var CategoryData = JsonConvert.DeserializeObject<CategoryListData>(Response.Content);
      var Categories = new List<Category>();

      foreach (var Category in CategoryData.Data) {

        Category.ConfigureCategory();
        
        Categories.Add(Category);
      }

      if (WithLabels && CategoryData.Included != null) {

        // Reset Label Data
        foreach (var Category in Categories) {
          Category.Labels = new List<Label>();
        }

        foreach (var Label in CategoryData.Included) {

          Label.ConfigureLabel();

          var Category = Categories.Where(e => e.Id == Label.Relationships.LabelCategory.Category.Id).FirstOrDefault();

          Category.Labels.Add(Label);
        }
      }

      return Categories;
    }

    public Category UpdateCategory(Category MyCategory) {
      
      RestRequest request = new RestRequest($"/{_ver}/admin/categories/{MyCategory.Id}", Method.PATCH);

      request.AddParameter("application/json", API.CategoryJson(MyCategory), ParameterType.RequestBody);
      request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      IRestResponse response = _client.Execute(request);
      HttpStatusCode statusCode = response.StatusCode;
      int numericCode = (int)statusCode;

      if (numericCode != 200) {
        throw new FoundryException(response.ErrorMessage, numericCode, response.Content);
      }

      CategoryData categoryData = JsonConvert.DeserializeObject<CategoryData>(response.Content);
      Console.WriteLine("Category successfully updated.");
      Category category = categoryData.Data;

      category.ConfigureCategory();

      return category;
    }

    public string DeleteCategory(Category MyCategory) {

      RestRequest request = new RestRequest($"/{_ver}/admin/categories/{MyCategory.Id}", Method.DELETE);

      request.AddHeader("Content-Type", "application/json");
      request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      IRestResponse response = _client.Execute(request);
      HttpStatusCode statusCode = response.StatusCode;
      int numericCode = (int)statusCode;

      if (numericCode != 204) {
        throw new FoundryException(response.ErrorMessage, numericCode, response.Content);
      }

      Console.WriteLine("Category successfully deleted.");

      return response.Content;
    }
  }
}
