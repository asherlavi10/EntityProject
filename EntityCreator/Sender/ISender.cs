namespace EntityCreator.Sender
{
    public interface IRabbitSender
    {
        public void PublishMessage<T>(T entity, string key) where T : class;
    }
}
