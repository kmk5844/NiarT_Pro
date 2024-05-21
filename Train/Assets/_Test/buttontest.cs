using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttontest : MonoBehaviour
{
    public SA_MercenaryData playerData;

    public void Click_Button()
    {
        playerData.SA_Mercenary_Buy(1);
    }

    public void Init_Button()
    {
        playerData.Init();
    }

    public void Load_Button()
    {
        playerData.Load();
    }
}
