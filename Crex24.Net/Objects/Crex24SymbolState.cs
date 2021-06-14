using Crex24.Net.Converters;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using CryptoExchange.Net.ExchangeInterfaces;

namespace Crex24.Net.Objects
{
    /// <summary>
    /// Symbol state info
    /// </summary>
    public class Crex24SymbolState : ICommonTicker
    { 
        /// <summary>
        /// The timestamp of the data
        /// </summary>
        [JsonProperty("timestamp")]//, JsonConverter(typeof(TimestampConverter))]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The open price based on a 24H ticker
        /// </summary>
        [JsonProperty("open"), JsonConverter(typeof(DecimalConverter))]
        public decimal Open { get; set; }
        /// <summary>
        /// The high price based on a 24H ticker
        /// </summary>
        [JsonProperty("high"), JsonConverter(typeof(DecimalConverter))]
        public decimal High { get; set; }
        /// <summary>
        /// The low price based on a 24H ticker
        /// </summary>
        [JsonProperty("low"), JsonConverter(typeof(DecimalConverter))]
        public decimal Low { get; set; }
        /// <summary>
        /// The price of the last trade
        /// </summary>
        [JsonProperty("close"), JsonConverter(typeof(DecimalConverter))]
        public decimal Close { get; set; }
        /// <summary>
        /// The volume of the quote asset. i.e. for symbol ETHBTC this is the volume in ETH
        /// </summary>
        [JsonProperty("volume"), JsonConverter(typeof(DecimalConverter))]
        public decimal Volume { get; set; }
        string ICommonTicker.CommonSymbol => "";
        decimal ICommonTicker.CommonHigh => High;
        decimal ICommonTicker.CommonLow => Low;
        decimal ICommonTicker.CommonVolume => Volume;
    }

    /// <summary>
    /// Symbol state list
    /// </summary>
    public class Crex24SymbolStatesList : ICommonTicker
    {
        /// <summary>
        /// The timestamp of the data
        /// </summary>
        [JsonProperty("timestamp")]//, JsonConverter(typeof(TimestampConverter))]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The open price based on a 24H ticker
        /// </summary>
        [JsonProperty("open"), JsonConverter(typeof(DecimalConverter))]
        public decimal Open { get; set; }
        /// <summary>
        /// The high price based on a 24H ticker
        /// </summary>
        [JsonProperty("high"), JsonConverter(typeof(DecimalConverter))]
        public decimal High { get; set; }
        /// <summary>
        /// The low price based on a 24H ticker
        /// </summary>
        [JsonProperty("low"), JsonConverter(typeof(DecimalConverter))]
        public decimal Low { get; set; }
        /// <summary>
        /// The price of the last trade
        /// </summary>
        [JsonProperty("close"), JsonConverter(typeof(DecimalConverter))]
        public decimal Close { get; set; }
        /// <summary>
        /// The volume of the quote asset. i.e. for symbol ETHBTC this is the volume in ETH
        /// </summary>
        [JsonProperty("volume"), JsonConverter(typeof(DecimalConverter))]
        public decimal Volume { get; set; }
        string ICommonTicker.CommonSymbol => "";
        decimal ICommonTicker.CommonHigh => High;
        decimal ICommonTicker.CommonLow => Low;
        decimal ICommonTicker.CommonVolume => Volume;
    }
}
