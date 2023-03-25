using System;
using System.Collections;
using System.Collections.Generic;
using Casual.Entities;
using Casual.Enums;
using Casual.Utilities;
using UnityEditor;
using UnityEngine;

public class LevelEditor : EditorWindow
{
    private static LevelEditor window;
    public static SquareBlock[] levelSquares;
    public static Target[] targetSquares = new Target[4];
    public static ColourRatio[] colourRatios = new ColourRatio[6];

    private static Colour[] colours = new[]
        { Colour.Blue, Colour.Red, Colour.Green, Colour.Yellow, Colour.Pink, Colour.Purple };
    
    static int gridWidth = 9;
    static int gridHeight = 11;
    private int maxMove;
    private string levelName;
    private bool isBlockSelected;
    private Texture itemTexture;
    private ItemType selectedItemType = ItemType.None;
    private ObstacleType selectedObstacleType = ObstacleType.None;
    private Colour selectedColour = Colour.None;

    [MenuItem("Casual/Level editor")]
    public static void Init()
    {
        window = (LevelEditor)EditorWindow.GetWindow(typeof(LevelEditor));
        
        levelSquares = new SquareBlock[gridHeight * gridWidth];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                SquareBlock sqBlocks = new SquareBlock();
                sqBlocks.ItemType = ItemType.None;
                sqBlocks.ObstacleType = ObstacleType.None;

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
        GUILayout.Label("Colour Ratios:", EditorStyles.boldLabel, new GUILayoutOption[]
        {
            GUILayout.Width(100),
            GUILayout.Height(15)
        });
        GUILayout.BeginHorizontal();
        GUILayout.Box(ImageLibrary.Instance.BlueCube, new GUILayoutOption[]
        {
            GUILayout.Width(50),
            GUILayout.Height(50)
        });
        GUILayout.Box(ImageLibrary.Instance.RedCube, new GUILayoutOption[]
        {
            GUILayout.Width(50),
            GUILayout.Height(50)
        });
        GUILayout.Box(ImageLibrary.Instance.GreenCube, new GUILayoutOption[]
        {
            GUILayout.Width(50),
            GUILayout.Height(50)
        });
        GUILayout.Box(ImageLibrary.Instance.YellowCube, new GUILayoutOption[]
        {
            GUILayout.Width(50),
            GUILayout.Height(50)
        });
        GUILayout.Box(ImageLibrary.Instance.PinkCube, new GUILayoutOption[]
        {
            GUILayout.Width(50),
            GUILayout.Height(50)
        });
        GUILayout.Box(ImageLibrary.Instance.PurpleCube, new GUILayoutOption[]
        {
            GUILayout.Width(50),
            GUILayout.Height(50)
        });
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        for (int i = 0; i < 6; i++)
        {
            var colourRatio = colourRatios[i] == null ? new ColourRatio() : colourRatios[i];
            colourRatio.Colour = colours[i];
            colourRatio.Ratio = EditorGUILayout.IntField("", colourRatio.Ratio, new GUILayoutOption[] {
                GUILayout.Width (51)
            });
            colourRatios[i] = colourRatio;
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
        AddSelectedObstacleTypeButton(null, ObstacleType.None);
        AddSelectedObstacleTypeButton(ImageLibrary.Instance.Balloon, ObstacleType.Box);
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
        AddSelectedItemTypeButton(null, ItemType.Cube, Colour.None);
        AddSelectedItemTypeButton(new Texture2D(40,40), ItemType.Empty, Colour.Empty);
        AddSelectedItemTypeButton(ImageLibrary.Instance.BlueCube, ItemType.Cube, Colour.Blue);
        AddSelectedItemTypeButton(ImageLibrary.Instance.PinkCube, ItemType.Cube, Colour.Pink);
        AddSelectedItemTypeButton(ImageLibrary.Instance.GreenCube, ItemType.Cube, Colour.Green);
        AddSelectedItemTypeButton(ImageLibrary.Instance.PurpleCube, ItemType.Cube, Colour.Purple);
        AddSelectedItemTypeButton(ImageLibrary.Instance.RedCube, ItemType.Cube, Colour.Red);
        AddSelectedItemTypeButton(ImageLibrary.Instance.YellowCube, ItemType.Cube, Colour.Yellow);
        AddSelectedItemTypeButton(ImageLibrary.Instance.Balloon, ItemType.Balloon, Colour.Empty);
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
                if (isBlockSelected)
                {
                    if (sqr.ItemType == ItemType.None)
                    {
                        imageButton = null;
                    }
                    else if (sqr.ItemType == ItemType.Empty)
                    {
                        imageButton = new Texture2D(40,40);
                    }
                    else if (sqr.ItemType == ItemType.Balloon)
                    {
                        imageButton = ImageLibrary.Instance.Balloon;
                    }
                    else if (sqr.ItemType == ItemType.Cube)
                    {
                        if (sqr.Colour == Colour.None)
                        {
                            imageButton = null;
                        }
                        else if (sqr.Colour == Colour.Blue)
                        {
                            imageButton = ImageLibrary.Instance.BlueCube;
                        }
                        else if (sqr.Colour == Colour.Red)
                        {
                            imageButton = ImageLibrary.Instance.RedCube;
                        }
                        else if (sqr.Colour == Colour.Green)
                        {
                            imageButton = ImageLibrary.Instance.GreenCube;
                        }
                        else if (sqr.Colour == Colour.Yellow)
                        {
                            imageButton = ImageLibrary.Instance.YellowCube;
                        }
                        else if (sqr.Colour == Colour.Pink)
                        {
                            imageButton = ImageLibrary.Instance.PinkCube;
                        }
                        else if (sqr.Colour == Colour.Purple)
                        {
                            imageButton = ImageLibrary.Instance.PurpleCube;
                        }
                    }
                }
                else
                {
                    if (sqr.ObstacleType == ObstacleType.None)
                    {
                        imageButton = null;
                    }
                    if (sqr.ObstacleType == ObstacleType.Bush)
                    {
                        imageButton = ImageLibrary.Instance.Balloon;
                    }
                    if (sqr.ObstacleType == ObstacleType.Box)
                    {
                        imageButton = ImageLibrary.Instance.Balloon;
                    }
                }
                
                if (GUILayout.Button(imageButton as Texture, new GUILayoutOption[]
                    {
                        GUILayout.Width(50),
                        GUILayout.Height(50)
                    }))
                {
                    if (isBlockSelected)
                    {
                        SetType(x, y, selectedItemType, sqr.ObstacleType, selectedColour);
                    }
                    else
                    {
                        SetType(x, y, sqr.ItemType, selectedObstacleType, sqr.Colour);
                    }
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
            List<ColourRatio> temp = new ();
            for (int i = 0; i < colourRatios.Length; i++)
            {
                if(colourRatios[i].Ratio == 0) continue;
                temp.Add(colourRatios[i]);
            }
            if(temp.Count > 0)
                scriptable.ColourRatios = temp.ToArray();

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

            scriptable.maxMove = maxMove;
            scriptable.Blocks = new SquareBlock[gridWidth * gridHeight];

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = gridHeight - 1; y >= 0; y--)
                {
                    var block = new SquareBlock();
                    block.Colour = levelSquares[gridWidth * y + x].Colour;
                    block.ItemType = levelSquares[gridWidth * y + x].ItemType;
                    block.ObstacleType = levelSquares[gridWidth * y + x].ObstacleType;
                    scriptable.Blocks[(gridHeight - 1 - y) * gridWidth + x] = block;
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
    
    void AddSelectedObstacleTypeButton(Texture texture, ObstacleType obstacleType)
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

    void SetType(int x, int y, ItemType itemType, ObstacleType obstacleType, Colour colour)
    {
        levelSquares[gridWidth * y + x].ItemType = itemType;
        levelSquares[gridWidth * y + x].ObstacleType = obstacleType;
        levelSquares[gridWidth * y + x].Colour = colour;
    }
}
