using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class PlayerData
{
    public string Name;
    public float TotalExp;
    public string Gold;
    public ItemSlot[] Items;
}

public class Data : MonoBehaviour
{
    PlayerStatus status;
    public PlayerData data;

    public static Data instance;

    private void Awake()
    {
        instance = this;
        status = GetComponent<PlayerStatus>();
        data = new PlayerData();
    }

    public void OnClickSaveButton()
    {
        Save();
    }
    public void OnClickLoadButton()
    {
        Load();
    }
    private void Save()
    {
        data.Name = status.name;
        data.TotalExp = status.totalExp;
        data.Gold = Gold.instance.gold.ToString();
        data.Items = (ItemSlot[])(Inventory.instance.slots).Clone();


        string jsonData = JsonUtility.ToJson(data);
        string fileName = "playerData.json";
        File.WriteAllText(fileName, jsonData);
    }
    private void Load()
    {
        string fileName = "playerData.json";
        if (File.Exists(fileName))
        {
            string jsonData = File.ReadAllText("playerData.json");
            data = JsonUtility.FromJson<PlayerData>(jsonData);

            if (data != null) 
            {
                Gold.instance.LoadGold();
                status.LoadStatus();
                Inventory.instance.LoadItems();
            }

        }
    }
}
