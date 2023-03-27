using UnityEngine;

/// <summary>
/// Controllerは、製作したいゲームによって、必要性が決定されるため必須ではない
/// 以下のコードは例で、今後機能別モジュール化予定
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// プレーヤー移動速度
    /// </summary>
    [SerializeField] private float _speed = 10f;
    /// <summary>
    /// 目的地に移動するかどうか（マウス）
    /// </summary>
    //private bool _moveToDest = false;
    /// <summary>
    /// 目的地の位置
    /// </summary>
    private Vector3 _destPos;
    /// <summary>
    /// プレーヤー状態
    /// </summary>
    private enum PlayerState { Moving, Die, Idle}
    /// <summary>
    /// プレーヤー状態(Current)
    /// </summary>
    private PlayerState _state = PlayerState.Idle;

    private void Start()
    {
        // 購読申し込み
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }
    
    private void Update()
    {
        switch (_state)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Die:
                UpdateDie();
                break;
        }
    }
    
    private void OnMouseClicked(Define.MouseEvent evt)
    {
        
        if (_state == PlayerState.Die)
            return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
        UnityEngine.Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.magenta, 1.0f);
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Wall")))
        {
            // 目的地を保存した後、プレイヤーが移動しなければならない
            // 目的地ベクトルを取得
            _destPos = hit.point;
            // 移動true
            _state = PlayerState.Moving;
        }
    }

    private void UpdateIdle()
    {
        // アニメーション
        Animator anime = GetComponent<Animator>();
        anime.SetFloat("Speed", 0f);
    }
    
    private void UpdateMoving()
    {
        // 方向ベクトル
        Vector3 dir = _destPos - transform.position;
            
        // 小さい値だと仮定（到着？？）
        // float - floatの場合は誤差が生じるため
        if (dir.magnitude < 0.0001f)
        {
            _state = PlayerState.Idle;
        }
        // 到着前
        else
        {
            // 移動する値が、残りの距離より小さいことをチェックしないと、キャラクターの動きバグが発生する。
            // そのため、Clamp を用いて計算が必要。
            float moveDist = Mathf.Clamp(Time.deltaTime * _speed, 0, dir.magnitude);
            // 移動
            transform.position += dir.normalized * moveDist;
            // プレイヤーが目標を見る
            //transform.LookAt(_destPos);
            // プレイヤーが目標を見る (スムーズに回転)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
        
        // 移動アニメーション
        Animator anime = GetComponent<Animator>();
        anime.SetFloat("Speed", _speed);
    }

    private void UpdateDie()
    {
        // TODO. 死亡
    }
    
    
}
