using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Domain.Entities
{
    public class BlockchainResponse
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }
    }
}
