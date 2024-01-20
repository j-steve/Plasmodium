using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    HexBoard hexBoard;
    Slime slime;

    public int CurrentDifficultyLevel;


    // Start is called before the first frame update
    void Start()
    {
        hexBoard = GetComponent<HexBoard>();
        slime = GetComponent<Slime>();

        hexBoard.GenerateHexBoard();
        CurrentDifficultyLevel = 0;
    }

    public void PlaceSlime(Hex startingTile)
    {
        /*int middleRange = hexBoard.BoardRadius / 5;

        int q = Random.Range(-middleRange, middleRange);
        int r = Random.Range(-middleRange, middleRange);
        Hex startingTile = HexBoard.Active.Hexes[new HexCoordinates(q, r)];*/
        slime.OccupyHex(startingTile);
        Instantiate(slime.SlimeModel, new Vector3(startingTile.transform.position.x, startingTile.transform.position.y + .5f, startingTile.transform.position.z), new Quaternion(45f,0f,0f,0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
