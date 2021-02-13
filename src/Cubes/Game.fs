module Game

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

type Game1() as _this =
    inherit Game()

    let mutable input = Unchecked.defaultof<Input.State>
    let mutable pickerTarget = Unchecked.defaultof<RenderTarget2D>
    let mutable pickerData = Unchecked.defaultof<PackedVector.HalfVector2 []>

    let mutable gameContent = Unchecked.defaultof<Cubes.Content>
    let mutable axesContent = Unchecked.defaultof<Axes.Content>
    let mutable textContent = Unchecked.defaultof<Text.Content>
    let mutable debugContent = Unchecked.defaultof<Debug.Content>

    let mutable frames = 0
    let mutable lastFrames = 0
    let mutable lastMilliseconds = 0.0
    let mutable fps = 0.0

    let graphics = new GraphicsDeviceManager(_this)

    do graphics.GraphicsProfile <- GraphicsProfile.HiDef

    do graphics.PreferredBackBufferWidth <- 800
    do graphics.PreferredBackBufferHeight <- 600
    do graphics.IsFullScreen <- false

    do graphics.ApplyChanges()
    do base.Content.RootDirectory <- "content"

    do _this.IsMouseVisible <- true

    let updateInputState() =
        input <- Keyboard.GetState() |> Input.updated input

    override _this.Initialize() =
        base.Initialize()

    override _this.LoadContent() =
        let device = _this.GraphicsDevice

        input <- Input.initialState()

        axesContent <- Axes.loadContent _this device
        gameContent <- Cubes.loadContent _this device
        textContent <- Text.loadContent _this device
        debugContent <- Debug.loadContent _this

        let pp = device.PresentationParameters

        pickerTarget <- new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.HalfVector2, pp.DepthStencilFormat)
        pickerData <- Array.zeroCreate 1

    override _this.Update(gameTime) =
        updateInputState()

        if Input.justPressed input Keys.Escape then _this.Exit()

        base.Update(gameTime)

    override _this.Draw(gameTime) =

        frames <- frames + 1
        let elapsed = gameTime.TotalGameTime.TotalMilliseconds - lastMilliseconds
        if elapsed > 1000.0 then
            fps <- float (frames - lastFrames) * 1000.0 / elapsed
            lastMilliseconds <- gameTime.TotalGameTime.TotalMilliseconds
            lastFrames <- frames

        let device = _this.GraphicsDevice

        device.SetRenderTarget(pickerTarget)
        device.Clear(Color.Black)

        Cubes.drawPicker device gameContent gameTime

        // TODO

        let mouse = Mouse.GetState()

        let x, y = mouse.X, mouse.Y;

        let cubeIndex, faceIndex =
            if x >= 0 && y >= 0 && x < pickerTarget.Width && y < pickerTarget.Height then
                pickerTarget.GetData<PackedVector.HalfVector2>(0, Rectangle(x, y, 1, 1), pickerData, 0, 1)
                let pickColour = pickerData.[0].ToVector2()
                8.0f * pickColour.X, 6.0f * pickColour.Y
            else
                0.0f, 0.0f

        device.SetRenderTarget(null)
        device.Clear(Color.DarkGray)

        Cubes.draw device gameContent gameTime cubeIndex faceIndex
        Axes.draw device axesContent gameTime
        Text.draw device textContent fps
        Debug.draw device debugContent pickerTarget

        base.Draw(gameTime)
