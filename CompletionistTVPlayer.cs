using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CompletionistTVPlayer : MonoBehaviour
{
    public VideoPlayer player;
    public VideoClip[] clips;

    public VideoClip selectedClip;

    int index = 0;

    List<VideoClip> queue = new List<VideoClip>();

    void Start()
    {
        BuildQueue();  // build from all clips
        PlayCurrent();

        player.loopPointReached += OnClipEnd;
    }

    void BuildQueue()
    {
        queue.Clear();

        foreach (var clip in clips)
            queue.Add(clip);

        // shuffle full list
        for (int i = 0; i < queue.Count; i++)
        {
            int r = Random.Range(i, queue.Count);
            var temp = queue[i];
            queue[i] = queue[r];
            queue[r] = temp;
        }

        index = 0;
    }

    void PlayCurrent()
    {
        if (queue.Count == 0) return;

        player.clip = queue[index];
        player.Play();
    }

    void OnClipEnd(VideoPlayer vp)
    {
        index++;

        if (index >= queue.Count)
        {
            BuildQueue(); // reshuffle after full cycle
        }

        PlayCurrent();
    }
}
