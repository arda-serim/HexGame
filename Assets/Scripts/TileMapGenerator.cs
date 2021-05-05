using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TileMapGenerator : MonoBehaviour
{
    int width;
    int height;

    [SerializeField] GameObject realtileGO;
    [SerializeField] GameObject tilesGO;

    void Start()
    {
        width = GameManager.Instance.width;
        height = GameManager.Instance.height;
        StartCoroutine(GenerateMap());
    }

    IEnumerator GenerateMap()
    {
        //Generate interactable tiles
        for (int col = 0; col < width; col++)
        {
            for (int row = 0; row < height; row++)
            {
                Vector3 pos = new Vector3(0.5f * Mathf.Sqrt(3) * (col - 0.5f * (row & 1)), 0.5f * 3 / 2 * row);

                GameObject go = Instantiate(realtileGO, pos, Quaternion.identity);
                Tile tempTile = go.GetComponent<Tile>();

                //Assigning Cordinates
                tempTile.col = col;
                tempTile.row = row;
                Vector3 tempVec = OffsetToCubeConversion(col, row);
                tempTile.x = (int)tempVec.x;
                tempTile.y = (int)tempVec.y;
                tempTile.z = (int)tempVec.z;

                go.name = $"Tile ({col}, {row})";
                go.transform.SetParent(tilesGO.transform);
                GameManager.Instance.tiles[col, row] = tempTile;

                yield return null;
            }
        }

        //Adds neigbours to the real tiles
        for (int col = 0; col < width; col++)
        {
            for (int row = 0; row < height; row++)
            {
                Tile tempTile = GameManager.Instance.tiles[col, row];
                if ((row & 1) == 0)
                {
                    if (col +  1 >= 0 && col + 1 < width && row + 1 >= 0 && row + 1 < height)
                        tempTile.neighbours.Add(GameManager.Instance.tiles[col + 1, row + 1]);
                    if (row + 1 >= 0 && row + 1 < height)
                        tempTile.neighbours.Add(GameManager.Instance.tiles[col, row + 1]);
                    if (col - 1 >= 0 && col - 1 < width)
                        tempTile.neighbours.Add(GameManager.Instance.tiles[col - 1, row]);
                    if (row - 1 >= 0 && row - 1 < height)
                        tempTile.neighbours.Add(GameManager.Instance.tiles[col, row - 1]);
                    if (col + 1 >= 0 && col + 1 < width && row - 1 >= 0 && row - 1 < height)
                        tempTile.neighbours.Add(GameManager.Instance.tiles[col + 1, row - 1]);
                    if (col + 1 >= 0 && col + 1 < width)
                        tempTile.neighbours.Add(GameManager.Instance.tiles[col + 1, row]);
                }
                else
                {
                    if (row + 1 >= 0 && row + 1 < height)
                        tempTile.neighbours.Add(GameManager.Instance.tiles[col, row + 1]);
                    if (col - 1 >= 0 && col - 1 < width && row + 1 >= 0 && row + 1 < height)
                        tempTile.neighbours.Add(GameManager.Instance.tiles[col - 1, row + 1]);
                    if (col - 1 >= 0 && col - 1 < height)
                        tempTile.neighbours.Add(GameManager.Instance.tiles[col - 1, row]);
                    if (col - 1 >= 0 && col - 1 < width && row - 1 >= 0 && row - 1 < height)
                        tempTile.neighbours.Add(GameManager.Instance.tiles[col - 1, row - 1]);
                    if (row - 1 >= 0 && row - 1 < height)
                        tempTile.neighbours.Add(GameManager.Instance.tiles[col, row - 1]);
                    if (col + 1 >= 0 && col + 1 < width)
                        tempTile.neighbours.Add(GameManager.Instance.tiles[col + 1, row]);
                }
            }
        }

        GameManager.Instance.StartGame();
        Destroy(gameObject);
    }

    public Vector3 OffsetToCubeConversion(int col, int row)
    {
        var x = col - (row + (row & 1)) / 2;
        var z = row;
        var y = -x - z;

        return new Vector3(x, y, z);
    }
}
