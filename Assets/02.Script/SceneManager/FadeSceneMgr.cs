using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeSceneMgr : MonoBehaviour
{
    // CanvasGroup 저장할 변수
    public CanvasGroup FadeCanvasGroup;

    // Fade In 처리 시간
    [Range(0.5f, 2.0f)]
    public float FadeDuration = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        // 처음 불투명 알파값 설정
        FadeCanvasGroup.alpha = 1.0f;

        //Fade In 함수 호출
        StartCoroutine(Fade(0.0f));
    }

    // Fade In/Out 시키는 함수
    IEnumerator Fade(float finalAlpha)
    {
        FadeCanvasGroup.blocksRaycasts = true;

        float fadeSpeed = Mathf.Abs(FadeCanvasGroup.alpha - finalAlpha) / FadeDuration;

        while (!Mathf.Approximately(FadeCanvasGroup.alpha, finalAlpha))
        {
            FadeCanvasGroup.alpha = Mathf.MoveTowards(FadeCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        FadeCanvasGroup.blocksRaycasts = false;

        // fade In이 완료되면 SceneLoader 씬은 삭제
        SceneManager.UnloadSceneAsync("FadeScene");
    }
}
