using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BlockchainServer
{
    class Block
    {
        public int index { get; set; }
        public DateTime timeStamp { get; set; }
        public string prevHash { get; set; }
        public string hash { get; set; }
        public string balsuota { get; set; }

        public Block(DateTime timeStamp, string prevHash, string data)
        {
            index = 0;
            this.timeStamp = timeStamp;
            this.prevHash = prevHash;
            this.balsuota = data;
            hash = CalculateHash();
        }

        public string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{timeStamp}-{prevHash ?? ""}-{balsuota}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }
    }
}
