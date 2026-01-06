using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLightList_12 : MonoBehaviour
{
    public GameObject LightPrefab;
    public int Count = 5;

    float MinX;
    float MaxX;

    public bool UseUnbalance = false;
    [Range(0f, 1f)]
    public float UnbalanceStrength = 0.5f;

    private void Start()
    {
        MinX = MonsterDirector.MinPos_Sky.x - 25f;
        MaxX = MonsterDirector.MaxPos_Sky.x + 25f;

        if (Count <= 0) return;

        // 1개면 중앙
        if (Count == 1)
        {
            Instantiate(
                LightPrefab,
                new Vector2(0f, 15f),
                Quaternion.identity,
                transform
            );
            return;
        }

        float step = (MaxX - MinX) / (Count - 1);

        for (int i = 0; i < Count; i++)
        {
            float x = MinX + step * i;

            if (UseUnbalance && i != 0 && i != Count - 1)
            {
                // 중앙일수록 1, 양끝일수록 0
                float centerWeight =
                    1f - Mathf.Abs((i / (float)(Count - 1)) * 2f - 1f);

                float offset =
                    Random.Range(-step, step) *
                    UnbalanceStrength *
                    centerWeight;

                x += offset;
            }

            Vector2 spawnPos = new Vector2(x, 15f);
            Instantiate(LightPrefab, spawnPos, Quaternion.identity, transform);
        }
    }
}
