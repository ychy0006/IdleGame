using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Stat;
    public TextMeshProUGUI Gold;

    public int index;

    public void Set(ItemData dataOS)
    {
        if(dataOS == null) return; //이거 안하니까 오류남.. 왤까?? null인 상황이 아예 없는데
        icon.sprite = dataOS.icon;
        Name.text = dataOS.displayName;
        Description.text = dataOS.description;
        Stat.text = null;
        SetStatText(dataOS);
        Gold.text = dataOS.price.ToString();
    }

    private void SetStatText(ItemData dataOS)
    {
        if (dataOS.statsChangeType == StatsChangeType.Add)
        {
            if (dataOS.Luck != 0) Stat.text += "행운 +" + dataOS.Luck.ToString() + "\n";
            if (dataOS.Charm != 0) Stat.text += "매력 +" + dataOS.Charm.ToString() + "\n";
            if (dataOS.Exp != 0) Stat.text += "경험치 +" + dataOS.Exp.ToString() + "\n";
        }
        else if (dataOS.statsChangeType == StatsChangeType.Multiple)
        {
            if (dataOS.Luck != 1) Stat.text += "행운 X" + dataOS.Luck.ToString() + "\n";
            if (dataOS.Charm != 1) Stat.text += "매력 X" + dataOS.Charm.ToString() + "\n";
            if (dataOS.Exp != 1) Stat.text += "경험치 X" + dataOS.Exp.ToString() + "\n";
        }
    }

    private string OperationCode(ItemData dataOS)
    {
        if (dataOS.statsChangeType == StatsChangeType.Add) return "+";
        else if(dataOS.statsChangeType == StatsChangeType.Multiple) return "X";
        
        return "";
    }
    public void OnButtonClick()
    {
        Shop.instance.BuyItem(index);
    }
}
