using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexBoard : MonoBehaviour
{
    public static HexBoard Active;

    public Hex HexPrefab;

    public Dictionary<HexCoordinates, Hex> Hexes = new Dictionary<HexCoordinates, Hex>();

    public int BoardRadius; // The radius of the board (in hexes)


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
        Material[] materials = Resources.LoadAll<Material>("Materials");
        if (materials.Length == 0)
        {
            Debug.LogError("No materials found in the Materials directory.");
            return;
        }

        for (int q = -BoardRadius; q <= BoardRadius; q++)
        {
            int r1 = Mathf.Max(-BoardRadius, -q - BoardRadius);
            int r2 = Mathf.Min(BoardRadius, -q + BoardRadius);
            for (int r = r1; r <= r2; r++)
            {
                // Instantiate a Hex
                Hex hex = Instantiate(HexPrefab);
                HexCoordinates coordinates = new HexCoordinates(q, r);
                hex.Initialize(new HexCoordinates(q, r));
                Material randomMaterial = materials[Random.Range(0, materials.Length)];
                hex.GetComponent<Renderer>().material = randomMaterial;
                Hexes.Add(coordinates, hex);
            }
        }
    }

}
