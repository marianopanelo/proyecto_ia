using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PatrolState : State
{
    /*aca para la maquina vamos a neceitar los datos q se necesiten ,
    para traerlo lo pongo en agent o en data, las 2 opciones estan bien */
    //private FMSAgent _agent;para poder usar este, tendria q tener las cosas en publico en el otro para poder traerlas
    private PatrolData _data;//termine usando data con todos 
    private int NumberPosition;
    private int sentido = 1;


    public PatrolState(/*FMSAgent agent*/ PatrolData data, FMS strateMachine) : base(strateMachine)
    {
        /*aca llamamos a el fms q esta con el contructor en State para tener las funciones de la maquina de estado
         FMS strateMachine aca le estoy mandando lo de cambiar y en base , lo q hace es q primero se ejecute strateMachine*/
        //yo aca solo use data
        //_agent = agent;
        _data = data;
    }
    public override void Enter()
    {
        Debug.Log("entre PatrolState ");
    }

    public override void Update()
    {
        Debug.Log("actualizo PatrolState");
        PatrollingLoop();
    }

    public override void Exit()
    {
        Debug.Log("sali PatrolState");
    }

    //ESTO VIENE DE FMSAgents
    //lo q puedo hacer aca es traer los datos de la forma de agent o de data(cualquiera esta bien)
    private void PatrollingLoop()
    {
        int numberOfPoints = _data.listPositions.Count;

        var nextposition = _data.listPositions[NumberPosition];
        if (Vector3.Distance(nextposition.position, _data.transform.position) <= _data.minDistans)
        {
            NumberPosition = NumberPosition + 1 < _data.listPositions.Count ? NumberPosition + 1 : 0;
        }

        var direccion = nextposition.position - _data.transform.position;
        _data.transform.position += direccion.normalized * _data.velocity * Time.deltaTime;
    }

    private void PatroPingPong()
    {
        int numberOfPoints = _data.listPositions.Count;

        var nextposition = _data.listPositions[NumberPosition];
        if (Vector3.Distance(nextposition.position, _data.transform.position) <= _data.minDistans)
        {
            if (NumberPosition == numberOfPoints - 1)
            {
                sentido = -1;
            }
            else if (NumberPosition == 0)
            {
                sentido = 1;
            }
            NumberPosition += sentido;
        }


        var direccion = nextposition.position - _data.transform.position;
        _data.transform.position += direccion.normalized * _data.velocity * Time.deltaTime;
    }
}


[System.Serializable]
public class PatrolData
{
    public List<Transform> listPositions;
    public Transform transform;
    public float minDistans;
    public float velocity;
}
