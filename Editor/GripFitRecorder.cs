using System.Linq;
using Sebanne.GripFit;
using UnityEditor;

namespace Sebanne.GripFit.Editor
{
    internal static class GripFitRecorder
    {
        private const string PendingSessionKey = "GripFit.PendingRecords";

        public static void RecordCurrentPose(GripFitOffset target)
        {
            var list = LoadPendingList();
            var existing = list.records.FirstOrDefault(r => r.targetInstanceId == target.GetInstanceID());
            var transform = target.transform;

            if (existing != null)
            {
                existing.position = transform.localPosition;
                existing.rotation = transform.localRotation;
            }
            else
            {
                list.records.Add(new GripFitPendingRecord
                {
                    targetInstanceId = target.GetInstanceID(),
                    position = transform.localPosition,
                    rotation = transform.localRotation,
                });
            }

            SavePendingList(list);
        }

        public static bool HasPendingRecord(GripFitOffset target)
        {
            return LoadPendingList().records.Any(r => r.targetInstanceId == target.GetInstanceID());
        }

        internal static GripFitPendingList LoadPendingList()
        {
            var json = SessionState.GetString(PendingSessionKey, "");
            if (string.IsNullOrEmpty(json))
            {
                return new GripFitPendingList();
            }

            return UnityEngine.JsonUtility.FromJson<GripFitPendingList>(json) ?? new GripFitPendingList();
        }

        internal static void SavePendingList(GripFitPendingList list)
        {
            SessionState.SetString(PendingSessionKey, UnityEngine.JsonUtility.ToJson(list));
        }

        internal static void ClearPendingList()
        {
            SessionState.EraseString(PendingSessionKey);
        }
    }

    [InitializeOnLoad]
    internal static class GripFitAutoCommit
    {
        static GripFitAutoCommit()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            if (change != PlayModeStateChange.EnteredEditMode)
            {
                return;
            }

            var list = GripFitRecorder.LoadPendingList();
            if (list.records.Count == 0)
            {
                return;
            }

            foreach (var record in list.records)
            {
                if (!(EditorUtility.InstanceIDToObject(record.targetInstanceId) is GripFitOffset offset))
                {
                    continue;
                }

                Undo.RecordObject(offset, "Grip Fit: Commit Recorded Pose");
                offset.OffsetPosition = record.position;
                offset.OffsetRotation = record.rotation;
                offset.HasRecordedValue = true;
                EditorUtility.SetDirty(offset);
            }

            GripFitRecorder.ClearPendingList();
        }
    }
}
