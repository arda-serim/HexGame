using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Unit.Teams playerTeam;

    Camera camera;

    public Tile tileOnMouseOver;

    void Awake()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();   
    }

    void Update()
    {

    }
}
