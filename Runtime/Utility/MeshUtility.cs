using System;
using TMPro;

namespace BP.TextMotionPro
{
    internal static class MeshUtility
    {
        public static void UpdateMeshInfo(TMP_Text text, ref TMP_MeshInfo[] meshInfo)
        {
            if (meshInfo == null) return;
            TMP_TextInfo textInfo = text.textInfo;
            CopyMeshInfo(meshInfo, ref textInfo.meshInfo);
        }

        public static void CopyMeshInfo(TMP_MeshInfo[] src, ref TMP_MeshInfo[] dst)
        {
            for (int i = 0; i < src.Length; i++)
            {
                ref var srcMeshInfo = ref src[i];
                ref var dstMeshInfo = ref dst[i];
                CopyResizeArray(srcMeshInfo.uvs0, ref dstMeshInfo.uvs0);
                CopyResizeArray(srcMeshInfo.uvs2, ref dstMeshInfo.uvs2);
                CopyResizeArray(srcMeshInfo.vertices, ref dstMeshInfo.vertices);
                CopyResizeArray(srcMeshInfo.colors32, ref dstMeshInfo.colors32);
            }
        }

        public static void CopyResizeArray<T>(T[] src, ref T[] dst)
        {
            if (dst == null || dst.Length < src.Length)
                Array.Resize(ref dst, src.Length);

            Array.Copy(src, dst, src.Length);
        }
    }
}
