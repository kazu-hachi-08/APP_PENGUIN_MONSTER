namespace PenguinMonster.View
{
    /// <summary>
    /// 技のエフェクトの「型」（粒の動き方）。タイプをまたいで同じ動きの技は同じ型にする（ブレスは氷でも火でも「吹き抜ける」）。
    /// 粒の絵はタイプの絵が基本。型の絵を使うかどうかは MoveEffectView.Stages.cs で決める
    /// </summary>
    public enum MoveStyle
    {
        /// <summary>体当たり・叩く。星が3つ、少しずつずれて「バシッ」と弾ける</summary>
        Hit,
        /// <summary>クチバシで突く。使った側から粒が3回、少しずつずれて突き刺さる</summary>
        Pierce,
        /// <summary>切る・ひっかく。切り傷が3本、斜めに走る</summary>
        Slash,
        /// <summary>噛む。上下の牙が2回かみ合う。下の牙は同じ絵を逆さにして使う</summary>
        Bite,
        /// <summary>まわりから粒が飛んできて刺さる</summary>
        Converge,
        /// <summary>ブレス・風。使った側から粒が吹き抜ける</summary>
        Blow,
        /// <summary>撃ち出す。使った側から粒が一列に飛んできて弾ける</summary>
        Shot,
        /// <summary>上から粒が次々に落ちてくる</summary>
        Fall,
        /// <summary>上から大きな粒が1つ落ち、まわりに散る</summary>
        Plunge,
        /// <summary>足元から粒が立ちのぼり、小さくなって消える</summary>
        Rise,
        /// <summary>真ん中から山なりに飛び散る</summary>
        Splash,
        /// <summary>渦を巻きながら集まる</summary>
        Swirl,
        /// <summary>まわりで粒がまたたく</summary>
        Twinkle,
    }
}
