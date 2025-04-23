using UnityEngine;

namespace Avocado.Weapons
{
    public class OptionalSpriteMarker : MonoBehaviour
    {
        /*
        * This is an empty MonoBehaviour used to help identify a specific GameObject. This GameObject should be a child of the Base weapon GameObject
        * and is animated by the base weapon animations.
        */      
        public SpriteRenderer SpriteRenderer => gameObject.GetComponent<SpriteRenderer>();       
    }
}
