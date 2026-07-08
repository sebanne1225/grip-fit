using UnityEngine;

namespace Sebanne.GripFit
{
    [AddComponentMenu("Sebanne/Grip Fit Offset")]
    public sealed class GripFitOffset : MonoBehaviour
    {
        [SerializeField] private Vector3 offsetPosition;
        [SerializeField] private Quaternion offsetRotation = Quaternion.identity;
        [SerializeField] private bool hasRecordedValue;

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
