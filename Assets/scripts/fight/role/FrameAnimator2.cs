using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 序列帧动画播放器
/// 支持UGUI的_sprite和Unity2D的SpriteRenderer
/// </summary>
public class FrameAnimator2 : MonoBehaviour
{
    /// <summary>
    /// 序列帧数
    /// </summary>
    public int SpriteCount;
    /// <summary>
    /// 序列帧名字前半部分  例如： image_1.jpg中的   image_为前半部分
    /// </summary>
    public string SpriteName;
    /// <summary>
    /// resource文件夹下的文件夹路径 例如：Resources/Test/Texture  则直接赋值  Test/Texture   序列帧放在Texture文件夹下就好
    /// </summary>
    public string FolderPath;
    /// <summary>
    /// 序列帧
    /// </summary>
    private Sprite[] _sprites = null;

    /// <summary>
    /// 帧率，为正时正向播放，为负时反向播放
    /// </summary>
    public float Framerate { get { return _framerate; } set { _framerate = value; } }

    [SerializeField] private float _framerate = 20.0f;

    /// <summary>
    /// 是否忽略timeScale
    /// </summary>
    public bool IgnoreTimeScale { get { return _ignoreTimeScale; } set { _ignoreTimeScale = value; } }

    [SerializeField] private bool _ignoreTimeScale = true;

    /// <summary>
    /// 是否循环
    /// </summary>
    public bool Loop { get { return _loop; } set { _loop = value; } }

    [SerializeField] private bool _loop = true;

    //动画曲线
    [SerializeField] private AnimationCurve _curve = new AnimationCurve(new Keyframe(0, 1, 0, 0), new Keyframe(1, 1, 0, 0));

    /// <summary>
    /// 是否允许播放
    /// </summary>
    public bool IsPlayAnimator { get { return _isPlayAnimator; } set { _isPlayAnimator = value; } }
    [SerializeField] private bool _isPlayAnimator = false;

    /// <summary>
    /// 结束事件
    /// 在每次播放完一个周期时触发
    /// 在循环模式下触发此事件时，当前帧不一定为结束帧
    /// </summary>
    public event Action FinishEvent;

    //目标_sprite组件
    private SpriteRenderer _sprite;
    //当前帧索引
    private int _currentFrameIndex = 0;
    //下一次更新时间
    private float _timer = 0.0f;
    //当前帧率，通过曲线计算而来
    private float _currentFramerate = 20.0f;

    /// <summary>
    /// 重设动画
    /// </summary>
    public void Reset()
    {
        _currentFrameIndex = _framerate < 0 ? _sprites.Length - 1 : 0;
    }

    /// <summary>
    /// 从停止的位置播放动画
    /// </summary>
    public void Play()
    {
        this.enabled = true;
    }

    /// <summary>
    /// 暂停动画
    /// </summary>
    public void Pause()
    {
        this.enabled = false;
    }

    /// <summary>
    /// 停止动画，将位置设为初始位置
    /// </summary>
    public void Stop()
    {
        Pause();
        Reset();
    }
    
    //每次显示时，初始化当前帧数
    private void OnEnable()
    {
        Reset();
    }

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();

        SetData();
    }

    void Update()
    {
        //帧数据无效，禁用脚本
        if (_sprites == null || _sprites.Length == 0)
        {
            this.enabled = false;
        }
        else
        {
            //是否允许播放
            if (!PlayFrameAnimator()) return;
            //从曲线值计算当前帧率
            float curveValue = _curve.Evaluate((float)_currentFrameIndex / _sprites.Length);
            float curvedFramerate = curveValue * _framerate;
            //帧率有效
            if (curvedFramerate != 0)
            {
                //获取当前时间
                float time = _ignoreTimeScale ? Time.unscaledTime : Time.time;
                //计算帧间隔时间
                float interval = Mathf.Abs(1.0f / curvedFramerate);
                //满足更新条件，执行更新操作
                if (time - _timer > interval)
                {
                    //执行更新操作
                    DoUpdate();
                }
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogWarning("Framerate got '0' value, animation stopped.");
            }
#endif
        }
    }

    public void SetData()
    {
        //初始化精灵体数组
        _sprites = new Sprite[SpriteCount];
        for (int i = 0; i < SpriteCount; i++)
        {
            _sprites[i] = PPTextureManage.GetInstance().LoadAtlasSprite(FolderPath, SpriteName + i);
        }
#if UNITY_EDITOR
        if (_sprite == null)
        {
            Debug.LogWarning("No available component found. 'RawImage' required.", this.gameObject);
        }
#endif
    }

    /// <summary>
    /// 判断是否允许播放
    /// </summary>
    /// <returns>是否允许播放</returns>
    public bool PlayFrameAnimator()
    {
        return IsPlayAnimator;
    }

    //具体更新操作
    private void DoUpdate()
    {
        //计算新的索引
        int nextIndex = _currentFrameIndex + (int)Mathf.Sign(_currentFramerate);
        //索引越界，表示已经到结束帧
        if (nextIndex < 0 || nextIndex >= _sprites.Length)
        {
            //广播事件
            if (FinishEvent != null)
            {
                FinishEvent();
            }
            //非循环模式，禁用脚本
            if (_loop == false)
            {
                _currentFrameIndex = Mathf.Clamp(_currentFrameIndex, 0, _sprites.Length - 1);
                this.enabled = false;
                return;
            }
        }
        //钳制索引
        _currentFrameIndex = nextIndex % _sprites.Length;
        //更新图片
        if (_sprite != null)
        {
            _sprite.sprite = _sprites[_currentFrameIndex];
        }
        //设置计时器为当前时间
        _timer = _ignoreTimeScale ? Time.unscaledTime : Time.time;
    }
}