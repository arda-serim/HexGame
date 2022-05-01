using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   public Unit.Teams team;
   Tile castleTile;
   Camera myCamera;

   bool isPlacing;
   bool isRoundStarted;

   public Tile tileOnMouseOver;
   public Tile selectedTile;

   int credits;

   void Start()
   {
      myCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
      GameManager.Instance.teamsOnGame.Add(team);
      isPlacing = true;
      isRoundStarted = false;
      credits = 10;
   }



   void Update()
   {

      if (!GameManager.Instance.canMove || GameManager.Instance.whosTurn != team)
         return;

      if (isRoundStarted)
      {
         credits += 8 + (GameManager.Instance.masterTurn / 15 + 2) * GameManager.Instance.masterTurn / 5;
         isRoundStarted = false;
      }
      if (team == Unit.Teams.Red)
      {
         castleTile = GameManager.Instance.tiles[3, 15];
      }
      else if (team == Unit.Teams.Blue)
      {
         castleTile = GameManager.Instance.tiles[3, 2];
      }
      else if (team == Unit.Teams.Green)
      {
         castleTile = GameManager.Instance.tiles[28, 15];
      }
      else if (team == Unit.Teams.Yellow)
      {
         castleTile = GameManager.Instance.tiles[28, 2];
      }
      if (isPlacing && GameManager.Instance.whosTurn == team)
      {
         GameManager.Instance.RemoveHighlightTile();
         GameManager.Instance.CreateHighlightTile(castleTile.RangeWithEmpty(2));
         Unit tempUnit = new Unit();
         tempUnit.type = Unit.Types.Warrior;
         // GameManager.Instance.CreateHighlightTile(castleTile.RangeWithEmpty(2));
         if (Input.GetKeyDown(KeyCode.Mouse0) && tileOnMouseOver.DistanceTo(castleTile) <= 2 && tileOnMouseOver.unit.type == Unit.Types.Empty) // will add skip this round
         {
            selectedTile = tileOnMouseOver;
            PlaceUnit(tempUnit.type);
            isPlacing = false;
            selectedTile = null;
         }
      }
      if (!isPlacing && Input.GetKeyDown(KeyCode.Mouse0))
      {

         if (team == tileOnMouseOver.unit.team)
         {
            GameManager.Instance.RemoveHighlightTile();
            selectedTile = tileOnMouseOver;
            if (tileOnMouseOver.unit.type == Unit.Types.Warrior)
            {
               GameManager.Instance.CreateHighlightTile(selectedTile.RangeWithEmpty(2));
            }
            else if (tileOnMouseOver.unit.type == Unit.Types.Archer)
            {
               GameManager.Instance.CreateHighlightTile(selectedTile.RangeWithEmpty(2));
            }
            else if (tileOnMouseOver.unit.type == Unit.Types.Cavalry)
            {
               GameManager.Instance.CreateHighlightTile(selectedTile.RangeWithEmpty(4));
            }
            else if (tileOnMouseOver.unit.type == Unit.Types.Knight)
            {
               GameManager.Instance.CreateHighlightTile(selectedTile.RangeWithEmpty(1));
            }
            else if (tileOnMouseOver.unit.type == Unit.Types.Assassin)
            {
               GameManager.Instance.CreateHighlightTile(selectedTile.RangeWithEmpty(4));
            }

         }
         else if (!isPlacing)
         {
            if (team == tileOnMouseOver.unit.team)
            {
               selectedTile = tileOnMouseOver;
            }
            else if (((selectedTile.unit.type == Unit.Types.Warrior || selectedTile.unit.type == Unit.Types.Archer) && selectedTile.Pathfind(tileOnMouseOver).Count <= 2)
            || ((selectedTile.unit.type == Unit.Types.Cavalry || selectedTile.unit.type == Unit.Types.Assassin) && selectedTile.Pathfind(tileOnMouseOver).Count <= 4)
            || (selectedTile.unit.type == Unit.Types.Knight && selectedTile.Pathfind(tileOnMouseOver).Count <= 1))
            {
               if (tileOnMouseOver.unit.type == Unit.Types.Empty)
               {
                  Debug.Log("Found a tile can be gone, sir");
                  GameManager.Instance.RemoveHighlightTile();
                  isRoundStarted = true;
                  StartCoroutine(selectedTile.GoTo(tileOnMouseOver));
                  isPlacing = true;
               }
               else if (team != tileOnMouseOver.unit.team && tileOnMouseOver.unit.team != Unit.Teams.Universal)
               {
                  int maxDamage;
                  if (selectedTile.unit.type == Unit.Types.Warrior && selectedTile.InRange(tileOnMouseOver, 1))
                  {
                     maxDamage = 8;
                  }
                  else if (selectedTile.unit.type == Unit.Types.Archer && selectedTile.InRange(tileOnMouseOver, 3))
                  {
                     maxDamage = 6;
                  }
                  else if (selectedTile.unit.type == Unit.Types.Cavalry && selectedTile.InRange(tileOnMouseOver, 1))
                  {
                     maxDamage = 8;
                  }
                  else if (selectedTile.unit.type == Unit.Types.Knight && selectedTile.InRange(tileOnMouseOver, 1))
                  {
                     maxDamage = 8;
                  }
                  else if (selectedTile.unit.type == Unit.Types.Assassin && selectedTile.InRange(tileOnMouseOver, 2))
                  {
                     maxDamage = 12;
                  }
                  else
                  {
                     maxDamage = 0;
                  }
                  if (maxDamage > 0)
                  {
                     GameManager.Instance.RemoveHighlightTile();
                     GameManager.Instance.Attack(maxDamage, tileOnMouseOver);
                     isRoundStarted = true;
                     GameManager.Instance.NextTurn();
                  }
                  if (tileOnMouseOver.unit.type == Unit.Types.Castle)
                  {
                     Debug.Log("Found a Enemy Castle, sir");
                  }
                  else
                  {
                     Debug.Log("Found a Enemy attacker, sir");
                  }
                  isPlacing = true;

               }
            }

         }
      }
   }
   void PlaceUnit(Unit.Types type)
   {

      if (selectedTile != null && selectedTile.DistanceTo(castleTile) <= 2)
      {
         Unit unit = new Unit();
         unit.team = team;
         unit.type = type;
         if (type == Unit.Types.Warrior)
         {
            unit.size = 10;
            credits -= 8;
         }
         else if (type == Unit.Types.Archer)
         {
            unit.size = 8;
            credits -= 6;
         }
         else if (type == Unit.Types.Cavalry)
         {
            unit.size = 10;
            credits -= 12;
         }
         else if (type == Unit.Types.Knight)
         {
            unit.size = 16;
            credits -= 20;
         }
         else if (type == Unit.Types.Assassin)
         {
            unit.size = 6;
            credits -= 20;
         }
         GameManager.Instance.tiles[selectedTile.col, selectedTile.row].unit = unit;
         GameManager.Instance.CreateDisplay(GameManager.Instance.tiles[selectedTile.col, selectedTile.row], unit);

      }
   }

}
