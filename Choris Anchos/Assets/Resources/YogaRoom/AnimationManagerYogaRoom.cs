using FMODUnity;
using UnityEngine;

public class AnimationManagerYogaRoom : MonoBehaviour
{
    public Animator animator;
    public bool playSequence = false;

    public string[] animationStates;

    private int animationCount;
    private int[] animationIndices;
    private int currentAnimationIndex = -1;
    private bool isPlaying = false;

    public StudioEventEmitter sound;

    private int defaultStateHash;

    private void Awake()
    {
        animationCount = animationStates.Length;
        animationIndices = new int[animationCount];

        // Initialize the array with sequential indices
        for (int i = 0; i < animationCount; i++)
        {
            animationIndices[i] = i;
        }

        defaultStateHash = Animator.StringToHash("Default State");
    }

    private void OnEnable()
    {
        animator.Play(defaultStateHash); // Start with the default state
    }

    private void Update()
    {
        
        if (playSequence && !isPlaying)
        {
            PlayNextAnimation();
            playSequence = false;
        }
    }

    private void PlayNextAnimation()
    {
        currentAnimationIndex++;
        if (currentAnimationIndex >= animationCount)
        {
            animator.Play(defaultStateHash); // Go back to the default state
            currentAnimationIndex = -1; // Reset the index to start from the beginning
            Debug.Log("All animations played. Going back to the default state.");
            return;
        }

        int nextAnimationIndex = animationIndices[currentAnimationIndex];
        animator.SetInteger("RandomPose", nextAnimationIndex);
        animator.SetTrigger("Play");
        sound.Play();

        isPlaying = true;
        Debug.Log("Playing animation at index: " + nextAnimationIndex);

        StartCoroutine(WaitForAnimation(animationStates[nextAnimationIndex]));
    }

    private System.Collections.IEnumerator WaitForAnimation(string animationState)
    {
        yield return new WaitForSeconds(0.1f); // Adjust the duration to allow enough time for the animation to transition

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationState))
        {
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        isPlaying = false;

        // Remove the last played animation index from the available indices
        int lastPlayedIndex = animationIndices[currentAnimationIndex];
        int[] remainingIndices = new int[animationCount - currentAnimationIndex - 1];
        int j = 0;

        for (int i = 0; i < animationCount; i++)
        {
            if (i != currentAnimationIndex && animationIndices[i] != lastPlayedIndex)
            {
                remainingIndices[j] = animationIndices[i];
                j++;
            }
        }

        // Shuffle the remaining indices
        for (int i = remainingIndices.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = remainingIndices[i];
            remainingIndices[i] = remainingIndices[randomIndex];
            remainingIndices[randomIndex] = temp;
        }

        // Assign the remaining indices to animationIndices starting from currentAnimationIndex + 1
        j = 0;
        for (int i = currentAnimationIndex + 1; i < animationCount; i++)
        {
            animationIndices[i] = remainingIndices[j];
            j++;
        }
    }
    public void Play()
    {
        playSequence = true;
    }
}
