using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
   public Button NextButton;


   void Start()
   {
    NextButton.onClick.AddListener(OnNextButtonClicked);
   }

   void OnNextButtonClicked()
   {
    SceneManager.LoadScene("200_GameplaySandbox");
   }


}
