using Engine.RpgLogic;
using Godot;
using System.Collections.Generic;
using System.Linq;

public class NamedListDisplay : Control
{
	#region Events
	public delegate void ItemSelected(INamed seletedItem);
	/// <summary>
	/// Raised when a item is selected in the list.
	/// </summary>
	public event ItemSelected OnSelectedItem;
	#endregion

	#region Members
	/// <summary>
	/// The display list of items.
	/// </summary>
	private VBoxContainer _list;
	/// <summary>
	/// The Item Displayer of the List Items.
	/// </summary>
	private INamedDisplay _listItemTemplate;
	/// <summary>
	/// The Item Displayer of the currently selected Item.
	/// </summary>
	private INamedDisplay _itemDisplayer;
	/// <summary>
	/// Contains the ItemDisplayer.
	/// </summary>
	private MarginContainer _displayContainer;
	private IEnumerable<INamed> _items;
	#endregion

	#region Properties
	/// <summary>
	/// Template of the list items display.
	/// </summary>
	public INamedDisplay ListItemTemplate
	{
		get { return _listItemTemplate; }
		set
		{
			if (_listItemTemplate != value)
			{
				_listItemTemplate.Control.QueueFree();
				_listItemTemplate = value;
				// Let pass mouse event for it to be processed by NamedListDisplay.
				_listItemTemplate.Control.MouseFilter = MouseFilterEnum.Pass;
				// Update display
				if (_items != null && _items.Count() > 0)
					SetList(_items);
			}
		}
	}

	/// <summary>
	/// Control displaying the selected item.
	/// </summary>
	public INamedDisplay ItemDisplayer
	{
		get { return _itemDisplayer; }
		set
		{
			if (_itemDisplayer != value)
			{
				_itemDisplayer.Control.QueueFree();
				_itemDisplayer = value;
				_displayContainer.AddChild(_itemDisplayer.Control);
				if (_displayContainer.Visible)
					ItemDisplayer.SetDisplay(SelectedItem);
			}
		}
	}

	/// <summary>
	/// The currently selected item.
	/// </summary>
	public INamed SelectedItem { get; private set; } 
	#endregion

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_list = GetNode<VBoxContainer>("List");
		_listItemTemplate = GetNode<INamedDisplay>("Template");
		_displayContainer = GetNode<MarginContainer>("List/MarginContainer");
		_itemDisplayer = GetNode<INamedDisplay>("List/MarginContainer/DisplayerTemplate");
		_displayContainer.Visible = false;
	}

	/// <summary>
	/// Add the list to display.
	/// </summary>
	/// <param name="items">List to display.</param>
	public void SetList(IEnumerable<INamed> items)
	{
		// Update list
		_items = items;

		// Remove previous ListItems
		foreach (INamedDisplay item in _list.GetChildren().OfType<INamedDisplay>())
		{
			item.Control.QueueFree();
		}

		if (_items == null)
			return;

		// Add items using the ListItemTemplate
		foreach (var newItem in _items)
		{
			INamedDisplay newLabel = (INamedDisplay)ListItemTemplate.Control.Duplicate();
			_list.AddChild(newLabel.Control);
			newLabel.SetDisplay(newItem);
			newLabel.Control.Visible = true;
		}

		// No item selected
		SelectedItem = null;
		_displayContainer.Visible = false;
	}

	private void OnItemClicked(int index)
	{
		// Check if already selected
		INamed newItem = _items.ElementAt(index);

		// If already selected, swap the displayer
		if(newItem == SelectedItem) {
			_displayContainer.Visible = !_displayContainer.Visible;
			return;
		}

		// Do update
		SelectedItem = newItem;

		// Update the displayer
		_list.MoveChild(_displayContainer, index + 1);
		ItemDisplayer.SetDisplay(SelectedItem);
		_displayContainer.Visible = true;

		// Raise selected item event
		OnSelectedItem?.Invoke(SelectedItem);
	}

	private void _on_List_gui_input(object @event)
	{
		if (@event is InputEventMouseButton mouseEvent
			&& mouseEvent.IsPressed())
		{
			// Find if a list item has been clicked
			int index = 0;
			foreach (Control item in _list.GetChildren().OfType<Control>())
			{
				if (item == _displayContainer) continue;

				// Is the label clicked?
				if (item.GetGlobalRect().HasPoint(mouseEvent.GlobalPosition))
				{
					// Do click update
					OnItemClicked(index);
					break;
				}
				index++;
			}
		}
	}
}
