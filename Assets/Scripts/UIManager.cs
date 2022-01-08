using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance { get { return instance; } }
    private static UIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    [SerializeField] GameObject TapToStartPanel;
    [SerializeField] GameObject GameInPanel;
    [SerializeField] Image expImage;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject LosePanel;

    public void StartTheGame()
    {
        GameManagement.Instance.StartTheGame();

        GameInPanel.SetActive(true);

        TapToStartPanel.SetActive(false);
    }

    public void WinTheGame()
    {
        GameManagement.Instance.WinTheGame();

        WinPanel.SetActive(true);

        GameInPanel.SetActive(false);
    }

    public void LoseTheGame()
    {
        GameManagement.Instance.LoseTheGame();

        LosePanel.SetActive(true);

        GameInPanel.SetActive(false);
    }

    public void UpdateExpImageFillAmount(float fillAmount)
    {
        DOTween.To(() => expImage.fillAmount, x => expImage.fillAmount = x, fillAmount, 0.25f);
    }

    public void UpdateMoneyText(int money)
    {
        moneyText.SetText(money.ToString());
    }
}
