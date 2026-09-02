using UnityEngine;

public class Motion : MonoBehaviour
{
    [SerializeField] private GameObject _GameObject;
    [SerializeField] private float _MaxSpeed = 5f;
    [SerializeField] private float _MaxSterring = 5f;
    [SerializeField] private float _SlowingDistance = 3f;
    [SerializeField] private float _MinDistance = 0.1f;

    private Vector3 _TargetPosition;
    private Vector3 _velocity;

    public enum SteeringModes { Seek, Flee, Arrive, Pursuit, Evade, Flocking }
    public SteeringModes currentSterring;

    private void Awake()
    {
        Vector3 randomDireccion = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        _velocity += randomDireccion.normalized * _MaxSpeed;
    }

    private void Update()
    {
        //_velocity += sterringVector(); si lo descomento , los casazos se van a cualquier lado pero el casador anda con las demas funciones
        transform.position += _velocity * Time.deltaTime;

        if (_velocity != Vector3.zero)
        {
            transform.forward = _velocity;
        }
        transform.position = Map.instance.OutOfMap(transform.position);
    }

    private Vector3 sterringVector()
    {
        switch (currentSterring)
        {
            case SteeringModes.Seek:
                return Seek(_GameObject.transform.position);
            case SteeringModes.Flee:
                return Flee(_GameObject.transform.position);
             case SteeringModes.Arrive:
                 return Arrive(_GameObject.transform.position);
             case SteeringModes.Pursuit:
                 return Pursuit(_GameObject.transform.position);
             /*case SteeringModes.Evade:
                 return Evade();*/
            case SteeringModes.Flocking:
                return transform.position;
            default:
                return Vector3.zero;
        }
    }

    private Vector3 CalculateSteering(Vector3 desired)
    {
        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _MaxSterring * Time.deltaTime);
        return steering;
    }

    private Vector3 DesiredVector(Vector3 target)
    {
        Vector3 desired = (target - transform.position).normalized * _MaxSpeed;
        return desired;
    }

    private Vector3 Seek(Vector3 target)
    {
        Vector3 desired = DesiredVector(target);
        return CalculateSteering(desired);
    }

    private Vector3 Flee(Vector3 target)
    {
        Vector3 desired = DesiredVector(target);
        return CalculateSteering(-desired);
    }

    private Vector3 Arrive(Vector3 target)
    {
        Vector3 dir = (target - transform.position);
        float velocity = _MaxSpeed;
        float distance = dir.magnitude;

        if (distance <= _SlowingDistance)
        {
            float percentDistance = distance / _SlowingDistance;
            velocity *= percentDistance;
        }

        Vector3 desired = dir.normalized * velocity;
        return CalculateSteering(desired);
    }
    private Vector3 Pursuit(Vector3 target)
    {
        Vector3 dir = (target - transform.position);
        float velocity = _MaxSpeed;
        float distance = dir.magnitude;

        if (distance <= _SlowingDistance)
        {
            float percentDistance = distance / _SlowingDistance;
            velocity *= percentDistance;
        }

        Vector3 desired = dir.normalized * velocity;
        return CalculateSteering(-desired);
    }

}








