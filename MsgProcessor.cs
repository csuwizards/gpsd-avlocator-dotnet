//using System.Threading.Channels;
//using System.Threading.Task;
//
//namespace serial_read {
//	public class MsgProcessor
//	{
//		private readonly Channel<string> messageQueue;
//		public MsgProcessor(Channel<string> messageQueue)
//		{
//			this.messageQueue = messageQueue;
//		}
//		public async Task StartProcessing(CancellationToken cancelToken)
//		{
//			await foreach(var message in messageQueue.Reader.ReadAllAsync(cancelToken))
//			{
//				var reversedString = new string(message.Reverse().ToArray());
//	
//				Console.WriteLine($"Thread={Thread.CurrentThread.ManagedThreadId} reverse({message})=>{reversedString}");
//			}
//		}
//		public async Task QueueForProcessing(string Message, CancellationToken cancelToken)
//		{
//			await messageQueue.Writer.WriteAsync(Message, cancelToken);
//		}
//	}
//}