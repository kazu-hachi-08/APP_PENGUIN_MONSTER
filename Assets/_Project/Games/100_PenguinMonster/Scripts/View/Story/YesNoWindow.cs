using System;
using System.Collections;
using PenguinMonster.Story;
using UnityEngine;
using UnityEngine.UI;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// 「はい / いいえ」の窓（PLAN_STORY_TOWN01.md フェーズ4）。会話窓の右上に重ねる（ダイパと同じ）。
    /// 押して選ぶほか、PC は上下で ▶ を動かして A（Z）で決める。B（X）は いいえ（本家と同じく、断る側へ逃げられるように）
    /// </summary>
    public class YesNoWindow : MonoBehaviour
    {
        [SerializeField] private Button _yes;
        [SerializeField] private Button _no;
        [Tooltip("選んでいる方の左に出す ▶。はい・いいえの順")]
        [SerializeField] private GameObject[] _cursors;
        [SerializeField] private FieldInput _input;

        private bool? _answer;
        private bool _cursorOnYes;

        private void Awake()
        {
            _yes.onClick.AddListener(() => Decide(true));
            _no.onClick.AddListener(() => Decide(false));
        }

        public IEnumerator Ask(Action<bool> onAnswer)
        {
            _answer = null;
            MoveCursor(true);
            gameObject.SetActive(true);
            // 会話を送った A がそのまま「はい」にならないよう、窓を出したフレームは入力を見ない
            yield return null;
            Direction? lastHeld = _input.HeldDirection;
            while (_answer == null)
            {
                lastHeld = ReadKeys(lastHeld);
                yield return null;
            }
            gameObject.SetActive(false);
            onAnswer(_answer.Value);
        }

        /// <summary>上下は押し続けても1回だけ動かす（押した瞬間だけ見る）</summary>
        private Direction? ReadKeys(Direction? lastHeld)
        {
            var held = _input.HeldDirection;
            if (held != lastHeld && (held == Direction.Up || held == Direction.Down))
            {
                MoveCursor(!_cursorOnYes);
            }
            if (_input.IsADown)
            {
                Decide(_cursorOnYes);
            }
            else if (_input.IsBDown)
            {
                Decide(false);
            }
            return held;
        }

        private void MoveCursor(bool onYes)
        {
            _cursorOnYes = onYes;
            _cursors[0].SetActive(onYes);
            _cursors[1].SetActive(!onYes);
        }

        private void Decide(bool yes)
        {
            if (_answer != null) return;
            BattleSounds.Play(BattleSound.Cursor);
            _answer = yes;
        }
    }
}
