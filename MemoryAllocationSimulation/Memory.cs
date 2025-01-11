using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryAllocationSimulation
{
    internal struct FreeChunk
    {
        public int Ptr;
        public int Size; 

        public static int Compare(FreeChunk x, FreeChunk y)
        {
            if (x.Ptr == y.Ptr) 
                return 0;
            if (x.Ptr > y.Ptr) 
                return 1;

            return -1;
        }
    }

    internal struct AllocatedChunk
    {
        public int Ptr;
        public int Size; 
        // let`s say that when deallocating, size can be calculated from type. 
        // so size - is real size of object
    }

    internal class Memory
    {
        private LinkedList<FreeChunk> _freeChunks = new ();
        private int _base;
        private int _ramSize;
       
        internal void Initalize(int ramBaseAddress, int ramSize)
        {
            _base = ramBaseAddress;
            _ramSize = ramSize;

            _freeChunks.AddFirst(new FreeChunk()
            {
                Ptr = _base,
                Size = ramSize
            });
        }

        /// <summary>
        /// Return pointer to free memory chunk
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        internal int Allocate(int size)
        {
            if(_freeChunks.Count == 0)
            {
                Console.WriteLine("No free chunks");
                Defragmentate();
                if (_freeChunks.Count == 0)
                    throw new Exception("Not enough RAM memory");
            }

            var chunk = FindRequiredChunk(size);

            if (chunk.Size < size)
            {
                Console.WriteLine("Can`t find free memory space to allocate");
                Defragmentate();
                chunk = FindRequiredChunk(size);
            }

            if (chunk.Size < size)
                throw new Exception($"Can`t find free memory space to allocate {size}. Free memory spaces: {_freeChunks.Count}");

            var memoryAddress = chunk.Ptr;

            UseFreeMemoryChunkForSize(chunk, size);

            return memoryAddress;
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Deallocate(int ptr, int size)
        {
            var newFreeChunk = new FreeChunk()
            {
                Ptr = ptr,
                Size = size
            };

            _freeChunks.AddLast(newFreeChunk);
        }

        /// <summary>
        /// join small fragments 
        /// that are placed physically sequentially in the memory
        /// into one bigger segment
        /// </summary>
        internal void Defragmentate()
        {
            // chunks are placed not in order of their adresses in _freeChunks.
            // so for each chunkA, algorithm should find other chunkB, where:
            // * chunkA.Ptr + Size + 1 == chunkB.ptr 
            // or 
            // * chunkA.Ptr - 1 == chunkB.ptr + Size

            // so, to make this task faster, SortedList can be created first -> 
            // sort by chunk.Ptr. So, just going one by one, comparing will be available.

            if (_freeChunks.Count < 2)
            {
                Console.WriteLine($"Nothing to defragmentate, fragments count: {_freeChunks.Count}");
                return;
            }

            var sortedFreeChunks = new List<FreeChunk>();


            var chunk = _freeChunks.First;
            while (chunk != null)
            {
                sortedFreeChunks.Add(chunk!.Value);
                chunk = chunk.Next;
            }

            sortedFreeChunks.Sort(FreeChunk.Compare);
            var sequence = new List<FreeChunk>(); 
            
            for (int i = 1; i < sortedFreeChunks.Count; i++)
            {
                if(IsSequential(sortedFreeChunks[i-1], sortedFreeChunks[i]))
                {
                    sequence.Add(sortedFreeChunks[i - 1]);
                    sequence.Add(sortedFreeChunks[i]);
                }
                else
                {
                    if (sequence.Count == 0)
                        continue;

                    // Join chunks into one
                    var distinct = sequence.Distinct();

                    var baseAddress = distinct.First().Ptr;
                    var totalSize = 0;
                    foreach (var uniqueChunk in distinct)
                    {
                        totalSize += uniqueChunk.Size;
                        _freeChunks.Remove(uniqueChunk);
                    }

                    var joinedMemoryChunk = new FreeChunk()
                    {
                        Ptr = baseAddress,
                        Size = totalSize
                    };

                    _freeChunks.AddLast(joinedMemoryChunk);

                    sequence.Clear();
                }
            }
        }

        private bool IsSequential(FreeChunk previous, FreeChunk current)
        {
            if (previous.Ptr + previous.Size + 1 == current.Ptr)
                return true;

            return false;
        }

        private FreeChunk FindRequiredChunk(int size)
        {
            var node = _freeChunks.First;
            var chunk = node!.Value;
            while (node.Next != null)
            {
                node = node.Next;
                if (node.Value.Size < size)
                    continue;

                if (chunk.Size < size)
                {
                    chunk = node.Value;
                    continue;
                }

                if (chunk.Size > node.Value.Size)
                    chunk = node.Value;
            }

            return chunk;
        }

        private void UseFreeMemoryChunkForSize(FreeChunk chunk, int size)
        {
            // need to remove freeChunk, but not whole in case size < chunk.Size

            if (chunk.Size > size)
            {
                var newMemoryChunk = new FreeChunk()
                {
                    Ptr = chunk.Ptr + size,
                    Size = chunk.Size - size
                };

                var previousNode = _freeChunks.Find(chunk);
                _freeChunks.AddAfter(previousNode!, newMemoryChunk);
            }

            _freeChunks.Remove(chunk);
        }
    }
}