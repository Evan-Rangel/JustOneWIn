using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif

namespace Avocado
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(EdgeCollider2D))]
    [RequireComponent(typeof(WaterTriggerHandler))]
    public class InteractableWater : MonoBehaviour
    {
        [Header("Springs")]
        [SerializeField] private float _spriteConstant = 1.4f;
        [SerializeField] private float _damping = 1.1f;
        [SerializeField] private float _spread = 2.0f;
        [SerializeField, Range(1, 10)] private int _wavePropogationIterations = 6;
        [SerializeField, Range(0f, 20f)] private float _speedMult = 5.5f;

        [Header("Force")]
        public float ForceMultiplier = 0.08f;
        [Range(0.1f, 50f)] public float MaxForce = 2.5f;

        [Header("Collision")]
        [SerializeField, Range(0.1f, 10f)] private float _playerCollisionRadiusMult = 1.2f;

        [Header("Mesh Generation")]
        [Range(2, 500)] public int NumOfXVertices = 70;
        public float Width = 10f;
        public float Height = 4f;
        public Material WaterMaterial;
        private const int NUM_OF_Y_VERTICES = 2;

        [Header("Sorting")]
        public string SortingLayerName = "Water";
        public int SortingOrder = 0;

        [Header("Gizmo")]
        public Color GizmoColor = Color.white;

        private Mesh _mesh;
        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;

        private Vector3[] _vertices;
        private int[] _topVerticesIndex;

        private EdgeCollider2D _coll;

        private class WaterPoint
        {
            public float velocity;
            public float pos;
            public float targetHeight;
        }

        private readonly List<WaterPoint> _waterPoints = new List<WaterPoint>();

        private void Awake()
        {
            _coll = GetComponent<EdgeCollider2D>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshFilter = GetComponent<MeshFilter>();

            if (_coll != null) _coll.isTrigger = true;
        }

        private void Start()
        {
            GenerateMesh();
        }

        private void FixedUpdate()
        {
            if (_mesh == null || _vertices == null || _topVerticesIndex == null) return;
            if (_waterPoints == null || _waterPoints.Count != _topVerticesIndex.Length) return;

            float dt = Time.fixedDeltaTime;

            // Springs update
            for (int i = 1; i < _waterPoints.Count - 1; i++)
            {
                WaterPoint p = _waterPoints[i];

                float x = p.pos - p.targetHeight;
                float a = (-_spriteConstant * x) - (_damping * p.velocity);

                p.velocity += a * dt;
                p.pos += p.velocity * _speedMult * dt;

                // Safety clamp so it never "explodes"
                float maxDisp = Mathf.Max(0.01f, Height * 0.45f);
                p.pos = Mathf.Clamp(p.pos, p.targetHeight - maxDisp, p.targetHeight + maxDisp);

                _waterPoints[i] = p;
                _vertices[_topVerticesIndex[i]].y = p.pos;
            }

            // Wave propagation (stable: velocity only)
            for (int iter = 0; iter < _wavePropogationIterations; iter++)
            {
                for (int i = 1; i < _waterPoints.Count - 1; i++)
                {
                    float leftDelta = _spread * (_waterPoints[i].pos - _waterPoints[i - 1].pos);
                    _waterPoints[i - 1].velocity += leftDelta;

                    float rightDelta = _spread * (_waterPoints[i].pos - _waterPoints[i + 1].pos);
                    _waterPoints[i + 1].velocity += rightDelta;
                }
            }

            _mesh.vertices = _vertices;
            _mesh.RecalculateBounds();
        }

        /// <summary>
        /// Splash mapped by X position to vertex index (robust).
        /// </summary>
        public void Splash(Collider2D collision, float force)
        {
            if (collision == null) return;
            if (_waterPoints == null || _waterPoints.Count == 0) return;

            force = Mathf.Clamp(force, -MaxForce, MaxForce);

            // 1) local center of the collider relative to water transform
            Vector2 localCenter = transform.InverseTransformPoint(collision.bounds.center);

            // 2) map local x (-Width/2..+Width/2) to index
            float t = Mathf.InverseLerp(-Width * 0.5f, Width * 0.5f, localCenter.x);
            int centerIndex = Mathf.RoundToInt(t * (NumOfXVertices - 1));
            centerIndex = Mathf.Clamp(centerIndex, 1, NumOfXVertices - 2);

            // 3) radius -> points
            float worldRadius = collision.bounds.extents.x * _playerCollisionRadiusMult;
            float localRadius = worldRadius / Mathf.Max(0.0001f, transform.lossyScale.x);
            float spacing = Width / (NumOfXVertices - 1);

            int radiusPts = Mathf.Clamp(Mathf.CeilToInt(localRadius / Mathf.Max(0.0001f, spacing)), 1, NumOfXVertices);

            int start = Mathf.Clamp(centerIndex - radiusPts, 1, NumOfXVertices - 2);
            int end = Mathf.Clamp(centerIndex + radiusPts, 1, NumOfXVertices - 2);

            // 4) apply with falloff
            for (int i = start; i <= end; i++)
            {
                float falloff = 1f - Mathf.Abs(i - centerIndex) / (float)(radiusPts + 0.0001f);
                float f = force * falloff;

                WaterPoint p = _waterPoints[i];
                p.velocity = Mathf.Clamp(p.velocity + f, -MaxForce, MaxForce);
                _waterPoints[i] = p;
            }
        }

        public void ResetEdgeCollider()
        {
            if (_coll == null) _coll = GetComponent<EdgeCollider2D>();
            if (_coll == null) return;

            if (_vertices == null || _topVerticesIndex == null || _topVerticesIndex.Length < 2) return;

            Vector2[] newPoints = new Vector2[2];

            Vector3 v0 = _vertices[_topVerticesIndex[0]];
            Vector3 v1 = _vertices[_topVerticesIndex[_topVerticesIndex.Length - 1]];

            newPoints[0] = new Vector2(v0.x, v0.y);
            newPoints[1] = new Vector2(v1.x, v1.y);

            _coll.offset = Vector2.zero;
            _coll.points = newPoints;
            _coll.isTrigger = true;
        }

        public void GenerateMesh()
        {
            _mesh = new Mesh();

            // vertices
            _vertices = new Vector3[NumOfXVertices * NUM_OF_Y_VERTICES];
            _topVerticesIndex = new int[NumOfXVertices];

            for (int y = 0; y < NUM_OF_Y_VERTICES; y++)
            {
                for (int x = 0; x < NumOfXVertices; x++)
                {
                    float xPos = (x / (float)(NumOfXVertices - 1)) * Width - Width / 2f;
                    float yPos = (y / (float)(NUM_OF_Y_VERTICES - 1)) * Height - Height / 2f;

                    int idx = y * NumOfXVertices + x;
                    _vertices[idx] = new Vector3(xPos, yPos, 0f);

                    if (y == NUM_OF_Y_VERTICES - 1)
                        _topVerticesIndex[x] = idx;
                }
            }

            // triangles
            int[] triangles = new int[(NumOfXVertices - 1) * (NUM_OF_Y_VERTICES - 1) * 6];
            int tri = 0;

            for (int y = 0; y < NUM_OF_Y_VERTICES - 1; y++)
            {
                for (int x = 0; x < NumOfXVertices - 1; x++)
                {
                    int bottomLeft = y * NumOfXVertices + x;
                    int bottomRight = bottomLeft + 1;
                    int topLeft = bottomLeft + NumOfXVertices;
                    int topRight = topLeft + 1;

                    triangles[tri++] = bottomLeft;
                    triangles[tri++] = topLeft;
                    triangles[tri++] = bottomRight;

                    triangles[tri++] = bottomRight;
                    triangles[tri++] = topLeft;
                    triangles[tri++] = topRight;
                }
            }

            // uvs
            Vector2[] uvs = new Vector2[_vertices.Length];
            for (int i = 0; i < _vertices.Length; i++)
                uvs[i] = new Vector2((_vertices[i].x + Width / 2f) / Width, (_vertices[i].y + Height / 2f) / Height);

            // renderer setup
            if (_meshRenderer == null) _meshRenderer = GetComponent<MeshRenderer>();
            if (_meshFilter == null) _meshFilter = GetComponent<MeshFilter>();

            _meshRenderer.sortingLayerName = SortingLayerName;
            _meshRenderer.sortingOrder = SortingOrder;

            if (WaterMaterial != null)
                _meshRenderer.sharedMaterial = WaterMaterial;

            _mesh.vertices = _vertices;
            _mesh.triangles = triangles;
            _mesh.uv = uvs;

            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();

            _meshFilter.sharedMesh = _mesh;

            CreateWaterPoints();
            ResetEdgeCollider();
        }

        private void CreateWaterPoints()
        {
            _waterPoints.Clear();

            if (_topVerticesIndex == null) return;

            for (int i = 0; i < _topVerticesIndex.Length; i++)
            {
                float y = _vertices[_topVerticesIndex[i]].y;
                _waterPoints.Add(new WaterPoint
                {
                    pos = y,
                    targetHeight = y,
                    velocity = 0f
                });
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(InteractableWater))]
    public class InteractableWaterEditor : Editor
    {
        private InteractableWater _water;

        private void OnEnable()
        {
            _water = (InteractableWater)target;
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            root.Add(new VisualElement { style = { height = 10 } });

            root.Add(new Button(() => _water.GenerateMesh()) { text = "Generate Mesh" });
            root.Add(new Button(() => _water.ResetEdgeCollider()) { text = "Place Edge Collider" });

            return root;
        }

        private void OnSceneGUI()
        {
            Handles.color = _water.GizmoColor;
            Vector3 center = _water.transform.position;
            Vector3 size = new Vector3(_water.Width, _water.Height, 0.1f);
            Handles.DrawWireCube(center, size);

            // if you want, keep your handle-resize logic here.
        }
    }
#endif
}