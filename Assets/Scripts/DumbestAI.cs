using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbestAI : MonoBehaviour
{
    public Unit.Teams team;

    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.Instance.canMove && GameManager.Instance.whosTurn == team)
        {
            DoMostStupidThingTheWorldHaveEverSeen();
        }   
    }

    void DoMostStupidThingTheWorldHaveEverSeen()
    {
        List<Tile> tempList = new List<Tile>();
        foreach (var item in GameManager.Instance.tiles)
        {
            if (item.unit.team == team && item.unit.type != Unit.Types.Castle)
            {
                tempList.Add(item);
            }
        }

        StartCoroutine(tempList[0].GoTo(tempList[0].Range(2)[Random.Range(0, tempList[0].Range(2).Count - 1)]));
    }
}
