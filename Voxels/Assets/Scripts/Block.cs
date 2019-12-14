using System;
using UnityEngine;

public enum Block : ushort
{
    air,
    nothing,
    stone,
    grass,
    dirt,
    bedrock,
};

public static class BlockExt
{
    public static bool IsTransparent(this Block block) => block == Block.air;
};

public class BlockTexture
{
    public Block type = Block.nothing;
    public UInt16 North;
    public UInt16 East;
    public UInt16 South;
    public UInt16 West;
    public UInt16 Up;
    public UInt16 Down;

    public BlockTexture(Block type, ushort north, ushort east, ushort south, ushort west, ushort up, ushort down)
    {
        this.type = type;
        North = north;
        East = east;
        South = south;
        West = west;
        Up = up;
        Down = down;
    }

    public BlockTexture(Block type, ushort side, ushort up, ushort down)
    {
        this.type = type;
        North = side;
        East = side;
        South = side;
        West = side;
        Up = up;
        Down = down;
    }

    public BlockTexture(Block type, ushort side, ushort head)
    {
        this.type = type;
        North = side;
        East = side;
        South = side;
        West = side;
        Up = head;
        Down = head;
    }

    public BlockTexture(Block type, ushort faces)
    {
        this.type = type;
        North = faces;
        East = faces;
        South = faces;
        West = faces;
        Up = faces;
        Down = faces;
    }

    public UInt16 GetDirection(Direction direction)
    {
        if (direction == Direction.North) return North;
        if (direction == Direction.East)  return East;
        if (direction == Direction.South) return South;
        if (direction == Direction.West)  return West;
        if (direction == Direction.Up)    return Up;
        if (direction == Direction.Down)  return Down;
        return (ushort)0;
    }
};