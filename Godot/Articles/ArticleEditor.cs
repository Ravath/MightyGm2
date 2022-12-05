using Godot;
using System;
using System.Linq;

public class ArticleEditor : Control
{
	private float _updatePreview;
	private const float UpdatePreviewDelay = 0.2f;

	private TextEdit _editor;
	private RichTextLabel _preview;
	private Container _buttons;
	private ColorPickerButton _selectedColor;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_editor = GetNode<TextEdit>("VBoxContainer/TextEdit");
		_preview = GetNode<RichTextLabel>("VBoxContainer/RichTextLabel");
		_buttons = GetNode<Container>("VBoxContainer/Buttons");
		_selectedColor = GetNode<ColorPickerButton>("VBoxContainer/Buttons/current_color");

		foreach (var item in _buttons.GetChildren().OfType<Button>())
		{
			if(item != _selectedColor)
				item.Connect("pressed", this, nameof(_on_Button_pressed));
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if(_updatePreview > 0)
		{
			_updatePreview -= delta;
			if (_updatePreview < 0)
				UpdatePreview();
		}
	}

	private void UpdatePreview()
	{
		_preview.BbcodeText = _editor.Text;
	}

	private void _on_Button_pressed()
	{
		foreach (var item in _buttons.GetChildren().OfType<Button>())
		{
			// Find pressed button
			if (item.Pressed)
			{
				// Get the bbCode
				string name = item.Name;
				string options = "";

				// Specific balises arguments
				if (name == "color")
				{
					options = "=#" + _selectedColor.Color.ToHtml(false);
				}

				// Insert balises in text
				if (_editor.IsSelectionActive())
				{
					int selFromLine = _editor.GetSelectionFromLine();
					int selToLine = _editor.GetSelectionToLine();

					int selTo = 0;
					int selFrom = 0;

					// Get line positions in the text
					string newText = _editor.Text;
					while(selFromLine > 0)
					{
						selFrom = newText.Find("\n", selFrom)+1;
						selFromLine--;
					}
					while (selToLine > 0)
					{
						selTo = newText.Find("\n", selTo)+1;
						selToLine--;
					}

					// Get selection position
					selTo += _editor.GetSelectionToColumn();
					selFrom += _editor.GetSelectionFromColumn();

					// Update text
					newText = _editor.Text.Insert(selTo, "[/"+name+"]");
					newText = newText.Insert(selFrom, "["+name + options + "]");
					_editor.Text = newText;
				}
				else
				{
					_editor.InsertTextAtCursor(string.Format("[{0}{1}][/{0}]", name, options));
				}
			}
		}
		_updatePreview = UpdatePreviewDelay;
	}

	private void _on_TextEdit_text_changed()
	{
		_updatePreview = UpdatePreviewDelay;
	}
}
