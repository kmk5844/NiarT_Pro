using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyRefresh_Item : MonoBehaviour
{
    public GameDirector director;
    public AudioClip GetItemSFX;
    Vector2 SupplyItem_Position;
    bool bounceFlag;

    public bool SupplyFlag;
    int fuelPersent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!bounceFlag)
        {
            if (transform.position.y > SupplyItem_Position.y -0.35f)
            {
                transform.Translate(10 * Vector2.down * Time.deltaTime);
            }
            else
            {
                if (transform.position.x < MonsterDirector.MinPos_Ground.x || transform.position.x > MonsterDirector.MaxPos_Ground.x)
                {
                    transform.Translate(2 * Vector2.down * Time.deltaTime);
                    if (SupplyItem_Position.y - 2f > transform.position.y)
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    bounceFlag = true;
                }
            }
        }
        else
        {
            float offset = Mathf.Sin(Time.time * 10f) * 0.2f; // 2 : 스피드, 1.0f 이동거리
            Vector2 movement = new Vector2(0f, offset);
            transform.Translate(movement * Time.deltaTime);
        }
    }

    public void FuelSignalSet(GameDirector gmdirector, int fuelPercent)
    {
        director = gmdirector;
        fuelPersent = fuelPercent;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MMSoundManagerSoundPlayEvent.Trigger(GetItemSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
            if (!SupplyFlag)
            {
                director.RefreshReward();
            }
            else
            {
                director.FuelSignalSupply(fuelPersent);
            }
            Destroy(gameObject);
        }
    }
}
