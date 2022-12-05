using Engine.RpgLogic;
using Godot;
using MightyGm2.Engine.Control;
using MightyGm2.Engine.RpgDatabase;
using MightyGm2.RPG.L5R4.Data;
using MightyGm2.RPG.L5R4.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class RpgDataViewer : Control
{
	private DatabaseCtrl data;

	private MenuButton _menuButton;
	private ItemList _itemList;
	private LineEdit _filter;
	private DataViewer _dataViewer;
	PopupMenu popup;

	// All the data sets
	private IEnumerable<DataCollection> _data;
	// The currently selected data set
	private IEnumerable<INamed> _currentDataSet;
	// The currently selected data set with filter
	private IEnumerable<INamed> _filteredDataSet;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		data = ApplicationControl.Control.Database;

		_menuButton = GetNode<MenuButton>("HBoxContainer/VBoxContainer/MenuButton");
		_itemList = GetNode<ItemList>("HBoxContainer/VBoxContainer/ItemList");
		_filter = GetNode<LineEdit>("HBoxContainer/VBoxContainer/FilterEdit");
		_dataViewer = GetNode<DataViewer>("HBoxContainer/MarginContainer/DataViewer");
		_dataViewer.SetData(null);

		popup = _menuButton.GetPopup();
		popup.Connect("index_pressed", this, nameof(_on_item_pressed));

		_data = ModelFactory.Factory.GetAllData();
		foreach (DataCollection collection in _data)
		{
			popup.AddItem(collection.CollectionName);
		}

	}

	/// <summary>
	/// Update the data items list whith the current dataset items.
	/// </summary>
	private void UpdateElements()
	{
		_itemList.Clear();
		foreach (var item in _filteredDataSet)
		{
			_itemList.AddItem(item.Name);
		}
	}

	/// <summary>
	/// When a dataset is selected, update the data items list.
	/// </summary>
	/// <param name="index"></param>
	private void _on_item_pressed(int index)
	{
		_currentDataSet = _data.ElementAt(index).Collection;
		_filteredDataSet = _currentDataSet;
		_filter.Text = "";
		UpdateElements();
	}

	/// <summary>
	/// When a data item is selected, display the item.
	/// </summary>
	/// <param name="index"></param>
	private void _on_ItemList_item_selected(int index)
	{
		var rpgItem = _filteredDataSet.ElementAt(index);
		_dataViewer.SetData(rpgItem);
	}

	/// <summary>
	/// When the text filter is updated, filter the current dataset items.
	/// </summary>
	/// <param name="new_text"></param>
	private void _on_FilterEdit_text_changed(string new_text)
{
		if (_currentDataSet == null)
			return;
		if (string.IsNullOrWhiteSpace(new_text))
		{
			_filteredDataSet = _currentDataSet;
		}
		else
		{
			Regex reg = new Regex(new_text, RegexOptions.IgnoreCase);
			_filteredDataSet = _currentDataSet.Where(i => reg.IsMatch(i.Name));
		}
		UpdateElements();
	}
}
