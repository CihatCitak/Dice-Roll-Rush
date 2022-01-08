using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelChangeType
{
    LEVELUP, LEVELDOWN
}

public class LevelManager : MonoBehaviour
{
    #region Singleton
    public static LevelManager Instance { get { return instance; } }
    private static LevelManager instance;

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

    [SerializeField] List<DiceSettings> diceSettings;

    DiceSettings currentDiceSetting;

    private int currentExp, maxExp, currentLevel, maxLevel;

    private const int EXPUP_VALUE = 2;
    private const int EXPDOWN_VALUE = -2;

    void Start()
    {
        currentLevel = 0; maxLevel = diceSettings.Count - 1;

        SetCurrentDiceSettings(diceSettings[currentLevel]);

        SetCurrentAndMaxExp(0, currentDiceSetting.Exp, LevelChangeType.LEVELUP);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("ExpUp"))
        {
            OnPickupObject(EXPUP_VALUE);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("ExpDown"))
        {
            OnPickupObject(EXPDOWN_VALUE);
            Destroy(other.gameObject);
        }
    }

    #region  PickupObject Issues
    //Pickup the object object can be positive or negative
    private void OnPickupObject(int exp)
    {
        AddExpToCurrentExp(exp);

        if (IsCurrentExpBiggerThenEqualMaxExp())
        {
            //Increase currentLevel if it is bigger then maxLevel do nothing.
            IncreaseCurrentLevel();
        }
        else if (IsCurrentExpLessThenEqualZero())
        {
            //Decrease currentLevel if it is less then 0 we are dead.
            DecreaseCurrentLevel();
        }
    }

    #region Exp Add and Exp Limit Checks
    private void AddExpToCurrentExp(int exp)
    {
        currentExp += exp;

        SetExpBarFillAmount();
    }

    private bool IsCurrentExpBiggerThenEqualMaxExp()
    {
        if (currentExp >= maxExp)
            return true;
        else
            return false;
    }

    private bool IsCurrentExpLessThenEqualZero()
    {
        if (currentExp <= 0)
            return true;
        else
            return false;
    }
    #endregion

    #region Level Increase, Decrease And Level Limits Checks
    private void IncreaseCurrentLevel()
    {
        if(currentLevel < maxLevel)
        {
            currentLevel++;

            //Level Up
            NewLevelSetup(LevelChangeType.LEVELUP);
        }
    }

    private void DecreaseCurrentLevel()
    {
        if (currentLevel > 0)
        {
            currentLevel--;

            //Level Down
            NewLevelSetup(LevelChangeType.LEVELDOWN);
        }
        else
        {
            //Öldük;
            currentExp = 0;
        }
    }

    #endregion

    #region New Level Setups

    private void NewLevelSetup(LevelChangeType levelChangeType)
    {
        SetCurrentDiceSettings(diceSettings[currentLevel]);

        SetCurrentAndMaxExp(0, currentDiceSetting.Exp, levelChangeType);

        SetExpBarFillAmount();

        PlayerController.Instance.OnNewLevel(currentDiceSetting, levelChangeType);
    }

    private void SetCurrentDiceSettings(DiceSettings diceSettings)
    {
        currentDiceSetting = diceSettings;
    }

    private void SetCurrentAndMaxExp(int current, int max, LevelChangeType levelChangeType)
    {
        if(levelChangeType == LevelChangeType.LEVELUP)
        {
            currentExp = current; maxExp = max;
        }
        else
        {
            currentExp = max - 2; maxExp = max;
        }
    }

    private float CalculateExpBarFillAmount()
    {
        return (float)(currentExp) / (float)maxExp;
    }

    private void SetExpBarFillAmount()
    {
        UIManager.Instance.UpdateExpImageFillAmount(CalculateExpBarFillAmount());
    }

    #endregion

    #endregion
}
