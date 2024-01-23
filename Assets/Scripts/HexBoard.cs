using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static System.Math;

[RequireComponent(typeof(GameManager))]
public class HexBoard : MonoBehaviour
{
    public static HexBoard Active;

    public Hex ActiveHex { get; private set; }

    public Dictionary<HexCoordinates, Hex> Hexes = new Dictionary<HexCoordinates, Hex>();

    public int BoardRadius; // The radius of the board (in hexes)

    [SerializeField] GameManager gameManager;

    [SerializeField] Hex HexPrefab;
    [SerializeField] int numberOfGoals;

    public List<Hex> SpreadableHexes { get; set; }


    /// <summary>
    /// Sets active board, on initialization or after script recompilation. 
    /// </summary>
    void OnEnable()
    {
        Active = this;
        gameManager = GetComponent<GameManager>();
    }

    public void GenerateHexBoard()
    {
        Material[] materials = Resources.LoadAll<Material>("HexMaterials");
        if (materials.Length == 0)
        {
            Debug.LogError("No materials found in the HexMaterials directory.");
            return;
        }

        int middleRange = BoardRadius / 5;

        int startingQ = Random.Range(-middleRange, middleRange);
        int startingR = Random.Range(-middleRange, middleRange);
        Hex startingTile = null;

        for (int q = -BoardRadius; q <= BoardRadius; q++)
        {
            int r1 = Mathf.Max(-BoardRadius, -q - BoardRadius);
            int r2 = Mathf.Min(BoardRadius, -q + BoardRadius);
            for (int r = r1; r <= r2; r++)
            {
                Hex hex = Instantiate(HexPrefab);
                HexCoordinates coordinates = new HexCoordinates(q, r);
                Biome biome = Biome.BIOMES.GetRandom();
                hex.Initialize(coordinates, biome, elevation: 0);
                Hexes.Add(coordinates, hex);

                if (q == startingQ && r == startingR)
                {
                    startingTile = hex;
                }
            }
        }
        SetGoalHexes();
        gameManager.PlaceSlime(startingTile);
        CenterCamera(startingTile);

    }

    void SetGoalHexes()
    {
        IEnumerable<HexCoordinates> boundaryCoords = Hexes.Keys.Where(key =>
        {
            int distance = (Abs(key.Q) + Abs(key.R) + Abs(key.Q + key.R)) / 2;
            return distance == BoardRadius;
        });
        foreach (HexCoordinates goalCoord in boundaryCoords.OrderBy(x => Random.value).Take(numberOfGoals))
        {
            Hexes[goalCoord].SetAsGoal();
        }
    }

    public void MakeVisibleWihinRange(Hex startingHex, int range)
    {
        foreach (Hex hex in FindHexesWithinRange(startingHex, range))
        {
            hex.RevealFogOfWar();
        }
    }

    public List<Hex> FindHexesWithinRange(Hex startingHex, int range)
    {
        HexCoordinates startingCoords = startingHex.Coordinates;
        List<Hex> results = new List<Hex>();
        for (int dq = -range; dq <= range; dq++)
        {
            for (int dr = -range; dr <= range; dr++)
            {
                int ds = -dq - dr;
                if (Mathf.Abs(ds) <= range)
                {
                    HexCoordinates coordinates = new HexCoordinates(startingCoords.Q + dq, startingCoords.R + dr);
                    Hex hex = Hexes.GetValueOrDefault(coordinates);
                    if (hex != null)
                    {
                        results.Add(hex);
                    }
                }
            }
        }
        return results;
    }


    void CenterCamera(Hex hex)
    {
        Transform cameraTransform = Camera.main.transform;
        Vector3 newPosition = hex.transform.position;
        newPosition.y = cameraTransform.position.y;
        newPosition.z -= 5; // Move camera back so starting hex is centered.
        cameraTransform.position = newPosition;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { // Left Mouse Button
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                // Check if hitObject is a hex tile, and if so, highlight it
                if (gameManager.CurrentState == GameManager.TurnState.SpreadingToHex)
                {
                    SelectHex(hitObject);
                }
            }
            else
            {
                Debug.LogFormat("Hit nothin!");
            }
        }
    }
    void SelectHex(GameObject gameObject)
    {
        if (gameObject != null)
        {
            Hex clickedHex = gameObject.GetComponent<Hex>();

            if (clickedHex != null && SpreadableHexes.Contains(clickedHex))
            {
                if (ActiveHex != null)
                {
                    ActiveHex.UnHighlight();
                }
                clickedHex.Highlight();
                ActiveHex = clickedHex;
            }
        }
    }
}
