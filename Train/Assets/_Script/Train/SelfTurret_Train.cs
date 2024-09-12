using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfTurret_Train : MonoBehaviour
{
    Train_InGame SelfTurretTrain;
    GameDirector gameDirector;

    public int SelfTurretTrain_Fuel;
    [HideInInspector]
    public int Max_SelfTurretTrain_Fuel;

    bool changeFlag;

    bool FuelFlag;
    public bool UseFlag;
    public bool isAtacking;
    public GameObject Player_Object;
    SpriteRenderer[] Player_Part;

    int Turret_Atk;
    float Turret_AtkDelay;
    int Turret_Second;

    float timebet;
    float lastTime;

    float atk_lastTime;
    bool isMouseDown;

    public Transform Turret_column;
    public Transform TurretObject;
    Transform FireZone;
    GameObject Bullet_Object;
    Vector3 TurretObject_Scale;
    Camera mainCam;
    Vector3 mousePos;
    public AudioClip Shoot_SFX;

    private void Start()
    {
        SelfTurretTrain = GetComponentInParent<Train_InGame>(); 
        gameDirector = SelfTurretTrain.gameDirector.GetComponent<GameDirector>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        SelfTurretTrain_Fuel = 0;
        SelfTurretTrain_Fuel = SelfTurretTrain.Train_Self_UseFuel;
        Max_SelfTurretTrain_Fuel = SelfTurretTrain.Train_Self_UseFuel;
        Turret_Atk = SelfTurretTrain.Train_Self_Attack;
        Turret_AtkDelay = SelfTurretTrain.Train_Self_Attack_Delay;
        Turret_Second = SelfTurretTrain.Train_Self_Second;

        changeFlag = false;
        FuelFlag = false;
        UseFlag = false;
        isAtacking = false;

        TurretObject_Scale = TurretObject.localScale;
        FireZone = TurretObject.GetChild(0);
        Bullet_Object = Resources.Load<GameObject>("Bullet/Player/Self_Turret_Bullet");

        Player_Part = new SpriteRenderer[Player_Object.transform.childCount];
        for(int i = 0; i < Player_Object.transform.childCount; i++)
        {
            Player_Part[i] = Player_Object.transform.GetChild(i).GetComponent<SpriteRenderer>();
        }

        Player_Object.SetActive(false);

        timebet = 0.05f;
        lastTime = Time.time;
    }


    private void Update()
    {
        if (!changeFlag)
        {
            gameDirector.player.GetComponent<Player_Chage>().ChangePlayer_NoneGun(gameDirector.player.PlayerNum, Player_Part);
            changeFlag = true;
        }

        if (gameDirector.gameType == GameType.Playing || gameDirector.gameType == GameType.Boss)
        {
            if (!FuelFlag)
            {
                if (Time.time > lastTime + timebet)
                {
                    if (SelfTurretTrain_Fuel < Max_SelfTurretTrain_Fuel)
                    {
                        if (gameDirector.TrainFuel > 0)
                        {
                            SelfTurretTrain_Fuel += 1;
                            gameDirector.TrainFuel -= 1;
                        }
                        lastTime = Time.time;
                    }
                    else if (SelfTurretTrain_Fuel >= Max_SelfTurretTrain_Fuel)
                    {
                        SelfTurretTrain_Fuel = Max_SelfTurretTrain_Fuel;
                        FuelFlag = true;
                        UseFlag = true;
                        lastTime = Time.time;
                    }
                }
            }

            if (isAtacking)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    isMouseDown = true;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    isMouseDown = false;
                }


            }
            else
            {
                isMouseDown = false;
            }

            if (isMouseDown)
            {
                BulletFire();
            }
        }
        else if(gameDirector.gameType == GameType.Ending)
        {
            if(isAtacking)
            {
                if (isMouseDown)
                {
                    isMouseDown = false;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (isAtacking)
        {
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

            Vector3 rot = mousePos - TurretObject.transform.position;
            float rotZ = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;
            TurretObject. transform.rotation = Quaternion.Euler(0, 0, rotZ);

            if(rotZ >= -90 && rotZ <= 90)
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

    public IEnumerator UseSelfTurret()
    {
        UseFlag = false;
        isAtacking = true;
        Player_Object.SetActive(true);
        atk_lastTime = Time.time;
        yield return new WaitForSeconds(10);
        isAtacking = false;
        FuelFlag = false;
        Player_Object.SetActive(false);
        SelfTurretTrain_Fuel = 0;
        lastTime = Time.time;
    }

    void BulletFire()
    {
        if(Time.time >= atk_lastTime + Turret_AtkDelay)
        {
            Bullet_Object.GetComponent<Bullet>().atk = Turret_Atk;
            Instantiate(Bullet_Object, FireZone.position, Quaternion.identity);
            MMSoundManagerSoundPlayEvent.Trigger(Shoot_SFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);

            atk_lastTime = Time.time;
        }
    }
}
