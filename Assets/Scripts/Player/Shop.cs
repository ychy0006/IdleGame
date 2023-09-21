using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public ShopSlotUI[] uiSlots;
    public ItemData[] shopItemDB;

    public GameObject shopWindow;
    public GameObject shopDescriptionWindow;
    public TextMeshProUGUI shopPopup;

    public static Shop instance;

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (i == shopItemDB.Length) break;
            uiSlots[i].Set(shopItemDB[i]);
            uiSlots[i].index = i;
        }
    }

    private void Start()
    {
        shopWindow.SetActive(false);
        shopDescriptionWindow.SetActive(false);
    }

    public void BuyItem(int index)
    {
        shopWindow.GetComponent<GraphicRaycaster>().enabled = false;
        shopDescriptionWindow.SetActive(true);
        shopPopup.text = null;
        if (Gold.instance.minusGold((decimal)(shopItemDB[index].price)))
        {
            if (Inventory.instance.AddItem(shopItemDB[index]))
            {
                shopPopup.text = "구매완료했습니다";
            }
            else
            {
                shopPopup.text = "구매실패했습니다";
                Gold.instance.addGold((decimal)(shopItemDB[index].price));
            }
        }
        else
        {
            shopPopup.text = "구매실패했습니다";
        }
    }

    public void OnCloseDescription()
    {
        shopWindow.GetComponent<GraphicRaycaster>().enabled = true;
        shopDescriptionWindow.SetActive(false);
    }

}
