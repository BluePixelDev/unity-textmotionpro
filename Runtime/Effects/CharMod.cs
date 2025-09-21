using System.Runtime.CompilerServices;
using UnityEngine;

namespace BP.TextMotionPro
{
    public enum Corner { TopLeft, TopRight, BottomLeft, BottomRight }
    /// <summary>
    /// Handles manipulation of currently processed character.
    /// </summary>
    public readonly ref struct CharMod
    {
        private readonly Vector3[] vertices;
        private readonly Color32[] colors;
        private readonly int vertexIndex;

        internal CharMod(in Vector3[] vertexArray, in Color32[] colorArray, int index)
        {
            vertexIndex = index;
            vertices = vertexArray;
            colors = colorArray;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void MoveY(float delta)
        {
            Move(0, delta);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void MoveX(float delta)
        {
            Move(delta, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Move(float dx, float dy, float dz = 0f)
        {
            ref Vector3 v0 = ref vertices[vertexIndex];
            ref Vector3 v1 = ref vertices[vertexIndex + 1];
            ref Vector3 v2 = ref vertices[vertexIndex + 2];
            ref Vector3 v3 = ref vertices[vertexIndex + 3];

            v0.x += dx; v0.y += dy; v0.z += dz;
            v1.x += dx; v1.y += dy; v1.z += dz;
            v2.x += dx; v2.y += dy; v2.z += dz;
            v3.x += dx; v3.y += dy; v3.z += dz;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Scale(Vector2 factor)
        {
            ref Vector3 bottomLeftVertex = ref vertices[vertexIndex];
            float cx = bottomLeftVertex.x;
            float cy = bottomLeftVertex.y;

            for (int i = 0; i < 4; i++)
            {
                ref Vector3 v = ref vertices[vertexIndex + i];
                v.x = cx + (v.x - cx) * factor.x;
                v.y = cy + (v.y - cy) * factor.y;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void AddVertPos(Corner corner, Vector2 delta)
        {
            int cornerIndex = (int)corner;
            vertices[vertexIndex + cornerIndex] += (Vector3)delta;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void RotateAroundPoint(Vector2 point, float angle)
        {
            float rad = angle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);

            for (int i = 0; i < 4; i++)
            {
                ref Vector3 v = ref vertices[vertexIndex + i];
                v.x -= point.x;
                v.y -= point.y;
                float x = v.x * cos - v.y * sin;
                float y = v.x * sin + v.y * cos;
                v.x = x + point.x;
                v.y = y + point.y;
                vertices[vertexIndex + i] = v;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void SetColor(byte r, byte g, byte b, byte a)
        {
            var targetColor = new Color32(r, g, b, a);
            colors[vertexIndex] = targetColor;
            colors[vertexIndex + 1] = targetColor;
            colors[vertexIndex + 2] = targetColor;
            colors[vertexIndex + 3] = targetColor;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void SetColor(byte r, byte g, byte b)
        {
            ref var c0 = ref colors[vertexIndex];
            ref var c1 = ref colors[vertexIndex + 1];
            ref var c2 = ref colors[vertexIndex + 2];
            ref var c3 = ref colors[vertexIndex + 3];

            c0 = new Color32(r, g, b, c0.a);
            c1 = new Color32(r, g, b, c1.a);
            c2 = new Color32(r, g, b, c2.a);
            c3 = new Color32(r, g, b, c3.a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FastHSVToRGB(float h, out byte r, out byte g, out byte b)
        {
            h = h % 1f;
            float rf = Mathf.Clamp01(Mathf.Abs(h * 6f - 3f) - 1f);
            float gf = Mathf.Clamp01(2f - Mathf.Abs(h * 6f - 2f));
            float bf = Mathf.Clamp01(2f - Mathf.Abs(h * 6f - 4f));

            r = (byte)(rf * 255);
            g = (byte)(gf * 255);
            b = (byte)(bf * 255);
        }
    }
}
