using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ContextMenuItems
{
	private List<ContextMenuItem> _items = new List<ContextMenuItem>();

	public void AddMenuItem(ContextMenuItem newItem)
	{
		_items.Add(newItem);
	}

	public void AddMenuItem(params ContextMenuItem[] newItem)
	{
		_items.AddRange(newItem);
	}

	public void SetMenu(PopupMenu menu)
	{
		menu.Clear();
		foreach (var item in _items.Where(i=>i.Used))
		{
			menu.AddItem(item.Name);
		}
	}

	public ContextMenuItem GetItem(int index)
	{
		var t = from i in _items
				where i.Used
				select i;
		return t.ElementAt(index);
	}

	public bool UpdateUsedItems()
	{
		bool result = false;
		foreach (var item in _items)
		{
			if(item.IsUsedCondition != null)
				item.Used = item.IsUsedCondition();
			result |= item.Used;
		}
		return result;
	}

	public void OpenMenu(PopupMenu menu)
	{
		menu.Visible = UpdateUsedItems();
		if (menu.Visible)
		{
			SetMenu(menu);
			menu.SetPosition(menu.GetGlobalMousePosition());
		}
	}

	public void DoAction(int index)
	{
		GetItem(index).DoAction();
	}
}

public class ContextMenuItem
{
	public bool Used { get; set; }
	public string Name { get; set; }

	public delegate bool UsedCondition();
	public UsedCondition IsUsedCondition { get; set; }

	public delegate void ContextMenuOnClick();
	public ContextMenuOnClick DoAction { get; set; }
}
