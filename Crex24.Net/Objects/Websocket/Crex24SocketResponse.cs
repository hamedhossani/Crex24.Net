using Newtonsoft.Json;

namespace Crex24.Net.Objects.Websocket
{
    internal class Crex24SocketResponse
    {
        [JsonProperty("method")]
        public string Method { get; set; } = "";
        [JsonProperty("params")]
        public object[] Parameters { get; set; } = new object[0];
        [JsonProperty("id")]
        public int? Id { get; set; }
    }
}
