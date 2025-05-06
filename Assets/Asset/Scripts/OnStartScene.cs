using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartScene : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;
        SoundManager.Instance.StopMusic();
    }
}
