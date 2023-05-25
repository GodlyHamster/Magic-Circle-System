using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ring
{
    [SerializeField] private bool isFilled;
    [SerializeField] private int circleAmount = 1;
    [SerializeField] private float circleOffset = 1.0f;
    [SerializeField] private int edges = 10;
    [SerializeField] private float radius = 1.0f;
    [SerializeField] private float ringWidth = 0.1f;
    [SerializeField] private Vector3 offset = new Vector3(0,0,0);
    [SerializeField] private Vector3 rotation = new Vector3(0, 0, 0);
    [ColorUsage(true, true)]
    [SerializeField] private Color color;
    [SerializeField] private Material material;
    [Header("Animation")]
    [SerializeField] private AnimationCurve xPos;
    [SerializeField] private AnimationCurve yPos;
    [SerializeField] private AnimationCurve zPos;
    [Space]
    [SerializeField] private AnimationCurve xRot;
    [SerializeField] private AnimationCurve yRot;
    [SerializeField] private AnimationCurve zRot;

    public void CreateCirclesWithAmount(GameObject parent)
    {
        if (circleAmount <= 1)
        {
            GameObject circle = CreateMagicCircle(parent);
            RingAnimator animator = circle.AddComponent<RingAnimator>();
            animator.xPos = xPos;
            animator.yPos = yPos;
            animator.zPos = zPos;
            animator.xRot = xRot;
            animator.yRot = yRot;
            animator.zRot = zRot;
            return;
        }
        
        GameObject circleParent = new GameObject("magic circles");
        RingAnimator ringAnimator = circleParent.AddComponent<RingAnimator>();
        ringAnimator.xPos = xPos;
        ringAnimator.yPos = yPos;
        ringAnimator.zPos = zPos;
        ringAnimator.xRot = xRot;
        ringAnimator.yRot = yRot;
        ringAnimator.zRot = zRot;
        
        for (int i = 0; i < circleAmount; i++)
        {
            GameObject circle = CreateMagicCircle(parent);

            if (circleAmount <= 1) break;
            float x;
            float z;
            
            x = circleOffset * Mathf.Sin((2 * Mathf.PI * i) / circleAmount);
            z = circleOffset * Mathf.Cos((2 * Mathf.PI * i) / circleAmount);
            circle.transform.position = offset + new Vector3(x, 0, z);
            circle.transform.LookAt(circleParent.transform.position + offset);
            circle.transform.rotation = circle.transform.rotation * Quaternion.Euler(rotation);
            circle.transform.parent = circleParent.transform;
        }
        circleParent.transform.parent = parent.transform;
        circleParent.transform.position = parent.transform.position;
    }

    public GameObject CreateMagicCircle(GameObject parent)
    {
        GameObject magicCircle = new GameObject();
        magicCircle.name = "magic circle";
        magicCircle.layer = 10;
        magicCircle.transform.parent = parent.transform;
        magicCircle.transform.position = parent.transform.position;
        magicCircle.transform.rotation = Quaternion.Euler(rotation);

        Mesh mesh = new Mesh();
        mesh = CreateMesh(mesh);
        mesh.name = "circleMesh";

        MeshRenderer meshRenderer = magicCircle.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = material;
        meshRenderer.material.SetColor("Color_GlowColor", color);
        
        MeshFilter meshFilter = magicCircle.AddComponent<MeshFilter>();
        
        MeshCollider meshCollider = magicCircle.AddComponent<MeshCollider>();

        meshCollider.sharedMesh = mesh;

        meshFilter.mesh = mesh;

        return magicCircle;
    }

    private Mesh CreateMesh(Mesh mesh)
    {
        //vertices
        List<Vector3> verticesList = new List<Vector3>();
        float x;
        float y = 0.01f;
        float z;
        //outer ring
        for (int i = 0; i < edges; i++)
        {
            x = radius * Mathf.Sin((2 * Mathf.PI * i) / edges);
            z = radius * Mathf.Cos((2 * Mathf.PI * i) / edges);
            verticesList.Add(new Vector3(x, y, z));
        }
        //inner ring
        for (int i = 0; i < edges; i++)
        {
            x = (radius - ringWidth) * Mathf.Sin((2 * Mathf.PI * i) / edges);
            z = (radius - ringWidth) * Mathf.Cos((2 * Mathf.PI * i) / edges);
            verticesList.Add(new Vector3(x, y, z));
        }
        Vector3[] vertices = verticesList.ToArray();
        
        //tris
        List<int> trisList = new List<int>();
        for (int i = 0; i < (edges); i++)
        {
            trisList.Add(i);
            trisList.Add((i+1)%edges);
            if (isFilled) trisList.Add(edges);
            else trisList.Add(edges+i);
            
            if (isFilled) trisList.Add(edges);
            else trisList.Add(edges+i);
            trisList.Add((i+1)%edges);
            trisList.Add(i);

            if (isFilled) continue;
            if (i == edges-1)
            {
                trisList.Add(i+1);
                trisList.Add(edges+i);
                trisList.Add((i+edges+1)%(edges*2));
                trisList.Add((i+edges+1)%(edges*2));
                trisList.Add(edges+i);
                trisList.Add(i+1);
            }
            else
            {
                trisList.Add((i+edges+1)%(edges*2));
                trisList.Add(edges+i);
                trisList.Add(i+1);
                trisList.Add(i+1);
                trisList.Add(edges+i);
                trisList.Add((i+edges+1)%(edges*2));
            }
        }
        int[] tris = trisList.ToArray();
        
        //normals
        List<Vector3> normalsList = new List<Vector3>();
        for (int i = 0; i < vertices.Length; i++)
        {
            normalsList.Add(-Vector3.up);
        }
        Vector3[] normals = normalsList.ToArray();


        List<Vector2> uvsList = new List<Vector2>();
        for (int i = 0; i < vertices.Length; i++)
        {
            uvsList.Add(new Vector2(0.5f + (vertices[i].x)/(2*radius), 0.5f + (vertices[i].y)/(2*radius)));
        }
        Vector2[] uvs = uvsList.ToArray();

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.normals = normals;
        mesh.uv = uvs;

        return mesh;
        
    }
}
