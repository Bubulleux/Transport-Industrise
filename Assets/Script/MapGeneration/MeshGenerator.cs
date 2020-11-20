using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Threading.Tasks;

public static class MeshGenerator
{
    public static Mesh[,] MeshGenerat(Parcel[,] parcels)
    {
        int width = parcels.GetLength(0) / 50;
        int height = parcels.GetLength(1) / 50;
        Mesh[,] chunks = new Mesh[width, height];

        for (int y = 0;  y < height;  y++)
        {
            for (int x = 0; x < width; x++)
            {
                //chunks[x, y] = GenerateChunck(x, y, parcels);
            }
        }
        return chunks;
    }

    public static async Task AsyncGenerateChunck(int chunkX, int chunkY, Parcel[,] parcels, GameObject chunckGo)
    {
        MeshData chunk = new MeshData();
        for (int _y = 0; _y < 50; _y++)
        {
            for (int _x = 0; _x < 50; _x++)
            {
                int x = chunkX * 50 + _x;
                int y = chunkY * 50 + _y;
                
                if (parcels[x, y].seeTerrain)
                {
                    Vector3[] cornerPos = new Vector3[]
                    {
                        new Vector3(parcels[x, y].pos.x, parcels[x, y].corner[0], parcels[x, y].pos.y),
                        new Vector3(parcels[x, y].pos.x + 1, parcels[x, y].corner[1], parcels[x, y].pos.y),
                        new Vector3(parcels[x, y].pos.x, parcels[x, y].corner[2], parcels[x, y].pos.y + 1),
                        new Vector3(parcels[x, y].pos.x + 1, parcels[x, y].corner[3], parcels[x, y].pos.y + 1)
                    };  
                    chunk.AddTriangles(new Vector3[] { cornerPos[2], cornerPos[3], cornerPos[0] }, new Vector2Int(chunkX, chunkY));
                    chunk.AddTriangles(new Vector3[] { cornerPos[3], cornerPos[1], cornerPos[0] }, new Vector2Int(chunkX, chunkY));
                }
            }
            await Task.Delay(1);
        }
        Mesh mesh = await chunk.GetMesh();
        chunckGo.GetComponent<MeshFilter>().mesh = mesh;
        chunckGo.GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
public class MeshData
{
    public List<Vector3> verticies =  new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector2> uvs = new List<Vector2>();

    public void AddTriangles(Vector3[] corner, Vector2Int chunk)
    {
        if (corner.Length != 3)
        {
            Debug.Log("Triangle coner is not 3");
            return;
        }
        for (int i = 0; i < 3; i++)
        {
            verticies.Add(corner[i]);
            uvs.Add(new Vector2((corner[i].x - chunk.x * 50f) / 50f, (corner[i].z - chunk.y * 50f) / 50f));
            triangles.Add(verticies.Count - 1);
        }
    }

    public async Task<Mesh> GetMesh()
    {
        Vector3[] verticesArray = new Vector3[verticies.Count];
        Vector2[] uvsArray = new Vector2[verticies.Count];
        int[] triangleArray = new int[triangles.Count];
        for (int i = 0; i < verticies.Count; i++)
        {
            verticesArray[i] = verticies[i];
            uvsArray[i] = uvs[i];
            if (i % 50 == 0)
            {
                await Task.Delay(1);
            }
        }
        for (int i = 0; i < triangles.Count; i++)
        {
            triangleArray[i] = triangles[i];
            if (i % 50 == 0)
            {
                await Task.Delay(1);
            }
        }
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = verticesArray;
        mesh.triangles = triangleArray;
        mesh.uv = uvsArray;
        mesh.RecalculateNormals();
        return mesh;
    }
}