module Diagnostics

open Microsoft.Xna.Framework

type Fps() as _this =
    let mutable frames = 0
    let mutable lastFrames = 0
    let mutable lastMilliseconds = 0.0
    let mutable fps = 0.0

    member _this.Update (gameTime: GameTime) =
        frames <- frames + 1
        let elapsed = gameTime.TotalGameTime.TotalMilliseconds - lastMilliseconds

        if elapsed > 1000.0 then
            fps <- float (frames - lastFrames) * 1000.0 / elapsed
            lastMilliseconds <- gameTime.TotalGameTime.TotalMilliseconds
            lastFrames <- frames
    
    member _this.Fps = fps
