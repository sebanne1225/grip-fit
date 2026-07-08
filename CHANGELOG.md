# Changelog

このファイルは `Grip Fit` の変更履歴を管理します。

## [0.1.2] - 2026-07-08

### Fixed

- 実アップロード時に `GripFitOffset` が VRChat SDK のコンポーネント検証で弾かれ、アバターのアップロードがブロックされる問題を修正（`INDMFEditorOnly` 化で検証対象外にした）。0.1.1 の除去方式は、検証が NDMF ビルドより手前の SDK パネル検証段階で走るため解消できていなかった

### Changed

- 記録を専用ウィンドウ「Tools > Sebanne > Grip Fit 記録」へ移設。Play モードで対象の位置・角度を合わせてから記録する（Grip Fit Offset の Inspector からもウィンドウを開ける）

## [0.1.1] - 2026-07-08

### Fixed

- 実アップロードビルド時に `GripFitOffset` を除去し、VRChat SDK の非ホワイトリストコンポーネント検証（「removed by the client」でアップロードがブロックされる問題）を修正。Play Mode 中は記録のため残す（`RuntimeUtil.IsPlaying` で判別）

## [0.1.0] - 2026-07-08

### Added

- `GripFitOffset` コンポーネント（記録した手首相対姿勢の保持）
- Play Mode 中の記録 + Play Mode 終了時の自動確定（複数ギミック対応）
- NDMF カスタムパスによる非破壊のビルド時反映
