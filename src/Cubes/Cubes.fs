﻿module Sample

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

open VertexPositionNormalColour

type Content =
    {
        SpriteBatch: SpriteBatch
        SpriteFont: SpriteFont
        Effect: Effect
        Vertices: VertexPositionNormalColour []
        PickerVertices: VertexPositionColor []
        Indices: int []
        Cubes: (Vector3 * float32) list
    }

let positions = [|
    // Front face
    (-1.0f, -1.0f,  1.0f)
    (-1.0f,  1.0f,  1.0f)
    ( 1.0f, -1.0f,  1.0f)
    ( 1.0f,  1.0f,  1.0f)

    // Back face
    ( 1.0f, -1.0f, -1.0f)
    ( 1.0f,  1.0f, -1.0f)
    (-1.0f, -1.0f, -1.0f)
    (-1.0f,  1.0f, -1.0f)

    // Top face
    (-1.0f,  1.0f,  1.0f)
    (-1.0f,  1.0f, -1.0f)
    ( 1.0f,  1.0f,  1.0f)
    ( 1.0f,  1.0f, -1.0f)

    // Bottom face
    (-1.0f, -1.0f, -1.0f)
    (-1.0f, -1.0f,  1.0f)
    ( 1.0f, -1.0f, -1.0f)
    ( 1.0f, -1.0f,  1.0f)

    // Right face
    ( 1.0f, -1.0f,  1.0f)
    ( 1.0f,  1.0f,  1.0f)
    ( 1.0f, -1.0f, -1.0f)
    ( 1.0f,  1.0f, -1.0f)

    // Left face
    (-1.0f, -1.0f, -1.0f)
    (-1.0f,  1.0f, -1.0f)
    (-1.0f, -1.0f,  1.0f)
    (-1.0f,  1.0f,  1.0f) |] |> Array.map (fun (x, y, z) -> Vector3(x, y, z))

let normals = [|
    // Front face
    (0.0f,  0.0f,  1.0f)
    (0.0f,  0.0f,  1.0f)
    (0.0f,  0.0f,  1.0f)
    (0.0f,  0.0f,  1.0f)

    // Back face
    (0.0f,  0.0f, -1.0f)
    (0.0f,  0.0f, -1.0f)
    (0.0f,  0.0f, -1.0f)
    (0.0f,  0.0f, -1.0f)

    // Top face
    (0.0f,  1.0f,  0.0f)
    (0.0f,  1.0f,  0.0f)
    (0.0f,  1.0f,  0.0f)
    (0.0f,  1.0f,  0.0f)

    // Bottom face
    (0.0f, -1.0f,  0.0f)
    (0.0f, -1.0f,  0.0f)
    (0.0f, -1.0f,  0.0f)
    (0.0f, -1.0f,  0.0f)

    // Right face
    (1.0f,  0.0f,  0.0f)
    (1.0f,  0.0f,  0.0f)
    (1.0f,  0.0f,  0.0f)
    (1.0f,  0.0f,  0.0f)

    // Left face
    (-1.0f,  0.0f,  0.0f)
    (-1.0f,  0.0f,  0.0f)
    (-1.0f,  0.0f,  0.0f)
    (-1.0f,  0.0f,  0.0f) |] |> Array.map (fun (x, y, z) -> Vector3(x, y, z))

let indices = [|
    0;  1;  2;      2;   1;  3;   // front
    4;  5;  6;      6;   5;  7;   // back
    8;  9;  10;     10;  9; 11;   // top
    12; 13; 14;     14; 13; 15;   // bottom
    16; 17; 18;     18; 17; 19;   // right
    20; 21; 22;     22; 21; 23    // left
|]

let colours = [|
    Color.Red     // front
    Color.Green   // back
    Color.Blue    // top
    Color.Orange  // bottom
    Color.Brown   // right
    Color.Purple  // left
|]

let loadContent (_this: Game) device =
    {
        Effect = _this.Content.Load<Effect>("effects/effects")
        SpriteFont = _this.Content.Load<SpriteFont>("Fonts/Arial")
        SpriteBatch = new SpriteBatch(device)

        Vertices =
            Array.init 24 (fun i ->
                VertexPositionNormalColour(positions.[i], normals.[i], colours.[i / 4]))
        
        PickerVertices =
            Array.init 24 (fun i ->
                let faceIndex = 50 * (i / 4)
                VertexPositionColor(positions.[i], Color(faceIndex, faceIndex, faceIndex) ))

        Indices = indices
        Cubes =
            [ Vector3(0.0f, 0.0f, 0.0f), 1.0f
              Vector3(-1.0f, 0.0f, 0.0f), 0.5f
              Vector3(-2.0f, 0.0f, 0.0f), 0.25f ]
    }

let showParameters gameContent =
    let colour = Color.DarkSlateGray

    gameContent.SpriteBatch.Begin()
    gameContent.SpriteBatch.DrawString(gameContent.SpriteFont, "monogame-fsharp", Vector2(10.0f, 10.0f), colour)
    gameContent.SpriteBatch.End()

let draw (device: GraphicsDevice) gameContent (gameTime: GameTime) =
    let time = (single gameTime.TotalGameTime.TotalMilliseconds) / 100.0f

    let effect = gameContent.Effect

    effect.CurrentTechnique <- effect.Techniques.["Cube"]
    effect.Parameters.["xView"].SetValue(Matrix.CreateLookAt(Vector3(-5.0f, 2.0f, 5.0f), Vector3.Zero, Vector3.UnitY))
    effect.Parameters.["xProjection"].SetValue(Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.1f, 100.0f))
    effect.Parameters.["xAmbient"].SetValue(0.2f)
    effect.Parameters.["xLightPosition"].SetValue(Vector3(-5.0f, 2.0f, 5.0f))
    
    device.DepthStencilState <- DepthStencilState.Default

    gameContent.Cubes
    |> List.iter (fun (centre, size) ->

        effect.CurrentTechnique.Passes |> Seq.iter
            (fun pass ->
                pass.Apply()

                effect.Parameters.["xWorld"].SetValue(Matrix.CreateScale(size * 0.5f) * Matrix.CreateTranslation(centre))
                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, gameContent.Vertices, 0, gameContent.Vertices.Length, gameContent.Indices, 0, gameContent.Indices.Length / 3)
            ))

    showParameters gameContent

let drawPicker (device: GraphicsDevice) gameContent (gameTime: GameTime) =
    let time = (single gameTime.TotalGameTime.TotalMilliseconds) / 100.0f

    let effect = gameContent.Effect

    effect.CurrentTechnique <- effect.Techniques.["Picker"]
    effect.Parameters.["xView"].SetValue(Matrix.CreateLookAt(Vector3(-5.0f, 2.0f, 5.0f), Vector3.Zero, Vector3.UnitY))
    effect.Parameters.["xProjection"].SetValue(Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.1f, 100.0f))
    
    device.DepthStencilState <- DepthStencilState.Default

    gameContent.Cubes
    |> List.iteri (fun cubeIndex (centre, size) ->

        effect.CurrentTechnique.Passes |> Seq.iter
            (fun pass ->
                pass.Apply()

                effect.Parameters.["xWorld"].SetValue(Matrix.CreateScale(size * 0.5f) * Matrix.CreateTranslation(centre))
                effect.Parameters.["xCubeIndex"].SetValue((float32 (1 + cubeIndex)) / 10.0f)
                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, gameContent.Vertices, 0, gameContent.Vertices.Length, gameContent.Indices, 0, gameContent.Indices.Length / 3)
            ))

    showParameters gameContent
