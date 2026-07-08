# TOOL_INFO

このファイルは、`Grip Fit` の repo 補助文書です。README の代わりではなく、公開準備や listing 反映時に確認したい情報を短くまとめています。

## 基本情報

- ツール名: `Grip Fit`
- package名: `com.sebanne.grip-fit`
- 表示名: `Grip Fit`
- Runtime asmdef: `Sebanne.GripFit`
- Editor asmdef: `Sebanne.GripFit.Editor`
- 現在 version: `0.1.0`

## 公開メタ情報

- GitHub repo: `https://github.com/sebanne1225/grip-fit`
- changelogUrl: `https://github.com/sebanne1225/grip-fit/blob/main/CHANGELOG.md`
- listing repo: `https://github.com/sebanne1225/sebanne-listing`
- 参考 listing page (`VCC` 追加先ではない): `https://sebanne1225.github.io/sebanne-listing/`
- VCC に追加する URL: `https://sebanne1225.github.io/sebanne-listing/index.json`
- listing 側に追加する `githubRepos`: `sebanne1225/grip-fit`
- BOOTH 販売名: TBD

## 公開スコープの要約

- Play Mode 中に記録した握り拳等ジェスチャー時のギミック姿勢を、Play Mode 終了時に自動確定
- NDMF カスタムパスによる非破壊のビルド時反映（Edit Mode の Transform は書き換えない）
- 複数ギミック同時記録に対応

## 導入導線の前提

- 主導線は VCC / VPM
- Git URL / local package 導入は補助扱い

## 既知の制限

- 素の Play ボタン（NDMF Apply on Play）前提。VRCSDK Build & Test 経由での記録は動作保証外
- Modular Avatar Bone Proxy の attachmentMode は Keep World Pose を想定
