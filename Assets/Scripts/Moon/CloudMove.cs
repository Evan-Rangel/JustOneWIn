using UnityEngine;

public class CloudMove : MonoBehaviour
{
    public float speed = 2f;
    public float destroyX = -50f;

    void Start()
    {
        speed = Random.Range(1f, 3f);
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= destroyX)
        {
            Destroy(gameObject);
        }
    }
}
