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
    public bool canMove;

    public Tile[,] tiles;
    public Dictionary<Tile, AttackingUnitDisplay> displayTiles = new Dictionary<Tile, AttackingUnitDisplay>();

    [SerializeField] GameObject attackingDisplay;
    [SerializeField] GameObject displayTilesContainer;

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

    public void NextTurn()
    {
        if (whosTurn.GetHashCode() < 4)
        {
            whosTurn = (Unit.Teams)whosTurn.GetHashCode() + 1;
        }
        else
        {
            whosTurn = (Unit.Teams)1;
        }
        canMove = true;
    }
}
