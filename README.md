# OSC-XR

### A Toolkit for Extended Reality (XR) Immersive Music Interfaces

OSC-XR, a toolkit for rapidly prototyping immersive musical environments in XR using [Open Sound Control (OSC)](http://opensoundcontrol.org/introduction-osc), a communication protocol widely used in audio software. Influenced by multi-touch OSC controllers, OSC-XR provides designers with a wide range of readily available components in order to make designing immersive environment more accessible to researchers and sound designers.

## Video Demo

Video in editting...Check back soon

## OSC-XR Prefab Controllers

Adding OSC controller prefabs to a Unity scene is the quickest way to get started with OSC-XR. To implement a controller simply add the prefab from the `OSCXR/Prefabs` folder to the Unity game hierarchy.

### Currently Available Prefabs

#### OSC Slider

- A virtual slider with position mapped to a configurable range value
- Configuration Options
  - OSC Address
    - Default: `/slider`
  - Range: min and max values for the slider
  - Step Size: size of slider increments for controlling data resolution
- OSC Events and Data
  - Value Event
    - Slider value event sent anytime the slider handle is moved
  - Included Data
    - Controller ID
    - Slider value
  - OSC Message
      - `/slider/value 2 0.2`

#### OSC Pad
- A virtual drum pad
- Configuration Options
  - OSC Address
    - Default: `/slider`
  - Range: min and max values for the slider
  - Step Size: size of slider increments for controlling data resolution
- OSC Control Data
  - Value
      - Data
      - ID
      - Slider value
      - Message
      - `/slider/value 2 4.5`

## OSC-XR Scripting Interface
OSC-XR scripting allows developers to quickly add OSC capabilities to any Unity `GameObject` by attaching any of the readily available controller scripts to the object.

### Currently Available Scripts