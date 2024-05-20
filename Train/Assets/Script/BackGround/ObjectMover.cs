using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectMover : MonoBehaviour
{
    public float force;
    public bool Flag;

    private void Start()
    {
        Flag = false;
        // 자식 오브젝트 추가
        foreach (Transform child in transform)
        {
            child.GetComponent<Rigidbody2D>().AddForce(Vector2.left * force, ForceMode2D.Impulse);
        }
    }
}