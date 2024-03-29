using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttackingUnitDisplay : MonoBehaviour
{
   public Unit unit;

   List<Color> colors = new List<Color>();
   [SerializeField] List<Sprite> sprites = new List<Sprite>();

   public SpriteRenderer spriteRendererForTile;
   [SerializeField] public TextMeshPro textMesh;
   [SerializeField] SpriteRenderer spriteRendererForUnitType;

   void Start()
   {
      //Adding colors to "colors" list
      colors.Add(Color.black);
      colors.Add(Color.red);
      colors.Add(Color.blue);
      colors.Add(Color.green);
      colors.Add(Color.yellow);

      spriteRendererForTile.color = colors[unit.team.GetHashCode()];
      textMesh.text = unit.size.ToString();
      spriteRendererForUnitType.sprite = sprites[unit.type.GetHashCode()];
   }

   private void Update()
   {
      if (int.Parse(textMesh.text) <= 0)
      {
         if (unit.type == Unit.Types.Castle)
         {
            GameManager.Instance.DestroyAttackingDisplayTiles(unit.team);
         }
         Destroy(gameObject);
      }
   }
}
