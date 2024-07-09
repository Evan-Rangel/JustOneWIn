using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CirclesColors
{ 
    yellow,
    green,
    red,
    blue,
    white
}
public class LoadCircles : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Dictionary<CirclesColors, Color> colors= new Dictionary<CirclesColors, Color>();

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
  
        colors.Add(CirclesColors.yellow, Color.yellow);
        colors.Add(CirclesColors.green, Color.green);
        colors.Add(CirclesColors.red, Color.red);
        colors.Add(CirclesColors.blue, Color.blue);
        colors.Add(CirclesColors.white, Color.white);

    }
    public void ChangeColor(float time, CirclesColors _targetColor, bool _withTime)
    {
        if (_withTime)
        {
            StartCoroutine(ChangeColor(time, _targetColor));
            return;
        }
        spriteRenderer.color = colors[_targetColor];
    }

    IEnumerator ChangeColor(float _time, CirclesColors _targetColor)
    {
        float mTime = 0;
        while (spriteRenderer.color != colors[_targetColor])
        {
            if (mTime >= _time)
            {
                spriteRenderer.color = colors[_targetColor];
                yield break;
            }
            mTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, colors[_targetColor], _time * Time.deltaTime * 10);
            yield return new WaitForSeconds(_time * Time.deltaTime);
        }
        yield break;
    }


}
