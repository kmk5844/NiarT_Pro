using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemUse_Window_57 : MonoBehaviour
{
    public SA_PlayerData playerData;
    public TextMeshProUGUI PlayerPoint;

    private void OnEnable()
    {
        playerData.SA_Get_Point(1);
        PlayerPoint.text = (playerData.Point - 1) + " - > <color=red>" + playerData.Point + "</color>";
    }
}
