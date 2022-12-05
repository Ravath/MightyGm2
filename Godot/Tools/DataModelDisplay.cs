using Engine.RpgLogic;
using Godot;
using MightyGm2.Engine.RpgDatabase;

public class DataModelDisplay : Control, INamedDisplay
{
	private Label _name;
	private Label _description;

	public bool DisplayName
	{
		get => _name.Visible;
		set => _name.Visible = value;
	}

	public Control Control => this;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_name = GetNode<Label>("NameLabel");
		_description = GetNode<Label>("DescriptionLabel");
	}

	public void SetDisplay(INamed toDisplay)
	{
		if(toDisplay == null)
		{
			_name.Text = "RpgDataModel";
			_description.Text = "";
		}
		else
		{
			_name.Text = toDisplay.Name;
			var desProp = toDisplay.GetType().GetProperty("Description");
			_description.Text = (desProp?.GetValue(toDisplay) as string) ?? "";
		}
	}
}
