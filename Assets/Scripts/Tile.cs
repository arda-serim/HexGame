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

    public Unit unit = new Unit();
    public List<Tile> neighbours = new List<Tile>();

    Player player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    /// <summary>
    /// Calculates shortest distnace to given tile
    /// </summary>
    /// <param name="to">Tile which want to calculate</param>
    /// <returns></returns>
    public float DistanceTo(Tile to)
    {
        return Mathf.Max(Mathf.Abs(x - to.x), Mathf.Abs(y - to.y), Mathf.Abs(z - to.z));
    }

    /// <summary>
    /// Checks every tile's distance to this tile then returns tiles which has in range
    /// </summary>
    /// <param name="range">Max distance to tile</param>
    /// <returns></returns>
    public List<Tile> Range(int range)
    {
        List<Tile> tempList = new List<Tile>();

        foreach (var item in GameManager.Instance.tiles)
        {
            if ((item.col != col || item.row != row))
            {
                if (item.DistanceTo(this) <= range)
                {
                    tempList.Add(item);
                }
            }
        }

        return tempList;
    }

    /// <summary>
    /// Uses Dijkstra's Algorithm to find the shortest path for given tile
    /// </summary>
    /// <param name="to">Tile want to find path</param>
    /// <returns></returns>
    public List<Tile> Pathfind(Tile to)
    {
        Dictionary<Tile, float> dist = new Dictionary<Tile, float>();
        Dictionary<Tile, Tile> prev = new Dictionary<Tile, Tile>();

        List<Tile> unvisited = new List<Tile>();

        dist[this] = 0;
        prev[this] = null;
        
        //Adds empty tiles to unvisited list
        foreach (Tile v in GameManager.Instance.tiles)
        {
            if (v.unit.type == Unit.Types.Empty || v == this)
            {
                if (v != this)
                {
                    dist[v] = Mathf.Infinity;
                    prev[v] = null;
                }
                unvisited.Add(v);
            }
        }

        //Proceeds until every unvisited tile has been checked
        while (unvisited.Count > 0)
        {
            Tile u = null;

            foreach (var possibleU in unvisited)
            {
                if (u == null || (dist[possibleU] < dist[u]))
                {
                    u = possibleU;
                }
            }

            //Reached the target so break the while loop
            if (u == to)
            {
                break;
            }

            unvisited.Remove(u);

            //Checks every v's neighbours for best possible route
            foreach (var v in u.neighbours)
            {
                float alt = dist[u] + u.DistanceTo(v);

                if (v.unit.type == Unit.Types.Empty && alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        Tile tempCur = to;
        List<Tile> tempPath = new List<Tile>();

        //Checks if found any path
        while (tempCur != null && prev.ContainsKey(tempCur))
        {
            tempPath.Add(tempCur);
            tempCur = prev[tempCur];
        }

        //If found any path, assign it
        if (tempPath != null)
        {
            tempPath.Remove(this);
            tempPath.Reverse();
            return tempPath;
        }

        //If code execute this line then there is no path at all
        return null;
    }

    /// <summary>
    /// Goes to given tile with Pathfind()
    /// </summary>
    /// <param name="to">Tile which want to go</param>
    /// <returns></returns>
    public IEnumerator GoTo(Tile to)
    {
        GameManager.Instance.canMove = false;

        List<Tile> path = new List<Tile>();
        var display = GameManager.Instance.displayTiles[this];

        path = Pathfind(to);
        while (path.Count > 0)
        {
            float t = 0;
            Vector3 tempVec = display.transform.position;
            while (display.transform.position != path[0].transform.position)
            {
                display.transform.position = Vector3.Lerp(tempVec, path[0].transform.position, t);
                yield return new WaitForSeconds(0.005f);
                t += 0.02f;
            }
            yield return new WaitForSeconds(0.05f);
            path.RemoveAt(0);
        }

        GameManager.Instance.displayTiles.Remove(this);
        this.unit.team = Unit.Teams.Universal;
        this.unit.type = Unit.Types.Empty;
        this.unit.size = 0;
        GameManager.Instance.displayTiles.Add(to, display);
        to.unit.team = display.unit.team;
        to.unit.type = display.unit.type;
        to.unit.size = display.unit.size;

        player.selectedTile = null;
        GameManager.Instance.NextTurn();
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
    public enum Types
    {
        Empty,
        Warrior,
        Archer,
        Knight,
        Assassin,
        Castle,
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
    

    public Types type;

    public Teams team;

    public int size;
}
