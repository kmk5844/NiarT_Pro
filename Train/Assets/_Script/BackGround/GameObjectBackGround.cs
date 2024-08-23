using UnityEngine;
public class GameObjectBackground : MonoBehaviour
{
    public GameDirector gamedirector;
    public GameObject[] W_Object; // 오브젝트
    public GameObject[] B_Object; // 블러 오브젝트

    float spawnRate; // 오브젝트 생성 주기

    [Header("W_Object")]
    public float W_maxX;
    public float W_maxY;
    public float W_minX;
    public float W_minY;

    [Header("B_Object")]
    public float B_maxX;
    public float B_maxY;
    public float B_minX;
    public float B_minY;

    private float timer;
    float Train_Speed;
    public float Force;

    private void Start()
    {
        spawnRate = Random.Range(2f, 6f);
        B_minX = (-10.94f * gamedirector.SA_TrainData.Train_Num.Count) - 5f;
        W_minX = (-10.94f * gamedirector.SA_TrainData.Train_Num.Count) - 5f;
    }

    private void Update()
    {
        Train_Speed = gamedirector.TrainSpeed;
        // 타이머 증가
        timer += Time.deltaTime;

        // 스폰 주기에 도달하면 새 오브젝트 생성
        if (timer >= spawnRate)
        {
            timer = 0f;
            SpawnObject();
            spawnRate = Random.Range(2f, 6f);
        }
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f)
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<ObjectMover>().Flag)
                {
                    child.GetComponent<ObjectMover>().Flag = false;
                }
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                if (!child.GetComponent<ObjectMover>().Flag)
                {
                    child.GetComponent<Rigidbody2D>().AddForce(Vector2.left * child.GetComponent<ObjectMover>().force, ForceMode2D.Impulse);
                    child.GetComponent<ObjectMover>().Flag = true;
                }

                // 화면 밖으로 나가면 파괴
                if (child.position.x < W_minX)
                {
                    Destroy(child.gameObject);
                }
            }
        }
        // 모든 자식 오브젝트 이동
    }

    private void SpawnObject()
    {
        Vector3 position;
        GameObject newObject;
        // 랜덤 위치에 새 오브젝트 생성
        if (Random.Range(0,2) == 0)
        {
            position = new Vector3(W_maxX, Random.Range(W_minY, W_maxY), 186f);
            newObject = Instantiate(W_Object[Random.Range(0, W_Object.Length)], position, Quaternion.identity, transform);
        }
        else
        {
            position = new Vector3(B_maxX, Random.Range(B_minY, B_maxY), 0f);
            newObject = Instantiate(B_Object[Random.Range(0, B_Object.Length)], position, Quaternion.identity, transform);
        }

        Force = Train_Speed * 0.05f;
        float Scale = Random.Range(0.4f, 0.8f);
        newObject.transform.localScale = new Vector3(Scale, Scale, Scale);
        newObject.GetComponent<ObjectMover>().force = Force;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(new Vector3(W_maxX, W_maxY, 0), new Vector3(W_maxX, W_minY, 0));
        Gizmos.DrawLine(new Vector3(W_maxX, W_minY, 0), new Vector3(W_minX, W_minY, 0));
        Gizmos.DrawLine(new Vector3(W_minX, W_minY, 0), new Vector3(W_minX, W_maxY, 0));
        Gizmos.DrawLine(new Vector3(W_minX, W_maxY, 0), new Vector3(W_maxX, W_maxY, 0));

        Gizmos.color = Color.black;
        Gizmos.DrawLine(new Vector3(B_maxX, B_maxY, 0), new Vector3(B_maxX, B_minY, 0));
        Gizmos.DrawLine(new Vector3(B_maxX, B_minY, 0), new Vector3(B_minX, B_minY, 0));
        Gizmos.DrawLine(new Vector3(B_minX, B_minY, 0), new Vector3(B_minX, B_maxY, 0));
        Gizmos.DrawLine(new Vector3(B_minX, B_maxY, 0), new Vector3(B_maxX, B_maxY, 0));
    }
}

