using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameWish.Game
{
    public class PatternLoader : MonoBehaviour
    {
        public static PatternLoader instance = null;
        public bool isPersistant = true;

        [HideInInspector]
        public string patternLocation = "";

        [HideInInspector]
        private Transform[] patterns;
        [HideInInspector]
        private List<Transform> introductoryPatterns = new List<Transform>();
        [HideInInspector]
        private List<Transform> startPatterns = new List<Transform>();
        [HideInInspector]
        private List<Transform> endPatterns = new List<Transform>();
        [HideInInspector]
        private List<Transform> connectorPatterns = new List<Transform>();
        [HideInInspector]
        private List<Transform> easyPatterns = new List<Transform>();
        [HideInInspector]
        private List<int> easyPatterns_Entrances = new List<int>();
        [HideInInspector]
        private List<Transform> mediumPatterns = new List<Transform>();
        [HideInInspector]
        private List<int> mediumPatterns_Entrances = new List<int>();
        [HideInInspector]
        private List<Transform> hardPatterns = new List<Transform>();
        [HideInInspector]
        private List<int> hardPatterns_Entrances = new List<int>();

        //Easy enemy patterns
        [HideInInspector]
        private List<Transform> enemyEasyPatterns = new List<Transform>();
        [HideInInspector]
        private List<int> enemyEasyPatterns_Entrances = new List<int>();
        [HideInInspector]
        private List<Transform> enemyEasyBossPatterns = new List<Transform>();
        [HideInInspector]
        private List<int> enemyEasyBossPatterns_Entrances = new List<int>();

        //Medium enemy patterns
        [HideInInspector]
        private List<Transform> enemyMediumPatterns = new List<Transform>();
        [HideInInspector]
        private List<int> enemyMediumPatterns_Entrances = new List<int>();
        [HideInInspector]
        private List<Transform> enemyMediumBossPatterns = new List<Transform>();
        [HideInInspector]
        private List<int> enemyMediumBossPatterns_Entrances = new List<int>();

        //Hard enemy patterns
        [HideInInspector]
        private List<Transform> enemyHardPatterns = new List<Transform>();
        [HideInInspector]
        private List<int> enemyHardPatterns_Entrances = new List<int>();
        [HideInInspector]
        private List<Transform> enemyHardBossPatterns = new List<Transform>();
        [HideInInspector]
        private List<int> enemyHardBossPatterns_Entrances = new List<int>();

        [HideInInspector]
        public bool patternsLoaded = false;
        private List<int> tempEntranceList = new List<int>();

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            if (isPersistant)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        public void LoadPatterns()
        {
            if (patternsLoaded == false)
            {
                Debug.Log("Loading Patterns...");
                patterns = Resources.LoadAll<Transform>(patternLocation);
                for (int i = 0; i < patterns.Length; i++)
                {
                    Pattern indexedPatternScript = patterns[i].GetComponent<Pattern>();
                    switch (indexedPatternScript.type)
                    {
                        case Pattern.Type.Introductory:
                            introductoryPatterns.Add(patterns[i]);
                            break;
                        case Pattern.Type.Start:
                            startPatterns.Add(patterns[i]);
                            break;
                        case Pattern.Type.End:
                            endPatterns.Add(patterns[i]);
                            break;
                        case Pattern.Type.Connector:
                            connectorPatterns.Add(patterns[i]);
                            break;
                        case Pattern.Type.Easy:
                            easyPatterns.Add(patterns[i]);
                            easyPatterns_Entrances.Add(indexedPatternScript.bottomEntrances);
                            break;
                        case Pattern.Type.Medium:
                            mediumPatterns.Add(patterns[i]);
                            mediumPatterns_Entrances.Add(indexedPatternScript.bottomEntrances);
                            break;
                        case Pattern.Type.Hard:
                            hardPatterns.Add(patterns[i]);
                            hardPatterns_Entrances.Add(indexedPatternScript.bottomEntrances);
                            break;
                        case Pattern.Type.Enemy:
                            if (indexedPatternScript.enemyType == Pattern.EnemyType.Easy)
                            {
                                enemyEasyPatterns.Add(patterns[i]);
                                enemyEasyPatterns_Entrances.Add(indexedPatternScript.bottomEntrances);
                            }
                            else if (indexedPatternScript.enemyType == Pattern.EnemyType.Easy_Boss)
                            {
                                enemyEasyBossPatterns.Add(patterns[i]);
                                enemyEasyBossPatterns_Entrances.Add(indexedPatternScript.bottomEntrances);
                            }
                            else if (indexedPatternScript.enemyType == Pattern.EnemyType.Medium)
                            {
                                enemyMediumPatterns.Add(patterns[i]);
                                enemyMediumPatterns_Entrances.Add(indexedPatternScript.bottomEntrances);
                            }
                            else if (indexedPatternScript.enemyType == Pattern.EnemyType.Medium_Boss)
                            {
                                enemyMediumBossPatterns.Add(patterns[i]);
                                enemyMediumBossPatterns_Entrances.Add(indexedPatternScript.bottomEntrances);
                            }
                            else if (indexedPatternScript.enemyType == Pattern.EnemyType.Hard)
                            {
                                enemyHardPatterns.Add(patterns[i]);
                                enemyHardPatterns_Entrances.Add(indexedPatternScript.bottomEntrances);
                            }
                            else if (indexedPatternScript.enemyType == Pattern.EnemyType.Hard_Boss)
                            {
                                enemyHardBossPatterns.Add(patterns[i]);
                                enemyHardBossPatterns_Entrances.Add(indexedPatternScript.bottomEntrances);
                            }
                            break;
                    }
                }
                patternsLoaded = true;
            }
        }

        public Transform GetIntroductoryPattern()
        {
            Transform pattern = null;
            if (introductoryPatterns.Count > 0)
            {
                pattern = introductoryPatterns[Random.Range(0, introductoryPatterns.Count)];
            }
            else
            {
                Debug.LogWarning("There are no Introductory patterns");
            }
            return pattern;
        }

        public Transform GetStartPattern()
        {
            Transform pattern = null;
            if (startPatterns.Count > 0)
            {
                pattern = startPatterns[Random.Range(0, startPatterns.Count)];
            }
            else
            {
                Debug.LogWarning("There are no Start patterns");
            }
            return pattern;
        }

        public Transform GetEndPattern()
        {
            Transform pattern = null;
            if (endPatterns.Count > 0)
            {
                pattern = endPatterns[Random.Range(0, endPatterns.Count)];
            }
            else
            {
                Debug.LogWarning("There are no End patterns");
            }
            return pattern;
        }

        public Transform GetConnectorPattern()
        {
            Transform pattern = null;
            if (connectorPatterns.Count > 0)
            {
                pattern = connectorPatterns[Random.Range(0, connectorPatterns.Count)];
            }
            else
            {
                Debug.LogWarning("There are no Connector patterns");
            }
            return pattern;
        }

        public Transform GetEasyPattern(int entranceInt = -1)
        {
            Transform pattern = null;
            if (entranceInt == -1)
            {
                if (easyPatterns.Count > 0)
                {
                    pattern = easyPatterns[Random.Range(0, easyPatterns.Count)];
                }
            }
            else
            {
                tempEntranceList.Clear();
                for (int i = 0; i < easyPatterns_Entrances.Count; i++)
                {
                    tempEntranceList.Add(i);
                }
                int index = -1;
                while (tempEntranceList.Count > 0)
                {
                    index = Random.Range(0, tempEntranceList.Count);
                    if ((easyPatterns_Entrances[tempEntranceList[index]] & entranceInt) > 0)
                    {
                        index = tempEntranceList[index];
                        break;
                    }
                    else
                    {
                        tempEntranceList.RemoveAt(index);
                    }
                }

                if (index > -1)
                {
                    pattern = easyPatterns[index];
                }
                else
                {
                    pattern = GetConnectorPattern();
                }
            }
            return pattern;
        }

        public Transform GetMediumPattern(int entranceInt = -1)
        {
            Transform pattern = null;
            if (entranceInt == -1)
            {
                if (mediumPatterns.Count > 0)
                {
                    pattern = mediumPatterns[Random.Range(0, mediumPatterns.Count)];
                }
            }
            else
            {
                tempEntranceList.Clear();
                for (int i = 0; i < mediumPatterns_Entrances.Count; i++)
                {
                    tempEntranceList.Add(i);
                }
                int index = -1;
                while (tempEntranceList.Count > 0)
                {
                    index = Random.Range(0, tempEntranceList.Count);
                    if ((mediumPatterns_Entrances[tempEntranceList[index]] & entranceInt) > 0)
                    {
                        index = tempEntranceList[index];
                        break;
                    }
                    else
                    {
                        tempEntranceList.RemoveAt(index);
                    }
                }

                if (index > -1)
                {
                    pattern = mediumPatterns[index];
                }
                else
                {
                    pattern = GetEasyPattern(entranceInt);
                }
            }
            return pattern;
        }

        public Transform GetHardPattern(int entranceInt = -1)
        {
            Transform pattern = null;
            if (entranceInt == -1)
            {
                if (hardPatterns.Count > 0)
                {
                    pattern = hardPatterns[Random.Range(0, hardPatterns.Count)];
                }
            }
            else
            {
                tempEntranceList.Clear();
                for (int i = 0; i < hardPatterns_Entrances.Count; i++)
                {
                    tempEntranceList.Add(i);
                }
                int index = -1;
                while (tempEntranceList.Count > 0)
                {
                    index = Random.Range(0, tempEntranceList.Count);
                    if ((hardPatterns_Entrances[tempEntranceList[index]] & entranceInt) > 0)
                    {
                        index = tempEntranceList[index];
                        break;
                    }
                    else
                    {
                        tempEntranceList.RemoveAt(index);
                    }
                }

                if (index > -1)
                {
                    pattern = hardPatterns[index];
                }
                else
                {
                    pattern = GetMediumPattern(entranceInt);
                }
            }
            return pattern;
        }

        //FOR EASY ENEMY PATTERNS
        public Transform GetEnemyEasyPattern(int entranceInt = -1)
        {
            Transform pattern = null;
            if (entranceInt == -1)
            {
                if (enemyEasyPatterns.Count > 0)
                {
                    pattern = enemyEasyPatterns[Random.Range(0, enemyEasyPatterns.Count)];
                }
            }
            else
            {
                tempEntranceList.Clear();
                for (int i = 0; i < enemyEasyPatterns_Entrances.Count; i++)
                {
                    tempEntranceList.Add(i);
                }
                int index = -1;
                while (tempEntranceList.Count > 0)
                {
                    index = Random.Range(0, tempEntranceList.Count);
                    if ((enemyEasyPatterns_Entrances[tempEntranceList[index]] & entranceInt) > 0)
                    {
                        index = tempEntranceList[index];
                        break;
                    }
                    else
                    {
                        tempEntranceList.RemoveAt(index);
                    }
                }

                if (index > -1)
                {
                    pattern = enemyEasyPatterns[index];
                }
                else
                {
                    pattern = GetConnectorPattern();
                }
            }
            return pattern;
        }

        public Transform GetEnemyEasyBossPattern(int entranceInt = -1)
        {
            Transform pattern = null;
            if (entranceInt == -1)
            {
                if (enemyEasyBossPatterns.Count > 0)
                {
                    pattern = enemyEasyBossPatterns[Random.Range(0, enemyEasyBossPatterns.Count)];
                }
            }
            else
            {
                tempEntranceList.Clear();
                for (int i = 0; i < enemyEasyBossPatterns_Entrances.Count; i++)
                {
                    tempEntranceList.Add(i);
                }
                int index = -1;
                while (tempEntranceList.Count > 0)
                {
                    index = Random.Range(0, tempEntranceList.Count);
                    if ((enemyEasyBossPatterns_Entrances[tempEntranceList[index]] & entranceInt) > 0)
                    {
                        index = tempEntranceList[index];
                        break;
                    }
                    else
                    {
                        tempEntranceList.RemoveAt(index);
                    }
                }

                if (index > -1)
                {
                    pattern = enemyEasyBossPatterns[index];
                }
                else
                {
                    pattern = GetEnemyEasyPattern(entranceInt);
                }
            }
            return pattern;
        }

        //FOR MEDIUM ENEMY PATTERNS
        public Transform GetEnemyMediumPattern(int entranceInt = -1)
        {
            Transform pattern = null;
            if (entranceInt == -1)
            {
                if (enemyMediumPatterns.Count > 0)
                {
                    pattern = enemyMediumPatterns[Random.Range(0, enemyMediumPatterns.Count)];
                }
            }
            else
            {
                tempEntranceList.Clear();
                for (int i = 0; i < enemyMediumPatterns_Entrances.Count; i++)
                {
                    tempEntranceList.Add(i);
                }
                int index = -1;
                while (tempEntranceList.Count > 0)
                {
                    index = Random.Range(0, tempEntranceList.Count);
                    if ((enemyMediumPatterns_Entrances[tempEntranceList[index]] & entranceInt) > 0)
                    {
                        index = tempEntranceList[index];
                        break;
                    }
                    else
                    {
                        tempEntranceList.RemoveAt(index);
                    }
                }

                if (index > -1)
                {
                    pattern = enemyMediumPatterns[index];
                }
                else
                {
                    pattern = GetEnemyEasyPattern(entranceInt);
                }
            }
            return pattern;
        }

        public Transform GetEnemyMediumBossPattern(int entranceInt = -1)
        {
            Transform pattern = null;
            if (entranceInt == -1)
            {
                if (enemyMediumBossPatterns.Count > 0)
                {
                    pattern = enemyMediumBossPatterns[Random.Range(0, enemyMediumBossPatterns.Count)];
                }
            }
            else
            {
                tempEntranceList.Clear();
                for (int i = 0; i < enemyMediumBossPatterns_Entrances.Count; i++)
                {
                    tempEntranceList.Add(i);
                }
                int index = -1;
                while (tempEntranceList.Count > 0)
                {
                    index = Random.Range(0, tempEntranceList.Count);
                    if ((enemyMediumBossPatterns_Entrances[tempEntranceList[index]] & entranceInt) > 0)
                    {
                        index = tempEntranceList[index];
                        break;
                    }
                    else
                    {
                        tempEntranceList.RemoveAt(index);
                    }
                }

                if (index > -1)
                {
                    pattern = enemyMediumBossPatterns[index];
                }
                else
                {
                    pattern = GetEnemyEasyBossPattern(entranceInt);
                }
            }
            return pattern;
        }

        //FOR HARD ENEMY PATTERNS
        public Transform GetEnemyHardPattern(int entranceInt = -1)
        {
            Transform pattern = null;
            if (entranceInt == -1)
            {
                if (enemyHardPatterns.Count > 0)
                {
                    pattern = enemyHardPatterns[Random.Range(0, enemyHardPatterns.Count)];
                }
            }
            else
            {
                tempEntranceList.Clear();
                for (int i = 0; i < enemyHardPatterns_Entrances.Count; i++)
                {
                    tempEntranceList.Add(i);
                }
                int index = -1;
                while (tempEntranceList.Count > 0)
                {
                    index = Random.Range(0, tempEntranceList.Count);
                    if ((enemyHardPatterns_Entrances[tempEntranceList[index]] & entranceInt) > 0)
                    {
                        index = tempEntranceList[index];
                        break;
                    }
                    else
                    {
                        tempEntranceList.RemoveAt(index);
                    }
                }

                if (index > -1)
                {
                    pattern = enemyHardPatterns[index];
                }
                else
                {
                    pattern = GetEnemyMediumPattern(entranceInt);
                }
            }
            return pattern;
        }

        public Transform GetEnemyHardBossPattern(int entranceInt = -1)
        {
            Transform pattern = null;
            if (entranceInt == -1)
            {
                if (enemyHardBossPatterns.Count > 0)
                {
                    pattern = enemyHardBossPatterns[Random.Range(0, enemyHardBossPatterns.Count)];
                }
            }
            else
            {
                tempEntranceList.Clear();
                for (int i = 0; i < enemyHardBossPatterns_Entrances.Count; i++)
                {
                    tempEntranceList.Add(i);
                }
                int index = -1;
                while (tempEntranceList.Count > 0)
                {
                    index = Random.Range(0, tempEntranceList.Count);
                    if ((enemyHardBossPatterns_Entrances[tempEntranceList[index]] & entranceInt) > 0)
                    {
                        index = tempEntranceList[index];
                        break;
                    }
                    else
                    {
                        tempEntranceList.RemoveAt(index);
                    }
                }

                if (index > -1)
                {
                    pattern = enemyHardBossPatterns[index];
                }
                else
                {
                    pattern = GetEnemyMediumBossPattern(entranceInt);
                }
            }
            return pattern;
        }
    }
}