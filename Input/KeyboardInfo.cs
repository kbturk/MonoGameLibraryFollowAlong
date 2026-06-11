using Microsoft.Xna.Framework.Input;

namespace MonogameLibrary.Input;

public class KeyboardInfo
{
    ///<summary>get the state of the keyboard input during the previous update cycle.</summary>
    public KeyboardState PreviousState { get; private set; }

    ///<summary>get the current state of the keyboard input. </summary>
    public KeyboardState CurrentState  { get; private set; }

    public KeyboardInfo()
    {
        PreviousState = new KeyboardState();
        CurrentState =  Keyboard.GetState();
    }

    /// <summary>updates the state information about keyboard input.</summary>
    public void Update()
    {
        PreviousState = CurrentState;
        CurrentState  = Keyboard.GetState();
    }

    /// <summary> helper function that returns whether or not a key is currently down
    ///</summary>
    ///<param name="key">The key to check.</param>
    public bool IsKeyDown(Keys key)
    {
        return CurrentState.IsKeyDown(key);
    }

    /// <summary> helper function that returns whether or not a key is currently up.
    ///</summary>
    ///<param name="key">The key to check.</param>
    public bool IsKeyUp(Keys key)
    {
        return CurrentState.IsKeyUp(key);
    }

    /// <summary> Returns a bool that indicates if the key was just pressed on the current frame.
    ///</summary>
    ///<param name="key">The key to check.</param>
    public bool WasKeyJustPressed(Keys key)
    {
        return CurrentState.IsKeyDown(key) && PreviousState.IsKeyUp(key);
    }

    ///<summary> Returns a bool that indicates if the key was just released on the current frame.
    ///</summary>
    ///<param name="key">The key to check.</param>
    public bool WasKeyJustReleased(Keys key)
    {
        return CurrentState.IsKeyUp(key) && PreviousState.IsKeyDown(key);
    }

}
