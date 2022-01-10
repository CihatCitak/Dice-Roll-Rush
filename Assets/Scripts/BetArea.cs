using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BetArea : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] List<Door> doors;
    [Header("Dice Settings")]
    [SerializeField] DiceSettings diceSetting;
    [SerializeField] Transform diceCanvasTransform, diceRootTransform;
    [SerializeField] List<TextMeshProUGUI> diceValuesTexts;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Vector3 diceThrowForce = Vector3.one;

    private GameObject currentCharacterModel;
    private int[] diceValues = new int[] { 1, 2, 3, 4, 5, 6 };
    private int diceValue;
    private bool isValueFound = false, isThrowed = false;

    private void Start()
    {
        InstantiateNewCharacterModelInPlayerObject();

        SetDiceValues(diceSetting.BonusDiceValue);

        SetDiceValueTexts();
    }

    private void Update()
    {
        if (rb.velocity == Vector3.zero && rb.angularVelocity == Vector3.zero && !isValueFound && isThrowed)
        {
            //Find dice value create blocks 
            StartCoroutine(FindDiceValue());
        }
    }

    public void OneDoorChoseen()
    {
        StartCoroutine(ThrowDice());

        foreach (var door in doors)
        {
            door.OneDoorTriggered();
        }

    }

    private void InstantiateNewCharacterModelInPlayerObject()
    {
        currentCharacterModel =
            Instantiate(diceSetting.DiceModelPrefab, diceRootTransform.position, diceRootTransform.rotation);

        currentCharacterModel.transform.parent = diceRootTransform;
    }

    private IEnumerator ThrowDice()
    {
        yield return new WaitForSeconds(0f);

        isThrowed = true;
        //rb.velocity = Vector3.zero;
        rb.AddForce(diceThrowForce, ForceMode.VelocityChange);
        rb.AddTorque(new Vector3(Random.Range(-2000f, 2000f), 0, 2000f));
    }

    private IEnumerator FindDiceValue()
    {
        yield return new WaitForSeconds(0f);

        if (DrawRayFromDiceCenterAndCheckGroundTouch(diceCanvasTransform.forward))
        {
            isValueFound = true; isThrowed = false;
            diceValue = diceValues[5];
            PlayerController.Instance.SetEnemyDiceValueAndFindWinner(this, diceValue);
            Debug.Log(diceValue);
        }
        else if (DrawRayFromDiceCenterAndCheckGroundTouch(-diceCanvasTransform.forward))
        {
            isValueFound = true; isThrowed = false;
            diceValue = diceValues[0];
            PlayerController.Instance.SetEnemyDiceValueAndFindWinner(this, diceValue);
            Debug.Log(diceValue);
        }
        else if (DrawRayFromDiceCenterAndCheckGroundTouch(diceCanvasTransform.up))
        {
            isValueFound = true; isThrowed = false;
            diceValue = diceValues[4];
            PlayerController.Instance.SetEnemyDiceValueAndFindWinner(this, diceValue);
            Debug.Log(diceValue);
        }
        else if (DrawRayFromDiceCenterAndCheckGroundTouch(-diceCanvasTransform.up))
        {
            isValueFound = true; isThrowed = false;
            diceValue = diceValues[2];
            PlayerController.Instance.SetEnemyDiceValueAndFindWinner(this, diceValue);
            Debug.Log(diceValue);
        }
        else if (DrawRayFromDiceCenterAndCheckGroundTouch(diceCanvasTransform.right))
        {
            isValueFound = true; isThrowed = false;
            diceValue = diceValues[1];
            PlayerController.Instance.SetEnemyDiceValueAndFindWinner(this, diceValue);
            Debug.Log(diceValue);
        }
        else if (DrawRayFromDiceCenterAndCheckGroundTouch(-diceCanvasTransform.right))
        {
            isValueFound = true; isThrowed = false;
            diceValue = diceValues[3];
            PlayerController.Instance.SetEnemyDiceValueAndFindWinner(this, diceValue);
            Debug.Log(diceValue);
        }
        else
        {
            diceValue = 0;
            Debug.Log(diceValue);
        }
    }

    private bool DrawRayFromDiceCenterAndCheckGroundTouch(Vector3 direction)
    {
        Debug.DrawRay(diceCanvasTransform.position, direction * 1f, Color.red);
        if (Physics.Raycast(diceCanvasTransform.position, direction, out RaycastHit hitInfo, 1f, layerMask))
        {
            return true;
        }
        else
            return false;
    }

    private void SetDiceValues(int diceBonusValue)
    {
        for (int i = 0; i < diceValues.Length - 1; i++)
        {
            diceValues[i] += diceBonusValue;
        }
    }

    private void SetDiceValueTexts()
    {
        for (int i = 0; i < diceValues.Length - 1; i++)
        {
            diceValuesTexts[i].SetText(diceValues[i].ToString());
        }
    }

}
