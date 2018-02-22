using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script para manejar el comportamiento de un objeto que puede ser arrastrado con el mouse y moverse dentro de los límites de la pantalla
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
	public float bounceFactor;
	public float offset;
	public float mouseForceFactor;

	// Use this for initialization
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


	protected void checkLimits(){
		//La Camara puede ser redimensionarze
        float vertExtent = BoundaryChecker.instance.VertExtent;
		float HorzExtent = BoundaryChecker.instance.HorzExtent;
	
		if(transform.position.x > HorzExtent){//Derecha-> Rebote a la izquierda
			
			Debug.Log("derecha");
			Bounce(Vector3.left);

		}
		if (transform.position.x < -HorzExtent){//Izquierda -> Rebote a la derecha
			
			Debug.Log("izquierda");
			Bounce(Vector3.right);
		}
		if(transform.position.y > vertExtent){//Arriba -> Rebote abajo
			
			Debug.Log("arriba");
			Bounce(Vector3.down);
		}
		if (transform.position.y < -vertExtent){//Abajo-> Rebote arriba
			
			Debug.Log("abajo");
			Bounce(Vector3.up);
			if(velocity.y < offset){
				grounded = true;
			}
			
			
		}

			
	}


	void Movement (Vector3 move){
		transform.position = transform.position + move;
	}

	void Bounce(Vector3 normalVector){
		var speed = velocity.magnitude;
        var direction = Vector3.Reflect(velocity.normalized, normalVector);
		velocity = speed * direction;
	}



}
