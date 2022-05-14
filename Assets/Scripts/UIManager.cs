using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
   private static UIManager _instance;
   [SerializeField] GameObject howToPlay;
   bool isHowToPlayShown = false;

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
      howToPlay.gameObject.SetActive(false);
   }

   private void Update()
   {
      if (isHowToPlayShown)
      {
         if (Input.GetKeyDown(KeyCode.Escape))
         {
            howToPlay.gameObject.SetActive(false);
            isHowToPlayShown = false;
         }
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

   public void OnClickHowToPlay()
   {
      howToPlay.gameObject.SetActive(true);
      isHowToPlayShown = true;
   }
}
