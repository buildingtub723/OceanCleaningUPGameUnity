using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
   public Button NextButton;
   public Button QuitButton;
   public Button CreditsButton;


   void Start()
   {
    NextButton.onClick.AddListener(OnNextButtonClicked);
    QuitButton.onClick.AddListener(OnQuitButtonClicked);
    CreditsButton.onClick.AddListener(OnCreditsButtonClicked);
   }

   void OnNextButtonClicked()
   {
    SceneManager.LoadScene("Main Level");
   }

   void OnQuitButtonClicked()
   {
    Application.Quit();
   }

   void OnCreditsButtonClicked()
   {
    SceneManager.LoadScene("101_Credits");
   }


}
