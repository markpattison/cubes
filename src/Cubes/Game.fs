module Game

open System.IO
open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input

type Game1() as _this =
    inherit Game()
    let mutable input = Unchecked.defaultof<Input.State>
    let mutable gameContent = Unchecked.defaultof<Sample.Content>
    let mutable axesContent = Unchecked.defaultof<Axes.Content>
    let mutable pickerTarget = Unchecked.defaultof<RenderTarget2D>
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
        input <- Input.initialState()

        axesContent <- Axes.loadContent _this _this.GraphicsDevice
        gameContent <- Sample.loadContent _this _this.GraphicsDevice
        debugContent <- Debug.loadContent _this

        let pp = _this.GraphicsDevice.PresentationParameters

        pickerTarget <- new RenderTarget2D(_this.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.HalfVector2, pp.DepthStencilFormat)

    override _this.Update(gameTime) =
        updateInputState()

        if Input.justPressed input Keys.Escape then _this.Exit()

        base.Update(gameTime)

    override _this.Draw(gameTime) =

        _this.GraphicsDevice.SetRenderTarget(pickerTarget)
        _this.GraphicsDevice.Clear(Color.Black)

        Sample.drawPicker _this.GraphicsDevice gameContent gameTime

        _this.GraphicsDevice.SetRenderTarget(null)
        _this.GraphicsDevice.Clear(Color.DarkGray)

        Sample.draw _this.GraphicsDevice gameContent gameTime
        Axes.draw _this.GraphicsDevice axesContent gameTime
        Debug.draw _this.GraphicsDevice debugContent pickerTarget

        base.Draw(gameTime)
