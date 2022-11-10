using UnityEngine;

public abstract class InputController : ScriptableObject
{
    public abstract float RetrieveMoveInput();
    public abstract float RetrieveMoveInput2();
    public abstract bool RetrieveJumpInput();
}
