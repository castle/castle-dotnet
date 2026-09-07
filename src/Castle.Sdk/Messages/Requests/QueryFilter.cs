namespace Castle.Messages.Requests
{
    public class QueryFilter
    {
        public string Field { get; set; }

        public string Op { get; set; }

        public object Value { get; set; }
    }
}
