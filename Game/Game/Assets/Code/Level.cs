using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This is a class that gets serialized to json when writing 
/// machine data to file.
/// </summary>
public class MachineToFile
{
    public int x;
    public int y;
    public Direction dir;
    public string type;
}

/// <summary>
/// This is a class that gets serialized to json when 
/// writing Inputter data to file
/// </summary>
public class InputToFile : MachineToFile
{
    public string ingredient;
}

/// <summary>
/// This is the class which gets serialized to json when saving 
/// the level to a file, this is how it remembers all machines and 
/// inputs that exist within the level.
/// </summary>
public class LevelToFile
{
    public bool[,] tilesActive;
    public List<MachineToFile> machines = new List<MachineToFile>();
    public List<InputToFile> inputs = new List<InputToFile>();
}

/// <summary>
/// This is the class which a factory owns an object of this type. This object
/// holds information about the 2d array of files, all machines and all items 
/// in the level.
/// </summary>
public class Level
{
    /* Level Data*/
    //public LevelData levelData;

    /* This is the factories current level, the 2d array of tiles */
    public Tile[,] grid;

    /* This is the list of all machines that are currently inside of the factory */
    public List<Machine> machines = new List<Machine>();

    /* This is the list of all items currently inside of the factory */
    public List<Item> items = new List<Item>();

    /* A property getter for the total cost of all machines in the level */
    public int CalculateTotalMachineCost { get { return machines.Sum(machine => machine.Cost); } }
}