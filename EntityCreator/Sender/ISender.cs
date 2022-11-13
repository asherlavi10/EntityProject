namespace EntityCreator.Sender
{
    public interface ISender
    {
        public void PublishMessage<T>(T entity, string key) where T : class;
    }
}
