using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    public class CameraGridContainer : MonoBehaviour
    {
        PolygonCollider2D polygonCollider2D;
        // Start is called before the first frame update
        void Start()
        {
            polygonCollider2D = GetComponent<PolygonCollider2D>();
            CinemachineConfiner2D confiner2d= FindObjectOfType<CinemachineConfiner2D>();
            confiner2d.m_BoundingShape2D = polygonCollider2D;
        }

       
    }
}
