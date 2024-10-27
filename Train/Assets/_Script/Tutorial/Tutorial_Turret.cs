using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Turret : MonoBehaviour
{
    public bool UseFlag;
    public float atkDelay;
    public GameObject Player_object;
    public bool isAttacking;
    public float lastTime;
    public GameObject bullet;
    public Transform FireZone;

    
    public Transform Turret_column;
    public Transform TurretObject;
    Vector3 TurretObject_Scale;


    Camera mainCam;
    Vector3 mousePos;

    bool mouseButtonDonw_Flag;
    public AudioClip ShootSFX;

    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        TurretObject_Scale = TurretObject.localScale;
        atkDelay = 0.3f;
    }
    private void Update()
    {
        if (UseFlag)
        {
            Player_object.SetActive(true);
            isAttacking = true;
        }
        else
        {
            Player_object.SetActive(false);
            isAttacking = false;
        }

        if (isAttacking)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseButtonDonw_Flag = true;
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                mouseButtonDonw_Flag = false;
            }

            if (mouseButtonDonw_Flag)
            {
                BulletFire();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isAttacking)
        {
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

            Vector3 rot = mousePos - TurretObject.transform.position;
            float rotZ = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;
            TurretObject.transform.rotation = Quaternion.Euler(0,0,rotZ);

            if (rotZ >= -90 && rotZ <= 90)
            {
                Turret_column.transform.localScale = new Vector3(1, 1, 1);
                TurretObject.transform.localScale = new Vector3(TurretObject_Scale.x, TurretObject_Scale.y, TurretObject_Scale.z);
            }
            else
            {
                Turret_column.transform.localScale = new Vector3(-1, 1, 1);
                TurretObject.transform.localScale = new Vector3(TurretObject_Scale.x * -1, TurretObject_Scale.y * -1, TurretObject_Scale.z);
            }
        }
    }

    public void BulletFire()
    {
        if(Time.time >= lastTime + atkDelay)
        {
            Instantiate(bullet, FireZone.position, Quaternion.identity);
            MMSoundManagerSoundPlayEvent.Trigger(ShootSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
            lastTime = Time.time;
        }
    }

    public IEnumerator useTurret(Tutorial_Player player)
    {
        player.gameObject.SetActive(false);
        UseFlag = true;
        yield return new WaitForSeconds(5f);
        player.gameObject.SetActive(true);
        UseFlag = false;
        player.T_Train_Flag = true;
        Player_object.SetActive(false);
        isAttacking = false;
        script_Off();
    }


    public void script_Off()
    {
        GetComponent<Tutorial_Turret>().enabled = false;
    }
}
