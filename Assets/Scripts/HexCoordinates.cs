using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCoordinates
{

    private static float hexWidth = 1.73f;
    private static float hexHeight = 2;

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
