using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region �̱���
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

    public SA_PlayerData PlayerData;
    public SA_StoryData StoryData;
    public List<int> Story_Equals_Stage; //�ӽ÷� ���а�


    public void Update()
    {
        if(Input.GetKeyUp(KeyCode.Alpha0))
        {
            //������ �ʱ�ȭ
            SceneManager.LoadScene(0);
        }
    }

    public void Start_Enter()
    {
        if (Story_Equals_Stage.Contains(PlayerData.Stage))
        {
            StoryData.Start_Story(PlayerData.Stage);
        }
        else
        {
            StoryData.Start_Story(-1);
        }
    }

    public void End_Enter()
    {
        if (Story_Equals_Stage.Contains(PlayerData.Stage))
        {
            StoryData.End_Story(PlayerData.Stage);
        }
        else
        {
            StoryData.End_Story(-1);
        }
    }
}