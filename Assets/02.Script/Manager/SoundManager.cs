using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigValue
{
    public static int UseBgmSound = 1; // 0 : 사용 안함 , 1 : 사용함
    public static int UseEffSound = 1;
    public static float BgmSdVolume = 1f; // 0.0f ~ 1.0f
    public static float EffSdVolume = 1f;
}

public class SoundManager : MonoBehaviour
{
    // Bgm + UI AudioSource
    [HideInInspector] public AudioSource AudioSrc = null;
    // 모든 AudioClip 
    Dictionary<string, AudioClip> AudioClipList = new Dictionary<string, AudioClip>();

    [HideInInspector] public bool SoundOnOff = true;

    //효과음 버퍼
    int EffSdCount = 10; //최대 10번 플레이
    int CurSdIdx = 0;
    List<GameObject> SdObjList = new List<GameObject>();

    // Effect AudioSource
    List<AudioSource> SdSrcList = new List<AudioSource>();

    public static SoundManager Inst;
    private void Awake()
    {
        if (ReferenceEquals(Inst, null))
        {
            Inst = this;
            LoadChildGameObj();
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioClip clip = null;
        object[] temp = Resources.LoadAll("Sounds");
        for (int i = 0; i < temp.Length; i++)
        {
            clip = temp[i] as AudioClip;
            if (AudioClipList.ContainsKey(clip.name))
                continue;

            AudioClipList.Add(clip.name, clip);
        }

        ConfigValue.UseBgmSound = PlayerPrefs.GetInt("SoundOnOff_Bgm", 1);
        bool a_soundOnOff = (ConfigValue.UseBgmSound == 1);
        SoundOnOff_Bgm(a_soundOnOff);

        ConfigValue.UseEffSound = PlayerPrefs.GetInt("SoundOnOff_Eff", 1);
        a_soundOnOff = (ConfigValue.UseEffSound == 1);
        SoundOnOff_Eff(a_soundOnOff);

        ConfigValue.BgmSdVolume = PlayerPrefs.GetFloat("SoundVolume_Bgm", 1f);
        ConfigValue.EffSdVolume = PlayerPrefs.GetFloat("SoundVolume_Eff", 1f);
        SoundVolume_Bgm(ConfigValue.BgmSdVolume);
        SoundVolume_Eff(ConfigValue.EffSdVolume);
    }

    void LoadChildGameObj()
    {
        if (ReferenceEquals(this, null))
            return;

        AudioSrc = gameObject.AddComponent<AudioSource>();

        for (int i = 0; i < EffSdCount; i++)
        {
            GameObject newSdObj = new GameObject();
            newSdObj.transform.SetParent(transform);
            newSdObj.transform.localPosition = Vector3.zero;
            AudioSource audioSource = newSdObj.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            newSdObj.name = "SoundEffObj";

            SdSrcList.Add(audioSource);
            SdObjList.Add(newSdObj);
        }
    }

    public void PlayBGM(string fileName, bool loop = true)
    {
        if (!SoundOnOff)
            return;
        
        if (!AudioClipList.ContainsKey(fileName))
            AudioClipList.Add(fileName, Resources.Load("Sounds/" + fileName) as AudioClip);

        if (ReferenceEquals(AudioSrc, null))
            return;
        
        if (!ReferenceEquals(AudioSrc.clip, null) && AudioSrc.clip.name == fileName)
            return;

        AudioSrc.clip = AudioClipList[fileName];
        AudioSrc.loop = loop;
        AudioSrc.Play();
    }

    public void PlayUISound(string fileName, float volume = 1f)
    {
        if (!SoundOnOff)
            return;

        if (!AudioClipList.ContainsKey(fileName))
            AudioClipList.Add(fileName, Resources.Load("Sounds/" + fileName) as AudioClip);

        if (ReferenceEquals(AudioSrc, null))
            return;

        AudioSrc.PlayOneShot(AudioClipList[fileName], volume * ConfigValue.BgmSdVolume);
    }

    public void PlayEffSound(string fileName, float volume = 1f)
    {
        if (!SoundOnOff)
            return;

        if (!AudioClipList.ContainsKey(fileName))
            AudioClipList.Add(fileName, Resources.Load("Sounds/" + fileName) as AudioClip);

        if (!ReferenceEquals(AudioClipList[fileName], null) && !ReferenceEquals(SdSrcList[CurSdIdx], null))
        {
            SdSrcList[CurSdIdx].clip = AudioClipList[fileName];
            SdSrcList[CurSdIdx].loop = false;
            SdSrcList[CurSdIdx].volume = volume;
            SdSrcList[CurSdIdx].Play();

            CurSdIdx++;
            if (EffSdCount <= CurSdIdx)
                CurSdIdx = 0;
        }
    }

    public void SoundOnOff_Bgm(bool soundOn = true)
    {
        if (!ReferenceEquals(AudioSrc, null))
            AudioSrc.mute = !soundOn;
    }

    public void SoundOnOff_Eff(bool soundOn = true)
    {
        for (int i = 0; i < SdSrcList.Count; i++)
        {
            if (!ReferenceEquals(SdSrcList[i], null))
                SdSrcList[i].mute = !soundOn;
        }
    }

    public void SoundVolume_Bgm(float volume)
    {
        if (!ReferenceEquals(AudioSrc, null))
            AudioSrc.volume = volume;
    }

    public void SoundVolume_Eff(float volume)
    {
        for (int i = 0; i < SdSrcList.Count; i++)
            SdSrcList[i].volume = volume;
    }
}