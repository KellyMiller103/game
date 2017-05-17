using UnityEngine;

public class Unit : MonoBehaviour {

	public int unitID = 0;

	private float health = 100f;
	private float healthMax = 100f;
	private float shield = 20f;
	private float shieldMax = 20f;

	public void Hurt (float damage, int type = 0) {
		// type: 0 = normal, 1 = percent max, 2 = percent current
		if (type == 1) {
			damage = healthMax * damage;
		}
		else if (type == 2) {
			damage = health * damage;
		}

		if (shield > 0f) {
			shield -= damage;
			if (shield <= 0f) {
				damage = -shield;
				shield = 0f;
			}
			else {
				return;
			}
		}

		health -= damage;
		if (health <= 0f) {
			Destroy(gameObject);
		}
	}

	public void Heal (float heal, int type = 0) {
		// type: 0 = normal, 1 = percent max, 2 = percent current
		if (type == 1) {
			heal = healthMax * heal;
		}
		else if (type == 2) {
			heal = health * heal;
		}

		health += heal;
		if (health > healthMax) {
			health = healthMax;
		}
	}

}