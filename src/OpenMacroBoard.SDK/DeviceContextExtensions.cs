using OpenMacroBoard.SDK.Internals;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace OpenMacroBoard.SDK
{
    /// <summary>
    /// A collection of extensions for <see cref="IDeviceContext"/>.
    /// </summary>
    public static class DeviceContextExtensions
    {
        /// <summary>
        /// Wait for and open the first matching <see cref="IMacroBoard"/> in this context.
        /// </summary>
        public static async Task<IMacroBoard> OpenAsync(
            this IDeviceContext context,
            Func<IDeviceReference, bool> selector,
            CancellationToken cancellationToken
        )
        {
            return (await context.GetDeviceReferenceAsync(selector, cancellationToken)).Open();
        }

        /// <summary>
        /// Wait for and return the first matching <see cref="IDeviceReference"/>.
        /// </summary>
        public static async Task<IDeviceReference> GetDeviceReferenceAsync(
            this IDeviceContext context,
            Func<IDeviceReference, bool> selector,
            CancellationToken cancellationToken
        )
        {
            // check if there already is a matching device.
            var devRef = context.KnownDevices.FirstOrDefault(selector);

            if (devRef is not null)
            {
                return devRef;
            }

            // if not, register event handler
            // and wait for a matching device to show up

            var eventSync = new object();
            var waitForDeviceBlocker = new TaskCompletionSource<int>();

            IDeviceReference? foundDevice = null;

            void ProcessReport(DeviceStateReport report)
            {
                lock (eventSync!)
                {
                    if (foundDevice != null)
                    {
                        // ignore events if we've already found a device.
                        return;
                    }

                    if (report.Connected && selector(report.DeviceReference))
                    {
                        foundDevice = report.DeviceReference;
                        waitForDeviceBlocker!.TrySetResult(0);
                    }
                }
            }

            IDisposable subscription = null!;
            CancellationTokenRegistration registration = default;

            try
            {
                registration = cancellationToken.Register(() => waitForDeviceBlocker.TrySetCanceled());
                var deviceReportObserver = new DeviceStateObserver(ProcessReport);
                subscription = context.DeviceStateReports.Subscribe(deviceReportObserver);

                await waitForDeviceBlocker.Task;
            }
            finally
            {
                registration.Dispose();
                subscription.Dispose();
            }

            if (foundDevice is null)
            {
                // We don't document that exception because it shouldn't happen,
                // and we don't expect the consumer to handle this exception type.
                throw new InvalidOperationException("Couldn't locate device with matching criteria. This is a bug, please file an issue.");
            }

            return foundDevice;
        }
    }
}
