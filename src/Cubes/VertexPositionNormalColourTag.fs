module VertexPositionNormalColourTag

open System

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

[<StructAttribute>]
type VertexPositionNormalColourTag(position: Vector3, normal: Vector3, colour: Color, tag: float32) =
    member _this.Position = position
    member _this.Normal = normal
    member _this.Colour = colour
    member _this.Tag = tag
    static member VertexDeclaration =
        new VertexDeclaration(
            VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            VertexElement(24, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            VertexElement(28, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 0)
        );
    interface IVertexType with
        member _this.VertexDeclaration = VertexPositionNormalColourTag.VertexDeclaration
