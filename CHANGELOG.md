# Changelog

このファイルは `Grip Fit` の変更履歴を管理します。

## [0.1.1] - 2026-07-08

### Fixed

- 実アップロードビルド時に `GripFitOffset` を除去し、VRChat SDK の非ホワイトリストコンポーネント検証（「removed by the client」でアップロードがブロックされる問題）を修正。Play Mode 中は記録のため残す（`RuntimeUtil.IsPlaying` で判別）

## [0.1.0] - 2026-07-08

### Added

- `GripFitOffset` コンポーネント（記録した手首相対姿勢の保持）
- Play Mode 中の記録 + Play Mode 終了時の自動確定（複数ギミック対応）
- NDMF カスタムパスによる非破壊のビルド時反映
