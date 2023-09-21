using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gold : MonoBehaviour
{
    public decimal gold{ get; private set; }
    public decimal goldRate;

    public static Gold instance;

    private void Awake()
    {
        instance = this;
        gold = 0;
    }

    private void Update()
    {
        gold += goldRate;
        MainInfoUI.instance.GoldUI.text = gold.ToString();
    }

    public void addGold(decimal amount)
    {
        gold += amount;
    }

    public bool minusGold(decimal amount)
    {
        if (amount > 0 && amount <= gold)
        {
            gold -= amount;
            return true;
        }

        return false;
    }
}
