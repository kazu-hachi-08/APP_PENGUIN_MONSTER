# ペンモン

（ゲーム概要は仕様書が固まったら記載する）

---

## ドキュメント

| ドキュメント | 内容 |
| ---- | ---- |
| [Docs/PENGUIN_MONSTER_SPEC.md](Docs/PENGUIN_MONSTER_SPEC.md) | 仕様書（ルール・キャラ・画面・コード構成） |
| [Docs/IMPLEMENTATION_PLAN.md](Docs/IMPLEMENTATION_PLAN.md) | 実装計画書（フェーズごとの作業と完了条件） |
| [Docs/DEVELOPMENT_SETUP.md](Docs/DEVELOPMENT_SETUP.md) | 開発環境のセットアップ・テスト |
| [CLAUDE.md](CLAUDE.md) | AI 開発ガイドライン |

---

## 技術スタック

| 項目 | 内容 |
| ---- | ---- |
| ゲームエンジン | Unity 6000.6.2f1（URP 2D） |
| 言語 | C# |
| グラフィック | 2Dドット絵 |
| オンライン | Unity Relay + Netcode for GameObjects |
| 対象プラットフォーム | Android / Windows / ブラウザ（WebGL）。横画面固定 |
| ソース管理 | GitHub |
| 開発人数 | 2人 |

## ディレクトリ構成

```text
Assets/_Project/
├── Common/                     # 共通基盤（Scene / Input / UI / Audio / Online / Profile など）
└── Games/100_PenguinMonster/   # ペンモン本体（Scripts / Editor / Data / Sprites / Audio / Scenes / Tests）
Docs/                           # 仕様書・実装計画書・開発手順
Tools/Release/                  # Windows版のリリーススクリプト
```

* フォルダ名 `Games/100_PenguinMonster` は変えない
* ゲームのルール・計算は Unity に依存しない純C#に置く
* シーン・データはコードから生成する

詳しくは [Docs/DEVELOPMENT_SETUP.md](Docs/DEVELOPMENT_SETUP.md) を参照。

---

## 開発ルール

### 基本方針

1. 小さく作る
2. 早く遊べる状態にする
3. 毎週末 Playable Build を残す
4. 必要以上に仕様を増やさない

**Playable Build** = 完成版ではなく、その時点の機能で実際に操作・プレイできる状態。グラフィックや演出が未完成でもよい。

実装の優先順位：

```text
必須機能 → ゲームとして成立する機能 → 操作性 → 演出 → 追加要素
```

### Git運用

```text
main
├── feature/#xx   # Issue番号でブランチを切る
└── fix/xxx
```

featureブランチで実装 → 動作確認 → Pull Request → レビュー → main へ Merge

### Unity開発時の注意

* ゲームのシーンは生成物なので手で編集しない。変えたいときは SceneBuilder を直して Rebuild する
* キャラの数値は定義表で調整する（アセットを Inspector で直しても Rebuild で戻る）
* アセットを追加したら `.meta` も必ず一緒にコミットする
