using PenguinMonster.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View
{
    /// <summary>
    /// 場のペンモン1体の表示（仮の絵・名前・HP）
    /// </summary>
    public class MonsterPanel : MonoBehaviour
    {
        [SerializeField] private Image _body;
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _hpText;
        [SerializeField] private RectTransform _hpFill;
        [SerializeField] private Image _hpFillImage;
        [SerializeField] private float _faintedAlpha = 0.25f;
        [SerializeField] private float _hpYellowRatio = 0.5f;
        [SerializeField] private float _hpRedRatio = 0.2f;

        public void Show(BattleMonster monster, int hp)
        {
            var color = TypeColors.Get(monster.Definition.Types[0]);
            _body.color = color;
            _nameText.text = $"{monster.Name}  {TypeLabel(monster)}";
            SetHp(hp, monster.MaxHp);
        }

        public void SetHp(int hp, int maxHp)
        {
            float ratio = maxHp > 0 ? (float)hp / maxHp : 0f;
            _hpFill.anchorMax = new Vector2(ratio, 1f);
            _hpFillImage.color = ratio > _hpYellowRatio ? Color.green : ratio > _hpRedRatio ? Color.yellow : Color.red;
            _hpText.text = $"{hp} / {maxHp}";
        }

        public void SetFainted()
        {
            var color = _body.color;
            color.a = _faintedAlpha;
            _body.color = color;
        }

        private static string TypeLabel(BattleMonster monster)
        {
            var types = monster.Definition.Types;
            return types.Count == 1 ? types[0].ToJapanese() : $"{types[0].ToJapanese()}/{types[1].ToJapanese()}";
        }
    }
}
