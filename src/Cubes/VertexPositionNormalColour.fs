module VertexPositionNormalColour

open System

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

[<StructAttribute>]
type VertexPositionNormalColour(position: Vector3, normal: Vector3, colour: Color) =
    member _this.Position = position
    member _this.Normal = normal
    member _this.Colour = colour
    static member VertexDeclaration =
        new VertexDeclaration(
            VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            VertexElement(24, VertexElementFormat.Color, VertexElementUsage.Color, 0)
        );
    interface IVertexType with
        member _this.VertexDeclaration = VertexPositionNormalColour.VertexDeclaration
