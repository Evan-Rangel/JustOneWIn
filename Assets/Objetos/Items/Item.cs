using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Item : MonoBehaviour
{
    [SerializeField] Sprite sprite;
    public Sprite GetSprite (){ return sprite; }

}
public interface ItemAction
{
    public void Action(Transform _player);
}