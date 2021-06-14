﻿using System;
using System.Net.Http;
using CryptoExchange.Net.Objects;

namespace Crex24.Net.Objects
{
    /// <summary>
    /// Client options
    /// </summary>
    public class Crex24ClientOptions: RestClientOptions
    {
        /// <summary>
        /// Create new client options
        /// </summary>
        public Crex24ClientOptions() : this(null, "https://api.crex24.com/v2")
        {
        }

        /// <summary>
        /// Create new client options
        /// </summary>
        /// <param name="client">HttpClient to use for requests from this client</param>
        public Crex24ClientOptions(HttpClient client) : this(client, "https://api.crex24.com/v1")
        {
        }

        /// <summary>
        /// Create new client options
        /// </summary>
        /// <param name="apiAddress">Custom API address to use</param>
        /// <param name="client">HttpClient to use for requests from this client</param>
        public Crex24ClientOptions(HttpClient? client, string apiAddress) : base(apiAddress)
        {
            HttpClient = client;
        }
    }

    /// <summary>
    /// Socket client options
    /// </summary>
    public class Crex24SocketClientOptions : SocketClientOptions
    {
        /// <summary>
        /// The amount of subscriptions that should be made on a single socket connection. Not all exchanges support multiple subscriptions on a single socket.
        /// Setting this to a higher number increases subscription speed, but having more subscriptions on a single connection will also increase the amount of traffic on that single connection.
        /// Not supported on Crex24
        /// </summary>
        public new int? SocketSubscriptionsCombineTarget
        {
            get => 1;
            set
            {
                if (value != 1)
                    throw new ArgumentException("Can't change SocketSubscriptionsCombineTarget; server implementation does not allow multiple subscription on a socket");
            }
        }

        /// <summary>
        /// ctor
        /// </summary>
        public Crex24SocketClientOptions(): base("wss://socket.Crex24.com/")
        {
        }
    }

    /// <summary>
    /// Order book options
    /// </summary>
    public class Crex24OrderBookOptions : OrderBookOptions
    {
        /// <summary>
        /// ctor
        /// </summary>
        public Crex24OrderBookOptions() : base("Crex24", false, false)
        {
        }
    }
}
