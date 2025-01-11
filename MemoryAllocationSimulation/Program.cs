var memory = new MemoryAllocationSimulation.SimulatedMemory();
memory.Initalize(2048, 16384);

var visualizer = new MemoryAllocationSimulation.SimulatedMemoryVisualizer(memory);

visualizer.ShowFreeAndBusy();

memory.Allocate(5);
memory.Allocate(15);
memory.Allocate(30);
memory.Allocate(2);
memory.Allocate(1);

visualizer.ShowFreeAndBusy();

Console.WriteLine("Dealloc all busy memory");

while(memory.BusyChunks.Count > 0)
{
    memory.Deallocate(memory.BusyChunks.First());
}

visualizer.ShowFreeAndBusy();

memory.Defragmentate();
visualizer.ShowFreeAndBusy();

Console.ReadLine();
