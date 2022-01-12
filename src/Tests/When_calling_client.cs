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

        [Fact]
        public void Should_throw_exception_if_client_config_is_null()
        {
            Action act = () => new CastleClient(null);

            act.Should().Throw<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_authenticate(ActionRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.Authenticate(request);
            act.Should().NotThrow();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_not_throw_exception_if_calling_authenticate_with_null_request(CastleClient sut)
        {
            Func<Task> act = async () => await sut.Authenticate(null);
            act.Should().NotThrow();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_send_risk(ActionRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.Risk(request);
            act.Should().NotThrow();
        }


        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_not_throw_exception_if_calling_risk_with_null_request(CastleClient sut)
        {
            Func<Task> act = async () => await sut.Risk(null);
            act.Should().NotThrow();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_send_filter(ActionRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.Filter(request);
            act.Should().NotThrow();
        }


        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_not_throw_exception_if_calling_filter_with_null_request(CastleClient sut)
        {
            Func<Task> act = async () => await sut.Filter(null);
            act.Should().NotThrow();
        }


        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_track(ActionRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.Track(request);
            act.Should().NotThrow();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_not_throw_exception_if_calling_track_with_null_request(CastleClient sut)
        {
            Func<Task> act = async () => await sut.Track(null);
            act.Should().NotThrow();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_get_user_devices(string userId, CastleClient sut)
        {
            Func<Task> act = async () => await sut.GetDevicesForUser(userId);
            act.Should().NotThrow();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_throw_exception_if_calling_user_devices_with_empty_userId(CastleClient sut)
        {
            Func<Task> act = async () => await sut.GetDevicesForUser("");
            act.Should().Throw<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_throw_exception_if_calling_user_devices_with_null_userId(CastleClient sut)
        {
            Func<Task> act = async () => await sut.GetDevicesForUser(null);
            act.Should().Throw<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_get_device(string deviceToken, CastleClient sut)
        {
            Func<Task> act = async () => await sut.GetDevice(deviceToken);
            act.Should().NotThrow();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_throw_exception_if_calling_get_device_with_empty_token(CastleClient sut)
        {
            Func<Task> act = async () => await sut.GetDevice("");
            act.Should().Throw<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_throw_exception_if_calling_get_device_with_null_token(CastleClient sut)
        {
            Func<Task> act = async () => await sut.GetDevice(null);
            act.Should().Throw<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_approve_device(string deviceToken, CastleClient sut)
        {
            Func<Task> act = async () => await sut.ApproveDevice(deviceToken);
            act.Should().NotThrow();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_throw_exception_if_calling_approve_device_with_empty_token(CastleClient sut)
        {
            Func<Task> act = async () => await sut.ApproveDevice("");
            act.Should().Throw<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_throw_exception_if_calling_approve_device_with_null_token(CastleClient sut)
        {
            Func<Task> act = async () => await sut.ApproveDevice(null);
            act.Should().Throw<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_report_device(string deviceToken, CastleClient sut)
        {
            Func<Task> act = async () => await sut.ReportDevice(deviceToken);
            act.Should().NotThrow();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_throw_exception_if_calling_report_device_with_empty_token(CastleClient sut)
        {
            Func<Task> act = async () => await sut.ReportDevice("");
            act.Should().Throw<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_throw_exception_if_calling_report_device_with_null_token(CastleClient sut)
        {
            Func<Task> act = async () => await sut.ReportDevice(null);
            act.Should().Throw<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_start_impersonation(ImpersonateStartRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.ImpersonateStart(request);
            act.Should().NotThrow();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_throw_exception_if_calling_impersonate_start_with_null_request(CastleClient sut)
        {
            Func<Task> act = async () => await sut.ImpersonateStart(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_end_impersonation(ImpersonateEndRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.ImpersonateEnd(request);
            act.Should().NotThrow();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_throw_exception_if_calling_impersonate_end_with_null_request(CastleClient sut)
        {
            Func<Task> act = async () => await sut.ImpersonateEnd(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_archive_devices(string userId, CastleClient sut)
        {
            Func<Task> act = async () => await sut.ArchiveDevices(userId);
            act.Should().NotThrow();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_throw_exception_if_calling_archive_devices_with_empty_userId(CastleClient sut)
        {
            Func<Task> act = async () => await sut.ArchiveDevices("");
            act.Should().Throw<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public void Should_throw_exception_if_calling_archive_devices_with_null_userId(CastleClient sut)
        {
            Func<Task> act = async () => await sut.ArchiveDevices(null);
            act.Should().Throw<ArgumentException>();
        }
    }
}
