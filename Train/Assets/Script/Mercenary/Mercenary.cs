using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mercenary : MonoBehaviour
{
    public Transform Train_List;
    int TrainCount;
    int move_X;
    float MaxMove_X;
    float MinMove_X;
    public float moveSpeed;



    SpriteRenderer sprite;

    private void Start()
    {
        TrainCount = Train_List.childCount;
        move_X = 1;
        MaxMove_X = 3f;
        MinMove_X = -4f + ((TrainCount - 1) * -8.5f);
        transform.position = new Vector3(Random.Range(MinMove_X, MaxMove_X), -1, 0);
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (transform.position.x > MaxMove_X)
        {
            move_X *= -1;
            sprite.flipX = true;
        }
        else if (transform.position.x < MinMove_X)
        {
            move_X *= -1;
            sprite.flipX = false;
        }
        transform.Translate(move_X * moveSpeed, 0, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(MinMove_X, -3, 0), new Vector3(MaxMove_X, -3, 0));
    }
}

public enum Mercenary_Job
{
    Engineer,
    Medic,
}