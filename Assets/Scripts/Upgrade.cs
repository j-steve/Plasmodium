using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Slime;

public class Upgrade : MonoBehaviour
{
    [SerializeField] string UpgradeName;
    [SerializeField] string UpgradeDescription;
    [SerializeField] Upgrades UpgradeEnum;
    public int MoistureCost;
    public int OxygenCost;
    public int NutrientsCost;

    [SerializeField] TextMeshProUGUI txtUpgradeName;
    [SerializeField] TextMeshProUGUI txtUpgradeDescription;
    [SerializeField] TextMeshProUGUI txtMoistureCost;
    [SerializeField] TextMeshProUGUI txtNutrientsCost;
    [SerializeField] TextMeshProUGUI txtOxygenCost;
    [SerializeField] TextMeshProUGUI txtButtonText;
    [SerializeField] GameObject Panel;
    [SerializeField] Button btnUnlock;

    GameManager GameManager;

    public bool IsUnlocked;

    // Start is called before the first frame update
    void Start()
    {
        IsUnlocked = false;

        txtUpgradeName.text = UpgradeName;
        txtUpgradeDescription.text = UpgradeDescription;
        txtMoistureCost.text = "Moisture Cost: " + MoistureCost;
        txtNutrientsCost.text = "Nutrients Cost: " + NutrientsCost;
        txtOxygenCost.text = "Oxygen Cost: " + OxygenCost;

        GameManager = FindObjectsOfType<GameManager>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Unlock()
    {
        btnUnlock.enabled = false;
        txtButtonText.text = "Unlocked";
        IsUnlocked = true;
        GameManager.UnlockUpgrade(UpgradeEnum, MoistureCost, NutrientsCost, OxygenCost);
        GameManager.CheckUpgradeCosts();
    }

    public void UpdateUnlockability(bool canBeUnlocked)
    {
        if(canBeUnlocked)
        {
            btnUnlock.enabled = true;
        }
        else
        {
            btnUnlock.enabled = false;
        }
    }
}
