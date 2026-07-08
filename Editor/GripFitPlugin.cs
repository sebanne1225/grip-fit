using nadena.dev.ndmf;
using Sebanne.GripFit;
using UnityEngine;

[assembly: ExportsPlugin(typeof(Sebanne.GripFit.Editor.GripFitPlugin))]

namespace Sebanne.GripFit.Editor
{
    public sealed class GripFitPlugin : Plugin<GripFitPlugin>
    {
        public override string DisplayName => "Grip Fit";
        public override string QualifiedName => "com.sebanne.grip-fit";

        protected override void Configure()
        {
            InPhase(BuildPhase.Transforming)
                .AfterPlugin("nadena.dev.modular-avatar")
                .Run("Apply Grip Fit Offsets", ctx =>
                {
                    foreach (var offset in ctx.AvatarRootObject.GetComponentsInChildren<GripFitOffset>(true))
                    {
                        if (offset.HasRecordedValue)
                        {
                            var t = offset.transform;
                            t.localPosition = offset.OffsetPosition;
                            t.localRotation = offset.OffsetRotation;
                        }

                        // 実アップロードビルドでは VRCSDK の非ホワイトリスト検証を避けるため除去する。
                        // Play Mode（ApplyOnPlay）では記録のため残す。isPlayingOrWillChangePlaymode は
                        // Play 遷移中も true（NDMF の RuntimeUtil.IsPlaying と同一実装）。
                        if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                        {
                            Object.DestroyImmediate(offset);
                        }
                    }
                });
        }
    }
}
