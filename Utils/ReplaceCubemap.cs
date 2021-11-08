using UnityEngine;

namespace Evolve.Utils
{
    internal class ReplaceCubemap
    {
        internal static Cubemap BuildCubemap(Texture2D sourceTex)
        {
            ReplaceCubemap.CubemapResolution = sourceTex.width;
            Cubemap cubemap = new Cubemap(ReplaceCubemap.CubemapResolution, TextureFormat.RGBA32, false);
            ReplaceCubemap.source = sourceTex;
            for (int i = 0; i < 6; i++)
            {
                Color[] arr = ReplaceCubemap.CreateCubemapTexture(ReplaceCubemap.CubemapResolution, (CubemapFace) i);
                cubemap.SetPixels(arr, (CubemapFace) i);
            }
            cubemap.Apply();
            return cubemap;
        }

        private static Color[] CreateCubemapTexture(int resolution, CubemapFace face)
        {
            Texture2D texture2D = new Texture2D(resolution, resolution, TextureFormat.RGB24, false);
            Vector3 b = (ReplaceCubemap.faces[(int) face][1] - ReplaceCubemap.faces[(int) face][0]) / resolution;
            Vector3 b2 = (ReplaceCubemap.faces[(int) face][3] - ReplaceCubemap.faces[(int) face][2]) / resolution;
            float num = 1f / resolution;
            float num2 = 0f;
            Color[] array = new Color[resolution];
            for (int i = 0; i < resolution; i++)
            {
                Vector3 a = ReplaceCubemap.faces[(int) face][0];
                Vector3 vector = ReplaceCubemap.faces[(int) face][2];
                for (int j = 0; j < resolution; j++)
                {
                    array[j] = ReplaceCubemap.Project(Vector3.Lerp(a, vector, num2).normalized);
                    a += b;
                    vector += b2;
                }
                texture2D.SetPixels(0, i, resolution, 1, array, 0);
                num2 += num;
            }
            texture2D.wrapMode = TextureWrapMode.Clamp;
            texture2D.Apply();
            return texture2D.GetPixels();
        }

        private static Color Project(Vector3 direction)
        {
            float num = Mathf.Atan2(direction.z, direction.x) + 0.0174532924f;
            float num2 = Mathf.Acos(direction.y);
            int num3 = (int) ((num / 3.1415925f * 0.5f + 0.5f) * source.width);
            if (num3 < 0)
            {
                num3 = 0;
            }
            if (num3 >= ReplaceCubemap.source.width)
            {
                num3 = ReplaceCubemap.source.width - 1;
            }
            int num4 = (int) (num2 / 3.1415925f * source.height);
            if (num4 < 0)
            {
                num4 = 0;
            }
            if (num4 >= ReplaceCubemap.source.height)
            {
                num4 = ReplaceCubemap.source.height - 1;
            }
            return ReplaceCubemap.source.GetPixel(num3, ReplaceCubemap.source.height - num4 - 1);
        }

        public static int CubemapResolution = 256;

        private static Texture2D source;

        private static Vector3[][] faces = new Vector3[][]
        {
            new Vector3[]
            {
                new Vector3(1f, 1f, -1f),
                new Vector3(1f, 1f, 1f),
                new Vector3(1f, -1f, -1f),
                new Vector3(1f, -1f, 1f)
            },
            new Vector3[]
            {
                new Vector3(-1f, 1f, 1f),
                new Vector3(-1f, 1f, -1f),
                new Vector3(-1f, -1f, 1f),
                new Vector3(-1f, -1f, -1f)
            },
            new Vector3[]
            {
                new Vector3(-1f, 1f, 1f),
                new Vector3(1f, 1f, 1f),
                new Vector3(-1f, 1f, -1f),
                new Vector3(1f, 1f, -1f)
            },
            new Vector3[]
            {
                new Vector3(-1f, -1f, -1f),
                new Vector3(1f, -1f, -1f),
                new Vector3(-1f, -1f, 1f),
                new Vector3(1f, -1f, 1f)
            },
            new Vector3[]
            {
                new Vector3(-1f, 1f, -1f),
                new Vector3(1f, 1f, -1f),
                new Vector3(-1f, -1f, -1f),
                new Vector3(1f, -1f, -1f)
            },
            new Vector3[]
            {
                new Vector3(1f, 1f, 1f),
                new Vector3(-1f, 1f, 1f),
                new Vector3(1f, -1f, 1f),
                new Vector3(-1f, -1f, 1f)
            }
        };
    }
}