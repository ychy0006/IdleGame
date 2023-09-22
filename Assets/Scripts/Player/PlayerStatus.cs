using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum StatsChangeType
{
    Add,
    Multiple,
    Override
}

public class PlayerStatus : MonoBehaviour
{
    public string name {  get; private set; }
    public float totalExp {  get; private set; }

    [SerializeField] private StatusSO baseStatusSO;
    public StatusSO currentStatusSO;

    public List<StatusSO> statusModifiers = new List<StatusSO>();

    private Gold gold;

    private void Awake()
    {
        name = "아기고양이";
        totalExp = 0.0f;
        gold = GetComponent<Gold>();
        UpdatePlayerStatus();
    }

    private void UpdatePlayerStatus()
    {
        StatusSO statusSO = null;
        if (baseStatusSO != null)
        {
            statusSO = Instantiate(baseStatusSO);
        }

        currentStatusSO = statusSO;

        UpdateStatus((a, b) => b, baseStatusSO);

        foreach (StatusSO modifierSO in statusModifiers.OrderBy(o => o.statsChangeType))
        {
            if (modifierSO.statsChangeType == StatsChangeType.Override)
            {
                UpdateStatus((o, o1) => o1, modifierSO);
            }
            else if (modifierSO.statsChangeType == StatsChangeType.Add)
            {
                UpdateStatus((o, o1) => o + o1, modifierSO);
            }
            else if (modifierSO.statsChangeType == StatsChangeType.Multiple)
            {
                UpdateStatus((o, o1) => o * o1, modifierSO);
            }
        }

        gold.goldRate = (decimal)currentStatusSO.Luck * (decimal)currentStatusSO.Charm;
    }
    private void UpdateStatus(Func<float, float, float> operation, StatusSO newModifierSO)
    {
        if (currentStatusSO == null || newModifierSO == null)
            return;

        currentStatusSO.Luck = operation(currentStatusSO.Luck, newModifierSO.Luck);
        currentStatusSO.Charm = operation(currentStatusSO.Charm, newModifierSO.Charm);
    }
    public void AddStatusModifier(StatusSO statusModifierSO)
    {
        statusModifiers.Add(statusModifierSO);
        UpdatePlayerStatus();
    }
    public void AddEXP(StatusSO statusModifierSO)
    {
        totalExp += statusModifierSO.Exp;
    }
    public void RemoveStatModifier(StatusSO statusModifierSO)
    {
        statusModifiers.Remove(statusModifierSO);
        UpdatePlayerStatus();
    }
    public void RemoveAllStatModifier()
    {
        statusModifiers.Clear();
        UpdatePlayerStatus();
        MainInfoUI.instance.UpdateUI();
    }
    public void LoadStatus()
    {
        name = Data.instance.data.Name;
        totalExp = Data.instance.data.TotalExp;
        MainInfoUI.instance.UpdateUI();
    }
}
