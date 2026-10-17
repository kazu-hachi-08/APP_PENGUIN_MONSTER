using System;
using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>捕獲タマゴを投げた結果。揺れた回数（0〜3）と、捕まえたか</summary>
    public readonly struct CaptureResult
    {
        public readonly int Shakes;
        public readonly bool Caught;

        public CaptureResult(int shakes, bool caught)
        {
            Shakes = shakes;
            Caught = caught;
        }
    }

    /// <summary>
    /// 捕獲の式（本家の第3〜4世代の簡略版）。
    /// 最大HP・今の HP・種の捕まえやすさ・状態異常から「捕まえやすさの値」を出し、4回の揺れの判定を引く。
    /// 4回とも通れば捕まえる（揺れは3回見せる）。途中で外れたら、それまでに通った回数だけ揺れて出てくる
    /// </summary>
    public static class Capture
    {
        /// <summary>捕まえやすさの値。大きいほど捕まえやすく、MaxOdds 以上なら必ず捕まえる</summary>
        public static int Odds(int maxHp, int currentHp, int catchRate, StatusCondition status)
        {
            int hpFactor = StoryBalance.CaptureMaxHpWeight * maxHp - StoryBalance.CaptureHpWeight * currentHp;
            long odds = (long)hpFactor * catchRate / (StoryBalance.CaptureMaxHpWeight * maxHp);
            return (int)(odds * StatusBonusPercent(status) / StoryBalance.Percent);
        }

        public static CaptureResult Roll(int odds, Random random)
        {
            if (odds >= StoryBalance.CaptureMaxOdds)
            {
                return new CaptureResult(StoryBalance.CaptureShownShakes, true);
            }
            int threshold = ShakeThreshold(odds);
            for (int check = 0; check < StoryBalance.CaptureChecks; check++)
            {
                if (random.Next(StoryBalance.CaptureShakeRoll) >= threshold)
                {
                    return new CaptureResult(check, false);
                }
            }
            return new CaptureResult(StoryBalance.CaptureShownShakes, true);
        }

        /// <summary>1回の揺れが通る境目（本家の 1048560 ÷ √√(16711680 ÷ 値)）。4回通る確率がおおよそ 値 ÷ 255 になる</summary>
        private static int ShakeThreshold(int odds)
        {
            if (odds <= 0)
            {
                return 0;
            }
            return (int)(StoryBalance.CaptureShakeNumerator / Math.Sqrt(Math.Sqrt(StoryBalance.CaptureShakeDivisor / odds)));
        }

        /// <summary>眠り・氷は 2 倍、麻痺・毒・火傷は 1.5 倍（第3〜4世代と同じ）</summary>
        private static int StatusBonusPercent(StatusCondition status)
        {
            switch (status)
            {
                case StatusCondition.Sleep:
                case StatusCondition.Freeze:
                    return StoryBalance.CaptureSleepBonusPercent;
                case StatusCondition.None:
                    return StoryBalance.Percent;
                default:
                    return StoryBalance.CaptureStatusBonusPercent;
            }
        }
    }
}
