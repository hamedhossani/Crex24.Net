﻿using Crex24.Net.Objects;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace Crex24.Net.Converters
{
    internal class WithdrawStatusConverter: BaseConverter<WithdrawStatus>
    {
        public WithdrawStatusConverter() : this(true) { }
        public WithdrawStatusConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<WithdrawStatus, string>> Mapping => new List<KeyValuePair<WithdrawStatus, string>>
        {
            new KeyValuePair<WithdrawStatus, string>(WithdrawStatus.Audit, "audit"),
            new KeyValuePair<WithdrawStatus, string>(WithdrawStatus.Cancel, "cancel"),
            new KeyValuePair<WithdrawStatus, string>(WithdrawStatus.Confirming, "confirming"),
            new KeyValuePair<WithdrawStatus, string>(WithdrawStatus.Fail, "fail"),
            new KeyValuePair<WithdrawStatus, string>( WithdrawStatus.Finish, "finish"),
            new KeyValuePair<WithdrawStatus, string>(WithdrawStatus.NotPass, "not_pass"),
            new KeyValuePair<WithdrawStatus, string>(WithdrawStatus.Pass, "pass"),
            new KeyValuePair<WithdrawStatus, string>(WithdrawStatus.Processing, "processing")
        };
    }
}
