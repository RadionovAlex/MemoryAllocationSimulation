using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryAllocationSimulation
{
    internal class SimulatedMemoryVisualizer
    {
        private SimulatedMemory _memory;

        public SimulatedMemoryVisualizer(SimulatedMemory memory)
        {
            _memory = memory;
        }

        public void ShowFreeAndBusy()
        {
            Console.WriteLine("\n --- Free and busy chunks --- ");
            DisplayFreeChunks();
            DisplayBusyChunks();
            Console.WriteLine(" ---                      ---\n");
        }

        public void DisplayFreeChunks()
        {
            var freeChunks = _memory.FreeChunks;

            if (freeChunks.Count == 0)
            {
                Console.WriteLine("No free memory chunks available.");
                return;
            }

            Console.WriteLine("Free Memory Chunks:");
            Console.WriteLine("{0, -10} {1, -10}", "Pointer", "Size");

            foreach (var chunk in freeChunks)
            {
                Console.WriteLine("{0, -10} {1, -10}", chunk.Ptr, chunk.Size);
            }
        }

        public void DisplayBusyChunks()
        {
            var busyChunks = _memory.BusyChunks;

            if (busyChunks.Count == 0)
            {
                Console.WriteLine("No busy memory chunks");
                return;
            }

            Console.WriteLine("Busy Memory Chunks:");
            Console.WriteLine("{0, -10} {1, -10}", "Pointer", "Size");

            foreach (var chunk in busyChunks)
            {
                Console.WriteLine("{0, -10} {1, -10}", chunk.Ptr, chunk.Size);
            }
        }
    }
}