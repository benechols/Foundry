using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using EVERFI.Foundry.Classes;
using System.Net.Http;
using RestSharp.Authenticators;
using System.Web.UI;

namespace EVERFI.Foundry {
  public partial class API {

    #region Add User

    public User AddUser(User MyUser) {

      var Request = new RestRequest($"{_ver}/admin/registration_sets", Method.POST);

      Request.Parameters.Clear();
      Request.AddParameter("application/json", API.UserJson(MyUser), ParameterType.RequestBody);
      Request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      var Response = _client.Execute<User>(Request);
      var NumericCode = (int)Response.StatusCode;

      if (NumericCode != 201) {
        throw new FoundryException(Response.ErrorMessage, NumericCode, Response.Content);
      }

      var UserData = JsonConvert.DeserializeObject<UserDataJson>(Response.Content);

      var User = UserData.Data.UserAttributes;

      if (User.Location != null) {
        User.Location = GetLocationById(User.LocationId);
      }

      User.ConfigureUserData(UserData.Data);

      return User;
    }

    #endregion Add User

    #region Update User

    public User UpdateUser(User MyUser) {

      var Request = new RestRequest($"{_ver}/admin/registration_sets/{MyUser.UserId}", Method.PATCH);

      Request.Parameters.Clear();
      Request.AddParameter("application/json", API.UserJson(MyUser), ParameterType.RequestBody);
      Request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      var Response = _client.Execute(Request);
      var NumericCode = (int)Response.StatusCode;

      if (NumericCode != 200) {
        throw new FoundryException(Response.ErrorMessage, NumericCode, Response.Content);
      }

      var UserData = JsonConvert.DeserializeObject<UserDataJson>(Response.Content);

      var User = UserData.Data.UserAttributes;

      if (User.Location != null) {
        User.Location = GetLocationById(User.LocationId);
      }

      User.ConfigureUserData(UserData.Data);

      return User;
    }

    #endregion Update User

    #region Get Users

    public User GetUserById(string UserId) {

      var Request = new RestRequest($"{_ver}/admin/users/{UserId}", Method.GET);

      Request.AddHeader("Content-Type", "application/json");
      Request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      var Response = _client.Execute<User>(Request);
      var NumericCode = (int)Response.StatusCode;

      if (NumericCode != 201) {
        throw new FoundryException(Response.ErrorMessage, NumericCode, Response.Content);
      }

      var UserData = JsonConvert.DeserializeObject<UserDataJson>(Response.Content);

      var RetrievedUser = UserData.Data.UserAttributes;
      RetrievedUser.ConfigureUserData(UserData.Data);

      if (RetrievedUser.Location != null) {
        RetrievedUser.Location = GetLocationById(RetrievedUser.LocationId);
      }

      return RetrievedUser;
    }

    public List<User> GetUserByEmail(string UserEmail) {

      var Request = new RestRequest($"{_ver}/admin/users/", Method.GET);

      Request.Parameters.Clear();
      Request.AddParameter("filter[email]", UserEmail, ParameterType.QueryString);
      Request.AddHeader("Content-Type", "application/json");
      Request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      var Response = _client.Execute<User>(Request);
      var NumericCode = (int)Response.StatusCode;

      if (NumericCode != 201) {
        throw new FoundryException(Response.ErrorMessage, NumericCode, Response.Content);
      }

      var UserData = JsonConvert.DeserializeObject<UserDataJsonList>(Response.Content);
      var Users = new List<User>();

      foreach (var Data in UserData.Data) {

        var NewUser = Data.UserAttributes;
        NewUser.ConfigureUserData(Data);

        if (NewUser.Location != null) {
          NewUser.Location = GetLocationById(NewUser.LocationId);
        }

        Users.Add(NewUser);
      }

      return Users;
    }

    // TODO: Implement paging in this
    public List<User> GetUsersBySearch(Dictionary<SearchTerms, string> searchTerms) {

      /*
      using (var Client = new HttpClient()) {

        var URL = $"{_client.BaseUrl}{_ver}/admin/users/";

        foreach (var Term in searchTerms.Keys) {
          int Count = 0;
          var Separator = "&";

          if (Count == 0) {
            Separator = "?";
          }
          URL = $"{URL}{Separator}filter[{GetDescription(Term)}]={searchTerms[Term]}";
        }

        //Client.BaseAddress = new Uri(URL);
        Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.access_token);
        Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token.access_token}");

        var Response = Client.GetAsync(URL).Result;

        var Code = (int)Response.StatusCode;

        if (Code != 200) {
            throw new FoundryException(Response.Content.ToString(), Code, Response.Content.ToString());
          }

        var UserData = JsonConvert.DeserializeObject<UserDataJsonList>(Response.Content.ToString());
        var Users = new List<User>();

        foreach (var Data in UserData.Data) {

          var NewUser = Data.UserAttributes;
          NewUser.ConfigureUserData(Data);

          if (NewUser.Location != null) {
            NewUser.Location = GetLocationById(NewUser.LocationId);
          }

          Users.Add(NewUser);
        }

        return Users;
      }*/

      /*var Client = new RestClient(_client.BaseUrl) {
        Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(_token.access_token)
      };
      */
      var Request = new RestRequest($"{_ver}/admin/users/", Method.GET);

      Request.Parameters.Clear();

      foreach (var Term in searchTerms.Keys) {
        Request.AddParameter("filter[" + GetDescription(Term) + "]", searchTerms[Term], ParameterType.QueryString);
      }

      Request.AddHeader("Content-Type", "application/json");
      Request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      var Response = _client.Execute(Request);
      var NumericCode = (int)Response.StatusCode;

      if (NumericCode != 200) {
        throw new FoundryException(Response.ErrorMessage, NumericCode, Response.Content);
      }

      var UserData = JsonConvert.DeserializeObject<UserDataJsonList>(Response.Content);
      var Users = new List<User>();

      foreach (var Data in UserData.Data) {

        var NewUser = Data.UserAttributes;
        NewUser.ConfigureUserData(Data);

        if (NewUser.Location != null) {
          NewUser.Location = GetLocationById(NewUser.LocationId);
        }

        Users.Add(NewUser);
      }

      return Users;
    }

    public List<User> GetUsers(int page) {

      var Request = new RestRequest($"/{_ver}/admin/users", Method.GET);

      Request.Parameters.Clear();
      Request.AddParameter("page[page]", currPage, ParameterType.QueryString);
      Request.AddParameter("page[per_page]", returnPerPage, ParameterType.QueryString);
      Request.AddHeader("Content-Type", "application/json");
      Request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      var Response = _client.Execute<User>(Request);
      var NumericCode = (int)Response.StatusCode;

      if (NumericCode != 201) {
        throw new FoundryException(Response.ErrorMessage, NumericCode, Response.Content);
      }

      var UserData = JsonConvert.DeserializeObject<UserDataJsonList>(Response.Content);
      var Users = new List<User>();

      foreach (var Data in UserData.Data) {

        var NewUser = Data.UserAttributes;
        NewUser.ConfigureUserData(Data);

        if (NewUser.Location != null) {
          NewUser.Location = GetLocationById(NewUser.LocationId);
        }

        Users.Add(NewUser);
      }

      return Users;
    }

    public Tuple<List<User>, bool> GetUsers() {

      var FinalPage = true;

      var Request = new RestRequest($"/{_ver}/admin/users", Method.GET);

      Request.Parameters.Clear();
      Request.AddParameter("page[page]", currPage, ParameterType.QueryString);
      Request.AddParameter("page[per_page]", returnPerPage, ParameterType.QueryString);
      Request.AddHeader("Content-Type", "application/json");
      Request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      var Response = _client.Execute<User>(Request);
      var NumericCode = (int)Response.StatusCode;

      if (NumericCode != 201) {
        throw new FoundryException(Response.ErrorMessage, NumericCode, Response.Content);
      }

      var UserData = JsonConvert.DeserializeObject<UserDataJsonList>(Response.Content);
      var Users = new List<User>();

      foreach (var Data in UserData.Data) {

        var NewUser = Data.UserAttributes;
        NewUser.ConfigureUserData(Data);

        if (NewUser.Location != null) {
          NewUser.Location = GetLocationById(NewUser.LocationId);
        }

        Users.Add(NewUser);
      }

      var MetaData = JsonConvert.DeserializeObject<MetaJson>(Response.Content);

      if (currPage * returnPerPage >= MetaData.Meta.Count) {
        FinalPage = false;
        currPage = 1;
      } else {
        currPage += 1;
      }

      return new Tuple<List<User>, bool>(Users, FinalPage);
    }

    public void ResetGetUsers() {
      currPage = 1;
    }

    public int GetUserCount() {

      var Request = new RestRequest($"/{_ver}/admin/users/?page[page]={1}&page[per_page]={returnPerPage}", Method.GET);

      Request.Parameters.Clear();
      Request.AddHeader("Content-Type", "application/json");
      Request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      var Response = _client.Execute<User>(Request);
      var NumericCode = (int)Response.StatusCode;

      if (NumericCode != 201) {
        throw new FoundryException(Response.ErrorMessage, NumericCode, Response.Content);
      }

      var MetaData = JsonConvert.DeserializeObject<MetaJson>(Response.Content);

      return MetaData.Meta.Count;
    }

    #endregion Get Users

  }
}
