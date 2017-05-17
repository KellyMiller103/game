using UnityEngine;

[System.Serializable]
public class Gun {
	public int ownerID = 0;
	[Space(10)]
	public int clipSize = 10;
	public float fireRate = 0.2f;
	public float reloadRate = 2f;
	public Vector3 launchOffset = Vector3.Zero;
	[Space(10)]
	public GameObject projectile;
	public GameObject launchFX;
	[Space(10)]
	public AudioClip soundLaunch;
}

public class GunObject : MonoBehaviour {

	[Header("Gun")]
	public Gun gun;
	[Header("Projectile")]
	public Projectile pj;

}