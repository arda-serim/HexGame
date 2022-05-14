using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   #region Singleton

   static GameManager _instance;
   public static GameManager Instance
   {
      get
      {
         return _instance;
      }
   }

   private void Awake()
   {
      _instance = this;
      tiles = new Tile[width, height];

      masterTurn = 1;
      selectionButtons.SetActive(false);
      winnerText.gameObject.SetActive(false);
   }

   #endregion

   public Unit.Teams whosTurn;
   public List<Unit.Teams> teamsOnGame = new List<Unit.Teams>();
   public int masterTurn;
   public bool canMove;

   Player player, player2, player3, player4;
   public bool buttonPressed = false;
   public Unit.Types unitToPlace = Unit.Types.Empty;

   [SerializeField]
   GameObject selectionButtons;
   [SerializeField]
   TextMeshProUGUI masterTurnText;

   public Tile[,] tiles;
   public Dictionary<Tile, AttackingUnitDisplay> displayTiles = new Dictionary<Tile, AttackingUnitDisplay>();
   public Dictionary<Tile, GameObject> highlightTiles = new Dictionary<Tile, GameObject>();

   [SerializeField] GameObject attackingDisplay;
   [SerializeField] GameObject displayTilesContainer;
   [SerializeField] GameObject highlightTile;
   [SerializeField] GameObject highlightTilesContainer;

   [SerializeField] TextMeshProUGUI textMesh;
   [SerializeField] TextMeshProUGUI winnerText;

   public int width;
   public int height;

   private void Update()
   {
      masterTurnText.text = "Turn:" + masterTurn.ToString();
      if (IsGameEnded())
      {
         if (Input.GetKeyDown(KeyCode.Space))
         {
            SceneManager.LoadScene(0);
         }
      }


   }


   /// <summary>
   /// Will start the game by placing all castle and required attacking units
   /// </summary>
   public void StartGame()
   {
      whosTurn = Unit.Teams.Red;
      canMove = true;

      Unit tempUnit = new Unit();

      #region Castles
      tempUnit.team = Unit.Teams.Red;
      tempUnit.type = Unit.Types.Castle;
      tempUnit.size = 20;
      tempUnit.isMoved = false;
      tiles[2, 5].unit = tempUnit;
      CreateDisplay(tiles[2, 5], tempUnit);

      tempUnit.team = Unit.Teams.Blue;
      tempUnit.type = Unit.Types.Castle;
      tempUnit.size = 20;
      tempUnit.isMoved = false;
      tiles[17, 5].unit = tempUnit;
      CreateDisplay(tiles[17, 5], tempUnit);

      // tempUnit.team = Unit.Teams.Green;
      // tempUnit.type = Unit.Types.Castle;
      // tempUnit.size = 20;
      // tempUnit.isMoved = false;
      // tiles[24, 15].unit = tempUnit;
      // CreateDisplay(tiles[24, 15], tempUnit);

      // tempUnit.team = Unit.Teams.Yellow;
      // tempUnit.type = Unit.Types.Castle;
      // tempUnit.size = 20;
      // tempUnit.isMoved = false;
      // tiles[24, 2].unit = tempUnit;
      // CreateDisplay(tiles[24, 2], tempUnit);
      #endregion
      player = GameObject.Find("Player").GetComponent<Player>();
      player2 = GameObject.Find("Player2").GetComponent<Player>();
      // player3 = GameObject.Find("Player3").GetComponent<Player>();
      // player4 = GameObject.Find("Player4").GetComponent<Player>();

      player.castleTile = tiles[2, 5];
      player2.castleTile = tiles[17, 5];
   }

   /// <summary>
   /// Can be used for creating display for tile has unit
   /// </summary>
   /// <param name="tile">Tile which want to create display to it</param>
   /// <param name="unit">Unit info for creating display output</param>
   public void CreateDisplay(Tile tile, Unit unit)
   {
      GameObject go = Instantiate(attackingDisplay);

      go.transform.position = tile.gameObject.transform.position;
      go.GetComponent<AttackingUnitDisplay>().unit.team = unit.team;
      go.GetComponent<AttackingUnitDisplay>().unit.type = unit.type;
      go.GetComponent<AttackingUnitDisplay>().unit.size = unit.size;
      go.transform.SetParent(displayTilesContainer.transform);

      displayTiles.Add(tile, go.GetComponent<AttackingUnitDisplay>());
   }

   /// <summary>
   /// Creates a HighlightTile on given tile
   /// </summary>
   /// <param name="tile"></param>
   public void CreateHighlightTile(Tile tile)
   {
      GameObject go = Instantiate(highlightTile);

      go.transform.position = tile.gameObject.transform.position;
      go.transform.SetParent(highlightTilesContainer.transform);

      highlightTiles.Add(tile, go);
   }

   /// <summary>
   /// Creates highlight tiles on given tiles
   /// </summary>
   /// <param name="tiles"></param>
   public void CreateHighlightTile(List<Tile> tiles)
   {
      foreach (var tile in tiles)
      {
         GameObject go = Instantiate(highlightTile);

         go.transform.position = tile.gameObject.transform.position;
         go.transform.SetParent(highlightTilesContainer.transform);

         highlightTiles.Add(tile, go);
      }
   }

   /// <summary>
   /// Removes all highlight tiles on the map
   /// </summary>
   public void RemoveHighlightTile()
   {
      foreach (var item in highlightTiles)
      {
         Destroy(item.Value);
      }
      highlightTiles.Clear();
   }

   /// <summary>
   /// Removes given highlight tile
   /// </summary>
   /// <param name="tile"></param>
   public void RemoveHighlightTile(Tile tile)
   {
      Destroy(highlightTiles[tile]);
      highlightTiles.Remove(tile);
   }

   /// <summary>
   /// Removes given highlight tiles
   /// </summary>
   /// <param name="tiles"></param>
   public void RemoveHighlightTile(List<Tile> tiles)
   {
      foreach (var tile in tiles)
      {
         Destroy(highlightTiles[tile]);
         highlightTiles.Remove(tile);
      }

   }

   // make all tiles in team not is moved
   public void ResetTiles()
   {
      foreach (var tile in tiles)
      {
         if (tile.unit.type != Unit.Types.Empty)
         {
            tile.unit.isMoved = false;
         }
      }
   }



   /// <summary>
   /// Change whosTurn based on which colors are playing and which are not
   /// </summary>
   public void NextTurn()
   {
      switch (whosTurn)
      {
         case Unit.Teams.Red:
            if (teamsOnGame.Contains(Unit.Teams.Blue))
            {
               whosTurn = Unit.Teams.Blue;
            }
            else if (teamsOnGame.Contains(Unit.Teams.Green))
            {
               whosTurn = Unit.Teams.Green;
            }
            else if (teamsOnGame.Contains(Unit.Teams.Yellow))
            {
               whosTurn = Unit.Teams.Yellow;
            }
            player.isRoundStarted = true;
            break;
         case Unit.Teams.Blue:
            if (teamsOnGame.Contains(Unit.Teams.Green))
            {
               whosTurn = Unit.Teams.Green;
            }
            else if (teamsOnGame.Contains(Unit.Teams.Yellow))
            {
               whosTurn = Unit.Teams.Yellow;
            }
            else if (teamsOnGame.Contains(Unit.Teams.Red))
            {
               whosTurn = Unit.Teams.Red;
               masterTurn += 1;
            }
            player2.isRoundStarted = true;
            break;
         case Unit.Teams.Green:
            if (teamsOnGame.Contains(Unit.Teams.Yellow))
            {
               whosTurn = Unit.Teams.Yellow;
            }
            else if (teamsOnGame.Contains(Unit.Teams.Red))
            {
               whosTurn = Unit.Teams.Red;
               masterTurn += 1;
            }
            else if (teamsOnGame.Contains(Unit.Teams.Blue))
            {
               whosTurn = Unit.Teams.Blue;
            }
            player3.isRoundStarted = true;
            break;
         case Unit.Teams.Yellow:
            if (teamsOnGame.Contains(Unit.Teams.Red))
            {
               whosTurn = Unit.Teams.Red;
               masterTurn += 1;
            }
            else if (teamsOnGame.Contains(Unit.Teams.Blue))
            {
               whosTurn = Unit.Teams.Blue;
            }
            else if (teamsOnGame.Contains(Unit.Teams.Green))
            {
               whosTurn = Unit.Teams.Green;
            }
            player4.isRoundStarted = true;
            break;
      }
      ResetTiles();
      canMove = true;
      textMesh.text = "Turn: " + whosTurn;
   }

   /// <summary>
   /// Makes an attack
   /// </summary>
   /// <param name="maxDamage">Maximum damage that can be caused by attacker</param>
   /// <param name="tile">Tile to check</param>
   public void Attack(int maxDamage, Tile tile)
   {
      int damage = Random.Range(1, maxDamage);
      AttackingUnitDisplay attackerDisplay = displayTiles[tile];
      tile.unit.size -= damage;

      int attackerDisplayText = int.Parse(attackerDisplay.textMesh.text);
      if (attackerDisplayText - damage <= 0)
      {
         tile.unit.type = Unit.Types.Empty;
      }
      attackerDisplayText -= damage;
      attackerDisplay.textMesh.text = attackerDisplayText.ToString();
   }

   /// <summary>
   /// Destroys all units for a team
   /// </summary>
   public void DestroyAttackingDisplayTiles(Unit.Teams team)
   {
      List<Tile> tiles = new List<Tile>();
      foreach (var item in displayTiles)
      {
         if (item.Value.unit.team == team)
         {
            tiles.Add(item.Key);
         }
      }
      foreach (var tile in tiles)
      {
         Destroy(displayTiles[tile]);
         AttackingUnitDisplay displayUnit = displayTiles[tile];
         displayTiles.Remove(tile);
         Destroy(displayUnit.gameObject);
      }
   }

   public void OnClickSelectUnitToPlace()
   {
      // get game object tag
      string tag = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.tag;

      if (tag == "Warrior")
      {
         unitToPlace = Unit.Types.Warrior;
      }
      else if (tag == "Archer")
      {
         unitToPlace = Unit.Types.Archer;
      }
      else if (tag == "Cavalry")
      {
         unitToPlace = Unit.Types.Cavalry;
      }
      else if (tag == "Knight")
      {
         unitToPlace = Unit.Types.Knight;
      }
      else if (tag == "Assassin")
      {
         unitToPlace = Unit.Types.Assassin;
      }
      buttonPressed = true;
      selectionButtons.SetActive(false);
   }


   public bool CheckIfUnitExists(Unit.Teams team)
   {
      foreach (var item in tiles)
      {
         if (item.unit.team == team && item.unit.type != Unit.Types.Castle)
         {
            Debug.Log("heree");
            return true;
         }
      }
      return false;
   }


   public void OnClickFinishRound()
   {
      if (whosTurn == Unit.Teams.Red)
      {
         player.isRoundStarted = true;
         GameManager.Instance.RemoveHighlightTile();
         GameManager.Instance.NextTurn();
      }
      else if (whosTurn == Unit.Teams.Blue)
      {
         player2.isRoundStarted = true;
         GameManager.Instance.RemoveHighlightTile();
         GameManager.Instance.NextTurn();
      }
      else if (whosTurn == Unit.Teams.Green)
      {
         player3.isRoundStarted = true;
         GameManager.Instance.RemoveHighlightTile();
         GameManager.Instance.NextTurn();
      }
      else if (whosTurn == Unit.Teams.Yellow)
      {
         player4.isRoundStarted = true;
         GameManager.Instance.RemoveHighlightTile();
         GameManager.Instance.NextTurn();
      }
      RemoveHighlightTile();

   }
   public void OnClickMainMenu()
   {
      UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
   }

   // check if only one player remained
   public bool IsGameEnded()
   {
      Unit.Teams team = Unit.Teams.Universal;
      if (player.castleTile.unit.type == Unit.Types.Empty)
      {
         team = player.team;
      }
      else if (player2.castleTile.unit.type == Unit.Types.Empty)
      {
         team = player2.team;
      }
      // else if (player3.castleTile.unit.type == Unit.Types.Empty) {

      // }
      // else if (player4.castleTile.unit.type == Unit.Types.Empty) {

      // }

      if (team != Unit.Teams.Universal)
      {
         GameManager.Instance.EndGame(team);
         return true;
      }
      return false;
   }

   public void EndGame(Unit.Teams team)
   {
      winnerText.text = team.ToString() + "Win\nPress Space to go back to main menu";
      winnerText.gameObject.SetActive(true);
   }
}

