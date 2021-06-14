using System;
using System.Threading.Tasks;
using Crex24.Net.Objects;
using Crex24.Net.Objects.Websocket;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.Sockets;

namespace Crex24.Net
{
    /// <summary>
    /// Symbol order book implementation
    /// </summary>
    public class Crex24SymbolOrderBook: SymbolOrderBook
    {
        private readonly Crex24SocketClient socketClient;

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol of the order book</param>
        /// <param name="options">The options for the order book</param>
        public Crex24SymbolOrderBook(string symbol, Crex24OrderBookOptions? options = null) : base(symbol, options ?? new Crex24OrderBookOptions())
        {
            symbol.ValidateCrex24Symbol();
            socketClient = new Crex24SocketClient();
            Levels = 20;
        }

        /// <inheritdoc />
        protected override async Task<CallResult<UpdateSubscription>> DoStart()
        {
            var result = await socketClient.SubscribeToOrderBookUpdatesAsync(Symbol, Levels!.Value, 0, HandleUpdate).ConfigureAwait(false);
            if (!result)
                return result;

            Status = OrderBookStatus.Syncing;

            var setResult = await WaitForSetOrderBook(10000).ConfigureAwait(false);
            return setResult ? result : new CallResult<UpdateSubscription>(null, setResult.Error);
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> DoResync()
        {
            return await WaitForSetOrderBook(10000).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override void DoReset()
        {
        }

        private void HandleUpdate(string symbol, bool full, Crex24SocketOrderBook data)
        {
            if (full)
            { 
                SetInitialOrderBook(DateTime.UtcNow.Ticks, data.Bids, data.Asks);
            }
            else
            {
                UpdateOrderBook(DateTime.UtcNow.Ticks, data.Bids, data.Asks);
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            processBuffer.Clear();
            asks.Clear();
            bids.Clear();

            socketClient?.Dispose();
        }
    }
}
