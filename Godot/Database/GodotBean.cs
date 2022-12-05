using MightyGm2.Engine.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GodotResource : Godot.Object
{
    public ResourceFile File { get; }
    public GodotResource(ResourceFile file)
    {
        File = file;
    }
}
public class GodotTag : Godot.Object
{
    public Tag Tag { get; }
    public GodotTag(Tag tag)
    {
        Tag = tag;
    }
}
