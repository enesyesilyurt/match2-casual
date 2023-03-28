using System;
using System.Collections;
using System.Collections.Generic;
using Casual.Controllers.Items;
using Casual.Entities;
using Casual.Enums;
using Casual.Utilities;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Casual
{
    public class LevelEditor : EditorWindow
    {
        private static LevelConfig levelConfig;
        private Vector2 scrollPos;
        private int width;
        private int height;
        private ItemData selectedItemData;
        private bool isItemSelected = false;
        private int gridSize;

        [OnOpenAsset]
        public static bool OpenData(int instanceId, int line)
        {
            levelConfig = EditorUtility.InstanceIDToObject(instanceId) as LevelConfig;
            if (levelConfig == null)
            {
                return false;
            }

            CreateWindow<LevelEditor>("Level Editor", typeof(SceneView)).Init(levelConfig);
            return true;
        }

        void Init(LevelConfig obj)
        {
            levelConfig = obj;
            width = levelConfig.GridWidth;
            height = levelConfig.GridHeight;
            gridSize = width * height;
            if (levelConfig.ItemDatas == null)
            {
                levelConfig.ItemDatas = new ItemData[gridSize];
                for (int x = 0; x < levelConfig.GridWidth; x++)
                {
                    for (int y = 0; y < levelConfig.GridHeight; y++)
                    {
                        ItemData sqBlocks = new ItemData();
                        sqBlocks.ItemType = null;
                        sqBlocks.ObstacleType = null;
                        levelConfig.ItemDatas[levelConfig.GridWidth * y + x] = sqBlocks;
                    }
                }
            }
            selectedItemData = new ItemData();

            levelConfig.ItemRatios = new ItemRatio[4];
            for (int i = 0; i < 4; i++)
            {
                levelConfig.ItemRatios[i] = new ItemRatio();
                levelConfig.ItemRatios[i].ItemData = new ItemData();
            }
        }

        private void OnGUI()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            DrawLevelName();
            SetGridSize();
            if(gridSize != levelConfig.GridHeight * levelConfig.GridWidth) return;
            SetMoveCount();
            SetAwards();
            SetColourRatios();
            SetSelectableItems();
            SetSelectableObstacles();
            DrawBlocks();
            GUILayout.EndScrollView();
            // GUI.DrawTexture(new Rect(50,50,50,50),ImageLibrary.Instance.GreenCube);
            // GUI.DrawTexture(new Rect(50,50,50,50),ImageLibrary.Instance.Balloon);
        }

        private void DrawLevelName()
        {
            GUILayout.Label(levelConfig.name, EditorStyles.boldLabel, new GUILayoutOption[]
            {
                GUILayout.Width(150),
                GUILayout.Height(15)
            });
        }

        private void SetMoveCount()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Max Move:", EditorStyles.boldLabel, new GUILayoutOption[]
            {
                GUILayout.Width(150),
                GUILayout.Height(15)
            });
            levelConfig.MaxMove = EditorGUILayout.IntField("", levelConfig.MaxMove, new GUILayoutOption[]
            {
                GUILayout.Width(150),
                GUILayout.MaxWidth(200)
            });
            GUILayout.EndHorizontal();
        }

        private void SetGridSize()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Grid Width/Height:", EditorStyles.boldLabel, new GUILayoutOption[]
            {
                GUILayout.Width(150),
                GUILayout.Height(15)
            });
             width = EditorGUILayout.IntField("", Math.Clamp(width, 0, 9), new GUILayoutOption[] {
                GUILayout.Width (50)
            });
            height = EditorGUILayout.IntField("", Math.Clamp(height, 0, 11), new GUILayoutOption[] {
                GUILayout.Width (50),
            });

            levelConfig.GridWidth = Math.Clamp(width, 0, 9);
            levelConfig.GridHeight = Math.Clamp(height, 0, 11);
            if (gridSize != (levelConfig.GridHeight * levelConfig.GridWidth))
            {
                gridSize = levelConfig.GridWidth * levelConfig.GridHeight;
                levelConfig.ItemDatas = new ItemData[gridSize];
                for (int x = 0; x < levelConfig.GridWidth; x++)
                {
                    for (int y = 0; y < levelConfig.GridHeight; y++)
                    {
                        ItemData sqBlocks = new ItemData();
                        sqBlocks.ItemType = null;
                        sqBlocks.ObstacleType = null;
                        levelConfig.ItemDatas[levelConfig.GridWidth * y + x] = sqBlocks;
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private void SetAwards()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Star Count:", EditorStyles.boldLabel, new GUILayoutOption[]
            {
                GUILayout.Width(150),
                GUILayout.Height(15)
            });
            levelConfig.GridHeight = EditorGUILayout.IntField("", levelConfig.GridHeight, new GUILayoutOption[] {
                GUILayout.Width (50)
            });
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Coin Count:", EditorStyles.boldLabel, new GUILayoutOption[]
            {
                GUILayout.Width(150),
                GUILayout.Height(15)
            });
            levelConfig.GridWidth = EditorGUILayout.IntField("", levelConfig.GridWidth, new GUILayoutOption[] {
                GUILayout.Width (50)
            });
            GUILayout.EndHorizontal();
        }
        
        private void SetSelectableItems()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Obstacle Type:", EditorStyles.boldLabel, new GUILayoutOption[]
            {
                GUILayout.Width(100),
                GUILayout.Height(50)
            });
            AddSelectedTypeButton(null, null);
            AddSelectedTypeButton(new Texture2D(40,40), null);
            AddSelectedTypeButton(ImageLibrary.Instance.BlueCube, typeof(CubeItem).FullName, true, Colour.Blue);
            AddSelectedTypeButton(ImageLibrary.Instance.PinkCube, "Casual.Controllers.Items.CubeItem", true, Colour.Pink);
            AddSelectedTypeButton(ImageLibrary.Instance.GreenCube, "Casual.Controllers.Items.CubeItem",  true, Colour.Green);
            AddSelectedTypeButton(ImageLibrary.Instance.PurpleCube, "Casual.Controllers.Items.CubeItem",  true, Colour.Purple);
            AddSelectedTypeButton(ImageLibrary.Instance.RedCube, "Casual.Controllers.Items.CubeItem",  true, Colour.Red);
            AddSelectedTypeButton(ImageLibrary.Instance.YellowCube, "Casual.Controllers.Items.CubeItem",  true, Colour.Yellow);
            AddSelectedTypeButton(ImageLibrary.Instance.Balloon, "Casual.Controllers.Items.BalloonItem");
            AddSelectedTypeButton(ImageLibrary.Instance.Pumpkin, "Casual.Controllers.Items.PumpkinItem");
            AddSelectedTypeButton(ImageLibrary.Instance.Box, "Casual.Controllers.Items.BoxItem");
            GUILayout.EndHorizontal();
        }

        private void SetColourRatios()
        {
            GUILayout.BeginHorizontal();
            for (int i = 0; i < 4; i++)
            {
                AddSpawnRatioButton(levelConfig.ItemRatios[i]);
            }
            GUILayout.EndHorizontal();
        }

        private void AddSpawnRatioButton(ItemRatio itemRatio)
        {
            GUILayout.BeginVertical();

            if (GUILayout.Button(itemRatio.ItemData.Texture as Texture, new GUILayoutOption[]
                {
                    GUILayout.Width(50),
                    GUILayout.Height(50)
                }))
            {
                itemRatio.ItemData.ItemType = selectedItemData.ItemType;
                itemRatio.ItemData.Colour = selectedItemData.Colour;
                itemRatio.ItemData.Texture = selectedItemData.Texture;
            }
            
            itemRatio.Ratio = EditorGUILayout.IntField("", itemRatio.Ratio, new GUILayoutOption[] {
                GUILayout.Width (50)
            });
            GUILayout.EndVertical();
        }

        private void SetSelectableObstacles()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Obstacle Type:", EditorStyles.boldLabel, new GUILayoutOption[]
            {
                GUILayout.Width(100),
                GUILayout.Height(50)
            });
            AddSelectedTypeButton(null, null, false);
            AddSelectedTypeButton(ImageLibrary.Instance.Bubble, "BubbleObstacle", false);
            AddSelectedTypeButton(ImageLibrary.Instance.Bush, "BushObstacle", false);
            GUILayout.EndHorizontal();
        }

        private void DrawBlocks()
        {
            GUILayout.BeginVertical();
            for (int y = 0; y < levelConfig.GridHeight; y++)
            {
                GUILayout.BeginHorizontal();
                for (int x = 0; x < levelConfig.GridWidth; x++)
                {
                    var itemData = levelConfig.ItemDatas[y * levelConfig.GridWidth + x];
                    if (GUILayout.Button(itemData.Texture, new GUILayoutOption[]
                        {
                            GUILayout.Width(50),
                            GUILayout.Height(50)
                        }))
                    {
                        if (isItemSelected)
                        {
                            itemData.ItemType = selectedItemData.ItemType;
                        }
                        else
                        {
                            itemData.ObstacleType = selectedItemData.ObstacleType;
                        }

                        itemData.Colour = selectedItemData.Colour;
                        itemData.Texture = selectedItemData.Texture;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        
        void AddSelectedTypeButton(Texture texture, string type, bool isItem = true, Colour colour = Colour.None)
        {
            if (GUILayout.Button(texture as Texture, new GUILayoutOption[]
                {
                    GUILayout.Width(50),
                    GUILayout.Height(50)
                }))
            {
                if (isItem)
                {
                    selectedItemData.ItemType = type;

                }
                else
                {
                    selectedItemData.ObstacleType = type;
                }

                selectedItemData.Texture = texture;
                selectedItemData.Colour = colour;
                isItemSelected = isItem;
            }
        }
    }
}
