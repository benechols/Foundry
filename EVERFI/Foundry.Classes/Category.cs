using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVERFI.Foundry.Classes {

  internal class CategoryData {

    [JsonProperty("data")]
    internal Category Data { get; set; }
  }

  public class CategoryListData {

    [JsonProperty("data")]
    public List<Category> Data { get; set; }

    [JsonProperty("included")]
    public List<Label> Included { get; set; }
  }

  internal class CategoryAttributes {

    [JsonProperty("name")]
    internal string Name { get; set; }

    [JsonProperty("users_count")]
    internal int UserCount { get; set; }
  }

  internal class CategoryRelationships {

    [JsonProperty("category_labels")]
    internal CategoryLabels CategoryLabels { get; set; }
  }

  internal class CategoryLabels {

    [JsonProperty("data")]
    internal List<Label> Labels { get; set; }
  }

  public class Category {

    [JsonProperty("id")]
    public string Id { get; internal set; }

    public string Name { get; set; }

    public int UserCount { get; internal set; }

    [JsonProperty("attributes")]
    internal CategoryAttributes Attributes { get; set; }

    [JsonProperty("relationships")]
    internal CategoryRelationships Relationships { get; set; }

    public List<Label> Labels { get; internal set; }

    public void ConfigureCategory() {

      if (Attributes != null) {
        Name = Attributes.Name;
        UserCount = Attributes.UserCount;
      }

      // This only holds the ids of the Labels
      if (Relationships != null && Relationships.CategoryLabels != null) {
        Labels = Relationships.CategoryLabels.Labels;
      } else {
        Labels = new List<Label>();
      }
    }

  }
}
