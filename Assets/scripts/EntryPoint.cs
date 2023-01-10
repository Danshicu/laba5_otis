using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using UnityEditor.SceneManagement;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EntryPoint : MonoBehaviour
{
    private Vector2 _screenSize;
    private float _clickCount = 0;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Arrow arrowPref;
    [SerializeField] private Node nodepref;
    private bool _canvasActive = false;
    private GameObject currentObject;
    public SceneController controller;

    public NodeCanvas _nodeCanvas;
    public ArrowCanvas _arrowCanvas;
    private List<Arrow> _arrows;
    private List<Node> _nodes;
    public Vector3 firstClickPos;
    public Vector3 secondClickPos;
    [SerializeField] private float minDistance = 40f;
    public TextAsset SavedData;
    
    
    
    void Awake()
    {
        _nodes = new List<Node>();
        _arrows = new List<Arrow>();
        firstClickPos = new Vector3(0,0, 0);
        secondClickPos = new Vector3(0,0, 0);
    }

    private void Start()
    {
        _screenSize = new Vector2(Screen.width, Screen.height);
        Load();
    }

     public bool OnCanvas()
     {
         Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
         bool hittedCanvas = false;
         RaycastHit canvasHit;
         Physics.Raycast(mouseWorldPosition, Vector3.forward, out canvasHit, 202f);
         if (canvasHit.collider != null)
         {
             hittedCanvas = canvasHit.collider.gameObject.CompareTag("UI");
         }

         return hittedCanvas;
     }

    public IEnumerator WaitForClose(Vector3 mouseWorldPosition)
    {
        _canvasActive = true;
        yield return new WaitForSecondsRealtime(2);
        RaycastHit newhit;
        Physics.Raycast(mouseWorldPosition, Vector3.forward, out newhit, 202f);
        GameObject currentObject = newhit.collider.gameObject;

        while (OnCanvas())
        {
            yield return new WaitForSeconds(2);
        }
        
        _canvasActive = false;
        if (currentObject.CompareTag("UI"))
            {currentObject.SetActive(false);}

        yield return 0;
    }

    void to_spawn_place(ref Vector3 spawnPos, Vector3 mouseWorldPosition)
    {
        if (mouseWorldPosition.x > _screenSize.x / 2)
        {
            spawnPos.x -= 350f*0.22f;
        }
        else
        {
            spawnPos.x += 350f*0.22f;
        }
        if (mouseWorldPosition.y > _screenSize.y / 2)
        {
            spawnPos.y -= 145f*0.22f;
        }
        else
        {
            spawnPos.y += 145f*0.22f;
        }

        spawnPos.z = -190f;
    }

    void Update()
    {
        if (!controller.IsActive)
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                if (_canvasActive)
                {
                    currentObject.SetActive(false);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector3 spawnPos = mouseWorldPosition;

                RaycastHit hit;
                Physics.Raycast(spawnPos, Vector3.forward, out hit, 201f);

                Debug.DrawRay(spawnPos, Vector3.forward * hit.distance, Color.yellow, 10f);

                bool attacked = false;
                if (hit.collider.gameObject != null)
                {
                    GameObject attackedObj = hit.collider.gameObject;
                    if (attackedObj.CompareTag("Arrowtag"))
                    {
                        attacked = true;
                    }

                    if (attackedObj.CompareTag("Nodetag"))
                    {
                        attacked = true;
                    }

                    if (attackedObj.CompareTag("UI"))
                    {
                        attackedObj.SetActive(false);
                    }

                    Debug.Log(attackedObj.layer);

                    to_spawn_place(ref spawnPos, mouseWorldPosition);

                    if (attacked)
                    {
                        currentObject = hit.collider.gameObject;
                        if (currentObject.CompareTag("Nodetag"))
                        {
                            if (!_canvasActive)
                            {
                                var currentCanvas = Instantiate(_nodeCanvas, spawnPos, Quaternion.identity);
                                currentCanvas.SetNode(currentObject.GetComponentInParent<Node>());
                                currentCanvas.Setname();
                                StartCoroutine(WaitForClose(mouseWorldPosition));
                            }
                        }

                        if (currentObject.CompareTag("Arrowtag"))
                        {
                            if (!_canvasActive)
                            {
                                var currentCanvas = Instantiate(_arrowCanvas, spawnPos, Quaternion.identity);
                                currentCanvas.SetArrow(currentObject.GetComponent<Arrow>());
                                currentCanvas.SetVeigth();
                                StartCoroutine(WaitForClose(mouseWorldPosition));
                            }
                        }
                        attacked = false;
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!_canvasActive)
                {
                    Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    mouseWorldPosition.z = 0f;
                    _clickCount++;
                    if (_clickCount == 2)
                    {
                        secondClickPos = mouseWorldPosition;
                    }
                    else
                    {
                        firstClickPos = mouseWorldPosition;
                    }

                    if (_clickCount == 2)
                    {
                        float distance = Vector3.Distance(secondClickPos, firstClickPos);

                        if (distance < minDistance)
                        {
                            var node = Instantiate(nodepref, firstClickPos, Quaternion.identity);
                            _nodes.Add(node);
                        }

                        if (distance > minDistance)
                        {
                            Vector3 angleVector = new Vector3((secondClickPos.x - firstClickPos.x),
                                (secondClickPos.y - firstClickPos.y), 0f);
                            float length = Vector3.Distance(firstClickPos, secondClickPos);
                            float angle = Vector3.Angle(Vector3.right, angleVector);
                            if (angleVector.y < 0)
                            {
                                angle *= -1;
                            }

                            var arrow = Instantiate(arrowPref, firstClickPos, Quaternion.identity);
                            arrow.GenerateArrow(length, Vector3.zero);
                            arrow.transform.Rotate(Vector3.forward * angle, Space.Self);
                            _arrows.Add(arrow);
                        }

                        firstClickPos = secondClickPos = Vector3.zero;
                        _clickCount = 0;
                    }
                }
            }
        }
    }

    public void SaveAll(string FilePath)
    {
        int NodeCount = 0;
        int ArrowCount = 0;
        using (BinaryWriter writer = new BinaryWriter(File.Open(FilePath, FileMode.OpenOrCreate)))
        {
            foreach (var arrow in _arrows)
            {
                if (arrow.gameObject.activeInHierarchy)
                {
                    ArrowCount++;
                }
            }
            foreach (var node in _nodes)
            {
                if (node.gameObject.activeInHierarchy)
                {
                    NodeCount++;
                }
            }
        writer.Write(ArrowCount);
        foreach (var arrow in _arrows)
            {
                if (arrow.gameObject.activeInHierarchy)
                {
                    writer.Write(arrow.transform.position.x);
                    writer.Write(arrow.transform.position.y);
                    writer.Write(arrow.transform.rotation.eulerAngles.z);
                    writer.Write(arrow.stemLength);
                    writer.Write(arrow.tipLength);
                    writer.Write(arrow.stemWidth);
                    writer.Write(arrow.tipWidth);
                    writer.Write(arrow.value);
                }
            }
        writer.Write(NodeCount);
        foreach (var node in _nodes)
        {
            if (node.gameObject.activeInHierarchy)
            {
                writer.Write(node.transform.position.x);
                writer.Write(node.transform.position.y);
                writer.Write(node.name);
                writer.Write(node.radius);
            }
        }
        writer.Close();
        }
    }

    public void Load()
    {
        if (SavedData!=null)
        {
            string[] path = EditorSceneManager.GetActiveScene().path.Split(char.Parse("/"));
            path[path.Length - 1] = SavedData.name + ".txt";
            string currentpath = string.Join("/", path);
            using (BinaryReader reader = new BinaryReader(File.Open(currentpath, FileMode.Open)))
            {
                var countOfArrows = reader.ReadInt32();
                for (int i = 0; i < countOfArrows; i++)
                {
                    float arrowPosX = reader.ReadSingle();
                    float arrowPosY = reader.ReadSingle();
                    float arrowRotatZ = reader.ReadSingle();
                    float StemLength = reader.ReadSingle();
                    float TipLength = reader.ReadSingle();
                    float StemWidth = reader.ReadSingle();
                    float TipWidth = reader.ReadSingle();
                    float thisValue = reader.ReadSingle();
                    var arrow = Instantiate(arrowPref, new Vector3(arrowPosX, arrowPosY, 0f),
                        Quaternion.identity);
                    arrow.stemLength = StemLength;
                    arrow.tipLength = TipLength;
                    arrow.stemWidth = StemWidth;
                    arrow.tipWidth = TipWidth;
                    arrow.value = thisValue;
                    arrow.GenerateArrow(StemLength+TipLength, Vector3.zero);
                    arrow.transform.Rotate(Vector3.forward * arrowRotatZ, Space.Self);
                    Debug.Log(arrowRotatZ);
                    _arrows.Add(arrow);
                } 
                var countOfNodes = reader.ReadInt32();
                for (int i = 0; i < countOfNodes; i++)
                {
                    float posX = reader.ReadSingle();
                    float posY = reader.ReadSingle();
                    string name = reader.ReadString();
                    float radius = reader.ReadSingle();
                    var node = Instantiate(nodepref, new Vector3(posX, posY, 0f), Quaternion.identity);
                    node.name = name;
                    node.radius = radius;
                    node.transform.localScale = new Vector3(radius, radius, 0.1f);
                    _nodes.Add(node);
                    node.nameCanvasText._text.text = name;
                }
                reader.Close();
            }
        }
    }
}
