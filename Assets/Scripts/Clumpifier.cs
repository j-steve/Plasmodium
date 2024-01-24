using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clumpifier : MonoBehaviour
{
    public float maxRadius, minClumpSize, maxClumpSize; 
    public int minClumps, maxClumps;
    public GameObject clumpObj;
    // Start is called before the first frame update
    void Start()
    {
        GenerateClump();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateClump()
    {
        int randClumps = Random.Range(minClumps, maxClumps);
        for (int i = 0; i <= randClumps; i++)
        {
            GenerateClumpPiece();
        }
    }

    private void GenerateClumpPiece()
    {
        //Randomize location within radius
        float randPosX = Random.Range(-maxRadius, maxRadius);
        float randPosZ = Random.Range(-maxRadius, maxRadius);
        Vector3 pos = new Vector3(transform.position.x + randPosX, transform.position.y, transform.position.z + randPosZ);
        GameObject newClump = Instantiate(clumpObj, pos, Quaternion.identity, this.gameObject.transform);
        float randScale = Random.Range(minClumpSize, maxClumpSize);
        newClump.transform.localScale = new Vector3(randScale, randScale, randScale);
        float randrot = Random.Range(0, 360);
        newClump.transform.Rotate(0, randrot, 0);
    }
}
