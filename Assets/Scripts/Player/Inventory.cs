using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemSlot
{
    public ItemData item;
    public int quantity;
}

public class Inventory : MonoBehaviour
{
    public ItemSlot[] slots;
    public ItemSlotUI[] uiSlots;

    public GameObject inventoryWindow;
    public GameObject inventoryDescriptionWindow;

    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public Image selectedItemIcon;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStat;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;

    private PlayerStatus status;
    public static Inventory instance;
    void Awake()
    {
        instance = this;
        status = GetComponent<PlayerStatus>();
    }

    private void Start()
    {
        inventoryWindow.SetActive(false);
        inventoryDescriptionWindow.SetActive(false);

        slots = new ItemSlot[uiSlots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new ItemSlot();
            uiSlots[i].index = i;
            uiSlots[i].Clear();
        }
        ClearSeletecItemWindow();
    }

    public bool AddItem(ItemData item)
    {
        if (item.canStack)
        {
            ItemSlot slotToStackTo = GetItemStack(item);
            if (slotToStackTo != null)
            {
                slotToStackTo.quantity++;
                UpdateUI();
                return true;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.quantity = 1;
            UpdateUI();
            return true;
        }

        return false;
    }
    ItemSlot GetItemStack(ItemData item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item && slots[i].quantity < item.maxStackAmount)
                return slots[i];
        }

        return null;
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
                uiSlots[i].Set(slots[i]);
            else
                uiSlots[i].Clear();
        }

    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                return slots[i];
        }

        return null;
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null)
            return;

        inventoryDescriptionWindow.SetActive(true);
        inventoryWindow.GetComponent<GraphicRaycaster>().enabled = false;

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemIcon.sprite = selectedItem.item.icon;
        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        selectedItemStat.text = string.Empty;

        if (selectedItem.item.statsChangeType == StatsChangeType.Add)
        {
            if (selectedItem.item.Luck != 0) selectedItemStat.text += "행운 +" + selectedItem.item.Luck.ToString() + "\n";
            if (selectedItem.item.Charm != 0) selectedItemStat.text += "매력 +" + selectedItem.item.Charm.ToString() + "\n";
            if (selectedItem.item.Exp != 0) selectedItemStat.text += "경험치 +" + selectedItem.item.Exp.ToString() + "\n";
        }
        else if (selectedItem.item.statsChangeType == StatsChangeType.Multiple)
        {
            if (selectedItem.item.Luck != 1) selectedItemStat.text += "행운 X" + selectedItem.item.Luck.ToString() + "\n";
            if (selectedItem.item.Charm != 1) selectedItemStat.text += "매력 X" + selectedItem.item.Charm.ToString() + "\n";
            if (selectedItem.item.Exp != 1) selectedItemStat.text += "경험치 X" + selectedItem.item.Exp.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !uiSlots[index].equipped);
        unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && uiSlots[index].equipped);
    }

    public void OnCloseDescription()
    {
        inventoryWindow.GetComponent<GraphicRaycaster>().enabled = true;
        inventoryDescriptionWindow.SetActive(false);
    }

    private void ClearSeletecItemWindow()
    {
        selectedItem = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemIcon.sprite = null;

        selectedItemStat.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
    }

    public void OnEquipButton()
    {
        uiSlots[selectedItemIndex].equipped = true;
        status.AddStatusModifier(selectedItem.item);
        MainInfoUI.instance.UpdateUI();
        UpdateUI(); //outline그리기
        SelectItem(selectedItemIndex); //나오는 버튼 종류 바꾸기
    }
    public void OnUnEquipButton()
    {
        uiSlots[selectedItemIndex].equipped = false;
        status.RemoveStatModifier(selectedItem.item);
        MainInfoUI.instance.UpdateUI();
        UpdateUI(); //outline지우기
        SelectItem(selectedItemIndex); //나오는 버튼 종류 바꾸기
    }
    public void OnUseButton()
    {
        status.AddEXP(selectedItem.item);
        MainInfoUI.instance.UpdateUI();
        RemoveSelectedItem();
    }
    private void RemoveSelectedItem()
    {
        selectedItem.quantity--;

        if (selectedItem.quantity <= 0)
        {
            selectedItem.item = null;
            ClearSeletecItemWindow();
            OnCloseDescription();
        }
        UpdateUI();
    }
    public void LoadItems()
    {
        slots = (ItemSlot[])(Data.instance.data.Items).Clone();
        UpdateUI();
    }
}
