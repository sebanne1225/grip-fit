using System.Collections.Generic;
using Sebanne.GripFit;
using UnityEditor;
using UnityEngine;

namespace Sebanne.GripFit.Editor
{
    /// <summary>
    /// Grip Fit の記録ウィンドウ。GripFitOffset は INDMFEditorOnly のため Play 突入時に strip され、
    /// component の Inspector が消える。そこで残る GameObject の live transform を読んで記録する。
    /// Edit Mode で対象を列挙し、ExitingEditMode でスナップショット（Play 中は列挙不能なため）。
    /// </summary>
    internal sealed class GripFitRecorderWindow : EditorWindow
    {
        private const string SnapshotSessionKey = "GripFit.TargetSnapshot";

        [MenuItem("Tools/Sebanne/Grip Fit 記録")]
        internal static void Open()
        {
            var window = GetWindow<GripFitRecorderWindow>();
            window.titleContent = new GUIContent("Grip Fit 記録");
            window.Show();
        }

        [System.Serializable]
        private sealed class TargetEntry
        {
            public int gameObjectInstanceId;
            public string displayName;
        }

        [System.Serializable]
        private sealed class TargetSnapshot
        {
            public List<TargetEntry> entries = new List<TargetEntry>();
        }

        // Play 突入前に対象 GameObject をスナップショットする（Play 中は GripFitOffset が strip され列挙不能）。
        [InitializeOnLoadMethod]
        private static void RegisterSnapshotHook()
        {
            EditorApplication.playModeStateChanged += change =>
            {
                if (change == PlayModeStateChange.ExitingEditMode)
                {
                    SnapshotTargets();
                }
            };
        }

        private static void SnapshotTargets()
        {
            var snapshot = new TargetSnapshot();
            foreach (var offset in UnityEngine.Object.FindObjectsOfType<GripFitOffset>(true))
            {
                snapshot.entries.Add(new TargetEntry
                {
                    gameObjectInstanceId = offset.gameObject.GetInstanceID(),
                    displayName = offset.gameObject.name,
                });
            }

            SessionState.SetString(SnapshotSessionKey, JsonUtility.ToJson(snapshot));
        }

        private static TargetSnapshot LoadSnapshot()
        {
            var json = SessionState.GetString(SnapshotSessionKey, "");
            if (string.IsNullOrEmpty(json))
            {
                return new TargetSnapshot();
            }

            return JsonUtility.FromJson<TargetSnapshot>(json) ?? new TargetSnapshot();
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        }

        private void OnPlayModeChanged(PlayModeStateChange change)
        {
            Repaint();
        }

        private void OnGUI()
        {
            if (EditorApplication.isPlaying)
            {
                DrawPlayMode();
            }
            else
            {
                DrawEditMode();
            }
        }

        private void DrawEditMode()
        {
            EditorGUILayout.HelpBox(
                "Grip Fit Offset と同じ GameObject の Modular Avatar Bone Proxy は、配置モードを" +
                "「子として・ワールドの位置と向きを維持」にしてください。\n" +
                "このウィンドウを開いたまま Play モードに入り、位置を合わせたい状態にしてください" +
                "（EX メニューで武器を出す、ハンドジェスチャーを変更するなど）。",
                MessageType.Info);

            var offsets = UnityEngine.Object.FindObjectsOfType<GripFitOffset>(true);
            if (offsets.Length == 0)
            {
                EditorGUILayout.LabelField("シーンに Grip Fit Offset がありません。");
                return;
            }

            EditorGUILayout.LabelField($"対象: {offsets.Length} 件", EditorStyles.boldLabel);
            foreach (var offset in offsets)
            {
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
                {
                    EditorGUILayout.ObjectField(offset.gameObject, typeof(GameObject), true);
                    EditorGUILayout.LabelField(
                        offset.HasRecordedValue ? "確定済み" : "未記録",
                        GUILayout.Width(60));
                }
            }
        }

        private void DrawPlayMode()
        {
            var snapshot = LoadSnapshot();
            if (snapshot.entries.Count == 0)
            {
                EditorGUILayout.HelpBox(
                    "記録対象がありません。Edit Mode でこのウィンドウを開いた状態から Play に入ってください。",
                    MessageType.Warning);
                return;
            }

            EditorGUILayout.HelpBox(
                "記録の手順:\n" +
                "1. 位置を合わせたい状態にする（EX メニューで武器を出す、ハンドジェスチャーを変更するなど）\n" +
                "2. 下のフィールドで対象を選び、Scene ビューで位置・角度を合わせる\n" +
                "3.「記録」を押す（複数あるときは「すべての相対位置を記録」）\n" +
                "4. Play を抜けると確定します",
                MessageType.Info);

            if (GUILayout.Button("すべての相対位置を記録"))
            {
                foreach (var entry in snapshot.entries)
                {
                    RecordEntry(entry);
                }
            }

            EditorGUILayout.Space();

            foreach (var entry in snapshot.entries)
            {
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
                {
                    var go = EditorUtility.InstanceIDToObject(entry.gameObjectInstanceId) as GameObject;
                    if (go == null)
                    {
                        EditorGUILayout.LabelField($"{entry.displayName}（見つかりません）");
                        continue;
                    }

                    // Edit Mode の一覧と同じく ObjectField で対象を示す（統一感）。クリックで Hierarchy 上の
                    // 対象をハイライトでき、Scene ビューで transform を合わせられる。Play 中は BoneProxy
                    // 配下へ reparent され Hierarchy から探しにくいため、この導線が要る。
                    EditorGUILayout.ObjectField(go, typeof(GameObject), true);

                    EditorGUILayout.LabelField(
                        GripFitRecorder.HasPendingRecord(entry.gameObjectInstanceId) ? "記録済み" : "",
                        GUILayout.Width(60));

                    if (GUILayout.Button("記録", GUILayout.Width(60)))
                    {
                        RecordEntry(entry);
                    }
                }
            }
        }

        private static void RecordEntry(TargetEntry entry)
        {
            var go = EditorUtility.InstanceIDToObject(entry.gameObjectInstanceId) as GameObject;
            if (go == null)
            {
                return;
            }

            var t = go.transform;
            GripFitRecorder.RecordPose(entry.gameObjectInstanceId, t.localPosition, t.localRotation);
        }
    }
}
