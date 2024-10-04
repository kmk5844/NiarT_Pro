using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    LineRenderer line;
    public Transform FireZoneTransform;
    private void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
    }

    private void Update()
    {
        ShowTrajectoryLine();
    }

    private void ShowTrajectoryLine()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 _playerPos = FireZoneTransform.transform.position;
        line.SetPosition(0, new Vector3(_playerPos.x , _playerPos.y, -2f));
        line.SetPosition(1, new Vector3(mousePos.x, mousePos.y, -2f));
    }
}
