using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{

    public int StartingOxygen { get; set; }
    public int StartingNutrients { get; set; }
    public int StartingMoisture { get; set; }

    public int CurrentOxygen { get; set; }
    public int CurrentNutrients { get; set; }
    public int CurrentMoisture { get; set; }


    private HexCoordinates coordinates;

    /// <summary>
    /// Constructs the hex on initial instantiation.
    /// </summary>
    public void Initialize(HexCoordinates coordinates)
    {
        // Calculate the position for this Hex
        this.coordinates = coordinates;
        transform.position = coordinates.ToWorldPosition();
        name = "Hex " + coordinates.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Randomly generate resources based on the tile type. eg log has 3-5 nutrients, 2-4 moisture, 1-3 oxygen

        CurrentMoisture = StartingMoisture;
        CurrentNutrients = StartingNutrients;
        CurrentOxygen = StartingOxygen;
    }

    public bool AbsorbOxygen()
    {
        if (CurrentOxygen > 0)
        {
            CurrentOxygen--;
            return true;
        }

        return false;
    }

    public bool AbsorbNutrients()
    {
        if (CurrentNutrients > 0)
        {
            CurrentNutrients--;
            return true;
        }

        return false;
    }

    public bool AbsorbMoisture()
    {
        if (CurrentMoisture > 0)
        {
            CurrentMoisture--;
            return true;
        }

        return false;
    }

}
