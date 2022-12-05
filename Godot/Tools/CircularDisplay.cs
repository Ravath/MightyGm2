using Godot;
using System;

public class CircularDisplay : Position2D
{
	[Export]
	public double AngularOffset;
	[Export]
	public int Radius = 50;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        UpdateDisplay();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        int count = GetChildCount();
        if (count == 0) return;
        double angStep = 2 * Math.PI / count;
        int i = 0;
        foreach (Control item in GetChildren())
        {
            // Compute position on circle
            Vector2 position = new Vector2
            {
                x = (float)(Radius * Math.Cos(AngularOffset + i * angStep)),
                y = (float)(Radius * Math.Sin(AngularOffset + i * angStep))
            };
            // use center of controler
            position.x -= item.RectSize.x / 2;
            position.y -= item.RectSize.y / 2;
            // set position
            item.RectPosition = position;
            i++;
        }
    }
}
