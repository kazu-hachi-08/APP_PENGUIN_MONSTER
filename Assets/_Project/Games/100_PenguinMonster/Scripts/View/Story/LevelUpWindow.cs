using PenguinMonster.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// レベルが上がったときの能力の窓（PLAN_STORY_TOWN01.md フェーズ5）。本家と同じく、先に上がった分（＋いくつ）、
    /// 次のタップで新しい値を見せる。左の列に能力の名前、右の列に数を出す
    /// </summary>
    public class LevelUpWindow : MonoBehaviour
    {
        [SerializeField] private Text _names;
        [SerializeField] private Text _values;

        private static readonly string Names = string.Join("\n", "HP", StatKind.Attack.ToJapanese(), StatKind.Defense.ToJapanese(),
            StatKind.SpAttack.ToJapanese(), StatKind.SpDefense.ToJapanese(), StatKind.Speed.ToJapanese());

        public void ShowGains(Stats before, Stats after)
        {
            Show($"+{after.Hp - before.Hp}\n+{after.Attack - before.Attack}\n+{after.Defense - before.Defense}\n"
                + $"+{after.SpAttack - before.SpAttack}\n+{after.SpDefense - before.SpDefense}\n+{after.Speed - before.Speed}");
        }

        public void ShowTotals(Stats after)
        {
            Show($"{after.Hp}\n{after.Attack}\n{after.Defense}\n{after.SpAttack}\n{after.SpDefense}\n{after.Speed}");
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Show(string values)
        {
            _names.text = Names;
            _values.text = values;
            gameObject.SetActive(true);
        }
    }
}
