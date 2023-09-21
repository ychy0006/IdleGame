using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainInfoUI : MonoBehaviour
{
    public GameObject Player;

    public TMP_Text GoldUI; //Gold.cs에서 instance불러서 사용
    public TMP_Text NameUI;
    public TMP_Text LevelUI;
    public TMP_Text ExpUI;
    public Image ExpBar;

    public TMP_Text LuckUI;
    public TMP_Text CharmUI;

    PlayerStatus status;

    public static MainInfoUI instance;

    private void Awake()
    {
        instance = this;
        status = Player.GetComponent<PlayerStatus>();
    }

    private void Start()
    {
        NameUI.text = status.name;
        UpdateUI();
    }

    public void UpdateUI()
    {
        LevelUI.text = ((int)(status.totalExp / 10.0f) + 1).ToString();
        ExpUI.text = $"{(status.totalExp % 10.0f).ToString()}/10";
        ExpBar.fillAmount = (status.totalExp % 10.0f) / 10.0f;
        LuckUI.text = status.currentStatusSO.Luck.ToString();
        CharmUI.text = status.currentStatusSO.Charm.ToString();
    }

}
