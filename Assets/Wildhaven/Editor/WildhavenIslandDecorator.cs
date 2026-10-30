using UnityEditor;
using UnityEngine;

namespace Wildhaven.Editor
{
    public static class WildhavenIslandDecorator
    {
        [MenuItem("Wildhaven/Generate Island Decorations")]
        public static void Generate()
        {
            var root = GameObject.Find("Island Decorations");
            if (root != null) Object.DestroyImmediate(root);
            root = new GameObject("Island Decorations");

            var ground = GameObject.Find("Island Ground");
            if (ground == null)
            {
                Debug.LogWarning("Create the Wildhaven demo scene first.");
                return;
            }

            var green = MakeMaterial(new Color(0.12f, 0.34f, 0.16f));
            var trunk = MakeMaterial(new Color(0.36f, 0.22f, 0.10f));
            var leaf = MakeMaterial(new Color(0.10f, 0.42f, 0.20f));
            var stone = MakeMaterial(new Color(0.38f, 0.40f, 0.39f));

            for (int i = 0; i < 18; i++)
            {
                float angle = i * Mathf.PI * 2f / 18f;
                float radius = 25f + (i % 3) * 2.5f;
                Vector3 p = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                CreateTree(root.transform, p, trunk, leaf, i % 2 == 0 ? 1f : 0.8f);
            }

            for (int i = 0; i < 22; i++)
            {
                float x = Mathf.Sin(i * 12.3f) * 27f;
                float z = Mathf.Cos(i * 7.7f) * 27f;
                var rock = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                rock.name = "Island Rock";
                rock.transform.SetParent(root.transform);
                rock.transform.position = new Vector3(x, 0.45f, z);
                rock.transform.localScale = new Vector3(1.1f, 0.75f, 0.9f) * (0.7f + (i % 4) * 0.2f);
                rock.GetComponent<Renderer>().sharedMaterial = stone;
            }

            var water = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            water.name = "Ocean Water";
            water.transform.SetParent(root.transform);
            water.transform.position = new Vector3(0, -1.1f, 0);
            water.transform.localScale = new Vector3(90, 0.1f, 90);
            water.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(0.04f, 0.30f, 0.48f));

            RenderSettings.ambientLight = new Color(0.62f, 0.68f, 0.72f);
            Debug.Log("Generated decorative trees, rocks, and ocean placeholder. Adjust layout and lighting in the Scene view.");
        }

        private static void CreateTree(Transform parent, Vector3 position, Material bark, Material leaves, float scale)
        {
            var trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            trunk.name = "Palm Tree Trunk";
            trunk.transform.SetParent(parent);
            trunk.transform.position = position + Vector3.up * (2f * scale);
            trunk.transform.localScale = new Vector3(0.28f, 2f, 0.28f) * scale;
            trunk.GetComponent<Renderer>().sharedMaterial = bark;

            for (int j = 0; j < 5; j++)
            {
                float a = j * Mathf.PI * 2f / 5f;
                var frond = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                frond.name = "Palm Canopy";
                frond.transform.SetParent(parent);
                frond.transform.position = position + Vector3.up * (4f * scale) +
                    new Vector3(Mathf.Cos(a) * 1.05f, 0.15f, Mathf.Sin(a) * 1.05f) * scale;
                frond.transform.localScale = new Vector3(1.5f, 0.18f, 0.55f) * scale;
                frond.transform.rotation = Quaternion.Euler(0, -a * Mathf.Rad2Deg, 15);
                frond.GetComponent<Renderer>().sharedMaterial = leaves;
            }
        }

        private static Material MakeMaterial(Color color)
        {
            var shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Universal Render Pipeline/Lit");
            var material = new Material(shader) { color = color };
            return material;
        }
    }
}