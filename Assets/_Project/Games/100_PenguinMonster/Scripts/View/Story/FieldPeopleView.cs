using System.Collections;
using System.Collections.Generic;
using PenguinMonster.Story;
using UnityEngine;

namespace PenguinMonster.View.Story
{
    /// <summary>
    /// 今のマップに立っている人の絵。マップを描くたびと、出来事の後（フラグで人が動くため）に並べ直す。
    /// 絵は SpriteRenderer を使い回し、足りなければ足す（1枚のマップに立つ人は数人なので、消さずに隠しておく）
    /// </summary>
    public class FieldPeopleView : MonoBehaviour
    {
        [Tooltip("主人公と同じ高さに描く（マップのタイルより手前）")]
        [SerializeField] private int _sortingOrder = 1;
        [Tooltip("「！」を人の足元からどれだけ上に出すか（マス）。頭の少し上")]
        [SerializeField] private float _markHeight = 1.05f;

        private readonly List<SpriteRenderer> _renderers = new List<SpriteRenderer>();
        private readonly Dictionary<string, SpriteRenderer> _byId = new Dictionary<string, SpriteRenderer>();
        private SpriteRenderer _mark;

        public void Draw(IEnumerable<PlacedNpc> people, FieldMapView mapView)
        {
            _byId.Clear();
            int used = 0;
            foreach (var placed in people)
            {
                var renderer = RendererAt(used++);
                renderer.transform.position = mapView.FeetPosition(placed.Position);
                renderer.sprite = NpcArt.Get(placed.Npc, placed.Placement.Spot.Facing);
                renderer.enabled = true;
                _byId[placed.Npc.Id] = renderer;
            }
            for (int i = used; i < _renderers.Count; i++)
            {
                _renderers[i].enabled = false;
            }
        }

        /// <summary>話しかけられた人を、話しかけた側へ向ける</summary>
        public void Face(FieldNpc npc, Direction facing)
        {
            if (_byId.TryGetValue(npc.Id, out var renderer))
            {
                renderer.sprite = NpcArt.Get(npc, facing);
            }
        }

        /// <summary>トレーナーと目が合った。頭の上に「！」を出して、少し待ってから消す</summary>
        public IEnumerator Exclaim(FieldNpc npc, float seconds)
        {
            if (!_byId.TryGetValue(npc.Id, out var renderer))
            {
                yield break;
            }
            var mark = MarkRenderer();
            mark.transform.position = renderer.transform.position + Vector3.up * _markHeight;
            mark.enabled = true;
            yield return new WaitForSeconds(seconds);
            mark.enabled = false;
        }

        /// <summary>目が合ったトレーナーが1マス歩く。前半は足を出したコマ、後半は立ちのコマ（主人公と同じ）</summary>
        public IEnumerator Walk(FieldNpc npc, Vector3 to, Direction facing, float seconds)
        {
            if (!_byId.TryGetValue(npc.Id, out var renderer))
            {
                yield break;
            }
            renderer.sprite = NpcArt.Get(npc, facing, 1);
            yield return FieldAvatar.Slide(renderer.transform, to, seconds, () => renderer.sprite = NpcArt.Get(npc, facing));
            renderer.sprite = NpcArt.Get(npc, facing);
        }

        private SpriteRenderer MarkRenderer()
        {
            if (_mark == null)
            {
                var obj = new GameObject("Exclamation");
                obj.transform.SetParent(transform, false);
                _mark = obj.AddComponent<SpriteRenderer>();
                _mark.sprite = NpcArt.Exclamation();
                // 人の絵より手前に出す
                _mark.sortingOrder = _sortingOrder + 1;
            }
            return _mark;
        }

        private SpriteRenderer RendererAt(int index)
        {
            while (_renderers.Count <= index)
            {
                var person = new GameObject($"Person{_renderers.Count}");
                person.transform.SetParent(transform, false);
                var renderer = person.AddComponent<SpriteRenderer>();
                renderer.sortingOrder = _sortingOrder;
                _renderers.Add(renderer);
            }
            return _renderers[index];
        }
    }
}
