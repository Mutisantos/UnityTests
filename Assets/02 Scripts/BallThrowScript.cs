using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script para manejar el comportamiento de un objeto que puede ser arrastrado con el mouse y moverse dentro de los límites de la pantalla
public class BallThrowScript : MonoBehaviour {


	Vector3 offset;
	Vector3 screenSpace;
	
	bool dragged;
	[SerializeField]
	float xSpeed;
	Vector3 lastCursorPos;


	protected Vector2 velocity;

	public float gravity;

	// Use this for initialization
	void Start () {
		lastCursorPos = new Vector3(Input.mousePosition.x,Input.mousePosition.y, screenSpace.z);
		Cursor.lockState = CursorLockMode.Confined;
	}
	
	// Calculo 
	void FixedUpdate () {
		velocity += gravity * Physics2D.gravity * Time.deltaTime;
		Vector2 deltapos = velocity * Time.deltaTime;
		Vector2 move = Vector2.up * deltapos.y;
		Movement(new Vector3(move.x,move.y,0));
		checkLimits();
	}

	void OnMouseDown()
	{
		screenSpace = Camera.main.WorldToScreenPoint(transform.position);
    	offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, screenSpace.z));
	}

	void OnMouseDrag()
	{
		Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);    
 	    //transformar la posición del mouse en una posición del mundo ajustada al offset
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
 	    //mover el objeto a la posición del cursor
    	transform.position = curPosition;
	}

	//Detectar los cambios de velocidad del cursor para la velocidad en los ejes
	void LateUpdate()
	{
		Vector3 actualCursorPos = new Vector3(Input.mousePosition.x,Input.mousePosition.y, screenSpace.z);
		xSpeed = Mathf.Abs(actualCursorPos.x - lastCursorPos.x);
		Debug.Log(xSpeed);
		lastCursorPos = actualCursorPos;
	}


	protected void checkLimits(){
		//La Camara puede ser redimensionarze
        float vertExtent = BoundaryChecker.instance.VertExtent;
		float HorzExtent = BoundaryChecker.instance.HorzExtent;
		Vector2 newPosition = transform.position;

		if(transform.position.x > HorzExtent){//Derecha-> Rebote a la izquierda
			Cursor.lockState = CursorLockMode.Confined;
			Debug.Log("derecha");

		}
		else if (transform.position.x < -HorzExtent){//Izquierda -> Rebote a la derecha
			Cursor.lockState = CursorLockMode.Confined;
			Debug.Log("izquierda");
		}
		if(transform.position.y > vertExtent){//Arriba -> Rebote abajo
			Cursor.lockState = CursorLockMode.Confined;
			Debug.Log("arriba");
		}
		else if (transform.position.y < -vertExtent){//Abajo-> Rebote arriba
			Cursor.lockState = CursorLockMode.Confined;
			Debug.Log("abajo");
		}
		if(newPosition.x != transform.position.x || newPosition.y != transform.position.y)
			transform.position = (newPosition);
			
	}


	void Movement (Vector3 move){
		transform.position = transform.position + move;
	}


}
