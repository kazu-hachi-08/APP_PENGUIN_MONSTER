using System.IO;
using System.Linq;
using PenguinMonster.View.Art;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace PenguinMonster.Editor
{
    /// <summary>
    /// アプリアイコンをペンモンのドット絵から生成し、Windows / Android / WebGL に設定する。
    /// PNG は生成物なので手で編集せず、ここを直して Tools > PenguinMonster > App Icon を実行し直す
    /// </summary>
    public static class AppIconBuilder
    {
        private const string IconFolder = "Assets/_Project/Games/100_PenguinMonster/Sprites/AppIcon";
        private const string IconPath = IconFolder + "/AppIcon.png";
        private const string BackgroundPath = IconFolder + "/AppIcon_Background.png";
        private const string ForegroundPath = IconFolder + "/AppIcon_Foreground.png";
        // カスタムテンプレートのフォルダにあるファイルはビルドにそのままコピーされる
        private const string FaviconPath = "Assets/WebGLTemplates/PenguinMonster/favicon.png";

        // 主役の No.1 ユキペンを顔にする
        private const int IconMonsterNo = 1;
        private const int IconSize = PenguinDesigns.CanvasSize;
        // Android のアダプティブアイコンは 108dp 角のうち中央 66dp しか必ずは見えないので、
        // 64 ドットの絵を 108 ドットの中央に置いて丸や角丸に切られても欠けないようにする
        private const int AdaptiveSize = 108;
        // 64x16 = 1024（ストア・Windows の最大）、108x4 = 432（アダプティブの最大）にそろえる
        private const int IconScale = 16;
        private const int AdaptiveScale = 4;
        private const int FaviconScale = 2;
        // 絵の足元が端に付かないよう、少し持ち上げる
        private const int PenguinLift = 2;

        private static readonly Color32 SkyTop = new Color32(96, 172, 232, 255);
        private static readonly Color32 SkyBottom = new Color32(200, 236, 255, 255);
        private static readonly Color32 Snow = new Color32(244, 250, 255, 255);
        private static readonly Color32 SnowShade = new Color32(200, 226, 242, 255);
        private const int SkyBands = 8;

        [MenuItem("Tools/PenguinMonster/App Icon")]
        public static void Build()
        {
            Directory.CreateDirectory(IconFolder);

            var icon = PaintBackground(IconSize);
            Stamp(icon, 0, PenguinLift);
            var iconTexture = Save(icon, IconScale, IconPath);
            var background = Save(PaintBackground(AdaptiveSize), AdaptiveScale, BackgroundPath);
            var foreground = new PixelCanvas(AdaptiveSize, AdaptiveSize);
            Stamp(foreground, (AdaptiveSize - IconSize) / 2, (AdaptiveSize - IconSize) / 2);
            var foregroundTexture = Save(foreground, AdaptiveScale, ForegroundPath);
            Save(icon, FaviconScale, FaviconPath);

            ApplyDefault(iconTexture);
            ApplyStandalone(iconTexture);
            ApplyAndroid(iconTexture, background, foregroundTexture);
            AssetDatabase.SaveAssets();
            Debug.Log($"[AppIconBuilder] アイコンを {IconFolder} と {FaviconPath} に生成し、Player Settings に設定しました");
        }

        /// <summary>上から下へ明るくなる空と、手前の雪原。ゲームの背景と同じく色を段に分けてドット絵らしくする</summary>
        private static PixelCanvas PaintBackground(int size)
        {
            var canvas = new PixelCanvas(size, size);
            int bandHeight = Mathf.CeilToInt(size / (float)SkyBands);
            for (int band = 0; band < SkyBands; band++)
            {
                var color = Color32.Lerp(SkyBottom, SkyTop, band / (float)(SkyBands - 1));
                canvas.FillRect(0, band * bandHeight, size, bandHeight, color);
            }
            float center = size / 2f;
            canvas.FillEllipse(center, 0f, size * 0.75f, size * 0.25f, SnowShade);
            canvas.FillEllipse(center - 2f, 0f, size * 0.7f, size * 0.22f, Snow);
            return canvas;
        }

        /// <summary>ペンモンの正面の絵を、透明なドット以外だけ上に重ねる</summary>
        private static void Stamp(PixelCanvas target, int offsetX, int offsetY)
        {
            var penguin = PenguinDesigns.Paint(IconMonsterNo, true);
            for (int y = 0; y < penguin.Height; y++)
            {
                for (int x = 0; x < penguin.Width; x++)
                {
                    var color = penguin.Pixels[y * penguin.Width + x];
                    if (color.a > 0) target.Set(x + offsetX, y + offsetY, color);
                }
            }
        }

        /// <summary>ドットのまま拡大して PNG に書く。Unity が縮小するときにぼやけにくいよう、元の解像度は大きくしておく</summary>
        private static Texture2D Save(PixelCanvas canvas, int scale, string path)
        {
            int width = canvas.Width * scale;
            int height = canvas.Height * scale;
            var pixels = new Color32[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    pixels[y * width + x] = canvas.Pixels[(y / scale) * canvas.Width + x / scale];
                }
            }
            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.SetPixels32(pixels);
            File.WriteAllBytes(path, texture.EncodeToPNG());
            Object.DestroyImmediate(texture);

            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            var importer = (TextureImporter)AssetImporter.GetAtPath(path);
            importer.textureType = TextureImporterType.Default;
            importer.mipmapEnabled = false;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
            return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }

        /// <summary>WebGL など個別の設定がないプラットフォームは Default Icon を使う</summary>
        private static void ApplyDefault(Texture2D icon)
            => PlayerSettings.SetIcons(NamedBuildTarget.Unknown, new[] { icon }, IconKind.Any);

        private static void ApplyStandalone(Texture2D icon)
        {
            int count = PlayerSettings.GetIconSizes(NamedBuildTarget.Standalone, IconKind.Any).Length;
            PlayerSettings.SetIcons(NamedBuildTarget.Standalone, Enumerable.Repeat(icon, count).ToArray(), IconKind.Any);
        }

        /// <summary>
        /// 種類は名前で探す（AndroidPlatformIconKind を直接使うと Android モジュールが無い環境でコンパイルできないため）。
        /// アダプティブは 0 番が背景・1 番が前景
        /// </summary>
        private static void ApplyAndroid(Texture2D icon, Texture2D background, Texture2D foreground)
        {
            var target = NamedBuildTarget.Android;
            foreach (var kind in PlayerSettings.GetSupportedIconKinds(target))
            {
                var icons = PlayerSettings.GetPlatformIcons(target, kind);
                foreach (var platformIcon in icons)
                {
                    if (kind.ToString() == "Adaptive") platformIcon.SetTextures(background, foreground);
                    else platformIcon.SetTexture(icon);
                }
                PlayerSettings.SetPlatformIcons(target, kind, icons);
            }
        }
    }
}
