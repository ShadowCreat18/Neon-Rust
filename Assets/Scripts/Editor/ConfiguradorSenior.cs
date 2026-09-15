using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class ConfiguradorSenior : EditorWindow
{
    [MenuItem("Neon-Rust/Fase Final: Ensamblaje Perfecto (Senior)")]
    public static void ConstruirPerfecto()
    {
        Debug.Log("--- INICIANDO CONSTRUCCIÓN SENIOR ---");
        try
        {
            AsegurarTag("Enemy");
            Debug.Log("Paso 1: Tags asegurados.");

        // 1. CREAR ANIMATOR CONTROLLER BÁSICO
        string controllerPath = "Assets/AnimadorNeonRust.controller";
        AnimatorController animController = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
        if (animController == null)
        {
            animController = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
            animController.AddParameter("velocidad", AnimatorControllerParameterType.Float);

            var rootStateMachine = animController.layers[0].stateMachine;

            // Buscar clips de Kevin Iglesias
            AnimationClip clipIdle = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/Kevin Iglesias/Human Animations/Animations/Male/Idles/HumanM@Idle01.fbx");
            AnimationClip clipWalk = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/Kevin Iglesias/Human Animations/Animations/Male/Movement/Walk/HumanM@Walk01_Forward.fbx");

            AnimatorState stateIdle = rootStateMachine.AddState("Idle");
            stateIdle.motion = clipIdle;

            AnimatorState stateWalk = rootStateMachine.AddState("Walk");
            stateWalk.motion = clipWalk;

            // Transiciones
            AnimatorStateTransition idleToWalk = stateIdle.AddTransition(stateWalk);
            idleToWalk.AddCondition(AnimatorConditionMode.Greater, 0.1f, "velocidad");
            idleToWalk.duration = 0.1f;

            AnimatorStateTransition walkToIdle = stateWalk.AddTransition(stateIdle);
            walkToIdle.AddCondition(AnimatorConditionMode.Less, 0.1f, "velocidad");
            walkToIdle.duration = 0.1f;
        }

        // 2. CREAR ESCENARIO (Eliminar el anterior si existe para evitar duplicados)
        GameObject viejoEscenario = GameObject.Find("Escenario");
        if (viejoEscenario != null) DestroyImmediate(viejoEscenario);
        
        GameObject escenarioBase = new GameObject("Escenario");
        GameObject prefabSuelo = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Creepy_Cat/3D Scifi Kit Starter Kit_HD/Prefabs/Floors/Room_Floor_3x3.prefab");
        if (prefabSuelo != null)
        {
            // Plataforma plana larga en Z = 0
            for (int x = -2; x <= 6; x++)
            {
                GameObject piso = PrefabUtility.InstantiatePrefab(prefabSuelo) as GameObject;
                piso.transform.SetParent(escenarioBase.transform);
                piso.transform.position = new Vector3(x * 12f, 0, 0);
            }

            // Un par de plataformas elevadas para probar el salto
            GameObject pisoElevado1 = PrefabUtility.InstantiatePrefab(prefabSuelo) as GameObject;
            pisoElevado1.transform.SetParent(escenarioBase.transform);
            pisoElevado1.transform.position = new Vector3(2 * 12f, 5f, 0);

            GameObject pisoElevado2 = PrefabUtility.InstantiatePrefab(prefabSuelo) as GameObject;
            pisoElevado2.transform.SetParent(escenarioBase.transform);
            pisoElevado2.transform.position = new Vector3(4 * 12f, 8f, 0);
        }
        Debug.Log("Paso 2: Escenario Base Creado.");

        // Proyectil
        GameObject prefabBala = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/PrefabBala.prefab");
        if (prefabBala == null)
        {
            GameObject balaObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            balaObj.transform.localScale = Vector3.one * 0.2f;
            balaObj.GetComponent<Renderer>().sharedMaterial.color = Color.cyan;
            balaObj.GetComponent<Collider>().isTrigger = true;
            balaObj.AddComponent<NeonRust.Armas.Proyectil>();
            prefabBala = PrefabUtility.SaveAsPrefabAsset(balaObj, "Assets/PrefabBala.prefab");
            DestroyImmediate(balaObj);
        }

        // Chatarra (Scrap)
        GameObject prefabScrap = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/PrefabChatarra.prefab");
        if (prefabScrap == null)
        {
            GameObject scrapObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            scrapObj.transform.localScale = Vector3.one * 0.4f;
            scrapObj.GetComponent<Renderer>().sharedMaterial.color = Color.yellow;
            
            BoxCollider bc = scrapObj.GetComponent<BoxCollider>();
            bc.isTrigger = false;
            
            // Trigger secundario más grande para detectar al jugador
            SphereCollider trigger = scrapObj.AddComponent<SphereCollider>();
            trigger.isTrigger = true;
            trigger.radius = 3f;
            
            scrapObj.AddComponent<Rigidbody>();
            scrapObj.AddComponent<NeonRust.Nucleo.Chatarra>();
            
            prefabScrap = PrefabUtility.SaveAsPrefabAsset(scrapObj, "Assets/PrefabChatarra.prefab");
            DestroyImmediate(scrapObj);
        }
        Debug.Log("Paso 3: Proyectiles y Chatarra Listos.");

        // 3. JUGADOR (Volt = Trooper)
        GameObject volt = GameObject.Find("Volt");
        if (volt != null) DestroyImmediate(volt);
        
        GameObject prefabVolt = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Sci-FI_Trooper_Man_v.3/Prefabs/SK_SciFiTrooperManV3.prefab");
        if (prefabVolt != null)
        {
            volt = Object.Instantiate(prefabVolt);
            volt.name = "Volt";
            volt.tag = "Player";
            volt.transform.position = new Vector3(-10, 5, 0);

            Debug.Log("Paso 4: Volt instanciado, limpiando colliders...");
            // Limpiar Colliders para evitar errores con Rigidbody Dinámico
            foreach (var col in volt.GetComponentsInChildren<Collider>(true)) DestroyImmediate(col);

            // Físicas Jugador 2.5D
            CapsuleCollider ccVolt = volt.AddComponent<CapsuleCollider>();
            ccVolt.height = 2f;
            ccVolt.center = new Vector3(0, 1f, 0);

            Rigidbody rbVolt = volt.AddComponent<Rigidbody>();
            rbVolt.useGravity = true;
            rbVolt.isKinematic = false;
            rbVolt.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY; 

            // Scripts Jugador
            volt.AddComponent<NeonRust.Nucleo.Entidad>().saludMaxima = 100f;
            volt.AddComponent<NeonRust.Jugador.ControladorJugador>();
            var sistDVolt = volt.AddComponent<NeonRust.Armas.SistemaDisparo>();
            sistDVolt.tagAliado = "Player";
            sistDVolt.cadencia = 0.3f;
            sistDVolt.prefabProyectil = prefabBala;

            Animator animVolt = volt.GetComponent<Animator>();
            if (animVolt != null) 
            {
                animVolt.runtimeAnimatorController = animController;
                animVolt.applyRootMotion = false; // EVITA QUE LA ANIMACIÓN BLOQUEE LA ROTACIÓN DEL MOUSE
            }

            // Arma Jugador (Sniper)
            Transform manoVolt = EncontrarManoDerecha(animVolt, volt.transform);
            if (manoVolt == null) 
            {
                GameObject holder = new GameObject("WeaponHolder");
                holder.transform.SetParent(volt.transform);
                holder.transform.localPosition = new Vector3(0.3f, 1.3f, 0.5f); // Altura del pecho/hombro
                manoVolt = holder.transform;
            }

            Debug.Log("Paso 5: Añadiendo arma a Volt...");
            GameObject prefabSniper = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/MAC_SciFiSniperRifle/Prefabs/MAC_SciFiSniperRifle.prefab");
            if (prefabSniper != null)
            {
                GameObject sniper = Object.Instantiate(prefabSniper);
                foreach (var col in sniper.GetComponentsInChildren<Collider>(true)) DestroyImmediate(col);

                sniper.name = "SniperRifle";
                sniper.transform.SetParent(manoVolt);
                sniper.transform.localPosition = Vector3.zero;
                sniper.transform.localRotation = Quaternion.Euler(-90, 0, 90);
                
                // Escala absoluta fija
                sniper.transform.localScale = new Vector3(0.012f, 0.012f, 0.012f);

                GameObject puntoD = new GameObject("PuntoDisparo");
                puntoD.transform.SetParent(sniper.transform);
                puntoD.transform.localPosition = new Vector3(0, 0, 1.5f);
                sistDVolt.puntoDisparo = puntoD.transform;
            }
        }

        // 4. ENEMIGO (Renegade)
        GameObject enemigo = GameObject.Find("RobotEnemigo");
        if (enemigo != null) DestroyImmediate(enemigo);
        
        GameObject prefabRenegade = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Renegade/Prefabs/Renegade.prefab");
        if (prefabRenegade == null)
        {
            // Fallback si no está en la ruta nueva, buscar en la vieja
            prefabRenegade = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Sci_Fi_Character_08/Prefabs/Sci_Fi_Character_08_02.prefab");
        }
        
        if (prefabRenegade != null)
        {
            Debug.Log("Paso 6: Instanciando Enemigo...");
            enemigo = Object.Instantiate(prefabRenegade);
            enemigo.name = "RobotEnemigo";
            enemigo.tag = "Enemy";
            enemigo.transform.position = new Vector3(24, 10, 0); // Ponerlo en una plataforma elevada

            // Limpiar Colliders del Enemigo
            foreach (var col in enemigo.GetComponentsInChildren<Collider>(true)) DestroyImmediate(col);
        }
        
        Debug.Log("Paso 7: Físicas e IA del Enemigo configuradas.");

        // Físicas Enemigo 2.5D
        CapsuleCollider ccEne = enemigo.AddComponent<CapsuleCollider>();
        ccEne.height = 2f;
        ccEne.center = new Vector3(0, 1f, 0);

        Rigidbody rbEne = enemigo.AddComponent<Rigidbody>();
        rbEne.useGravity = true; // Ahora sí usamos gravedad para que caiga en la plataforma
        rbEne.isKinematic = false; 
        rbEne.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

        // Scripts Enemigo
        enemigo.AddComponent<NeonRust.Nucleo.Entidad>().saludMaxima = 50f;
        var sistDEne = enemigo.AddComponent<NeonRust.Armas.SistemaDisparo>();
        sistDEne.tagAliado = "Enemy";
        sistDEne.cadencia = 1.2f;
        sistDEne.prefabProyectil = prefabBala;
        var IAEnemiga = enemigo.AddComponent<NeonRust.Enemigos.EnemigoBase>();
        IAEnemiga.prefabChatarra = prefabScrap;

        Animator animEne = enemigo.GetComponent<Animator>();
        if (animEne != null) 
        {
            animEne.runtimeAnimatorController = animController;
            animEne.applyRootMotion = false;
        }

        // Arma Enemigo (Shotgun)
        Transform manoEne = EncontrarManoDerecha(animEne, enemigo.transform);
        if (manoEne == null) 
        {
            GameObject holder = new GameObject("WeaponHolder");
            holder.transform.SetParent(enemigo.transform);
            holder.transform.localPosition = new Vector3(0.3f, 1.3f, 0.5f); // Altura del pecho/hombro
            manoEne = holder.transform;
        }

        Debug.Log("Paso 8: Añadiendo arma al Enemigo...");
        GameObject prefabShotgun = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/MONQO Sci-fi Shotgun/_Prefab/Sci-Fi Shotgun.prefab");
        if (prefabShotgun != null)
        {
            GameObject shotgun = Object.Instantiate(prefabShotgun);
            foreach (var col in shotgun.GetComponentsInChildren<Collider>(true)) DestroyImmediate(col);

            shotgun.name = "Shotgun";
            shotgun.transform.SetParent(manoEne);
            shotgun.transform.localPosition = Vector3.zero;
            shotgun.transform.localRotation = Quaternion.Euler(-90, 0, 90);
            
            shotgun.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);

            GameObject puntoDE = new GameObject("PuntoDisparo");
            puntoDE.transform.SetParent(shotgun.transform);
            puntoDE.transform.localPosition = new Vector3(0, 1f, 0);
            sistDEne.puntoDisparo = puntoDE.transform;
        }

        // 5. Cámara 2.5D
        Camera cam = Camera.main;
        if (cam != null)
        {
            cam.transform.position = new Vector3(0, 3f, -15f);
            cam.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            
            // Añadir script de seguimiento 2D si no lo tiene
            if (cam.GetComponent<NeonRust.Camara.SeguimientoCamara>() == null)
            {
                var seg = cam.gameObject.AddComponent<NeonRust.Camara.SeguimientoCamara>();
                seg.objetivo = volt.transform;
            }
            else
            {
                cam.GetComponent<NeonRust.Camara.SeguimientoCamara>().objetivo = volt.transform;
            }
        }

        // Limpiar archivo viejo para no confundir
        AssetDatabase.DeleteAsset("Assets/Scripts/Editor/ArmadorJuego.cs");

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        
        Debug.Log("--- CONSTRUCCIÓN SENIOR COMPLETADA CON ÉXITO ---");
        EditorUtility.DisplayDialog("¡Construcción Completada!", "He añadido logs a la consola. Por favor, revisa la consola para verificar que todos los pasos se completaron.", "Entendido");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"--- ERROR CRÍTICO EN LA CONSTRUCCIÓN: {e.Message}\n{e.StackTrace}");
            EditorUtility.DisplayDialog("Error", "Revisa la consola roja. Algo falló en la construcción.", "OK");
        }
    }

    private static Transform EncontrarManoDerecha(Animator anim, Transform raiz)
    {
        // Intentar primero con el sistema Humanoid de Unity (100% preciso si es humanoide)
        if (anim != null && anim.isHuman)
        {
            Transform manoHumanoid = anim.GetBoneTransform(HumanBodyBones.RightHand);
            if (manoHumanoid != null) return manoHumanoid;
        }

        // Búsqueda por nombre como respaldo
        Transform mano = null;
        foreach (Transform t in raiz.GetComponentsInChildren<Transform>())
        {
            string n = t.name.ToLower();
            if (n.Contains("hand") || n.Contains("mano") || n.Contains("wrist"))
            {
                if (n.Contains("r") || n.Contains("right") || n.Contains("der")) return t;
                mano = t; 
            }
        }
        return mano;
    }

    private static void AsegurarTag(string tag)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        bool existe = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(tag)) { existe = true; break; }
        }

        if (!existe)
        {
            tagsProp.InsertArrayElementAtIndex(0);
            SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
            n.stringValue = tag;
            tagManager.ApplyModifiedProperties();
        }
    }
}
