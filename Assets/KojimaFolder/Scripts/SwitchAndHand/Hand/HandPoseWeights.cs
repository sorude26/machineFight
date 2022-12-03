using OVRTouchSample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HandPose", menuName = "KJM/CreateHandPose", order = 1)]
public class HandPoseWeights : ScriptableObject
{
    public HandPoseId PoseId;
    [Range(0.0f, 1.0f)]
    public float FlexPose;
    [Range(0.0f, 1.0f)]
    public float FlexWeight;
    [Range(0.0f, 1.0f)]
    public float PointPose;
    [Range(0.0f, 1.0f)]
    public float PointWeight;
    [Range(0.0f, 1.0f)]
    public float ThumbsUpPose;
    [Range(0.0f, 1.0f)]
    public float ThumbsUpWeight;
    [Range(0.0f, 1.0f)]
    public float PinchPose;
    [Range(0.0f, 1.0f)]
    public float PinchWeight;

    public float GetFlex(float Weight)
    {
        return FlexPose * FlexWeight + Weight * (1.0f - FlexWeight);
    }
    public float GetPoint(float Weight)
    {
        return PointPose * PointWeight + Weight * (1.0f - PointWeight);
    }
    public float GetThumbsUp(float Weight)
    {
        return ThumbsUpPose * ThumbsUpWeight + Weight * (1.0f - ThumbsUpWeight);
    }
    public float GetPinch(float Weight)
    {
        return PinchPose * PinchWeight + Weight * (1.0f - PinchWeight);
    }
}
