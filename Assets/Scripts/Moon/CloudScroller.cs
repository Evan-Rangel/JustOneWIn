using UnityEngine;

public class CloudScroller : MonoBehaviour
{
    public float speed = 2f;
    public float width = 28f;
    public Transform cam;

    private Transform[] clouds;

    void Start()
    {
        clouds = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            clouds[i] = transform.GetChild(i);
        }
    }

    void Update()
    {
        foreach (Transform cloud in clouds)
        {
            cloud.localPosition += Vector3.left * speed * Time.deltaTime;
        }

        Transform leftMost = clouds[0];
        Transform rightMost = clouds[0];

        foreach (Transform cloud in clouds)
        {
            if (cloud.position.x < leftMost.position.x)
                leftMost = cloud;

            if (cloud.position.x > rightMost.position.x)
                rightMost = cloud;
        }

        if (leftMost.position.x < cam.position.x - width)
        {
            leftMost.localPosition = new Vector3(
                rightMost.localPosition.x + width,
                leftMost.localPosition.y,
                leftMost.localPosition.z
            );
        }
    }
}
