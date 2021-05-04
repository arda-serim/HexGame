using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapGenerator : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;

    [SerializeField] GameObject tileGO;
    [SerializeField] GameObject tilesGO;
    Tile[,] tiles = new Tile[20, 20];
    //Dictionary<Vector2, GameObject> tiles = new Dictionary<Vector2, GameObject>();

    void Start()
    {
        StartCoroutine(Method());
    }

    IEnumerator Method()
    {
        for (int col = 0; col < width; col++)
        {
            for (int row = 0; row < height; row++)
            {
                Vector3 pos = new Vector3(0.5f * Mathf.Sqrt(3) * (col - 0.5f * (row & 1)), 0.5f * 3 / 2 * row);

                GameObject go = Instantiate(tileGO, pos, Quaternion.identity);
                Tile tempTile = go.GetComponent<Tile>();
                tempTile.col = col;
                tempTile.row = row;
                go.name = $"Tile ({col}, {row})";
                go.transform.SetParent(tilesGO.transform);
                tiles[col, row] = tempTile;

                if ((row & 1) == 0 && row != 0 && col != 0)
                {
                    tempTile.neighbours[0] = tiles[col, row + 1];
                    tempTile.neighbours[1] = tiles[col - 1, row + 1];
                    tempTile.neighbours[2] = tiles[col - 1, row];
                    tempTile.neighbours[3] = tiles[col - 1, row - 1];
                    tempTile.neighbours[4] = tiles[col, row - 1];
                    tempTile.neighbours[5] = tiles[col + 1, row];
                }
                else
                {
                    tempTile.neighbours[0] = tiles[col + 1, row + 1];
                    tempTile.neighbours[1] = tiles[col, row + 1];
                    tempTile.neighbours[2] = tiles[col - 1, row];
                    tempTile.neighbours[3] = tiles[col, row - 1];
                    tempTile.neighbours[4] = tiles[col + 1, row - 1];
                    tempTile.neighbours[5] = tiles[col + 1, row];
                }

                if (col == 10 & row == 10)
                {
                    go.GetComponent<SpriteRenderer>().color = Color.blue;
                }
                yield return null;
            }
        }
        Destroy(gameObject);
    }
}
