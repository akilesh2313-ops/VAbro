using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Wildhaven.Editor
{
    // Ensures a playable starter scene exists and is included whenever an Android build starts.
    public sealed class WildhavenBuildSetup : IPreprocessBuildWithReport
    {
        private const string ScenePath = "Assets/Wildhaven/Scenes/WildhavenDemo.unity";
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            EnsureDemoScene();
        }

        [MenuItem("Wildhaven/Create or Repair Demo Scene")]
        public static void EnsureDemoScene()
        {
            Directory.CreateDirectory("Assets/Wildhaven/Scenes");
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var gameManager = new GameObject("WildhavenGameManager");
            gameManager.AddComponent<WildhavenGameManager>();

            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Island Ground";
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(8, 1, 8);
            ground.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(0.24f, 0.48f, 0.25f));

            CreatePlayer();
            CreateCamera();
            CreatePickup("Berries Pickup", ResourceType.Berries, 5, new Vector3(3, 0.6f, 2), new Color(0.75f, 0.12f, 0.18f));
            CreatePickup("Wood Pickup", ResourceType.Wood, 5, new Vector3(-3, 0.6f, 2), new Color(0.48f, 0.28f, 0.12f));
            CreatePickup("Stone Pickup", ResourceType.Stone, 3, new Vector3(3, 0.6f, -3), Color.gray);
            CreateShelter();
            CreateHud();

            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(ScenePath, true)
            };
            AssetDatabase.SaveAssets();
            Debug.Log("Wildhaven demo scene created and added to Build Settings.");
        }

        private static Material MakeMaterial(Color color)
        {
            var shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Universal Render Pipeline/Lit");
            var material = new Material(shader) { color = color };
            return material;
        }

        private static void CreatePlayer()
        {
            var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            player.tag = "Player";
            player.transform.position = new Vector3(0, 1.1f, 0);
            Object.DestroyImmediate(player.GetComponent<CapsuleCollider>());
            player.AddComponent<CharacterController>();
            player.AddComponent<PlayerMotor>();
            var customizer = player.AddComponent<CharacterCustomizer>();
            var customizerData = new SerializedObject(customizer);
            var renderers = customizerData.FindProperty("outfitRenderers");
            renderers.arraySize = 1;
            renderers.GetArrayElementAtIndex(0).objectReferenceValue = player.GetComponent<Renderer>();
            customizerData.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void CreateCamera()
        {
            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            var camera = cameraObject.AddComponent<Camera>();
            cameraObject.AddComponent<AudioListener>();
            var follow = cameraObject.AddComponent<FollowCamera>();
            var player = GameObject.FindGameObjectWithTag("Player");
            var serialized = new SerializedObject(follow);
            serialized.FindProperty("target").objectReferenceValue = player.transform;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            var motor = player.GetComponent<PlayerMotor>();
            var motorSerialized = new SerializedObject(motor);
            motorSerialized.FindProperty("cameraTransform").objectReferenceValue = cameraObject.transform;
            motorSerialized.ApplyModifiedPropertiesWithoutUndo();
            cameraObject.transform.position = new Vector3(0, 5, -7);
            cameraObject.transform.LookAt(player.transform.position);
        }

        private static void CreatePickup(string title, ResourceType type, int amount, Vector3 position, Color color)
        {
            var pickup = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pickup.name = title;
            pickup.transform.position = position;
            pickup.transform.localScale = Vector3.one * 0.65f;
            pickup.GetComponent<Renderer>().sharedMaterial = MakeMaterial(color);
            var collider = pickup.GetComponent<SphereCollider>();
            collider.isTrigger = true;
            var item = pickup.AddComponent<WorldPickup>();
            var serialized = new SerializedObject(item);
            serialized.FindProperty("resourceType").enumValueIndex = (int)type;
            serialized.FindProperty("amount").intValue = amount;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void CreateShelter()
        {
            var shelter = new GameObject("Shelter Upgrade Station");
            shelter.transform.position = new Vector3(0, 0, 5);
            var station = shelter.AddComponent<ShelterStation>();
            var stageOne = GameObject.CreatePrimitive(PrimitiveType.Cube);
            stageOne.name = "Shelter Level 1";
            stageOne.transform.SetParent(shelter.transform);
            stageOne.transform.localPosition = new Vector3(0, 1, 0);
            stageOne.transform.localScale = new Vector3(3, 2, 2);
            stageOne.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(0.45f, 0.27f, 0.12f));
            var stageTwo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            stageTwo.name = "Shelter Level 2";
            stageTwo.transform.SetParent(shelter.transform);
            stageTwo.transform.localPosition = new Vector3(0, 1.5f, 0);
            stageTwo.transform.localScale = new Vector3(4, 3, 3);
            stageTwo.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(0.34f, 0.22f, 0.12f));
            stageTwo.SetActive(false);
            var serialized = new SerializedObject(station);
            var stages = serialized.FindProperty("upgradeStages");
            stages.arraySize = 2;
            stages.GetArrayElementAtIndex(0).objectReferenceValue = stageOne;
            stages.GetArrayElementAtIndex(1).objectReferenceValue = stageTwo;
            serialized.FindProperty("woodCost").intValue = 5;
            serialized.FindProperty("stoneCost").intValue = 3;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void CreateHud()
        {
            var canvasObject = new GameObject("HUD Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var textObject = new GameObject("Inventory Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            textObject.transform.SetParent(canvasObject.transform, false);
            var text = textObject.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 22;
            text.color = Color.white;
            text.alignment = TextAnchor.UpperLeft;
            text.rectTransform.anchorMin = new Vector2(0, 1);
            text.rectTransform.anchorMax = new Vector2(0, 1);
            text.rectTransform.pivot = new Vector2(0, 1);
            text.rectTransform.anchoredPosition = new Vector2(20, -20);
            text.rectTransform.sizeDelta = new Vector2(700, 45);

            var shelterObject = new GameObject("Shelter Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            shelterObject.transform.SetParent(canvasObject.transform, false);
            var shelterText = shelterObject.GetComponent<Text>();
            shelterText.font = text.font;
            shelterText.fontSize = 22;
            shelterText.color = Color.white;
            shelterText.rectTransform.anchorMin = new Vector2(0, 1);
            shelterText.rectTransform.anchorMax = new Vector2(0, 1);
            shelterText.rectTransform.pivot = new Vector2(0, 1);
            shelterText.rectTransform.anchoredPosition = new Vector2(20, -55);
            shelterText.rectTransform.sizeDelta = new Vector2(400, 40);

            var hud = canvasObject.AddComponent<RuntimeHUD>();
            var serialized = new SerializedObject(hud);
            serialized.FindProperty("inventoryText").objectReferenceValue = text;
            serialized.FindProperty("shelterText").objectReferenceValue = shelterText;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
