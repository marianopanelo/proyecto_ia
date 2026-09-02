using UnityEngine;

public class Motion : MonoBehaviour
{
    [SerializeField] private GameObject _GameObject;
    [SerializeField] private float _MaxSpeed = 5f;
    [SerializeField] private float _MaxSterring = 5f;
    [SerializeField] private float _SlowingDistance = 3f;
    [SerializeField] private float _MinDistance = 0.1f;

    private Vector3 _TargetPosition;
    private Vector3 _Direccion;

    public enum SteeringModes { Seek, Flee, Arrive, Pursuit, Evade, Flocking }
    public SteeringModes currentSterring;

    private void Awake()
    {
        Vector3 randomDireccion = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        _Direccion += randomDireccion.normalized * _MaxSpeed;
    }

    private void Update()
    {

       // _Direccion += sterringVector();
       transform.position += _Direccion * Time.deltaTime;

        if (_Direccion != Vector3.zero)
        {
            transform.forward = _Direccion;
        }
        transform.position = Map.instance.OutOfMap(transform.position);
    }

    private Vector3 sterringVector()
    {
        switch (currentSterring)
        {
            case SteeringModes.Seek:
                return Seek();
            case SteeringModes.Flee:
                return Flee();
            case SteeringModes.Arrive:
                return Arrive();
            case SteeringModes.Pursuit:
                return Pursuit();
            case SteeringModes.Evade:
                return Evade();
            case SteeringModes.Flocking:
                return Flocking();
            default:
                return Vector3.zero;
        }
    }

    private Vector3 CalculateSteering(Vector3 desired)
    {
        Vector3 steering = desired - _Direccion;
        steering = Vector3.ClampMagnitude(steering, _MaxSterring * Time.deltaTime);
        return steering;
    }

    private Vector3 DesiredVector(Vector3 target)
    {
        Vector3 desired = (target - transform.position).normalized * _MaxSpeed;
        return desired;
    }

   





}









