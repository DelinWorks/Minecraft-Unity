using System;
using UnityEngine;

public class Chunk
{
    public static Vector3Int size = new Vector3Int(16, 256, 16);
    public Vector3Int position;
    public Mesh mesh;
    public GameObject MeshCollider;
    Block[] blocks;
    public bool isReady = false;

    public Chunk (Vector3Int pos)
    {
        position = pos;
    }

    bool isColliderBaked = false;
    public void BakeCollider()
    {
        if (isColliderBaked)
            return;
        isColliderBaked = true;

        MeshCollider = new GameObject($"Chunk (x:{position.x}, z:{position.z}) Collider");
        MeshCollider.transform.position = new Vector3(0, 0, 0);
        MeshCollider.AddComponent<MeshCollider>();
        MeshCollider.GetComponent<MeshCollider>().sharedMesh = mesh;
        MeshCollider.GetComponent<MeshCollider>().tag = "ChunkCollider";
        MeshCollider.hideFlags = HideFlags.HideInHierarchy;
    }

    public Block GetSafe(int index, Block excludeType = Block.air)
    {
        if (index <= blocks.Length)
            return blocks[index];
        else
            return excludeType;
    }

    public void GenerateBlockArray_Normal(int seed, System.Random noNoisedSeed)
    {
        if (isGenerating)
            return;
        isGenerating = true;
        int RandomNoise = seed;
        blocks = new Block[size.x * size.y * size.z];
        int index = 0;
        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
                for (int z = 0; z < size.z; z++)
                {
                    int i = Mathf.CeilToInt((Mathf.PerlinNoise(UInt16.MaxValue + (x + position.x + RandomNoise) / 32f, UInt16.MaxValue + (z + position.z + RandomNoise) / 32f) * 10f) + 64f);
                    int rand = noNoisedSeed.Next(8, 11);
                    if (i == y)
                        blocks[index] = Block.grass;
                    if (y <= i - rand)
                        blocks[index] = Block.stone;
                    if (y < i && y > i - rand)
                        blocks[index] = Block.dirt;

                    /*bedrock*/ {
                        if (y < 1 || y < (noNoisedSeed.Next(0, 3) == 0 ? noNoisedSeed.Next(0, 8) : noNoisedSeed.Next(0, 4)))
                            blocks[index] = Block.bedrock;
                    }

                    index++;
                }
    }

    public bool isGenerating = false;
    MeshBuilder meshBuilder;
    public void GenerateMesh()
    {
        meshBuilder = new MeshBuilder(position, blocks);
        meshBuilder.Execute();
        isReady = true;
    }

    public void Render(Material material)
    {
        if (!isReady)
            return;

        if (mesh == null)
        {
            mesh = meshBuilder.GetMesh(ref mesh);
            BakeCollider();
        }
        else
            Graphics.DrawMesh(mesh, Matrix4x4.identity, material, 0);
    }

    public void Destroy()
    {
        try
        {
            mesh.Clear();
            mesh = null;
            blocks = null;
            isReady = false;
            isGenerating = false;
            MeshCollider.GetComponent<MeshCollider>().sharedMesh.Clear();
            UnityEngine.Object.Destroy(MeshCollider);
        }
        catch (Exception) { }
    }
}
