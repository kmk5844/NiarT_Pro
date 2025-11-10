using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackStationDirector : MonoBehaviour
{
    public Station_PlayerData playerData;


    int num;
    public CinemachineConfiner virtualCamera_Confiner;
    public PolygonCollider2D[] CameraRange;
    public GameObject[] StationObject;

    void Start()
    {
        num = playerData.EX_Game_Data.Information_Stage[playerData.SA_PlayerData.New_Stage].StationBackGround;
        virtualCamera_Confiner.m_BoundingShape2D = CameraRange[num];
        StationObject[num].SetActive(true);
    }
}
