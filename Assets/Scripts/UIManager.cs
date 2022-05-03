using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
   private static UIManager _instance;

   public static UIManager Instance
   {
      get
      {
         return _instance;
      }
   }

   private void Awake()
   {
      if (_instance == null)
      {
         _instance = this;
      }
   }

   public void OnClick4Player()
   {
      SceneManager.LoadScene("SampleScene");
   }

   public void OnQuitGame()
   {
      Application.Quit();
   }
}
