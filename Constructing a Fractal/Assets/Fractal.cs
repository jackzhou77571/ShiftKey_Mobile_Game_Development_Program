﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {
    public float maxTwist;

    public float maxRotationSpeed;
    
    public float spawnProbability;
    public Mesh[] meshes;
    public int maxDepth;
    public float childScale;

    private float rotationSpeed;
    private int depth;
    private Material[,] materials;
    public Mesh mesh;
    public Material material;
	// Use this for initialization
	void Start () {
        rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);
        if (materials==null)
        {
            InitializeMaterials();
        }


        gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
        gameObject.AddComponent<MeshRenderer>().material = materials[depth,Random.Range(0,2)];
        //GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.white, Color.yellow, (float)depth / maxDepth);

        if (depth<maxDepth)
        {
            StartCoroutine(CreateChildren());
            //new GameObject("Fractral Child").AddComponent<Fractal>().Initialiaze(this,Vector3.up);
           // new GameObject("Fractal Child").AddComponent<Fractal>().Initialiaze(this, Vector3.right);
        }
       
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0f,rotationSpeed * Time.deltaTime, 0f);
	}

    private void InitializeMaterials()
    {
        materials = new Material[maxDepth + 1,2];
        for (int i = 0; i <= maxDepth; i++)
        {
            float t = i / (maxDepth - 1f);
            t *= t;

            materials[i,0] = new Material(material);
            materials[i,0].color = Color.Lerp(Color.white, Color.yellow, t);
            materials[i, 1] = new Material(material);
            materials[i, 1].color = Color.Lerp(Color.white, Color.cyan, t);
        }
        materials[maxDepth,0].color = Color.magenta;
        materials[maxDepth, 1].color = Color.red;
    }


    private static Vector3[] childDirections =
    {
        Vector3.up,Vector3.right,Vector3.left,Vector3.forward,Vector3.back
    };

    private static Quaternion[] childOrientations =
    {
        Quaternion.identity,
        Quaternion.Euler(0f,0f,-90f),
        Quaternion.Euler(0f,0f,90f),
        Quaternion.Euler(90f,0f,0f),
        Quaternion.Euler(-90f,0f,0f)
    };

    private IEnumerator CreateChildren()
    {
        for (int i = 0; i < childDirections.Length; i++)
        {
            if (Random.value<spawnProbability)
            {
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
                new GameObject("Fractal Child").
                    AddComponent<Fractal>().Initialiaze(this, i);
            }
           
        }
        /*
        yield return new WaitForSeconds(0.5f);
        new GameObject("Fractal Child").
            AddComponent<Fractal>().Initialiaze(this, Vector3.up,Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        new GameObject("Fractal Child").
            AddComponent<Fractal>().Initialiaze(this, Vector3.right,Quaternion.Euler(0f,0f,-90f));
        yield return new WaitForSeconds(0.5f);
        new GameObject("Fractal Child").
            AddComponent<Fractal>().Initialiaze(this, Vector3.left,Quaternion.Euler(0f,0f,90f));
        */

    }
    private void Initialiaze(Fractal parent,int childIndex)
    {
        meshes = parent.meshes;
        materials = parent.materials;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one * childScale;

        maxTwist = parent.maxTwist;
        maxRotationSpeed = parent.maxRotationSpeed;
        spawnProbability = parent.spawnProbability;
        //transform.localPosition = Vector3.up * (0.5f + 0.5f * childScale);
        transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
        transform.localRotation = childOrientations[childIndex];

    }
}
