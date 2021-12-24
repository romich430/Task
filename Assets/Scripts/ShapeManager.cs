using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Task{
public class ShapeManager : MonoBehaviour
{
    List<Vector3> points = new List<Vector3>(); //Points got from mousePosition while drawing

    public LineRenderer pencil;
    private bool IsDrawing = false;

    [SerializeField]private float width; //Width of drawn shape
    [SerializeField]private float magnitude; //Difference between mousePosition needed to add new point
    private Vector3 top;

    [SerializeField]private MeshFilter LeftLeg;
    [SerializeField]private MeshFilter RightLeg;

    private MeshCollider leftcoll;
    private MeshCollider rightcoll;

    void Start()
    {
        pencil = GameObject.Find("Pencil").GetComponent<LineRenderer>();
    }

    //Called when mouse exits the easel
    public void PointerExit(){
        Debug.Log("Exited");
        IsDrawing = false;
        pencil.positionCount = 0;
    }

    //Called from easel when started drawing
    public void DrawingShape(){
        Time.timeScale = 0.2f;
        Touch touch = Input.GetTouch(0);
        IsDrawing = true;
        points = new List<Vector3>();
        pencil.positionCount = 0;

        //Starting to draw/count points with first point
        points.Add(Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 1f)));
        top = points[0];
        pencil.positionCount++;
        pencil.SetPosition(0, points.Last());

        //Getting other points
        StartCoroutine(GetPoints());
    }

    //Coroutine to get points over delay
    IEnumerator GetPoints(){
        while (Input.touchCount > 0 && IsDrawing){
            Touch touch = Input.GetTouch(0);
            Vector3 current = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 1f));

            //Checking for difference between two last points, if enough, adding new point
            //Decided to repeat three times to increase amount of points (better quality)
            if (new Vector3(current.x - points.Last().x, current.y - points.Last().y, 0.0f).magnitude > magnitude){
                pencil.positionCount++;
                points.Add(current);
                pencil.SetPosition(points.IndexOf(points.Last()), points.Last());
                points[points.IndexOf(points.Last())] = new Vector3(points.Last().x, points.Last().y, 0f);
                Debug.Log("magnitude is over 0.01f");
            }

            if (current.y > top.y){
                top = current;
            }

            touch = Input.GetTouch(0);
            current = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 1f));

            if (new Vector3(current.x - points.Last().x, current.y - points.Last().y, 0.0f).magnitude > magnitude){
                pencil.positionCount++;
                points.Add(current);
                pencil.SetPosition(points.IndexOf(points.Last()), points.Last());
                points[points.IndexOf(points.Last())] = new Vector3(points.Last().x, points.Last().y, 0f);
                Debug.Log("magnitude is over 0.01f");
            }

            if (current.y > top.y){
                top = current;
            }

            touch = Input.GetTouch(0);
            current = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 1f));

            if (new Vector3(current.x - points.Last().x, current.y - points.Last().y, 0.0f).magnitude > magnitude){
                pencil.positionCount++;
                points.Add(current);
                pencil.SetPosition(points.IndexOf(points.Last()), points.Last());
                points[points.IndexOf(points.Last())] = new Vector3(points.Last().x, points.Last().y, 0f);
                Debug.Log("magnitude is over 0.01f");
            }

            if (current.y > top.y){
                top = current;
            }
            yield return new WaitForSeconds(0f);  
        }

        //Setting highest point as (0, 0, 0) and moving all others accordingly
        for(int k = 0; k < points.Count; k++){
            points[k] = points[k] - top;
        }

        Mesh mesh = CreateMeshFromPoints(points);

        Destroy(leftcoll);
        Destroy(rightcoll);
        LeftLeg.mesh = mesh;
        leftcoll = LeftLeg.gameObject.AddComponent<MeshCollider>();
        leftcoll.convex = true;
        RightLeg.mesh = mesh;
        rightcoll = RightLeg.gameObject.AddComponent<MeshCollider>();
        rightcoll.convex = true;
        pencil.positionCount = 0;
        Time.timeScale = 1.0f;
    }

    //Taking points from drawing and assigning a cube to each one, then creating mesh from those qubes 
    public Mesh CreateMeshFromPoints(List<Vector3> centers){
        List<Cube> cubes = new List<Cube>();
        List<int> triangles = new List<int>();
        List<Vector3> points = new List<Vector3>();

        foreach(Vector3 center in centers){
            cubes.Add(new Cube(center, width));

            foreach(Vector3 vertex in cubes.Last().vertices){
                points.Add(vertex);
            }
        }

        //Creating trinagles from points in each qube
        foreach(Cube cube in cubes){
            
            triangles.AddRange(new List<int>
                {
                    cubes.IndexOf(cube) * 8 + 0, cubes.IndexOf(cube) * 8 + 4, cubes.IndexOf(cube) * 8 + 3, cubes.IndexOf(cube) * 8 + 4, cubes.IndexOf(cube) * 8 + 7, cubes.IndexOf(cube) * 8 + 3,
                    cubes.IndexOf(cube) * 8 + 2, cubes.IndexOf(cube) * 8 + 6, cubes.IndexOf(cube) * 8 + 1, cubes.IndexOf(cube) * 8 + 6, cubes.IndexOf(cube) * 8 + 5, cubes.IndexOf(cube) * 8 + 1,
                    cubes.IndexOf(cube) * 8 + 3, cubes.IndexOf(cube) * 8 + 7, cubes.IndexOf(cube) * 8 + 2, cubes.IndexOf(cube) * 8 + 7, cubes.IndexOf(cube) * 8 + 6, cubes.IndexOf(cube) * 8 + 2,
                    cubes.IndexOf(cube) * 8 + 1, cubes.IndexOf(cube) * 8 + 5, cubes.IndexOf(cube) * 8 + 0, cubes.IndexOf(cube) * 8 + 5, cubes.IndexOf(cube) * 8 + 4, cubes.IndexOf(cube) * 8 + 0,
                    cubes.IndexOf(cube) * 8 + 4, cubes.IndexOf(cube) * 8 + 5, cubes.IndexOf(cube) * 8 + 7, cubes.IndexOf(cube) * 8 + 5, cubes.IndexOf(cube) * 8 + 6, cubes.IndexOf(cube) * 8 + 7,
                    cubes.IndexOf(cube) * 8 + 3, cubes.IndexOf(cube) * 8 + 2, cubes.IndexOf(cube) * 8 + 0, cubes.IndexOf(cube) * 8 + 2, cubes.IndexOf(cube) * 8 + 1, cubes.IndexOf(cube) * 8 + 0,
                }
            );
        }

        //Assigning all points and triangles to new mesh
        Mesh mesh = new Mesh();
        mesh.vertices = points.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        return mesh;
    }
}
}