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
        // ������ ��� ���̴� ��ƼŬ �ý����� �������� �����ϴ� �����Դϴ�.
        ParticleSystem nextWindEffect;
        if (trainSpeed < 50)
        {
            // �������� ����� ��ƼŬ �ý����� �����Ƿ� null�� �����մϴ�.
            nextWindEffect = null;
        }
        else // trainSpeed >= 50
        {
            // �ӵ��� 50 �̻��� ��� windlist[1]�� ���� ��� ������� �����մϴ�.
            nextWindEffect = windlist[0];
        }

        // '���� ��� ���� ��ƼŬ �ý���'�� '�������� ����� �ý���'�� �ٸ� ��쿡��
        // ��ƼŬ �ý����� ���߰� ����ϴ� �۾��� �����մϴ�.
        if (currentWindEffect != nextWindEffect)
        {
            // ���� ��� ���� ��ƼŬ�� �ִٸ� ����ϴ�.
            if (currentWindEffect != null)
            {
                currentWindEffect.Stop();
            }

            // �������� ����� ��ƼŬ �ý����� �ִٸ� ����մϴ�.
            if (nextWindEffect != null)
            {
                nextWindEffect.Play();
            }

            // ���� ��� ���� ��ƼŬ �ý����� ������Ʈ�մϴ�.
            currentWindEffect = nextWindEffect;
        }

        // ��ƼŬ �ý����� �ӵ��� �� ������ ������Ʈ�մϴ�.
        if (nextWindEffect != null)
        {
            var mainModule = nextWindEffect.main;
            float normalizedSpeed = Mathf.Clamp01(trainSpeed / gameDirector.MaxSpeed);
            mainModule.simulationSpeed = Mathf.Lerp(1f, 4f, normalizedSpeed);
        }
    }

    // (���� �߰��� �Լ�)
    void checkExtraWind()
    {
        bool isExtraWindActive = gameDirector.MarryGold_Skill_Q_Flag;
        // �ܺο��� �޾ƿ� �Ҹ��� ������ ������� ��ƼŬ�� �Ѱ� ���ϴ�.
        if (isExtraWindActive)
        {
            // ���� �־�� �ϴ� ����
            if (currentExtraWindEffect == null)
            {
                windlist[1].Play();
                currentExtraWindEffect = windlist[1];
            }
        }
        else
        {
            // ���� �־�� �ϴ� ����
            if (currentExtraWindEffect != null)
            {
                windlist[1].Stop();
                currentExtraWindEffect = null;
            }
        }

        // �߰��� �䱸����: simulationSpeed �����ϰ� ����
        if (currentExtraWindEffect != null)
        {
            float trainSpeed = gameDirector.TrainSpeed;
            float normalizedSpeed = Mathf.Clamp01(trainSpeed / gameDirector.MaxSpeed);
            var mainModule = currentExtraWindEffect.main;
            mainModule.simulationSpeed = Mathf.Lerp(3f, 5f, normalizedSpeed);
        }
    }
}
