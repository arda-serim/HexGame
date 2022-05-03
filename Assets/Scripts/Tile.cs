using System;
using System.Threading.Tasks;
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
   Player player2;
   Player player3;
   Player player4;

   void Start()
   {
      player = GameObject.Find("Player").GetComponent<Player>();
      player2 = GameObject.Find("Player2").GetComponent<Player>();
      player3 = GameObject.Find("Player3").GetComponent<Player>();
      player4 = GameObject.Find("Player4").GetComponent<Player>();
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

      foreach (var tile in GameManager.Instance.tiles)
      {
         if (tile.col != col || tile.row != row && Pathfind(tile).Count <= range)
         {
            if (tile.DistanceTo(this) <= range)
            {
               tempList.Add(tile);
            }
         }
      }

      return tempList;
   }

   /// <summary>
   /// Checks every tile's distance to this tile then returns tiles which has in range
   /// </summary>
   /// <param name="range">Max distance to tile</param>
   /// <returns></returns>
   public bool InRange(Tile to, int range)
   {
      return DistanceTo(to) <= range;
   }

   public List<Tile> RangeWithEmpty(int range)
   {
      List<Tile> tempList = new List<Tile>();
      List<Tile> realTempList = new List<Tile>();

      foreach (var tile in GameManager.Instance.tiles)
      {
         if ((tile.col != col || tile.row != row) && tile.unit.type == Unit.Types.Empty)
         {
            if (tile.DistanceTo(this) <= range)
            {
               tempList.Add(tile);
            }
         }
      }
      foreach (var tile in tempList)
      {
         if (Pathfind(tile).Count <= range)
         {
            realTempList.Add(tile);
         }
      }
      return realTempList;
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
      to.unit.isMoved = true;

      player.selectedTile = null;
      player2.selectedTile = null;
      player3.selectedTile = null;
      player4.selectedTile = null;

      if (GameManager.Instance.IsAllTilesInTeamMoved(to.unit.team))
      {
         GameManager.Instance.NextTurn();
      }
      GameManager.Instance.canMove = true;
   }



   void OnMouseEnter()
   {
      if (!GameManager.Instance.highlightTiles.ContainsKey(this))
      {
         GameManager.Instance.CreateHighlightTile(this);
      }
      if (GameManager.Instance.whosTurn == player.team)
      {
         player.tileOnMouseOver = this;
      }
      else if (GameManager.Instance.whosTurn == player2.team)
      {
         player2.tileOnMouseOver = this;
      }
      else if (GameManager.Instance.whosTurn == player3.team)
      {
         player3.tileOnMouseOver = this;
      }
      else if (GameManager.Instance.whosTurn == player4.team)
      {
         player4.tileOnMouseOver = this;
      }
      // if (GameManager.Instance.whosTurn == Unit.Teams.Red && player.selectedTile == null && !GameManager.Instance.highlightTiles.ContainsKey(this))
      // {
      //    GameManager.Instance.CreateHighlightTile(this);
      // }
      // else if (GameManager.Instance.whosTurn == Unit.Teams.Blue && player2.selectedTile == null && !GameManager.Instance.highlightTiles.ContainsKey(this))
      // {
      //    GameManager.Instance.CreateHighlightTile(this);
      // }
      // else if (GameManager.Instance.whosTurn == Unit.Teams.Green && player3.selectedTile == null && !GameManager.Instance.highlightTiles.ContainsKey(this))
      // {
      //    GameManager.Instance.CreateHighlightTile(this);
      // }
      // else if (GameManager.Instance.whosTurn == Unit.Teams.Yellow && player4.selectedTile == null && !GameManager.Instance.highlightTiles.ContainsKey(this))
      // {
      //    GameManager.Instance.CreateHighlightTile(this);
      // }
   }

   void OnMouseOver()
   {

   }

   void OnMouseExit()
   {
      player.tileOnMouseOver = null;
      player2.tileOnMouseOver = null;
      player3.tileOnMouseOver = null;
      player4.tileOnMouseOver = null;
      if (GameManager.Instance.highlightTiles.ContainsKey(this))
      {
         GameManager.Instance.RemoveHighlightTile(this);
      }
      // if (GameManager.Instance.whosTurn == Unit.Teams.Red && player.selectedTile == null && GameManager.Instance.highlightTiles.Count > 0)
      // {
      //    GameManager.Instance.RemoveHighlightTile(this);
      // }
      // else if (GameManager.Instance.whosTurn == Unit.Teams.Blue && player2.selectedTile == null && GameManager.Instance.highlightTiles.Count > 0)
      // {
      //    GameManager.Instance.RemoveHighlightTile(this);
      // }
      // else if (GameManager.Instance.whosTurn == Unit.Teams.Green && player3.selectedTile == null && GameManager.Instance.highlightTiles.Count > 0)
      // {
      //    GameManager.Instance.RemoveHighlightTile(this);
      // }
      // else if (GameManager.Instance.whosTurn == Unit.Teams.Yellow && player4.selectedTile == null && GameManager.Instance.highlightTiles.Count > 0)
      // {
      //    GameManager.Instance.RemoveHighlightTile(this);
      // }
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
      Cavalry,
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

   public bool isMoved;
   public Types type;

   public Teams team;

   public int size;
}
