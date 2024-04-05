# WPF-Confetti-Effect

A customizable confetti effect for WPF applications.

## Features

- Easy integration into any WPF window
- Customizable properties to control the appearance and behavior of the confetti effect
- Adjustable confetti count, fall speed, size, rotation, and movement
- Random colors for a visually appealing effect

## Installation

1. Clone the repository or download the source code.
2. Add the `ConfettiEffect.cs` file to your WPF project.

## Usage

1. Add a `Canvas` control to your window where you want the confetti effect to appear.
2. Create an instance of the `ConfettiEffect` class in your window.
3. Set the desired properties of the `ConfettiEffect` instance to customize the effect.
4. Call the `StartConfettiEffect` method of the `ConfettiEffect` instance, passing the `Canvas` control as a parameter.

### Example

```csharp
public partial class MainWindow : Window
{
    private readonly ConfettiEffect confettiEffect = new ConfettiEffect
    {
        ConfettiCount = 400,
        ConfettiPerFrame = 20,
        MinFallSpeed = 200,
        MaxFallSpeed = 300,
        MinConfettiSize = 5.5,
        MaxConfettiSize = 16.5
    };

    public MainWindow()
    {
        InitializeComponent();
    }

    private void StartConfettiButton_Click(object sender, RoutedEventArgs e)
    {
        confettiEffect.StartConfettiEffect(ConfettiCanvas);
    }
}
```

## Customization

The `ConfettiEffect` class provides several properties to customize the appearance and behavior of the confetti effect:

- `ConfettiCount`: The number of confetti particles to create (default: 400).
- `ConfettiPerFrame`: The number of confetti particles to create in each frame (default: 20).
- `MinFallSpeed` and `MaxFallSpeed`: The minimum and maximum fall speed of the confetti particles (default: 200 and 300, respectively).
- `MinConfettiSize` and `MaxConfettiSize`: The minimum and maximum size of the confetti particles (default: 5.5 and 16.5, respectively).
- `MinRotationAngle` and `MaxRotationAngle`: The minimum and maximum rotation angle of the confetti particles (default: -30 and 30, respectively).
- `MinAmplitude` and `MaxAmplitude`: The minimum and maximum amplitude of the side-to-side movement (default: 5 and 15, respectively).
- `MinFrequency` and `MaxFrequency`: The minimum and maximum frequency of the side-to-side movement (default: 0.3 and 0.6, respectively).

Adjust these properties according to your preferences to create the desired confetti effect.

## Performance Optimization

To prevent the UI from freezing when creating a large number of confetti particles, the `ConfettiEffect` class spreads the creation and animation of confetti particles over multiple frames. The `ConfettiPerFrame` property allows you to control the number of confetti particles created in each frame, balancing performance and the smoothness of the effect.
If you encounter UI freezing issues, you can try reducing the ConfettiPerFrame value or increasing it if you want a more dense confetti effect at the cost of potential performance impact.

## Contributing

Contributions are welcome! If you find any issues or have suggestions for improvements, please open an issue or submit a pull request.
