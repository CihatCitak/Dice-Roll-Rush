using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inputs;
using DG.Tweening;
using TMPro;

public class PlayerController : MonoBehaviour
{
    #region Singleton
    public static PlayerController Instance { get { return instance; } }
    private static PlayerController instance;

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

    #region DiceStates
    public enum DiceStates
    {
        START,
        RUN,
        INBET,
        GAMEEND
    }
    private DiceStates diceState;

    public void StartRun()
    {
        diceState = DiceStates.RUN;
    }

    public void EndTheGame()
    {
        diceState = DiceStates.GAMEEND;
    }
    #endregion

    [SerializeField] InputSettings inputSettings;
    [Header("Movement")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float forwardMovementSpeed = 1f, sideMovementSensitivity = 1f;
    [Header("Level Settings")]
    [SerializeField] DiceSettings diceSetting;
    [SerializeField] Transform diceCanvas;
    [SerializeField] List<TextMeshProUGUI> diceValuesTexts;
    [SerializeField] ParticleSystem levelUpPartical;
    [SerializeField] ParticleSystem levelDownPartical;
    [Header("Bet Settup")]
    [SerializeField] LayerMask layerMask;
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] Vector3 diceThrowForce = Vector3.one;


    private GameObject currentCharacterModel;
    private int[] diceValues = new int[] { 1, 2, 3, 4, 5, 6 };
    private int diceValue;
    private bool isValueFound = false, isThrowed = false;
    private int enemyDiceValue;
    private BetArea lastBetarea;

    private void Start()
    {
        DestroyOldCharacterModel();
        InstantiateNewCharacterModelInPlayerObject();
    }


    private void Update()
    {
        if (diceState == DiceStates.INBET)
        {
            if (rb.velocity == Vector3.zero && rb.angularVelocity == Vector3.zero && !isValueFound && isThrowed)
            {
                //Find dice value create blocks 
                StartCoroutine(FindDiceValueAndWinner());
            }
        }
    }

    void FixedUpdate()
    {
        if (diceState == DiceStates.RUN)
            HandleForwardMovement();
    }

    private void HandleForwardMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x + sideMovementSensitivity * inputSettings.InputDrag.x, 0,
            forwardMovementSpeed);
    }

    #region LevelIssues
    public void OnNewLevel(DiceSettings settings, LevelChangeType levelChangeType)
    {
        ShowParticalEffectMakeItChildDestroyAfterOneSecond(levelChangeType);

        //Old Bonus Value Subtract
        SetDiceValues(-diceSetting.BonusDiceValue);

        SetCharacterSettings(settings);

        //New Bonus Value Add
        SetDiceValues(diceSetting.BonusDiceValue);

        SetDiceValueTexts();

        DestroyOldCharacterModel();

        InstantiateNewCharacterModelInPlayerObject();
    }

    private void SetCharacterSettings(DiceSettings settings)
    {
        diceSetting = settings;
    }

    private void InstantiateNewCharacterModelInPlayerObject()
    {
        currentCharacterModel =
            Instantiate(diceSetting.DiceModelPrefab, transform.position, transform.rotation);

        currentCharacterModel.transform.parent = transform;
    }

    private void ShowParticalEffectMakeItChildDestroyAfterOneSecond(LevelChangeType levelChangeType)
    {
        if (levelChangeType == LevelChangeType.LEVELUP)
        {
            levelUpPartical.Play();
        }
        else
        {
            levelDownPartical.Play();
        }
    }

    private void DestroyOldCharacterModel()
    {
        if (currentCharacterModel != null)
        {
            Destroy(currentCharacterModel);
        }
    }

    private void SetDiceValues(int diceBonusValue)
    {
        for (int i = 0; i < diceValues.Length; i++)
        {
            diceValues[i] += diceBonusValue;
        }
    }

    private void SetDiceValueTexts()
    {
        for (int i = 0; i < diceValues.Length; i++)
        {
            diceValuesTexts[i].SetText(diceValues[i].ToString());
        }
    }
    #endregion

    #region CollectMoney
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Money"))
        {
            StartCoroutine(WaitForCollectMoney());

            return;
        }
    }

    private IEnumerator WaitForCollectMoney()
    {
        yield return new WaitForSeconds(0.4f);

        GameManagement.Instance.EarnMoney(diceSetting.MoneyValue);
    }
    #endregion

    public void BetStart()
    {
        diceState = DiceStates.INBET;

        sphereCollider.enabled = false;

        boxCollider.enabled = true;

        DiceGoToMiddlePosition(0.5f);
    }

    public void BetEnd()
    {
        diceState = DiceStates.RUN;

        sphereCollider.enabled = true;

        boxCollider.enabled = false;
    }

    public Tween DiceGoToMiddlePosition(float duration)
    {
        return transform.DOLocalMoveX(0f, duration).OnComplete(() => ThrowDice());
    }

    #region DiceIssues
    private void ThrowDice()
    {
        isValueFound = false; isThrowed = true;
        rb.velocity = Vector3.zero;
        rb.AddForce(diceThrowForce, ForceMode.VelocityChange);
        rb.AddTorque(new Vector3(Random.Range(-2000f, 2000f), 0, 2000f));
    }

    private IEnumerator FindDiceValueAndWinner()
    {
        yield return new WaitForSeconds(0f);

        if (DrawRayFromDiceCenterAndCheckGroundTouch(diceCanvas.forward))
        {
            isValueFound = true; isThrowed = false;
            diceValue = diceValues[5];
            Debug.Log(diceValue);
        }
        else if (DrawRayFromDiceCenterAndCheckGroundTouch(-diceCanvas.forward))
        {
            isValueFound = true; isThrowed = false;
            diceValue = diceValues[0];
            Debug.Log(diceValue);
        }
        else if (DrawRayFromDiceCenterAndCheckGroundTouch(diceCanvas.up))
        {
            isValueFound = true; isThrowed = false;
            diceValue = diceValues[4];
            Debug.Log(diceValue);
        }
        else if (DrawRayFromDiceCenterAndCheckGroundTouch(-diceCanvas.up))
        {
            isValueFound = true; isThrowed = false;
            diceValue = diceValues[2];
            Debug.Log(diceValue);
        }
        else if (DrawRayFromDiceCenterAndCheckGroundTouch(diceCanvas.right))
        {
            isValueFound = true; isThrowed = false;
            diceValue = diceValues[1];
            Debug.Log(diceValue);
        }
        else if (DrawRayFromDiceCenterAndCheckGroundTouch(-diceCanvas.right))
        {
            isValueFound = true; isThrowed = false;
            diceValue = diceValues[3];
            Debug.Log(diceValue);
        }
        else
        {
            diceValue = 0;
            Debug.Log(diceValue);
        }

        StartCoroutine(FindWinner(diceValue, enemyDiceValue));
    }

    private bool DrawRayFromDiceCenterAndCheckGroundTouch(Vector3 direction)
    {
        Debug.DrawRay(diceCanvas.position, direction * 1f, Color.red);
        if (Physics.Raycast(diceCanvas.position, direction, out RaycastHit hitInfo, 1f, layerMask))
        {
            return true;
        }
        else
            return false;
    }


    public void SetEnemyDiceValueAndFindWinner(BetArea betArea, int enemyDiceValue)
    {
        if (lastBetarea != betArea)
        {
            this.enemyDiceValue = enemyDiceValue;
            lastBetarea = betArea;

            //StartCoroutine(FindWinner(diceValue, enemyDiceValue));
        }
    }

    private IEnumerator FindWinner(int playerDiceValue, int enemyDiceValue)
    {
        yield return new WaitForSeconds(1f);

        GameManagement.Instance.FindWinner(playerDiceValue, enemyDiceValue);
    }

    #endregion
}
