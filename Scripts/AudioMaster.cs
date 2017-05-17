using UnityEngine;

public static class AudioMaster {
	public static void Create (GameObject prefab, Vector3 pos, float delay = 0f) {
		GameObject prf = InstantiatePrefab(prefab) as GameObject;
		AudioSource as = prf.GetComponent<AudioSource>();
		as.PlayOnAwake = false;
		as.Stop();
		prf.transform.position = pos;
		if (delay > 0f) {
			StartCoroutine(CreateDelayed(as, delay));
		}
		else {
			as.Play();
		}
	}

	private static IEnumerator CreateDelayed (AudioSource as, float delay) {
		yield return new WaitForSeconds(delay);
		as.Play();
	}
}