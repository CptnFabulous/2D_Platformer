using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidAnimationHandler : MonoBehaviour
{
    public PlayerMovement player;
    public AimController aiming;
    public Animator animator;
    public AudioSource soundPlayer;

    [Header("Animator Variables")]
    public string movementSpeed = "Walk Speed";
    public string movementSpeedMagnitude = "Walk Speed Magnitude";
    public string grounded = "Grounded";
    public string jumping = "Jumping";
    public string velocityX = "Velocity X";
    public string velocityY = "Velocity Y";
    public string aimDirectionX = "Aim Direction X";
    public string aimDirectionY = "Aim Direction Y";

    [Header("Sounds")]
    public List<AudioClip> clips;

    private void Awake()
    {
        player.onJump.AddListener(() => animator.SetTrigger(jumping));
    }

    public void Update()
    {
        animator.SetBool(grounded, player.IsGrounded);

        float speed = player.movementValues.x;
        float lastSpeed = animator.GetFloat(movementSpeed);
        if (speed != 0)
        {
            animator.SetFloat(movementSpeed, player.movementValues.x);
        }
        else if (lastSpeed > 0.1f)
        {
            animator.SetFloat(movementSpeed, 0.1f);
        }
        else if (lastSpeed < -0.1f)
        {
            animator.SetFloat(movementSpeed, -0.1f);
        }
        animator.SetFloat(movementSpeedMagnitude, player.movementValues.magnitude);

        Vector2 velocity = player.Velocity;
        animator.SetFloat(velocityX, velocity.x);
        animator.SetFloat(velocityY, velocity.y);

        animator.SetFloat(aimDirectionX, aiming.aimDirection.x);
        animator.SetFloat(aimDirectionY, aiming.aimDirection.y);
    }

    public void PlaySound(string clipName)
    {
        //Debug.Log("Attempting to play sound clip " + name + " on frame " + Time.frameCount);
        AudioClip clip = clips.Find((c) => c.name == clipName);
        if (clip != null)
        {
            soundPlayer.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("HumanoidAnimationHandler " + name + " cannot find sound (" + clipName + ")!");
        }
    }
}
