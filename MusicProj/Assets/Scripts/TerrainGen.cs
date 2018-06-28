using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EthansProject
{
    [System.Serializable]
    public class RegionData
    {
        public string regionName;
        public Color regionColor;
        public Vector2 betweenHeight;
        public bool traverableRegion;

        [System.Serializable]
        public class ObjectData
        {
            public GameObject objToSpawnForRegion;
            public bool useNoise;
            public float noiseCap = 0.4f;
            public float spawnPossiblity = 3;
            public int spawnCap = 99999;
            [HideInInspector]
            public int spawnedAMT;
        }

        public ObjectData[] objectsToSpawn;

    }

    [RequireComponent(typeof(Terrain))]
    public class TerrainGen : MonoBehaviour
    {
        Terrain terrainComp;
        MeshFilter mfComp;

        public float XScale = 1f;
        public float ZScale = 1f;
        public float NoiseScale = 0.1f;
        public int NumPasses = 4;
        public float baseHeight;
        public RegionData[] regions;
        List<GameObject> genedObjs = new List<GameObject>();
        public float passStrengthScale;
        public float passStrength;
        public float passNoiseScalse;

        public float R_Float
        {
            get { return UnityEngine.Random.Range(0.6f, 1.3f); }
        }

        public float min = 0.9f;
        public float max = 1.1f;


        // Use this for initialization
        void Start()
        {
            GenerateTerrain();
        }

        public void GenerateTerrain()
        {
            // Runs through the gernerated objects and deletes them and clears the list to that objects like trees are still there when world is regened
            for (int i = 0; i < genedObjs.Count; i++)
            {
                Destroy(genedObjs[i]);

              
            }

            genedObjs.Clear();

            if (genedObjs.Count > 0)
            {
                GenerateTerrain();
                return;
            }

            // retrieve the terrain
            terrainComp = GetComponent<Terrain>();
            mfComp = GetComponent<MeshFilter>();
            // grab the terrain data

            TerrainData terrainData = terrainComp.terrainData;
            //NodeManager.instance.newData = terrainData;

            // retrieve the height map
            float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);

            //Resets the height map
            for (int x = 0; x < terrainData.heightmapWidth; ++x)
            {
                for (int z = 0; z < terrainData.heightmapHeight; ++z)
                {
                    heightMap[x, z] = baseHeight;
                }
            }

            float currentStrength = passStrength * R_Float;
            float xzScaleMultiplyer = R_Float;
            float newxScale = XScale * xzScaleMultiplyer;
            float newzScale = ZScale * xzScaleMultiplyer;
            float newNoiseScale = NoiseScale * R_Float;
            // run each individual pass
            for (int pass = 0; pass < NumPasses; ++pass)
            {
                //Generate noise
                for (int x = 0; x < terrainData.heightmapWidth; ++x)
                {
                    for (int z = 0; z < terrainData.heightmapHeight; ++z)
                    {
                        heightMap[x, z] += currentStrength * 2f * newNoiseScale * (Mathf.PerlinNoise(newxScale * x / terrainData.heightmapWidth,
                                                                                newzScale * z / terrainData.heightmapHeight) - 0.5f) * UnityEngine.Random.Range(min, max);

                    }
                }
                // 
                currentStrength *= passStrengthScale * R_Float;
                newxScale *= passStrengthScale * xzScaleMultiplyer;
                newzScale *= passStrengthScale * xzScaleMultiplyer;
                newNoiseScale *= passNoiseScalse * R_Float;
            }

            terrainData.SetHeights(0, 0, heightMap);
            terrainComp.enabled = false;
            int numVerts = Mathf.RoundToInt(terrainData.heightmapWidth * terrainData.heightmapHeight);
            Vector3[] vertices = new Vector3[numVerts];
            int[] triangles = new int[numVerts * 3 * 2]; // 3 verts per triangle, 2 triangles per quad/square
            Color[] colorMap = new Color[vertices.Length];
            // build up the vertices and triangles
            int vertIndex = 0;

            //Generate the mesh vertices and objects
            for (int x = 0; x < terrainData.heightmapWidth; x++)
            {
                for (int z = 0; z < terrainData.heightmapHeight; z++)
                {
                    float height = terrainData.GetHeight(x, z);
                    vertices[vertIndex] = new Vector3(x, height, z);

                  //  vertices[vertIndex].y *= UnityEngine.Random.Range(0.8f, 1.05f);

                    //Check the vert for its colour
                    colorMap[vertIndex] = CheckForColorRegion(height);
                    // place a node.
                   // NodeManager.instance.CreateNode(vertices[vertIndex], terrainData, x, z, CheckForTraversableRegion(height));

                    //if inbetween height spawn a object
                    //Hack: make better object gen system.

                    SpawnObject(vertices[vertIndex], x, z, height);
                    
                    //Builds the vertices
                    if ((x < (terrainData.heightmapWidth - 1)) && (z < (terrainData.heightmapHeight - 1)))
                    {
                        triangles[(vertIndex * 6) + 0] = vertIndex + 1;
                        triangles[(vertIndex * 6) + 1] = vertIndex + terrainData.heightmapWidth + 1;
                        triangles[(vertIndex * 6) + 2] = vertIndex + terrainData.heightmapWidth;
                        triangles[(vertIndex * 6) + 3] = vertIndex + terrainData.heightmapWidth;
                        triangles[(vertIndex * 6) + 4] = vertIndex;
                        triangles[(vertIndex * 6) + 5] = vertIndex + 1;
                    }
                    ++vertIndex;
                }
            }

            mfComp.mesh.vertices = vertices;
            mfComp.mesh.triangles = triangles;

            mfComp.mesh.colors = colorMap;

            mfComp.mesh.RecalculateBounds();
            mfComp.mesh.RecalculateNormals();
            //NodeManager.instance.Initialize();

        }

        void SpawnObject(Vector3 vertPoint, int x, int z, float currentHeight)
        {
            float noise = Mathf.PerlinNoise(x / (float)terrainComp.terrainData.heightmapWidth, z / (float)terrainComp.terrainData.heightmapHeight) * R_Float / 2;
            noise *= R_Float;
            Mathf.Clamp01(noise);
            //Looping through regions
            for (int i = 0; i < regions.Length; i++)
            {
                //checking if were at the region
                if (isBetween(regions[i].betweenHeight, currentHeight))
                {
                    //Creating a noise
                    //print(noise);
                    //Looping through the regions objects to spawn
                    for (int xIndex = 0; xIndex < regions[i].objectsToSpawn.Length; xIndex++)
                    {
                        //making sure the object was assigned
                        if (regions[i].objectsToSpawn[xIndex].objToSpawnForRegion == null)
                            continue;

                        // checking if the user wants to use noise to spawn the object
                        if (regions[i].objectsToSpawn[xIndex].useNoise)
                        {
                            //... if so - go to the next object to spawn
                            if (noise > regions[i].objectsToSpawn[xIndex].noiseCap)
                                continue;

                        }

                        // Runs a random roll to see if it can generate a object
                        float objProbabillity = UnityEngine.Random.Range(0.1f, 500.0f);

                        // if the possiblity is lower than a certain amount - spawn the object                   
                        if (regions[i].objectsToSpawn[xIndex].spawnPossiblity > objProbabillity && regions[i].objectsToSpawn[xIndex].spawnedAMT < regions[i].objectsToSpawn[xIndex].spawnCap)
                        {
                            Vector3 newRot = new Vector3(0, UnityEngine.Random.Range(0, 360), 0);
          
                            GameObject newObj = Instantiate(regions[i].objectsToSpawn[xIndex].objToSpawnForRegion, vertPoint, Quaternion.Euler(newRot));
                            regions[i].objectsToSpawn[xIndex].spawnedAMT++;
                            newObj.transform.localScale *= R_Float;
                            genedObjs.Add(newObj);
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// checks the passed through height for its assigned colour in the regions
        /// </summary>
        /// <param name="currentHeight"></param>
        /// <returns></returns>
        private Color CheckForColorRegion(float currentHeight)
        {
            //NodeManager.instance.StatusUpdate("Now checking for region and coloring accordingly");

            for (int i = 0; i < regions.Length; i++)
            {

                if (isBetween(regions[i].betweenHeight, currentHeight))
                {
                    return regions[i].regionColor;
                }
            }
            Debug.Log(currentHeight);

            return Color.red;
        }

        /// <summary>
        /// Checks of the current hieght is a traversable region
        /// </summary>
        /// <param name="currentHeight"></param>
        /// <returns></returns>
        bool CheckForTraversableRegion(float currentHeight)
        {
            for (int i = 0; i < regions.Length; i++)
            {
                if (isBetween(regions[i].betweenHeight, currentHeight))
                    return regions[i].traverableRegion;
            }

            return true;
        }

        static bool isBetween (Vector2 betweenValues, float input)
        {
            if (input > betweenValues.x && input < betweenValues.y)
                return true;
        
            else return false;
        }
    }
}