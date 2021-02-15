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

let draw (device: GraphicsDevice) (viewMatrix: Matrix) (projectionMatrix: Matrix) content (gameTime: GameTime) =
    let time = (single gameTime.TotalGameTime.TotalMilliseconds) / 100.0f

    let effect = content.Effect

    effect.CurrentTechnique <- effect.Techniques.["PosCol"]
    effect.Parameters.["xWorld"].SetValue(Matrix.Identity)
    effect.Parameters.["xView"].SetValue(viewMatrix)
    effect.Parameters.["xProjection"].SetValue(projectionMatrix)
    
    device.DepthStencilState <- DepthStencilState.Default

    effect.CurrentTechnique.Passes |> Seq.iter
        (fun pass ->
            pass.Apply()
            device.DrawUserPrimitives(PrimitiveType.LineList, content.Vertices, 0, content.Vertices.Length / 2)
        )
