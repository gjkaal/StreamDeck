using OpenMacroBoard.SDK;
using OpenMacroBoard.SocketIO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreamDeckSharp.Extensions;

public static class BoardHelper
{
    public static void WaitForKeyToExit()
    {
        Console.WriteLine("Please press any key (on PC keyboard) to exit this example.");
        Console.ReadKey();
    }

    private static void RedrawDeviceList(IReadOnlyList<IKnownDevice> devices)
    {
        // Alternative to Console.Clear without flicker.
        Console.SetCursorPosition(0, 0);

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Devices:");

        if (devices.Count == 0)
        {
            Console.WriteLine("   (none)");
        }
        else
        {
            for (int i = 0; i < devices.Count; i++)
            {
                var d = devices[i];
                Console.WriteLine($"{i,3}:  [{(d.Connected ? 'X' : ' ')}] {d.DeviceName}");
            }
        }

        Console.SetCursorPosition(0, 0);

        var text = "Select a device: ";
        Console.Write(text.PadRight(Console.BufferWidth - 1));
        Console.SetCursorPosition(0, 0);
        Console.Write(text);
    }

    public static IMacroBoard OpenBoard(Predicate<IDeviceReference> boardSelector)
    {
        using var ctx = DeviceContext.Create()
            .AddListener<StreamDeckListener>()
            .AddListener<SocketIOBoardListener>()
            ;

        var sync = new object();
        IReadOnlyList<IKnownDevice> deviceList = new List<IKnownDevice>();

        void UpdateAndRedraw()
        {
            lock (sync)
            {
                deviceList = ctx.KnownDevices.Where(d => boardSelector(d)).ToList();
                RedrawDeviceList(deviceList);
            }
        }

        using var subscription = ctx.DeviceStateReports.Subscribe(_ => UpdateAndRedraw());

        while (true)
        {
            UpdateAndRedraw();
            var selection = Console.ReadLine();

            var refcopyDeviceList = deviceList;

            if (int.TryParse(selection, out var id))
            {
                if (id >= 0 && id < refcopyDeviceList.Count)
                {
                    Console.Clear();

                    var device = refcopyDeviceList[id]
                        .Open()
                        .WithDisconnectReplay()
                        .WithButtonPressEffect();

                    device.SetBrightness(100);
                    return device;
                }
            }
        };
    }

    public static IMacroBoard OpenBoard()
    {
        return OpenBoard(x => true);
    }
}