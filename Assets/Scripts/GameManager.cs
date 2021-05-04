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
    public List<Tile> asd = new List<Tile>();

    public int width;
    public int height;
}
