using CryptoExchange.Net.Attributes;

namespace Crex24.Net.Objects
{
    internal class Crex24ApiResult<T>
    {
        public string? Message { get; set; }
        public int Code { get; set; }
        [JsonOptionalProperty] public T Data { get; set; } = default!;
    }
}
