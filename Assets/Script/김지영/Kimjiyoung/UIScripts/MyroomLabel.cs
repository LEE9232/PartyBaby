using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MyroomLabel : MonoBehaviour
{
    #region privat
    private bool mFaceOnActive = true;
    private bool mSkinOnActive = false;

    private bool mFaceOffActive = false;
    private bool mSkinOffActive = true;
    #endregion

    #region public
    public GameObject mFaceOn;
    public GameObject mFaceOff;
    public GameObject mSkinOn;
    public GameObject mSkinOff;
    #endregion

    public void OnMFaceClick()
    {
        if (!mFaceOnActive)
        {
            mFaceOffActive = !mFaceOffActive;
            mFaceOnActive = !mFaceOnActive;

            mFaceOff.SetActive(mFaceOffActive);
            mFaceOn.SetActive(mFaceOnActive);

            mSkinOnActive=!mSkinOnActive;
            mSkinOn.SetActive(mSkinOnActive);

            mSkinOffActive = !mSkinOffActive;
            mSkinOff.SetActive(mSkinOffActive);
        }
        else
        {
            mFaceOnActive = true;
            mFaceOn.SetActive(mFaceOnActive);

            mSkinOffActive = true;
            mSkinOff.SetActive(mSkinOffActive);
        }
    }

    public void OnMSkinClick()
    {
        if (!mSkinOnActive)
        {
            mSkinOnActive = !mSkinOnActive;
            mSkinOn.SetActive(mSkinOnActive);

            mSkinOffActive = !mSkinOffActive;
            mSkinOff.SetActive(mSkinOffActive);

            mFaceOnActive = !mFaceOnActive;
            mFaceOn.SetActive(mFaceOnActive);

            mFaceOffActive = !mFaceOffActive;
            mFaceOff.SetActive(mFaceOffActive);
        }
        else
        {
            mSkinOnActive = true;
            mSkinOn.SetActive(mSkinOnActive);

            mFaceOffActive = true;
            mFaceOff.SetActive(mFaceOffActive);
        }
    }
}
