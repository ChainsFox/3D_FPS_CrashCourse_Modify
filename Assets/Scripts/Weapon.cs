using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;

    //Shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    //Burst
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    //Spread
    public float spreadIntensity;

    //Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;

    //Effect
    public GameObject muzzleEffect;
    internal Animator animator; //"internal" means that it can be access by other script but not in the editor/inspector

    //Reload
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    //Weapon position and rotation
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;



    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public enum WeaponModel
    {
        M1911,
        M4  
    }

    public WeaponModel thisWeaponModel;



    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveWeapon)
        {
            GetComponent<Outline>().enabled = false;

            if (bulletsLeft == 0 && isShooting) //when try to shoot but no ammo
            {
                SoundManager.Instance.emptyMagazineSound_M1911.Play();
            }

            if (currentShootingMode == ShootingMode.Auto)
            {
                //holding m1
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
            {
                //clicking m1
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (readyToShoot && isShooting && bulletsLeft > 0 && !isReloading)
            {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                Reload();
            }

            //if you want to automatically reload when the magazine is empty
            if (readyToShoot && !isShooting && !isReloading && bulletsLeft <= 0)
            {
                Reload();
            }

            //ammo UI update(old - replace in p11)
            //if (HUBManager.Instance.ammoDisplay != null)
            //{
            //    //show the amount of bullets left and the bullets inside magazine - We also dividing with bulletsPerBurst because some weapon might have it
            //    //That also means even if any weapon doesn't fire in burst the burst variable would still need to be 1 because we don't want to divide with 0
            //    HUBManager.Instance.ammoDisplay.text = $"{bulletsLeft / bulletsPerBurst}/{magazineSize / bulletsPerBurst}";
            //}

        }

    }


    private void FireWeapon()
    {
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");
        //SoundManager.Instance.shootingSound_M1911.Play();
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false; //cant spam shoot if the first shot was not finished

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);//instantiate the bullet

        bullet.transform.forward = shootingDirection; //pointing the bullet to face the shooting direction

        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);//shoot

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime)); //destroy after delay

        //checking if we are done shooting
        if(allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        //burst mode
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1) //if we have more than 1 bullet left that means we still in the middle of the burst
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void Reload()
    {
        //SoundManager.Instance.reloadSound_M1911.Play();
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);

        animator.SetTrigger("RELOAD");
        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        //check if we have spare ammo or not and add it into the magazine to shoot
        if(WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > magazineSize)
        {
            bulletsLeft = magazineSize;
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }
        else
        {
            bulletsLeft = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }

        isReloading = false;
    }

    private void ResetShot() //Put it in a function so that it will have to wait for the delay and lock it into running only once 
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        //Shooting from the middle of the screen to check where are we pointing at - hit scan info
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //"Camera.main" -> find the main camera for the scene automatically
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            //hitting something - getting info 
            targetPoint = hit.point;
        }
        else
        {
            //shooting at the air
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position; //calculate the direction from the target point to the bullet spawn position, 

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        //returning the shooting direction and spread - the purpose of this function
        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
