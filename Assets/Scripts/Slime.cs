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
    [SerializeField] GameObject slimeBridge;

    bool usedHiddenReserves = false;


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

    public void OccupyHex(Hex hex, Hex hexFrom)
    {
        occupiedSpaces.Add(hex);
        Instantiate(slimeModel, new Vector3(hex.transform.position.x, hex.transform.position.y + .5f, hex.transform.position.z), new Quaternion(0f, 0f, 0f, 0f));
        hex.Occupy();

        if(hexFrom != null)
        {
            Quaternion rot = new Quaternion(0f, 0f, 0f, 0f);
            GameObject bridge = Instantiate(slimeBridge, new Vector3(hexFrom.transform.position.x, hexFrom.transform.position.y + .5f, hexFrom.transform.position.z), rot);
            bridge.transform.Rotate(0f, HexCoordinates.GetRotationToHex(hexFrom, hex), 0f);
        }

        HexBoard.Active.MakeVisibleWihinRange(hex, VisibilityRadius);
    }

    public void OnTurnStart()
    {
        if (MoistureCount < occupiedSpaces.Count)
        {
            if (UpgradeStatus[Upgrades.HiddenReserves] && !usedHiddenReserves)
            {
                usedHiddenReserves = true;
            }
            else
            {
                //Game over
            }
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
            hex.UpdateUI();
        }
    }

    public enum Upgrades
    {
        SendSpores = 0, 
        ExtraHexSpore = 1, //When the slime spreads to a new hex, a random adjacent hex also gets a slime on it
        HiddenReserves = 2, 
        GoalFinder = 3, 
        ResourceDrainer = 4, 
        MoistureConserver = 5,
        DiscountSpreading = 6 
    }
}