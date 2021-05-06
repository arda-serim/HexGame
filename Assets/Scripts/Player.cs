using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   public Unit.Teams team;

    public Tile tileOnMouseOver;
    public Tile selectedTile;

    void Awake()
    {
        team = Unit.Teams.Red;
    }

    void Update()
    {
        if (GameManager.Instance.canMove && GameManager.Instance.whosTurn != team)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (selectedTile == null)
            {
                if (team == tileOnMouseOver.unit.team)
                {
                    selectedTile = tileOnMouseOver;
                }
            }
            else
            {
                if (team == tileOnMouseOver.unit.team)
                {
                    selectedTile = tileOnMouseOver;
                }
                else if (selectedTile.Pathfind(tileOnMouseOver).Count <= 2)
                {
                    if (tileOnMouseOver.unit.type == Unit.Types.Empty)
                    {
                        Debug.Log("Found a tile can be gone, sir");
                        StartCoroutine(selectedTile.GoTo(tileOnMouseOver));
                    }
                    else if (team != tileOnMouseOver.unit.team && tileOnMouseOver.unit.team != Unit.Teams.Universal)
                    {
                        if (tileOnMouseOver.unit.type == Unit.Types.Castle)
                        {
                            Debug.Log("Found a Enemy Castle, sir");
                        }
                        else
                        {
                            Debug.Log("Found a Enemy attacker, sir");
                        }
                    }
                }
            }
        }
    }
}
