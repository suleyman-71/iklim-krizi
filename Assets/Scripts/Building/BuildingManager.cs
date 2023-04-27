using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public GameObject buildingPrefab;
    public GameObject preBuildingPrefab;
    public float buildTime = 15f;
    public GameObject buildProgress;
    public GameObject buildingProgressPrefab;
    public GameObject buildPopup;
    public GameObject buildingArea;
    public LayerMask buildLayerMask;

    private int build = 0;
    private float buildTimer = 0f;
    private Vector3 buildPosition;

    void Update()
    {
        if (build == 1)
        {
            Animator animator = preBuildingPrefab.GetComponent<Animator>();
            animator.Play("PreBuilding");
            buildingProgressPrefab.SetActive(true);
            buildTimer += Time.deltaTime;
            LeanTween.scaleX(buildProgress, 1, buildTime);

            if (buildTimer >= buildTime)
            {
                
                for (int i = 0; i < build; i++)
                {
                    GameObject building = Instantiate(buildingPrefab, buildPosition, Quaternion.identity);
                    building.GetComponent<BoxCollider>().enabled = true;
                    building = null;
                }
                //GameObject building = Instantiate(buildingPrefab, buildPosition, Quaternion.identity);
                //building.GetComponent<BoxCollider>().enabled = true;

                
                build += 1;
                buildTimer = 0f;
                buildingProgressPrefab.SetActive( false);
                preBuildingPrefab.SetActive(false);
            }
        }
        else if (Input.GetMouseButtonDown(0) && build < 1)
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildLayerMask))
            {
                buildPosition = buildingArea.transform.position;
                buildPopup.SetActive(true);
                buildTimer = 0f;

            }
        }
    }

    public void Build()
    {
        build = 1;
        buildPopup.SetActive(false);
        buildingArea.SetActive(false);
        preBuildingPrefab.SetActive(true);
    }
}


