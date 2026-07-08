using nadena.dev.modular_avatar.core;
using nadena.dev.ndmf;
using UnityEngine;

namespace Sebanne.GripFit
{
    // INDMFEditorOnly（= VRChat の IEditorOnly）: SDK パネル検証（excludeEditorOnly）を通してアップロード
    // ブロックを避ける。実ビルド／Play とも MA の ReplacementRemoveIEditorOnly が component を除去するため、
    // Play 中は存在しなくなる（記録は「Grip Fit 記録」ウィンドウが GameObject の transform を読む）。
    [AddComponentMenu("Sebanne/Grip Fit Offset")]
    public sealed class GripFitOffset : MonoBehaviour, INDMFEditorOnly
    {
        [SerializeField] private Vector3 offsetPosition;
        [SerializeField] private Quaternion offsetRotation = Quaternion.identity;
        [SerializeField] private bool hasRecordedValue;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (GetComponent<ModularAvatarBoneProxy>() != null)
            {
                return;
            }

            const string message =
                "Grip Fit Offset には Modular Avatar Bone Proxy が同じ GameObject に必要です。\n" +
                "Bone Proxy を先に付けてから Grip Fit Offset を追加してください。\n" +
                "Grip Fit Offset を取り除きます。";

            Debug.LogError(message, this);

            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (this == null)
                {
                    return;
                }

                UnityEditor.EditorUtility.DisplayDialog("Grip Fit", message, "OK");
                DestroyImmediate(this);
            };
        }
#endif

        public Vector3 OffsetPosition
        {
            get => offsetPosition;
            set => offsetPosition = value;
        }

        public Quaternion OffsetRotation
        {
            get => offsetRotation;
            set => offsetRotation = value;
        }

        public bool HasRecordedValue
        {
            get => hasRecordedValue;
            set => hasRecordedValue = value;
        }
    }
}
