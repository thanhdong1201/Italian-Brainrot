using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    [SerializeField] private MainMenuManager mainMenuManager;
    [Header("Buttons")]
    [SerializeField] private Button privacyPolicyBtn;
    [SerializeField] private Button contactSupportBtn;
    [SerializeField] private Button quitBtn;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI versionText;

    private void Start()
    {
        InitializeButtons();
        versionText.text = $"Version: {Application.version}";
    }
    private void InitializeButtons()
    {
        privacyPolicyBtn.onClick.AddListener(OpenPrivacyPolicy);
        contactSupportBtn.onClick.AddListener(ContactSupport);
        quitBtn.onClick.AddListener(() => mainMenuManager.ShowPanel(UIMenuPanel.Menu));
    }
    public void OpenPrivacyPolicy()
    {
        Application.OpenURL("https://sites.google.com/view/italianbrainrot-iqmystery/privacy-policy");
    }
    public void ContactSupport()
    {
        string email = "binshincute.2001@gmail.com";
        string subject = Uri.EscapeDataString("Support Request - Italian Brainrot: IQ Mystery");
        string body = Uri.EscapeDataString("Please describe your issue here...");

        Application.OpenURL($"mailto:{email}?subject={subject}&body={body}");
    }
}
