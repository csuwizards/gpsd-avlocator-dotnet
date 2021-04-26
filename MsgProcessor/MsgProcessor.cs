    //public class StringReverser
    //{
    //	private readonly Channel<string> messageQueue;
    //	public StringReverser(Channel<string> messageQueue)
    //	{
    //		this.messageQueue = messageQueue;
    //	}
    //	public async Task StartProcessing(CancellationToken cancelToken)
    //	{
    //		await foreach(var message in messageQueue.Reader.ReadAllAsync(cancelToken))
    //		{
    //			var reversedString = new string(message.Reverse().ToArray());

    //			Console.WriteLine($"Thread={Thread.CurrentThread.ManagedThreadId} reverse({message})=>{reversedString}");
    //		}
    //	}
    //	public async Task QueueForProcessing(string Message, CancellationToken cancelToken)
    //	{
    //		await messageQueue.Writer.WriteAsync(Message, cancelToken);
    //	}
    //}