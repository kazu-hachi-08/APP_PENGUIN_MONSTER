# ペンモン

ペンギンだけのモンスターをドラフトで集めて戦う、ターン制の対戦ゲーム。見た目と手触りはダイヤモンド・パール風の2D。

* **勝ち抜き戦（1人）**: 最初にドラフトで6体を選び、CPU と連戦する。勝つほど相手が強くなり、7戦ごとにボスが出る
* **オンライン対戦（2人）**: 参加コードで友達と1対1。毎回ドラフトで6体を選ぶ
* 3vs3 のシングルと、4vs4 で2体ずつ場に出るダブルスを選べる。ペンモンは150体

遊び方はゲーム内の「あそびかた」、細かいルールは仕様書を参照。

## ▶ ブラウザで遊ぶ

**https://kazu-hachi-08.github.io/penguin-monster-web/**

* PCのブラウザ（Chrome / Edge 推奨）で開く。横画面のゲームなので、スマホは横向きで

## ▶ Windows版をダウンロード

**https://github.com/kazu-hachi-08/APP_PENGUIN_MONSTER/releases/latest**

* zipを展開して exe を起動する
* 初回に「WindowsによってPCが保護されました」と出たら「詳細情報 → 実行」で起動できる
* 中身は `Tools/Release/release-windows-test.bat` で公開した最新のテスト版（`test-日時` タグ）
* 公開手順: `Tools/Release/release-windows.ps1` の先頭コメントを参照

---

## ドキュメント

| ドキュメント | 内容 |
| ---- | ---- |
| [Docs/PENGUIN_MONSTER_SPEC.md](Docs/PENGUIN_MONSTER_SPEC.md) | 仕様書（ルール・ペンモン・バトル・画面・オンライン・コード構成） |
| [Docs/DEVELOPMENT_SETUP.md](Docs/DEVELOPMENT_SETUP.md) | 開発環境のセットアップ・テスト |
| [CLAUDE.md](CLAUDE.md) | AI 開発ガイドライン |

---

## 技術スタック

| 項目 | 内容 |
| ---- | ---- |
| ゲームエンジン | Unity 6000.6.2f1（URP 2D） |
| 言語 | C# |
| グラフィック | 2Dドット絵（コードで生成） |
| オンライン | Unity Relay + Netcode for GameObjects |
| 対象プラットフォーム | Android / Windows / ブラウザ（WebGL）。横画面固定 |
| ソース管理 | GitHub |
| 開発人数 | 2人 |

## ディレクトリ構成

```text
Assets/_Project/
├── Common/                     # 共通基盤（Scene / Input / UI / Audio / Online / Profile など）
└── Games/100_PenguinMonster/   # ペンモン本体（Scripts / Editor / Data / Sprites / Audio / Scenes / Tests）
Docs/                           # 仕様書・開発手順
Tools/Release/                  # Windows版・ブラウザ版のリリーススクリプト
```

* フォルダ名 `Games/100_PenguinMonster` は変えない（Editor スクリプトに直書きパスがあるため）
* バトルのロジック（`Scripts/Battle/`）は Unity に依存しない純C#（`PenguinMonster.Battle.asmdef`）
* シーン・ペンモンのデータはコードから生成する（`Tools > PenguinMonster > Rebuild`）

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

* `PenguinMonsterScene.unity` は生成物なので手で編集しない。変えたいときは SceneBuilder を直して Rebuild する
* ペンモンの数値は定義表で調整する（アセットを Inspector で直しても Rebuild で戻る）
* アセットを追加したら `.meta` も必ず一緒にコミットする
