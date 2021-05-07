using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    #endregion

    public Unit.Teams whosTurn;
    public List<Unit.Teams> teamsOnGame = new List<Unit.Teams>();
    public int masterTurn = 0;
    public bool canMove;

    public Tile[,] tiles;
    public Dictionary<Tile, AttackingUnitDisplay> displayTiles = new Dictionary<Tile, AttackingUnitDisplay>();
    public Dictionary<Tile, GameObject> highlightTiles = new Dictionary<Tile, GameObject>();

    [SerializeField] GameObject attackingDisplay;
    [SerializeField] GameObject displayTilesContainer;
    [SerializeField] GameObject highlightTile;
    [SerializeField] GameObject highlightTilesContainer;

    public int width;
    public int height;

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
        tiles[5, 13].unit = tempUnit;
        CreateDisplay(tiles[5, 13], tempUnit);
        
        tempUnit.team = Unit.Teams.Blue;
        tempUnit.type = Unit.Types.Castle;
        tempUnit.size = 20;
        tiles[5, 7].unit = tempUnit;
        CreateDisplay(tiles[5, 7], tempUnit);        
        
        tempUnit.team = Unit.Teams.Green;
        tempUnit.type = Unit.Types.Castle;
        tempUnit.size = 20;
        tiles[16, 13].unit = tempUnit;
        CreateDisplay(tiles[16, 13], tempUnit);        
        
        tempUnit.team = Unit.Teams.Yellow;
        tempUnit.type = Unit.Types.Castle;
        tempUnit.size = 20;
        tiles[16, 7].unit = tempUnit;
        CreateDisplay(tiles[16, 7], tempUnit);
        #endregion

        #region Warriors
        tempUnit.team = Unit.Teams.Red;
        tempUnit.type = Unit.Types.Warrior;
        tempUnit.size = 20;
        tiles[6, 13].unit = tempUnit;
        CreateDisplay(tiles[6, 13], tempUnit);
        
        tempUnit.team = Unit.Teams.Blue;
        tempUnit.type = Unit.Types.Warrior;
        tempUnit.size = 20;
        tiles[6, 7].unit = tempUnit;
        CreateDisplay(tiles[6, 7], tempUnit);        
        
        tempUnit.team = Unit.Teams.Green;
        tempUnit.type = Unit.Types.Warrior;
        tempUnit.size = 20;
        tiles[15, 13].unit = tempUnit;
        CreateDisplay(tiles[15, 13], tempUnit);        
        
        tempUnit.team = Unit.Teams.Yellow;
        tempUnit.type = Unit.Types.Warrior;
        tempUnit.size = 20;
        tiles[15, 7].unit = tempUnit;
        CreateDisplay(tiles[15, 7], tempUnit);
        #endregion
    }

    /// <summary>
    /// Can be used for creating display for tile has unit
    /// </summary>
    /// <param name="tile">Tile which want to create display to it</param>
    /// <param name="unit">Unit info for creating display output</param>
    void CreateDisplay(Tile tile, Unit unit)
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
                break;
        }
        canMove = true;
    }
}
