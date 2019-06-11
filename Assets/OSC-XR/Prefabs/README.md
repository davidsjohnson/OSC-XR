## OSC-XR Prefab Controllers

(README In progress..)
Adding OSC controller prefabs to a Unity scene is the quickest way to get started with OSC-XR. To implement a controller simply add the prefab from the `OSCXR/Prefabs` folder to the Unity game hierarchy.

### OSC Slider

- A virtual slider with position mapped to a configurable range value
- Configuration Options
  - **OSC Base Address**
    - Default: `/slider`
  - **Controller ID**
  - **Range:** min and max values for the slider
  - **Step Size:** size of slider increments for controlling data resolution
- OSC Events and Data
  - **Value Event:** Slider value event sent anytime the slider handle is moved
    - **Included Data**
      - Controller ID
      - Slider value
    - **OSC Message**
      - `/slider/value 2 0.2`

### OSC Pad

- A virtual pad with optional velocity data
- Configuration Options
  - **OSC Base Address**
    - Default: `/pad`
  - **Controller ID**
  - **Send Velocity:** check to include velocity data in OSC Message
- OSC Events and Data
  - **Pressed Event:**
    - **Included Data**
      - Controller ID
      - Velocity
    - **OSC Message**
      - `/pad/pressed 2 1.5`
  - **Released Event:**
    - **Included Data**
      - Controller ID
      - Velocity
    - **OSC Message**
      - `/pad/released 2 1.5`
