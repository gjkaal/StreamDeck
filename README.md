`OpenMacroBoard.SDK` is library that helps you implement custom functions for different macro boards (with focus on devices with display keys, like the Stream Deck or Stream Deck Mini)

# Getting started

Use the BoardFactory to create a board and start experimenting.

```CSharp
using(var device = BoardFactory.SpawnVirtualBoard())
{
    var redKey = KeyBitmap.Create.FromRgb(255,0,0);
    device.SetKeyBitmap(redKey);
}
```

If you want to run your program on a real device you start with using the `StreamDeckSharp` project in your own application.

# Plans for the future
Add some sample projects and start using the Stream Deck for developer tasks.

# Is device _XYZ_ supported?
At the moment there are only three `IMacroBoard` providers
1. Stream Deck (via `StreamDeckSharp`)
    1. Stream Deck (original/classic)
    2. Stream Deck Rev 2
    3. Stream Deck MK2
    4. Stream Deck Mini
    5. Stream Deck XL
4. Virtual Macro Board (via `OpenMacroBoard.SocketIO` and VirtualBoard.exe)


# Credits
Based on the code for OpenMacroBoard.SDK and [StreamDeckSharp](https://github.com/OpenMacroBoard/StreamDeckSharp)

