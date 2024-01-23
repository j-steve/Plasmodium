using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Slime;

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

    [SerializeField] TextMeshProUGUI txtSpreadMoistureCost;
    [SerializeField] TextMeshProUGUI txtSpreadNutrientsCost;
    [SerializeField] TextMeshProUGUI txtSpreadOxygenCost;

    [SerializeField] Button btnConfirmSpread;

    [SerializeField] List<Upgrade> SlimeUpgrades;

    public int SpreadMoistureCost;
    public int SpreadNutrientsCost;
    public int SpreadOxygenCost;

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
        slime.OccupyHex(startingTile, null);
        UpdateResourceUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpgradeButtonClick()
    {
        CurrentState = TurnState.ChoosingUpgrade;
    }

    public void SpreadButtonClick()
    {
        CurrentState = TurnState.SpreadingToHex;

        List<Hex> spreadableHexes = new List<Hex>();

        foreach (Hex hex in slime.occupiedSpaces)
        {
            spreadableHexes.AddRange(hex.FindNeighbors().Where(h => !h.IsOccupied).Except(spreadableHexes));
        }

        if (slime.UpgradeStatus[Slime.Upgrades.SendSpores])
        {
            List<Hex> tempList = new List<Hex>();
            foreach (Hex hex in spreadableHexes)
            {
                tempList.AddRange(hex.FindNeighbors().Where(h => !h.IsOccupied).Except(spreadableHexes).Except(tempList));
            }
            spreadableHexes.AddRange(tempList);
        }

        hexBoard.SpreadableHexes = spreadableHexes;

        foreach (Hex hex in spreadableHexes)
        {
            hex.HighlightSpreadable();
        }

        if (slime.MoistureCount >= SpreadMoistureCost && slime.NutrientCount >= SpreadNutrientsCost && slime.OxygenCount >= SpreadOxygenCost)
        {
            btnConfirmSpread.interactable = true;
        }
        else
        {
            btnConfirmSpread.interactable = false;
        }
    }

    public void ConfirmSpreadClick()
    {
        if (hexBoard.ActiveHex != null)
        {
            Hex hexBridgeFrom = null;
            for(int i = slime.occupiedSpaces.Count -1; i >= 0; i--)
            {
                if(slime.occupiedSpaces[i].FindNeighbors().Contains(hexBoard.ActiveHex))
                {
                    hexBridgeFrom = slime.occupiedSpaces[i];
                    break;
                }
            }
            slime.OccupyHex(hexBoard.ActiveHex, hexBridgeFrom);

            if (slime.UpgradeStatus[Upgrades.ExtraHexSpore])
            {
                List<Hex> neightbors = hexBoard.ActiveHex.FindNeighbors().Where(h => !h.IsOccupied).ToList();

                if (neightbors.Count > 0)
                {
                    slime.OccupyHex(neightbors[Random.Range(0, neightbors.Count)], hexBoard.ActiveHex);
                }
            }

            bool hasSpreadCostUpgrade = slime.UpgradeStatus[Slime.Upgrades.DiscountSpreading];
            slime.MoistureCount -= (hasSpreadCostUpgrade ? ((int)(SpreadMoistureCost / 2)) : SpreadMoistureCost);
            slime.NutrientCount -= (hasSpreadCostUpgrade ? ((int)(SpreadNutrientsCost / 2)) : SpreadNutrientsCost);
            slime.OxygenCount -= (hasSpreadCostUpgrade ? ((int)(SpreadOxygenCost / 2)) : SpreadOxygenCost);

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

        TurnNumber++;

        UpdateResourceUI();

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

        bool hasSpreadCostUpgrade = slime.UpgradeStatus[Slime.Upgrades.DiscountSpreading];

        txtSpreadMoistureCost.text = "Moisture Cost: " + (hasSpreadCostUpgrade ? ((int)(SpreadMoistureCost / 2)) : SpreadMoistureCost);
        txtSpreadNutrientsCost.text = "Nutrients Cost: " + (hasSpreadCostUpgrade ? ((int)(SpreadNutrientsCost / 2)) : SpreadNutrientsCost);
        txtSpreadOxygenCost.text = "Oxygen Cost: " + (hasSpreadCostUpgrade ? ((int)(SpreadOxygenCost / 2)) : SpreadOxygenCost);
    }

    public void CheckUpgradeCosts()
    {
        foreach(Upgrade upgrade in SlimeUpgrades)
        {
            upgrade.UpdateUnlockability(slime.MoistureCount >= upgrade.MoistureCost && slime.NutrientCount >= upgrade.NutrientsCost && slime.OxygenCount >= upgrade.OxygenCost);
        }
    }

    public void UnlockUpgrade(Upgrades upgrade, int moisture, int nutrients, int oxygen)
    {
        slime.UpgradeStatus[upgrade] = true;

        slime.MoistureCount -= moisture;
        slime.NutrientCount -= nutrients;
        slime.OxygenCount -= oxygen;

        UpdateResourceUI();

    }

    public void RevealGoals()
    {
        foreach(Hex hex in hexBoard.Hexes.Values.Where(h=>h.IsGoal))
        {
            hex.RevealFogOfWar();
        }
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
