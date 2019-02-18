# OSC-XR

### A Toolkit for Extended Reality (XR) Immersive Music Interfaces

OSC-XR is a toolkit for rapidly prototyping immersive musical environments in XR using [Open Sound Control (OSC)](http://opensoundcontrol.org/introduction-osc), a communication protocol widely used in audio software. Influenced by multi-touch OSC controllers, OSC-XR provides designers with a wide range of readily available components in order to make designing immersive environment more accessible to researchers and sound designers.

## Video Demo

Video in editting...Check back soon

## OSC-XR Controller Prefabs

Adding OSC controller prefabs to a Unity scene is the quickest way to get started with OSC-XR. To implement a controller simply add the prefab from the `OSCXR/Prefabs` folder to the Unity game hierarchy.

### Controller Prefab OSC Messages

#### OSC Slider

- `/slider/value 1 0.65` - ID, Range Value

#### OSC Pad

- `/pad/pressed 1 1.5` - ID, Velocity
- `/pad/released 1 1.5` - ID, Velocity

#### OSC Gyro

- `/gyro/values 1 0.90 0.01 0.55` - ID, Angular Velocity x, y, z
  
See the [OSC-XR Prefabs](Assets/OSC-XR/Prefabs) Directory README for full controller details

## OSC-XR Scripting Interface
OSC-XR scripting allows developers to quickly add OSC capabilities to any Unity `GameObject` by attaching any of the readily available controller scripts to the object.

See the [OSC-XR Scripts](Assets/OSC-XR/Scripts) directory README for full controller details
