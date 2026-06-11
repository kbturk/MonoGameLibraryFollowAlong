using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonogameLibrary.Input;

public class MouseInfo
{

    ///<summary>the state of the mouse during the preivous update cycle</summary>
    public MouseState PreviousState {get; private set;}

    ///<summary>the state of the mouse during the current update cycle</summary>
    public MouseState CurrentState {get; private set;}

    ///<summary>
    ///get or set the current position of the mouse cursor in screen space
    ///</summary>
    public Point Position
    {
        get => CurrentState.Position;
        set => SetPosition(value.X, value.Y);
    }

    ///<summary>
    ///get or set the current X-coordinate position of the mouse cursor in screen space
    ///</summary>
    public int X
    {
        get => CurrentState.X;
        set => SetPosition(value, CurrentState.Y);
    }

    ///<summary>
    ///get or set the current Y-coordinate position of the mouse cursor in screen space
    ///</summary>
    public int Y
    {
        get => CurrentState.Y;
        set => SetPosition(CurrentState.X, value);
    }

    ///<summary>Mouse Move Properties</summary>

    ///<summary>
    ///gets the difference in the mouse cursor position between the previous and current frame
    ///</summary>
    public Point PositionDelta => CurrentState.Position - PreviousState.Position;
    
    ///<summary>
    ///gets the difference in the mouse cursor x-position between the previous and current frame
    ///</summary>
    public int XDelta => CurrentState.X - PreviousState.X;

    ///<summary>
    ///gets the difference in the mouse cursor y-position between the previous and current frame
    ///</summary>
    public int YDelta => CurrentState.Y - PreviousState.Y;

    ///<summary>
    ///bool indicating if the mosue cursor moved between previous and current frame.
    ///</summary>
    public bool WasMoved => PositionDelta != Point.Zero;

    ///<summary>
    ///Get the cumulative value of the mouse scroll wheel since the start of the game.
    ///</summary>
    public int ScrollWheel => CurrentState.ScrollWheelValue;

    ///<summary>
    ///Get the cumulative value of the mouse scroll wheel since the start of the game.
    ///</summary>
    public int ScrollWheelDelta => CurrentState.ScrollWheelValue - PreviousState.ScrollWheelValue;

    ///<summary>Creates a new MouseInfo</summary>
    public MouseInfo()
    {
        PreviousState = new MouseState();
        CurrentState  = Mouse.GetState();
    }

    ///<summary>Updates the state information about mouse input.</summary>
    public void Update()
    {
        PreviousState = CurrentState;
        CurrentState  = Mouse.GetState();
    }

    ///<summary>
    ///Returns a bool indicating whether the specific mouse button is currently down.
    ///</summary>
    public bool IsButtonDown(MouseButton button)
    {
        switch (button)
        {
            case MouseButton.Left:
                return CurrentState.LeftButton   == ButtonState.Pressed;
            case MouseButton.Middle:
                return CurrentState.MiddleButton == ButtonState.Pressed;
            case MouseButton.Right:
                return CurrentState.RightButton  == ButtonState.Pressed;
            case MouseButton.XButton1:
                return CurrentState.XButton1     == ButtonState.Pressed;
            case MouseButton.XButton2:
                return CurrentState.XButton2     == ButtonState.Pressed;
            default:
                return false;
        }
    }

    ///<summary>
    ///Returns a bool indicating whether the specific mouse button is currently up.
    ///</summary>
    public bool IsButtonUp(MouseButton button)
    {
        switch (button)
        {
            case MouseButton.Left:
                return CurrentState.LeftButton   == ButtonState.Released;
            case MouseButton.Middle:
                return CurrentState.MiddleButton == ButtonState.Released;
            case MouseButton.Right:
                return CurrentState.RightButton  == ButtonState.Released;
            case MouseButton.XButton1:
                return CurrentState.XButton1     == ButtonState.Released;
            case MouseButton.XButton2:
                return CurrentState.XButton2     == ButtonState.Released;
            default:
                return false;
        }
    }

    ///<summary>
    ///Returns a bool indicating whether the specific mouse button was just pressed.
    ///</summary>
    public bool WasButtonJustPressed(MouseButton button)
    {
        switch (button)
        {
            case MouseButton.Left:
                return CurrentState.LeftButton   == ButtonState.Pressed && PreviousState.LeftButton   == ButtonState.Released;
            case MouseButton.Middle:
                return CurrentState.MiddleButton == ButtonState.Pressed && PreviousState.MiddleButton == ButtonState.Released;
            case MouseButton.Right:
                return CurrentState.RightButton  == ButtonState.Pressed && PreviousState.RightButton  == ButtonState.Released;
            case MouseButton.XButton1:
                return CurrentState.XButton1     == ButtonState.Pressed && PreviousState.XButton1     == ButtonState.Released;
            case MouseButton.XButton2:
                return CurrentState.XButton2     == ButtonState.Pressed && PreviousState.XButton2     == ButtonState.Released;
            default:
                return false;
        }
    }

    ///<summary>
    ///Returns a bool indicating whether the specific mouse button was just released.
    ///</summary>
    public bool WasButtonJustReleased(MouseButton button)
    {
        switch (button)
        {
            case MouseButton.Left:
                return CurrentState.LeftButton   == ButtonState.Released && PreviousState.LeftButton   == ButtonState.Pressed;
            case MouseButton.Middle:
                return CurrentState.MiddleButton == ButtonState.Released && PreviousState.MiddleButton == ButtonState.Pressed;
            case MouseButton.Right:
                return CurrentState.RightButton  == ButtonState.Released && PreviousState.RightButton  == ButtonState.Pressed;
            case MouseButton.XButton1:
                return CurrentState.XButton1 == ButtonState.Released && PreviousState.XButton1         == ButtonState.Pressed;
            case MouseButton.XButton2:
                return CurrentState.XButton2 == ButtonState.Released && PreviousState.XButton2         == ButtonState.Pressed;
            default:
                return false;
        }
    }

    ///<summary>
    ///Sets the current position of the mouse cursor in screen space and updates the CurrentState
    ///with the new position.
    ///</summary>
    public void SetPosition(int x, int y)
    {
        Mouse.SetPosition(x,y);
        CurrentState = new MouseState(
                x,
                y,
                CurrentState.ScrollWheelValue,
                CurrentState.LeftButton,
                CurrentState.MiddleButton,
                CurrentState.RightButton,
                CurrentState.XButton1,
                CurrentState.XButton2
                );
    }
}
