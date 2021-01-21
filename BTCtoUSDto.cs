using System;
using Newtonsoft.Json;

namespace BtcUsdCalculator
{
    class BTCtoUSDto
    {
        [JsonProperty("trade_id")]
        public int TradeId { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("time")]
        public DateTime DatePrice { get; set; }
    }
}