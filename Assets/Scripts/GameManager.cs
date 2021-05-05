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

    public Tile[,] tiles;
    public Dictionary<Tile, GameObject> displayTiles = new Dictionary<Tile, GameObject>();

    [SerializeField] GameObject attackingDisplay;

    public int width;
    public int height;

    /// <summary>
    /// Will start the game by placing all castle and required attacking units
    /// </summary>
    public void StartGame()
    {
        Unit tempUnit = new Unit();

        tempUnit.team = Unit.Teams.Red;
        tempUnit.type = Unit.UnitTypes.Castle;
        tempUnit.size = 20;
        tiles[10, 10].unit = tempUnit;
        CreateDisplay(tiles[10, 10], tempUnit);
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
        go.GetComponent<AttackingUnitDisplay>().team = unit.team;
        go.GetComponent<AttackingUnitDisplay>().type = unit.type;
        go.GetComponent<AttackingUnitDisplay>().size = unit.size;

        displayTiles.Add(tile, go);
    }
}
