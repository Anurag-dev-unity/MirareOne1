using Lean.Touch;
using UnityEngine;
using System.Collections.Generic;


public class InteractableObject : MonoBehaviour, IPooledObject
{
    public bool isSelected;
    public bool isPlaced;
    //public BoxCollider boxCollider;
    private LeanScale leanScale;
    private Color original;

    Collider m_Collider;
    //Vector3 m_Center;
    public Vector3 m_collSize; //m_Min, m_Max;
    private Renderer rend;
    List<Material> materials = new List<Material>();
    List<Material> tempMaterials = new List<Material>();
    public List<Texture> originalTextures = new List<Texture>();
    public List<Texture> grayScaleTextures = new List<Texture>();
    private bool resetTextureToColor = false;
    private string myTag = string.Empty;


    public float colliderSize = 0.0f;

    private bool selectionMarkerSet;

    private Material[] ghostMaterials;
    private Material[] originalMaterials;
    private Material[] errorMaterials;
    private Material[] lastMaterials; // last materials before error material 

    private bool wasPlaced;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    private void Awake()
    {


    }

    /// <summary>
    /// Start this instance.
    /// </summary>
    private void Start()
    {
        //Fetch the Collider from the GameObject
        m_Collider = GetComponent<MeshCollider>();
        //Fetch the size of the Collider volume
       m_collSize = m_Collider.bounds.size;


        if (m_collSize.x >= m_collSize.z)
            colliderSize = m_collSize.x;

        else
            colliderSize = m_collSize.z;


        //Output this data into the console
        //OutputData();


        //---------------------------------------------------
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;

        leanScale = GetComponent<LeanScale>();
        original = this.gameObject.GetComponent<Renderer>().materials[0].color;
        //---------------------------------------------------


        rend = GetComponent<Renderer>();
        //rend.GetMaterials(materials);
        Debug.Log("RARO Hello from START");



        //-----------
        transform.GetChild(0).localScale = new Vector3(transform.GetChild(0).localScale.x *
                                                       colliderSize *
                                                       transform.localScale.x,
                                                       transform.GetChild(0).localScale.y,
                                                       transform.GetChild(0).localScale.z *
                                                       colliderSize *
                                                       transform.localScale.z);

        //-----------
        Debug.Log("RARO scale set 1");
        leanScale.ScaleClamp = true;
        leanScale.ScaleMax = Vector3.one * 1.5f;
        leanScale.ScaleMin = Vector3.one * 0.05f;
        //leanScale.ScaleMin = Vector3.one * 0.05f;
        Debug.Log("RARO scale set clamp");

        //------------------
        ghostMaterials = new Material[rend.materials.Length];
        originalMaterials = new Material[rend.materials.Length];
        errorMaterials = new Material[rend.materials.Length];
        for (int i = 0; i < rend.materials.Length; i++)
        {
            originalMaterials[i] = rend.materials[i];
            ghostMaterials[i] = AppManager.Instance.ghostMaterial;
            errorMaterials[i] = AppManager.Instance.errorMaterial;
        }

        //default state
        rend.materials = ghostMaterials;

        ///-----------------
    }

    /// <summary>
    /// Ons the enable.
    /// </summary>
    private void OnEnable()
    {
        //Fetch the Collider from the GameObject
        m_Collider = GetComponent<MeshCollider>();
        //Fetch the size of the Collider volume
        m_collSize = m_Collider.bounds.size;


        if (m_collSize.x >= m_collSize.z)
            colliderSize = m_collSize.x;

        else
            colliderSize = m_collSize.z;


        //Output this data into the console
        //OutputData();


        //leanScale.ScaleClamp = true;
        //leanScale.ScaleMax = Vector3.one;
        //leanScale.ScaleMin = Vector3.one * 0.2f;
    }

    /// <summary>
    /// Outputs the data.
    /// </summary>
    void OutputData()
    {
        //Output to the console the center and size of the Collider volume
        Debug.Log("RARO Collider Size : " + m_collSize);
        Debug.Log("RARO multiply  ; " + colliderSize);
    }

        /// <summary>
        /// Init the specified _tag.
        /// </summary>
        /// <param name="_tag">Tag.</param>
        public void Init(string _tag)
        {
            print("RARO Init object");

        Debug.Log("RARO Materials - " + materials.Count);
        Debug.Log("RARO Grayscale - " + grayScaleTextures.Count);

        //Debug.Log("RARO name - " + this.gameObject.name);
        //if (!this.gameObject.name.Contains("car"))
        //{
        //    Debug.Log("ye chair NAHI hai");
        //}

        Debug.Log("RARO - 2.1 "  + _tag);
            transform.localRotation = Quaternion.identity;
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            Debug.Log("RARO - 2.2");

            //if (!gameObject.name.Contains("car"))
            //{

            resetTextureToColor = false;

                if (grayScaleTextures.Count <= 0 && originalTextures.Count <= 0)
                {
                Debug.Log("RARO - 2.3");
                if (ObjectPooler.Instance.textureDictionary.ContainsKey(_tag))
                    {
                        Debug.Log("originalTextures-> " + ObjectPooler.Instance.textureDictionary[_tag].originalTextures.Count);
                        Debug.Log("grayScaleTextures-> " + ObjectPooler.Instance.textureDictionary[_tag].grayScaleTextures.Count);
                        grayScaleTextures = ObjectPooler.Instance.textureDictionary[_tag].grayScaleTextures;
                        originalTextures = ObjectPooler.Instance.textureDictionary[_tag].originalTextures;
                    Debug.Log("RARO - 2.4");
                    }
                    for (int i = 0; i < grayScaleTextures.Count; i++)
                    {
                        materials[i].mainTexture = grayScaleTextures[i];
                    Debug.Log("RARO - 2.5");
                    }
                    Debug.Log("RARO set grayScale texture");
                }
                else
                {
            Debug.Log("RARO - 2.6");

            //this.rend.GetMaterials(tempMaterials);
            //Debug.Log("RARO temp mat - " + tempMaterials.Count);

            for (int i = 0; i < grayScaleTextures.Count; i++)
                    {
                Debug.Log("RARO - " + tempMaterials[i]);
                materials[i].mainTexture = grayScaleTextures[i];
                //tempMaterials[i].mainTexture = grayScaleTextures[i];
                Debug.Log("RARO - 2.7");
                    }
                }
        //}

        Debug.Log("RARO - 2.8");
        leanScale = GetComponent<LeanScale>();
            original = this.gameObject.GetComponent<Renderer>().materials[0].color;
            Debug.Log("RARO - 2.9");
        }


    /// <summary>
    /// Graies the scale texture.
    /// </summary>
    /// <returns>The scale texture.</returns>
    public List<Texture> GrayScaleTexture()
    {
        if (rend == null)
            return null;
        rend.GetMaterials(materials);
        Debug.Log("RARO totalMaterials-> " + materials.Count);
        for (int i = 0; i < materials.Count; i++)
        {
            //originalTextures.Add(Instantiate(materials[i].mainTexture));
            grayScaleTextures.Add(Instantiate(materials[i].mainTexture));
            materials[i].mainTexture = grayScaleTextures[i];
            MakeGrayScale((Texture2D)grayScaleTextures[i]);
        }
        return grayScaleTextures;
    }


    /// <summary>
    /// Makes the gray scale.
    /// </summary>
    /// <param name="tex">Tex.</param>
    private void MakeGrayScale(Texture2D tex)
    {
        var texColors = tex.GetPixels();
        for (int i = 0; i < texColors.Length; i++)
        {
            var grayValue = texColors[i].grayscale;
            texColors[i] = new Color(grayValue, grayValue, grayValue, texColors[i].a);
        }
        tex.SetPixels(texColors);
        tex.Apply();
        Debug.Log("RARO texture grayScale complete");
    }


    /// <summary>
    /// Originals the texture.
    /// </summary>
    /// <returns>The texture.</returns>
    public List<Texture> OriginalTexture()
    {
        if (rend == null)
            return null;
        //rend = GetComponent<Renderer>();
        //rend.GetMaterials(materials);
        //Debug.Log("totalMaterials-> " + materials.Count);
        for (int i = 0; i < materials.Count; i++)
        {
            originalTextures.Add(Instantiate(materials[i].mainTexture));
        }
        Debug.Log("RARO set originalTextures");
        return originalTextures;
    }


    /// <summary>
    /// Update this instance.
    /// </summary>
    private void Update()
    {
        if (leanScale != null)
        {
            if (isSelected)
            {
                leanScale.enabled = true;
            }
            else
            {

                leanScale.enabled = false;
            }
        }

        //if (isPlaced)// && !gameObject.name.Contains("car1"))
        //{
            /*
            if (!resetTextureToColor)
            {
                for (int i = 0; i < materials.Count; i++)
                {
                    materials[i].mainTexture = originalTextures[i];
                    Debug.Log("RARO reset to originalTexture");
                }
                resetTextureToColor = true;
            }

                    */

         //   rend.materials = originalMaterials;

        //}
        //else
        //{
        //    rend.materials = ghostMaterials;
        //}


        if (isPlaced)
        {
            //wasPlaced = true;
            rend.materials = originalMaterials;
        }
        else{

            rend.materials = ghostMaterials;
        }

        if (isSelected)
        {
            //isPlaced = false; // RARO
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            //if (wasPlaced)
            //{
            //    isPlaced = true;
            //}
            transform.GetChild(0).gameObject.SetActive(false);
        }
            


        //------------------
        /*
        if (isSelected)
        {
            AppManager.Instance.selectionMarker.SetActive(true);

            AppManager.Instance.selectionMarker.transform.position = transform.position;
            AppManager.Instance.selectionMarker.transform.rotation = transform.rotation;
            AppManager.Instance.selectionMarker.transform.parent = transform;

            if (!selectionMarkerSet)
            {
                AppManager.Instance.selectionMarker.transform.localScale = new Vector3(AppManager.Instance.selectionMarker.transform.localScale.x *
                                                                                       colliderSize *
                                                                                       transform.localScale.x,
                                                                                       AppManager.Instance.selectionMarker.transform.localScale.y,
                                                                                       transform.localScale.z *
                                                                                       colliderSize *
                                                                                       AppManager.Instance.selectionMarker.transform.localScale.z);


                selectionMarkerSet = true;
            }


        }
        else
        {
            //AppManager.Instance.selectionMarker.transform.parent = null;
            //AppManager.Instance.selectionMarker.transform.localScale = new Vector3(1, 1, 1);
            AppManager.Instance.selectionMarker.SetActive(false);
            selectionMarkerSet = false;
            //AppManager.Instance.selectionMarkerScaleSet = false;
        }
        */
        //------------------


    }


    /// <summary>
    /// Ons the trigger enter.
    /// </summary>
    /// <param name="other">Other.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (isPlaced)
            return;

        else{
            //lastMaterials = rend.materials;
            rend.materials = errorMaterials;
        }

        /*
        if (other.gameObject.layer == 8)
        {
            lastMaterials = rend.materials;
            rend.materials = errorMaterials;
            //rend.materials = originalMaterials;
            //this.gameObject.GetComponent<Renderer>().materials[1] = null; 
            //transform.localScale = Vector3.one * 1.5f;
            //this.gameObject.GetComponent<Renderer>().materials[0].color = new Color(1, 0.4f, 0.4f, 0.3f);
            Debug.Log("RARO - on trigger enter");
        }
        */
    }


    /// <summary>
    /// Ons the trigger exit.
    /// </summary>
    /// <param name="other">Other.</param>
    private void OnTriggerExit(Collider other)
    {

        if (isPlaced)
            return;

        else{
            rend.materials = originalMaterials;
        }

        /*
        if (other.gameObject.layer == 8)
        {

            if (isSelected || wasPlaced)
            {
                rend.materials = originalMaterials;
            }
            else{
                rend.materials = ghostMaterials;
            }

            //this.gameObject.GetComponent<Renderer>().materials[0].color = original;
            //rend.materials = ghostMaterials;
            //transform.localScale = Vector3.one;
            Debug.Log("RARO - on trigger exit");
        }
        */

    }
}



/*
private void TurnGhost(bool ghostMode, GameObject objectToGhost)
{
    if (ghostMode)
    {
        ghostMaterials = new Material[objectToGhost.GetComponent<Renderer>().materials.Length];
        originalMaterials = new Material[objectToGhost.GetComponent<Renderer>().materials.Length];
        for (int i = 0; i < objectToGhost.GetComponent<Renderer>().materials.Length; i++)
        {
            originalMaterials[i] = objectToGhost.GetComponent<Renderer>().materials[i];
            ghostMaterials[i] = ghostMaterial;
        }
        objectToGhost.GetComponent<Renderer>().materials = ghostMaterials;
        Debug.Log("GHOST MODE TRUE");
    }
    else
    {
        objectToGhost.GetComponent<Renderer>().materials = originalMaterials;
        Debug.Log("GHOST MODE FALSE");
    }
}
*/
