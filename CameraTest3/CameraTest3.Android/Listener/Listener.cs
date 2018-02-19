using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Runtime;
using Android.Views;
using System;

namespace CameraTest3.Droid
{
    public class CameraStateListener : CameraDevice.StateCallback
    {
        private readonly MainActivity owner;

        public CameraStateListener(MainActivity owner)
        {
            this.owner = owner ?? throw new ArgumentNullException("owner");
        }

        public override void OnDisconnected(CameraDevice camera)
        {
            camera.Close();
            owner.mCameraDevice = null;
        }

        public override void OnError(CameraDevice camera, [GeneratedEnum] CameraError error)
        {
            throw new NotImplementedException();
        }

        public override void OnOpened(CameraDevice camera)
        {
            owner.mCameraDevice = camera;

            owner.mCaptureRequestBuilder = owner.mCameraDevice.CreateCaptureRequest(CameraTemplate.Preview);
            var surface = new Surface(owner.mSurfaceTexture);
            owner.mSurfaces.Add(surface);
            owner.mCaptureRequestBuilder.AddTarget(surface);

            owner.mCameraDevice.CreateCaptureSession(owner.mSurfaces, owner.mCaptureSessionCallBack, null);
        }
    }

    public class CameraCaptureSessionCallback : CameraCaptureSession.StateCallback
    {
        private readonly MainActivity owner;

        public CameraCaptureSessionCallback(MainActivity owner)
        {
            this.owner = owner ?? throw new ArgumentNullException("owner");
        }
        public override void OnConfigured(CameraCaptureSession session)
        {// sessionが生成されたとき
            owner.mCaptureSession = session;
            owner.mCaptureRequest = owner.mCaptureRequestBuilder.Build();
            session.SetRepeatingRequest(owner.mCaptureRequest, owner.mCameraCaptureSessionCaptureCallback, null);
        }

        public override void OnConfigureFailed(CameraCaptureSession session)
        {
            throw new NotImplementedException();
        }
    }

    public class CameraCaptureSessionCaptureCallback : CameraCaptureSession.CaptureCallback
    {

    }

    public class SurfaceTextureListener : Java.Lang.Object, TextureView.ISurfaceTextureListener
    {
        private readonly MainActivity owner;

        public SurfaceTextureListener(MainActivity owner)
        {
            this.owner = owner ?? throw new ArgumentNullException("owner");
        }

        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {// SurfaceViewの準備ができたら
            owner.mSurfaceTexture = surface;

            owner.OpenCamera();


        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            throw new NotImplementedException();
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
            throw new NotImplementedException();
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            //throw new NotImplementedException();
        }
    }
}