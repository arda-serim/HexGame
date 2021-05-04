using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int col;
    public int row;

    public int x;
    public int y;
    public int z;

    public Unit unit;
    public List<Tile> neighbours = new List<Tile>();

    Player player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        if (col == 10 && row == 10)
        {
            unit.team = Unit.Teams.Universal;
            unit.type = Unit.UnitTypes.Shop;
            unit.size = 0;


        }
    }

    public float DistanceTo(Tile to)
    {
        return Mathf.Max(Mathf.Abs(x - to.x), Mathf.Abs(y - to.y), Mathf.Abs(z - to.z));
    }

    public List<Tile> Range(int range)
    {
        List<Tile> tempList = new List<Tile>();

        foreach (var item in GameManager.Instance.tiles)
        {
            if (item.col != col || item.row != row)
            {
                if (item.DistanceTo(this) <= range)
                {
                    tempList.Add(item);
                }
            }
        }

        return tempList;
    }

    void OnMouseEnter()
    {
        player.tileOnMouseOver = this;
    }

    void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

        }
    }

    void OnMouseExit()
    {
        //Mouse is exited this tile
        //probably de-assign to the "Player" script for onOverTile(?) value

        player.tileOnMouseOver = null;
    }
}

[Serializable]
public struct Unit
{
    public enum UnitTypes
    {
        Empty,
        Swordsman,
        Archer,
        Horseman,
        Assassin,
        Castle,
        Barrack,
        Shop
    }
    public enum Teams
    {
        Universal,
        Red,
        Blue,
        Green,
        Yellow,
    }
    

    public UnitTypes type;

    public Teams team;

    public GameObject uiGO;

    public int size;
}
