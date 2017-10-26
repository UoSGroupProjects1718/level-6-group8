using System.Collections;
using System.Collections.Generic;

// Json
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public abstract class MTF
{

}

public class MachineToFile
{
    public int x;
    public int y;
    public Direction dir;
    public string type;
}

public class InputToFile : MachineToFile
{
    public string ingredient;
}

public class LevelToFile
{
    public List<MachineToFile> machines = new List<MachineToFile>();
    public List<InputToFile> inputs = new List<InputToFile>();
}