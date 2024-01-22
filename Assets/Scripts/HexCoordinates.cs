using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

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

    public HexCoordinates GetAdjacent(HexDirection direction)
    {
        switch (direction)
        {
            case HexDirection.NW:
                return new HexCoordinates(Q, R - 1);
            case HexDirection.NE:
                return new HexCoordinates(Q + 1, R - 1);
            case HexDirection.E:
                return new HexCoordinates(Q + 1, R);
            case HexDirection.SE:
                return new HexCoordinates(Q, R + 1);
            case HexDirection.SW:
                return new HexCoordinates(Q - 1, R + 1);
            case HexDirection.W:
                return new HexCoordinates(Q - 1, R);
            default:
                throw new ArgumentOutOfRangeException("Unknown HexDirection.");
        }

    }

    public override bool Equals(object obj)
    {
        // Check for null and compare run-time types.
        if (obj == null || !GetType().Equals(obj.GetType()))
        {
            return false;
        }

        HexCoordinates other = (HexCoordinates)obj;
        return (q == other.q) && (r == other.r);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(q, r);
    }

}
