module Text

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type Content =
    {
        SpriteBatch: SpriteBatch
        SpriteFont: SpriteFont
    }

let loadContent (_this: Game) device =
    {
        SpriteFont = _this.Content.Load<SpriteFont>("Fonts/Arial")
        SpriteBatch = new SpriteBatch(device)
    }

let draw (device: GraphicsDevice) content fps =
    let colour = Color.DarkSlateGray

    let s = sprintf "F# MonoGame, FPS: %.0f" fps

    content.SpriteBatch.Begin()
    content.SpriteBatch.DrawString(content.SpriteFont, s, Vector2(10.0f, 10.0f), colour)
    content.SpriteBatch.End()
