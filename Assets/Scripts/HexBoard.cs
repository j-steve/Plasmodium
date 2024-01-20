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
        Material[] materials = Resources.LoadAll<Material>("HexMaterials");
        if (materials.Length == 0)
        {
            Debug.LogError("No materials found in the HexMaterials directory.");
            return;
        }

        for (int q = -BoardRadius; q <= BoardRadius; q++)
        {
            int r1 = Mathf.Max(-BoardRadius, -q - BoardRadius);
            int r2 = Mathf.Min(BoardRadius, -q + BoardRadius);
            for (int r = r1; r <= r2; r++)
            {
                Hex hex = Instantiate(HexPrefab);
                HexCoordinates coordinates = new HexCoordinates(q, r);
                Biome biome = Biome.BIOMES[Random.Range(0, Biome.BIOMES.Count)];
                hex.Initialize(coordinates, biome, elevation: 0);
                Hexes.Add(coordinates, hex);
            }
        }
    }

}
