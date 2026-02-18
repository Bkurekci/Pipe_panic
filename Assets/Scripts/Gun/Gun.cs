using System;
using UnityEngine;
using UnityEngine.Pool;
using Unity.Cinemachine;

public class Gun : MonoBehaviour
{
    public static Action OnShoot;
    //public Transform BulletSpawnPoint => _bulletSpawnPoint;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _gunfireCD;

    private static readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private ObjectPool<Bullet> objectPool;
    private CinemachineImpulseSource _impulseSource;
    private Animator _fire;
    private Vector2 _mousePos;
    private float _lastFireTime = 0f;

    void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _fire = GetComponent<Animator>();
    }

    void Start()
    {
        CreateBullet();
    }
    void OnEnable()
    {
        OnShoot += ShootProjectile;
        OnShoot += FireAnimation;
        OnShoot += ResetLastTime;
        OnShoot += GunScreenShake;
    }

    void OnDisable()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= FireAnimation;        
        OnShoot -= ResetLastTime;
        OnShoot -= GunScreenShake;
    }

    public void ReleaseBullet(Bullet bullet)
    {
        objectPool.Release(bullet);
    }
    private void CreateBullet()//object pooling
    {
        objectPool = new ObjectPool<Bullet>(() => {
         return Instantiate(_bulletPrefab);
        }, bullet =>
        {
            bullet.gameObject.SetActive(true);
        }, bullet =>
        {
            bullet.gameObject.SetActive(false);
        }, bullet =>
        {
            Destroy(bullet);
        }, false, 20, 40);
    }
    private void Update()
    {
        RotateGun();
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time >= _lastFireTime) {//son vurustan bu yana ne kadar gecti?
                OnShoot?.Invoke();
        }
    }

    private void ShootProjectile()
    {
        Bullet newBullet = objectPool.Get();
        newBullet.Init(this, _bulletSpawnPoint.position, _mousePos);
    }

    private void FireAnimation()
    {
        _fire.Play(FIRE_HASH, 0, 0f);
    }
    private void ResetLastTime()
    {
        _lastFireTime = Time.time + _gunfireCD;//suanki zamana cooldown ekleyerek daha pratik takip edebiliriz
    }

    private void GunScreenShake()
    {
        _impulseSource.GenerateImpulse();
    }

    void RotateGun()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//piksel ekran konumundan dunya spaceine cevrimi yapariz
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePos);//silahin -xde ters donmemesi icin, bize gore konumu degil, oyun dunyasina gore konumu gerekir. Bu yuzden alinan degerler bana gore degil, oyunun icerisindeki karakterlere gore alinir(bana gore solda olan konumun, karsimdaki oyuncuya gore sagda olmasi)
        float _angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//x ve y dogrularinin arasindaki farki radyan cinsinden verir (1, -1 (x), -1, 1 (y)), radyani dereceye cevirmemiz gerektigi icin Rad2Deg kullaniriz(radyan to degree)
        transform.localRotation = Quaternion.Euler(0, 0, _angle);//lokal olarak silahin z de donusunu yaptiririz
    }
    
}
