using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDirector : MonoBehaviour
{
    public GameDirector gameDirector;
    public ParticleSystem[] windlist;
    private ParticleSystem currentWindEffect = null;
    private ParticleSystem currentExtraWindEffect = null;

    // Start is called before the first frame update
    void Start()
    {
        windlist[0].Stop();
        windlist[1].Stop();
        windlist[2].Stop();
    }

    // Update is called once per frame
    void Update()
    {
        checkWind();
        checkExtraWind();
    }

    void checkWind()
    {
        float trainSpeed = gameDirector.TrainSpeed;
        // 이전에 재생 중이던 파티클 시스템이 무엇인지 추적하는 변수입니다.
        ParticleSystem nextWindEffect;
        if (trainSpeed < 3)
        {
            // 다음으로 재생할 파티클 시스템이 없으므로 null로 설정합니다.
            nextWindEffect = null;
        }
        else if (trainSpeed < 50)
        {
            // 속도가 50 미만일 경우 windlist[0]을 다음 재생 대상으로 설정합니다.
            nextWindEffect = windlist[0];
        }
        else // trainSpeed >= 50
        {
            // 속도가 50 이상일 경우 windlist[1]을 다음 재생 대상으로 설정합니다.
            nextWindEffect = windlist[1];
        }

        // '현재 재생 중인 파티클 시스템'과 '다음으로 재생할 시스템'이 다를 경우에만
        // 파티클 시스템을 멈추고 재생하는 작업을 수행합니다.
        if (currentWindEffect != nextWindEffect)
        {
            // 현재 재생 중인 파티클이 있다면 멈춥니다.
            if (currentWindEffect != null)
            {
                currentWindEffect.Stop();
            }

            // 다음으로 재생할 파티클 시스템이 있다면 재생합니다.
            if (nextWindEffect != null)
            {
                nextWindEffect.Play();
            }

            // 현재 재생 중인 파티클 시스템을 업데이트합니다.
            currentWindEffect = nextWindEffect;
        }

        // 파티클 시스템의 속도는 매 프레임 업데이트합니다.
        if (nextWindEffect != null)
        {
            var mainModule = nextWindEffect.main;
            float normalizedSpeed = Mathf.Clamp01(trainSpeed / gameDirector.MaxSpeed);

            if (trainSpeed < 50)
            {
                mainModule.simulationSpeed = Mathf.Lerp(2f, 3f, normalizedSpeed);
            }
            else
            {
                mainModule.simulationSpeed = Mathf.Lerp(1f, 4f, normalizedSpeed);
            }
        }
    }

    // (새로 추가된 함수)
    void checkExtraWind()
    {
        bool isExtraWindActive = gameDirector.MarryGold_Skill_Q_Flag;
        // 외부에서 받아온 불리언 변수를 기반으로 파티클을 켜고 끕니다.
        if (isExtraWindActive)
        {
            // 켜져 있어야 하는 상태
            if (currentExtraWindEffect == null)
            {
                windlist[2].Play();
                currentExtraWindEffect = windlist[2];
            }
        }
        else
        {
            // 꺼져 있어야 하는 상태
            if (currentExtraWindEffect != null)
            {
                windlist[2].Stop();
                currentExtraWindEffect = null;
            }
        }

        // 추가된 요구사항: simulationSpeed 동일하게 적용
        if (currentExtraWindEffect != null)
        {
            float trainSpeed = gameDirector.TrainSpeed;
            float normalizedSpeed = Mathf.Clamp01(trainSpeed / gameDirector.MaxSpeed);
            var mainModule = currentExtraWindEffect.main;
            mainModule.simulationSpeed = Mathf.Lerp(3f, 5f, normalizedSpeed);
        }
    }
}
