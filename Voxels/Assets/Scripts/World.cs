using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static SEM;

public class World : MonoBehaviour
{
    public Material material;
    public Material GLMat;
    public string seed = "";
    public int renderDistance = 16;
    public bool drawDebugLines = false;

    Thread worldGenerator;
    Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();
    int wseed;
    public System.Random random;
    List<Vector3Int> pendingDeletions = new List<Vector3Int>();

    private void OnEnable()
    {
        foreach (var item in Resources.FindObjectsOfTypeAll<MeshCollider>())
            item.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        foreach (var item in Resources.FindObjectsOfTypeAll<MeshCollider>())
            item.gameObject.SetActive(false);
    }

    private void Awake()
    {
        int renderDistance = Snap(this.renderDistance, Chunk.size.x / 2) * 8;
        if (seed == "")
            seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue).ToString();
        wseed = GetRawSeed(seed);
        random = new System.Random(wseed);
        Debug.Log("Your Generated Seed: " + GetRawSeed(seed));
        ChunkTextureController.Initialize(Application.streamingAssetsPath + "/firstpass-texture.png", material);
        player = Camera.main.transform;
        worldGenerator = new Thread(new ThreadStart(delegate ()
        {
            while (true)
            {
                try
                {
                    while (pendingDeletions.Count > 0) Thread.Sleep(15);
                    Vector3Int Player = new Vector3Int(Snap(playerPos.x, Chunk.size.x), 0, Snap(playerPos.z, Chunk.size.z));
                    int minX = Player.x - renderDistance;
                    int maxX = Player.x + renderDistance;
                    int minZ = Player.z - renderDistance;
                    int maxZ = Player.z + renderDistance;
                    for (int z = minZ; z < maxZ; z += Chunk.size.z)
                        for (int x = minX; x < maxX; x += Chunk.size.x)
                        {
                            Vector3Int vector = new Vector3Int(x, 0, z);
                            Chunk chunk = null;
                            chunks.TryGetValue(vector, out chunk);
                            if (chunk == null)
                            {
                                Chunk nchunk = new Chunk(vector);
                                nchunk.GenerateBlockArray_Normal(wseed, random);
                                nchunk.GenerateMesh();
                                chunks.Add(vector, nchunk);
                            }
                        }

                    foreach (var chunk in chunks.Values)
                        if (chunk != null)
                            if (chunk.isReady)
                            {
                                Vector3Int vector = chunk.position;
                                if (vector.x > maxX ||
                                vector.x < minX ||
                                vector.z > maxZ ||
                                vector.z < minZ)
                                    pendingDeletions.Add(vector);
                            }
                }
                catch (Exception) { }
                Thread.Sleep(1);
            }
        }));
        worldGenerator.Start();
    }

    Transform player;
    Vector3 playerPos = Vector3.zero;
    void Update()
    {
        playerPos = player.transform.position;
        try
        {
            foreach (var item in pendingDeletions)
            {
                Chunk chunk;
                chunks.TryGetValue(item, out chunk);
                chunk.Destroy();
                chunks.Remove(item);
                pendingDeletions.Remove(item);
            }
        }
        catch (Exception) { goto tryAgain; }
    tryAgain:
        try
        {
            foreach (var chunk in chunks.Values)
                chunk.Render(material);
        }
        catch (Exception) { goto tryAgain; }
    }

    private void OnPostRender()
    {
        if (PlayerController.mesh == null || !drawDebugLines)
            return;

        GL.Begin(GL.LINE_STRIP);
        GLMat.SetPass(0);
        GL.Color(Color.black);
        foreach (var vertex in PlayerController.mesh.vertices)
            GL.Vertex(new Vector3(vertex.x, vertex.y + .001f, vertex.z));
        GL.End();
        GL.Begin(GL.LINE_STRIP);
        GLMat.SetPass(0);
        GL.Color(new Color(0, 1, 0, .05f));
        foreach (var vertex in PlayerController.mesh.vertices)
        {
            GL.Vertex(new Vector3(vertex.x, 0, vertex.z));
            GL.Vertex(new Vector3(vertex.x, 256, vertex.z));
            GL.Vertex(new Vector3(vertex.x, 0, vertex.z));
        }
        GL.End();
    }
}
