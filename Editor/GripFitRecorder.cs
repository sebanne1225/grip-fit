using System.Linq;
using Sebanne.GripFit;
using UnityEditor;
using UnityEngine;

namespace Sebanne.GripFit.Editor
{
    internal static class GripFitRecorder
    {
        private const string PendingSessionKey = "GripFit.PendingRecords";

        // Play 中は GripFitOffset が INDMFEditorOnly として strip されるため、記録は残る GameObject を
        // キーにする（targetInstanceId = GameObject の InstanceID）。値は BoneProxy 配下の live localPose。
        public static void RecordPose(int gameObjectInstanceId, Vector3 position, Quaternion rotation)
        {
            var list = LoadPendingList();
            var existing = list.records.FirstOrDefault(r => r.targetInstanceId == gameObjectInstanceId);

            if (existing != null)
            {
                existing.position = position;
                existing.rotation = rotation;
            }
            else
            {
                list.records.Add(new GripFitPendingRecord
                {
                    targetInstanceId = gameObjectInstanceId,
                    position = position,
                    rotation = rotation,
                });
            }

            SavePendingList(list);
        }

        public static bool HasPendingRecord(int gameObjectInstanceId)
        {
            return LoadPendingList().records.Any(r => r.targetInstanceId == gameObjectInstanceId);
        }

        internal static GripFitPendingList LoadPendingList()
        {
            var json = SessionState.GetString(PendingSessionKey, "");
            if (string.IsNullOrEmpty(json))
            {
                return new GripFitPendingList();
            }

            return JsonUtility.FromJson<GripFitPendingList>(json) ?? new GripFitPendingList();
        }

        internal static void SavePendingList(GripFitPendingList list)
        {
            SessionState.SetString(PendingSessionKey, JsonUtility.ToJson(list));
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
                // targetInstanceId = GameObject の InstanceID（Play 出入りで保持）→ Edit Mode で復元された
                // GripFitOffset へ確定する。
                if (!(EditorUtility.InstanceIDToObject(record.targetInstanceId) is GameObject go))
                {
                    continue;
                }

                var offset = go.GetComponent<GripFitOffset>();
                if (offset == null)
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
