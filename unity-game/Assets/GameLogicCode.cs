using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class GameLogicCode : MonoBehaviour
{
    public static GameLogicCode Instance;

    [Header("Login UI (Scene 0 Only)")]
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] TMP_InputField username_input;
    [SerializeField] TMP_InputField password_input;

    [Header("Game UI (Scene 1 Only)")]
    public TextMeshProUGUI savedText;
    public TextMeshProUGUI missedText;
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Gameplay")]
    public int catsSaved = 0;
    public int boxesMissed = 0;
    public int catsToWin = 10;
    public int missesToLose = 5;

    private AudioPlayer player;
    private LevelManager levelManager;

    private bool gameOver = false;

    private void Awake()
    {
        Instance = this;
        player = FindAnyObjectByType<AudioPlayer>();
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    void Start()
    {
        if (label != null)
            label.text = "Welcome to the cat shelter!";

        UpdateUI();

        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    void Update()
    {

    }

    // called when cat touches a CatBox
    public void CatSaved()
    {
        if (gameOver) return;

        catsSaved++;
        Debug.Log("Cats saved: " + catsSaved);
        UpdateUI();

        if (catsSaved >= catsToWin)
            WinGame();
    }

    // called when CatBox hits death barrier
    public void BoxMissed()
    {
        if (gameOver) return;

        boxesMissed++;
        Debug.Log("Boxes missed: " + boxesMissed);
        UpdateUI();

        if (boxesMissed >= missesToLose)
            LoseGame();
    }

    private void UpdateUI()
    {
        if (savedText != null)
            savedText.text = "Saved: " + catsSaved;

        if (missedText != null)
            missedText.text = "Missed: " + boxesMissed;
    }

    private void WinGame()
    {
        gameOver = true;
        Time.timeScale = 0f;

        if (winPanel != null) winPanel.SetActive(true);
        StartCoroutine(SendScoreToBackend(true));
    }

    private void LoseGame()
    {
        gameOver = true;
        Time.timeScale = 0f;

        if (losePanel != null) losePanel.SetActive(true);
        StartCoroutine(SendScoreToBackend(false));
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [System.Serializable]
    private class ScorePayload
    {
        public int savedCats;
        public int missedBoxes;
        public bool won;
    }

    private IEnumerator SendScoreToBackend(bool won)
    {
        string url = "http://localhost:5000/score";

        var payload = new ScorePayload
        {
            savedCats = this.catsSaved,
            missedBoxes = this.boxesMissed,
            won = won
        };

        string json = JsonUtility.ToJson(payload);

        UnityWebRequest request = new UnityWebRequest(url, "POST");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Score upload failed: " + request.error);
        }
        else
        {
            Debug.Log("Score upload success: " + request.downloadHandler.text);
        }
    }

    // scene 0 login functions
    public void PressedButton()
    {
        label.text = "Sending...";
        StartCoroutine(postWebContent());
    }

    public void PressRegisterButton()
    {
        StartCoroutine(registerUser());
    }

    public void PressLoginButton()
    {
        StartCoroutine(loginUser());
    }

    public IEnumerator postWebContent()
    {
        string url = "http://localhost:5000/";
        WWWForm form = new WWWForm();
        form.AddField("username", username_input.text);
        form.AddField("password", password_input.text);

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            label.text = "Error: " + request.error;
        else
            label.text = request.downloadHandler.text;
    }

    public IEnumerator registerUser()
    {
        string url = "http://localhost:5000/register";
        WWWForm form = new WWWForm();
        form.AddField("username", username_input.text);
        form.AddField("password", password_input.text);

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
        label.text = request.downloadHandler.text;
    }

    public IEnumerator loginUser()
    {
        string url = "http://localhost:5000/login";
        WWWForm form = new WWWForm();
        form.AddField("username", username_input.text);
        form.AddField("password", password_input.text);

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();

        string result = request.downloadHandler.text;
        label.text = result;

        if (result.Contains("logged in"))
            SceneManager.LoadScene(1);
    }
}
