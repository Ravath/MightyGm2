using Engine.RpgLogic;
using Godot;

/// <summary>
/// The Parent Control for displaying a INamed object from the NamedListDisplay.
/// </summary>
public interface INamedDisplay
{
    /// <summary>
    /// Display the given INamed Object.
    /// </summary>
    /// <param name="toDisplay">Object to display.</param>
    void SetDisplay(INamed toDisplay);
    /// <summary>
    /// Control for displaying in the GUI. (Generally 'this');
    /// </summary>
    Control Control { get; }
}
