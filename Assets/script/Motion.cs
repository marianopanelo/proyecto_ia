using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Motion : MonoBehaviour
{
    [SerializeField] private GameObject _GameObject;
    [SerializeField] private float _MaxSpeed = 5f;
    [SerializeField] private float _MaxSterring = 5f;
    [SerializeField] private float _SlowingDistance = 3f;
    [SerializeField] private float _MinDistance = 0.1f;
  
    //Flocking
    private Motion targetAgent;
    private static List<Motion> allAgents = new List<Motion>();
    [SerializeField] private float floatmaximumDetectionRange = 4f;
    [SerializeField] private float minimumSeparationDistance = 1.0f;
    [SerializeField] protected float alignment = 2f;
    //dar importancia 
    [SerializeField, Range(0f,3f)] private float SeparationWeight = 1f;
    [SerializeField, Range(0f,3f)] private float AlignmentWeight = 1f;
    [SerializeField, Range(0f,3f)] private float CohesionWeight = 1f;

    private Vector3 _TargetPosition;
    private Vector3 _velocity;

    public enum SteeringModes { Seek, Flee, Arrive, Pursuit, Evade, Flocking }
    public SteeringModes currentSterring;

    private void Awake()
    {
        Vector3 randomDireccion = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        _velocity += randomDireccion.normalized * _MaxSpeed;

        allAgents.Add(this);
    }

    private void Update()
    {
        _velocity.y = 0f; 

        _velocity += sterringVector();// si lo descomento , los casazos se van a cualquier lado pero el casador anda con las demas funciones
        transform.position += _velocity * Time.deltaTime;

        if (_velocity != Vector3.zero)
        {
            transform.forward = _velocity;
        }
        transform.position = Map.instance.OutOfMap(transform.position);
    }
    private void OnDestroy()
    {
        allAgents.Remove(this);
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
            case SteeringModes.Evade:
                 return Evade(_GameObject.transform.position);
            case SteeringModes.Flocking:
                return floking();
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
    private Vector3 Evade(Vector3 target)
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

    //floking como el profe lo hizo

    private Vector3 floking()
    {
        Vector3 desiredSeparation = CalculateSeparation();
        Vector3 desiredAlignment = CalculateAlignament();
        Vector3 desoredCohesion = CalculateCohesion();

        Vector3 desired = desiredSeparation * SeparationWeight + desiredAlignment * AlignmentWeight + desoredCohesion * CohesionWeight;

        if (desired == Vector3.zero)
            desired = _velocity;

        return CalculateSteering(desired); 
    }

    private Vector3 CalculateSeparation()
    {
        Vector3 dessired = default;
        int count = 0;
        foreach (var item in allAgents)
        {
            if (item == this) continue;
            if (Vector3.Distance(item.transform.position, transform.position) < minimumSeparationDistance)
            {
                dessired += (transform.position - item.transform.position).normalized;
                count++;
            }
        }
        if (count > 0)
            return (dessired / count).normalized * _MaxSpeed; 
        return Vector3.zero;
    }

    private Vector3 CalculateAlignament()
    {
        Vector3 dessired = default;
        int count = 0;
        foreach (var item in allAgents)
        {
            if (item == this) continue;
            if (Vector3.Distance(item.transform.position, transform.position) < floatmaximumDetectionRange)
            {
                dessired += item._velocity;
                count++;
            }
        }
        if (count > 0)
            return (dessired / count).normalized * _MaxSpeed; 
        return Vector3.zero;
    }

    private Vector3 CalculateCohesion()
    {
        Vector3 dessired = default;
        int count = 0;
        foreach (var item in allAgents)
        {
            if (item == this) continue;
            if (Vector3.Distance(item.transform.position, transform.position) < alignment) 
            {
                dessired += item.transform.position;
                count++;
            }
        }
        if (count > 0)
            return Seek(dessired);
        return Vector3.zero;
    }


    /*
    private Vector3 Flocking()
    {
        GameObject[] allAgents = GameObject.FindGameObjectsWithTag("Caught");

        Vector3 separation = Vector3.zero;
        Vector3 cohesion = Vector3.zero;
        Vector3 alignment = Vector3.zero;

        int neighborCount = 0;

        foreach (GameObject agent in allAgents)
        {
            if (agent == this.gameObject) continue; // no me comparo conmigo mismo

            float distance = Vector3.Distance(transform.position, agent.transform.position);

            if (distance < floatmaximumDetectionRange)
            {
                neighborCount++;

                // Separación: alejarse si está demasiado cerca
                if (distance < minimumSeparationDistance)
                {
                    separation += (transform.position - agent.transform.position).normalized / distance;
                }

                // Cohesión: acercarse al centro de los vecinos
                cohesion += agent.transform.position;

                // Alignment: alinearse con la dirección de los vecinos
                Motion otherMotion = agent.GetComponent<Motion>();
                if (otherMotion != null)
                {
                    alignment += otherMotion._velocity;
                }
            }
        }

        if (neighborCount > 0)
        {
            cohesion = ((cohesion / neighborCount) - transform.position).normalized;
            alignment = (alignment / neighborCount).normalized;
        }

        // Ajustá los pesos según el comportamiento que quieras
        Vector3 finalForce = (separation * 2f) + (cohesion * 1f) + (alignment * 1f);

        return Vector3.ClampMagnitude(finalForce, _MaxSteering);
    }
    */
}










