using System;
using System.Collections.Generic;
using System.Diagnostics;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace BeHappy
{
    public partial class TodoListPage : ContentPage
    {
        //MediaFile photo;

        public TodoListPage()
        {
            InitializeComponent();

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
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            if (photo != null)
                image.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
        }
    }
}
