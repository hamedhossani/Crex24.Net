﻿using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace Crex24.Net.Objects
{
    /// <summary>
    /// Mining difficulty info
    /// </summary>
    public class Crex24MiningDifficulty
    {
        /// <summary>
        /// The difficulty in CET/Hour
        /// </summary>
        public string Difficulty { get; set; } = "";
        /// <summary>
        /// Estimated hourly mining yield to distribute
        /// </summary>
        public string Prediction { get; set; } = "";
        /// <summary>
        /// The update time of the Prediction field
        /// </summary>
        [JsonConverter(typeof(TimestampSecondsConverter))]
        [JsonProperty("update_time")]
        public DateTime UpdateTime { get; set; }
    }
}
