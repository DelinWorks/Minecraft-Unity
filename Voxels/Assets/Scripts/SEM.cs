using UnityEngine;

static class SEM
{
    public static int GetRawSeed(string seed)
    {
        int nseed = 0;
        for (int i = 0; i < seed.Length; i++)
            nseed += seed[i] * 31 ^ i;
        return nseed;
    }

    public static int Snap(float i, int v) => (int)(Mathf.Round(i / v) * v);
}
