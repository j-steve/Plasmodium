using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexBoard : MonoBehaviour
{
    public static HexBoard Active;

    public Hex HexPrefab;

    public Dictionary<Vector3Int, Hex> Hexes = new Dictionary<Vector3Int, Hex>();

    public int BoardRadius; // The radius of the board (in hexes)

    private float hexWidth; // Calculated width of a hex tile
    private float hexHeight; // Calculated height of a hex tile
    private const float hexRadius = 1f; // Radius of a hex tile


    /// <summary>
    /// Sets active board, on initialization or after script recompilation. 
    /// </summary>
    void OnEnable()
    {
        Active = this;
    }

    void Start()
    {
        GenerateHexBoard();
    }

    void GenerateHexBoard()
    {

        hexWidth = 2 * hexRadius;
        hexHeight = Mathf.Sqrt(3) * hexRadius;

        for (int q = -BoardRadius; q <= BoardRadius; q++)
        {
            int r1 = Mathf.Max(-BoardRadius, -q - BoardRadius);
            int r2 = Mathf.Min(BoardRadius, -q + BoardRadius);
            for (int r = r1; r <= r2; r++)
            {
                // Instantiate a Hex
                Hex hex = Instantiate(HexPrefab);
                // Calculate the position for this Hex
                Vector2 position = HexToPosition(q, r);
                hex.transform.position = new Vector3(position.x, position.y, 0);
                hex.name = "Hex_" + q + "_" + r;
            }
        }
    }

    Vector2 HexToPosition(int q, int r)
    {
        float x = hexWidth * (q + r / 2f);
        float y = hexHeight * r * 0.75f; // 0.75 accounts for the vertical stacking of hexes
        return new Vector2(x, y);
    }
}
