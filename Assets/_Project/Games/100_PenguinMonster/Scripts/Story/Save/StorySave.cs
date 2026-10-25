using System;
using System.Collections.Generic;
using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>記録から戻した中身。続きは Position から歩き始める</summary>
    public sealed class RestoredStory
    {
        public StoryState State { get; }
        public FieldSpot Position { get; }

        public RestoredStory(StoryState state, FieldSpot position)
        {
            State = state;
            Position = position;
        }
    }

    /// <summary>
    /// StoryState と記録（StorySaveData）の行き来。JSON・PlayerPrefs は View が受け持つ
    /// </summary>
    public static class StorySave
    {
        /// <param name="position">今立っている場所（メニューを開いた場所。続きはここから）</param>
        public static StorySaveData Capture(StoryState state, FieldSpot position)
        {
            var data = new StorySaveData
            {
                Position = ToSave(position),
                LastHealSpot = ToSave(state.LastHealSpot),
                Money = state.Money,
                Flags = new List<string>(state.Flags.All),
                SeenNos = new List<int>(state.SeenNos),
                CaughtNos = new List<int>(state.CaughtNos),
                Badges = new List<string>(state.Badges),
            };
            foreach (var member in state.Party.Members)
            {
                data.Party.Add(ToSave(member));
            }
            foreach (var member in state.Box.Members)
            {
                data.Box.Add(ToSave(member));
            }
            foreach (var item in state.Bag.Items())
            {
                data.Bag.Add(new ItemCountSave { Item = item.ToString(), Count = state.Bag.CountOf(item) });
            }
            return data;
        }

        /// <summary>
        /// 記録から戻す。新しい版の記録・今は無い種や技やマップが入った記録（仮の子を本物に入れ替えた後など）は読めないので null。
        /// 古い版に無い項目は StorySaveData の初期値（無ければ home）で埋める
        /// </summary>
        /// <param name="home">記録に場所が無いときに使う場所（自分の家）</param>
        public static RestoredStory Restore(StorySaveData data, FieldWorld world, FieldSpot home)
        {
            if (data == null || data.Version > StorySaveData.CurrentVersion)
            {
                return null;
            }
            var state = new StoryState(ToSpot(data.LastHealSpot, world, home)) { Money = data.Money };
            if (!RestoreMonsters(data.Party, monster => state.Party.Add(monster))
                || !RestoreMonsters(data.Box, state.Box.Add)
                || !RestoreBag(data.Bag, state.Bag))
            {
                return null;
            }
            AddAll(data.Flags, state.Flags.Set);
            AddAll(data.SeenNos, no => state.SeenNos.Add(no));
            AddAll(data.CaughtNos, no => state.CaughtNos.Add(no));
            AddAll(data.Badges, badge => state.Badges.Add(badge));
            var position = ToSpot(data.Position, world, home);
            // 建物の中では乗れないので、乗ったまま記録されていても降りた状態で始める
            Bicycle.GetOffIfIndoors(state.Flags, world.Get(position.MapId));
            return new RestoredStory(state, position);
        }

        private static SpotSave ToSave(FieldSpot spot)
        {
            return new SpotSave { MapId = spot.MapId, X = spot.Position.X, Y = spot.Position.Y, Facing = spot.Facing };
        }

        /// <summary>記録に無い・今は無いマップなら home（マップの作り直しで消えた場所に立たせないように）</summary>
        private static FieldSpot ToSpot(SpotSave spot, FieldWorld world, FieldSpot home)
        {
            if (spot == null || string.IsNullOrEmpty(spot.MapId) || !world.Contains(spot.MapId))
            {
                return home;
            }
            return new FieldSpot(spot.MapId, new GridPos(spot.X, spot.Y), spot.Facing);
        }

        private static MonsterSave ToSave(OwnedMonster monster)
        {
            var moves = new List<string>();
            foreach (var move in monster.Moves)
            {
                moves.Add(move.Name);
            }
            return new MonsterSave
            {
                No = monster.No, Level = monster.Level, Exp = monster.Exp, Moves = moves,
                Hp = monster.CurrentHp, Status = monster.Status, Item = monster.Item,
            };
        }

        private static bool RestoreMonsters(List<MonsterSave> saved, Action<OwnedMonster> add)
        {
            foreach (var entry in saved ?? new List<MonsterSave>())
            {
                var monster = ToMonster(entry);
                if (monster == null)
                {
                    return false;
                }
                add(monster);
            }
            return true;
        }

        private static OwnedMonster ToMonster(MonsterSave saved)
        {
            var species = StorySpeciesTable.ByNo(saved.No);
            if (species == null)
            {
                return null;
            }
            var moves = new List<MoveDefinition>();
            foreach (var name in saved.Moves ?? new List<string>())
            {
                var move = MoveTable.ByName(name);
                if (move == null)
                {
                    return null;
                }
                moves.Add(move);
            }
            int level = Math.Max(StoryBalance.MinLevel, Math.Min(saved.Level, StoryBalance.MaxLevel));
            return OwnedMonster.Restore(species, level, saved.Exp, moves, saved.Hp, saved.Status, saved.Item);
        }

        private static bool RestoreBag(List<ItemCountSave> saved, Bag bag)
        {
            foreach (var entry in saved ?? new List<ItemCountSave>())
            {
                if (!Enum.TryParse(entry.Item, out StoryItem item))
                {
                    return false;
                }
                bag.Add(item, entry.Count);
            }
            return true;
        }

        private static void AddAll<T>(List<T> saved, Action<T> add)
        {
            foreach (var value in saved ?? new List<T>())
            {
                add(value);
            }
        }
    }
}
