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
            DisplayChunksWithLabel(_memory.FreeChunks, "Free memory chunks");
            DisplayChunksWithLabel(_memory.BusyChunks, "Busy memory chunks");
            Console.WriteLine(" ---                      ---\n");
        }

        public void DisplayChunksWithLabel(LinkedList<MemoryChunk> chunks, string label)
        {
            Console.WriteLine(label);
            if (chunks.Count == 0)
            {
                Console.WriteLine("No chunks available.");
                return;
            }

            Console.WriteLine("{0, -10} {1, -10}", "Pointer", "Size");

            foreach (var chunk in chunks)
                Console.WriteLine("{0, -10} {1, -10}", chunk.Ptr, chunk.Size);
        }
    }
}