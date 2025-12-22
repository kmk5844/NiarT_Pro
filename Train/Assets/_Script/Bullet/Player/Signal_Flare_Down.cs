using UnityEngine;

public class Signal_Flare_Down : Bullet
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Bullet_Player();
        Destroy(gameObject, 7f);
    }

    void Bullet_Player()
    {
        rid.velocity = Vector2.down * Speed;
    }
}
