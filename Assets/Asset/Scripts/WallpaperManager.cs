using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallpaperManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> wallpapers;

    private void OnEnable()
    {
        ChangeWallpaper();
    }
    private void ChangeWallpaper()
    {
        int randomIndex = Random.Range(0, wallpapers.Count);
        for (int i = 0; i < wallpapers.Count; i++)
        {
            wallpapers[i].SetActive(i == randomIndex);
        }
    }
}
