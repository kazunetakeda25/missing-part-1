using UnityEngine;
using System.Collections;

public class MaterialUtils
{
    //Returns true if successfully added it.
    public static bool AddMaterialToRenderer( Renderer renderObj, Material matToAdd )
    {
        if ((matToAdd != null) && ( renderObj != null))
        {
            //Ensure the material isn't already on the object.
            Material[] materials = renderObj.materials;
            foreach( Material mat in materials )
            {
                if ( mat.shader == matToAdd.shader )
                {
                    return false;   //Already there.
                }
            }
			
            //If we made it this far, it wasn't there.
            int oldLen = materials.Length;
            Material[] newMatList = new Material[ oldLen + 1 ];
            materials.CopyTo( newMatList, 0 );
            newMatList[ oldLen ] = matToAdd;
            renderObj.materials = newMatList;
            
            return true;    //Successfully added.
        }
        
        return false;
    }
    
    //Returns true if successfully removed it.
    public static bool RemoveMaterialFromRenderer( Renderer renderObj, Material matToRemove )
    {
        if ( (matToRemove != null) && (renderObj != null) )
        {
            int matIndex = -1;
            Material[] matList = renderObj.materials;
            for (int ctr = 0; ctr < matList.Length; ++ctr)
            {
                Material mat = matList[ ctr ];
                if ( mat.shader == matToRemove.shader )
                {
                    //It's there.
                    matIndex = ctr;
                    break;
                }
            }            
            if (matIndex != -1)
            {
                //Remove it.
                Material[] newMatList = new Material[ matList.Length - 1 ];
                
                //Copy everything UP TO the mat over.
                int destIdx = 0;
                for ( int ctr = 0; ctr < matList.Length; ++ctr )
                {
                    if ( ctr != matIndex )
                    {
                        newMatList[ destIdx ] = matList[ ctr ];
                        ++destIdx;
                    }
                }
                
                //Now set the materials list.
                renderObj.materials = newMatList;
                
                return true;    //Success.
            }
            else
            {
                return false;   //Didn't find it.
            }
        }
        
        return false;   //Something null.
    }
}

