using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class QuizzSOEditorWindow : EditorWindow
{
    private QuizzSO selectedQuizzSO;
    private SerializedObject serializedQuizzSO;
    private Vector2 leftScrollPosition;
    private Vector2 rightScrollPosition;
    private string[] quizzSONames;
    private List<QuizzSO> quizzSOs = new List<QuizzSO>();
    private List<int> sortedQuizIndices = new List<int>();
    private int selectedQuizzIndex = 0;
    private int selectedQuizIndex = -1;
    private bool isWindowActive = false;
    private float panelSplit = 0.3f;
    private bool isResizing = false;
    private SortMode sortMode = SortMode.NewestFirst;
    private bool needsSortUpdate = true;

    private enum SortMode
    {
        NewestFirst,
        OldestFirst,
        GroupByType
    }

    [MenuItem("Tools/QuizzSO Editor")]
    private static void OpenWindow()
    {
        var window = GetWindow<QuizzSOEditorWindow>("QuizzSO Editor");
        window.isWindowActive = true;
        window.Show();
    }

    private void OnEnable()
    {
        isWindowActive = true;
        LoadQuizzSOs();
        EditorApplication.projectChanged += LoadQuizzSOs;
    }

    private void OnDisable()
    {
        isWindowActive = false;
        EditorApplication.projectChanged -= LoadQuizzSOs;
    }

    private void LoadQuizzSOs()
    {
        quizzSOs.Clear();
        quizzSOs.AddRange(
            AssetDatabase.FindAssets("t:QuizzSO")
                .Select(guid => AssetDatabase.LoadAssetAtPath<QuizzSO>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(so => so != null)
        );
        quizzSONames = quizzSOs.Select(so => so.name).ToArray();
        selectedQuizzIndex = Mathf.Clamp(selectedQuizzIndex, 0, quizzSOs.Count - 1);
        LoadSelectedQuizzSO();
    }

    private void LoadSelectedQuizzSO()
    {
        selectedQuizzSO = quizzSOs.Count > 0 && selectedQuizzIndex < quizzSOs.Count ? quizzSOs[selectedQuizzIndex] : null;
        serializedQuizzSO = selectedQuizzSO != null ? new SerializedObject(selectedQuizzSO) : null;
        selectedQuizIndex = -1;
        needsSortUpdate = true;
    }

    private void UpdateSortedIndices()
    {
        if (!needsSortUpdate || selectedQuizzSO == null) return;

        sortedQuizIndices.Clear();
        sortedQuizIndices.AddRange(Enumerable.Range(0, selectedQuizzSO.Quizzes.Count));

        if (sortMode == SortMode.NewestFirst)
            sortedQuizIndices.Sort((a, b) => b.CompareTo(a));
        else if (sortMode == SortMode.OldestFirst)
            sortedQuizIndices.Sort();

        needsSortUpdate = false;
    }

    private void OnGUI()
    {
        if (!isWindowActive)
        {
            GUILayout.Label("Editor window is not active.", EditorStyles.boldLabel);
            return;
        }

        DrawHeader();

        if (selectedQuizzSO == null)
        {
            EditorGUILayout.HelpBox("No QuizzSO selected or no QuizzSO files found.", MessageType.Warning);
            return;
        }

        UpdateSortedIndices();

        EditorGUILayout.BeginHorizontal();
        DrawLeftPanel();
        DrawSplitter();
        DrawRightPanel();
        EditorGUILayout.EndHorizontal();

        if (GUI.changed && selectedQuizzSO != null && serializedQuizzSO != null)
        {
            serializedQuizzSO.ApplyModifiedProperties();
            EditorUtility.SetDirty(selectedQuizzSO);
        }
    }

    private void DrawHeader()
    {
        GUIStyle titleStyle = new GUIStyle(EditorStyles.label) { fontStyle = FontStyle.Bold, fontSize = 16 };
        GUILayout.Label("QuizzSO Editor", titleStyle);
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Quizz Selection", EditorStyles.label, GUILayout.Width(100));
        int newQuizzIndex = EditorGUILayout.Popup(selectedQuizzIndex, quizzSONames, GUILayout.Width(200));
        if (newQuizzIndex != selectedQuizzIndex)
        {
            selectedQuizzIndex = newQuizzIndex;
            LoadSelectedQuizzSO();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Sort Mode", EditorStyles.label, GUILayout.Width(100));
        SortMode newSortMode = (SortMode)EditorGUILayout.EnumPopup(sortMode, GUILayout.Width(200));
        if (newSortMode != sortMode)
        {
            sortMode = newSortMode;
            needsSortUpdate = true;
            Repaint();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawLeftPanel()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(position.width * panelSplit));
        EditorGUI.indentLevel++;
        GUIStyle boxStyle = new GUIStyle("box") { border = new RectOffset(2, 2, 2, 2) };
        EditorGUILayout.BeginVertical(boxStyle);
        leftScrollPosition = EditorGUILayout.BeginScrollView(leftScrollPosition, GUILayout.ExpandHeight(true));

        if (sortMode == SortMode.GroupByType)
        {
            var groupedQuizzes = sortedQuizIndices
                .Select(i => new { Index = i, Quiz = selectedQuizzSO.Quizzes[i] })
                .GroupBy(q => q.Quiz.Type)
                .OrderBy(g => g.Key.ToString());

            GUIStyle groupTitleStyle = new GUIStyle(EditorStyles.label) { fontStyle = FontStyle.Bold, fontSize = 14 };
            foreach (var group in groupedQuizzes)
            {
                Color bgColor = group.Key == QuizzType.TextToImage ? new Color(0.7f, 1f, 0.7f) : new Color(0.7f, 0.7f, 1f);
                Color textColor = group.Key == QuizzType.TextToImage ? new Color(0.2f, 0.4f, 0.2f) : new Color(0.2f, 0.2f, 0.4f);
                GUIStyle coloredGroupStyle = new GUIStyle(groupTitleStyle) { normal = { background = Texture2D.whiteTexture, textColor = textColor } };
                GUI.backgroundColor = bgColor;
                EditorGUI.indentLevel = 0;
                EditorGUILayout.LabelField(group.Key.ToString().Replace("To", " To "), coloredGroupStyle);
                GUI.backgroundColor = Color.white;

                EditorGUI.indentLevel++;
                foreach (var quiz in group.OrderBy(q => q.Index))
                {
                    DrawQuizButton(quiz.Index);
                }
                EditorGUI.indentLevel--;
            }
        }
        else
        {
            foreach (int i in sortedQuizIndices)
            {
                DrawQuizButton(i);
            }
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add", GUILayout.Width(position.width * panelSplit * 0.45f)))
        {
            AddQuiz();
        }
        if (GUILayout.Button("Remove", GUILayout.Width(position.width * panelSplit * 0.45f)))
        {
            DeleteQuiz();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }

    private void DrawQuizButton(int i)
    {
        bool isSelected = (i == selectedQuizIndex);
        string question = selectedQuizzSO.Quizzes[i].Question;

        GUIStyle buttonStyle = new GUIStyle(EditorStyles.label)
        {
            fontStyle = FontStyle.Bold,
            padding = new RectOffset(5, 5, 2, 2)
        };

        EditorGUILayout.BeginHorizontal();
        if (isSelected)
        {
            GUI.backgroundColor = Color.yellow;
            var icon = EditorGUIUtility.IconContent("d_SceneViewVisibility");
            GUILayout.Label(icon, GUILayout.Width(20), GUILayout.Height(20));
            GUI.backgroundColor = Color.white;
        }
        else
        {
            GUILayout.Space(20);
        }

        if (GUILayout.Button(question, isSelected ? buttonStyle : EditorStyles.label, GUILayout.Width(position.width * panelSplit * 0.85f)))
        {
            selectedQuizIndex = i;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(2);
    }

    private void DrawSplitter()
    {
        Rect splitterRect = GUILayoutUtility.GetRect(5, 5, GUILayout.Width(5), GUILayout.ExpandHeight(true));
        if (Event.current.type == EventType.MouseDown && splitterRect.Contains(Event.current.mousePosition))
        {
            isResizing = true;
        }
        else if (Event.current.type == EventType.MouseDrag && isResizing)
        {
            panelSplit = Mathf.Clamp(Event.current.mousePosition.x / position.width, 0.2f, 0.8f);
            Repaint();
        }
        else if (Event.current.type == EventType.MouseUp)
        {
            isResizing = false;
        }

        if (splitterRect.Contains(Event.current.mousePosition))
        {
            EditorGUIUtility.AddCursorRect(splitterRect, MouseCursor.ResizeHorizontal);
        }
        GUI.DrawTexture(splitterRect, EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill, true, 0, new Color(0.5f, 0.5f, 0.5f), 0, 0);
    }

    private void DrawRightPanel()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(position.width * (1 - panelSplit)));
        EditorGUI.indentLevel++;
        GUIStyle boxStyle = new GUIStyle("box") { border = new RectOffset(2, 2, 2, 2) };
        EditorGUILayout.BeginVertical(boxStyle);
        GUILayout.Label("Quiz Details", new GUIStyle(EditorStyles.label) { fontStyle = FontStyle.Bold, fontSize = 14 });
        rightScrollPosition = EditorGUILayout.BeginScrollView(rightScrollPosition, GUILayout.ExpandHeight(true));

        if (selectedQuizIndex >= 0 && selectedQuizIndex < selectedQuizzSO.Quizzes.Count && serializedQuizzSO != null)
        {
            serializedQuizzSO.Update();
            var quizz = selectedQuizzSO.Quizzes[selectedQuizIndex];

            EditorGUILayout.BeginVertical(boxStyle);
            EditorGUILayout.LabelField(quizz.Question, EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Question", EditorStyles.label, GUILayout.Width(100));
            quizz.Question = EditorGUILayout.TextField(quizz.Question, GUILayout.Width(200));
            if (quizz.Type == QuizzType.ImageToText && quizz.QuestionSprite != null)
            {
                Rect rect = GUILayoutUtility.GetRect(150, 150, GUILayout.ExpandWidth(false));
                EditorGUI.DrawPreviewTexture(rect, quizz.QuestionSprite.texture);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Type", EditorStyles.label, GUILayout.Width(100));
            var newType = (QuizzType)EditorGUILayout.EnumPopup(quizz.Type, GUILayout.Width(200));
            if (newType != quizz.Type)
            {
                quizz.Type = newType;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Correct Audio", EditorStyles.label, GUILayout.Width(100));
            quizz.CorrectAudioClip = (AudioClip)EditorGUILayout.ObjectField(quizz.CorrectAudioClip, typeof(AudioClip), false, GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();

            if (quizz.Type == QuizzType.ImageToText)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Question Sprite", EditorStyles.label, GUILayout.Width(100));
                quizz.QuestionSprite = (Sprite)EditorGUILayout.ObjectField(quizz.QuestionSprite, typeof(Sprite), false, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Answers", EditorStyles.boldLabel);
            for (int j = 0; j < quizz.QuizzAnswers.Count; j++)
            {
                var answer = quizz.QuizzAnswers[j];
                EditorGUILayout.BeginVertical(boxStyle);
                EditorGUILayout.LabelField($"Answer {j + 1} {(j == 0 ? "(Correct)" : "(Wrong)")}", EditorStyles.miniBoldLabel);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                if (quizz.Type != QuizzType.TextToImage)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Text", EditorStyles.label, GUILayout.Width(100));
                    answer.Text = EditorGUILayout.TextField(answer.Text, GUILayout.Width(200));
                    EditorGUILayout.EndHorizontal();
                }

                if (quizz.Type == QuizzType.TextToImage)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Sprite", EditorStyles.label, GUILayout.Width(100));
                    answer.Sprite = (Sprite)EditorGUILayout.ObjectField(answer.Sprite, typeof(Sprite), false, GUILayout.Width(200));
                    if (answer.Sprite != null)
                    {
                        Rect rect = GUILayoutUtility.GetRect(100, 100, GUILayout.ExpandWidth(false));
                        EditorGUI.DrawPreviewTexture(rect, answer.Sprite.texture);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();
        }
        else
        {
            EditorGUILayout.HelpBox("Select a quiz from the left panel.", MessageType.Info);
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }

    private void AddQuiz()
    {
        if (selectedQuizzSO == null) return;

        Undo.RecordObject(selectedQuizzSO, "Add Quiz");
        Quizz newQuiz;
        if (selectedQuizIndex >= 0 && selectedQuizIndex < selectedQuizzSO.Quizzes.Count)
        {
            var selectedQuiz = selectedQuizzSO.Quizzes[selectedQuizIndex];
            newQuiz = new Quizz
            {
                Question = selectedQuiz.Question + " (Copy)",
                Type = selectedQuiz.Type,
                CorrectAudioClip = selectedQuiz.CorrectAudioClip,
                QuestionSprite = selectedQuiz.QuestionSprite,
                QuestionAudioClip = selectedQuiz.QuestionAudioClip,
                QuizzAnswers = new List<QuizzAnswer>()
            };
            foreach (var answer in selectedQuiz.QuizzAnswers)
            {
                newQuiz.QuizzAnswers.Add(new QuizzAnswer
                {
                    Answer = answer.Answer,
                    Sprite = answer.Sprite,
                    Text = answer.Text
                });
            }
        }
        else
        {
            newQuiz = new Quizz
            {
                Question = "New Question",
                Type = QuizzType.TextToImage,
                QuizzAnswers = new List<QuizzAnswer>
                {
                    new QuizzAnswer { Answer = true, Text = "Correct Answer" },
                    new QuizzAnswer { Answer = false, Text = "Wrong Answer" }
                }
            };
        }
        selectedQuizzSO.Quizzes.Add(newQuiz);
        selectedQuizIndex = selectedQuizzSO.Quizzes.Count - 1;
        needsSortUpdate = true;
        EditorUtility.SetDirty(selectedQuizzSO);
        Repaint();
    }

    private void DeleteQuiz()
    {
        if (selectedQuizzSO == null || selectedQuizIndex < 0 || selectedQuizIndex >= selectedQuizzSO.Quizzes.Count) return;

        Undo.RecordObject(selectedQuizzSO, "Delete Quiz");
        selectedQuizzSO.Quizzes.RemoveAt(selectedQuizIndex);
        selectedQuizIndex = Mathf.Min(selectedQuizIndex, selectedQuizzSO.Quizzes.Count - 1);
        needsSortUpdate = true;
        EditorUtility.SetDirty(selectedQuizzSO);
        Repaint();
    }
}