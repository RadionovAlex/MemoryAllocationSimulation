what can be done with memory: 

* allocate memory for some purpose (for Object, Array, string as Array)
* deallocate memory of unused creature (Object, Array, string as Array)
* look at allocated memory
* look at free memory

For alloc/dealloc will be used linkedList of structure with fields: length; prtNext

--- Memory in initial state ---

one element with length = fullLength; without ptrNext

--- Memory when one object was allocated ---

first element with length = fullLength - allocatedLength; without ptrNext


--- Memory when object was deallocated --- 

first element with length = fullLength - allocatedLength; ptrNext = secondElement
second element with length = allocatedLength; ptrNext = null