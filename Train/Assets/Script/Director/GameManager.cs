using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    private static GameManager instance = null;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    public void Update()
    {
        if(Input.GetKeyUp(KeyCode.Alpha0))
        {
            //µ•¿Ã≈Õ √ ±‚»≠
            SceneManager.LoadScene(0);
        }
    }
}