using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ConfettiEffectExample
{
    public class ConfettiEffect
    {
        private readonly Random random = new Random();
        private readonly string[] possibleColors = {
            "#FF1E90FF", // Bright Blue
            "#FF32CD32", // Lime Green
            "#FFFFD700", // Gold
            "#FFFF69B4", // Hot Pink
            "#FF8A2BE2", // Blue Violet
            "#FF87CEFA", // Light Sky Blue
            "#FFFFB90F", // Dark Golden Rod
            "#FFEE82EE", // Violet
            "#FF98FB98", // Pale Green
            "#FFB0C4DE", // Light Steel Blue
            "#FFF4A460", // Sandy Brown
            "#FFD2691E", // Chocolate
            "#FFDC143C"  // Crimson
        };

        public int ConfettiCount { get; set; } = 400;
        public double MinFallSpeed { get; set; } = 200;
        public double MaxFallSpeed { get; set; } = 300;
        public double MinConfettiSize { get; set; } = 5.5;
        public double MaxConfettiSize { get; set; } = 16.5;
        public double MinRotationAngle { get; set; } = -30;
        public double MaxRotationAngle { get; set; } = 30;
        public double MinAmplitude { get; set; } = 5;
        public double MaxAmplitude { get; set; } = 15;
        public double MinFrequency { get; set; } = 0.3;
        public double MaxFrequency { get; set; } = 0.6;

        public void StartConfettiEffect(Canvas confettiCanvas)
        {
            for (int i = 0; i < ConfettiCount; i++)
            {
                var confetti = CreateConfettiParticle(confettiCanvas);
                AnimateConfettiParticle(confetti, confettiCanvas);
                confettiCanvas.Children.Add(confetti);
            }
        }

        private Path CreateConfettiParticle(Canvas confettiCanvas)
        {
            var confetti = new Path();
            confetti.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(possibleColors[random.Next(possibleColors.Length)]));
            confetti.StrokeThickness = random.NextDouble() * (MaxConfettiSize - MinConfettiSize) + MinConfettiSize;

            var geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                var segmentWidth = confettiCanvas.ActualWidth / ConfettiCount;
                var x = random.NextDouble() * segmentWidth;
                var y = -confetti.StrokeThickness * 2;
                var tilt = random.Next(-11, 11);
                var r = confetti.StrokeThickness * 2;

                context.BeginFigure(new Point(x + tilt + r / 3, y), false, false);
                context.LineTo(new Point(x + tilt, y + tilt + r / 5), true, false);
            }
            geometry.Freeze();

            confetti.Data = geometry;

            Canvas.SetLeft(confetti, random.NextDouble() * confettiCanvas.ActualWidth);
            Canvas.SetTop(confetti, -confetti.StrokeThickness);

            confetti.RenderTransform = new RotateTransform(random.NextDouble() * (MaxRotationAngle - MinRotationAngle) + MinRotationAngle);

            return confetti;
        }

        private void AnimateConfettiParticle(Path confetti, Canvas confettiCanvas)
        {
            double fallSpeed = random.NextDouble() * (MaxFallSpeed - MinFallSpeed) + MinFallSpeed;
            double distanceToBottom = confettiCanvas.ActualHeight + confetti.StrokeThickness * 6;
            TimeSpan duration = TimeSpan.FromSeconds(distanceToBottom / fallSpeed);
            TimeSpan delay = TimeSpan.FromSeconds(random.NextDouble() * 2.0);

            var transformGroup = new TransformGroup();
            var moveTransform = new TranslateTransform();
            var scaleTransform = new ScaleTransform();
            transformGroup.Children.Add(moveTransform);
            transformGroup.Children.Add(scaleTransform);
            var frameInterval = TimeSpan.FromSeconds(0.02);
            confetti.RenderTransform = transformGroup;

            var fallAnimation = new DoubleAnimation(0, distanceToBottom, duration)
            {
                BeginTime = delay,
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };
            fallAnimation.Completed += (s, e) => confettiCanvas.Children.Remove(confetti);

            double amplitude = random.NextDouble() * (MaxAmplitude - MinAmplitude) + MinAmplitude;
            double frequency = random.NextDouble() * (MaxFrequency - MinFrequency) + MinFrequency;

            var sideAnimation = new DoubleAnimationUsingKeyFrames() { Duration = duration + delay };
            for (TimeSpan sideCurrentTime = TimeSpan.Zero; sideCurrentTime <= duration; sideCurrentTime += frameInterval)
            {
                double value = amplitude * Math.Sin(2 * Math.PI * sideCurrentTime.TotalSeconds * frequency);
                var frame = new LinearDoubleKeyFrame(value, KeyTime.FromTimeSpan(sideCurrentTime + delay));
                sideAnimation.KeyFrames.Add(frame);
            }

            moveTransform.BeginAnimation(TranslateTransform.YProperty, fallAnimation);
            moveTransform.BeginAnimation(TranslateTransform.XProperty, sideAnimation);

            var scaleXAnimation = new DoubleAnimationUsingKeyFrames();
            var scaleYAnimation = new DoubleAnimationUsingKeyFrames();
            var currentTime = TimeSpan.Zero;
            var randomOffset = random.NextDouble() * Math.PI * 2;

            while (currentTime < duration)
            {
                var scaleX = Math.Sin(currentTime.TotalSeconds * 4 + randomOffset) * 0.1 + 0.9;
                var scaleXKeyFrame = new LinearDoubleKeyFrame(scaleX, currentTime);
                scaleXAnimation.KeyFrames.Add(scaleXKeyFrame);

                var scaleY = Math.Sin(currentTime.TotalSeconds * 4 + randomOffset + Math.PI / 2) * 0.1 + 0.9;
                var scaleYKeyFrame = new LinearDoubleKeyFrame(scaleY, currentTime);
                scaleYAnimation.KeyFrames.Add(scaleYKeyFrame);

                currentTime += frameInterval;
            }

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleXAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleYAnimation);
        }
    }
}