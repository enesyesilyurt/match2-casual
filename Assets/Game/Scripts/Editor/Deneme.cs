using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Casual.Entities;
using Casual.Enums;
using Casual.Utilities;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Object = System.Object;

public class Deneme : EditorWindow
{
    private static Deneme window;
    private static LevelConfig levelConfig;
    public static ItemData[] levelSquares;
    public static Target[] targetSquares = new Target[4];
    public static ItemRatio[] itemSpawnRatios = new ItemRatio[9];

    private static Colour[] colours = new[]
        { Colour.Blue, Colour.Red, Colour.Green, Colour.Yellow, Colour.Pink, Colour.Purple };

    // private static Texture[] itemTextures = new[]
    // {
    //     ImageLibrary.Instance.BlueCube,
    //     ImageLibrary.Instance.RedCube,
    //     ImageLibrary.Instance.YellowCube,
    //     ImageLibrary.Instance.PinkCube,
    //     ImageLibrary.Instance.PurpleCube,
    //     ImageLibrary.Instance.GreenCube,
    //     ImageLibrary.Instance.Balloon,
    //     ImageLibrary.Instance.Box,
    //     ImageLibrary.Instance.Pumpkin
    // };
    
    static int gridWidth = 9;
    static int gridHeight = 11;
    private int maxMove;
    private string levelName;
    private bool isBlockSelected;
    private Texture itemTexture;
    private ItemType selectedItemType = ItemType.None;
    private ItemType selectedObstacleType = ItemType.None;
    private Colour selectedColour = Colour.None;

    [MenuItem("Casual/Level editor")]
    public static void Init()
    {
        window = (Deneme)EditorWindow.GetWindow(typeof(Deneme));
           
        levelSquares = new ItemData[gridHeight * gridWidth];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                ItemData sqBlocks = new ItemData();
                // sqBlocks.ItemType = ItemType.None;
                // sqBlocks.ObstacleType = ItemType.None;

                levelSquares[gridWidth * y + x] = sqBlocks;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            Target target = new Target();
            target.Colour = Colour.None;
            targetSquares[i] = target;
        }
        
        window.Show();
    }
    
    // [OnOpenAssetAttribute]
    // public static bool OpenData(int instanceId, int line)
    // {
    //     var editedAsset = EditorUtility.InstanceIDToObject(instanceId) as LevelConfig;
    //     if (editedAsset == null)
    //     {
    //         return false;
    //     }
    //
    //     CreateWindow<LevelEditor>("Name", typeof(SceneView)).Prepare(editedAsset);
    //     return true;
    // }
    //
    // void Prepare(LevelConfig obj)
    // {
    //     levelConfig = obj; 
    // }

    private void OnGUI()
    {
        GUI.changed = false;
        Color squareColor = new Color(0.8f, 0.8f, 0.8f);
        GUI.color = squareColor;
        
        
        
        GUILayout.Space(10);
        // SetSize();
        GUILayout.Space(10);
        SetMaxMoves();
        GUILayout.Space(10);
        SetColourRatios();
        GUILayout.Space(10);
        AddTargetButtons();
        GUILayout.Space(10);
        SetItems();
        GUILayout.Space(10);
        SetObstacles();
        GUILayout.Space(10);
        SetGrid();
        GUILayout.Space(10);
        CreateLevel();
    }

    void SetColourRatios()
    {
        GUILayout.Label("Item Spawn Ratios:", EditorStyles.boldLabel, new GUILayoutOption[]
        {
            GUILayout.Width(150),
            GUILayout.Height(15)
        });
        GUILayout.BeginHorizontal();

        // for (int i = 0; i < itemTextures.Length; i++)
        // {
        //     GUILayout.Box(itemTextures[i], new GUILayoutOption[]
        //     {
        //         GUILayout.Width(50),
        //         GUILayout.Height(50)
        //     });
        // }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        for (int i = 0; i < 9; i++)
        {
            var colourRatio = itemSpawnRatios[i] == null ? new ItemRatio() : itemSpawnRatios[i];
            //colourRatio.Colour = colours[i]; TODO
            colourRatio.Ratio = EditorGUILayout.IntField("", colourRatio.Ratio, new GUILayoutOption[] {
                GUILayout.Width (51)
            });
            itemSpawnRatios[i] = colourRatio;
        }
        GUILayout.EndHorizontal();
    }

    void SetMaxMoves()
    {
        GUILayout.Label("Max Move:", EditorStyles.boldLabel, new GUILayoutOption[]
        {
            GUILayout.Width(100),
            GUILayout.Height(15)
        });
        
        maxMove = EditorGUILayout.IntField("", maxMove, new GUILayoutOption[] {
            GUILayout.Width (50),
            GUILayout.MaxWidth (200)
        });
    }

    void SetObstacles()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Obstacle Type:", EditorStyles.boldLabel, new GUILayoutOption[]
        {
            GUILayout.Width(100),
            GUILayout.Height(50)
        });
        AddSelectedObstacleTypeButton(null, ItemType.None);
        AddSelectedObstacleTypeButton(ImageLibrary.Instance.Bubble, ItemType.Bubble);
        AddSelectedObstacleTypeButton(ImageLibrary.Instance.Bush, ItemType.Bush);
        GUILayout.EndHorizontal();
    }

    void SetItems()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Item Type:", EditorStyles.boldLabel, new GUILayoutOption[]
        {
            GUILayout.Width(100),
            GUILayout.Height(50)
        });
        AddSelectedItemTypeButton(null, ItemType.None, Colour.None);
        AddSelectedItemTypeButton(new Texture2D(40,40), ItemType.Empty, Colour.Empty);
        AddSelectedItemTypeButton(ImageLibrary.Instance.BlueCube, ItemType.Cube, Colour.Blue);
        AddSelectedItemTypeButton(ImageLibrary.Instance.PinkCube, ItemType.Cube, Colour.Pink);
        AddSelectedItemTypeButton(ImageLibrary.Instance.GreenCube, ItemType.Cube, Colour.Green);
        AddSelectedItemTypeButton(ImageLibrary.Instance.PurpleCube, ItemType.Cube, Colour.Purple);
        AddSelectedItemTypeButton(ImageLibrary.Instance.RedCube, ItemType.Cube, Colour.Red);
        AddSelectedItemTypeButton(ImageLibrary.Instance.YellowCube, ItemType.Cube, Colour.Yellow);
        AddSelectedItemTypeButton(ImageLibrary.Instance.Balloon, ItemType.Balloon, Colour.None);
        AddSelectedItemTypeButton(ImageLibrary.Instance.Pumpkin, ItemType.Pumpkin, Colour.None);
        AddSelectedItemTypeButton(ImageLibrary.Instance.Box, ItemType.Box, Colour.None);
        GUILayout.EndHorizontal();
    }

    void SetGrid()
    {
        var imageButton = new object();
        
        GUILayout.BeginVertical();

        for (int y = 0; y < gridHeight; y++)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < gridWidth; x++)
            {
                var sqr = levelSquares[gridWidth * y + x];
                // if (isBlockSelected)
                // {
                //     if (sqr.ItemType == ItemType.None)
                //     {
                //         imageButton = null;
                //     }
                //     else if (sqr.ItemType == ItemType.Empty)
                //     {
                //         imageButton = new Texture2D(40,40);
                //     }
                //     else if (sqr.ItemType == ItemType.Balloon)
                //     {
                //         imageButton = ImageLibrary.Instance.Balloon;
                //     }
                //     else if (sqr.ItemType == ItemType.Pumpkin)
                //     {
                //         imageButton = ImageLibrary.Instance.Pumpkin;
                //     }
                //     else if (sqr.ItemType == ItemType.Box)
                //     {
                //         imageButton = ImageLibrary.Instance.Box;
                //     }
                //     else if (sqr.ItemType == ItemType.Cube)
                //     {
                //         if (sqr.Colour == Colour.Blue)
                //         {
                //             imageButton = ImageLibrary.Instance.BlueCube;
                //         }
                //         else if (sqr.Colour == Colour.Red)
                //         {
                //             imageButton = ImageLibrary.Instance.RedCube;
                //         }
                //         else if (sqr.Colour == Colour.Green)
                //         {
                //             imageButton = ImageLibrary.Instance.GreenCube;
                //         }
                //         else if (sqr.Colour == Colour.Yellow)
                //         {
                //             imageButton = ImageLibrary.Instance.YellowCube;
                //         }
                //         else if (sqr.Colour == Colour.Pink)
                //         {
                //             imageButton = ImageLibrary.Instance.PinkCube;
                //         }
                //         else if (sqr.Colour == Colour.Purple)
                //         {
                //             imageButton = ImageLibrary.Instance.PurpleCube;
                //         }
                //     }
                // }
                // else
                // {
                //     if (sqr.ObstacleType == ItemType.None)
                //     {
                //         imageButton = null;
                //     }
                //     if (sqr.ObstacleType == ItemType.Bush)
                //     {
                //         imageButton = ImageLibrary.Instance.Bush;
                //     }
                //     if (sqr.ObstacleType == ItemType.Bubble)
                //     {
                //         imageButton = ImageLibrary.Instance.Bubble;
                //     }
                // }
                
                if (GUILayout.Button(imageButton as Texture, new GUILayoutOption[]
                    {
                        GUILayout.Width(50),
                        GUILayout.Height(50)
                    }))
                {
                    // if (isBlockSelected)
                    // {
                    //     SetType(x, y, selectedItemType, sqr.ObstacleType, selectedColour);
                    // }
                    // else
                    // {
                    //     SetType(x, y, sqr.ItemType, selectedObstacleType, sqr.Colour);
                    // }
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    void CreateLevel()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Level Name:", new GUILayoutOption[]
        {
            GUILayout.Width(75),
            GUILayout.Height(20)
        });
        levelName = EditorGUILayout.TextField(levelName, new GUILayoutOption[]
        {
            GUILayout.Width(100),
            GUILayout.Height(20)
        });
        
        if (GUILayout.Button("Create Level", new GUILayoutOption[]
            {
                GUILayout.Width(100),
                GUILayout.Height(20)
            }))
        {
            var scriptable = ScriptableObject.CreateInstance<LevelConfig>();
            AssetDatabase.CreateAsset(scriptable, "Assets/Game/Scriptables/Levels/" + levelName + ".asset");

            scriptable.GridWidth = gridWidth;
            scriptable.GridHeight = gridHeight;
            List<ItemRatio> temp = new ();
            for (int i = 0; i < itemSpawnRatios.Length; i++)
            {
                if(itemSpawnRatios[i].Ratio == 0) continue;
                temp.Add(itemSpawnRatios[i]);
            }
            if(temp.Count > 0)
                scriptable.ItemRatios = temp.ToArray();

            List<Target> tempTarget = new ();

            for (int i = 0; i < 4; i++)
            {
                if ((targetSquares[i].Colour != Colour.None || targetSquares[i].ItemType != ItemType.None) && targetSquares[i].Count > 0)
                {
                    Target target = new Target();
                    target.ItemType = targetSquares[i].ItemType;
                    target.Colour = targetSquares[i].Colour;
                    target.Count = targetSquares[i].Count;
                    tempTarget.Add(target);
                }
            }
            
            if(tempTarget.Count > 0)
                scriptable.Targets = tempTarget.ToArray();

            scriptable.MaxMove = maxMove;
            scriptable.ItemDatas = new ItemData[gridWidth * gridHeight];

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = gridHeight - 1; y >= 0; y--)
                {
                    var block = new ItemData();
                    block.Colour = levelSquares[gridWidth * y + x].Colour;
                    block.ItemType = levelSquares[gridWidth * y + x].ItemType;
                    block.ObstacleType = levelSquares[gridWidth * y + x].ObstacleType;
                    scriptable.ItemDatas[(gridHeight - 1 - y) * gridWidth + x] = block;
                }
            }

            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();

            Selection.activeObject = scriptable;
        }
        GUILayout.EndHorizontal();
    }

    void AddSelectedItemTypeButton(Texture texture, ItemType itemType, Colour colour)
    {
        if (GUILayout.Button(texture as Texture, new GUILayoutOption[]
            {
                GUILayout.Width(50),
                GUILayout.Height(50)
            }))
        {
            selectedItemType = itemType;
            selectedColour = colour;
            isBlockSelected = true;
        }
    }
    
    void AddTargetButtons()
    {
        GUILayout.Label("Targets:", EditorStyles.boldLabel, new GUILayoutOption[]
        {
            GUILayout.Width(100),
            GUILayout.Height(15)
        });
        
        GUILayout.BeginHorizontal();
        
        var image = new object();
        for (int i = 0; i < 4; i++)
        {
            if (targetSquares[i].Colour == Colour.None)
            {
                image = null;
            }
            else if (targetSquares[i].Colour == Colour.Blue)
            {
                image = ImageLibrary.Instance.BlueCube;
            }
            else if (targetSquares[i].Colour == Colour.Red)
            {
                image = ImageLibrary.Instance.RedCube;
            }
            else if (targetSquares[i].Colour == Colour.Green)
            {
                image = ImageLibrary.Instance.GreenCube;
            }
            else if (targetSquares[i].Colour == Colour.Yellow)
            {
                image = ImageLibrary.Instance.YellowCube;
            }
            else if (targetSquares[i].Colour == Colour.Pink)
            {
                image = ImageLibrary.Instance.PinkCube;
            }
            else if (targetSquares[i].Colour == Colour.Purple)
            {
                image = ImageLibrary.Instance.PurpleCube;
            }
            else if (targetSquares[i].ItemType == ItemType.Balloon)
            {
                image = ImageLibrary.Instance.Balloon;
            }
            
            if (GUILayout.Button(image as Texture, new GUILayoutOption[]
                {
                    GUILayout.Width(50),
                    GUILayout.Height(50)
                }))
            {
                targetSquares[i].Colour = selectedColour;
            }
            
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        for (int i = 0; i < 4; i++)
        {
            targetSquares[i].Count = EditorGUILayout.IntField(targetSquares[i].Count, new GUILayoutOption[] {
                GUILayout.Width (50)
            });
        }
        GUILayout.EndHorizontal();
    }
    
    void AddSelectedObstacleTypeButton(Texture texture, ItemType obstacleType)
    {
        if (GUILayout.Button(texture as Texture, new GUILayoutOption[]
            {
                GUILayout.Width(50),
                GUILayout.Height(50)
            }))
        {
            selectedObstacleType = obstacleType;
            isBlockSelected = false;
        }
    }

    void SetType(int x, int y, ItemType itemType, ItemType obstacleType, Colour colour)
    {
        // levelSquares[gridWidth * y + x].ItemType = itemType;
        // levelSquares[gridWidth * y + x].ObstacleType = obstacleType;
        levelSquares[gridWidth * y + x].Colour = colour;
    }
}