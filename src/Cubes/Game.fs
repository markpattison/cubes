module Game

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

type Game1() as _this =
    inherit Game()

    let mutable input = Unchecked.defaultof<Input.State>
    let mutable pickerTarget = Unchecked.defaultof<RenderTarget2D>

    let mutable gameContent = Unchecked.defaultof<Cubes.Content>
    let mutable axesContent = Unchecked.defaultof<Axes.Content>
    let mutable textContent = Unchecked.defaultof<Text.Content>
    let mutable debugContent = Unchecked.defaultof<Debug.Content>

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

    override _this.Update(gameTime) =
        updateInputState()

        if Input.justPressed input Keys.Escape then _this.Exit()

        base.Update(gameTime)

    override _this.Draw(gameTime) =
        let device = _this.GraphicsDevice

        device.SetRenderTarget(pickerTarget)
        device.Clear(Color.Black)

        Cubes.drawPicker device gameContent gameTime

        // TODO
        let cubeIndex = 0
        let faceIndex = 0

        device.SetRenderTarget(null)
        device.Clear(Color.DarkGray)

        Cubes.draw device gameContent gameTime
        Axes.draw device axesContent gameTime
        Text.draw device textContent
        Debug.draw device debugContent pickerTarget

        base.Draw(gameTime)
