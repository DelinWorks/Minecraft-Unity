using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ChunkTextureController
{
    public static List<BlockTexture> blockTextures = new List<BlockTexture>()
    {
        new BlockTexture(Block.stone,4),
        new BlockTexture(Block.grass,1,0,2),
        new BlockTexture(Block.dirt,2),
        new BlockTexture(Block.bedrock,3),
    };

    public static UInt16 GetBlockTexture(Block type, Direction direction)
    {
        foreach (var block in blockTextures)
        {
            if (block.type == type)
                return block.GetDirection(direction);
        }
        return (ushort)0;
    }

    public static List<Vector2[]> textureMap = new List<Vector2[]>();

    const int DEFAULT_TEXTURE = 0;

    public static void Initialize(string texturePath, Material material, int textureTileSize = 16)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        List<Vector2> spriteMaps = new List<Vector2>();
        Texture2D texture = new Texture2D(0, 0);
        texture.LoadImage(File.ReadAllBytes(texturePath));
        texture.filterMode = FilterMode.Point;
        material.SetTexture("_MainTex", texture);
        for (int y = texture.height - textureTileSize; y > 0; y -= textureTileSize)
            for (int x = 0; x < texture.width; x += textureTileSize)
                spriteMaps.Add(new Vector2(x, y));
        int index = 0;
        foreach (var s in spriteMaps)
        {
            Vector2[] uvmap = new Vector2[4];
            uvmap[0] = new Vector2(s.x / texture.width, s.y / texture.height);
            uvmap[1] = new Vector2((s.x + textureTileSize) / texture.width, s.y / texture.height);
            uvmap[2] = new Vector2(s.x / texture.width, (s.y + textureTileSize) / texture.height);
            uvmap[3] = new Vector2((s.x + textureTileSize) / texture.width, (s.y + textureTileSize) / texture.height);
            textureMap.Add(uvmap);
            index++;
        }
        sw.Stop();
        Debug.Log(sw.ElapsedMilliseconds + "ms");
    }

    public static bool AddTexture(Block block, Direction direction, int index, Vector2[] uvmap)
    {
        int key = GetKey(block, direction);
        Vector2[] texture = textureMap[key] == null ? null : textureMap[key];
        if (texture != null)
        {
            uvmap[index] = texture[0];
            uvmap[index + 1] = texture[1];
            uvmap[index + 2] = texture[2];
            uvmap[index + 3] = texture[3];
            return true;
        }
        else
        {
            texture = textureMap[DEFAULT_TEXTURE];
            uvmap[index] = texture[0];
            uvmap[index + 1] = texture[1];
            uvmap[index + 2] = texture[2];
            uvmap[index + 3] = texture[3];
            return false;
        }
    }

    static int GetKey(Block block, Direction direction) => GetBlockTexture(block, direction);
}
