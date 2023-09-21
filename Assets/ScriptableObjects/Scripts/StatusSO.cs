using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultStatus", menuName = "Status/Default", order = 1)]
public class StatusSO : ScriptableObject
{
    [Header("Status")]
    public StatsChangeType statsChangeType;
    public float Luck;
    public float Charm;
    public float Exp;
}
