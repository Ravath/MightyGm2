using Godot;
using MightyGm2.Engine.RpgDatabase;
using System;
using System.IO;
using System.Text.Json;

public class ModelEditor : Control
{
	public String SaveFolder { get; set; } = "./";
	public RpgDataModel _object;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//TODO : everything
	}
	
	public void SetObject(RpgDataModel o)
	{
		_object = o;
	}

	public void SaveAsJson()
	{
		if(_object != null)
		{
			var options = new JsonSerializerOptions { WriteIndented = true };
			string jsonString = JsonSerializer.Serialize(_object, options);
			String FileName = SaveFolder + _object.GetType().Name + ".json";
			//TODO this is bullshit, have to store lists of items, no just one
			using (StreamWriter ws = new StreamWriter(System.IO.File.Create(FileName)))
			{
				ws.Write(jsonString);
			}
		}
	}
}
