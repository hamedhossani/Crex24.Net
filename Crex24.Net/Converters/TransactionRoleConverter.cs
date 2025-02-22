﻿using Crex24.Net.Objects;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace Crex24.Net.Converters
{
    internal class TransactionRoleConverter: BaseConverter<TransactionRole>
    {
        public TransactionRoleConverter() : this(true) { }
        public TransactionRoleConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<TransactionRole, string>> Mapping => new List<KeyValuePair<TransactionRole, string>>
        {
            new KeyValuePair<TransactionRole, string>(TransactionRole.Maker, "maker"),
            new KeyValuePair<TransactionRole, string>(TransactionRole.Taker, "taker")
        };
    }
}
