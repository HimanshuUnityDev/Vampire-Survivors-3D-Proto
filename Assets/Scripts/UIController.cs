using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] Slider playerExperienceSlider;
    [SerializeField] TextMeshProUGUI experienceText;
    public static UIController instance;
    public GameObject LevelUpPanel;
    [SerializeField] AudioSource musicSource;

    public LevelUpButton[] LevelUpButtons;
    public void Awake()
    {
        instance = this;
    }

    public void UpdateExperienceSlider()
    {
        playerExperienceSlider.maxValue = Player_Controller.instance.playerLevels[Player_Controller.instance.currentLevel - 1];
        playerExperienceSlider.value = Player_Controller.instance.experience;
        experienceText.text = playerExperienceSlider.value + " / " + playerExperienceSlider.maxValue;
/*        Debug.Log("Exp: " + Player_Controller.instance.experience);
*/    }
    public void LevelUpPanelOpen()
    {
        LevelUpPanel.SetActive(true);
        Time.timeScale = 0f;
       // musicSource.Pause();

    }
    public void LevelUpPanelClose()
    {
        LevelUpPanel.SetActive(false);
        Time.timeScale = 1f;
       // musicSource.UnPause();  
    }

}
