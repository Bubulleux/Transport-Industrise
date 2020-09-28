using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData MeshGenerat(float[,] noise)
    {
        int width = noise.GetLength(0);
        int height = noise.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;
        MeshData mesh = new MeshData(width, height);

        int vertexIndex = 0;
        for (int y = 0;  y < height;  y++)
        {
            for (int x = 0; x < width; x++)
            {
                mesh.vertices[vertexIndex] = new Vector3((topLeftX + x) , Mathf.Floor(noise[x,y] * 4) / 2, (topLeftZ - y) );
                mesh.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);
                
                if (x < width -1 && y < height -1)
                {
                    mesh.AddTriangles(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    mesh.AddTriangles(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++;
            }
        }
        //mesh.RemakeMeshToDiscrete();
        return mesh;
    }

    
}
public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    public void AddTriangles(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public Mesh GetMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }

    public void RemakeMeshToDiscrete()
    {
        Vector3[] vertDiscrete = new Vector3[triangles.Length];
        int[] trigDiscrete = new int[triangles.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            vertDiscrete[i] = vertices[triangles[i]];
            trigDiscrete[i] = i;
        }
        vertices = vertDiscrete;
        triangles = trigDiscrete;
    }

}