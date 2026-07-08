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
                        if (!offset.HasRecordedValue)
                        {
                            continue;
                        }

                        var t = offset.transform;
                        t.localPosition = offset.OffsetPosition;
                        t.localRotation = offset.OffsetRotation;
                    }
                });
        }
    }
}
