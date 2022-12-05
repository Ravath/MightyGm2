using Godot;
using L5R4;
using MightyGm2.Engine.Control;
using MightyGm2.Engine.RpgDatabase;
using MightyGm2.RPG.L5R4.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RpgManagementPanel : Control
{
	private const string META_ID = "ID";

	private MenuButton _rpgMenuButton;
	PopupMenu popup;
	private Tree _itemList;
	private TreeItem root;

	private DatabaseCtrl _data;
	private RpgControl _rpg;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_rpgMenuButton = GetNode<MenuButton>("RpgMenuButton");
		_itemList = GetNode<Tree>("ItemList");

		popup = _rpgMenuButton.GetPopup();
		popup.Connect("index_pressed", this, nameof(_on_rpg_item_pressed));

		_data = ApplicationControl.Control.Database;
		_rpg = ApplicationControl.Control.Rpg;
		_rpg.Rpgs.OnSelect += SetRpg;

		popup.Clear();
		foreach (Rpg rgp in _rpg.Rpgs)
		{
			popup.AddItem(rgp.Name);
		}
	}

	private void _on_rpg_item_pressed(int index)
	{
		_rpg.Rpgs.Select(index);
	}

	private void SetRpg(Rpg rpg)
	{
		_itemList.Clear();
		_itemList.HideRoot = true;
		root = _itemList.CreateItem();

		foreach (var campaign in rpg.Campaigns)
		{
			var campaignItem = _itemList.CreateItem(root);
			campaignItem.SetText(0, campaign.Name);
			campaignItem.SetMeta(META_ID, campaign.Id);

			foreach (var chapter in campaign.Chapters)
			{
				var chapterItem = _itemList.CreateItem(campaignItem);
				chapterItem.SetText(0, chapter.Name);
				chapterItem.SetMeta(META_ID, chapter.Id);

				foreach (var session in chapter.Sessions)
				{
					var sessionItem = _itemList.CreateItem(chapterItem);
					sessionItem.SetText(0, session.Name);
					sessionItem.SetMeta(META_ID, session.Id);

				}
			}
		}
	}

	private void _on_ItemList_item_selected()
	{
		var sel = _itemList.GetSelected();
		int id = (int)sel.GetMeta(META_ID);
		GD.Print("Id : ",id);

		if (sel.GetParent() == root)
		{
			GD.Print(sel.GetText(0), ": Campaign");
		} else if (sel.GetParent().GetParent() == root)
		{
			GD.Print(sel.GetText(0), ": Chapter");
		}
		else
		{
			GD.Print(sel.GetText(0), ": Session");
		}
		//TODO identify the type
	}

	private void UpdateElements()
	{
		throw new NotImplementedException();
		// TODO enable.disable buttons
	}
}
