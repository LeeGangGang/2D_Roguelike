using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeSceneMgr : MonoBehaviour
{
    // CanvasGroup ������ ����
    [SerializeField] private CanvasGroup FadeCanvasGroup;

    // Fade In ó�� �ð�
    [Range(0.5f, 1.0f)]
    private float FadeDuration = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        // ó�� ������ ���İ� ����
        FadeCanvasGroup.alpha = 1.0f;

        //Fade In �Լ� ȣ��
        StartCoroutine(Fade(0.0f));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            StartCoroutine(Fade(0.0f));
    }

    // Fade In/Out ��Ű�� �Լ�
    IEnumerator Fade(float finalAlpha)
    {
        FadeCanvasGroup.blocksRaycasts = true;

        float fadeSpeed = Mathf.Abs(FadeCanvasGroup.alpha - finalAlpha) / FadeDuration;

        while (!Mathf.Approximately(FadeCanvasGroup.alpha, finalAlpha))
        {
            FadeCanvasGroup.alpha = Mathf.MoveTowards(FadeCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.fixedDeltaTime);
            yield return null;
        }

        FadeCanvasGroup.blocksRaycasts = false;

        // fade In�� �Ϸ�Ǹ� SceneLoader ���� ����
        SceneManager.UnloadSceneAsync("FadeScene");
    }
}
