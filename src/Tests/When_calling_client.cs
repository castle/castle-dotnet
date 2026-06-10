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
        public async Task Should_authenticate(ActionRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.Authenticate(request);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_not_throw_exception_if_calling_authenticate_with_null_request(CastleClient sut)
        {
            Func<Task> act = async () => await sut.Authenticate(null);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_send_risk(ActionRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.Risk(request);
            await act.Should().NotThrowAsync();
        }


        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_not_throw_exception_if_calling_risk_with_null_request(CastleClient sut)
        {
            Func<Task> act = async () => await sut.Risk(null);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_send_filter(ActionRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.Filter(request);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_not_throw_exception_if_calling_filter_with_null_request(CastleClient sut)
        {
            Func<Task> act = async () => await sut.Filter(null);
            await act.Should().NotThrowAsync();
        }


        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_track(ActionRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.Track(request);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_not_throw_exception_if_calling_track_with_null_request(CastleClient sut)
        {
            Func<Task> act = async () => await sut.Track(null);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_log(ActionRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.Log(request);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_not_throw_exception_if_calling_log_with_null_request(CastleClient sut)
        {
            Func<Task> act = async () => await sut.Log(null);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_get_user_devices(string userId, CastleClient sut)
        {
            Func<Task> act = async () => await sut.GetDevicesForUser(userId);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_throw_exception_if_calling_user_devices_with_empty_userId(CastleClient sut)
        {
            Func<Task> act = async () => await sut.GetDevicesForUser("");
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_throw_exception_if_calling_user_devices_with_null_userId(CastleClient sut)
        {
            Func<Task> act = async () => await sut.GetDevicesForUser(null);
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_get_device(string deviceToken, CastleClient sut)
        {
            Func<Task> act = async () => await sut.GetDevice(deviceToken);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_throw_exception_if_calling_get_device_with_empty_token(CastleClient sut)
        {
            Func<Task> act = async () => await sut.GetDevice("");
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_throw_exception_if_calling_get_device_with_null_token(CastleClient sut)
        {
            Func<Task> act = async () => await sut.GetDevice(null);
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_approve_device(string deviceToken, CastleClient sut)
        {
            Func<Task> act = async () => await sut.ApproveDevice(deviceToken);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_throw_exception_if_calling_approve_device_with_empty_token(CastleClient sut)
        {
            Func<Task> act = async () => await sut.ApproveDevice("");
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_throw_exception_if_calling_approve_device_with_null_token(CastleClient sut)
        {
            Func<Task> act = async () => await sut.ApproveDevice(null);
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_report_device(string deviceToken, CastleClient sut)
        {
            Func<Task> act = async () => await sut.ReportDevice(deviceToken);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_throw_exception_if_calling_report_device_with_empty_token(CastleClient sut)
        {
            Func<Task> act = async () => await sut.ReportDevice("");
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_throw_exception_if_calling_report_device_with_null_token(CastleClient sut)
        {
            Func<Task> act = async () => await sut.ReportDevice(null);
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_start_impersonation(ImpersonateStartRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.ImpersonateStart(request);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_throw_exception_if_calling_impersonate_start_with_null_request(CastleClient sut)
        {
            Func<Task> act = async () => await sut.ImpersonateStart(null);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_end_impersonation(ImpersonateEndRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.ImpersonateEnd(request);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_throw_exception_if_calling_impersonate_end_with_null_request(CastleClient sut)
        {
            Func<Task> act = async () => await sut.ImpersonateEnd(null);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_archive_devices(string userId, CastleClient sut)
        {
            Func<Task> act = async () => await sut.ArchiveDevices(userId);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_throw_exception_if_calling_archive_devices_with_empty_userId(CastleClient sut)
        {
            Func<Task> act = async () => await sut.ArchiveDevices("");
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_throw_exception_if_calling_archive_devices_with_null_userId(CastleClient sut)
        {
            Func<Task> act = async () => await sut.ArchiveDevices(null);
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
}
