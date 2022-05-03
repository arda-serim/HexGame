using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
   public Unit.Teams team;
   Tile castleTile;
   Camera myCamera;

   [SerializeField]
   TextMeshProUGUI textMesh;
   bool isPlacing;
   public bool isRoundStarted;

   bool isPlaced;
   int newCredits;

   [SerializeField]
   GameObject selectionButtons;

   public Tile tileOnMouseOver;
   public Tile selectedTile;

   int credits;

   void Start()
   {
      myCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
      GameManager.Instance.teamsOnGame.Add(team);
      isPlacing = false;
      isRoundStarted = false;
      isPlaced = false;
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
      textMesh.text = credits.ToString();
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
         castleTile = GameManager.Instance.tiles[24, 15];
      }
      else if (team == Unit.Teams.Yellow)
      {
         castleTile = GameManager.Instance.tiles[24, 2];
      }
      try
      {
         if (Input.GetKeyDown(KeyCode.Mouse0) && tileOnMouseOver == selectedTile)
         {
            GameManager.Instance.RemoveHighlightTile();
            isPlacing = false;
            selectedTile = null;
         }
         else if (isPlacing && GameManager.Instance.whosTurn == team && tileOnMouseOver.unit.type != Unit.Types.Castle)
         {
            GameManager.Instance.RemoveHighlightTile();
            GameManager.Instance.CreateHighlightTile(castleTile.RangeWithEmpty(2));
            if (Input.GetKeyDown(KeyCode.Mouse0) && tileOnMouseOver.DistanceTo(castleTile) <= 2 && tileOnMouseOver.unit.type == Unit.Types.Empty && !tileOnMouseOver.unit.isMoved) // will add skip this round
            {
               selectedTile = tileOnMouseOver;
               StartCoroutine(PlaceUnit());
            }
         }
         else if (GameManager.Instance.whosTurn == team && !isPlacing && Input.GetKeyDown(KeyCode.Mouse0))
         {
            if (team == tileOnMouseOver.unit.team && !tileOnMouseOver.unit.isMoved)
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
               else if (tileOnMouseOver.unit.type == Unit.Types.Castle)
               {
                  isPlacing = true;
                  GameManager.Instance.CreateHighlightTile(selectedTile.RangeWithEmpty(2));
               }

            }

            else if (GameManager.Instance.whosTurn == team && !isPlacing)
            {
               if (team == tileOnMouseOver.unit.team && tileOnMouseOver.unit.isMoved == false)
               {
                  selectedTile = tileOnMouseOver;
               }
               else if (((selectedTile.unit.type == Unit.Types.Warrior || selectedTile.unit.type == Unit.Types.Archer) && selectedTile.Pathfind(tileOnMouseOver).Count <= 2)
               || ((selectedTile.unit.type == Unit.Types.Cavalry || selectedTile.unit.type == Unit.Types.Assassin) && selectedTile.Pathfind(tileOnMouseOver).Count <= 4)
               || (selectedTile.unit.type == Unit.Types.Knight && selectedTile.Pathfind(tileOnMouseOver).Count <= 1))
               {
                  if (tileOnMouseOver.unit.type == Unit.Types.Empty)
                  {
                     GameManager.Instance.RemoveHighlightTile();
                     StartCoroutine(selectedTile.GoTo(tileOnMouseOver));
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
                        selectedTile.unit.isMoved = true;

                        if (GameManager.Instance.IsAllTilesInTeamMoved(team))
                        {
                           isRoundStarted = true;
                           isPlacing = true;
                           GameManager.Instance.NextTurn();
                        }
                     }

                  }
               }

            }
         }

      }
      catch (System.Exception e)
      {
         // Debug.Log(e);
      }
   }
   IEnumerator PlaceUnit()
   {
      if (GameManager.Instance.unitToPlace == Unit.Types.Empty)
      {

         selectionButtons.SetActive(true);
         while (GameManager.Instance.buttonPressed == false)
         {
            yield return null;
         }
         if (selectedTile != null && selectedTile.DistanceTo(castleTile) <= 2)
         {
            Unit unit = new Unit();
            unit.team = team;
            unit.type = GameManager.Instance.unitToPlace;
            unit.isMoved = false;
            if (unit.type == Unit.Types.Warrior && credits >= 8)
            {
               unit.size = 10;
               credits -= 8;
               isPlaced = true;
            }
            else if (unit.type == Unit.Types.Archer && credits >= 6)
            {
               unit.size = 8;
               credits -= 6;
               isPlaced = true;
            }
            else if (unit.type == Unit.Types.Cavalry && credits >= 12 && GameManager.Instance.masterTurn >= 5)
            {
               unit.size = 10;
               credits -= 12;
               isPlaced = true;
            }
            else if (unit.type == Unit.Types.Knight && credits >= 20 && GameManager.Instance.masterTurn >= 7)
            {
               unit.size = 16;
               credits -= 20;
               isPlaced = true;
            }
            else if (unit.type == Unit.Types.Assassin && credits >= 20 && GameManager.Instance.masterTurn >= 10)
            {
               unit.size = 6;
               credits -= 20;
               isPlaced = true;
            }
            else
            {
               Debug.LogError("You cannot place " + unit.type);
            }
            GameManager.Instance.buttonPressed = false;
            if (isPlaced)
            {
               GameManager.Instance.tiles[selectedTile.col, selectedTile.row].unit = unit;
               GameManager.Instance.CreateDisplay(GameManager.Instance.tiles[selectedTile.col, selectedTile.row], unit);
               isPlaced = false;
            }
         }
         isPlacing = false;
         selectedTile = null;
         GameManager.Instance.RemoveHighlightTile();
         GameManager.Instance.unitToPlace = Unit.Types.Empty;

      }
   }

}
