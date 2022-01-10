using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    #region Singleton
    private static GameManagement instance;
    public static GameManagement Instance { get { return instance; } }

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

    #region GameStates
    public enum GameStates
    {
        INMENU,
        START,
        WIN,
        LOSE
    }
    public GameStates GameState { get { return gameState; } }
    private GameStates gameState = GameStates.INMENU;

    private int money, betMoney;

    public void StartTheGame()
    {
        gameState = GameStates.START;

        PlayerController.Instance.StartRun();
    }

    public void WinTheGame()
    {
        gameState = GameStates.WIN;
    }

    public void LoseTheGame()
    {
        gameState = GameStates.LOSE;
    }
    #endregion

    public void EarnMoney(int moneyValue)
    {
        money += moneyValue;

        UIManager.Instance.UpdateMoneyText(money);
    }

    public void StartBet(float moneyPercentage)
    {
        float betMoneyTemp = money * (moneyPercentage / 100f);
        betMoney = (int)betMoneyTemp;

        UIManager.Instance.ShowBetPanel(betMoney);

        PlayerController.Instance.BetStart();
    }

    public void EndBet()
    {
        UIManager.Instance.HideBetPanel();

        PlayerController.Instance.BetEnd();

        betMoney = 0;
    }

    public void FindWinner(int playerDiceValue, int enemyDiceValue)
    {
        if (playerDiceValue > enemyDiceValue)
        {
            EarnMoney(betMoney);

        }
        else if (playerDiceValue < enemyDiceValue)
        {
            EarnMoney(-betMoney);
        }
        else
        {
            EarnMoney(0);
        }

        EndBet();
    }
}
