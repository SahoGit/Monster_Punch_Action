using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
	public string name;
	public AudioClip clip;
    public AudioMixerGroup mixer;
	[Range(0f,1f)]
	public float volume = 0.7f;
	[Range(0f,1f)]
	public float pitch  = 1f;

	public bool loop = false;

	[HideInInspector]
	public AudioSource source;

    public void SetSource (AudioSource _source)
	{
		source = _source;
		source.clip = clip;
        source.outputAudioMixerGroup = mixer;
		source.loop = loop;
	}
	public void Play()
	{
		source.volume = volume;
		source.pitch = pitch;
		source.Play ();
	}
	public void Stop()
	{
		source.Stop();
	}
}

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	[SerializeField]
	Sound[] Sfx;
	[SerializeField]
	Sound[] BG;
	void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

	void Start()
	{
		for (int i = 0; i < BG.Length; i++)
		{
			GameObject _go = new GameObject("BG_" + BG[i].name);
			_go.transform.SetParent(this.transform);
			BG[i].SetSource(_go.AddComponent<AudioSource>());
		}
		for (int i = 0; i < Sfx.Length; i++)
		{
			GameObject _go = new GameObject("Sfx_" + Sfx[i].name);
			_go.transform.SetParent(this.transform);
			Sfx[i].SetSource(_go.AddComponent<AudioSource>());
		}
	}

	public void PlayBG(string _name)
	{
		for (int i = 0; i < BG.Length; i++)
		{
			if (BG[i].name == _name)
			{
				BG[i].Play();
				return;
			}
		}
	}
	public void SetBGVol(string _name, float vol)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).name == "BG_" + _name)
			{
				transform.GetChild(i).GetComponent<AudioSource>().volume = vol;
				return;
			}
		}
	}
	public void PlaySFX(string _name)
	{
		for (int i = 0; i < Sfx.Length; i++)
		{
			if (Sfx[i].name == _name)
			{
				Sfx[i].Play();
				return;
			}
		}
	}

	public void StopBG(string _name)
	{
		for (int i = 0; i < BG.Length; i++)
		{
			if (BG[i].name == _name)
			{
				BG[i].Stop();
				return;
			}
		}
	}
	public void StopSFX(string _name)
	{
		for (int i = 0; i < Sfx.Length; i++)
		{
			if (Sfx[i].name == _name)
			{
				Sfx[i].Stop();
				return;
			}
		}
	}
}
