using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dice Roll Rush/ Dice Settings")]
public class DiceSettings : ScriptableObject
{
    public GameObject DiceModelPrefab;
    public int MoneyValue;
    public int BonusDiceValue;
    public int Exp;
}
