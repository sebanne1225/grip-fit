# Grip Fit

握り拳などのハンドジェスチャー状態で、手に持たせる武器/小物の Transform を Modular Avatar Bone Proxy 基準に合わせ、非破壊で保存するツールです。

## 何ができるか

- Play Mode 中にジェスチャーを再現しながら Bone Proxy 先を目視で位置合わせし、その場で姿勢を記録できます
- Play Mode 終了時に記録した姿勢を自動的に保存します（明示操作は Play Mode 中の「記録」ボタンのみ）
- ビルド時（NDMF）に非破壊で位置を反映します。Edit Mode 上の Transform は書き換えません
- 複数のギミック（両手武器など）を同時に記録できます
- 元のアバターには直接変更を加えない非破壊設計です

## 対応環境

- Unity `2022.3`
- VRChat SDK（Avatars）
- Modular Avatar（`nadena.dev.modular-avatar`）
- VCC / VPM ベースの VRChat プロジェクトを推奨します

## VCC / VPM での導入

### 推奨: VCC / VPM から導入

1. VCC に追加する URL として `https://sebanne1225.github.io/sebanne-listing/index.json` を追加します。
2. package 一覧から `Grip Fit` (`com.sebanne.grip-fit`) を追加します。
3. Unity を開き、package が導入されていることを確認します。

参考ページ (`VCC` 追加先ではありません): `https://sebanne1225.github.io/sebanne-listing/`

### 補助: Git URL / Release zip から導入

- repo: `https://github.com/sebanne1225/grip-fit`
- Git URL や local package での導入は、開発確認や手動検証向けの補助導線です
- GitHub Release の zip も補助導線として使えます。`com.sebanne.grip-fit-<version>.zip` を展開すると、直下に `package.json` が見える package 構成です

## 使い方

1. 位置を合わせたいギミック側 GameObject に `Modular Avatar Bone Proxy`（attachmentMode: Keep World Pose）を設定します
2. 同じ GameObject に `Grip Fit Offset` コンポーネントを追加します
3. Play Mode に入り、Gesture Manager 等でジェスチャーを再現しながら Bone Proxy 先を動かして位置を合わせます
4. Inspector の「現在の姿勢を記録」ボタンを押します（複数ギミックがある場合はそれぞれで押せます）
5. Play Mode を終了します（記録した姿勢が自動的に確定します）
6. 次回のビルド（Play Mode 再突入 / アップロード）から、記録した姿勢が反映されます

## 制限事項

- 素の Unity Play ボタンでのテスト（NDMF Apply on Play 経由）を前提としています。VRCSDK の「Build & Test」経由での記録は動作保証外です
- `Modular Avatar Bone Proxy` の attachmentMode は `Keep World Pose` を想定しています

## ライセンス

MIT License です。詳細は `LICENSE` を参照してください。
