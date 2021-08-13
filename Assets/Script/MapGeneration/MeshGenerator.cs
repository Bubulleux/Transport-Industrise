using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Script.Mapping;
using UnityEngine;
using UnityEngine.UIElements;

namespace Script.MapGeneration
{
    public static class MeshGenerator
    {
        public static Mesh GetChunkMesh(Vector2Int chunk, Map map)
        {
            var meshData = new MeshData();
            for (var _y = 0; _y < Map.ChuckSize; _y++)
            {
                for (var _x = 0; _x < Map.ChuckSize; _x++)
                {
                    var x = chunk.x * Map.ChuckSize + _x;
                    var y = chunk.y * Map.ChuckSize + _y;

                    if (map.parcels[x, y].seeTerrain)
                    {
                        var cornerPos = new Vector3[]
                        {
                            new Vector3(map.parcels[x, y].pos.x + 1, map.parcels[x, y].corner[0], map.parcels[x, y].pos.y + 1),
                            new Vector3(map.parcels[x, y].pos.x + 1, map.parcels[x, y].corner[1], map.parcels[x, y].pos.y + 0),
                            new Vector3(map.parcels[x, y].pos.x + 0, map.parcels[x, y].corner[2], map.parcels[x, y].pos.y + 0),
                            new Vector3(map.parcels[x, y].pos.x + 0, map.parcels[x, y].corner[3], map.parcels[x, y].pos.y + 1),
                        };
                        meshData.AddTriangles(new Vector3[] { cornerPos[3], cornerPos[0], cornerPos[1] }, new Vector2Int(chunk.x, chunk.y));
                        meshData.AddTriangles(new Vector3[] { cornerPos[3], cornerPos[1], cornerPos[2] }, new Vector2Int(chunk.x, chunk.y));
                    }
                }
            }
            var mesh = meshData.GetMesh();
            return mesh;
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
                return;
            }
            for (var i = 0; i < 3; i++)
            {
                verticies.Add(corner[i]);
                uvs.Add(new Vector2((corner[i].x - chunk.x * Map.ChuckSize) / Map.ChuckSize,
                    (corner[i].z - chunk.y * Map.ChuckSize) / Map.ChuckSize));
                triangles.Add(verticies.Count - 1);
            }
        }

        public Mesh GetMesh()
        {
            // var verticesArray = new Vector3[verticies.Count];
            // var uvsArray = new Vector2[verticies.Count];
            // var triangleArray = new int[triangles.Count];
            // for (var i = 0; i < verticies.Count; i++)
            // {
            //     verticesArray[i] = verticies[i];
            //     uvsArray[i] = uvs[i];
            // }
            // for (var i = 0; i < triangles.Count; i++)
            // {
            //     triangleArray[i] = triangles[i];
            // }
            var mesh = new Mesh
            {
                indexFormat = UnityEngine.Rendering.IndexFormat.UInt32,
                vertices = verticies.ToArray(),
                triangles = triangles.ToArray(),
                uv = uvs.ToArray(),
            };
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}