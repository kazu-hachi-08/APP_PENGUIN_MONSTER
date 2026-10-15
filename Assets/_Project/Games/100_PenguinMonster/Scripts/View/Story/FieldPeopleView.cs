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

        private readonly List<SpriteRenderer> _renderers = new List<SpriteRenderer>();
        private readonly Dictionary<string, SpriteRenderer> _byId = new Dictionary<string, SpriteRenderer>();

        public void Draw(IEnumerable<PlacedNpc> people, FieldMapView mapView)
        {
            _byId.Clear();
            int used = 0;
            foreach (var placed in people)
            {
                var renderer = RendererAt(used++);
                renderer.transform.position = mapView.FeetPosition(placed.Position);
                renderer.sprite = NpcArt.Get(placed.Npc.Look, placed.Placement.Spot.Facing);
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
                renderer.sprite = NpcArt.Get(npc.Look, facing);
            }
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
