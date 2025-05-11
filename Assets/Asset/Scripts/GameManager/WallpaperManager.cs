using System.Collections.Generic;
using UnityEngine;

public class WallpaperManager : MonoBehaviour
{
    public static WallpaperManager Instance { get; private set; }
    [SerializeField] private Transform wallPaperHolder;
    private List<GameObject> wallpapers;
    private int currentIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        currentIndex = SaveManager.LoadInt("WallpaperIndex", 0);
        wallpapers = new List<GameObject>(wallPaperHolder.childCount);

        for (int i = 0; i < wallPaperHolder.childCount; i++)
        {
            GameObject wallpaper = wallPaperHolder.GetChild(i).gameObject;
            wallpapers.Add(wallpaper);
            wallpaper.SetActive(i == currentIndex);
        }
    }

    [Button]
    public void ChangeWallpaper()
    {
        int newIndex = GetRandomIndexExcludingCurrent();
        foreach (GameObject wallpaper in wallpapers)
        {
            wallpaper.SetActive(false);
        }
        wallpapers[newIndex].SetActive(true);
        currentIndex = newIndex;
        SaveManager.SaveInt("WallpaperIndex", currentIndex);
    }

    private int GetRandomIndexExcludingCurrent()
    {
        int newIndex;
        do
        {
            newIndex = Random.Range(0, wallpapers.Count);
        } while (newIndex == currentIndex && wallpapers.Count > 1);
        return newIndex;
    }
}