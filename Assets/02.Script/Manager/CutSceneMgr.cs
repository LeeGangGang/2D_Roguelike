using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class CutSceneMgr : MonoBehaviour
{
    private PlayableDirector PD;
    private TimelineAsset TA;
    private Object Cam;
    [SerializeField] private Text InfoText;

    // Start is called before the first frame update
    void Start()
    {
        PD = GetComponent<PlayableDirector>();
        TA = PD.playableAsset as TimelineAsset;
        Cam = Camera.main.gameObject as Object;
        foreach (var track in TA.GetOutputTracks())
        {
            if (track is CinemachineTrack)
                PD.SetGenericBinding(track, Cam);
        }

        GameObject.Find("GameManager").GetComponent<GameManager>().PlayerUIOnOff(false);
    }

    public void Play()
    {
        SoundManager.Inst.PlayBGM("BGM/Stage1_Boss");

        PD.Play();
    }

    public void ReceiveEndSignal()
    {
        Time.timeScale = 1f;
        GameObject.Find("GameManager").GetComponent<GameManager>().PlayerUIOnOff(true);
        Camera.main.orthographicSize = 5f;
    }

    public void TypingInfoText()
    {
        InfoText.text = string.Empty;

        string strInfo = "Statge1 : Boss";
        StartCoroutine(TextTyping(strInfo));
    }

    IEnumerator TextTyping(string infoTxt)
    {
        for (int i = 0; i < infoTxt.Length; i++)
        {
            yield return new WaitForSecondsRealtime(0.05f);
            InfoText.text += infoTxt[i];
        }
    }
}
