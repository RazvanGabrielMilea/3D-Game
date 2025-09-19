using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioOptions : MonoBehaviour
{
    [Header("References")]
    public AudioMixer mixer;       // drag in your GameAudioMixer
    public Slider soundSlider;     // your "Sound" (Master) slider
    public Slider musicSlider;     // your "Music" slider

    private const string MASTER_PARAM = "MasterVolume";
    private const string MUSIC_PARAM  = "MusicVolume";

    void Start()
    {
        // initialize slider positions from mixer
        float mVal, muVal;
        mixer.GetFloat(MASTER_PARAM, out mVal);
        mixer.GetFloat(MUSIC_PARAM,  out muVal);

        soundSlider.value = DecibelToLinear(mVal);
        musicSlider.value = DecibelToLinear(muVal);

        // hook up change events
        soundSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void SetMasterVolume(float linear)
    {
        mixer.SetFloat(MASTER_PARAM, LinearToDecibel(linear));
    }

    public void SetMusicVolume(float linear)
    {
        mixer.SetFloat(MUSIC_PARAM, LinearToDecibel(linear));
    }

    private float LinearToDecibel(float lin)
    {
        lin = Mathf.Clamp(lin, 0.0001f, 1f);
        return 20f * Mathf.Log10(lin);
    }

    private float DecibelToLinear(float dB)
    {
        return Mathf.Pow(10f, dB / 20f);
    }
}