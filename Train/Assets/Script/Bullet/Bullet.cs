using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    protected float Speed;

    [SerializeField]
    public int atk;
    protected Vector3 dir;
    protected Rigidbody2D rid;

    protected virtual void Start()
    {
        rid = GetComponent<Rigidbody2D>();
    }
}