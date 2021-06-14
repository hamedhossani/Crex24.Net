using Newtonsoft.Json;

namespace Crex24.Net.Objects.Websocket
{
    internal class Crex24SocketRequest
    {
        [JsonProperty("method")]
        public string Method { get; set; } = "";
        [JsonIgnore]
        public string Subject { get; set; } = "";
        [JsonProperty("params")]
        public object[] Parameters { get; set; } = new object[0];
        [JsonProperty("id")]
        public int Id { get; set; }

        public Crex24SocketRequest() { }

        public Crex24SocketRequest(int id, string subject, string action, params object[] parameters)
        {
            Id = id;
            Subject = subject;
            Method = $"{subject}.{action}";
            Parameters = parameters;
        }
    }
}
