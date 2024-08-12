using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemUse_Window_57 : MonoBehaviour
{
    public Station_PlayerData playerData;
    public TextMeshProUGUI PlayerPoint;

    private void OnEnable()
    {
        playerData.Player_Get_Point(1);
        PlayerPoint.text = (playerData.Player_Point - 1) + " - > <color=red>" + playerData.Player_Point + "</color>";
    }
}
