# 次にやること（作業メモ）

> セッションをまたいで作業を引き継ぐためのメモ。仕様書・実装計画書が固まったら削除してよい。

## これまでに終わったこと（Phase 0 の途中まで）

- Claude 用ファイル、Unity のプロジェクト設定、パッケージ、Build Profiles、共通基盤 `Assets/_Project/Common/`（名前空間 `PenguinMonster.Common.*`）を配置した
- 製品名は「ペンモン」、ID は `com.kazuhachi08.penguinmonster`。Unity Cloud のリンクとアプリアイコンは空にしてある
- ゲーム本体のフォルダ `Assets/_Project/Games/100_PenguinMonster/` を作った（中身は `.gitkeep` だけ）
- ドキュメントの雛形を作った
  - `CLAUDE.md` / `README.md` / `Docs/DEVELOPMENT_SETUP.md`
  - `Docs/PENGUIN_MONSTER_SPEC.md`（仕様書。中身はすべて「未定」）
  - `Docs/IMPLEMENTATION_PLAN.md`（実装計画書。Phase 0 だけ書いてある）
- まだコミットしていない（`main` は初回コミット前）

## 決まっていること

- Unity 6000.6.2f1 / URP 2D / Android・Windows・WebGL / 横画面固定（参考にした構成と同じ）
- 共通基盤（Scene / Input / UI / Audio / Online / Profile）は使う
- ドキュメントに、ほかのゲームから移植したという記載は入れない
- 仕様書と実装計画書は、対話しながら作る

## 次にやること

1. **Phase 1 の動作確認**（ユーザー側）
   - Unity で開き `Tools > PenguinMonster > Rebuild` → `Scenes/PenguinMonsterScene` を Play
   - CPU と 3vs3 が最後まで遊べるか、相性・急所・交代のメッセージが出るかを見る
   - EditMode テスト（27件）はバッチモードで全て通過済み
2. **コミットと push**（`feature/#1` → PR → main）
   - Unity が生成した `.meta`・シーン・`ProjectSettings/EditorBuildSettings.asset` も含める
3. Unity Cloud のリンク（Phase 6 までに）
4. 次は Phase 2（ダイパ風のバトル画面）
