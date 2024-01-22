using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Active;

    HexBoard hexBoard;
    Slime slime;

    public int CurrentDifficultyLevel;
    public int TurnNumber;
    public int StartingResources = 5;

    public TurnState CurrentState;

    [SerializeField] TextMeshProUGUI txtMoisture;
    [SerializeField] TextMeshProUGUI txtNutrients;
    [SerializeField] TextMeshProUGUI txtOxygen;

    void OnEnable()
    {
        Active = this;
    }

        // Start is called before the first frame update
    void Start()
    {
        hexBoard = GetComponent<HexBoard>();
        slime = GetComponent<Slime>();

        hexBoard.GenerateHexBoard();
        CurrentDifficultyLevel = 0;
        TurnNumber = 0;
        CurrentState = TurnState.Idle;

        slime.MoistureCount = StartingResources;
        slime.NutrientCount = StartingResources;
        slime.OxygenCount = StartingResources;
        UpdateResourceUI();
    }

    public void PlaceSlime(Hex startingTile)
    {
        /*int middleRange = hexBoard.BoardRadius / 5;

        int q = Random.Range(-middleRange, middleRange);
        int r = Random.Range(-middleRange, middleRange);
        Hex startingTile = HexBoard.Active.Hexes[new HexCoordinates(q, r)];*/
        slime.OccupyHex(startingTile);
        UpdateResourceUI();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //slime.OccupyHex(hexBoard.ActiveHex);
        }
    }

    public void UpgradeButtonClick()
    {
        CurrentState = TurnState.ChoosingUpgrade;
    }

    public void SpreadButtonClick()
    {
        CurrentState = TurnState.SpreadingToHex;

        List<Hex> spreadableHexes = new List<Hex>();

        foreach(Hex hex in slime.occupiedSpaces)
        {
            spreadableHexes.AddRange(hex.FindNeighbors().Where(h => !h.IsOccupied).Except(spreadableHexes));
        }

        if(slime.UpgradeStatus[Slime.Upgrades.SendSpores])
        {
            List<Hex> tempList = new List<Hex>();
            foreach (Hex hex in spreadableHexes)
            {
                tempList.AddRange(hex.FindNeighbors().Where(h => !h.IsOccupied).Except(spreadableHexes).Except(tempList));
            }
            spreadableHexes.AddRange(tempList);
        }

        hexBoard.SpreadableHexes = spreadableHexes;

        foreach(Hex hex in spreadableHexes)
        {
            hex.HighlightSpreadable();
        }
    }

    public void ConfirmSpreadClick()
    {
        if (hexBoard.ActiveHex != null)
        {
            slime.OccupyHex(hexBoard.ActiveHex);
            GoBackToIdleState();
            ClearSpreadableDisplay();
            UpdateResourceUI();
        }
    }

    public void ClearSpreadableDisplay()
    {
        foreach (Hex hex in hexBoard.SpreadableHexes)
        {
            hex.UnHighlight();
        }
        hexBoard.SpreadableHexes.Clear();
    }

    public void GoBackToIdleState()
    {
        CurrentState = TurnState.Idle;
    }

    public void EndTurnButtonClick()
    {
        CurrentState = TurnState.EndOfTurn;
        slime.OnTurnEnd();

        CurrentState = TurnState.StartOfTurn;
        slime.OnTurnStart();

        CurrentState = TurnState.Idle;
    }

    public void UpdateResourceUI()
    {
        int moisture = 0;
        int nutrients = 0;
        int oxygen = 0;

        bool hasDrainUpgrade = slime.UpgradeStatus[Slime.Upgrades.ResourceDrainer];

        foreach (Hex hex in slime.occupiedSpaces)
        {
            moisture += hex.AbsorbMoisture(hasDrainUpgrade, true);
            nutrients += hex.AbsorbNutrients(hasDrainUpgrade, true);
            oxygen += hex.AbsorbOxygen(hasDrainUpgrade, true);
        }

        if (slime.UpgradeStatus[Slime.Upgrades.MoistureConserver])
        {
            moisture -= slime.occupiedSpaces.Count / 2;
        }
        else
        {
            moisture -= slime.occupiedSpaces.Count;
        }

        txtMoisture.text = slime.MoistureCount + "(" + (moisture >= 0 ? "+" : "-") + moisture + ")";
        txtNutrients.text = slime.NutrientCount + "(" + (nutrients >= 0 ? "+" : "-") + nutrients + ")";
        txtOxygen.text = slime.OxygenCount + "(" + (oxygen >= 0 ? "+" : "-") + oxygen + ")";
    }

    public enum TurnState
    {
        Idle = 0,
        ChoosingUpgrade = 1,
        SpreadingToHex = 2,
        StartOfTurn = 3,
        EndOfTurn = 4
    }
}
