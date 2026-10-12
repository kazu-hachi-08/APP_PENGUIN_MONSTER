# 開発環境・プロジェクトセットアップ手順書

2人開発で Unity 環境をそろえるための手順をまとめる。

---

## 1. Unityバージョン

- **バージョン**: `Unity 6000.6.2f1`（`ProjectSettings/ProjectVersion.txt` と同じものを使う）
- レンダーパイプライン: URP（2D）

バージョンが違うと `ProjectSettings/` や `Packages/packages-lock.json` に差分が出るので、Unity Hub で同じバージョンを入れる。

---

## 2. 必要なUnityモジュール

Unity Hub の「Installs」タブから、対象バージョンに以下のモジュールを追加する。

- **Android Build Support**（Android SDK & NDK Tools / OpenJDK）
- **Web Build Support**（ブラウザ版）
- **Windows Build Support (IL2CPP)**（Windows版を IL2CPP で出す場合。Mono なら不要）

※ iOS (iPhone) 向けビルドは現時点では対象外。

---

## 3. 初回のセットアップ

1. リポジトリを clone し、Unity Hub の「Add」から開く（初回は `Library/` の生成に時間がかかる）
2. オンライン機能を試す場合は、Project Settings > Services で Unity Cloud のプロジェクトにリンクする（Relay と匿名サインインを使う）

---

## 4. ディレクトリ構成

```text
Assets/
├── _Project/
│   ├── Common/                     # 共通基盤（PenguinMonster.Common.* 名前空間）
│   │   ├── Scripts/                # Core / Audio / Input / Online / Profile / Scene / UI
│   │   ├── Editor/                 # 共通ダイアログのシーン生成・デバッグメニュー
│   │   ├── Plugins/WebGL/          # ブラウザ版の名前入力・全画面（.jslib）
│   │   └── Resources/              # フォント等
│   │
│   └── Games/
│       └── 100_PenguinMonster/     # ペンモン本体
│           ├── Scripts/            # ゲームのコード
│           ├── Editor/             # SceneBuilder・データの生成
│           ├── Data/ Sprites/ Audio/ Scenes/
│           └── Tests/Editor/       # EditMode テスト
│
├── Settings/Build Profiles/        # Android / Web / Windows
└── WebGLTemplates/PenguinMonster/  # ブラウザ版の index.html
Docs/                               # 仕様書・開発手順
Tools/Release/                      # Windows版・ブラウザ版のリリーススクリプト
```

フォルダ名 `Games/100_PenguinMonster` は変えない（Editor スクリプトに直書きパスを持たせるため、動かすときはまとめて直す）。

---

## 5. テスト

Window > General > Test Runner の EditMode でテストを実行する。純C#のロジックを変えたら通しておく。

---

## 6. Git・ファイル管理ルール

- **`.gitignore`**: プロジェクトルートに配置済み。`Library/`, `Temp/`, `Logs/`, `Builds/` などは除外される。
- **`.meta` ファイル**: アセットを追加・変更したときに生成される `.meta` は**必ずセットでコミット**する（GUID が変わるとシーン・アセットの参照が切れる）。
- **コンフリクト防止**: ゲームのシーンは生成物なので、2人で同時に Rebuild した場合はどちらかに寄せてから作り直す。
