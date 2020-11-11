﻿using Newtonsoft.Json;

namespace EVERFI.Foundry.Classes {
  public class AccessToken {

    [JsonProperty("access_token")]
    public string access_token { get; set; }
    
    [JsonProperty("token_type")]
    public string token_type { get; set; }
    
    [JsonProperty("expires_in")]
    public int expires_in { get; set; }
    
    [JsonProperty("created_at")]
    public int created_at { get; set; }

  }
}