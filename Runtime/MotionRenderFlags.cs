using TMPro;

namespace BP.TextMotionPro
{
    internal struct MotionRenderFlags
    {
        private bool Vertices;
        private bool Uv0;
        private bool Colors32;

        public readonly TMP_VertexDataUpdateFlags ConsumeFlags()
        {
            TMP_VertexDataUpdateFlags flags = TMP_VertexDataUpdateFlags.None;
            if (Vertices) flags |= TMP_VertexDataUpdateFlags.Vertices;
            if (Uv0) flags |= TMP_VertexDataUpdateFlags.Uv0;
            if (Colors32) flags |= TMP_VertexDataUpdateFlags.Colors32;
            return flags;
        }

        public void AddVertices() => Vertices = true;
        public void AddUV0() => Uv0 = true;
        public void AddColors() => Colors32 = true;
    }
}
