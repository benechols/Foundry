using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using EVERFI.Foundry.Classes;

namespace EVERFI.Foundry {
  public partial class API {

    public BulkAssignJob BulkAssignLabels(List<User> usersList, Label label) {

      if (usersList.Count > bulkActionCap) {
        throw new FoundryException("The limit for the bulk add function is 500 users!");
      }

      RestRequest request = new RestRequest($"/{_ver}/admin/bulk_actions/category", Method.POST);
      
      request.AddParameter("application/json", API.BulkUserLabelJson(usersList, label), ParameterType.RequestBody);
      request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      IRestResponse response = _client.Execute(request);
      HttpStatusCode statusCode = response.StatusCode;
      int numericCode = (int)statusCode;

      if (numericCode != 201) {
        throw new FoundryException(response.ErrorMessage, numericCode, response.Content);
      }

      JobJson jobJson = JsonConvert.DeserializeObject<JobJson>(response.Content);
      BulkAssignJob job = jobJson.BulkAssignJob;

      Console.WriteLine("Labels added to " + usersList.Count + " users.");

      return job;
    }

    public BulkAssignJob GetJobById(string JobId) {
      RestRequest request = new RestRequest($"/{_ver}/admin/bulk_actions/category/{JobId}", Method.GET);
      
      request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      IRestResponse response = _client.Execute(request);
      HttpStatusCode statusCode = response.StatusCode;
      int numericCode = (int)statusCode;

      if (numericCode != 200) {
        throw new FoundryException(response.ErrorMessage, numericCode, response.Content);
      }

      JobJson jobJson = JsonConvert.DeserializeObject<JobJson>(response.Content);
      BulkAssignJob job = jobJson.BulkAssignJob;

      return job;
    }

    public void AssignLabel(User user, Label label) {

      RestRequest request = new RestRequest($"/{_ver}/admin/category_label_users", Method.GET);
      
      request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      IRestResponse response = _client.Execute(request);
      HttpStatusCode statusCode = response.StatusCode;
      int numericCode = (int)statusCode;

      if (numericCode != 200) {
        throw new FoundryException(response.ErrorMessage, numericCode, response.Content);
      }
    }

    public void DeleteLabelFromUser(string category_label_user_id) {

      RestRequest request = new RestRequest($"/{_ver}/admin/category_label_users/category/{category_label_user_id}", Method.DELETE);
      
      request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      IRestResponse response = _client.Execute(request);
      HttpStatusCode statusCode = response.StatusCode;
      int numericCode = (int)statusCode;

      if (numericCode != 200) {
        throw new FoundryException(response.ErrorMessage, numericCode, response.Content);
      }
    }

  }
}
