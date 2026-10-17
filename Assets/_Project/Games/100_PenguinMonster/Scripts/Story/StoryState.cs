using System.Collections.Generic;

namespace PenguinMonster.Story
{
    /// <summary>
    /// ストーリー1回分の持ち物。フラグ・手持ち・あずかり箱・バッグ・お金・図鑑・羽章・最後に回復した場所。
    /// 記録（セーブ）への書き出し・読み込みは StorySave
    /// </summary>
    public sealed class StoryState
    {
        public StoryFlags Flags { get; } = new StoryFlags();
        public Party Party { get; } = new Party();
        public MonsterBox Box { get; } = new MonsterBox();
        public Bag Bag { get; } = new Bag();
        public int Money { get; internal set; } = StoryBalance.StartMoney;
        /// <summary>図鑑の「見た」「捕まえた」の No。中身を入れるのはフェーズ8（今はセーブの欄だけ用意する）</summary>
        public HashSet<int> SeenNos { get; } = new HashSet<int>();
        public HashSet<int> CaughtNos { get; } = new HashSet<int>();
        /// <summary>もらった羽章の名前。中身を入れるのはフェーズ8c（今はセーブの欄だけ用意する）</summary>
        public HashSet<string> Badges { get; } = new HashSet<string>();
        /// <summary>全滅したときに戻る場所。回復したところで書き換える</summary>
        public FieldSpot LastHealSpot { get; set; }

        /// <param name="home">最初の戻り先（自分の家）</param>
        public StoryState(FieldSpot home)
        {
            LastHealSpot = home;
        }

        public void EarnMoney(int amount)
        {
            Money += amount;
        }

        /// <summary>払う。足りなければ払わずに false</summary>
        public bool SpendMoney(int amount)
        {
            if (Money < amount)
            {
                return false;
            }
            Money -= amount;
            return true;
        }

        /// <summary>全滅した。お金を半分にし、手持ちを全回復して、戻る場所を返す（本家と同じく戻った時点で元気になっている）</summary>
        public FieldSpot BlackOut()
        {
            Money /= StoryBalance.BlackOutMoneyDivisor;
            Party.HealAll();
            return LastHealSpot;
        }

        /// <summary>もらった・捕まえた子を手持ちに入れる。いっぱいなら あずかり箱へ送り、false を返す（「箱へ 送られた」を出すため）</summary>
        public bool Receive(OwnedMonster monster)
        {
            if (Party.Add(monster))
            {
                return true;
            }
            Box.Add(monster);
            return false;
        }

        /// <summary>フィールドで手持ちの子に道具を使う。効かなければ使わずに false</summary>
        public bool UseItem(StoryItem item, OwnedMonster target)
        {
            var definition = StoryItems.Get(item);
            if (!definition.IsMedicine || !target.CanBeTreatedWith(definition) || !Bag.Remove(item))
            {
                return false;
            }
            target.Treat(definition);
            return true;
        }

        /// <summary>手持ちの子を あずかり箱へ預ける。戦える子が居なくなるなら預けずに false</summary>
        public bool Deposit(int partyIndex)
        {
            if (!Party.CanFightWithout(partyIndex))
            {
                return false;
            }
            Box.Add(Party.RemoveAt(partyIndex));
            return true;
        }

        /// <summary>あずかり箱の子を手持ちへ。いっぱいなら false</summary>
        public bool Withdraw(int boxIndex)
        {
            if (Party.IsFull)
            {
                return false;
            }
            Party.Add(Box.RemoveAt(boxIndex));
            return true;
        }

        /// <summary>手持ちの子と あずかり箱の子を入れ替える。入れ替えた後に戦える子が居なくなるなら false</summary>
        public bool SwapWithBox(int partyIndex, int boxIndex)
        {
            if (Box.Members[boxIndex].IsFainted && !Party.CanFightWithout(partyIndex))
            {
                return false;
            }
            var fromBox = Box.Members[boxIndex];
            Box.Replace(boxIndex, Party.Replace(partyIndex, fromBox));
            return true;
        }
    }
}
