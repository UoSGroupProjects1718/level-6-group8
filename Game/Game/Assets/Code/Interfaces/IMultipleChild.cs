using System.Collections.Generic;

interface IMultipleChild
{
    List<Item> bufferChildren { get; set; }
    List<Item> activeChildre { get; set; }
}