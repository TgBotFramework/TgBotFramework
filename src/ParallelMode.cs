namespace TgBotFramework
{
    public enum ParallelMode
    {
        /// <summary>
        /// All updates processed in 1 thread
        /// </summary>
        SingleThreaded,
        
        /// <summary>
        /// Each update processed in new thread from thread pool
        /// </summary>
        MultiThreaded,
        
        /// <summary>
        /// Look in documentation
        /// </summary>
        /// <see cref="https://github.com/Fedorus/TgBotFramework/wiki/Multithreading"/>
        Smart
    }
}