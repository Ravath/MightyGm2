using Godot;
using System;

public class MainScene : Control
{
	private DatabaseMngtPanel _dataManagementView;
	private RpgDataViewer _rpgDataView;

	private Control _currentView;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_dataManagementView = GetNode<DatabaseMngtPanel>("DatabaseMngtPanel");
		_rpgDataView = GetNode<RpgDataViewer>("RpgDataViewer");

		_currentView = _dataManagementView;
		DisplayCurrentView();
	}

	private void DisplayCurrentView()
	{
		_dataManagementView.Visible = false;
		_rpgDataView.Visible = false;

		_currentView.Visible = true;
	}

	/// <summary>
	/// Manage the input for switching from Ressources database to RPG database.
	/// Linked by override only.
	/// </summary>
	/// <param name="event"></param>
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey eventKey)
		{
			if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Tab
				&& eventKey.Control)
			{
				if (_currentView == _dataManagementView)
					_currentView = _rpgDataView;
				else
					_currentView = _dataManagementView;
				DisplayCurrentView();
			}
		}
	}
}
