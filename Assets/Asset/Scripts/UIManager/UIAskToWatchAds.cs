using UnityEngine;
using UnityEngine.UI;

public class UIAskToWatchAds : MonoBehaviour
{
    [SerializeField] private Button watchAdsBtn;
    [SerializeField] private Button ignoreBtn;
    [SerializeField] private CharacterManager characterManager;
    private CharacterSO characterSO;
    private  IUIAnimation iUIAnimation;

    private void Awake()
    {
        iUIAnimation = GetComponent<IUIAnimation>();
        watchAdsBtn.onClick.AddListener(() => WatchAds());
        ignoreBtn.onClick.AddListener(() => iUIAnimation.PlayCloseAnimation());
    }
    private void WatchAds()
    {
        AdManager.Instance.ShowRewardedAd(() =>
        {
            UnlockCharacter();
            iUIAnimation.PlayCloseAnimation();
            AnalyticsManager.Instance.LogAdImpression("rewarded");
            AnalyticsManager.Instance.LogRewardedAdCompleted("rewarded_unlock_character");
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
