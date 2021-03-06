using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfigBoxCtrl : MonoBehaviour
{
    public bool EnableHomeBtn = true;

    [SerializeField] private Button LobbyBtn;
    [SerializeField] private Button SaveBtn;
    [SerializeField] private Button CloseBtn;

    private bool IsBgmOn = true;
    private bool IsEffOn = true;
    [SerializeField] private Toggle UseBgmTog;
    [SerializeField] private Toggle UseEffTog;

    [SerializeField] private Slider BgmSlider;
    [SerializeField] private Slider EffSlider;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;

        if (!ReferenceEquals(LobbyBtn, null))
        {
            if (EnableHomeBtn)
                LobbyBtn.onClick.AddListener(GoLobby);
            else
                LobbyBtn.gameObject.SetActive(false);
        }
        if (!ReferenceEquals(SaveBtn, null))
            SaveBtn.onClick.AddListener(Save);
        if (!ReferenceEquals(CloseBtn, null))
            CloseBtn.onClick.AddListener(Close);

        if (!ReferenceEquals(UseBgmTog, null))
        {
            UseBgmTog.isOn = ConfigValue.UseBgmSound == 1;
            UseBgmTog.onValueChanged.AddListener(UseBgmTog_ValueChanged);
        }
        if (!ReferenceEquals(UseEffTog, null))
        {
            UseEffTog.isOn = ConfigValue.UseEffSound == 1;
            UseEffTog.onValueChanged.AddListener(UseEffTog_ValueChanged);
        }

        if (!ReferenceEquals(BgmSlider, null))
        {
            BgmSlider.value = ConfigValue.BgmSdVolume;
            BgmSlider.onValueChanged.AddListener(BgmSlider_ValueChanged);
        }
        if (!ReferenceEquals(EffSlider, null))
        {
            EffSlider.value = ConfigValue.EffSdVolume;
            EffSlider.onValueChanged.AddListener(EffSlider_ValueChanged);
        }
    }

    private void GoLobby()
    {
        SceneManager.LoadSceneAsync("FadeScene");
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Additive);
        Time.timeScale = 1f;
    }

    public void Save()
    {
        ConfigValue.UseBgmSound = IsBgmOn ? 1 : 0;
        PlayerPrefs.SetInt("SoundOnOff_Bgm", ConfigValue.UseBgmSound);
        ConfigValue.BgmSdVolume = BgmSlider.value;
        PlayerPrefs.SetFloat("SoundVolume_Bgm", ConfigValue.BgmSdVolume);

        ConfigValue.UseEffSound = IsEffOn ? 1 : 0;
        PlayerPrefs.SetInt("SoundOnOff_Eff", ConfigValue.UseEffSound);
        ConfigValue.EffSdVolume = EffSlider.value;
        PlayerPrefs.SetFloat("SoundVolume_Eff", ConfigValue.EffSdVolume);

        Time.timeScale = 1f;
        Destroy(this.gameObject);
    }

    public void Close()
    {
        // Revert UI
        UseBgmTog.isOn = ConfigValue.UseBgmSound == 1;
        UseEffTog.isOn = ConfigValue.UseEffSound == 1;
        BgmSlider.value = ConfigValue.BgmSdVolume;
        EffSlider.value = ConfigValue.EffSdVolume;

        SoundManager.Inst.SoundOnOff_Bgm(ConfigValue.UseBgmSound == 1);
        SoundManager.Inst.SoundVolume_Bgm(ConfigValue.BgmSdVolume);

        SoundManager.Inst.SoundOnOff_Eff(ConfigValue.UseEffSound == 1);
        SoundManager.Inst.SoundVolume_Eff(ConfigValue.EffSdVolume);

        Time.timeScale = 1f;
        Destroy(this.gameObject);
    }

    void UseBgmTog_ValueChanged(bool isOn)
    {
        IsBgmOn = isOn;
        SoundManager.Inst.SoundOnOff_Bgm(IsBgmOn);
    }

    void UseEffTog_ValueChanged(bool isOn)
    {
        IsEffOn = isOn;
        SoundManager.Inst.SoundOnOff_Eff(IsEffOn);
    }

    void BgmSlider_ValueChanged(float volume)
    {
        SoundManager.Inst.SoundVolume_Bgm(volume);
    }

    void EffSlider_ValueChanged(float volume)
    {
        SoundManager.Inst.SoundVolume_Eff(volume);
    }
}
