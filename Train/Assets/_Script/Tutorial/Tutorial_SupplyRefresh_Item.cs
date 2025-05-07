using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_SupplyRefresh_Item : MonoBehaviour
{
    GamePlay_Tutorial_Director tutorialDirector;
    public AudioClip GetItemSFX;
    float SupplyItem_Position;
    bool bounceFlag;

    // Start is called before the first frame update
    void Start()
    {
        tutorialDirector = GameObject.Find("TutorialDirector").GetComponent<GamePlay_Tutorial_Director>();
        SupplyItem_Position = 0f;
        transform.position = new Vector3(-13f, 14.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!bounceFlag)
        {
            if (transform.position.y > SupplyItem_Position)
            {
                transform.Translate(7 * Vector2.down * Time.deltaTime);
            }
            else
            {
                bounceFlag = true;
            }
        }
        else
        {
            float offset = Mathf.Sin(Time.time * 10f) * 0.2f; // 2 : 스피드, 1.0f 이동거리
            Vector2 movement = new Vector2(0f, offset);
            transform.Translate(movement * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MMSoundManagerSoundPlayEvent.Trigger(GetItemSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
            tutorialDirector.Refresh();
            Destroy(gameObject);
        }
    }
}
