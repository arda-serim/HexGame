using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Unit.Teams playerTeam;

    [SerializeField]GameObject attackingDisplay;

    public Tile tileOnMouseOver;

    void Awake()
    {
        playerTeam = Unit.Teams.Red;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {

        }
    }
}
