﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace decode01
{
    public class ThermoMeterPage : ContentPage
    {
        BoxView thermoBar;
        Image bgImage;
        public ThermoMeterPage()
        {
            AbsoluteLayout abs = new AbsoluteLayout();
            bgImage = new Image { Aspect = Aspect.AspectFit };
            bgImage.Source = ImageSource.FromResource("decode01.Thermo.png");

            abs.Children.Add(bgImage);
            abs.Children.Add(thermoBar =
                new BoxView
                {
                    Color = Color.FromHex("#bd3f3f"),
                    Rotation = -130,
                });
            Content = abs;
            SizeChanged += OnPageSizeChanged;
        }

        async void OnPageSizeChanged (object sender, EventArgs args)
        {
            Point center = new Point(this.Width / 2, this.Height / 2);
            double radius = 0.35 * Math.Min(this.Width, this.Height);


            double width = 0.08 * radius;
            double height = 1.0 * radius;
            double offset = 0.9;

            AbsoluteLayout.SetLayoutFlags(bgImage,
                AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(bgImage,
                new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            AbsoluteLayout.SetLayoutBounds(thermoBar,
                new Rectangle(center.X - 0.5 * width,
                                  center.Y - offset * height,
                                  width, height));
            thermoBar.AnchorX = 0.51;
            thermoBar.AnchorY = offset;

            

            var thermo = 25;
            var angle = (thermo - 10) * 3;
            await thermoBar.RotateTo(-120, 2000, Easing.CubicIn);
            await thermoBar.RotateTo(angle, 3000, Easing.CubicOut);

            System.Diagnostics.Debug.WriteLine("Done");
        }

        private static async Task<List<Temperature>> GetTemperatureAsync(DateTime from, DateTime to)
        {

            var uri = new Uri(
                string.Format(
                "http://decodexamarin.azurewebsites.net/api/temp?from={0}&to={1}", from, to));
            var client = new HttpClient();
            var response = await client.GetAsync(uri);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Temperature>>(json);
        }

    }
}