using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUpgradeTrain : MonoBehaviour
{
    Train_InGame trainData;
    GameDirector gameDirector;

    public float elapsed;
    public float SpawnTime;
    float Persent;
    float delayTime;

    float lastTime;
    public bool useflag;

    public Transform LeverObject;
    public SpriteRenderer GuageSprite;
    public ParticleSystem UseEffect;

    bool endflag;

    public SpriteRenderer[] FuelGuage;
    float segmentSize = 1.08f;
    int GuageNum = 4;
    void Start()
    {
        trainData = transform.GetComponentInParent<Train_InGame>();
        gameDirector = trainData.gameDirector;

        SpawnTime = float.Parse(trainData.trainData_Special_String[0]);
        Persent = float.Parse(trainData.trainData_Special_String[1]);
        delayTime = float.Parse(trainData.trainData_Special_String[2]);
    }

    void Update()
    {
        if (gameDirector.gameType == GameType.Playing || gameDirector.gameType == GameType.Boss)
        {
            if (!endflag)
            {
                elapsed += Time.deltaTime;
            }

            float progress = Mathf.Clamp01(elapsed / SpawnTime);

            // 현재 몇 번째 칸인지
            int section = Mathf.FloorToInt(progress * GuageNum);
            section = Mathf.Clamp(section, 0, GuageNum - 1);
            float sectionProgress = (progress * GuageNum) - section;
            // 모든 스프라이트 초기화
            for (int i = 0; i < GuageNum; i++)
            {
                if (i < section)
                {
                    // 이전 칸들은 꽉 채움
                    FuelGuage[i].size = new Vector2(FuelGuage[i].size.x, segmentSize);
                }
                else if (i == section)
                {
                    // 현재 칸은 진행률만큼 채움
                    FuelGuage[i].size = new Vector2(FuelGuage[i].size.x, sectionProgress * segmentSize);
                }
                else
                {
                    // 이후 칸들은 비움
                    FuelGuage[i].size = new Vector2(FuelGuage[i].size.x, 0f);
                }
            }
            // 강제로 초 단위로 변환
            int elapsedSeconds = Mathf.FloorToInt(elapsed);
            int spawnSeconds = Mathf.FloorToInt(SpawnTime);

            // 시간이 충분하면 useflag 허용 (한 번만)
            if (!useflag && elapsedSeconds >= spawnSeconds)
            {
                useflag = true;
            }
        }
    }
    public void ClickTrain()
    {
        UseEffect.Play();
        gameDirector.Item_Use_Train_Turret_All_SpeedUP(Persent,delayTime);
        StartCoroutine(Click());
        useflag = false;
    }

    public IEnumerator Click()
    {
        elapsed = 0;
        endflag = true;
        LeverObject.localScale = new Vector3(1, -1, 1);
        GuageSprite.size = new Vector2(1.66f, 1f);
        // 시작 사이즈
        Vector2 size = GuageSprite.size;
        Vector2 targetSize = new Vector2(0f, size.y);

        float elaspsedTime = 0f;

        while (elaspsedTime < delayTime)
        {
            elaspsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elaspsedTime / delayTime);

            GuageSprite.size = Vector2.Lerp(size, targetSize, t);

            yield return null; // 한 프레임 대기
        }

        GuageSprite.size = Vector2.zero;
        LeverObject.localScale = new Vector3(1, 1, 1);
        endflag = false;
    }

}
