using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainServer
{
    class BlockchainClass
    {
        public IList<Block> Chain { set; get; }

        public BlockchainClass()
        {
            InitializeChain();
            AddGenesisBlock();
        }


        public void InitializeChain()
        {
            Chain = new List<Block>();
        }

        public Block CreateGenesisBlock()
        {
            return new Block(DateTime.Now, null, "{}");
        }

        public void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }

        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        public void AddBlock(Block block)
        {
            Block latestBlock = GetLatestBlock();
            block.index = latestBlock.index + 1;
            block.prevHash = latestBlock.hash;
            block.hash = block.CalculateHash();
            Chain.Add(block);
        }

        public bool IsValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];

                if (currentBlock.hash != currentBlock.CalculateHash())
                {
                    return false;
                }

                if (currentBlock.prevHash != previousBlock.hash)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
