## Goal

VRChat アバターの握り拳などのハンドジェスチャー状態で、手に持たせる武器/小物の Transform を Modular Avatar Bone Proxy 基準に合わせ、非破壊で保存する NDMF プラグイン。

## Current State

0.1.2 patch リリース完了（2026-07-08・VPM）。実機アップロード成功確認済み。**0.1.1 の validation blocker は実は解けておらず**（`!isPlayingOrWillChangePlaymode` 除去は的外れ）、0.1.2 で真因解消: 非ホワイトリストのブロックは NDMF ビルドでなく **SDK パネル検証**（`AvatarValidation.FindIllegalComponents` を元アバターに走らせる段階）で起きるため、`GripFitOffset` を **`INDMFEditorOnly` 化**して検証対象外にした。IEditorOnly は Play 突入時に MA `ReplacementRemoveIEditorOnly` で剥がれるので、記録は専用ウィンドウ「Tools > Sebanne > Grip Fit 記録」が GameObject の live transform を読む方式へ移設（詳細 = knowledge-base `notes/technical/unity-ndmf.md` 前例節）。default branch は main。BOOTH は 0.1.0 据え置き。関連セッション = `2026-07-08_grip-fit-0.1.2-panel-block-fix`（+ mvp-implementation / initial-release / 0.1.1-editoronly-fix）。

## Current Blocker

なし。

## Tasks

- 0.1.2 リリース済み（VPM）。BOOTH は 0.1.0 据え置き（せ判断・また変更あるかもなので作り込まない）。後続機能候補 = TD-GF-T2/T3（knowledge-base next-phase/tool-dev.md「## Grip Fit」節）

## Rules

- 非破壊を最優先にし、既存データや既存設定を直接書き換える前に確認手段を用意する
- Editor ファイルの namespace は `Sebanne.GripFit.Editor`、Runtime ファイルの namespace は `Sebanne.GripFit` に統一する
- 適用（ビルド時反映）は NDMF カスタムパスで行い、Edit Mode の Transform を直接書き換えない（真の非破壊設計。設計経緯は knowledge-base `next-phase/details/TD-GF-T1.md` 参照）

## ファイル構成

- `Runtime/GripFitOffset.cs` — データコンポーネント（offsetPosition/offsetRotation/hasRecordedValue）+ **`INDMFEditorOnly`**（SDK パネル検証を通す・0.1.2）。`OnValidate` で `ModularAvatarBoneProxy` 非同居なら自身を自動除去（Runtime asmdef 参照 = `nadena.dev.modular-avatar.core` + `nadena.dev.ndmf.runtime`）
- `Editor/GripFitPendingList.cs` — SessionState 用の複数ギミック対応ペンディングリスト（JsonUtility シリアライズ）
- `Editor/GripFitRecorder.cs` — 記録 + Play→Edit をまたぐ自動確定（`[InitializeOnLoad]`）。記録キーは **GameObject InstanceID**（IEditorOnly で Play 中 component が消えるため・0.1.2）
- `Editor/GripFitRecorderWindow.cs` — **記録専用 EditorWindow**（`Tools > Sebanne > Grip Fit 記録`・0.1.2）。`ExitingEditMode` で対象 GameObject をスナップショット、Play 中に GameObject の live transform を記録
- `Editor/GripFitOffsetEditor.cs` — `GripFitOffset` の Custom Inspector（記録ウィンドウを開くボタン + 確定値表示）
- `Editor/GripFitPlugin.cs` — NDMF Plugin。`InPhase(BuildPhase.Transforming).AfterPlugin("nadena.dev.modular-avatar")` で offset を非破壊適用（component 除去は IEditorOnly strip が担うため自前 DestroyImmediate なし・0.1.2）

## 次回再開用の要点

- 設計正本は knowledge-base `next-phase/details/TD-GF-T1.md`（対応付け問題の解消・NDMF Plugin 順序制御の一次ソース確認済み）
- 実装計画は knowledge-base `plans/029_td-gf-t1-grip-fit-mvp/`（research.md / plan.md）
- 参考実装: `Repos/afk-manager/Editor/AfkManagerPlugin.cs`（同型の NDMF 2 パス構成）

## 技術知見（任意）

- MA 側の QualifiedName は `"nadena.dev.modular-avatar"`（`PluginDefinition.cs` で確認済み）。`BoneProxyPluginPass` は同じ `BuildPhase.Transforming` 内で MA 自身の Sequence に登録されているため、`AfterPlugin("nadena.dev.modular-avatar")` で BoneProxy の reparent 処理後に確実に実行される
- 素の Play ボタン（NDMF `ApplyOnPlay`）は avatar を複製せず in-place でビルドする。BoneProxy 対象オブジェクトは同一インスタンスのまま reparent されるだけなので、Edit Mode で保持したオブジェクト参照は Play Mode 突入後もそのまま有効（GUID/パス対応不要）
- **`IEditorOnly` は Play Mode 中に剥がれる（MA `ReplacementRemoveIEditorOnly` @ `callbackOrder=MaxValue`）**: `GripFitOffset`（`INDMFEditorOnly`）は素の Play ボタンでも `OnPreprocessAvatar` 経由で早期に破棄され、component の Inspector で記録する UI は Play 中に対象を見失う（Edit 復帰で巻き戻し復元されるため気付きにくい）。NDMF 自身の `RemoveEditorOnlyPass` は GameObject の `"EditorOnly"` **タグ**専用で無関係（混同注意）。**0.1.2 の解**: それでも `IEditorOnly` は必須（SDK パネル検証 `FindIllegalComponents` を通す唯一のレバー・非ホワイトリストブロックは NDMF ビルドでなくパネル検証で起きる）なので付けたまま、記録を専用ウィンドウ（GameObject の live transform を読む）へ移した。真因・全体像は knowledge-base `notes/technical/unity-ndmf.md` 前例節が正本

- **BoneProxy 前提の強制は `RequireComponent` でなく自己除去**: `[RequireComponent(typeof(ModularAvatarBoneProxy))]` は誤操作時に BoneProxy まで巻き添えで自動付与してしまう。`OnValidate` + `EditorApplication.delayCall` での自己 `DestroyImmediate`（+ Console エラー + `EditorUtility.DisplayDialog`）なら、誤って別オブジェクトに追加してもコンポーネントが増えず即座に取り除かれる

## 次フェーズ候補

knowledge-base `next-phase/tool-dev.md`「Grip Fit」節（後回しの正本、TD-GF-T2/T3 = 対象を探す専用ウィンドウ + VRCFury 対応）を参照。
