﻿using Crex24.Net.Converters;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace Crex24.Net.Objects.Websocket
{
    /// <summary>
    /// Transaction data
    /// </summary>
    public class Crex24SocketSymbolTrade
    {
        /// <summary>
        /// The type of the transaction
        /// </summary>
        [JsonConverter(typeof(TransactionTypeConverter))]
        public TransactionType Type { get; set; }
        /// <summary>
        /// The timestamp of the transaction
        /// </summary>
        [JsonConverter(typeof(TimestampSecondsConverter))]
        [JsonProperty("time")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// The price per unit of the transaction
        /// </summary>
        [JsonConverter(typeof(DecimalConverter))]
        public decimal Price { get; set; }
        /// <summary>
        /// The order id of the transaction
        /// </summary>
        [JsonProperty("id")]
        public long OrderId { get; set; }
        /// <summary>
        /// The amount of the transaction
        /// </summary>
        [JsonConverter(typeof(DecimalConverter))]
        public decimal Amount { get; set; }
    }
}
