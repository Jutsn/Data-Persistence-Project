using System;
using System.IO;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance;

	public Text highScoreText;
	private TextMeshProUGUI nameInput;
	public string playerName;
    public int highScore;
    public string highScoreName;

	private void Awake()
	{
		if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

		LoadHighScore();
	}
	
	void Start()
    {
		nameInput = GameObject.Find("Input").GetComponent<TextMeshProUGUI>();
		SceneManager.sceneLoaded += OnSceneLoaded;

	}

    public void StartButton()
    {
        playerName = nameInput.text;
        SceneManager.LoadScene(1);
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
			nameInput = GameObject.Find("Input").GetComponent<TextMeshProUGUI>();
		}
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
			highScoreText = GameObject.Find("High Score Text").GetComponent<Text>();
			highScoreText.text = $"Best Score : {highScoreName} : {highScore}";
		}
		Debug.Log("Neue Szene geladen: " + scene.name);
	}

	public void SetHighscore(int m_points)
    {
        if (highScore < m_points)
        {
			highScore = m_points;
            highScoreName = playerName;
			highScoreText.text = $"Best Score : {highScoreName} : {highScore}";
			SaveHighScore();
		}
    }
	[Serializable]
	class SaveData
	{
		public int highScore;
		public string highScoreName;
	}

	public void SaveHighScore()
	{
		SaveData data = new SaveData();
		data.highScore = highScore;
		data.highScoreName = highScoreName;

		string json = JsonUtility.ToJson(data);

		File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
	}

	public void LoadHighScore()
	{
		string path = Application.persistentDataPath + "/savefile.json";
		if (File.Exists(path))
		{
			string json = File.ReadAllText(path);
			SaveData data = JsonUtility.FromJson<SaveData>(json);
			highScore = data.highScore;
			highScoreName = data.highScoreName;
		}
	}
}
