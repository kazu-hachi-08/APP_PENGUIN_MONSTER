# Webビルドを GitHub Pages（penguin-monster-web リポジトリ）に公開する。
#
# 使い方（リポジトリのルートで実行）:
#   .\Tools\Release\release-web.ps1
#   .\Tools\Release\release-web.ps1 -Message "v0.2 を公開"
#
# 前提:
#   - UnityでWeb向けに -BuildDir（既定は <repo>\..\APP_PENGUIN_MONSTER_WEB）へビルド済みであること
#   - gh CLI がインストール済みで `gh auth login` 済みであること
#
# 初回は公開用リポジトリの作成・clone・GitHub Pages の有効化までまとめて行う。
# ゲーム本体のリポジトリに数十MBのビルドを積まないよう、公開物は別リポジトリに置く。

param(
    [string]$BuildDir,
    [string]$PagesDir,
    [string]$Repo = "kazu-hachi-08/penguin-monster-web",
    [string]$Message = "WebGL版を更新 " + (Get-Date -Format "yyyy-MM-dd HH:mm")
)

$ErrorActionPreference = "Stop"

# 存在確認だけしたい gh 呼び出し用。PowerShell 5.1 は native の stderr をリダイレクトすると
# エラー扱いにして Stop で止まってしまうので、この中だけ Continue にする
function Test-Quietly([scriptblock]$Command) {
    $ErrorActionPreference = "Continue"
    & $Command *> $null
    return ($LASTEXITCODE -eq 0)
}

# スクリプトの置き場所から辿ることで、どのフォルダで実行しても同じ結果にする
$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "../..")
$parentDir = Split-Path $repoRoot
if ([string]::IsNullOrEmpty($BuildDir)) { $BuildDir = Join-Path $parentDir "APP_PENGUIN_MONSTER_WEB" }
if ([string]::IsNullOrEmpty($PagesDir)) { $PagesDir = Join-Path $parentDir ($Repo.Split("/")[1]) }
$pagesUrl = "https://" + $Repo.Split("/")[0] + ".github.io/" + $Repo.Split("/")[1] + "/"

if (-not (Test-Path (Join-Path $BuildDir "index.html"))) {
    Write-Error "$BuildDir に index.html がありません。先にUnityで Web ビルドを出力してください。"
}
if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
    Write-Error "gh CLI が見つかりません。インストールして gh auth login してください。"
}

# 初回だけ公開用リポジトリを作って clone する
if (-not (Test-Path (Join-Path $PagesDir ".git"))) {
    if (-not (Test-Quietly { gh repo view $Repo })) {
        Write-Host "公開用リポジトリを作成: $Repo"
        gh repo create $Repo --public --description "ペンモン ブラウザ版"
        if ($LASTEXITCODE -ne 0) { Write-Error "リポジトリの作成に失敗しました。" }
    }
    git clone "https://github.com/$Repo.git" $PagesDir
    if ($LASTEXITCODE -ne 0) { Write-Error "clone に失敗しました。" }
}

# 前回のビルドのファイル名が残ると古いデータを読みに行くことがあるので、.git 以外は入れ替える
Get-ChildItem -Path $PagesDir -Force | Where-Object { $_.Name -ne ".git" } | Remove-Item -Recurse -Force
Copy-Item -Path (Join-Path $BuildDir "*") -Destination $PagesDir -Recurse -Force
# GitHub Pages の Jekyll は _ で始まるファイルを無視するため、そのまま配信させる
New-Item -ItemType File -Path (Join-Path $PagesDir ".nojekyll") -Force | Out-Null

git -C $PagesDir add -A
git -C $PagesDir commit -m $Message
if ($LASTEXITCODE -ne 0) { Write-Error "コミットできませんでした（前回と同じビルドの可能性があります）。" }
git -C $PagesDir push -u origin HEAD:main
if ($LASTEXITCODE -ne 0) { Write-Error "push に失敗しました。" }

# Pages が未設定なら main のルートで公開する
if (-not (Test-Quietly { gh api "repos/$Repo/pages" })) {
    Write-Host "GitHub Pages を有効化"
    gh api -X POST "repos/$Repo/pages" -f "source[branch]=main" -f "source[path]=/" | Out-Null
}

Write-Host "公開完了（反映まで1〜2分かかる）: $pagesUrl"
