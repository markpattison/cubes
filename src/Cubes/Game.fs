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
    let mutable debugVertices = Unchecked.defaultof<VertexPositionTexture[]>
    let mutable debugEffect = Unchecked.defaultof<Effect>

    let graphics = new GraphicsDeviceManager(_this)

    do graphics.GraphicsProfile <- GraphicsProfile.HiDef

    do graphics.PreferredBackBufferWidth <- 800
    do graphics.PreferredBackBufferHeight <- 600
    do graphics.IsFullScreen <- false

    do graphics.ApplyChanges()
    do base.Content.RootDirectory <- "content"

    let updateInputState() =
        input <- Keyboard.GetState() |> Input.updated input

    override _this.Initialize() =
        base.Initialize()

    override _this.LoadContent() =
        input <- Input.initialState()

        axesContent <- Axes.loadContent _this _this.GraphicsDevice
        gameContent <- Sample.loadContent _this _this.GraphicsDevice

        debugVertices <- [|
            VertexPositionTexture(Vector3(-0.9f, -0.9f, 0.0f), Vector2(0.0f, 1.0f));
            VertexPositionTexture(Vector3(-0.9f, -0.5f, 0.0f), Vector2(0.0f, 0.0f));
            VertexPositionTexture(Vector3(-0.5f, -0.9f, 0.0f), Vector2(1.0f, 1.0f));

            VertexPositionTexture(Vector3(-0.5f, -0.9f, 0.0f), Vector2(1.0f, 1.0f));
            VertexPositionTexture(Vector3(-0.9f, -0.5f, 0.0f), Vector2(0.0f, 0.0f));
            VertexPositionTexture(Vector3(-0.5f, -0.5f, 0.0f), Vector2(1.0f, 0.0f));
        |]

        debugEffect <- _this.Content.Load<Effect>("effects/debug")

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
        
        // debug

        let effect = debugEffect

        effect.CurrentTechnique <- effect.Techniques.["Debug"]
    
        effect.Parameters.["xDebugTexture"].SetValue(pickerTarget)

        effect.CurrentTechnique.Passes |> Seq.iter
            (fun pass ->
                pass.Apply()
                _this.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, debugVertices, 0, debugVertices.Length / 3)
            )

        base.Draw(gameTime)
