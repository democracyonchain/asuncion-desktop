using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Domain.Entities
{
    public class IpfsUploadResponse
    {
        public string name { get; set; }
        public string ipfs_hash { get; set; }
        public string size { get; set; }

        public string GetIpfsPath() => $"ipfs://{ipfs_hash}";
    }
}
