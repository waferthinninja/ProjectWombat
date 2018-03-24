using UnityEngine;

namespace Standard_Assets.Effects.ImageEffects.Scripts
{
    internal class Triangles
    {
        private static Mesh[] meshes;
        private static int currentTris;

        private static bool HasMeshes()
        {
            if (meshes == null)
                return false;
            for (var i = 0; i < meshes.Length; i++)
                if (null == meshes[i])
                    return false;

            return true;
        }

        private static void Cleanup()
        {
            if (meshes == null)
                return;

            for (var i = 0; i < meshes.Length; i++)
                if (null != meshes[i])
                {
                    Object.DestroyImmediate(meshes[i]);
                    meshes[i] = null;
                }

            meshes = null;
        }

        private static Mesh[] GetMeshes(int totalWidth, int totalHeight)
        {
            if (HasMeshes() && currentTris == totalWidth * totalHeight) return meshes;

            var maxTris = 65000 / 3;
            var totalTris = totalWidth * totalHeight;
            currentTris = totalTris;

            var meshCount = Mathf.CeilToInt(1.0f * totalTris / (1.0f * maxTris));

            meshes = new Mesh[meshCount];

            var i = 0;
            var index = 0;
            for (i = 0; i < totalTris; i += maxTris)
            {
                var tris = Mathf.FloorToInt(Mathf.Clamp(totalTris - i, 0, maxTris));

                meshes[index] = GetMesh(tris, i, totalWidth, totalHeight);
                index++;
            }

            return meshes;
        }

        private static Mesh GetMesh(int triCount, int triOffset, int totalWidth, int totalHeight)
        {
            var mesh = new Mesh();
            mesh.hideFlags = HideFlags.DontSave;

            var verts = new Vector3[triCount * 3];
            var uvs = new Vector2[triCount * 3];
            var uvs2 = new Vector2[triCount * 3];
            var tris = new int[triCount * 3];

            for (var i = 0; i < triCount; i++)
            {
                var i3 = i * 3;
                var vertexWithOffset = triOffset + i;

                var x = Mathf.Floor(vertexWithOffset % totalWidth) / totalWidth;
                var y = Mathf.Floor(vertexWithOffset / totalWidth) / totalHeight;

                var position = new Vector3(x * 2 - 1, y * 2 - 1, 1.0f);

                verts[i3 + 0] = position;
                verts[i3 + 1] = position;
                verts[i3 + 2] = position;

                uvs[i3 + 0] = new Vector2(0.0f, 0.0f);
                uvs[i3 + 1] = new Vector2(1.0f, 0.0f);
                uvs[i3 + 2] = new Vector2(0.0f, 1.0f);

                uvs2[i3 + 0] = new Vector2(x, y);
                uvs2[i3 + 1] = new Vector2(x, y);
                uvs2[i3 + 2] = new Vector2(x, y);

                tris[i3 + 0] = i3 + 0;
                tris[i3 + 1] = i3 + 1;
                tris[i3 + 2] = i3 + 2;
            }

            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.uv = uvs;
            mesh.uv2 = uvs2;

            return mesh;
        }
    }
}