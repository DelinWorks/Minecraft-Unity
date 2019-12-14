using UnityEngine;

public struct Vector3Int
{
    public int x, y, z;

    public Vector3Int(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static Vector3 Vector3(Vector3Int vector) => new Vector3(vector.x, vector.y, vector.z);

    public static bool Compare(Vector3Int Left, Vector3Int Right)
    {
        if (Left.x == Right.x && Left.z == Right.z && Left.z == Right.z)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Warning: returns Vector3(x, 0, z)
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Vector3 Vector2(Vector3Int vector) => new Vector3(vector.x, 0, vector.z);

    public static int Total(Vector3Int vector) => vector.x * vector.y * vector.z;

    public static Vector3Int Zero() => new Vector3Int(0, 0, 0);

    public static Vector3Int One() => new Vector3Int(1, 1, 1);
}
