module Input

open Microsoft.Xna.Framework.Input

type State = 
    {
        Keyboard: KeyboardState
        PreviousKeyboard: KeyboardState

        Mouse: MouseState
        PreviousMouse: MouseState
    }

let initialState() =
    let keyboard = Keyboard.GetState()
    let mouse = Mouse.GetState()
    {
        Keyboard = keyboard
        PreviousKeyboard = keyboard
        Mouse = mouse
        PreviousMouse = mouse
    }

let update input  =
    {
        Keyboard = Keyboard.GetState()
        PreviousKeyboard = input.Keyboard
        Mouse = Mouse.GetState()
        PreviousMouse = input.Mouse
    }

let justPressed input key =
    input.Keyboard.IsKeyDown(key) && input.Keyboard.IsKeyUp(key)

let isPressed input key =
    input.Keyboard.IsKeyDown(key)

let isRightDragging input =
    input.Mouse.RightButton = ButtonState.Pressed && input.PreviousMouse.RightButton = ButtonState.Pressed

let mouseMovement input =
    (input.Mouse.X - input.PreviousMouse.X, input.Mouse.Y - input.PreviousMouse.Y)
