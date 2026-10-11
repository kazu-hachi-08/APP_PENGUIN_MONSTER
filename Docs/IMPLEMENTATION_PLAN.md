# ペンモン 実装計画書

> 対話しながらフェーズを決めていく。各フェーズは「週末1〜2回で終わる大きさ」「終わったら Playable Build が残る」を目安に切る。

## 進め方

- 1フェーズ = 1つ以上の feature ブランチ → PR → main へ Merge
- フェーズの完了条件を満たしたら Playable Build を残す
- 仕様が変わったら `Docs/PENGUIN_MONSTER_SPEC.md` も更新する

## フェーズ一覧

| フェーズ | 内容 | 状態 |
| ---- | ---- | ---- |
| Phase 0 | リポジトリ・プロジェクトの準備 | 進行中 |
| Phase 1 | 未定 | - |

---

## Phase 0: リポジトリ・プロジェクトの準備

**目的:** Unity で開けて、共通基盤が入った空のプロジェクトを用意する。

- [x] Claude 用ファイル（`CLAUDE.md` / `.claude/skills/`）を配置
- [x] Unity のプロジェクト設定・パッケージ・Build Profiles を配置（製品名「ペンモン」、ID `com.kazuhachi08.penguinmonster`）
- [x] 共通基盤 `Assets/_Project/Common/` を配置（名前空間 `PenguinMonster.Common.*`）
- [x] ゲーム本体のフォルダ `Assets/_Project/Games/100_PenguinMonster/` を作成
- [x] ドキュメント（README / 仕様書 / 実装計画書 / 開発手順）の雛形を作成
- [ ] Unity 6000.6.2f1 で開き、コンパイルエラーが無いことを確認
- [ ] Unity Cloud のプロジェクトにリンク（オンラインを使う場合）
- [ ] 初回コミット・GitHub へ push

**完了条件:** clone して Unity で開くとエラーなく開ける。

---

## Phase 1: 未定

（対話で決める）
