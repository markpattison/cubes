module Axes

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type Content =
    {
        Effect: Effect
        Vertices: VertexPositionColor []
    }

let positions = [|
    // Front face
    (0.0f, 0.0f, 0.0f)
    (2.0f, 0.0f, 0.0f)
    (0.0f, 0.0f, 0.0f)
    (0.0f, 2.0f, 0.0f)
    (0.0f, 0.0f, 0.0f)
    (0.0f, 0.0f, 2.0f) |] |> Array.map (fun (x, y, z) -> Vector3(x, y, z))

let colours = [|
    Color.Red
    Color.Red
    Color.Green
    Color.Green
    Color.Blue
    Color.Blue |]

let loadContent (_this: Game) device =
    {
        Effect = _this.Content.Load<Effect>("effects/posCol")

        Vertices =
            Array.init 6 (fun i ->
                VertexPositionColor(positions.[i], colours.[i]))
    }

let draw (device: GraphicsDevice) content (gameTime: GameTime) =
    let time = (single gameTime.TotalGameTime.TotalMilliseconds) / 100.0f

    let effect = content.Effect

    effect.CurrentTechnique <- effect.Techniques.["PosCol"]
    effect.Parameters.["xWorld"].SetValue(Matrix.Identity)
    effect.Parameters.["xView"].SetValue(Matrix.CreateLookAt(Vector3(-5.0f, 2.0f, 5.0f), Vector3.Zero, Vector3.UnitY))
    effect.Parameters.["xProjection"].SetValue(Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.1f, 100.0f))
    
    device.DepthStencilState <- DepthStencilState.Default

    effect.CurrentTechnique.Passes |> Seq.iter
        (fun pass ->
            pass.Apply()
            device.DrawUserPrimitives(PrimitiveType.LineList, content.Vertices, 0, content.Vertices.Length / 2)
        )
