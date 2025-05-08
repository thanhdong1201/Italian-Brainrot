//using UnityEngine;

//public class MenuBannerAd : MonoBehaviour
//{
//    private void Start()
//    {
//        if (AdManager.Instance != null && AdManager.Instance.IsAdMobInitialized)
//        {
//            AdManager.Instance.ShowBanner();
//        }
//        else
//        {
//            AdManager.Instance.WaitForInitialization(() =>
//            {
//                AdManager.Instance.ShowBanner();
//            });
//        }
//    }

//    private void OnDestroy()
//    {
//        AdManager.Instance?.HideBanner();
//    }
//}
