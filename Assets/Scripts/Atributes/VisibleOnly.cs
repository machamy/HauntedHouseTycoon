


using UnityEngine;

public class VisibleOnly : PropertyAttribute
{
    

    public EditableIn EditableIn;
    public VisibleOnly(EditableIn editableIn = EditableIn.None)
    {
        this.EditableIn = editableIn;
    }
}
public enum EditableIn
{
    None,
    EditMode,
    PlayMode
}