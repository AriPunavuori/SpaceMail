using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI uiTextfield1;
    public TextMeshProUGUI uiTextfield2;
  
    public AudioSource[] audioSources;

    public Button buttonMenu;
    public Button buttonGame;
    public Player player;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        buttonMenu = GameObject.Find("MenuButton").GetComponent<Button>();
        buttonMenu.onClick.AddListener(GameLoad);
        audioSources[0].Play();
    }

    public void GameLoad()
    {
        buttonMenu.onClick.RemoveAllListeners();
        SceneManager.LoadScene(1);
    }

    public void GameStart()
    {
        buttonGame = GameObject.Find("GameButton").GetComponent<Button>();
        buttonGame.onClick.AddListener(GameManager.Instance.BTN_Intro);

        uiTextfield1 = GameObject.Find("TextUI1").GetComponent<TextMeshProUGUI>();
        uiTextfield1.text = "Can you help me to deliver this mail?";

        uiTextfield2 = GameObject.Find("TextUI2").GetComponent<TextMeshProUGUI>();
        uiTextfield2.text = "'tap to send me on my way!'";
    }

    public void GameWon()
    {
        audioSources[1].Stop();
        audioSources[4].Play();
        Debug.Log("Game Won!");
        uiTextfield1.text = "You helped me BIGTIME,\nThank You!!!";
        uiTextfield2.text = "'Tap to help again'";
        buttonGame.onClick.RemoveAllListeners();
        buttonGame.onClick.AddListener(BTN_Restart);
    }

    public void GameOver()
    {
        audioSources[1].Stop();
        audioSources[5].Play();
        Debug.Log("Game Over!");
        uiTextfield1.text = "Oh my gosh,\nYou sent me to the VOID!!!";
        uiTextfield2.text = "'Tap to try again'";
        buttonGame.onClick.RemoveAllListeners();
        buttonGame.onClick.AddListener(BTN_Restart);
    }

    public void BTN_Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BTN_Intro()
    {
        audioSources[0].Stop();
        audioSources[1].Play();
        uiTextfield1.text = "";
        uiTextfield2.text = "";
        buttonGame.onClick.RemoveAllListeners();
        buttonGame.onClick.AddListener(player.Launch);
        player.Launch();
    }
}
