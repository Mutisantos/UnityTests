using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**Script para manejar el comportamiento de un objeto que puede ser arrastrado con el mouse y moverse dentro de los límites de la pantalla
* Esteban.Hernandez
 */
public class BallThrowScript : MonoBehaviour {

	[SerializeField]
	Vector3 screenOffset;
	[SerializeField]
	Vector3 screenSpace;
	//Controlar que el elemento no sufra aceleraciones mientras sea arrastrado por el usuariogit 
	[SerializeField]
	bool dragged;
	bool grounded;
	Vector3 lastCursorPos;

	[SerializeField]
	Vector2 velocity;
	[SerializeField]
	Vector2 initialSpeed;
	public float gravity;
	[Tooltip("Valores de 0.0f - 1.0f")]
	public float bounceFactor;
	public float offset;
	public float mouseForceFactor;

	void Start () {
		dragged = false;
		lastCursorPos = new Vector3(Input.mousePosition.x,Input.mousePosition.y, screenSpace.z);
		Cursor.lockState = CursorLockMode.Confined;
		initialSpeed = Vector3.zero;
		velocity = Vector3.zero;
	}
	
	// Calculo de los movimientos de la pelota en movimiento
	void FixedUpdate () {
		if(!dragged && !grounded){
			//velocidad uniformemente acelerada en Ys
			velocity.y += initialSpeed.y + (gravity * Physics2D.gravity.y * Time.deltaTime);
			initialSpeed.y *= 0.5f;
			//velocidad constante en X
			velocity.x = initialSpeed.x;
			Vector2 deltapos = velocity * Time.deltaTime;
			//Movimiento horizontal
			Vector2 move = Vector2.right * deltapos.x;
			Movement(new Vector3(move.x,move.y,0));
			//Movimiento vertical
			move = Vector2.up * deltapos.y;
			Movement(new Vector3(move.x,move.y,0));
			checkLimits();
		}
	}

	void OnMouseDown()
	{
		screenSpace = Camera.main.WorldToScreenPoint(transform.position);
    	screenOffset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, screenSpace.z));
	}

	void OnMouseUp()
	{
		dragged = false;
	}

	void OnMouseDrag()
	{
		dragged = true;
		grounded = false;
		Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);    
 	    //transformar la posición del mouse en una posición del mundo ajustada al screenOffset
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + screenOffset;
 	    //mover el objeto a la posición del cursor
    	transform.position = curPosition;
		velocity = Vector3.zero;
	}


	//Detectar los cambios de velocidad del cursor para la velocidad en los ejes
	void Update()
	{
		if(dragged){
			Vector3 actualCursorPos = new Vector3(Input.mousePosition.x,Input.mousePosition.y, screenSpace.z);
			float xSpeed = actualCursorPos.x - lastCursorPos.x;
			float ySpeed = actualCursorPos.y - lastCursorPos.y;
			initialSpeed = new Vector2(xSpeed/mouseForceFactor,ySpeed/mouseForceFactor);
						
			lastCursorPos = actualCursorPos;
		}
		
	}


	void checkLimits(){
		//La Camara puede ser redimensionarze
        float vertExtent = BoundaryChecker.instance.VertExtent;
		float HorzExtent = BoundaryChecker.instance.HorzExtent;
		Vector2 bounceDir = Vector2.zero;
		if(transform.position.x > HorzExtent){//Derecha-> Rebote a la izquierda

			Debug.Log("derecha");
			Bounce(Vector2.left);
			bounceDir += Vector2.left;
		}
		if (transform.position.x < -HorzExtent){//Izquierda -> Rebote a la derecha
			Debug.Log("izquierda");
			Bounce(Vector2.right);
			bounceDir += Vector2.right;
		}
		if(transform.position.y > vertExtent){//Arriba -> Rebote abajo
			Debug.Log("arriba");
			Bounce(Vector2.down);
			bounceDir += Vector2.down;
		}
		if (transform.position.y < -vertExtent){//Abajo-> Rebote arriba
			Debug.Log("abajo");
			Bounce(Vector2.up);
			bounceDir += Vector2.up;
			if(velocity.y < offset){
				grounded = true;
				velocity.y = 0;
				initialSpeed.y = 0;
			}
			
		}
		


			
	}


	void Movement (Vector3 move){
		transform.position = transform.position + move;
	}

	void Bounce(Vector2 normalVector){
		var speed = velocity.magnitude * bounceFactor;
        var direction = Vector2.Reflect(velocity.normalized, normalVector);
		velocity = speed * direction;
		initialSpeed.x = speed * direction.x;
	}



}
