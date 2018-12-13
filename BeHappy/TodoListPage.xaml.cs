using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Plugin.Media;
using Plugin.Media.Abstractions;
using XamAI.Exceptions;
using XamAI.Models;
using XamAI.Services;
using Xamarin.Forms;

namespace BeHappy
{
    public partial class TodoListPage : ContentPage
    {
        //MediaFile photo;
        FaceRecognitionService _faceRecongnizationService;

        public TodoListPage()
        {
            InitializeComponent();

            _faceRecongnizationService = new FaceRecognitionService();

            CameraButton.Clicked += CameraButton_Clicked;

            //// https://github.com/xamarin/xamarin-forms-samples/search?q=TapGestureRecognizer&unscoped_q=TapGestureRecognizer
            //var tapGesture = new TapGestureRecognizer
            //{
            //    TappedCallback = async (v, o) =>
            //    {
            //        CameraButton_Clicked(null, null);
            //    },
            //    NumberOfTapsRequired = 2
            //};
        }

        // Reused stuff from here https://xamarinhelp.com/use-camera-take-photo-xamarin-forms/

        private async void CameraButton_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            // Take photo
            if (CrossMedia.Current.IsCameraAvailable || CrossMedia.Current.IsTakePhotoSupported)
            {
                var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

                if (photo != null)
                {
                    image.Source = ImageSource.FromStream(() => { return photo.GetStream(); });

                    // Recognize emotion
                    try
                    {
                        var faceAttributes = new FaceAttributeType[] { FaceAttributeType.Emotion };
                        using (var photoStream = photo.GetStream())
                        {
                            CameraButton.IsEnabled = false;
                            Face[] faces = await _faceRecongnizationService.DetectAsync(photoStream, true, false, faceAttributes);
                            if (faces.Any())
                            {
                                // Emotions detected are happiness, sadness, surprise, anger, fear, contempt, disgust, or neutral.
                                CameraButton.Text = faces.FirstOrDefault().FaceAttributes.Emotion.ToRankedList().FirstOrDefault().Key;
                                CameraButton.IsEnabled = true;
                            }
                            photo.Dispose();
                        }
                    }
                    catch (FaceAPIException fx)
                    {
                        Debug.WriteLine(fx.Message);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
            else
            {
                await DisplayAlert("No Camera", "Camera unavailable.", "OK");
            }
        }
    }
}
