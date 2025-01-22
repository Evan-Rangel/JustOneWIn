using UnityEngine;

public class DoorParent : Interactuable
{
    Collider2D coll;
    SpriteRenderer spr;
    public override void Awake()
    {
        base.Awake(); 
        coll = GetComponent<Collider2D>();
        spr = GetComponent<SpriteRenderer>();
    }
    public override void Activate()
    {
        base.Activate();
        coll.enabled = true;
        spr.enabled = true;
    }
    public override void Deactivate()
    {
        base.Deactivate();
        coll.enabled = false;
        spr.enabled = false;
    }
}
