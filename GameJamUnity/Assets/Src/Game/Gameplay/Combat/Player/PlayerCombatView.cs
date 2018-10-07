using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCombatView : GhostGen.EventDispatcherBehavior, IDamageable
{
    private const int kFXPoolSize = 50;


    public Rigidbody _rigidBody;
    public Transform _weaponEndPoint;
    public Transform _rotateTransform;

    // (This should all be in a weapon class)
    public GameObject _bulletFXPrefab;
    private TrailRenderer[] _fxPool;
    private Tween[] _fxTweens;
    private int _fxIndex;

    public PlayerCombatController controller { get; set; }

    // Use this for initialization
    void Awake ()
    {
        setupFXPool();
    }

    public Vector3 rigidPosition
    {
        get { return _rigidBody.position; }
    }

    public Rigidbody physbody
    {
        get { return _rigidBody; }
    }

    public Vector3 viewPosition
    {
        get { return _rotateTransform.position; }
    }

    public Vector3 weaponBarrelPosition
    {
        get { return _weaponEndPoint.transform.position; }
    }

    public Quaternion rotation
    {
        get { return _rotateTransform.rotation; }
        set { _rotateTransform.rotation = value; }
    }

    public void SetAimPosition(Vector3 aimPosition)
    {
        aimPosition.y = _rotateTransform.position.y;
        _rotateTransform.LookAt(aimPosition);
    }

    public void SetCurrentWeapon(string weapon)
    {

    }


    public float health
    {
        get { return controller != null ? controller.health : 0; }
    }

    public DamageResult TakeDamage(object attacker, float damage)
    {
        return controller != null ? controller.TakeDamage(attacker, damage) : null;
    }

    public void VisualFireWeapon(float speed, Vector3 target)
    {
        Vector3 startPos = _weaponEndPoint.position;

        TrailRenderer fx = getNextFX();
        Tween t = _fxTweens[_fxIndex];

        //fx.Clear();

        fx.transform.position = startPos;
        
        t = fx.transform.DOMove(target, speed);
        t.SetEase(Ease.Linear);
        t.SetSpeedBased(true);

        t.OnStart(() =>
        {
           // fx.time = time;
            fx.transform.position = startPos;
            fx.gameObject.SetActive(true);
            //fx.emitting = (true);
            fx.Clear();
        });

        t.OnComplete(()=>
        {
            //fx.Clear();
            //fx.emitting = (false);
            fx.gameObject.SetActive(false);
            t = null;
        });
    }

    private TrailRenderer getNextFX()
    {
        TrailRenderer fx = _fxPool[_fxIndex];
        _fxIndex = (_fxIndex + 1) % _fxPool.Length;
        return fx;
    }

    private void setupFXPool()
    {
        _fxIndex = 0;

        _fxPool = new TrailRenderer[kFXPoolSize];
        _fxTweens = new Tween[kFXPoolSize];

        for(int i = 0; i < kFXPoolSize; ++i)
        {
            GameObject fxObj = GameObject.Instantiate<GameObject>(_bulletFXPrefab, transform.position, transform.rotation);
            _fxPool[i] = fxObj.GetComponent<TrailRenderer>();
            _fxPool[i].gameObject.SetActive(false);
            //_fxPool[i].emitting = (false);
        }
    }
    
}
