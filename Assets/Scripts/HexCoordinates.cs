using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCoordinates
{

    private const float hexRadius = 1f;

    private static float hexWidth = 2 * hexRadius;
    private static float hexHeight = Mathf.Sqrt(3) * hexRadius;

    public int Q { get { return q; } }
    public int R { get { return r; } }
    public int S { get { return -q - r; } }

    [SerializeField] readonly int q;

    [SerializeField] readonly int r;

    public HexCoordinates(int q, int r)
    {
        this.q = q;
        this.r = r;
    }
    public override string ToString()
    {
        return string.Concat("(", Q, ", ", R, ", ", S, ")");
    }

    public Vector3 ToWorldPosition()
    {

        float x = hexWidth * (q + r / 2f);
        float y = hexHeight * r * 0.75f; // 0.75 accounts for the vertical stacking of hexes
        return new Vector3(x, 0, y);
    }
}
