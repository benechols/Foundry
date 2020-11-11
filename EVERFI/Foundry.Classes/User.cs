using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EVERFI.Foundry.Classes {

  internal class MetaJson {

    [JsonProperty("meta")]
    internal MetaData Meta { get; set; }
  }

  internal class MetaData {

    [JsonProperty("total_count")]
    internal int Count { get; set; }
  }

  internal class UserDataJson {

    [JsonProperty("data")]
    internal UserData Data { get; set; }
  }

  internal class UserDataJsonList {

    [JsonProperty("data")]
    internal List<UserData> Data { get; set; }
  }

  internal class UserData {

    [JsonProperty("id")]
    internal string UserId { get; set; }

    [JsonProperty("attributes")]
    internal User UserAttributes { get; set; }
  }

  internal class ExternalAttributes {

    [JsonProperty("position", NullValueHandling = NullValueHandling.Ignore)]
    internal string Position { get; set; }

    [JsonProperty("first_day_of_work", NullValueHandling = NullValueHandling.Ignore)]
    internal DateTime FirstDay { get; set; }

    [JsonProperty("last_day_of_work", NullValueHandling = NullValueHandling.Ignore)]
    internal DateTime LastDay { get; set; }
  }

  public class UserLabels {

    internal string Id { get; set; }
  }

  public class User {

    [JsonProperty("external_attributes")]
    private ExternalAttributes ExternalAttributes { get; set; }

    [JsonProperty("user_types")]
    private Dictionary<string, string> TypesDictionary { get; set; }

    /* user_rule_set */
    [JsonProperty("first_name", Required = Required.Always)]
    public string FirstName { get; set; }

    [JsonProperty("last_name", Required = Required.Always)]
    public string LastName { get; set; }

    [JsonProperty("email", Required = Required.Always)]
    public string Email { get; set; }

    [JsonProperty("sso_id")]
    public string SingleSignOnId { get; set; }

    [JsonProperty("employee_id")]
    public string EmployeeId { get; set; }

    [JsonProperty("student_id")]
    public string StudentId { get; set; }

    public Location Location { get; set; }

    [JsonProperty("location_id")]
    public string LocationId { get; set; }

    [JsonProperty("active")]
    public bool Active { get; set; }

    [JsonProperty("category_labels")]
    public List<string> CategoryLabels { get; set; }

    /* second registration array */
    public List<UserType> UserTypes { get; set; }

    public string Position { get; set; }

    public DateTime FirstDay { get; set; }

    public DateTime LastDay { get; set; }

    public string UserId { get; internal set; }

    public User() {
      UserTypes = new List<UserType>();
      //CategoryLabels = new List<UserLabels>();
    }

    internal void ConfigureUserData(UserData data) {

      Position = ExternalAttributes.Position;
      FirstDay = ExternalAttributes.FirstDay;
      LastDay = ExternalAttributes.LastDay;

      UserId = data.UserId;

      foreach (var Type in TypesDictionary.Keys) {
        UserTypes.Add(new UserType(UserType.StringToType(Type), UserType.StringToRole(TypesDictionary[Type])));
      }

      //foreach (var Type in CategoryLabelsDictionary.Keys) {
      //  CategoryLabels.Add(new UserLabels() { Id = Type.ToString() });
      //}

    }

  }
}
