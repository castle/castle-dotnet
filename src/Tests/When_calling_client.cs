using System;
using System.Threading.Tasks;
using Castle;
using Castle.Messages.Requests;
using FluentAssertions;
using Tests.SetUp;
using Xunit;

namespace Tests
{
    public class When_calling_client
    {
        // We can test the Castle client by making use of the Do Not Track feature,
        // which makes the client not send any real requests.

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_authenticate(ActionRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.Authenticate(request);
            act.Should().NotThrow();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_track(ActionRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.Track(request);
            act.Should().NotThrow();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_get_user_devices(string userId, CastleClient sut)
        {
            Func<Task> act = async () => await sut.GetDevicesForUser(userId);
            act.Should().NotThrow();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_get_device(string deviceToken, CastleClient sut)
        {
            Func<Task> act = async () => await sut.GetDevice(deviceToken);
            act.Should().NotThrow();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_approve_device(string deviceToken, CastleClient sut)
        {
            Func<Task> act = async () => await sut.ApproveDevice(deviceToken);
            act.Should().NotThrow();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_start_impersonation(ImpersonateStartRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.ImpersonateStart(request);
            act.Should().NotThrow();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_end_impersonation(ImpersonateEndRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.ImpersonateEnd(request);
            act.Should().NotThrow();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_archive_devices(string userId, CastleClient sut)
        {
            Func<Task> act = async () => await sut.ArchiveDevices(userId);
            act.Should().NotThrow();
        }
    }
}
