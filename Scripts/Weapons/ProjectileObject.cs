using UnityEngine;

[System.Serializable]
public class Projectile {
	public int ownerID = 0;
	[Space(10)]
	public float damage = 10f;
	public float falloffRange = 20f;
	public float falloffRate = 1f;
	[Space(10)]
	public float speed = 30f;
	public int bounces = 1;
	public float piercePower = 0f;
	public bool killOnHurt = false;
	public float dropRange = 10f;
	public float dropRate = 1f;
	[Space(10)]
	public float lifespan = 10f;
	[Space(10)]
	public GameObject decalBounce;
	public GameObject decalEnd;
	[Space(10)]
	public GameObject soundHit;
	public GameObject soundBounce;
	public GameObject soundMiss;
	public float soundMissRange;
}

// Physics material Bouncy
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConstantForce))]
public class ProjectileObject : MonoBehaviour {

	public Projectile pj;

	private bool dropping = false;

	private ConstantForce cf;

	private void Start () {
		cf = GetComponent<ConstantForce>();
		GetComponent<SphereCollider>().radius = pj.soundMissRange;
		if (speed < 0f) {
			Hitscan();
		}
		else {
			if (pj.dropRange >= 0f) {
				StartCoroutine(StartDrop());
			}
			if (pj.lifespan > 0f) {
				StartCoroutine(StartLifespan());
			}
		}
	}

	private void FixedUpdate () {
		if (dropping) {
			cf.relativeForce = new Vector3(0, cf.relativeForce.y - Time.fixedTime * pj.dropRate, pj.speed);
		}
	}

	private IEnumerator StartDrop () {
		cf.relativeForce = new Vector3(0, 0, pj.speed);
		yield return new WaitForSeconds(pj.dropRange);
		dropping = true;
	}

	private IEnumerator StartLifespan () {
		yield return new WaitForSeconds(pj.lifespan);
		Kill();
	}

	private void OnCollisionEnter (Collision collision) {
		// Hits a unit
		if (collision.gameObject.tag == "Unit") {
			Unit unit = collision.gameObject.GetComponent<Unit>();
			if (unit.unitID == ownerID) {
				// Disc return
				Kill();
			}
			else {
				AudioMaster.Create(pj.soundHit, transform.position);
				unit.Hurt(pj.damage);
				pj.bounces = pj.killOnHurt ? 1 : pj.bounces;
			}
		}
		pj.bounces--;
		if (pj.bounces == 0) {
			GameObject prf = InstantiatePrefab(pj.decalEnd) as GameObject;
			prf.transform.rotation = transform.rotation;
			prf.transform.position = transform.position;
			AudioMaster.Create(pj.soundBounce, transform.position);
			Kill();
		}
		else {
			GameObject prf = InstantiatePrefab(pj.decalBounce) as GameObject;
			prf.transform.rotation = transform.rotation;
			prf.transform.position = transform.position;
			AudioMaster.Create(pj.soundBounce, transform.position);
		}
	}

	// Near miss
	private void OnTriggerExit (Collider collider) {
		if (collider.gameObject.tag == "Unit") {
			AudioMaster.Create(pj.soundMiss, transform.position);
		}
	}

	private void Hitscan () {
		if (pj.piercePower > 0f && !forceHit) {
			RaycastHit hits;
			hits = Physics.RaycastAll(transform.position, transform.forward, pj.lifespan * 10f);
			if (hits.Length == 1) {
				HitscanHit(hits[0], 1f);
				return;
			}
			else if (hits.Length > 1) {
				coll = GetComponent<Collider>();
				for (int i = 0; i < hits.Length-1; i++) {
					float distance;
					Vector3 direction;
					bool overlapped = Physics.ComputePenetration(
						coll, transform.position, transform.rotation,
						hits[i].collider, hits[i+1].transform.position, hits[i+1].transform.rotation,
						out direction, out distance
					);
					if (overlapped) {
						pj.piercePower -= distance;
					}
				}
				Hitscan(hits[hits.Length-1], piercePower / pj.piercePower);
			}
		}
		else {
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit, pj.lifespan * 10f)) {
				HitscanHit(hit, 1f);
			}
		}
		Kill();
	}

	private void HitscanHit (RaycastHit hit, float pierceMult) {
		if (hit.transform.GetComponent<Unit>()) {
			float falloffMult = hit.distance - pj.falloffRange;
			// Is within falloff range
			if (falloffMult <= 0f) {
				falloffMult = 1f;
			}
			else {
				falloffMult = 1 / (falloffMult * pj.falloffRate);
			}
			hit.transform.GetComponent<Unit>().Hurt(pj.damage * pierceMult * falloffMult);

			// Create sound and decal
			AudioMaster.Create(pj.soundHit, transform.position);
			GameObject prf = InstantiatePrefab(pj.decalEnd) as GameObject;
			prf.transform.rotation = transform.rotation;
			prf.transform.position = hit.point;
			prf.transform.parent = hit.transform;
		}
		else {
			GameObject prf = InstantiatePrefab(pj.decalEnd) as GameObject;
			prf.transform.rotation = hit.normal;
			prf.transform.posiiton = hit.point;
			AudioMaster.Create(pj.soundBounce, transform.position);
		}
	}

	private void Kill () {
		Destroy(gameObject);
	}

}