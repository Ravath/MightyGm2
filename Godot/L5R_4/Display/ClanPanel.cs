using Godot;
using L5R.Model.School;
using L5R4;
using MightyGm2.RPG.L5R4.Data;
using MightyGm2.RPG.L5R4.Model;
using System;
using System.Collections.Generic;
using System.Linq;

public class ClanPanel : Control
{
	private const string Format = "[color="+ LocalContext.GOLD + "]{0}[/color]\n\n{1}";
	private Label _clanName;
	private Label _clanDesc;
	private Label _familyName;
	private RichTextLabel _familyDesc;

	private ItemList _familyList;
	private ItemList _schoolList;
	private SchoolPanel _schoolPanel;

	private List<Famille> _familySet = new List<Famille>();
	private List<Ecole> _schoolSet = new List<Ecole>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_clanName = GetNode<Label>("VBoxContainer/ClanName");
		_clanDesc = GetNode<Label>("VBoxContainer/MarginContainer/ClanDescription");
		_familyName = GetNode<Label>("VBoxContainer/VBoxContainer/VBoxContainer/FamilyName");
		_familyDesc = GetNode<RichTextLabel>("VBoxContainer/VBoxContainer/VBoxContainer/FamilyDescription");
		_familyList = GetNode<ItemList>("VBoxContainer/VBoxContainer/FamilyList");
		_schoolList = GetNode<ItemList>("VBoxContainer/VBoxContainer/SchoolList");
		_schoolPanel = GetNode<SchoolPanel>("VBoxContainer/VBoxContainer/SchoolPanel");
	}

	public void SetClan(Clan clan)
	{
		_clanName.Text = clan.Name;
		_clanDesc.Text = clan.Description;

		_familySet.Clear();
		_schoolSet.Clear();
		_familyList.Clear();
		_schoolList.Clear();
		foreach (var item in ModelFactory.Factory.data.Ecole.Items.Where(s=>s.Clan_Tag == clan.Tag))
		{
			Ecole e = ModelFactory.Factory.InstantiateSchool(item);
			_schoolSet.Add(e);
			_schoolList.AddItem(e.Name);
		}
		foreach (var item in ModelFactory.Factory.data.Family.Items.Where(s => s.Clan_Tag == clan.Tag))
		{
			Famille f = new Famille();
			f.SetModel(item);
			_familySet.Add(f);
			_familyList.AddItem(f.Name);
		}

		if (_familySet.Count() > 0)
		{
			_familyList.Select(0);
			SetFamily(_familySet.ElementAt(0));
		}
		else { SetFamily(null); }
		if (_schoolSet.Count() > 0)
		{
			_schoolList.Select(0);
			SetSchool(_schoolSet.ElementAt(0));
		}
		else { SetSchool(null); }
	}

	public void SetFamily(Famille family)
	{
		if(family == null)
		{
			_familyName.Text = "";
			_familyDesc.Text = "";
		}
		else
		{
			_familyName.Text = family.Name;
			_familyDesc.BbcodeText = string.Format(Format, family.BonusTrait, family.Description);
		}
	}

	public void SetSchool(Ecole school)
	{
		_schoolPanel.SetSchool(school);
	}

	private void _on_FamilyList_item_selected(int index)
	{
		SetFamily(_familySet[index]);
	}

	private void _on_SchoolList_item_selected(int index)
	{
		SetSchool(_schoolSet[index]);
	}
}
