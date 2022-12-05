using Godot;
using System;

public class UserInputPopup : ConfirmationDialog
{
	private Label messageLabel;
	private Label errorLabel;
	private LineEdit inputTextEdit;

	public delegate bool TextChecker(string input, ref string errorFeedback);
	public TextChecker OnCheck { get; set; }
	public Action<UserInputPopup> OnConfirm { get; set; }

	public string Message
	{
		get => messageLabel.Text;
		set => messageLabel.Text = value;
	}

	public string MessageError
	{
		get => errorLabel.Text;
		set => errorLabel.Text = value;
	}

	public string InputText {
		get => inputTextEdit.Text.Trim(' ');
		set => inputTextEdit.Text = value;
	}
	public int InputTextAsInt { get { return int.Parse(InputText); } }
	public double InputTextAsDouble { get { return double.Parse(InputText); } }

	#region Internal events
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		messageLabel = GetNode<Label>("MarginContainer/VBoxContainer/MessageLabel");
		errorLabel = GetNode<Label>("MarginContainer/VBoxContainer/ErrorLabel");
		inputTextEdit = GetNode<LineEdit>("MarginContainer/VBoxContainer/InputTextEdit");

		inputTextEdit.FocusMode = FocusModeEnum.All;

		inputTextEdit.FocusNext = GetOk().GetPath();
		GetOk().FocusNext = GetCancel().GetPath();
		GetCancel().FocusNext = inputTextEdit.GetPath();
		inputTextEdit.FocusPrevious = GetCancel().GetPath();
		GetOk().FocusPrevious = inputTextEdit.GetPath();
		GetCancel().FocusPrevious = GetOk().GetPath();

	}

	private void _on_InputTextEdit_text_changed(string newText)
	{
		string errMessage = "";
		bool isTextOk = OnCheck?.Invoke(inputTextEdit.Text, ref errMessage) ?? true;

		MessageError = isTextOk ? "" : errMessage ;
		GetOk().Disabled = !isTextOk;
	}

	private void _on_UserInputPopup_confirmed()
	{
		OnConfirm?.Invoke(this);
	}
	#endregion

	public void SetLineEditFocused()
	{
		inputTextEdit.GrabClickFocus();
	}

	#region Default Checkers
	public static bool IsNotEmptyStringChecker(string input, ref string errorMessage)
	{
		errorMessage = "Please enter some text...";
		return !string.IsNullOrWhiteSpace(input);
	}

	public static bool IsIntegerChecker(string input, ref string errorMessage)
	{
		errorMessage = "Please enter an integer number...";
		return int.TryParse(input, out int notUsed);
	}

	public static bool IsDoubleChecker(string input, ref string errorMessage)
	{
		errorMessage = "Please enter a real number...";
		return double.TryParse(input, out double notUsed);
	}
	#endregion

}

#region Static Popup access
static class UserInputPopupExtension
{
	public static UserInputPopup DefaultUserInputPopup(this Control control)
	{
		return control.GetNode<UserInputPopup>("/root/UserInputPopup");
	}

	public static UserInputPopup AskUser(this Control control,
		string message,
		UserInputPopup.TextChecker textChecker,
		Action<UserInputPopup> confirmAction)
	{
		UserInputPopup p = DefaultUserInputPopup(control);
		p.Message = message;
		p.OnCheck = textChecker;
		p.OnConfirm = confirmAction;
		p.InputText = "";
		p.MessageError = "";
		p.GetOk().Disabled = true;
		p.SetLineEditFocused();
		p.PopupCentered();
		return p;
	}

	public static UserInputPopup AskText(this Control control, string message, Action<UserInputPopup> confirmAction)
	{
		return AskUser(control, message, UserInputPopup.IsNotEmptyStringChecker, confirmAction);
	}

	public static UserInputPopup AskInteger(this Control control, string message, Action<UserInputPopup> confirmAction)
	{
		return AskUser(control, message, UserInputPopup.IsIntegerChecker, confirmAction);
	}

	public static UserInputPopup AskReal(this Control control, string message, Action<UserInputPopup> confirmAction)
	{
		return AskUser(control, message, UserInputPopup.IsDoubleChecker, confirmAction);
	}
}
#endregion
