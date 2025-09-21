using BP.TextMotionPro.Parsing;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BP.TextMotionPro
{
    [AddComponentMenu("TextMotionPro/TextMotionPro"), RequireComponent(typeof(TMP_Text))]
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public class TextMotionPro : MonoBehaviour
    {
        internal readonly struct ResolvedTagComponents
        {
            public readonly TextComponent[] Components;

            public ResolvedTagComponents(TextComponent[] components)
            {
                Components = components;
            }
        }
        private Dictionary<TagRange, ResolvedTagComponents> rangeComponentCache = new();


        public enum UpdateMode
        {
            RuntimeOnly,
            RuntimeAndEditor,
            RuntimeAndSelected
        }

        [SerializeField] private MotionProfile profile;
        [SerializeField] private UpdateMode updateMode = UpdateMode.RuntimeAndSelected;
        [SerializeField] private float timeScale = 1.0f;
        [SerializeField] private int frameRate = 24;

        private float timeSinceStart;
        private float animationTime = 0;
        private float lastUpdateTime;
        private TMP_MeshInfo[] meshInfoCopy;

        private PreProcessor preprocessor;
        internal PreProcessor Processor => preprocessor ??= new PreProcessor();

        private TMP_Text textComponent;
        public TMP_Text TextComponent => textComponent == null ? textComponent = GetComponent<TMP_Text>() : textComponent;

        private bool isDirty;

        private void OnEnable()
        {
            timeSinceStart = 0;
            animationTime = 0;
            lastUpdateTime = 0;

            Processor.ClearCache();
            TextComponent.textPreprocessor = Processor;
            EditorHelper.EditorUpdate += EditorUpdate;
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChange);

            TextComponent.ForceMeshUpdate();
            CopyMeshInfo();
            RenderUpdate();
        }
        private void OnDisable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChange);
            EditorHelper.EditorUpdate -= EditorUpdate;
            TextComponent.textPreprocessor = null;
            TextComponent.ForceMeshUpdate(true, true);
        }

        private void OnValidate()
        {
            timeScale = Mathf.Clamp(timeScale, 0, 100);
            frameRate = Mathf.Clamp(frameRate, 0, 120);

            if (!CanUpdate())
            {
                ResetMeshIfDirty();
            }
        }

        private void Update()
        {
            if (!Application.isPlaying)
                return;

            CoreUpdate();
        }

        private void OnTextChange(Object obj)
        {
            if (obj != TextComponent) return;
            CopyMeshInfo();
            RenderUpdate();
        }

        private void CopyMeshInfo() => meshInfoCopy = TextComponent.textInfo.CopyMeshInfoVertexData();

        private void EditorUpdate()
        {

            if (Application.isPlaying)
                return;

            if (!CanUpdate())
            {
                ResetMeshIfDirty();
                return;
            }

            CoreUpdate();
            EditorHelper.QueueEditorUpdate();
        }

        public void CoreUpdate()
        {
            if (!TextComponent || !profile)
                return;

            timeSinceStart += Time.deltaTime;
            float targetUpdateInterval = 1f / frameRate;
            float timeSinceLastUpdate = timeSinceStart - lastUpdateTime;

            if (timeSinceLastUpdate >= targetUpdateInterval)
            {
                int updateCount = Mathf.FloorToInt(timeSinceLastUpdate / targetUpdateInterval);
                animationTime += updateCount * targetUpdateInterval * timeScale;
                RenderUpdate();
                lastUpdateTime = timeSinceStart;
            }
        }

        public void RenderUpdate()
        {
            if (!TextComponent || !profile)
                return;

            var charInfo = TextComponent.textInfo.characterInfo;
            TagRange currentRange = default;
            List<TextComponent> currentComponents = null;
            var meshInfoArray = TextComponent.textInfo.meshInfo;

            for (int i = 0; i < charInfo.Length; i++)
            {
                ref TMP_CharacterInfo character = ref charInfo[i];
                if (!character.isVisible)
                    continue;


                if (!currentRange.Contains(character.index))
                {
                    if (!Processor.TryGetRangeAt(character.index, out currentRange))
                    {
                        currentComponents = null;
                        currentRange = default;
                        continue;
                    }

                    var tags = currentRange.Tags;
                    currentComponents ??= new List<TextComponent>(tags.Count);
                    currentComponents.Clear();

                    for (int j = 0; j < tags.Count; j++)
                    {
                        var tag = tags[j];
                        if (profile.TagEffects.TryGetComponent(tag.Name, out var component))
                        {
                            currentComponents.Add(component);
                        }
                    }
                }

                var meshInfo = meshInfoArray[character.materialReferenceIndex];
                var charMod = new CharMod(
                    meshInfo.vertices,
                    meshInfo.colors32,
                    character.vertexIndex
                );

                var charState = new CharState(character.index, animationTime);
                if (currentComponents != null)
                {
                    for (int j = 0; j < currentComponents.Count; j++)
                    {
                        currentComponents[j].Apply(charMod, charState);
                    }
                }
            }

            TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            MeshUtility.UpdateMeshInfo(textComponent, ref meshInfoCopy);
            isDirty = true;
        }

        private bool CanUpdate()
        {
            return updateMode == UpdateMode.RuntimeAndEditor ||
           (updateMode == UpdateMode.RuntimeAndSelected && EditorHelper.IsSelected(gameObject));
        }

        private void ResetMeshIfDirty()
        {
            if (!isDirty) return;
            MeshUtility.UpdateMeshInfo(textComponent, ref meshInfoCopy);
            TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            isDirty = false;
        }
    }
}
