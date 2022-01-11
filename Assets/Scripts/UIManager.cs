using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject LosePanel;
    [SerializeField] GameObject GameInPanel;
    [SerializeField] Image expImage;
    [Header("Bet Panel")]
    [SerializeField] GameObject betPanel;
    [SerializeField] TextMeshProUGUI betMoneyText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] GameObject betWinText;
    [SerializeField] GameObject betLoseText;
    [SerializeField] GameObject betDrawText;

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

    public void ShowBetPanel(int betMoney)
    {
        betPanel.SetActive(true);

        betMoneyText.SetText(betMoney.ToString());
    }

    public void HideBetPanel()
    {


        StartCoroutine(SetFalseBetTextsSetActive());
    }

    public void WinTheBet()
    {
        betWinText.SetActive(true);
    }

    public void LoseTheBet()
    {
        betLoseText.SetActive(true);
    }

    public void DrawTheBet()
    {
        betDrawText.SetActive(true);
    }

    private IEnumerator SetFalseBetTextsSetActive()
    {
        yield return new WaitForSeconds(2f);

        betPanel.SetActive(false);

        betDrawText.SetActive(false);

        betWinText.SetActive(false);

        betLoseText.SetActive(false);
    }
}
