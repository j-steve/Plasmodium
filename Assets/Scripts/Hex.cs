using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;


[RequireComponent(typeof(Renderer))]

public class Hex : MonoBehaviour
{

    [SerializeField] Canvas uiCanvas;
    [SerializeField] TextMeshProUGUI oxygenLabel;
    [SerializeField] TextMeshProUGUI nutrientsLabel;
    [SerializeField] TextMeshProUGUI moistureLabel;
    [SerializeField] SpriteRenderer hexOutline;
    [SerializeField] SpriteRenderer goalOutline;

    [SerializeField] Color hexOutlineColorDefault;
    [SerializeField] Color hexOutlineColorHighlighted;
    [SerializeField] Color hexOutlineColorOccupied;
    [SerializeField] Color hexOutlineColorSpreadOption;

    public Biome Biome { get; private set; }

    public int StartingOxygen { get; set; }
    public int StartingNutrients { get; set; }
    public int StartingMoisture { get; set; }

    public int CurrentOxygen { get; set; }
    public int CurrentNutrients { get; set; }
    public int CurrentMoisture { get; set; }

    public bool IsGoal { get; private set; }
    public bool IsRevealed { get; private set; }
    public bool IsOccupied { get; private set; }

    public string UniqueID { get; set; }

	public HexCoordinates coordinates { get; private set; }

    /// <summary>
    /// Constructs the hex on initial instantiation.
    /// </summary>
    public void Initialize(HexCoordinates coordinates, Biome biome, int elevation)
    {
        // Calculate the position for this Hex
        this.Coordinates = coordinates;
        Vector3 position = coordinates.ToWorldPosition();
        position.y = elevation;
        transform.position = position;
        // Set biome attributes
        Biome = biome;
        StartingOxygen = UnityEngine.Random.Range(biome.Oxygen - 1, biome.Oxygen + 1);
        StartingNutrients = UnityEngine.Random.Range(biome.Nutrients - 1, biome.Nutrients + 1);
        StartingMoisture = UnityEngine.Random.Range(biome.Moisture - 1, biome.Moisture + 1);
        oxygenLabel.text = StartingOxygen.ToString();
        nutrientsLabel.text = StartingNutrients.ToString();
        moistureLabel.text = StartingMoisture.ToString();

        CurrentMoisture = StartingMoisture;
        CurrentNutrients = StartingNutrients;
        CurrentOxygen = StartingOxygen;

        goalOutline.enabled = false;

        IsOccupied = false;

        name = biome.Name + " " + coordinates.ToString();
        UniqueID = name;
        UnHighlight();
        HideFogOfWar();
    }

    public void HighlightSpreadable()
    {
        hexOutline.color = hexOutlineColorSpreadOption;
        hexOutline.sortingOrder = 3; // Prioritize this cell border so it's shown on top.
    }

    public void Highlight()
    {
        hexOutline.color = hexOutlineColorHighlighted;
        hexOutline.sortingOrder = 5; // Prioritize this cell border so it's shown on top.

    }
    public void UnHighlight()
    {
        if (IsOccupied)
        {
            hexOutline.color = hexOutlineColorOccupied;
            hexOutline.sortingOrder = 4;

        }
        else if (!(GameManager.Active.CurrentState == GameManager.TurnState.SpreadingToHex))
        {
            hexOutline.color = hexOutlineColorDefault;
            hexOutline.sortingOrder = 2; // Revert to normal cell border priority.
        }
        else
        {
            hexOutline.color = hexOutlineColorSpreadOption;
            hexOutline.sortingOrder = 3; // Prioritize this cell border so it's shown on top.
        }
    }

    public void SetAsGoal()
    {
        IsGoal = true;
        goalOutline.enabled = true;
    }

    public void HideFogOfWar()
    {
        uiCanvas.gameObject.SetActive(false);
        IsRevealed = false;
    }

    public void RevealFogOfWar()
    {
        uiCanvas.gameObject.SetActive(true);
        GetComponent<Renderer>().material = Biome.Material;
        IsRevealed = true;
    }

    public List<Hex> FindNeighbors()
    {
        return Utils.GetEnumValues<HexDirection>().Select(
            dir => HexBoard.Active.Hexes.GetValueOrDefault(
                Coordinates.GetAdjacent(dir))).ToList();
    }

    public void Occupy()
    {
        IsOccupied = true;
        hexOutline.color = hexOutlineColorOccupied;
        hexOutline.sortingOrder = 4;
    }

    public int AbsorbOxygen(bool hasDrainUpgrade, bool predictionMode)
    {
        if (hasDrainUpgrade && CurrentOxygen > 1)
        {
            if (!predictionMode)
                CurrentOxygen -= 2;
            return 2;
        }
        else if (CurrentOxygen > 0)
        {
            if (!predictionMode)
                CurrentOxygen--;
            return 1;
        }

        return 0;
    }

    public int AbsorbNutrients(bool hasDrainUpgrade, bool predictionMode)
    {
        if (hasDrainUpgrade && CurrentNutrients > 1)
        {
            if (!predictionMode)
                CurrentNutrients -= 2;
            return 2;
        }
        else if (CurrentNutrients > 0)
        {
            if (!predictionMode)
                CurrentNutrients--;
            return 1;
        }

        return 0;
    }

    public int AbsorbMoisture(bool hasDrainUpgrade, bool predictionMode)
    {
        if (hasDrainUpgrade && CurrentMoisture > 1)
        {
            if (!predictionMode)
                CurrentMoisture -= 2;
            return 2;
        }
        else if (CurrentMoisture > 0)
        {
            if (!predictionMode)
                CurrentMoisture--;
            return 1;
        }

        return 0;
    }

}
