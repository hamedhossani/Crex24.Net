using Newtonsoft.Json;

namespace Crex24.Net.Objects.Websocket
{
    internal class Crex24SocketRequestResponse<T>
    {
        [JsonProperty("error")]
        public Crex24SocketError? Error { get; set; }

        [JsonProperty("result")] 
        public T Result { get; set; } = default!;
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
