using nadena.dev.modular_avatar.core;
using UnityEngine;

namespace Sebanne.GripFit
{
    [AddComponentMenu("Sebanne/Grip Fit Offset")]
    public sealed class GripFitOffset : MonoBehaviour
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
