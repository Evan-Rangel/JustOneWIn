using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    //Player Reference Var
    private Transform player;

    //Player SpriteRenders Vars
    private SpriteRenderer sR;
    private SpriteRenderer playerSR;

    //Player Color Vars
    private Color color;

    //Player DashEffect Vars
    [Header("Dash Effect")]
    [SerializeField]
    private float activeTime = 0.1f;
    private float timeActivated;
    private float alpha;
    [SerializeField]
    private float alphaSet = 0.7f;
    [SerializeField]
    private float alphaDecay = 1f;//This var change the velocity of the fade, more small number much faste will be

    //OnEnble
    private void OnEnable()
    {
        try
        {

            player = GameObject.Find("LocalGamePlayer").transform;
            //Get Reference
            sR = GetComponent<SpriteRenderer>();

            //player = GameObject.FindGameObjectWithTag("Player").transform;
            playerSR = player.GetComponent<SpriteRenderer>();

            //Set Values
            alpha = alphaSet;
            sR.sprite = playerSR.sprite;
            transform.position = player.position;
            transform.rotation = player.rotation;
            timeActivated = Time.time;
        }
        catch { return; }

    }

    //Update
    private void Update()
    {
        alpha -= alphaDecay * Time.deltaTime;//Decreasing the alpha
        color = new Color(1f, 1f, 1f, alpha);//Crate new color with the alpha 
        sR.color = color;//Set the color to the sprite

        //Condition that helps to know if the time was long enough
        if(Time.time >= (timeActivated + activeTime))
        {
            //Add to the pool
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
