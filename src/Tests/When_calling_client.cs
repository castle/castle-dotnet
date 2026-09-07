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

        #region lists

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_create_list(CreateListRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.CreateList(request);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_throw_if_creating_list_with_null_request(CastleClient sut)
        {
            Func<Task> act = async () => await sut.CreateList(null);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_get_all_lists(CastleClient sut)
        {
            Func<Task> act = async () => await sut.GetAllLists();
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_get_list(string listId, CastleClient sut)
        {
            Func<Task> act = async () => await sut.GetList(listId);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_throw_if_getting_list_with_empty_id(CastleClient sut)
        {
            Func<Task> act = async () => await sut.GetList("");
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_update_list(string listId, UpdateListRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.UpdateList(listId, request);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_delete_list(string listId, CastleClient sut)
        {
            Func<Task> act = async () => await sut.DeleteList(listId);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_query_lists(SearchQuery query, CastleClient sut)
        {
            Func<Task> act = async () => await sut.QueryLists(query);
            await act.Should().NotThrowAsync();
        }

        #endregion

        #region list items

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_create_list_item(string listId, CreateListItemRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.CreateListItem(listId, request);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_throw_if_creating_list_item_with_empty_list_id(CreateListItemRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.CreateListItem("", request);
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_get_list_item(string listId, string itemId, CastleClient sut)
        {
            Func<Task> act = async () => await sut.GetListItem(listId, itemId);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_update_list_item(string listId, string itemId, UpdateListItemRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.UpdateListItem(listId, itemId, request);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_query_list_items(string listId, SearchQuery query, CastleClient sut)
        {
            Func<Task> act = async () => await sut.QueryListItems(listId, query);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_count_list_items(string listId, CountListItemsRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.CountListItems(listId, request);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_archive_list_item(string listId, string itemId, CastleClient sut)
        {
            Func<Task> act = async () => await sut.ArchiveListItem(listId, itemId);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_unarchive_list_item(string listId, string itemId, CastleClient sut)
        {
            Func<Task> act = async () => await sut.UnarchiveListItem(listId, itemId);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_create_batch_list_items(string listId, BatchListItemsRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.CreateBatchListItems(listId, request);
            await act.Should().NotThrowAsync();
        }

        #endregion

        #region privacy

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_request_user_data(PrivacyRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.RequestUserData(request);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_throw_if_requesting_user_data_with_null_request(CastleClient sut)
        {
            Func<Task> act = async () => await sut.RequestUserData(null);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_delete_user_data(PrivacyRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.DeleteUserData(request);
            await act.Should().NotThrowAsync();
        }

        #endregion

        #region events

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_get_events_schema(CastleClient sut)
        {
            Func<Task> act = async () => await sut.EventsSchema();
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_query_events(EventsQueryRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.QueryEvents(request);
            await act.Should().NotThrowAsync();
        }

        [Theory, AutoFakeData(typeof(CastleConfigurationNoTrackCustomization))]
        public async Task Should_group_events(EventsGroupRequest request, CastleClient sut)
        {
            Func<Task> act = async () => await sut.GroupEvents(request);
            await act.Should().NotThrowAsync();
        }

        #endregion
    }
}
