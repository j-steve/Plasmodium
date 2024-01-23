using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public int OxygenCount { get; set; }
    public int NutrientCount { get; set; }
    public int MoistureCount { get; set; }


    public int VisibilityRadius = 2;

    public Dictionary<Upgrades, bool> UpgradeStatus;

    public List<Hex> occupiedSpaces { get; private set; }

    [SerializeField] GameObject slimeModel;


    // Start is called before the first frame update
    void Start()
    {
        occupiedSpaces = new List<Hex>();
        UpgradeStatus = new Dictionary<Upgrades, bool>();

        foreach (Upgrades upgrade in Enum.GetValues(typeof(Upgrades)))
        {
            UpgradeStatus.Add(upgrade, false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OccupyHex(Hex hex)
    {
        occupiedSpaces.Add(hex);
        Instantiate(slimeModel, new Vector3(hex.transform.position.x, hex.transform.position.y + .5f, hex.transform.position.z), new Quaternion(0f, 0f, 0f, 0f));
        hex.Occupy();

        if (UpgradeStatus[Upgrades.ExtraHexSpore])
        {
            //spawn additional slime
        }

        HexBoard.Active.MakeVisibleWihinRange(hex, VisibilityRadius);

    }

    public void OnTurnStart()
    {
        if (MoistureCount < occupiedSpaces.Count)
        {
            if (UpgradeStatus[Upgrades.HiddenReserves])
            {
                //Don't die, get 1 more turn
            }
            //Slime dies
        }
        else
        {
            if (UpgradeStatus[Upgrades.MoistureConserver])
            {
                MoistureCount -= occupiedSpaces.Count / 2;
            }
            else
            {
                MoistureCount -= occupiedSpaces.Count;
            }
        }
    }

    public void OnTurnEnd()
    {
        bool hasDrainUpgrade = UpgradeStatus[Upgrades.ResourceDrainer];
        foreach (Hex hex in occupiedSpaces)
        {
            OxygenCount += hex.AbsorbOxygen(hasDrainUpgrade, false);
            NutrientCount += hex.AbsorbNutrients(hasDrainUpgrade, false);
            MoistureCount += hex.AbsorbMoisture(hasDrainUpgrade, false);
        }
    }

    public enum Upgrades
    {
        SendSpores = 0, //Allows the slime to be able to spread further than just adjacent hexes
        ExtraHexSpore = 1, //When the slime spreads to a new hex, a random adjacent hex also gets a slime on it
        HiddenReserves = 2, //When the slime would die from now moisture, it gets 1 more turn
        GoalFinder = 3, //Reveals the goal hexes
        ResourceDrainer = 4, //Drains 2 of each resource on each hex
        MoistureConserver = 5, //The slime needs half the amount of moisture at the start of each turn
        DiscountSpreading = 6 //The cost to spread is halved
    }
}