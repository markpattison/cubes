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

let draw (device: GraphicsDevice) content =
    let colour = Color.DarkSlateGray

    content.SpriteBatch.Begin()
    content.SpriteBatch.DrawString(content.SpriteFont, "monogame-fsharp", Vector2(10.0f, 10.0f), colour)
    content.SpriteBatch.End()
