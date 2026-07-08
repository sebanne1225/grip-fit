using nadena.dev.ndmf;
using Sebanne.GripFit;

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

                        // component の除去は不要。GripFitOffset は INDMFEditorOnly なので、実ビルド／Play とも
                        // MA の ReplacementRemoveIEditorOnly（callbackOrder=MaxValue = 最後）が除去する。
                        // 本パスはそれより前に走るため offset を適用するだけでよい。
                    }
                });
        }
    }
}
