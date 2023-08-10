using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using System.Linq;

namespace OpenMacroBoard.SDK.Extensions;

public static class DeckExtensions
{
    public static int GetDeviceImageSize(this IMacroBoard deck)
    {
        if (deck.Keys is not GridKeyLayout gridKeys)
        {
            throw new NotSupportedException("Device is not supported");
        }

        return gridKeys.KeySize;
    }
}
