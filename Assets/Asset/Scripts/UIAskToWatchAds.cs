using UnityEngine;
using UnityEngine.UI;

public class UIAskToWatchAds : MonoBehaviour
{
    [SerializeField] private Button watchAdsBtn;
    [SerializeField] private Button ignoreBtn;
    [SerializeField] private CharacterManager characterManager;
    private CharacterSO characterSO;
    private  UIScaleAnimation uIScaleAnimation;

    private void Awake()
    {
        uIScaleAnimation = GetComponent<UIScaleAnimation>();
        watchAdsBtn.onClick.AddListener(() => WatchAds());
        ignoreBtn.onClick.AddListener(() => uIScaleAnimation.PlayCloseAnimation());
    }
    private void WatchAds()
    {
        AdManager.Instance.ShowRewardedAd(() =>
        {
            AnalyticsManager.Instance.LogAdImpression("rewarded");
            AnalyticsManager.Instance.LogRewardedAdCompleted("rewarded_unlock_character");
            uIScaleAnimation.PlayCloseAnimation();
            UnlockCharacter();
        });
    }
    private void UnlockCharacter()
    {
        if (characterSO == null) return; 
        characterSO.Unlock();
        characterManager.UpdateUICharacters();
    }
    public void SetCharacter(CharacterSO characterSO)
    {
        this.characterSO = characterSO;
    }
}
