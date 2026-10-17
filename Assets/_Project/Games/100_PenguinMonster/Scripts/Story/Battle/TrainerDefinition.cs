using System.Collections.Generic;
using PenguinMonster.Battle;

namespace PenguinMonster.Story
{
    /// <summary>トレーナーの手持ちの1体。技は覚える技の表から自動で決める</summary>
    public readonly struct TrainerMon
    {
        public readonly StorySpecies Species;
        public readonly int Level;

        public TrainerMon(StorySpecies species, int level)
        {
            Species = species;
            Level = level;
        }
    }

    /// <summary>
    /// トレーナー1人。見た目・立つ場所・セリフは人（FieldNpc）の側に持ち、ここはバトルの中身だけ。
    /// 勝つ（負けてもよい戦いなら、負けても）と DefeatedFlag が立ち、二度と戦わない
    /// </summary>
    public sealed class TrainerDefinition
    {
        private const string FlagPrefix = "trainer.";

        public string Id { get; }
        /// <summary>「〇〇が 勝負を しかけてきた！」の〇〇</summary>
        public string Name { get; }
        public IReadOnlyList<TrainerMon> Party { get; }
        /// <summary>勝ったときにもらえるお金</summary>
        public int Prize { get; }
        /// <summary>向いている先の何マスまで見えるか。0 なら目が合っても来ない（話しかけたときだけ戦う）</summary>
        public int SightRange { get; }
        public CpuLevel Cpu { get; }

        public string DefeatedFlag => DefeatedFlagOf(Id);

        /// <summary>トレーナーを作らずにフラグだけ知りたいとき（相手によって手持ちの変わるライバルなど）</summary>
        public static string DefeatedFlagOf(string id) => FlagPrefix + id;

        public TrainerDefinition(string id, string name, TrainerMon[] party, int prize, int sightRange,
            CpuLevel cpu = CpuLevel.Weak)
        {
            Id = id;
            Name = name;
            Party = party;
            Prize = prize;
            SightRange = sightRange;
            Cpu = cpu;
        }

        /// <summary>戦うたびに新しく作る（トレーナーの子は毎回満タン）</summary>
        public List<OwnedMonster> CreateParty()
        {
            var party = new List<OwnedMonster>();
            foreach (var mon in Party)
            {
                party.Add(OwnedMonster.Create(mon.Species, mon.Level));
            }
            return party;
        }
    }
}
