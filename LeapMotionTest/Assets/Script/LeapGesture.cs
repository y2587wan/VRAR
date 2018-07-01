using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using System;

public class LeapGesture : MonoBehaviour {

    private static bool GestureLeft = false;
    private static bool GestureRight = false;
    private static bool GestureUp = false;
    private static bool GestureDown = false;
    private static bool GestureForward = false;
    private static bool GestureBackward = false;

    private static bool GestureZoom = false;
    private static float movePOs = 0.0f;

    private LeapProvider mProvider;
    private Frame mFrame;
    private Hand mHand;

    private Vector leftPosition;
    private Vector rightPosition;

    private static float zoom = 1.0f;
    private float distanceFactor = 10;

    [Tooltip("Velocity (m/s) of Palm")]
    [SerializeField]
    private float smallestVelocity = 1.45f;

    [Tooltip("Velocity (m/s) of Single Direction")]
    [SerializeField]
    [Range(0, 1)]
    private float deltaVelocity = 1.0f;

    private void Start()
    {
        mProvider = FindObjectOfType<LeapProvider>() as LeapProvider;
    }

    private void Update()
    {
        mFrame = mProvider.CurrentFrame;

        Debug.Log("Hand Number: " + mFrame.Hands.Count);

        if(mFrame.Hands.Count > 0)
        {
            if (mFrame.Hands.Count == 2)
                zoom = CalculateDistance(mFrame);

            if (mFrame.Hands.Count == 1)
                LRUDGestures(mFrame, ref movePOs);
        }
    }

    private float CalculateDistance(Frame mFrame)
    {
        GestureZoom = true;
        GestureLeft = false;
        GestureRight = false;
        GestureDown = false;
        GestureUp = false;
        GestureBackward = false;
        GestureForward = false;

        float distance = 0f;

        foreach (var itemHand in mFrame.Hands)
        {
            if (itemHand.IsLeft)
                leftPosition = itemHand.PalmPosition;
            if (itemHand.IsRight)
                rightPosition = itemHand.PalmPosition;
        }

        if (leftPosition != Vector.Zero && rightPosition != Vector.Zero)
        {
            Vector3 leftPos = new Vector3(leftPosition.x, leftPosition.y, leftPosition.z);
            Vector3 rightPos = new Vector3(rightPosition.x, rightPosition.y, rightPosition.z);

            distance = distanceFactor * Vector3.Distance(leftPos, rightPos);
            Debug.Log("Distance: " + distance);
        }

        if (distance != 0)
            return distance;
        else
            return distance = 1;
    }

    private void LRUDGestures(Frame mFrame, ref float movePOs)
    {
        GestureZoom = false;

        foreach (var itemHand in mFrame.Hands)
        {
            int numFingers = itemHand.Fingers.Count;

            if (IsFist(itemHand))
            {
                Debug.Log("Fist");
            }
            else if(!IsStationary(itemHand))
            {
                movePOs = itemHand.PalmPosition.x;
                if (IsMoveLeft(itemHand))
                {
                    GestureLeft = true;
                    GestureRight = false;
                    Debug.Log("Move left");
                }
                else if(IsMoveRight(itemHand))
                {
                    GestureRight = true;
                    GestureLeft = false;
                    Debug.Log("Move right");
                }

                if(IsMoveUp(itemHand))
                {
                    GestureDown = false;
                    GestureUp = true;
                    Debug.Log("Move up");
                }

                else if(IsMoveDown(itemHand))
                {
                    GestureDown = true;
                    GestureUp = false;
                    Debug.Log("Move down");
                }

                if(IsMoveForward(itemHand))
                {
                    GestureForward = true;
                    GestureBackward = false;
                    Debug.Log("Move forward");
                }
                else if (IsMoveBackWard(itemHand))
                {
                    GestureForward = false;
                    GestureBackward = true;
                    Debug.Log("Move backward");
                }
            }
        }
    }

    private bool IsStationary(Hand itemHand)
    {
        return itemHand.PalmVelocity.Magnitude < smallestVelocity;
    }

    private bool IsMoveBackWard(Hand itemHand)
    {
        return itemHand.PalmVelocity.z < -deltaVelocity;
    }

    private bool IsMoveForward(Hand itemHand)
    {
        return itemHand.PalmVelocity.z > deltaVelocity;
    }

    private bool IsMoveUp(Hand itemHand)
    {
        return itemHand.PalmVelocity.y > deltaVelocity;
    }

    private bool IsMoveDown(Hand itemHand)
    {
        return itemHand.PalmVelocity.y < -deltaVelocity;
    }

    private bool IsMoveLeft(Hand itemHand)
    {
        return itemHand.PalmVelocity.x < -deltaVelocity;
    }

    private bool IsMoveRight(Hand itemHand)
    {
        return itemHand.PalmVelocity.x > deltaVelocity;
    }

    private bool IsFist(Hand itemHand)
    {
        return itemHand.GrabStrength == 1;
    }

    private bool IsGrabHand(Hand itemHand)
    {
        return itemHand.GrabStrength > 0.8f;
    }

    private bool IsHandOpen(Hand itemHand)
    {
        return itemHand.GrabStrength == 0;
    }
}
