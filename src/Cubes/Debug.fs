module Debug

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type Content =
    {
        Vertices: VertexPositionTexture[]
        Effect: Effect
    }

let loadContent (_this: Game) =
    {
        Vertices = [|
            VertexPositionTexture(Vector3(-0.9f, -0.9f, 0.0f), Vector2(0.0f, 1.0f));
            VertexPositionTexture(Vector3(-0.9f, -0.5f, 0.0f), Vector2(0.0f, 0.0f));
            VertexPositionTexture(Vector3(-0.5f, -0.9f, 0.0f), Vector2(1.0f, 1.0f));

            VertexPositionTexture(Vector3(-0.5f, -0.9f, 0.0f), Vector2(1.0f, 1.0f));
            VertexPositionTexture(Vector3(-0.9f, -0.5f, 0.0f), Vector2(0.0f, 0.0f));
            VertexPositionTexture(Vector3(-0.5f, -0.5f, 0.0f), Vector2(1.0f, 0.0f));
        |]

        Effect = _this.Content.Load<Effect>("effects/debug")
    }

let draw (device: GraphicsDevice) content (texture: Texture2D) =
    let effect = content.Effect

    effect.CurrentTechnique <- effect.Techniques.["Debug"]

    effect.Parameters.["xDebugTexture"].SetValue(texture)

    effect.CurrentTechnique.Passes |> Seq.iter
        (fun pass ->
            pass.Apply()
            device.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, content.Vertices, 0, 2)
        )
