using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI uiTextfield;
    public Button button;
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

    public void GameStart()
    {
        button = GameObject.Find("LaunchButton").GetComponent<Button>();
        button.onClick.AddListener(GameManager.Instance.BTN_Intro);

        uiTextfield = GameObject.Find("TextUI").GetComponent<TextMeshProUGUI>();
        uiTextfield.text = "Can you help me to deliver this mail?\n\n(tap screen to send me on my way!)";
    }

    public void GameWon()
    {
        Debug.Log("Game Won!");
        uiTextfield.text = "You helped me BIGTIME,\nThank You!!!\n\n(Tap to try again)";
        button.onClick.AddListener(BTN_Restart);
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        uiTextfield.text = "Oh my gosh,\nYou sent me to the VOID!!!\n\n(Tap to try again)";
        button.onClick.AddListener(BTN_Restart);
    }

    public void BTN_Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BTN_Intro()
    {
        uiTextfield.text = "";
        button.onClick.AddListener(player.Launch);
        player.Launch();
    }
}
