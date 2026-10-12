# Phase 1: バトル計算の核＋最小の画面

**目的:** ペンモンのバトルの中心（ダメージ・相性・ターン順・交代・勝敗）を純C#で作り、仮の画面で CPU と 3vs3 ができるようにする。

**仕様書:** §3.2 能力値 / §3.3〜3.4 タイプ / §4.1〜4.3 ターン・技・ダメージ

## 作るもの

### ロジック（純C#、`noEngineReferences: true`）

`Scripts/Battle/PenguinMonster.Battle.asmdef`（名前空間 `PenguinMonster.Battle`）

- [x] `PokeType`（仮名。10タイプの enum）と `TypeChart`（相性表。§3.4 の表をそのままコードに）
- [x] `MonsterDefinition`（No・名前・タイプ1〜2・種族値6つ・技4つ）と `MoveDefinition`（名前・タイプ・物理/特殊・威力・命中・優先度）
- [x] 仮の定義表: ペンモン 6体・技 12個程度（数値は仮でよい。名前もあとで変える）
- [x] `Stats.FromBase`（Lv50 の実数値。HP = 種族値+75、ほか = 種族値+20）
- [x] `DamageCalculator`（§4.3 の式。急所・乱数・タイプ一致・相性。乱数は `System.Random` を受け取る）
- [x] `BattleState`（両陣営・選出3体・場のペンモン。**場の数**を持たせ、シングルでは1）
- [x] `BattleEngine.ResolveTurn(行動A, 行動B)`: 交代 → 優先度 → すばやさ（同速ランダム）→ 命中判定 → ダメージ → ひんし
- [x] ひんし後の入れ替え、勝敗判定
- [x] 結果を `BattleEvent` の列で返す（「〇〇の △△！」「こうかは ばつぐんだ！」など。表示は View 側）
- [x] CPU（よわい: 技をランダム、ひんし時は控えの先頭を出す）

### テスト（EditMode、`Tests/Editor/`）

- [x] 実数値の計算
- [x] タイプ相性（等倍・2倍・½・0・2タイプの掛け算）
- [x] ダメージ（乱数を固定して期待値と一致）
- [x] 行動順（優先度 > すばやさ、交代が最優先）
- [x] 3体ひんしで勝敗が決まる

### 画面（最小）

- [x] SceneBuilder（`Editor/PenguinMonsterSceneBuilder.cs`、`Tools > PenguinMonster > Rebuild`）でバトルシーンを生成
- [x] 仮の絵: 色付きの四角＋名前。HP は数字か簡単なバー
- [x] 技4つ＋「交代」ボタン、メッセージは1行ずつ表示
- [x] 先頭3体は仮に固定（定義表の先頭3体 vs 次の3体）

## 完了条件

- エディタで Rebuild → Play すると、CPU と 3vs3 が最後まで遊べる
- タイプ相性・急所・交代がメッセージで確認できる
- EditMode テストがすべて通る
- 仕様書 §10.2 フォルダを、実際に作った構成で埋める
