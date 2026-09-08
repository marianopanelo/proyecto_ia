using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class IdleState : State
{
    [SerializeField] private float changePatronTimer = 3f;
    [SerializeField] float timer = 0;

    public IdleState(FMS strateMachine) : base(strateMachine)
    {
        //aca no le tengo q poner nada , xq solo tiene el debug.log, si se lo paso a un estado le tendria q pasar los parametros
    }
    public override void Enter()
    {
        timer = 0;
        Debug.Log("entre IdleState");
    }

    public override void Update()
    {
        timer += Time.deltaTime;
        if (timer >= changePatronTimer) 
        {

            StrateMachine.ChangeState(Estados.PatrolState);
            //llamo a StrateMachine xq heredan de State q tiene la maquina de estado y aca estoy cambiando a el estado PatrolState 

            Debug.Log("me muevo");
        }

        Debug.Log("actualizo IdleState");
    }

    public override void Exit()
    {
        Debug.Log("sali IdleState");
    }

}
