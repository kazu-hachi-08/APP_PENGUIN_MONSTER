# 実装計画: 持ち物を「ドラフト後に1個もらう」方式に変える

## 0. 背景

- いまは全150体が定義表 `MonsterTable` で持ち物を1つずつ固定で持っている（仕様書 §3.1 / §4.6）
- 図鑑で見ると「ペンモンの種族が持ち物を持っている」ように見えて不自然
- → ペンモンは持ち物なしにして、**ドラフトの後にランダムで持ち物を1個もらい、6体の中の好きな1体に持たせる** 方式にする

## 1. 決めた仕様

| 項目 | 内容 |
| --- | --- |
| ペンモンの持ち物 | 全ペンモン持ち物なし。図鑑・ドラフトのカードから持ち物の行を消す |
| もらえる数 | 1チーム1個（ItemDraw.Draw を1チームにつき1回だけ呼ぶ） |
| 何がもらえるか | 持ち物の候補（後述）から等確率で1個 |
| いつ | ドラフトで6体そろった直後。新しい「持ち物」画面をはさむ |
| 持たせ方 | 画面上に持ち物のカード（名前・説明）、下に6体。押したペンモンが持つ（押したらすぐ決定。ドラフトと同じ手触り） |
| 勝ち抜き戦 | 最初の1回だけ。そのまま最後まで同じ持ち主（チームを途中で変えない §2.3 に合わせる）。「1回だけ」の持ち物は試合ごとに戻る（今と同じ） |
| オンライン | 15秒（`OnlineDraftSeconds` を流用）。過ぎたら6体からランダムな1体に持たせる |
| CPU | CPU も1個ランダムでもらい、6体からランダムな1体に持たせる |
| ボス | ボスも1個ランダムでもらい、切り札（伝説）に持たせる |
| 見せ合い | 自分のチームは持ち主に「持ち物名」を出す。**相手の持ち物は隠す**（バトルで働いたときに初めて分かる） |

### 1.1 持ち物の候補

- `HeldItem.None` と `ChickDown`（ヒナの綿毛）を除く12個
- お天気石は天候の特性がいないチームだと何も起きない「はずれ」になるが、ランダムの面白さとして残す（§5 の決めごと②）

### 1.2 ヒナの綿毛の扱い

ヒナの綿毛（防御・特防 1.5倍）は「種族値の低いヒナを戦えるようにする」ための補正で、持ち物というより種族の特徴。
持ち物から外し、**ランク「ヒナ」の全員が常に防御・特防 1.5倍**にする（図鑑の特性の欄に、ラベル「ヒナ」・名前「ヒナの綿毛」・説明「防御と 特防が 1.5倍」の行を足す。ヒナのみ）。

- いま綿毛を持つヒナは 19体中 11体。残り 8体も少し強くなるが、全体の持ち物が減る（6個 → 1個）ので許容範囲と見る
- 実装は `BattleMonster` の `HasItem(HeldItem.ChickDown)` を `Definition.Rank == MonsterRank.Hina` に変えるだけ

## 2. 設計の方針（最小の変更で済ませる）

持ち物は `MonsterDefinition.Item` に入ったまま、バトル側（`BattleMonster.HasItem` / `DamageCalculator` / `BattleEngine.Items.cs`）は **一切変えない**。

- `MonsterTable` の全150体は `Item = None` にする
- 持たせるときは `MonsterDefinition.WithItem(HeldItem item)` で **持ち物だけ違うコピー** を作って6体のリストの中身を差し替える
- これでバトル・CPU・図鑑はほぼ今のコードのまま動く

### 注意: 参照の比較

コピーを作ると「同じ No でも別のインスタンス」になる。今のコードで参照で比べているところ:

- `CpuPlayer.ChooseTeam` の `rest.Remove(trumpCard)` / `team.Add(trumpCard)` — ボスの切り札に持ち物を持たせると、`BossDefinition.TrumpCard`（表の元）と `Six` の中のコピーが別物になる
  → 実装は `SurvivalRun.NextOpponent` が、ボスの `Team` から切り札の位置を参照で探し、持ち物を配った写しの切り札を `SurvivalOpponent.TrumpCard` として渡す
- `Draft._picked.Contains` は持ち物を配る前なので影響なし

## 3. フェーズ分け

各フェーズの終わりで「とりあえず遊べる」状態を保つ。

### フェーズ1: 純C#のロジック（持ち物なし化 + 配る仕組み）【済 2026-10-14】

終わったときの状態: 誰も持ち物を持たないバトルになる（UI はまだ）。勝ち抜き戦・オンラインとも今まで通り遊べる。

1. `MonsterTable` の全150体から持ち物の引数を消す（`MonsterDefinition` の既定値 `None` になる）
2. `MonsterDefinition.WithItem(HeldItem)` を追加（他の値は同じでコピー）
3. ヒナの綿毛をランク補正に変える（`BattleMonster` の防御計算、§1.2）
4. 新規 `Scripts/Battle/Draft/ItemDraw.cs`（純C#）
   - `HeldItem Draw(System.Random random)` — 候補（§1.1）から1個
   - `static List<MonsterDefinition> Give(IReadOnlyList<MonsterDefinition> six, int holderIndex, HeldItem item)` — 指定の1体だけ `WithItem` したリストを返す
   - 候補の除外リストはバランス定義に置く（`BattleBalance` か `Balance/` 配下）
5. CPU と ボスに配る
   - `SurvivalRun` で相手の6体を作るところで `ItemDraw` を使う（CPU はランダムな1体、ボスは切り札）
   - 実装は `SurvivalRun.NextOpponent` が、ボスの `Team` から切り札の位置を参照で探し、持ち物を配った写しの切り札を `SurvivalOpponent.TrumpCard` として渡す（§2 注意）
6. テスト（`Tests/Editor/`）
   - `MonsterTableTests`: 「ヒナの綿毛はヒナだけが持つ」→「全ペンモンが持ち物を持たない」に置き換え
   - 新規 `ItemDrawTests`: 候補に None / ヒナの綿毛が出ない、`Give` で指定の1体だけ持ち物が付き No・技は同じ
   - ヒナの綿毛のテスト（`AbilityItemTests` など）を「ヒナなら 1.5倍・ヒナ以外は等倍」に直す
   - CPU・ボスの相手チームに持ち物がちょうど1個、ボスは切り札が持つ
7. 仕様書: §3.1（持ち物を定義から外す）/ §3.5（ヒナの補正）/ §4.6（持ち物は1チーム1個・候補・ヒナの綿毛を表から外す）
8. 図鑑・ドラフトのカード: `MonsterTexts.Traits` で `Item == None` のときは持ち物の行を出さない（ヒナなら綿毛の行を出す）

コスト目安: **小〜中**（表の150行の機械的な置換が一番の手間）

### フェーズ2: 「持ち物」画面（勝ち抜き戦）【済 2026-10-14】

終わったときの状態: 勝ち抜き戦でドラフト後に持ち物をもらって持たせられる。オンラインはまだ持ち物なし。

1. 新規 `Scripts/View/ItemPanel.cs`
   - `Show(IReadOnlyList<MonsterDefinition> six, HeldItem item, Action<List<MonsterDefinition>> onDone, float seconds = 0f)`
   - 上: 「持ち物を もらった！」と持ち物カード（名前・説明）。下: 6体のボタン（絵・名前）。押したらすぐ `ItemDraw.Give` して `onDone`
   - `seconds > 0` なら `CountdownTimer` を使い、時間切れでランダムな1体（フェーズ3で使う）
2. SceneBuilder: 新規 `PenguinMonsterSceneBuilder.Item.cs` でパネルを作る（ドラフトのカード・ボタンの作り方を流用）
3. `GameFlow.cs` の勝ち抜き戦: ドラフトの後に `ItemPanel` をはさみ、返ってきた6体を `SurvivalRun` に渡す
4. 見せ合い・選出（`SelectPanel`）: 自分の持ち主のセルに持ち物名を小さく出す。相手側は出さない
5. 「やめる」をこの画面にも出す（§7 のドラフト〜バトルの範囲に含める）
6. `HowToPanel` のドラフトのタブ: 「カードには … 持ち物 …」を消し、「6体 そろったら 持ち物を 1つ もらえます。好きな ペンモンに 持たせよう」を足す
7. 仕様書: §2 の流れ（ドラフト → **持ち物** → 見せ合い）/ §2.1 の後に「2.1.1 持ち物」節 / §7 の「やめる」の範囲 / §10 のクラス一覧

コスト目安: **中**（新しい画面1枚。SceneBuilder の追加がメイン）

### フェーズ3: オンライン対応【済 2026-10-14】

終わったときの状態: オンラインでもお互い持ち物を持って戦える。

1. `OnlineCodec.EncodeMonsters` / `DecodeMonsters` を「No・持ち物・技4つ」を並べた組にする（1体 `FieldsPerMonster`（= 6）個の int。持ち物は2番目）
   - `Six`（見せ合い用）と `Team`（バトル用）の両方に効く。見せ合いで相手の持ち物を隠すのは表示側だけで行う
   - これを入れないと、ホストの手元のゲストのチームに持ち物が無く、ゲストの持ち物が働かない
2. `GameFlow.Online.cs`: ドラフトの後に `ItemPanel` を 15秒（`OnlineDraftSeconds`）ではさむ。持ち物の抽選はドラフトと同じ `draftRandom` で各自の端末が引く（どちらの端末で引いても結果は `Six` で送るので、種の共有は不要）
3. 持ち物画面の間の切断も「相手が 切断しました」で勝ち扱い（StopOnline で持ち物画面も閉じる）
4. テスト: `OnlineCodecTests` を組の形に直す（持ち物つきで往復して同じになる）
5. 仕様書: §8 の流れ / §8.1 の `Six` / `Team` の中身（「No と持ち物の組」）

コスト目安: **小**

## 4. 触るファイル（まとめ）

| フェーズ | ファイル |
| --- | --- |
| 1 | `Battle/Data/MonsterTable.cs`, `Battle/Data/MonsterDefinition.cs`, `Battle/BattleMonster.cs`, `Battle/Draft/ItemDraw.cs`（新規）, `Battle/Survival/SurvivalRun.cs`, `Battle/BattleBalance.cs`, `View/GameFlow.cs`, `View/MonsterTexts.cs`, `Tests/Editor/*` |
| 2 | `View/ItemPanel.cs`（新規）, `Editor/PenguinMonsterSceneBuilder.Item.cs`（新規）, `Editor/PenguinMonsterSceneBuilder.cs`, `View/GameFlow.cs`, `View/SelectPanel.cs`, `View/HowToPanel.cs` |
| 3 | `Battle/Online/OnlineCodec.cs`, `View/GameFlow.Online.cs`, `Tests/Editor/OnlineCodecTests.cs` |
| 全部 | `Docs/PENGUIN_MONSTER_SPEC.md` |

（パスは `Assets/_Project/Games/100_PenguinMonster/Scripts/` 等からの相対）

## 5. 決めごと（2026-10-14 決定: すべて推奨の案で進める）

| # | 決めごと | 決定 | 採らなかった案 |
| --- | --- | --- | --- |
| ① | 持ち物の決め方 | **ランダム1個**（提案どおり。画面が1段で済む） | 3つ提示から1つ選ぶ（ドラフトと同じ形。「はずれ」が減るが1タップ増える） |
| ② | お天気石の「はずれ」 | 残す | 候補から外す / チームに天候の特性がいるときだけ候補に入れる |
| ③ | ヒナの綿毛 | ランク「ヒナ」全員の常時補正にする | 完全に消す（ヒナが弱くなる）/ 持ち物の候補に残す（ヒナ以外は無意味） |
| ④ | 見せ合いで相手の持ち物 | 隠す | 見せる（読み合いは減るが分かりやすい） |
| ⑤ | 勝ち抜き戦で追加の持ち物 | なし（今回のスコープ外） | ボスに勝つたびに1個もらう、など（§11 の候補に書いておく） |

## 6. 動作確認

- EditMode テストが全部通る
- 図鑑: どのペンモンにも持ち物の行が出ない。ヒナには綿毛の行が出る
- 勝ち抜き戦: ドラフト → 持ち物画面 → 押した1体が持つ → 選出画面に持ち物名 → バトルで働く（干し魚の回復文など）
- ボス戦: 切り札が持ち物を使う（ボスの選出で切り札が3番目に出ることも崩れていない）
- オンライン（2端末）: 時間切れでもランダムに持つ / ゲスト側の持ち物がホストの計算で働く
