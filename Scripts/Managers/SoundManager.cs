using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 本Sound Managerは、基本的な形態であり、
/// 使用に応じて、構造変更が必要
/// </summary>
public class SoundManager
{
    /// <summary>
    /// Defineに設定したサウンドタイプ
    /// </summary>
    private AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    /// <summary>
    /// 経路とClipのキャッシング
    /// </summary>
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        // サウンド管理用オブジェクトを探していない場合は、作成
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject() { name = "@Sound" };
            Object.DontDestroyOnLoad(root);
            
            string[] soundTypeNames = System.Enum.GetNames(typeof(Define.Sound));
            // DefineのMaxCount除去のため-1
            for (int i = 0; i < soundTypeNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundTypeNames[i] };
                // 該当soundTypのAudioSource登録
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }
            
            // BGMはループする
            _audioSources[(int)Define.Sound.Bgm].loop = true;
        }
    }

    /// <summary>
    /// 再生Pathバージョン
    /// </summary>
    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }
    
    /// <summary>
    /// TODO. 再生AudioClipバージョン
    /// </summary>
    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;
        
        // BGM
        if (type == Define.Sound.Bgm)
        {
            // Bgm オーディオソース
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
            
            // 他のBGMが、再生中であれば、停止する。
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        // Effect
        else
        {
            // Effect オーディオソース
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
            audioSource.pitch = pitch;
            // 一度だけ再生
            audioSource.PlayOneShot(audioClip);
        }
    }
    
    /// <summary>
    /// AudioClipをインポートまたは追加
    /// </summary>
    private AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        // 経路に、Soundsがなければ貼ってあげよう
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";
        
        AudioClip audioClip = null;
        
        // BGM 
        if (type == Define.Sound.Bgm)
            // 該当Clipをロード
            audioClip = Managers.Resource.Load<AudioClip>(path);
        // Effect
        else
        {
            // 見つけたいAudioClipがあるかどうかを検索
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                // ない場合はロード後、追加
                audioClip = Managers.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }
        
        if (audioClip == null)
            Debug.Log($"AudioClipがありません。 {audioClip}");
        
        return audioClip;
    }
    
    /// <summary>
    /// クリア
    /// 本スクリプトは、Dontdestroyonloadであるため
    /// メモリが無限に蓄積される。
    /// そのため、注意してタイミングに合わせて、Clearを管理しなければならない。
    /// </summary>
    public void Clear()
    {
        _audioClips.Clear();

        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
    }
}