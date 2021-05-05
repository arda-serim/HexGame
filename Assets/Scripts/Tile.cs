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

    public List<Tile> Pathfind(Tile to)
    {
        Dictionary<Tile, float> dist = new Dictionary<Tile, float>();
        Dictionary<Tile, Tile> prev = new Dictionary<Tile, Tile>();

        List<Tile> unvisited = new List<Tile>();

        dist[this] = 0;
        prev[this] = null;

        foreach (Tile v in GameManager.Instance.tiles)
        {
            if (v != this)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }

            unvisited.Add(v);
        }

        while (unvisited.Count > 0)
        {
            Tile u = null;

            foreach (var possibleU in unvisited)
            {
                if (u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            //Reach the target so break the while loop
            if (u == to)
            {
                break;
            }

            unvisited.Remove(u);

            foreach (var v in u.neighbours)
            {
                float alt = dist[u] + u.DistanceTo(v);

                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        Tile tempCur = to;
        List<Tile> tempPath = new List<Tile>();

        while (tempCur != null)
        {
            tempPath.Add(tempCur);
            tempCur = prev[tempCur];
        }

        if (prev[to] != null)
        {
            tempPath.Remove(this);
            tempPath.Reverse();
            return tempPath;
        }

        return null;
    }

    void OnMouseEnter()
    {
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
        Warrior,
        Archer,
        Knight,
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

    public int size;
}
