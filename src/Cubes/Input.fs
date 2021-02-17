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

let justPressed inputState key =
    inputState.Keyboard.IsKeyDown(key) && inputState.Keyboard.IsKeyUp(key)

let isPressed inputState key =
    inputState.Keyboard.IsKeyDown(key)