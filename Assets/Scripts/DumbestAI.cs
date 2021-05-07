using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbestAI : MonoBehaviour
{
    public Unit.Teams team;

    void Start()
    {
        GameManager.Instance.teamsOnGame.Add(team);
    }

    void Update()
    {
        if (GameManager.Instance.canMove && GameManager.Instance.whosTurn == team)
        {
            DoMostStupidThingTheWorldHaveEverSeen();
        }   
    }

    /// <summary>
    /// Makes the stupidest things for now(?!??)
    /// </summary>
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

        var tempRangeList = tempList[0].RangeWithEmpty(2);

        StartCoroutine(tempList[0].GoTo(tempRangeList[Random.Range(0, tempRangeList.Count - 1)]));
    }
}
