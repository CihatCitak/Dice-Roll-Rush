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
        if(instance == null)
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

    public enum GameStates
    {
        INMENU,
        START,
        WIN,
        LOSE
    }
    public GameStates GameState { get { return gameState; } }
    private GameStates gameState = GameStates.INMENU;

    private int money;

    public void StartTheGame()
    {
        gameState = GameStates.START;
    }

    public void WinTheGame()
    {
        gameState = GameStates.WIN;
    }

    public void LoseTheGame()
    {
        gameState = GameStates.LOSE;
    }

    public void EarnMoney(int moneyValue)
    {
        money += moneyValue;

        UIManager.Instance.UpdateMoneyText(money);
    }

}
