## Goal

VRChat アバターの握り拳などのハンドジェスチャー状態で、手に持たせる武器/小物の Transform を Modular Avatar Bone Proxy 基準に合わせ、非破壊で保存する NDMF プラグイン。

## Current State

MVP 実装・実機検証完了（2026-07-08）。`ririka_PFTest` シーンで左右手2ギミック同時の記録→自動確定→ビルド反映を実機で完全一致確認済み。BOOTH_PACKAGE/README 等の公開文書もプレースホルダ置換済みだが、公開作業（sync-check・リリース）は未着手。

## Current Blocker

なし。

## Tasks

- なし（MVP 完了）。公開準備は次フェーズ候補へ

## Rules

- 非破壊を最優先にし、既存データや既存設定を直接書き換える前に確認手段を用意する
- Editor ファイルの namespace は `Sebanne.GripFit.Editor`、Runtime ファイルの namespace は `Sebanne.GripFit` に統一する
- 適用（ビルド時反映）は NDMF カスタムパスで行い、Edit Mode の Transform を直接書き換えない（真の非破壊設計。設計経緯は knowledge-base `next-phase/details/TD-GF-T1.md` 参照）

## ファイル構成

- `Runtime/GripFitOffset.cs` — データコンポーネント（offsetPosition/offsetRotation/hasRecordedValue）
- `Editor/GripFitPendingList.cs` — SessionState 用の複数ギミック対応ペンディングリスト（JsonUtility シリアライズ）
- `Editor/GripFitRecorder.cs` — Play Mode 中の記録 + Play→Edit をまたぐ自動確定（`[InitializeOnLoad]`）
- `Editor/GripFitOffsetEditor.cs` — `GripFitOffset` の Custom Inspector
- `Editor/GripFitPlugin.cs` — NDMF Plugin。`InPhase(BuildPhase.Transforming).AfterPlugin("nadena.dev.modular-avatar")` でビルド時に非破壊適用

## 次回再開用の要点

- 設計正本は knowledge-base `next-phase/details/TD-GF-T1.md`（対応付け問題の解消・NDMF Plugin 順序制御の一次ソース確認済み）
- 実装計画は knowledge-base `plans/029_td-gf-t1-grip-fit-mvp/`（research.md / plan.md）
- 参考実装: `Repos/afk-manager/Editor/AfkManagerPlugin.cs`（同型の NDMF 2 パス構成）

## 技術知見（任意）

- MA 側の QualifiedName は `"nadena.dev.modular-avatar"`（`PluginDefinition.cs` で確認済み）。`BoneProxyPluginPass` は同じ `BuildPhase.Transforming` 内で MA 自身の Sequence に登録されているため、`AfterPlugin("nadena.dev.modular-avatar")` で BoneProxy の reparent 処理後に確実に実行される
- 素の Play ボタン（NDMF `ApplyOnPlay`）は avatar を複製せず in-place でビルドする。BoneProxy 対象オブジェクトは同一インスタンスのまま reparent されるだけなので、Edit Mode で保持したオブジェクト参照は Play Mode 突入後もそのまま有効（GUID/パス対応不要）
- **`VRC.SDKBase.IEditorOnly` は Play Mode 中に剥がれる罠**（実機検証で発見・修正済み）: `GripFitOffset` に付けると、素の Play ボタンでも VRCSDK の `OnPreprocessAvatar` 経路で早期に破棄され、Play Mode 中に記録しようとしても対象を見失う。Edit Mode 復帰で Play Mode 変更巻き戻しにより復元されるため気付きにくい。NDMF 自身の `RemoveEditorOnlyPass` は GameObject の `"EditorOnly"` **タグ**専用で無関係（混同注意）。Play Mode 中も生存させ続けたいコンポーネントには `IEditorOnly` を付けない。詳細は knowledge-base `next-phase/details/TD-GF-T1.md`「実装・実機検証」節参照

## 次フェーズ候補

knowledge-base `next-phase/tool-dev.md`「Grip Fit」節（後回しの正本）を参照。
