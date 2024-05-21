using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stroy_FadeOut : MonoBehaviour
{
    public void FadeOUt()
    {
        GameManager.Instance.End_Enter();
    }
}
