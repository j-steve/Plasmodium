using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public int OxygenCount { get; set; }
    public int NutrientCount { get; set; }
    public int MoistureCount { get; set; }

    List<Hex> occupiedSpaces;

    [SerializeField] GameObject slimeModel;


    // Start is called before the first frame update
    void Start()
    {
        occupiedSpaces = new List<Hex>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OccupyHex(Hex hex)
    {
        occupiedSpaces.Add(hex);
        Instantiate(slimeModel, new Vector3(hex.transform.position.x, hex.transform.position.y + .5f, hex.transform.position.z), new Quaternion(0f, 0f, 0f, 0f));
    }

    public void OnTurnStart()
    {
        if(MoistureCount < occupiedSpaces.Count)
        {
            //Slime dies
        }
        else
        {
            MoistureCount -= occupiedSpaces.Count;
        }
    }

    public void OnTurnEnd()
    {
        foreach(Hex hex in occupiedSpaces)
        {
            OxygenCount += hex.AbsorbOxygen() ? 1 : 0;
            NutrientCount += hex.AbsorbNutrients() ? 1 : 0;
            MoistureCount += hex.AbsorbMoisture() ? 1 : 0;
        }
    }
}