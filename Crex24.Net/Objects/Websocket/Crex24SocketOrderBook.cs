using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Crex24.Net.Objects.Websocket
{
    /// <summary>
    /// Order book
    /// </summary>
    public class Crex24SocketOrderBook
    {
        /// <summary>
        /// The price of the last trade. Only filled on a full update.
        /// </summary>
        public decimal? Last { get; set; }

        /// <summary>
        /// The timestamp of the data. Only filled on a full update.
        /// </summary>
        [JsonProperty("time"), JsonConverter(typeof(TimestampConverter))]
        public DateTime? Timestamp { get; set; }
        /// <summary>
        /// The asks on the symbol
        /// </summary>
        public IEnumerable<Crex24DepthEntry> Asks { get; set; } = new List<Crex24DepthEntry>();
        /// <summary>
        /// The bids on the symbol
        /// </summary>
        public IEnumerable<Crex24DepthEntry> Bids { get; set; } = new List<Crex24DepthEntry>();
    }
}
