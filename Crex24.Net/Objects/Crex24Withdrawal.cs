﻿using Crex24.Net.Converters;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace Crex24.Net.Objects
{
    /// <summary>
    /// Withdrawal info
    /// </summary>
    public class Crex24Withdrawal
    {
        /// <summary>
        /// The actual amount of the withdrawal, i.e. the amount which will be transferred to the destination address
        /// </summary>
        [JsonProperty("actual_amount"), JsonConverter(typeof(DecimalConverter))]
        public decimal ActualAmount { get; set; }
        /// <summary>
        /// The total amount of the withdrawal
        /// </summary>
        [JsonConverter(typeof(DecimalConverter))]
        public decimal Amount { get; set; }
        /// <summary>
        /// The destination address of the withdrawal
        /// </summary>
        [JsonProperty("coin_address")]
        public string CoinAddress { get; set; } = "";
        /// <summary>
        /// The name of the coin of the withdrawal
        /// </summary>
        [JsonProperty("coin_type")]
        public string CoinType { get; set; } = "";
        /// <summary>
        /// The id of this withdrawal
        /// </summary>
        [JsonProperty("coin_withdraw_id")]
        public long CoinWithdrawalId { get; set; }
        /// <summary>
        /// The current number of confirmations
        /// </summary>
        public int Confirmations { get; set; }
        /// <summary>
        /// The time the withdrawal was created
        /// </summary>
        [JsonProperty("create_time"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// The status of the withdrawal
        /// </summary>
        [JsonConverter(typeof(WithdrawStatusConverter))]
        public WithdrawStatus Status { get; set; }
        /// <summary>
        /// The fee for the withdrawal
        /// </summary>
        [JsonProperty("tx_fee"), JsonConverter(typeof(DecimalConverter))]
        public decimal TransactionFee { get; set; }
        /// <summary>
        /// The transaction id of the withdrawal
        /// </summary>
        [JsonProperty("tx_id")]
        public string TransactionId { get; set; } = "";
    }
}
