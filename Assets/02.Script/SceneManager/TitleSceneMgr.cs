using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneMgr : MonoBehaviour
{
    public Button PlayBtn;
    public Button ExitBtn;

    // Start is called before the first frame update
    void Start()
    {
        if (!ReferenceEquals(PlayBtn, null))
            PlayBtn.onClick.AddListener(GameStart);

        if (!ReferenceEquals(ExitBtn, null))
            ExitBtn.onClick.AddListener(GameExit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GameStart()
    {
        SceneManager.LoadSceneAsync("FadeScene");
        SceneManager.LoadScene("MainScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("Stage1_1", LoadSceneMode.Additive);
    }

    void GameExit()
    {
        Application.Quit();
    }
}
