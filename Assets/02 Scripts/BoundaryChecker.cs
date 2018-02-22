using UnityEngine;

/** Singleton que revisa los límites de la pantalla (las dimensiones pueden cambiarse) 
 * Esteban.Hernandez
 */
public class BoundaryChecker : MonoBehaviour {

	public static BoundaryChecker instance;
	public float VertExtent;
	public float HorzExtent;

	public Vector2 offset;

	void Awake() {
		MakeSingleton ();
	}

	private void MakeSingleton() {
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}
	void Start () {
		calculateBounds();
	}
	
	void Update () {
		calculateBounds();		
	}

	void calculateBounds(){
		VertExtent = Camera.main.orthographicSize * offset.y;
		HorzExtent = (VertExtent * Screen.width / Screen.height) * offset.x;
	}
}
