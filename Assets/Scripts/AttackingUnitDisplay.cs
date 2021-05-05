using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttackingUnitDisplay : MonoBehaviour
{
    public Unit.Teams team;
    public Unit.UnitTypes type;
    public int size;

    List<Color> colors = new List<Color>();
    [SerializeField]List<Sprite> sprites = new List<Sprite>();

    [SerializeField] SpriteRenderer spriteRendererForTile;
    [SerializeField] TextMeshPro textMesh;
    [SerializeField] SpriteRenderer spriteRendererForUnitType;

    void Start()
    {
        //Adding colors to "colors"
        colors.Add(Color.black);
        colors.Add(Color.red);
        colors.Add(Color.blue);
        colors.Add(Color.green);
        colors.Add(Color.yellow);

        spriteRendererForTile.color = colors[team.GetHashCode()];
        textMesh.text = size.ToString();
        spriteRendererForUnitType.sprite = sprites[type.GetHashCode()];
    }
}
