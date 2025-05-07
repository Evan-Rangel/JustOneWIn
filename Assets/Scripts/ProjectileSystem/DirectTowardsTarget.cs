using System;
using System.Collections.Generic;
using System.Linq;
using Avocado.ProjectileSystem.Components;
using Avocado.ProjectileSystem.DataPackages;
using Avocado.Utilities;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es un componente que se adjunta a un proyectil y le permite rotar suavemente 
hacia su objetivo más cercano, ideal para proyectiles dirigidos. 
La rotación:
-Se suaviza con Mathf.Lerp para crear una sensación de aceleración en la curva del proyectil.
-El objetivo se elige automáticamente entre una lista de transformaciones que el 
proyectil recibe en un TargetsDataPackage.
-La rotación se calcula usando una extensión Vector2ToRotation, probablemente una función 
auxiliar que convierte un vector en un Quaternion.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem
{
    public class DirectTowardsTarget : ProjectileComponent
    {
        [SerializeField] private float minStep;         // Velocidad de rotación mínima
        [SerializeField] private float maxStep;         // Velocidad de rotación máxima
        [SerializeField] private float timeToMaxStep;   // Tiempo que tarda en llegar a la velocidad máxima

        private List<Transform> targets;     // Lista de posibles objetivos
        private Transform currentTarget;     // Objetivo actual más cercano

        private float step;                  // Paso de rotación actual
        private float startTime;             // Tiempo de inicio para calcular el lerp

        private Vector2 direction;           // Dirección hacia el objetivo

        // Método llamado al inicializar el componente
        protected override void Init()
        {
            base.Init();

            currentTarget = null;
            startTime = Time.time;
            step = minStep;
        }

        // Se llama en cada FixedUpdate del proyectil
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!HasTarget())
                return;

            // Aumenta progresivamente la velocidad de rotación
            step = Mathf.Lerp(minStep, maxStep, (Time.time - startTime) / timeToMaxStep);

            // Calcula la dirección hacia el objetivo
            direction = (currentTarget.position - transform.position).normalized;

            // Aplica rotación hacia la dirección deseada
            Rotate(direction);
        }

        // Determina si hay un objetivo válido, y si no hay, lo busca
        private bool HasTarget()
        {
            if (currentTarget)
                return true;

            // Elimina objetivos nulos
            targets.RemoveAll(item => item == null);

            if (targets.Count <= 0)
                return false;

            // Ordena objetivos por cercanía y elige el más cercano
            targets = targets.OrderBy(target => (target.position - transform.position).sqrMagnitude).ToList();
            currentTarget = targets[0];

            return true;
        }

        // Rota el proyectil hacia la dirección indicada
        private void Rotate(Vector2 dir)
        {
            if (dir.Equals(Vector2.zero))
                return;

            var toRotation = QuaternionExtensions.Vector2ToRotation(dir);

            // Rota progresivamente hacia la nueva rotación
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, step * Time.deltaTime);
        }

        // Recibe un paquete de datos que contiene objetivos
        protected override void HandleReceiveDataPackage(ProjectileDataPackage dataPackage)
        {
            base.HandleReceiveDataPackage(dataPackage);

            if (dataPackage is not TargetsDataPackage targetsDataPackage)
                return;

            targets = targetsDataPackage.targets;
        }

        // Dibuja una línea hacia el objetivo en la escena para depuración
        private void OnDrawGizmos()
        {
            if (!currentTarget)
                return;

            Gizmos.DrawLine(transform.position, currentTarget.position);
        }
    }
}
