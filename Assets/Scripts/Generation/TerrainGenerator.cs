using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {
    [Range(3, 101)]
    public int size;
    [Range(0.1f, 20f)]
    public float maxheight;
    protected float[,] terrain;

    public void Init() {
        if (size % 2 == 0) {
            size++;
        }
        terrain = new float[size, size];
        terrain[0, 0] = Random.value;
        terrain[0, size - 1] = Random.value;
        terrain[size - 1, 0] = Random.value;
        terrain[size - 1, size - 1] = Random.value;
    }

    public void Build() {
        int step = size - 1;
        float height = maxheight;
        float r = Random.Range(0, height);
        //loop that traverses the whole grid
        for (int sideLength = size - 1; sideLength >= 2; sideLength /= 2) {
            //SQUARE step loop
            int half = size / 2;
            for (int y = 0; y < size - 1; y += sideLength) {
                for (int x = 0; x < size - 1; x += sideLength) {
                    //compute the values for the corners
                    float average = terrain[y, x];
                    average += terrain[x + sideLength, y];
                    average += terrain[x, y + sideLength];
                    average += terrain[x + sideLength, y + sideLength];
                    average /= 4f;
                    average += Random.value * 2f * height;
                    terrain[y + half, x + half] = average;
                }
            }

            //DIAMOND step loop
            for (int j = 0; j < size - 1; j = half) {
                for (int i = (j + half) % sideLength; i < size - 1; i += sideLength) {
                    //compute the values for the corners
                    float average = terrain[(j - half + size) % size, i];
                    average += terrain[(j + half) % size, i];
                    average += terrain[j, (i + half) % size];
                    average += terrain[j, (j - half + size) % size];
                    average = average + (Random.value * 2f * height) - height;
                    terrain[j, i] = average;
                    //wrap value of the edges for increased smoothness
                    if (i == 0) {
                        terrain[j, size - 1] = average;
                    }
                    if (j == 0) {
                        terrain[size - 1, i] = average;
                    }
                    height /= 2f;
                }
            }
        }
    }
}
