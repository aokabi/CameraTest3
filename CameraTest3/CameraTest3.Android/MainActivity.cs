
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.OS;
using Android.Views;
using System.Collections.Generic;

namespace CameraTest3.Droid
{
    [Activity(Label = "CameraTest3", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public CameraStateListener stateCallBack;
        public CameraCaptureSession mCaptureSession;
        public CameraCaptureSessionCallback mCaptureSessionCallBack;
        public CameraCaptureSessionCaptureCallback mCameraCaptureSessionCaptureCallback;
        public CaptureRequest.Builder mCaptureRequestBuilder;
        public CaptureRequest mCaptureRequest;

        public CameraDevice mCameraDevice;

        public SurfaceTexture mSurfaceTexture;

        public List<Surface> mSurfaces = new List<Surface>();

        private TextureView mTextureView;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            stateCallBack = new CameraStateListener(this);

            mTextureView = new TextureView(this);
            mSurfaceTexture = mTextureView.SurfaceTexture;

            this.SetContentView(mTextureView);


            mTextureView.SurfaceTextureListener = new SurfaceTextureListener(this);
            mCaptureSessionCallBack = new CameraCaptureSessionCallback(this);
            


        }

        public void OpenCamera()
        {
            var manager = (CameraManager)this.GetSystemService(Context.CameraService);
            foreach (var cameraId in manager.GetCameraIdList())
            {
                CameraCharacteristics characteristics = manager.GetCameraCharacteristics(cameraId);
                if ((int)characteristics.Get(CameraCharacteristics.LensFacing) == (int)LensFacing.Back)
                {
                    manager.OpenCamera(cameraId, stateCallBack, null);
                    return;
                }
            }
        }
    }

    
}

