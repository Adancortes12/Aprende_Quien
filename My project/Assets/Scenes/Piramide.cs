using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pyramid : MonoBehaviour
{
    void Start()
    {
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        // Define los vértices de la pirámide
        Vector3[] vertices = new Vector3[]
        {
            // Base (cuadrada)
            new Vector3(-1, 0, -1),
            new Vector3(1, 0, -1),
            new Vector3(1, 0, 1),
            new Vector3(-1, 0, 1),

            // Vértice superior (punta de la pirámide)
            new Vector3(0, 1, 0)
        };

        // Define los triángulos (lados de la pirámide)
        int[] triangles = new int[]
        {
            // Base
            0, 2, 1,
            0, 3, 2,

            // Lados (triángulos)
            0, 1, 4,
            1, 2, 4,
            2, 3, 4,
            3, 0, 4
        };

        // Aplica los vértices y triángulos al mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Recalcular normales para iluminación correcta
        mesh.RecalculateNormals();
    }
}

