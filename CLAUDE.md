> このファイルは雛形です。新しいツール repo を作る時は、このファイルをコピーして
> 各セクションの内容をそのツール固有の内容に書き換えてください。

## テンプレからの立ち上げ手順

1. この repo をコピーして新しい repo を作成する
2. 以下の置換ポイントをツール固有の値に置き換える
3. Unity プロジェクトに読み込んで動作確認する
4. ToolTemplateCheckWindow.cs を削除し、ツール固有の実装を始める

### 置換ポイント一覧

| プレースホルダ | 説明 | 例 |
|---|---|---|
| `{{ToolName}}` | 表示名 | `Flipbook Material Generator` |
| `{{repo-name}}` | repo 名・URL | `flipbook-material-generator` |
| `{{package-id}}` | package.json name | `com.sebanne.flipbook-material-generator` |
| `{{ToolId}}` | asmdef・namespace | `FlipbookMaterialGenerator` |
| `{{description}}` | 日本語一言説明 | `VRChat 向けフリップブック素材自動生成ツール` |

### 置換対象ファイル

- `package.json` — name, displayName, description, URL 群
- `Editor/Sebanne.ToolTemplate.Editor.asmdef` — `Sebanne.{{ToolId}}.Editor` にリネーム + name 変更
- `Runtime/Sebanne.ToolTemplate.asmdef` — `Sebanne.{{ToolId}}` にリネーム + name 変更
- `Editor/ToolTemplateCheckWindow.cs` — 削除してツール固有の実装に置き換え
- `README.md` / `TOOL_INFO.md` / `CHANGELOG.md` — プレースホルダをツール固有の内容に
- `BOOTH_PACKAGE/` 内の全ファイル — ツール名・手順をツール固有の内容に
- `CLAUDE.md` — このファイル自体をツール固有の内容に書き換え
- `AGENTS.md` — このファイル自体をツール固有の内容に書き換え

### VRC SDK / NDMF 依存がある場合

`package.json` に `vpmDependencies` を追加する:
```json
"vpmDependencies": {
  "com.vrchat.avatars": ">=3.5.0",
  "nadena.dev.ndmf": ">=1.4.0"
}
```

---

<!-- TEMPLATE-PREAMBLE-END -->

## Goal

（このツールで達成したいことを簡潔に書く）

## Current State

（現在の実装状況を書く）

## Current Blocker

なし。

## Tasks

（進行中タスクを箇条書きで。節目で更新。なければ「なし」）

## Rules

- 非破壊を最優先にし、既存データや既存設定を直接書き換える前に確認手段を用意する
- Editor ファイルの namespace は `Sebanne.{{ToolId}}.Editor`、Runtime ファイルの namespace は `Sebanne.{{ToolId}}` に統一する

## ファイル構成

（主要ファイルの一覧と役割を書く）

## 次回再開用の要点

（次スレ開始時に拾う要点を 3〜5 行で書く）

## 技術知見（任意）

（この repo 固有の技術知見・罠。複数 repo で踏んだら `notes/technical/` へ昇格。KB §9 参照）

## 次フェーズ候補

knowledge-base `next-phase/tool-dev.md`（後回しの正本。当該ツール節）を参照。旧 Notion 次フェーズ候補 DB は凍結。
