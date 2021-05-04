using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int col;
    public int row;

    [SerializeField]Unit unit;
    public Tile[] neighbours = new Tile[6];

    Player player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        if (col == 10 && row == 10)
        {
            unit.team = Unit.Teams.Universal;
            unit.type = Unit.UnitTypes.Shop;
            unit.size = 0;

            FuncForNow();
        }
    }

    public Vector3 OffsetToCubeConversion()
    {
        var x = col - (row + (row & 1)) / 2;
        var z = row;
        var y = -x - z;

        return new Vector3(x, y, z);
    }

    public float DistanceTo(Tile tile)
    {
        Vector3 a = OffsetToCubeConversion();
        Vector3 b = tile.OffsetToCubeConversion();

        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y), Mathf.Abs(a.z - b.z));
    }

    public List<Tile> wanted;
    public List<Tile> FuncForNow()
    {
        Vector3 temp = OffsetToCubeConversion();

        foreach (var neighbour in neighbours)
        {
            wanted.Add(neighbour);
            foreach (var a in neighbour.neighbours)
            {
                wanted.Add(neighbour);
            }
        }
        return wanted;
    }

    void OnMouseEnter()
    {
        //Mouse is over this tile
        //probably assign to the "Player" script for onOverTile(?) value

        player.tileOnMouseOver = this;
    }

    void OnMouseOver()
    {

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
