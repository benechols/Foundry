using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using EVERFI.Foundry.Classes;

namespace EVERFI.Foundry {
  public partial class API {

    public Location AddLocation(Location MyLocation) {

      var Request = new RestRequest($"/{_ver}/admin/locations", Method.POST);
      
      Request.AddParameter("application/json", API.LocationJson(MyLocation), ParameterType.RequestBody);
      Request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      IRestResponse response = _client.Execute(Request);
      HttpStatusCode statusCode = response.StatusCode;
      int numericCode = (int)statusCode;

      if (numericCode != 201) {
        throw new FoundryException(response.ErrorMessage, numericCode, response.Content);
      }

      //verify if adding was okay with status code

      LocationDataJson locationData = JsonConvert.DeserializeObject<LocationDataJson>(response.Content);

      Location location = locationData.LocationData.LocationAttributes;
      location.Id = locationData.LocationData.Id;

      FoundryLocations.Add(location);
      return location;
    }

    public Location UpdateLocation(Location MyLocation) {

      RestRequest request = new RestRequest($"/{_ver}/admin/locations/{MyLocation.Id}", Method.PATCH);

      request.AddParameter("application/json", API.LocationJson(MyLocation), ParameterType.RequestBody);
      request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      IRestResponse response = _client.Execute(request);
      HttpStatusCode statusCode = response.StatusCode;
      int numericCode = (int)statusCode;

      if (numericCode != 200) {
        throw new FoundryException(response.ErrorMessage, numericCode, response.Content);
      }

      LocationDataJson locationData = JsonConvert.DeserializeObject<LocationDataJson>(response.Content);
      Location location = locationData.LocationData.LocationAttributes;
      location.AddIdFromData(locationData.LocationData);

      FoundryLocations.Remove(MyLocation);
      FoundryLocations.Add(location);

      return location;
    }

    public List<Location> GetLocations() {

      RestRequest request = new RestRequest($"/{_ver}/admin/locations");

      request.AddHeader("Content-Type", "application/json");
      request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      IRestResponse response = _client.Execute(request);
      HttpStatusCode statusCode = response.StatusCode;
      int numericCode = (int)statusCode;

      if (numericCode != 200) {
        throw new FoundryException(response.ErrorMessage, numericCode, response.Content);
      }

      LocationDataJsonList locationData = JsonConvert.DeserializeObject<LocationDataJsonList>(response.Content);
      List<Location> locations = new List<Location>();

      foreach (LocationData data in locationData.LocationsData) {
        Location newLocation = data.LocationAttributes;
        newLocation.AddIdFromData(data);
        locations.Add(newLocation);
      }

      return locations;
    }

    public Location GetLocationById(string LocationId) {

      RestRequest request = new RestRequest($"/{_ver}/admin/locations/{LocationId}", Method.GET);
      
      request.AddHeader("Content-Type", "application/json");
      request.AddParameter("Authorization", $"Bearer {_token.access_token}", ParameterType.HttpHeader);

      IRestResponse response = _client.Execute(request);
      HttpStatusCode statusCode = response.StatusCode;
      int numericCode = (int)statusCode;

      if (numericCode != 200) {
        throw new FoundryException(response.ErrorMessage, numericCode, response.Content);
      }

      LocationDataJson locationData = JsonConvert.DeserializeObject<LocationDataJson>(response.Content);
      Location location = locationData.LocationData.LocationAttributes;
      location.AddIdFromData(locationData.LocationData);

      return location;
    }

  }
}
