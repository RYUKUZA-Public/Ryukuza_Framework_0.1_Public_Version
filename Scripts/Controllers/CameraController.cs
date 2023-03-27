using UnityEngine;

/// <summary>
/// Controllerは、製作したいゲームによって、必要性が決定されるため必須ではない
/// 以下のコードは例で、今後機能別モジュール化予定
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>
    /// カメラモード
    /// </summary>
    [SerializeField]
    private Define.CameraMode _mode = Define.CameraMode.QuarterView;
    /// <summary>
    /// (0, 6, -5) カメラの位置づけによる
    /// </summary>
    [SerializeField]
    private Vector3 _delta = new Vector3(0, 6f, -5f);
    /// <summary>
    /// プレイヤー
    /// </summary>
    [SerializeField] private GameObject _player = null;

    private void LateUpdate()
    {
        if (_mode == Define.CameraMode.QuarterView)
        {
            // 壁を確認
            RaycastHit hit;
            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Wall")))
            {
                // ここでカメラを移動させる
                // 衝突した座標 - プレイヤーのPos = 方向ベクトル .magnitude = 方向ベクトルのサイズ
                float dist = (hit.point - _player.transform.position).magnitude * .8f;
                transform.position = _player.transform.position + _delta.normalized * dist;
            }
            else
            {
                // カメラPos(プレイヤー基準 + デルタ値)
                transform.position = _player.transform.position + this._delta;
                // ローテーションは、LookAtを使ってプレイヤーを見渡せる。
                transform.LookAt(_player.transform);
            }
        }
    }
    
    private void SetQuarterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuarterView;
        _delta = delta;
    }
}
