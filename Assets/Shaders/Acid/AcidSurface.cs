using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class AcidSurface : MonoBehaviour
{
    [Header("Geometry")]
    public float width = 8f;
    public float height = 2f;
    [Min(2)] public int surfacePoints = 32; 

    [Header("Spring")]
    public float springStrength = 120f; 
    public float damping = 10f;        

    [Header("Wave Propagation")]
    public float spread = 0.25f;
    [Range(1, 20)] public int spreadIterations = 8;

    [Header("Debug")]
    public bool drawGizmos = true;

    Mesh _mesh;
    Vector3[] _verts;

    float[] _y;   
    float[] _v;   

    MeshFilter _mf;

    int TopIndex(int i) => i;                   
    int BottomIndex(int i) => surfacePoints + i; 

    void Awake()
    {
        _mf = GetComponent<MeshFilter>();
        BuildMesh();
    }

    void OnEnable()
    {
        if (_mf == null) _mf = GetComponent<MeshFilter>();
        if (_mesh == null) BuildMesh();
    }

    void OnValidate()
    {
        if (Application.isPlaying) return; 
    }

    void BuildMesh()
    {
        surfacePoints = Mathf.Max(2, surfacePoints);

        _y = new float[surfacePoints];
        _v = new float[surfacePoints];

        if (_mesh == null)
        {
            _mesh = new Mesh();
            _mesh.name = "AcidSurfaceMesh_Runtime";
            _mesh.MarkDynamic();
            _mesh.indexFormat = IndexFormat.UInt32; 

            GetComponent<MeshFilter>().mesh = _mesh;
        }
        else
        {
            _mesh.Clear();
        }

        _verts = new Vector3[surfacePoints * 2];
        Vector2[] uv = new Vector2[_verts.Length];
        int[] tris = new int[(surfacePoints - 1) * 6];

        float left = -width * 0.5f;
        float right = width * 0.5f;

        for (int i = 0; i < surfacePoints; i++)
        {
            float t = i / (float)(surfacePoints - 1);
            float x = Mathf.Lerp(left, right, t);

            _verts[TopIndex(i)] = new Vector3(x, height, 0f);
            _verts[BottomIndex(i)] = new Vector3(x, 0f, 0f);

            uv[TopIndex(i)] = new Vector2(t, 1f);
            uv[BottomIndex(i)] = new Vector2(t, 0f);
        }

        int ti = 0;
        for (int i = 0; i < surfacePoints - 1; i++)
        {
            int a = TopIndex(i);
            int b = TopIndex(i + 1);
            int c = BottomIndex(i);
            int d = BottomIndex(i + 1);

            tris[ti++] = a;
            tris[ti++] = b;
            tris[ti++] = c;

            tris[ti++] = b;
            tris[ti++] = d;
            tris[ti++] = c;
        }

        _mesh.vertices = _verts;
        _mesh.uv = uv;
        _mesh.triangles = tris;

        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();
    }

    void Update()
    {
        if (_mesh == null || _verts == null || _y == null || _v == null) return;

        float dt = Time.deltaTime;

        for (int i = 0; i < surfacePoints; i++)
        {
            float y = _y[i];
            float v = _v[i];

            float a = (-springStrength * y) - (damping * v);
            v += a * dt;
            y += v * dt;

            _v[i] = v;
            _y[i] = y;
        }

        for (int iter = 0; iter < spreadIterations; iter++)
        {
            for (int i = 0; i < surfacePoints; i++)
            {
                if (i > 0)
                {
                    float leftDelta = _y[i] - _y[i - 1];
                    _v[i - 1] += leftDelta * spread * dt;
                }
                if (i < surfacePoints - 1)
                {
                    float rightDelta = _y[i] - _y[i + 1];
                    _v[i + 1] += rightDelta * spread * dt;
                }
            }
        }

        for (int i = 0; i < surfacePoints; i++)
        {
            int idx = TopIndex(i);
            Vector3 p = _verts[idx];
            p.y = height + _y[i];
            _verts[idx] = p;
        }

        _mesh.vertices = _verts;
        _mesh.RecalculateBounds();
    }

    public void Splash(float x01, float velocity)
    {
        int i = Mathf.RoundToInt(Mathf.Lerp(0, surfacePoints - 1, Mathf.Clamp01(x01)));
        _v[i] += velocity;
    }

    public void SplashWorldX(float worldX, float velocity)
    {
        float left = transform.position.x - width * 0.5f;
        float x01 = (worldX - left) / width;
        Splash(x01, velocity);
    }

    void OnDrawGizmosSelected()
    {
        if (!drawGizmos || _verts == null) return;

        Gizmos.color = Color.cyan;
        for (int i = 0; i < surfacePoints; i++)
        {
            Vector3 wp = transform.TransformPoint(_verts[TopIndex(i)]);
            Gizmos.DrawSphere(wp, 0.03f);
        }
    }
}