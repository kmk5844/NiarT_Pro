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
        // �ڽ� ������Ʈ �߰�
        foreach (Transform child in transform)
        {
            child.GetComponent<Rigidbody2D>().AddForce(Vector2.left * force, ForceMode2D.Impulse);
        }
    }
}