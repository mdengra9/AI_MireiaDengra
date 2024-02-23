using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RepasoExamen : MonoBehaviour
{
    //Crear estados
    public enum State
    {
        Patrolling,
        Chasing,
        Attacking
    }

    public State currentState;

    //Para acceder al inspector
    private NavMeshAgent agent;

    //Si lo pongo publico puedo arrastar el player desde el inspector y no hacer el player del Awake
    private Transform player;

    //Para almacenar dentro los puntos de patrulla
    [SerializeField] private Transform[] patrolPoints;

    //Rango de detección
    [SerializeField] private float detectionRange = 15;
    //Rango de ataque
    [SerializeField] private float attackingRange = 5;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //Si la variable es privada y asegurarnos que el player tiene el tag de Player
        player = GameObject.FindWithTag("Player").transform;
    }

    void Start()
    {
        SetRandomPoint();
        currentState = State.Patrolling;
    }

    // Update is called once per frame
    void Update()
    {
        //Según en el estado que estemos ejecuta una cosa o otra
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
            break;

            case State.Chasing:
                Chase();
            break;

            case State.Attacking:
                Attack();
            break; 
        }
    }

    //Creamos las funciones que hemos puesto en el paso anterior
    void Patrol()
    {
        //Si el jugador esta dentro del rango que cambie el estado
        if(IsInRange(detectionRange) == true)
        {
            currentState = State.Chasing;
        }

        //Comprobar si ha llegado al punto de destino y para que la función funcione
        if(agent.remainingDistance < 0.5f)
        {
            //Pegamos la primera linea del Start
            SetRandomPoint();
        }
    }

    void Chase()
    {//Para que vuelva al estado de patrullar (Copiamos el primer if del estado anterior)
        if(IsInRange(detectionRange) == false)
        {
            //Pegamos la ultima linea del estado anterior
            SetRandomPoint();
            currentState = State.Patrolling;
        }

        //Para que cambie al estado de ataque (otra vez el mismo if pero cambiando el final)
        if(IsInRange(attackingRange) == true)
        {
            currentState = State.Attacking;
        }

        //Hacer que la función funcione   
        agent.destination = player.position;
        
    }

    void Attack()
    {
        Debug.Log("Atacando");

        currentState = State.Chasing;
    }

    //Para que se mueva aleatoriamente entre los puntos de ruta que tenemos
    void SetRandomPoint()
    {
        agent.destination = patrolPoints[Random.Range(0, 4)].position;
    }

    //Para indicar si esta dentro de un rango o no
    bool IsInRange(float range) 
    {
        if(Vector3.Distance(transform.position, player.position) < range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Opcional para ver dibujados los puntos de ruta
    void OnDarwGizmos()
    {
        Gizmos.color = Color.blue;

        foreach(Transform point in patrolPoints)
        {
            Gizmos.DrawWireSphere(point.position, 0.5f);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackingRange);
    }
}

//Crear en el inspector 5 puntos de ruta y asignarlos en el inspector
//Acordarme de poner el Unity Engine arriba
//Asignar todo lo que me salga en el inspector

//Para descargar la ia descargamos desde el package manager "AI Navigation"
//Para que la ia funcione creamos una maya de navegacion desde "window" - "ai" - "navegation" y cambiamos el valor de los dos ultimos puntos a 12 por ejemplo
//En el inspector creamos "ai" - "Nav mesh surface" y backeamos "Bake"
//Para que genere los salto activamos "Generate Links" y le volvemos a dar a "Bake"

//Para el escenario instalamos "pro builder" desde package manager
//Para sacar la ventana "Tools" - "Pro builder window"
//Se estruye con el shift y si tambien presionamos control es más preciso
//En la escalera cambiar "count" por "Height" y poner una altura de escalon de 0.5
//Para cambiar el color es desde vertex color 
