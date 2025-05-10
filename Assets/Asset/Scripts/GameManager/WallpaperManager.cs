using System.Collections.Generic;
using UnityEngine;

public class WallpaperManager : MonoBehaviour
{
    public static WallpaperManager Instance { get; private set; }
    [SerializeField] private Transform wallPaperHolder;
    private List<GameObject> wallpapers;

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
        }

        int savedIndex = SaveManager.LoadInt("WallpaperIndex", 0);
        wallpapers = new List<GameObject>();
        for (int i = 0; i < wallPaperHolder.childCount; i++)
        {
            wallpapers.Add(wallPaperHolder.GetChild(i).gameObject);
            wallpapers[i].SetActive(i == savedIndex);
        }
    }
    [Button]
    public void ChangeWallpaper()
    {
        int randomIndex = Random.Range(0, wallpapers.Count);
        for (int i = 0; i < wallpapers.Count; i++)
        {
            wallpapers[i].SetActive(i == randomIndex);
        }
        SaveManager.SaveInt("WallpaperIndex", randomIndex);
    }
}
