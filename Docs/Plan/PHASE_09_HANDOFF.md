# Phase 9 引継ぎメモ（2026-10-12 時点）

> 残りは Unity での確認だけ。確認が済んだら PHASE_09.md の完了と IMPLEMENTATION_PLAN.md の状態を「完了」にして、このメモは消してよい。

## 状態

- ブランチ `feature/#1`。**まだコミットしていない**（作業ツリーに変更が残っている）
- Unity ではまだ一度も開いていない（コンパイル・Rebuild・プレイの確認はしていない）
- 純C#のロジックとテストは dotnet で確認済み。163 件すべて成功
- ドット絵は、スタブの UnityEngine を当てたコンソールアプリで PNG に書き出して 50 体とも目で確認済み

## 終わったこと

- ランク・50 体・新しい技 / 特性 / 持ち物・ドラフトの重み・ボスの切り札・バランス検証ツール（前回）
- 図鑑の一覧を 8 列 x 7 行（マス 100）にして 50 体が枠に収まるようにした（`PenguinMonsterSceneBuilder.Dex.cs`）
- 目を 3 種類追加: きらきら・星・糸（`EyeStyle` / `PenguinBrush`）
- 伝説は黒い輪郭の外に金の縁取り（`PenguinDesigns.Paint`）
- No.25〜50 のドット絵 26 体。ふつうはタイプ別のファイル、ヒナ・エース・伝説はランク別のファイル（`PenguinDesigns.Hina.cs` / `.Ace.cs` / `.Legend.cs`）
- コメントに残っていたひらがなの仮名を漢字にそろえた

## Unity での確認（人がやる）

- コンパイルが通るか。新しいファイルの .meta は Unity が作る
- Test Runner で EditMode のテストを流す
- Tools > PenguinMonster > Rebuild でシーンを作り直し、図鑑で 50 体が枠に収まっているか見る
- Tools > PenguinMonster > Balance Report で勝率の表が出るか
- 遊んで確かめる: 羽リレーの交代、ボス戦で切り札が3番目に出るか、オンラインで羽リレーが同期するか
- 新しい絵が実機の大きさで見分けられるか（特にヒナは小さく描いている）

## 作業メモ

- Unity なしでテストを回す方法: 作業用フォルダに次の csproj を置いて `dotnet test` を実行する（NUnit はローカルの NuGet キャッシュにある）

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>disable</Nullable>
    <ImplicitUsings>disable</ImplicitUsings>
    <LangVersion>9.0</LangVersion>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include="C:/MyApp/.temp/APP_PENGUIN_MONSTER/Assets/_Project/Games/100_PenguinMonster/Scripts/Battle/**/*.cs" />
    <Compile Include="C:/MyApp/.temp/APP_PENGUIN_MONSTER/Assets/_Project/Games/100_PenguinMonster/Tests/Editor/**/*.cs" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="*" />
    <PackageReference Include="NUnit" Version="3.14.0" />
    <PackageReference Include="NUnit3TestAdapter" Version="4.6.0" />
  </ItemGroup>
</Project>
```

- 行末コードについて: 作業ツリーは LF と CRLF が混在している。Windows の Python で読み書きするときは `newline=''` を付けて元の行末を保つこと
