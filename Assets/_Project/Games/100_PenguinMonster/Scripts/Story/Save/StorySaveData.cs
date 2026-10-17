using System;
using System.Collections.Generic;
using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>
    /// ストーリーの記録（PLAN_STORY_TOWN01.md フェーズ7）。View が JSON にして保存する形なので、public の項目だけで持つ。
    /// 項目を足したら Version を上げ、初期値に「古い記録に無かったときの値」を書いておく（JSON に無い項目は初期値のまま残るため）。
    /// 道具・技は名前で持つ（並びの番号だと、後の区切りで足したときに古い記録がずれるため）
    /// </summary>
    [Serializable]
    public sealed class StorySaveData
    {
        /// <summary>今の版。読めるのはこれ以下の版だけ（新しい版の記録は中身が分からないので読まない）</summary>
        public const int CurrentVersion = 1;

        public int Version = CurrentVersion;
        public SpotSave Position = new SpotSave();
        public SpotSave LastHealSpot = new SpotSave();
        public List<MonsterSave> Party = new List<MonsterSave>();
        public List<MonsterSave> Box = new List<MonsterSave>();
        public List<ItemCountSave> Bag = new List<ItemCountSave>();
        public int Money = StoryBalance.StartMoney;
        public List<string> Flags = new List<string>();
        public List<int> SeenNos = new List<int>();
        public List<int> CaughtNos = new List<int>();
        public List<string> Badges = new List<string>();
    }

    [Serializable]
    public sealed class SpotSave
    {
        /// <summary>空なら記録に無い（呼ぶ側の決めた場所を使う）</summary>
        public string MapId = "";
        public int X;
        public int Y;
        public Direction Facing;
    }

    [Serializable]
    public sealed class MonsterSave
    {
        public int No;
        public int Level;
        public int Exp;
        public List<string> Moves = new List<string>();
        public int Hp;
        public StatusCondition Status;
        public HeldItem Item;
    }

    [Serializable]
    public sealed class ItemCountSave
    {
        public string Item = "";
        public int Count;
    }
}
