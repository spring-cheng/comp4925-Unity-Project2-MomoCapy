using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class GameLogicCode : MonoBehaviour
{
    [Header("Input Fields")]
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] TMP_InputField username_input;
    [SerializeField] TMP_InputField password_input;

    [Space(10)]
    [Header("Prefabs")]
    [SerializeField] GameObject cat_prefab;

    [Space(10)]
    [Header("Variables")]
    //[Range(0.1f, 20f)][SerializeField] float period = 1.0f;
    //private float nextActionTime = 0.0f;

    private AudioPlayer player;
    private LevelManager levelManager;

    //[SerializeField] ParticleSystem particles = null;

    private void Awake()
    {
        player = FindAnyObjectByType<AudioPlayer>();
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        label.text = "Welcome to the cat shelter!";
    }

    // Update is called once per frame
    void Update()
    {

    }

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


    public IEnumerator getWebContent()
    {
        string url = "http://localhost:3000/";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        label.text = request.downloadHandler.text;
    }

    public IEnumerator postWebContent()
    {
        string url = "http://localhost:3000/";

        WWWForm form = new WWWForm();
        form.AddField("username", username_input.text);
        form.AddField("password", password_input.text);

        UnityWebRequest request = UnityWebRequest.Post(url, form);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            label.text = "Error: " + request.error;
        }
        else
        {
            label.text = request.downloadHandler.text;
        }
    }

    public IEnumerator registerUser()
    {
        string url = "http://localhost:3000/register";

        WWWForm form = new WWWForm();
        form.AddField("username", username_input.text);
        form.AddField("password", password_input.text);

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();

        label.text = request.downloadHandler.text;
    }

    public IEnumerator loginUser()
    {
        string url = "http://localhost:3000/login";

        WWWForm form = new WWWForm();
        form.AddField("username", username_input.text);
        form.AddField("password", password_input.text);

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();

        label.text = request.downloadHandler.text;
    }

}
