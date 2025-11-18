using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{

    public void LoadLevel()
    {
        //SceneManager.LoadScene("Level 2");
    }

    public void LoadNewLevel()
    {
        //StartCoroutine(LoadAfterWait("Level 2", 3.0f));
    }

    //IEnumerator LoadAfterWait(string scene, float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    SceneManager.LoadScene(scene);
    //}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
