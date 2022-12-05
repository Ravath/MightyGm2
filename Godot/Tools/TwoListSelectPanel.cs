using Godot;
using System;

public class TwoListSelectPanel : Control
{
	public event Action OnAddButtonPressed;
	public event Action OnRemoveButtonPressed;
	
	private ItemList fromList;
	private ItemList destList;

	public ItemList FromList{ get => fromList; }
	public ItemList DestList{ get => destList; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		fromList = GetNode<ItemList>("HB/FromList");
		destList = GetNode<ItemList>("HB/DestList");
	}

	#region Events
	private void _on_AddButton_pressed()
	{
		OnAddButtonPressed?.Invoke();
	}
	
	private void _on_RemoveButton_pressed()
	{
		OnRemoveButtonPressed?.Invoke();
	}
	#endregion

}
