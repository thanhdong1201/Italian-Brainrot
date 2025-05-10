using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(SetVolume);
    }

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        slider.value = savedVolume;
    }

    private void SetVolume(float value)
    {
        SoundManager.Instance.SetVolume(value);
    }
}
